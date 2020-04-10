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
        private IHubContext<NotifyHub, IHubClient> _hubContext;
        private VoterService _voterService;

        public VoteController(IHubContext<NotifyHub, IHubClient> hubContext, VoterService voterService)
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

            var newVoter = _voterService.AddVoter(newVoterDto.Name);
            await _hubContext.Clients.All.VoterAdded(newVoter);

            return Ok(newVoter.Id);
        }
    }
}