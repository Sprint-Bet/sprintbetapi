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
            _voterService.RemoveVoter(Context.ConnectionId);
            return base.OnDisconnectedAsync(exception);
        }

        /// <summary>
        ///     Method called when user updates their vote
        /// </summary>
        /// <param name="vote"></param>
        /// <returns></returns>
        public async Task UpdateVote(Vote vote)
        {
            var connectionId = Context.ConnectionId;
            _voterService.UpdateVote(connectionId, vote);

            await Clients.Others.VoteUpdated(connectionId, vote);
        }

        /// <summary>
        ///     Method called when user first joins, to receive all other players
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public async Task<Dictionary<string, Vote>> setupVoter(string name)
        {
            var connectionId = Context.ConnectionId;
            _voterService.UpdateVote(connectionId, new Vote(name, ""));
            await Clients.Others.VoterAdded(new Voter(name, connectionId));;

            return _voterService.GetAllVoters()
                .Where(voter => voter.Key != connectionId)
                .ToDictionary(o => o.Key, o => o.Value);
        }
    }
}
