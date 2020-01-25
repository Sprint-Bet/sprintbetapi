using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using PlanningPoker.Dtos;
using PlanningPoker.Hubs;
using PlanningPoker.Interfaces;
using System;

namespace PlanningPoker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VoteController : ControllerBase
    {
        private IHubContext<NotifyHub, IHubClient> _hubContext;

        public VoteController(IHubContext<NotifyHub, IHubClient> hubContext)
        {
            _hubContext = hubContext;
        }

        [HttpPost]
        public string Post([FromBody]Vote vote)
        {
            string returnMessage;

            try
            {
                _hubContext.Clients.All.BroadcastVote(vote);
                returnMessage = "Success";
            }
            catch (Exception error)
            {
                returnMessage = error.ToString();
            }

            return returnMessage;
        }
    }
}