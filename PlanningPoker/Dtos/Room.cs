namespace PlanningPoker.Dtos
{
    public class Room
    {
        /// <summary>
        ///     Room's ID
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        ///     The room's current voting locked status
        /// </summary>
        public bool VotingLocked { get; set; }
    }
}
