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

        [Route("setup")]
        [HttpPost]
        public Dictionary<string, Vote> setupVoter([FromBody] SetupVoterDto voter)
        {
            var voters = this._voterService.GetAllVoters();
            _voterService.UpdateVote(voter.Id, new Vote(voter.Name, ""));
            //_hubContext.Clients.AllExcept(id).PlayerAdded(name);
            return voters;
        }

        [Route("cast-vote")]
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