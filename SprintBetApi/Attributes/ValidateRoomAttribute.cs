using Microsoft.AspNetCore.Mvc.Filters;
using SprintBet.Services;

namespace SprintBetApi.Attributes
{
    public class ValidateRoom : IActionFilter
    {
        private IRoomService _roomService;

        public ValidateRoom(IRoomService roomService)
        {
            _roomService = roomService;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var roomId = context.RouteData.Values["roomId"].ToString();
            if (string.IsNullOrWhiteSpace(roomId))
            {
                context.ModelState.AddModelError("roomId", "Invalid room id");
            }

            if (!context.ModelState.IsValid)
            {
                return;
            }

            var room = _roomService.GetRoomById(roomId);
            if (room == null)
            {
                context.ModelState.AddModelError("roomNotFound", "No room found for the provided id");
            }
        }

        public void OnActionExecuted(ActionExecutedContext context) { }
    }
}
