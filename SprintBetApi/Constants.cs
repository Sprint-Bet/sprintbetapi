namespace SprintBetApi.Constants
{
    /// <summary>
    ///     A list of the constants used in the app
    /// </summary>
    public static class Constants
    {
        /// <summary>
        ///     Custom claim type for the voter id
        /// </summary>
        public const string VoterIdClaimType = "voterId";

        /// <summary>
        ///     Custom claim type for the room id
        /// </summary>
        public const string RoomIdClaimType = "roomId";

        /// <summary>
        ///     Auth policy name for matching voter ids
        /// </summary>
        public const string VoterIdMatchesRequestPolicy = "VoterIdMatchesRequestPolicy";
    }
}