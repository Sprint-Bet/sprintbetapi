using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Net.Http.Headers;
using SprintBetApi.Auth.Requirements;
using SprintBetApi.Services;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SprintBetApi.Auth.Handlers
{
    public class VoterIdMatchesRequestHandler : AuthorizationHandler<VoterIdMatchesRequestRequirement>
    {
        private readonly IActionContextAccessor _actionContextAccessor;
        private IAuthService _authService;

        public VoterIdMatchesRequestHandler(
            IActionContextAccessor actionContextAccessor,
            IAuthService authService
        ) {
            _actionContextAccessor = actionContextAccessor;
            _authService = authService;
        }

        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            VoterIdMatchesRequestRequirement requirement
        ) {
            var voterIdFromRoute = _actionContextAccessor.ActionContext.RouteData.Values["voterId"].ToString();
            var authHeader = _actionContextAccessor.ActionContext.HttpContext.Request.Headers[HeaderNames.Authorization].ToString();
            if (string.IsNullOrWhiteSpace(voterIdFromRoute) || string.IsNullOrWhiteSpace(authHeader))
            {
                return Task.CompletedTask;
            }

            var token = _authService.ReadToken(authHeader);
            var voterIdFromToken = token.Claims.First(claim => claim.Type == ClaimTypes.NameIdentifier).ToString();
            if (voterIdFromRoute == voterIdFromToken)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
