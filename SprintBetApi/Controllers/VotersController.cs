using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SprintBet.Dtos;
using SprintBet.Hubs;
using SprintBet.Services;

namespace SprintBet.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class VotersController : SprintBetController
    {
        private IHubContext<VoteHub, IVoteHub> _hubContext;
        private IVoterService _voterService;
        private IRoomService _roomService;

        public VotersController(IHubContext<VoteHub, IVoteHub> hubContext, IVoterService voterService, IRoomService roomService)
        {
            _hubContext = hubContext;
            _voterService = voterService;
            _roomService = roomService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> SetupPlayer([FromBody] NewVoterDto newVoterDto)
        {
            if (string.IsNullOrWhiteSpace(newVoterDto.Name) || string.IsNullOrWhiteSpace(newVoterDto.Group))
            {
                return BadRequest();
            }

            var room = _roomService.GetRoomById(newVoterDto.Group);
            if (room == null)
            {
                return NotFound();
            }

            var newVoter = _voterService.AddVoter(newVoterDto, room);

            await _hubContext.Groups.AddToGroupAsync(newVoterDto.ConnectionId, newVoter.Room.Id);
            await _hubContext.Clients.Group(newVoter.Room.Id).VotingUpdated(_voterService.GetVotersByRoomId(newVoter.Room.Id));

            var locationPath = $"voters/{newVoter.Id}";
            var location = GetBaseUri(Request, locationPath);

            return Created(location.ToString(), newVoter);
        }

        [HttpGet]
        public IActionResult GetVoters([FromQuery] string roomId = "") 
        {
            if (string.IsNullOrWhiteSpace(roomId))
            {
                return Ok(_voterService.GetAllVoters());
            }

            return Ok(_voterService.GetVotersByRoomId(roomId));
        }

        [HttpGet("{id}")]
        public IActionResult GetVoterById([FromRoute] string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return BadRequest();
            }

            var voter = _voterService.GetVoterById(id);
            if (voter == null)
            {
                return NotFound(voter);
            }

            return Ok(voter);
        }

        [HttpGet("{id}/reconnect")]
        public async Task<IActionResult> GetVoterById([FromRoute] string id, [FromHeader] string connectionId)
        {
            if (string.IsNullOrWhiteSpace(id) || string.IsNullOrWhiteSpace(connectionId))
            {
                return BadRequest();
            }

            var voter = _voterService.GetVoterById(id);
            if (voter == null)
            {
                return NotFound(voter);
            }

            await _hubContext.Groups.AddToGroupAsync(connectionId, voter.Room.Id);

            return Ok(voter);
        }

        [HttpPut("{id}/cast")]
        public async Task<IActionResult> CastVote([FromRoute] string id, [FromBody] UpdatedVoteDto updatedVoteDto)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return BadRequest();
            }

            var voter = _voterService.GetVoterById(id);
            if (voter == null)
            {
                return NotFound(voter);
            }

            if (updatedVoteDto.Point != "" && !voter.Room.Items.Contains(updatedVoteDto.Point))
            {
                return BadRequest();
            }

            voter.Point = updatedVoteDto.Point;
            _voterService.UpdateVoter(voter);

            await _hubContext.Clients.Group(voter.Room.Id).VotingUpdated(_voterService.GetVotersByRoomId(voter.Room.Id));

            return Ok();
        }

        [HttpPut("{id}/change-role")]
        public async Task<IActionResult> ChangeRole([FromRoute] string id, [FromBody] UpdatedRoleDto updatedRoleDto)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return BadRequest();
            }

            var voter = _voterService.GetVoterById(id);
            if (voter == null)
            {
                return NotFound(voter);
            }

            voter.Role = updatedRoleDto.Role;
            _voterService.UpdateVoter(voter);

            await _hubContext.Clients.Group(voter.Room.Id).VotingUpdated(_voterService.GetVotersByRoomId(voter.Room.Id));

            return Ok(voter);

        }

        [HttpDelete("{id}/leave")]
        public async Task<IActionResult> RemoveVoter([FromRoute] string id, [FromHeader] string connectionId)
        {
            if (string.IsNullOrWhiteSpace(id) || string.IsNullOrWhiteSpace(connectionId))
            {
                return BadRequest();
            }

            var voter = _voterService.GetVoterById(id);
            if (voter == null)
            {
                return NotFound(voter);
            }

            _voterService.RemoveVoterById(id);

            await _hubContext.Groups.RemoveFromGroupAsync(connectionId, voter.Room.Id);
            await _hubContext.Clients.Group(voter.Room.Id).VotingUpdated(_voterService.GetVotersByRoomId(voter.Room.Id));

            return NoContent();
        }
    }
}