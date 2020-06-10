using Microsoft.AspNetCore.Mvc.Filters;
using SprintBet.Services;

namespace SprintBetApi.Attributes
{
    public class ValidateVoter : IActionFilter
    {
        private IVoterService _voterService;

        public ValidateVoter(IVoterService voterService)
        {
            _voterService = voterService;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var voterId = context.RouteData.Values["voterId"].ToString();
            if (string.IsNullOrWhiteSpace(voterId))
            {
                context.ModelState.AddModelError("voterId", "Invalid voter id");
            }

            if (!context.ModelState.IsValid)
            {
                return;
            }

            var voter = _voterService.GetVoterById(voterId);
            if (voter == null)
            {
                context.ModelState.AddModelError("voterNotFound", "No voter found for the provided id");
            }
        }

        public void OnActionExecuted(ActionExecutedContext context) { }
    }
}
