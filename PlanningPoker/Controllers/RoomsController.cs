using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using PlanningPoker.Dtos;
using PlanningPoker.Hubs;
using PlanningPoker.Services;

namespace PlanningPoker.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class RoomsController : SprintBetController
    {
        private IHubContext<VoteHub, IVoteHub> _hubContext;
        private IVoterService _voterService;
        private IRoomService _roomService;

        public RoomsController(IHubContext<VoteHub, IVoteHub> hubContext, IVoterService voterService, IRoomService roomService)
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
        public async Task<IActionResult> CreateRoom([FromBody] NewRoomDto newRoomDto, [FromHeader] string connectionId)
        {
            if (String.IsNullOrWhiteSpace(connectionId))
            {
                return BadRequest();
            }

            var itemsType = newRoomDto?.ItemsType;
            if (String.IsNullOrWhiteSpace(itemsType))
            {
                return BadRequest();
            }

            var newRoom = _roomService.AddRoom(itemsType);

            await _hubContext.Groups.AddToGroupAsync(connectionId, newRoom.Id);

            var location = GetBaseUri(Request, $"rooms/{newRoom.Id}");

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
            _voterService.ClearVotesByRoomId(room.Id);

            await _hubContext.Clients.Group(room.Id).VotingUpdated(_voterService.GetVotersByRoomId(room.Id));
            await _hubContext.Clients.Group(room.Id).VotingUnlocked();

            return Ok(room);
        }

        [HttpDelete("{roomId}/finish")]
        public IActionResult FinishGame([FromRoute] string roomId)
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

            _voterService.RemoveVotersByRoomId(room.Id);
            _roomService.RemoveRoom(room);

            return NoContent();
        }
    }
}
