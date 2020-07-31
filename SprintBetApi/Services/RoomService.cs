﻿using System;
using System.Collections.Generic;
using System.Linq;
using SprintBet.Dtos;
using SprintBet.Enums;

namespace SprintBet.Services
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
            string[] fibonacciItems = { "1", "2", "3", "5", "8", "13", "20", "?" };
            string[] spikeItems = { "1", "2", "3", "4", "5", "6", "7", "8" };
            string[] tshirtItems = {"XXS", "XS", "S", "M", "L", "XL", "XXL", "?"};
            
            var items = fibonacciItems;
            if (roomType == RoomType.Spike)
            {
                items = spikeItems;
            }
            else if (roomType == RoomType.TShirtSizing)
            {
                items = tshirtItems;
            }

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