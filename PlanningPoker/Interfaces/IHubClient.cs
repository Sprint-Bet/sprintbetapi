using PlanningPoker.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PlanningPoker.Interfaces
{
    public interface IHubClient
    {
        /// <summary>
        ///     Event called when someone has updated their vote
        /// </summary>
        /// <param name="connectionId"></param>
        /// <param name="vote"></param>
        /// <returns></returns>
        Task VoteUpdated(string connectionId, Vote vote);

        /// <summary>
        ///     Event called when a new voter/player has joined the room
        /// </summary>
        /// <param name="voter"></param>
        /// <returns></returns>
        Task VoterAdded(Voter voter);
    }
}
