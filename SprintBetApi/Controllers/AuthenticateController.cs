using Microsoft.AspNetCore.Mvc;
using SprintBetApi.Dtos;
using SprintBetApi.Services;
using System.Linq;
using System.Security.Claims;

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

            var token = _authService.GenerateToken(authenticateVoterDto.Id, authenticateVoterDto.RoomId);
            return Ok(token);
        }

        [HttpGet("voter/{voterId}")]
        public IActionResult DecodeToken([FromRoute] string voterId, [FromHeader] string Authorization)
        {
            var voterIdFromRoute = voterId;
            var authHeader = Authorization;
            if (string.IsNullOrWhiteSpace(voterIdFromRoute) || string.IsNullOrWhiteSpace(authHeader))
            {
                return BadRequest("blah");
            }

            var token = _authService.ReadToken(authHeader);
            var voterIdFromToken = token.Claims.First(claim => claim.Type == ClaimTypes.NameIdentifier).ToString();
            if (voterIdFromRoute == voterIdFromToken)
            {
                return Unauthorized("doesn't match");
            }

            return Ok(new { VoterIdFromRoute = voterIdFromRoute, VoterIdFromToken = voterIdFromToken });

        }
    }
}