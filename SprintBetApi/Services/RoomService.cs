using System;
using System.Collections.Generic;
using System.Linq;
using SprintBetApi.Dtos;

namespace SprintBetApi.Services
{
    /// <inheritdoc/>
    public class RoomService : IRoomService
    {
        /// <summary>
        ///     Collection of rooms
        /// </summary>
        private ICollection<Room> _rooms = new List<Room>();

        /// <inheritdoc/>
        public IEnumerable<Room> GetAllRooms()
        {
            return _rooms;
        }

        /// <inheritdoc/>
        public Room GetRoomById(string id)
        {
            return _rooms.FirstOrDefault(room => room.Id == id);
        }

        /// <inheritdoc/>
        public Room AddRoom(string roomType)
        {
            string[] fibonacciItems = { "1", "2", "3", "5", "8", "13", "20", "?" };
            string[] tshirtItems = {"XXS", "XS", "S", "M", "L", "XL", "XXL", "?"};

            var items = (roomType == "fibonacci") ? fibonacciItems : tshirtItems;

            var newRoom = new Room
            {
                Id = DateTimeOffset.Now.ToUnixTimeSeconds().ToString("X"),
                Locked = false,
                Items = items
            };

            _rooms.Add(newRoom);

            return newRoom;
        }

        /// <inheritdoc/>
        public void RemoveRoom(Room room)
        {
            _rooms.Remove(room);
        }
    }
}
