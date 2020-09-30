using System.ComponentModel.DataAnnotations;
using SprintBetApi.Enums;

namespace SprintBetApi.Dtos
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
