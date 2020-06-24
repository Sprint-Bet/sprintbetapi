using Microsoft.AspNetCore.Mvc;
using SprintBetApi.Dtos;
using SprintBetApi.Services;

namespace SprintBetApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        private IAuthService _authService;

        public AuthenticateController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost]
        public IActionResult RegisterVoter([FromBody] AuthenticateVoterDto authenticateVoterDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var token = _authService.GenerateToken(authenticateVoterDto);
            return Ok(token);
        }
    }
}