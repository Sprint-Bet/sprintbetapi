using System;
namespace PlanningPoker.Dtos
{
    public class SetupVoterDto
    {
        public SetupVoterDto() { }

        /// <summary>
        ///     Voter's name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     Voter's connetion id
        /// </summary>
        public string Id { get; set; }
    }
}
