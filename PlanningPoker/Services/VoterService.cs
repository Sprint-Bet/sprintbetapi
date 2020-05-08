using System;
using System.Collections.Generic;
using System.Linq;
using PlanningPoker.Dtos;
using PlanningPoker.Dtos.Enums;

namespace PlanningPoker.Services
{
    public class VoterService
    {
        /// <summary>
        ///     Collection of players
        /// </summary>
        private List<Voter> _voters = new List<Voter>();

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
        public IEnumerable<Voter> GetVotersByRoomId(string roomId)
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

            if (GetVotersByRoomId(room.Id).Count() == 1)
            {
                room.DealerId = newVoter.Id;
            }

            return newVoter;
        }

        /// <summary>
        ///     Remove a voter
        /// </summary>
        /// <param name="id"></param>
        public void RemoveVoterById(string id)
        {
            var voterToRemove = GetVoterById(id);
            _voters.Remove(voterToRemove);
        }

        /// <summary>
        ///     Update a single voter
        /// </summary>
        /// <param name="updatedVoter"></param>
        public void UpdateVoter(Voter updatedVoter)
        {
            var voterIndex = _voters.FindIndex(voter => voter.Id == updatedVoter.Id);
            _voters[voterIndex] = updatedVoter;
        }

        /// <summary>
        ///     Clear all current votes (for participants)
        /// </summary>
        public void ClearVotesByRoomId(string roomId)
        {
            var voters = _voters.Where(voter => (voter.Role == PlayerType.Participant) && (voter.Room.Id == roomId));
            foreach (var voter in voters)
            {
                voter.Point = "";
            }
        }

        /// <summary>
        ///     Remove all voters for a room
        /// </summary>
        /// <param name="roomId"></param>
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
