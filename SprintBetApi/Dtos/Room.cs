using System.Collections.Generic;

namespace SprintBetApi.Dtos
{
    public class Room
    {
        /// <summary>
        ///     The room's ID
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        ///     The ID of the room's Dealer
        /// </summary>
        public string DealerId { get; set; } 

        /// <summary>
        ///     The room's current voting locked status
        /// </summary>
        public bool Locked { get; set; }

        /// <summary>
        ///     The voting items used by the room
        /// </summary>
        public IEnumerable<string> Items { get; set; }
    }
}
