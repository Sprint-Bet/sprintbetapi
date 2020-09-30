using System.ComponentModel.DataAnnotations;
using SprintBetApi.Enums;

namespace SprintBetApi.Dtos
{
    public class AuthenticateVoterDto
    {
        /// <summary>
        ///     Voter's ID
        /// </summary>
        [Required]
        public string Id { get; set; }

        /// <summary>
        ///     Room ID of the voter
        /// </summary>
        [Required]
        public string RoomId { get; set; }
    }
}
