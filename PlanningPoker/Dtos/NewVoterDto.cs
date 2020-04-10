﻿using System;
namespace PlanningPoker.Dtos
{
    public class NewVoterDto
    {
        /// <summary>
        ///     Voter's entered name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     The role the voter has chosen
        /// </summary>
        public PlayerType Role { get; set; }

        /// <summary>
        ///     Room the voter would like to join
        /// </summary>
        public string Group { get; set; }
    }
}
