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
        ///     Get single voter
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Voter GetVoterById(string id)
        {
            return _voters.FirstOrDefault(voter => voter.Id == id);
        }

        /// <summary>
        ///     Add a voter by name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        /// <remarks>Returns newly created voter</remarks>
        public Voter AddVoter(string name)
        {
            var id = Guid.NewGuid().ToString();
            var newVoter = new Voter(id, name);
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
        public void UpdateVote(string id, Vote vote)
        {
            var voterToUpdate = GetVoterById(id);
            voterToUpdate.Point = vote.Point;
        }
    }
}
