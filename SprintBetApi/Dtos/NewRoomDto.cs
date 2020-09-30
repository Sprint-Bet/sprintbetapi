using System.ComponentModel.DataAnnotations;

namespace SprintBetApi.Dtos
{
    public class NewRoomDto
    {
        /// <summary>
        ///     Key to determine which items the room should use
        /// </summary>
        [Required]
        public string ItemsType { get; set; }
    }
}
