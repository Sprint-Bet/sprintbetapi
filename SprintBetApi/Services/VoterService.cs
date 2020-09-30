using System;
using System.Collections.Generic;
using System.Linq;
using SprintBetApi.Dtos;
using SprintBetApi.Enums;

namespace SprintBetApi.Services
{
    public class VoterService : IVoterService
    {
        /// <summary>
        ///     Collection of players
        /// </summary>
        private List<Voter> _voters = new List<Voter>();

        /// <inheritdoc/>
        public IEnumerable<Voter> GetAllVoters()
        {
            return _voters;
        }

        /// <inheritdoc/>
        public IEnumerable<Voter> GetVotersByRoomId(string roomId)
        {
            return _voters.Where(voter => voter.Room.Id == roomId);
        }

        /// <inheritdoc/>
        public Voter GetVoterById(string id)
        {
            return _voters.FirstOrDefault(voter => voter.Id == id);
        }

        /// <inheritdoc/>
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

            if (GetVotersByRoomId(room.Id).Count() == 1)
            {
                room.DealerId = newVoter.Id;
            }

            return newVoter;
        }

        /// <inheritdoc/>
        public void RemoveVoterById(string id)
        {
            var voterToRemove = GetVoterById(id);
            _voters.Remove(voterToRemove);
        }

        /// <inheritdoc/>
        public void UpdateVoter(Voter updatedVoter)
        {
            var voterIndex = _voters.FindIndex(voter => voter.Id == updatedVoter.Id);
            _voters[voterIndex] = updatedVoter;
        }

        /// <inheritdoc/>
        public void ClearVotesByRoomId(string roomId)
        {
            var voters = _voters.Where(voter => (voter.Role == PlayerType.Participant) && (voter.Room.Id == roomId));
            foreach (var voter in voters)
            {
                voter.Point = "";
            }
        }

        /// <inheritdoc/>
        public void RemoveVotersByRoomId(string roomId)
        {
            var participants = _voters.Where(voter => voter.Room.Id == roomId).ToList<Voter>();
            foreach (var participant in participants)
            {
                RemoveVoterById(participant.Id);
            }
        }
    }
}
