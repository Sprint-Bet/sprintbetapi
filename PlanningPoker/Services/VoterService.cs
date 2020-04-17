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
        ///     Get all voters (only participants)
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Voter> GetAllVoters()
        {
            return _voters.Where(voter => voter.Role == PlayerType.Participant);
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
        /// <remarks>Returns newly created voter id</remarks>
        public string AddVoter(NewVoterDto newVoterDto)
        {
            var id = Guid.NewGuid().ToString();
            var newVoter = new Voter
            {
                Name = newVoterDto.Name,
                Id = id,
                Role = newVoterDto.Role
            };

            _voters.Add(newVoter);

            return id;
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
        public void ClearVotes()
        {
            var participants = _voters.Where(voter => voter.Role == PlayerType.Participant);
            foreach (var participant in participants)
            {
                participant.Point = "";
            }
        }
    }
}
