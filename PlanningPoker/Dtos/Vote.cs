namespace PlanningPoker.Dtos
{
    public class Vote
    {
        /// <summary>
        ///     Constructor 1
        /// </summary>
        public Vote() { }


        /// <summary>
        ///     Constructor 2
        /// </summary>
        /// <param name="name"></param>
        /// <param name="point"></param>
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
