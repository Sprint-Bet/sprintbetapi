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
        private VoterService _voterService;

        /// <summary>
        ///     Constructor for the votehub
        /// </summary>
        /// <param name="voterService"></param>
        public VoteHub(VoterService voterService)
        {
            _voterService = voterService;
        }

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
        ///     Method called when user updates their vote
        /// </summary>
        /// <param name="vote"></param>
        /// <returns></returns>

        // ATTEMPTING TO DO THIS VIA POST REQUEST NOW

        //public async Task UpdateVote(Vote vote)
        //{
        //    var connectionId = Context.ConnectionId;
        //    _voterService.UpdateVote(connectionId, vote);

        //    await Clients.Others.VoteUpdated(connectionId, vote);
        //}

        /// <summary>
        ///     Method called when user first joins, to receive all other players
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        /// 
        //public async Task<IEnumerable<Voter>> setupPlayer(string name)
        //{
        //    var newVoter = _voterService.AddVoter(name);

        //    // TODO: Need to update with 'push change' or whatever 
        //    //await Clients.Others.VoterAdded(new Voter(name, sessionId));
        //    await Clients.Others.VoterAdded(newVoter);

        //    return _voterService.GetAllVoters();    
        //}
    }
}
