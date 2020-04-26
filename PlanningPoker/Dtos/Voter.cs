using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlanningPoker.Dtos
{
    /// <summary>
    ///     The different roles a voter can have
    /// </summary>
    public enum PlayerType
    {
        Participant,
        Spectator,
        Dealer
    }

    /// <summary>
    ///     Voter class
    /// </summary>
    public class Voter
    {
        /// <summary>
        ///     Voter's name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     Voter's ID, also used to track client session
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        ///     The role of the player
        /// </summary>
        public PlayerType Role { get; set; }

        /// <summary>
        ///     Voter's estimate/point
        /// </summary>
        public string Point { get; set; }

        /// <summary>
        ///     The room the voter belongs to
        /// </summary>
        public Room Room { get; set; }
    }
}
