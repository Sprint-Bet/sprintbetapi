using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SprintBet.Dtos;
using SprintBet.Hubs;
using SprintBet.Services;
using SprintBetApi.Attributes;
using SprintBetApi.Dtos;

namespace SprintBet.Controllers
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
        [TypeFilter(typeof(ValidateRoom))]
        public IActionResult GetRoomById([FromRoute] string roomId)
        {
            if (!ModelState.IsValid)
            {
                return ModelState.ContainsKey("roomNotFound")
                    ? (IActionResult)NotFound(ModelState.Values.First(v => v.Errors.Count > 0))
                    : (IActionResult)BadRequest(ModelState.Values.First(v => v.Errors.Count > 0));
            }

            var room = _roomService.GetRoomById(roomId);
            return Ok(room);
        }

        [HttpPost]
        public async Task<IActionResult> CreateRoom([FromBody] NewRoomDto newRoomDto, [FromHeader] string connectionId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (string.IsNullOrWhiteSpace(connectionId))
            {
                return BadRequest(new ErrorMessage("Invalid connection id"));
            }

            var newRoom = _roomService.AddRoom(newRoomDto.ItemsType);
            await _hubContext.Groups.AddToGroupAsync(connectionId, newRoom.Id);

            return Created(GetBaseUri(Request, $"rooms/{newRoom.Id}").ToString(), newRoom);
        }

        [HttpPut("{roomId}/locked")]
        [TypeFilter(typeof(ValidateRoom))]
        public async Task<IActionResult> LockVoting([FromRoute] string roomId, [FromBody] UpdatedRoomLockedDto updatedRoomLockedDto)
        {
            if (!ModelState.IsValid)
            {
                return ModelState.ContainsKey("roomNotFound")
                    ? (IActionResult)NotFound(ModelState.Values.First(v => v.Errors.Count > 0))
                    : (IActionResult)BadRequest(ModelState.Values.First(v => v.Errors.Count > 0));
            }

            var room = _roomService.GetRoomById(roomId);
            room.Locked = updatedRoomLockedDto.Lock;

            if (room.Locked)
            {
                await _hubContext.Clients.Group(room.Id).VotingLocked();
                return Ok(room.Locked);
            }

            _voterService.ClearVotesByRoomId(room.Id);
            await _hubContext.Clients.Group(room.Id).VotingUpdated(_voterService.GetVotersByRoomId(room.Id));

            await _hubContext.Clients.Group(room.Id).VotingUnlocked();
            return Ok(room.Locked);
        }

        [HttpPut("{roomId}/items")]
        [TypeFilter(typeof(ValidateRoom))]
        public async Task<IActionResult> ChangeItems([FromRoute] string roomId, [FromBody] UpdatedRoomItemsDto updatedRoomItemsDto)
        {
            if (!ModelState.IsValid)
            {
                return ModelState.ContainsKey("roomNotFound")
                    ? (IActionResult)NotFound(ModelState.Values.First(v => v.Errors.Count > 0))
                    : (IActionResult)BadRequest(ModelState.Values.First(v => v.Errors.Count > 0));
            }

            var room = _roomService.GetRoomById(roomId);
            room.Items = _roomService.GetItemsByRoomType(updatedRoomItemsDto.ItemsType);
            _voterService.ClearVotesByRoomId(room.Id);

            await _hubContext.Clients.Group(room.Id).VotingUpdated(_voterService.GetVotersByRoomId(room.Id));
            
            if (room.Locked)
            {
                room.Locked = false;
                await _hubContext.Clients.Group(room.Id).VotingUnlocked();
            }

            return Ok(room.Items);
        }

        [HttpDelete("{roomId}")]
        [TypeFilter(typeof(ValidateRoom))]
        public IActionResult FinishGame([FromRoute] string roomId)
        {
            if (!ModelState.IsValid)
            {
                return ModelState.ContainsKey("roomNotFound")
                    ? (IActionResult)NotFound(ModelState.Values.First(v => v.Errors.Count > 0))
                    : (IActionResult)BadRequest(ModelState.Values.First(v => v.Errors.Count > 0));
            }

            var room = _roomService.GetRoomById(roomId);

            _voterService.RemoveVotersByRoomId(room.Id);
            _roomService.RemoveRoom(room);

            return NoContent();
        }
    }
}
