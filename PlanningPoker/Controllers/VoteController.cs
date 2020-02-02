using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using PlanningPoker.Dtos;
using PlanningPoker.Hubs;
using PlanningPoker.Interfaces;
using PlanningPoker.Services;
using System;
using System.Collections.Generic;

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

        [HttpPost("setup")]
        public Dictionary<string, Vote> setupVoter([FromBody]string name, string id)
        {
            var voters = this._voterService.GetAllVoters();
            voters[id].Name = name;
            //_hubContext.Clients.AllExcept(id).PlayerAdded(name);
            return voters;
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