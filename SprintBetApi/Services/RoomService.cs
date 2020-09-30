using System;
using System.Collections.Generic;
using System.Linq;
using SprintBetApi.Enums;
using SprintBetApi.Dtos;

namespace SprintBetApi.Services
{
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
        public Room AddRoom(RoomType roomType)
        {
            var newRoom = new Room
            {
                Id = DateTimeOffset.Now.ToUnixTimeSeconds().ToString("X").ToLower(),
                Locked = false
            };

            newRoom.Items = GetItemsByRoomType(roomType);

            _rooms.Add(newRoom);

            return newRoom;
        }

        /// <inheritdoc/>
        public IEnumerable<string> GetItemsByRoomType(RoomType roomType)
        {
            string[] fibonacciItems = { "1", "2", "3", "5", "8", "13", "20", "?" };
            string[] spikeItems = { "1", "2", "3", "4", "5", "6", "7", "8" };
            string[] tshirtItems = { "XXS", "XS", "S", "M", "L", "XL", "XXL", "?" };

            var items = fibonacciItems;
            if (roomType == RoomType.Spike)
            {
                items = spikeItems;
            }
            else if (roomType == RoomType.TShirtSizing)
            {
                items = tshirtItems;
            }

            return items;
        }

        /// <inheritdoc/>
        public void RemoveRoom(Room room)
        {
            _rooms.Remove(room);
        }
    }
}