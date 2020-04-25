﻿using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using PlanningPoker.Dtos;
using PlanningPoker.Hubs;
using PlanningPoker.Interfaces;
using PlanningPoker.Services;

namespace PlanningPoker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VoteController : ControllerBase
    {
        private IHubContext<VoteHub, IHubClient> _hubContext;
        private VoterService _voterService;
        private RoomService _roomService;

        public VoteController(IHubContext<VoteHub, IHubClient> hubContext, VoterService voterService, RoomService roomService)
        {
            _hubContext = hubContext;
            _voterService = voterService;
            _roomService = roomService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> SetupPlayer([FromBody] NewVoterDto newVoterDto)
        {
            if (string.IsNullOrWhiteSpace(newVoterDto.Name))
            {
                return BadRequest();
            }

            // TODO: ADD VOTER TO THE CORRECT ROOM
            var newVoter = _voterService.AddVoter(newVoterDto);
            await _hubContext.Clients.All.VotingUpdated(_voterService.GetAllVoters());

            var locationPath = $"api/vote/voters/{newVoter.Id}";
            var location = GetBaseUri(Request, locationPath);

            return Created(location.ToString(), newVoter);
        }

        [HttpGet("voters")]
        public IActionResult GetVoters()
        {
            return Ok(_voterService.GetAllVoters());
        }

        [HttpGet("voters/{id}")]
        public IActionResult GetVoterById([FromRoute] string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return BadRequest();
            }

            var voter = _voterService.GetVoterById(id);
            if (voter == null)
            {
                return NotFound(voter);
            }

            return Ok(voter);
        }

        [HttpPut("voters/{id}/cast")]
        public async Task<IActionResult> CastVote([FromRoute] string id, [FromBody] UpdatedVoteDto updatedVoteDto)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return BadRequest();
            }

            var voter = _voterService.GetVoterById(id);
            if (voter == null)
            {
                return NotFound(voter);
            }

            _voterService.UpdateVote(id, updatedVoteDto.Point);
            await _hubContext.Clients.All.VotingUpdated(_voterService.GetAllVoters());

            return Ok();
        }

        [HttpDelete("voters/{id}/leave")]
        public async Task<IActionResult> RemoveVoter([FromRoute] string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return BadRequest();
            }

            var voter = _voterService.GetVoterById(id);
            if (voter == null)
            {
                return NotFound(voter);
            }

            _voterService.RemoveVoter(id);
            await _hubContext.Clients.All.VotingUpdated(_voterService.GetAllVoters());

            return NoContent();
        }

        /// <summary>
        ///     Helper uri builder method
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static Uri GetBaseUri(HttpRequest request, string path)
        {
            var uriBuilder = new UriBuilder
            {
                Scheme = request.Scheme,
                Host = request.Host.Host,
                Port = request.Host.Port.GetValueOrDefault(-1),
                Path = path
            };

            return uriBuilder.Uri;
        }
    }
}