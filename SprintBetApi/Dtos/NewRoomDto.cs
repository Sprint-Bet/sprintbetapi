using SprintBetApi.Enums;
using System.ComponentModel.DataAnnotations;

namespace SprintBetApi.Dtos
{
    public class NewRoomDto
    {
        /// <summary>
        ///     Display name for the room
        /// <summary>
        public string Name { get; set; }

        /// <summary>
        ///     Key to determine which items the room should use
        /// </summary>
        [Required]
        public RoomType ItemsType { get; set; }
    }
}
