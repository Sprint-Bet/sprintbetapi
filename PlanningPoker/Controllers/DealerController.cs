using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using PlanningPoker.Hubs;
using PlanningPoker.Interfaces;
using PlanningPoker.Services;

namespace PlanningPoker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DealerController : ControllerBase
    {
        private IHubContext<VoteHub, IHubClient> _hubContext;
        private VoterService _voterService;

        public DealerController(IHubContext<VoteHub, IHubClient> hubContext, VoterService voterService)
        {
            _hubContext = hubContext;
            _voterService = voterService;
        }

        [HttpPost("lock")]
        public async Task<IActionResult> LockVoting()
        {
            await _hubContext.Clients.All.VotingLocked();
            return NoContent();
        }

        [HttpPost("clear")]
        public async Task<IActionResult> ClearVotes()
        {
            _voterService.ClearVotes();
            await _hubContext.Clients.All.VotingUpdated(_voterService.GetAllVoters());
            await _hubContext.Clients.All.VotingUnlocked();
            return NoContent();
        }
    }
}
