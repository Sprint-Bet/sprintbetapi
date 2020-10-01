namespace SprintBetApi.Constants
{
    /// <summary>
    ///     A list of the constants used in the app
    /// </summary>
    public static class Constants
    {
        /// <summary>
        ///     Custom claim name for the room id
        /// </summary>
        public const string RoomId = "roomId";

        /// <summary>
        ///     Auth policy name for whether voter id claim exists
        /// </summary>
        public const string VoterHasIdClaimTypePolicy = "VoterHasIdClaimTypePolicy";

        /// <summary>
        ///     Auth policy name for matching voter ids
        /// </summary>
        public const string VoterIdMatchesRequestPolicy = "VoterIdMatchesRequestPolicy";
    }
}
