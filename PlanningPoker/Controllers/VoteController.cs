using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
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
    }
}