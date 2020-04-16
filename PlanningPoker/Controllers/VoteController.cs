using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using PlanningPoker.Dtos;
using PlanningPoker.Hubs;
using PlanningPoker.Interfaces;
using PlanningPoker.Services;

namespace PlanningPoker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VoteController : ControllerBase
    {
        private IHubContext<VoteHub, IHubClient> _hubContext;
        private VoterService _voterService;

        public VoteController(IHubContext<VoteHub, IHubClient> hubContext, VoterService voterService)
        {
            _hubContext = hubContext;
            _voterService = voterService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> SetupPlayer([FromBody] NewVoterDto newVoterDto)
        {
            if (string.IsNullOrWhiteSpace(newVoterDto.Name))
            {
                return BadRequest();
            }

            var newVoterId = _voterService.AddVoter(newVoterDto.Name);
            await _hubContext.Clients.All.VotingUpdated(_voterService.GetAllVoters());

            return Ok(newVoterId);
        }

        [HttpGet("voters")]
        public IActionResult GetVoters()
        {
            return Ok(_voterService.GetAllVoters());
        }

        [HttpGet("voters/{id}")]
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

        [HttpPut("voters/{id}/cast")]
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

            _voterService.UpdateVote(id, updatedVoteDto.Point);
            await _hubContext.Clients.All.VotingUpdated(_voterService.GetAllVoters());

            return Ok();
        }

        [HttpDelete("voters/{id}/leave")]
        public async Task<IActionResult> RemoveVoter([FromRoute] string id)
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

            _voterService.RemoveVoter(id);
            await _hubContext.Clients.All.VotingUpdated(_voterService.GetAllVoters());

            return NoContent();
        }
    }
}