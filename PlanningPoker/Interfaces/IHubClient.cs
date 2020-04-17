using PlanningPoker.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PlanningPoker.Interfaces
{
    public interface IHubClient
    {
        /// <summary>
        ///     Event called when voter list or a vote has been updated
        /// </summary>
        /// <param name="voter"></param>
        /// <returns></returns>
        Task VotingUpdated(IEnumerable<Voter> voters);

        /// <summary>
        ///     Event called when a dealer has locked voting
        /// </summary>
        Task VotingLocked();
    }
}
