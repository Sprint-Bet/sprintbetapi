using System.ComponentModel.DataAnnotations;
using SprintBet.Enums;

namespace SprintBet.Dtos
{
    public class UpdatedRoleDto
    {
        /// <summary>
        ///     The new voter role
        /// </summary>
        [Required]
        public PlayerType Role { get; set; }
    }
}
