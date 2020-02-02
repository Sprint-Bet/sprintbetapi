using System.Collections.Generic;
using PlanningPoker.Dtos;

namespace PlanningPoker.Services
{
    public class VoterService
    {
        private Dictionary<string, Vote> _voters = new Dictionary<string, Vote>();

        public Dictionary<string, Vote> GetAllVoters()
        {
            return _voters;
        }

        public void AddVoter(string id)
        {
            _voters.Add(id, new Vote());
        }

        public void RemoveVoter(string id)
        {
            _voters.Remove(id);
        }

        public void UpdateVote(string id, Vote vote)
        {
            _voters[id] = vote;
        }
    }
}
