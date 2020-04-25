using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlanningPoker.Dtos
{
    public enum PlayerType
    {
        Participant,
        Spectator,
        Dealer
    }

    public class Voter
    {
        /// <summary>
        ///     New Voter
        /// </summary>
        public Voter() { }

        public Voter(string id)
        {
            Id = id;
        }

        /// <summary>
        ///     New voter with initial values
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        public Voter(string id, string name)
        {
            Id = id;
            Name = name;
        }

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
    }
}
