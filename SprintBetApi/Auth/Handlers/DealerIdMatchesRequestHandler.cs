using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Net.Http.Headers;
using SprintBetApi.Auth.Requirements;
using SprintBetApi.Services;
using System.Linq;
using System.Threading.Tasks;

namespace SprintBetApi.Auth.Handlers
{
    public class DealerIdMatchesRequestHandler : AuthorizationHandler<DealerIdMatchesRequestRequirement>
    {
        private readonly IActionContextAccessor _actionContextAccessor;
        private IAuthService _authService;
        private IRoomService _roomService;

        public DealerIdMatchesRequestHandler(
            IActionContextAccessor actionContextAccessor,
            IAuthService authService,
            IRoomService roomService
        ) {
            _actionContextAccessor = actionContextAccessor;
            _authService = authService;
            _roomService = roomService;
        }

        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            DealerIdMatchesRequestRequirement requirement
        ) {
            var roomIdFromRoute = _actionContextAccessor.ActionContext.RouteData.Values["roomId"].ToString();
            var authHeader = _actionContextAccessor.ActionContext.HttpContext.Request.Headers[HeaderNames.Authorization].ToString();
            if (string.IsNullOrWhiteSpace(roomIdFromRoute) || string.IsNullOrWhiteSpace(authHeader))
            {
                return Task.CompletedTask;
            }

            var room = _roomService.GetRoomById(roomIdFromRoute);
            if (room == null)
            {
                return Task.CompletedTask;
            }

            var token = _authService.ReadToken(authHeader);
            var dealerIdClaim = token.Claims.First(claim => claim.Type == Constants.Constants.VoterIdClaimType);
            if (room.DealerId == dealerIdClaim.Value)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
