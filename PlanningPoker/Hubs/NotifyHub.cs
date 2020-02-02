using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using PlanningPoker.Dtos;
using PlanningPoker.Interfaces;
using PlanningPoker.Services;

namespace PlanningPoker.Hubs
{
    public class NotifyHub: Hub<IHubClient>
    {
        private VoterService _voterService;

        /// <summary>
        ///     Constructor for the notify hub
        /// </summary>
        /// <param name="voterService"></param>
        public NotifyHub(VoterService voterService)
        {
            _voterService = voterService;
        }

        /// <summary>
        ///     Override, add voter id to list of clients
        /// </summary>
        /// <returns></returns>
        public override Task OnConnectedAsync()
        {
            _voterService.AddVoter(Context.ConnectionId);
            return base.OnConnectedAsync();
        }

        /// <summary>
        ///     Override, Remove this client to the list of clients when disconnecting
        /// </summary>
        /// <returns></returns>
        public override Task OnDisconnectedAsync(Exception exception)
        {
            var id = Context.ConnectionId;
            _voterService.RemoveVoter(id);

            return base.OnDisconnectedAsync(exception);
        }
    }
}
