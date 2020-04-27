using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.SignalR;
using PlanningPoker.Dtos;
using PlanningPoker.Hubs;
using PlanningPoker.Interfaces;
using PlanningPoker.Services;

namespace PlanningPoker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomsController : ControllerBase
    {
        private IHubContext<VoteHub, IHubClient> _hubContext;
        private VoterService _voterService;
        private RoomService _roomService;

        public RoomsController(IHubContext<VoteHub, IHubClient> hubContext, VoterService voterService, RoomService roomService)
        {
            _hubContext = hubContext;
            _voterService = voterService;
            _roomService = roomService;
        }

        [HttpGet]
        public IActionResult GetRooms()
        {
            return Ok(_roomService.GetAllRooms());
        }

        [HttpGet("{roomId}")]
        public IActionResult GetRoomById([FromRoute] string roomId)
        {
            if (string.IsNullOrWhiteSpace(roomId))
            {
                return BadRequest();
            }

            var room = _roomService.GetRoomById(roomId);
            if (room == null)
            {
                return NotFound();
            }

            return Ok(room);
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateRoom([FromBody] NewRoomDto newRoomDto)
        {
            if (String.IsNullOrWhiteSpace(newRoomDto.Name))
            {
                return BadRequest();
            }

            var newRoom = _roomService.AddRoom(newRoomDto.Name);
            await _hubContext.Groups.AddToGroupAsync(newRoomDto.ConnectionId, newRoom.Id);

            var location = GetBaseUri(Request, $"api/rooms/{newRoom.Id}");

            return Created(location.ToString(), newRoom);
        }

        [HttpPut("{roomId}/lock")]
        public async Task<IActionResult> LockVoting([FromRoute] string roomId)
        {
            if (String.IsNullOrWhiteSpace(roomId))
            {
                return BadRequest();
            }

            var room = _roomService.GetRoomById(roomId);
            if (room == null)
            {
                return NotFound();
            }

            room.VotingLocked = true;
            await _hubContext.Clients.Group(room.Id).VotingLocked();

            return Ok(room);
        }

        [HttpPut("{roomId}/clear")]
        public async Task<IActionResult> ClearVotes([FromRoute] string roomId)
        {
            if (String.IsNullOrWhiteSpace(roomId))
            {
                return BadRequest();
            }

            var room = _roomService.GetRoomById(roomId);
            if (room == null)
            {
                return NotFound();  
            }

            room.VotingLocked = false;
            _voterService.ClearVotesByRoom(room.Id);

            await _hubContext.Clients.Group(room.Id).VotingUpdated(_voterService.GetVotersByRoom(room.Id));
            await _hubContext.Clients.Group(room.Id).VotingUnlocked();

            return Ok(room);
        }

        [HttpDelete("{roomId}/finish")]
        public async Task<IActionResult> FinishGame()
        {
            await _hubContext.Clients.All.GameFinished();
            return NoContent();
        }

        /// <summary>
        ///     Helper uri builder method
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static Uri GetBaseUri(HttpRequest request, string path)
        {
            var uriBuilder = new UriBuilder
            {
                Scheme = request.Scheme,
                Host = request.Host.Host,
                Port = request.Host.Port.GetValueOrDefault(-1),
                Path = path
            };

            return uriBuilder.Uri;
        }
    }
}
