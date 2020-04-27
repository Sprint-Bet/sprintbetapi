using System;
using System.Collections.Generic;
using System.Linq;
using PlanningPoker.Dtos;

namespace PlanningPoker.Services
{
    public class VoterService
    {
        /// <summary>
        ///     Collection of players
        /// </summary>
        private ICollection<Voter> _voters = new List<Voter>();

        /// <summary>
        ///     Get all voters
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Voter> GetAllVoters()
        {
            return _voters;
        }

        /// <summary>
        ///     Get all voters by room name
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Voter> GetVotersByRoom(string roomId)
        {
            return _voters.Where(voter => voter.Room.Id == roomId);
        }

        /// <summary>
        ///     Get single voter
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Voter GetVoterById(string id)
        {
            return _voters.FirstOrDefault(voter => voter.Id == id);
        }

        /// <summary>
        ///     Add a new voter
        /// </summary>
        /// <returns></returns>
        /// <remarks>Returns newly created voter</remarks>
        public Voter AddVoter(NewVoterDto newVoterDto, Room room)
        {
            var newVoter = new Voter
            {
                Name = newVoterDto.Name,
                Id = Guid.NewGuid().ToString(),
                Role = newVoterDto.Role,
                Room = room
            };

            _voters.Add(newVoter);

            return newVoter;
        }

        /// <summary>
        ///     Remove a voter
        /// </summary>
        /// <param name="id"></param>
        public void RemoveVoter(string id)
        {
            var voterToRemove = GetVoterById(id);
            _voters.Remove(voterToRemove);
        }

        /// <summary>
        ///     Update a single vote
        /// </summary>
        /// <param name="id"></param>
        /// <param name="vote"></param>
        public void UpdateVote(string id, string point)
        {
            var voterToUpdate = GetVoterById(id);
            voterToUpdate.Point = point;
        }

        /// <summary>
        ///     Clear all current votes (for participants)
        /// </summary>
        public void ClearVotesByRoom(string roomId)
        {
            var participants = _voters.Where(voter => (voter.Role == PlayerType.Participant) && (voter.Room.Id == roomId));
            foreach (var participant in participants)
            {
                participant.Point = "";
            }
        }
    }
}
