using SprintBet.Enums;
using System.ComponentModel.DataAnnotations;

namespace SprintBet.Dtos
{
    public class NewRoomDto
    {
        /// <summary>
        ///     Key to determine which items the room should use
        /// </summary>
        [Required]
        public RoomType ItemsType { get; set; }
    }
}
