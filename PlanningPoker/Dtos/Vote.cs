namespace PlanningPoker.Dtos
{
    public class Vote
    {
        public Vote() { }

        public Vote(string name, string point)
        {
            Name = name;
            Point = point;
        }

        /// <summary>
        ///     Voter's name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     Voter's estimate/point
        /// </summary>
        public string Point { get; set; }
    }
}
