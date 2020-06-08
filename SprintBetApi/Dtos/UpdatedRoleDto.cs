using SprintBet.Enums;

namespace SprintBet.Dtos
{
    public class UpdatedRoleDto
    {
        /// <summary>
        ///     The new voter role
        /// </summary>
        public PlayerType Role { get; set; }
    }
}
