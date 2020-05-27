using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using PlanningPoker.Dtos;
using PlanningPoker.Interfaces;
using PlanningPoker.Services;

namespace PlanningPoker.Hubs
{
    public class VoteHub: Hub<IHubClient>
    {
        /// <summary>
        ///     Override, add voter id to list of clients
        /// </summary>
        /// <returns></returns>
        public override Task OnConnectedAsync()
        {
            return base.OnConnectedAsync();
        }

        /// <summary>
        ///     Override, Remove this client to the list of clients when disconnecting
        /// </summary>
        /// <returns></returns>
        public override Task OnDisconnectedAsync(Exception exception)
        {
            return base.OnDisconnectedAsync(exception);
        }

        /// <summary>
        ///     Method called to inform groups clients the game has ended
        /// </summary>
        public async Task FinishGame(string roomId)
        {
            await Clients.OthersInGroup(roomId).GameFinished();
        }
    }
}
