using SprintBetApi.Enums;
using System.ComponentModel.DataAnnotations;

namespace SprintBetApi.Dtos
{
    public class UpdatedRoomItemsDto
    {
        /// <summary>
        ///     The new items for the room
        /// </summary>
        [Required]
        public RoomType ItemsType { get; set; }
    }
}
