using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlanningPoker.Dtos
{
    public class Voter
    {
        /// <summary>
        ///     New Voter
        /// </summary>
        public Voter() { }

        /// <summary>
        ///     New voter with initial values
        /// </summary>
        /// <param name="name"></param>
        /// <param name="id"></param>
        public Voter(string name, string id)
        {
            Name = name;
            Id = id;
        }

        /// <summary>
        ///     Voter's name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     Voter's connection ID
        /// </summary>
        public string Id { get; set; }
    }
}
