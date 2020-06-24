using System.ComponentModel.DataAnnotations;

namespace SprintBetApi.Dtos
{
    public class UpdatedRoomLockedDto
    {
        /// <summary>
        ///     The new locked status for a room
        /// </summary>
        [Required]
        public bool Lock { get; set; }
    }
}
