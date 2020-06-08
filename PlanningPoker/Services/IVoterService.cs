using System.Collections.Generic;
using PlanningPoker.Dtos;

namespace PlanningPoker.Services
{
    /// <summary>
    ///     Interface for VoterService that interacts directy with stored voters
    /// </summary>
    public interface IVoterService
    {
        /// <summary>
        ///     Get all voters
        /// </summary>
        /// <returns>All voters</returns>
        public IEnumerable<Voter> GetAllVoters();

        /// <summary>
        ///     Get all voters by room name
        /// </summary>
        /// <param name="roomId"></param>
        /// <returns>List of voters for the given room</returns>
        public IEnumerable<Voter> GetVotersByRoomId(string roomId);

        /// <summary>
        ///     Get single voter
        /// </summary>
        /// <param name="id"></param>
        /// <returns>The voter entity</returns>
        public Voter GetVoterById(string id);

        /// <summary>
        ///     Add a new voter
        /// </summary>
        /// <param name="newVoterDto"></param>
        /// <param name="room"></param>
        /// <returns>Newly created voter</returns>
        public Voter AddVoter(NewVoterDto newVoterDto, Room room);

        /// <summary>
        ///     Remove a voter
        /// </summary>
        /// <param name="id"></param>
        public void RemoveVoterById(string id);

        /// <summary>
        ///     Update a single voter
        /// </summary>
        /// <param name="updatedVoter"></param>
        public void UpdateVoter(Voter updatedVoter);

        /// <summary>
        ///     Clear all current votes (for participants)
        /// </summary>
        /// <param name="roomId"></param>
        public void ClearVotesByRoomId(string roomId);

        /// <summary>
        ///     Remove all voters for a room
        /// </summary>
        /// <param name="roomId"></param>
        public void RemoveVotersByRoomId(string roomId);
    }
}
