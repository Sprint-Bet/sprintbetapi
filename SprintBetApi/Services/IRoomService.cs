using System.Collections.Generic;
using SprintBetApi.Enums;
using SprintBetApi.Dtos;

namespace SprintBetApi.Services
{
    /// <summary>
    ///     Interface for RoomService that interacts directy with stored rooms
    /// </summary>
    public interface IRoomService
    {
        /// <summary>
        ///     Get all rooms
        /// </summary>
        /// <returns>All rooms</returns>
        public IEnumerable<Room> GetAllRooms();

        /// <summary>
        ///     Get single room
        /// </summary>
        /// <param name="id"></param>
        /// <returns>The room entity</returns>
        public Room GetRoomById(string id);

        /// <summary>
        ///      Add a new room
        /// </summary>
        /// <param name="roomType"></param>
        /// <returns>The newly created room</returns>
        public Room AddRoom(NewRoomDto newRoomDto);

        /// <summary>
        ///     Get a rooms items
        /// </summary>
        /// <param name="roomType"></param>
        /// <returns></returns>
        public IEnumerable<string> GetItemsForRoomType(RoomType roomType);

        /// <summary>
        ///     Remove a room
        /// </summary>
        /// <param name="room"></param>
        public void RemoveRoom(Room room);
    }
}
