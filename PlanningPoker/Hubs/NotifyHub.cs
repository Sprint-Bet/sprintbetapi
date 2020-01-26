using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using PlanningPoker.Dtos;
using PlanningPoker.Interfaces;

namespace PlanningPoker.Hubs
{
    public class NotifyHub: Hub<IHubClient>
    {
        private Dictionary<string, Vote> _connections = new Dictionary<string, Vote>();

        /// <summary>
        ///     Add this client to the list of clients when connected
        /// </summary>
        /// <returns></returns>
        public override Task OnConnectedAsync()
        {
            var id = Context.ConnectionId;
            var vote = new Vote("John", "3");
            _connections.Add(id, vote);

            return base.OnConnectedAsync();
        }

        /// <summary>
        ///     Remove this client to the list of clients when disconnecting
        /// </summary>
        /// <returns></returns>
        public override Task OnDisconnectedAsync(Exception exception)
        {
            var id = Context.ConnectionId;
            _connections.Remove(id);

            return base.OnDisconnectedAsync(exception);
        }
    }
}
