using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlanningPoker.Dtos
{
    public class Message
    {
        /// <summary>
        ///     Message type
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        ///     Message payload
        /// </summary>
        public string Payload { get; set; }
    }
}
