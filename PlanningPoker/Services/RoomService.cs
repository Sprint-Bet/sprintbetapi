using System;
using System.Collections.Generic;
using System.Linq;
using PlanningPoker.Dtos;

namespace PlanningPoker.Services
{
    public class RoomService
    {
        /// <summary>
        ///     Collection of rooms
        /// </summary>
        private ICollection<Room> _rooms = new List<Room>();

        /// <summary>
        ///     Get all rooms
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Room> GetAllRooms()
        {
            return _rooms;
        }

        /// <summary>
        ///     Get single room
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Room GetRoomById(string id)
        {
            return _rooms.FirstOrDefault(room => room.Id == id);
        }

        /// <summary>
        ///     Add a new room
        /// </summary>
        /// <returns></returns>
        /// <remarks>Returns newly created room</remarks>
        public Room AddRoom(string name)
        {
            var newRoom = new Room
            {
                Name = name,
                Id = DateTimeOffset.Now.ToUnixTimeSeconds().ToString("X"),
                VotingLocked = false
            };

            _rooms.Add(newRoom);

            return newRoom;
        }

        /// <summary>
        ///     Remove a room
        /// </summary>
        /// <param name="room"></param>
        public void RemoveRoom(Room room)
        {
            _rooms.Remove(room);
        }
    }
}
