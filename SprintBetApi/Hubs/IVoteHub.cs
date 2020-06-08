using SprintBet.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SprintBet.Hubs
{
    public interface IVoteHub
    {
        /// <summary>
        ///     Event called when voter list or a vote has been updated
        /// </summary>
        /// <param name="voters"></param>
        /// <returns></returns>
        Task VotingUpdated(IEnumerable<Voter> voters);

        /// <summary>
        ///     Event called when a dealer has locked voting
        /// </summary>
        Task VotingLocked();

        /// <summary>
        ///     Event called when a dealer has unlocked voting
        /// </summary>
        Task VotingUnlocked();

        /// <summary>
        ///     Event called when a dealer has finished a game
        /// </summary>
        Task GameFinished();
    }
}
