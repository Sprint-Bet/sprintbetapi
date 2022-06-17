using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SprintBetApi.Dtos;
using SprintBetApi.Hubs;
using SprintBetApi.Services;

namespace SprintBetApi.Controllers
{
  [Route("[controller]")]
  [ApiController]
  public class VotersController : SprintBetBaseController
  {
    private IHubContext<VoteHub, IVoteHub> _hubContext;
    private IVoterService _voterService;
    private IRoomService _roomService;

    public VotersController(
        IHubContext<VoteHub, IVoteHub> hubContext,
        IVoterService voterService,
        IRoomService roomService)
    {
      _hubContext = hubContext;
      _voterService = voterService;
      _roomService = roomService;
    }

    [HttpGet]
    public IActionResult GetVoters([FromQuery] string roomId = "")
    {
      if (string.IsNullOrWhiteSpace(roomId))
      {
        return Ok(_voterService.GetAllVoters());
      }

      return Ok(_voterService.GetVotersByRoomId(roomId));
    }

    [HttpGet("{voterId}")]
    public IActionResult GetVoterById([FromRoute] string voterId)
    {
      if (!ModelState.IsValid)
      {
        return ModelState.ContainsKey("voterNotFound")
            ? (IActionResult)NotFound(ModelState.Values.First(v => v.Errors.Count > 0))
            : (IActionResult)BadRequest(ModelState.Values.First(v => v.Errors.Count > 0));
      }

      var voter = _voterService.GetVoterById(voterId);
      return Ok(voter);
    }

    [HttpPost]
    public async Task<IActionResult> SetupPlayer([FromBody] NewVoterDto newVoterDto)
    {
      if (!ModelState.IsValid)
      {
        return BadRequest(ModelState);
      }

      var room = _roomService.GetRoomById(newVoterDto.Group);
      if (room == null)
      {
        return BadRequest("No room found for the provided id");
      }

      var newVoter = _voterService.AddVoter(newVoterDto, room);

      await _hubContext.Groups.AddToGroupAsync(newVoterDto.ConnectionId, newVoter.Room.Id);
      await _hubContext.Clients.Group(newVoter.Room.Id).VotingUpdated(_voterService.GetVotersByRoomId(newVoter.Room.Id));

      var resourcePath = GetBaseUri(Request, $"voters/{newVoter.Id}").ToString();
      return Created(resourcePath, newVoter);
    }

    [HttpGet("{voterId}/reconnect")]
    public async Task<IActionResult> GetVoterById([FromRoute] string voterId, [FromHeader] string connectionId)
    {
      if (!ModelState.IsValid)
      {
        return ModelState.ContainsKey("voterNotFound")
            ? (IActionResult)NotFound(ModelState.Values.First(v => v.Errors.Count > 0))
            : (IActionResult)BadRequest(ModelState.Values.First(v => v.Errors.Count > 0));
      }

      if (string.IsNullOrWhiteSpace(connectionId))
      {
        return BadRequest(new ErrorMessage("Invalid connection id"));
      }

      var voter = _voterService.GetVoterById(voterId);
      if (voter == null) {
        return NotFound(new ErrorMessage("Could not find voter"));
      }

      var room = _roomService.GetRoomById(voter.Room.Id);
      if (room == null) {
        return NotFound(new ErrorMessage("Could not find room, or has already closed."));
      }

      await _hubContext.Groups.AddToGroupAsync(connectionId, voter.Room.Id);

      return Ok(voter);
    }

    [HttpPut("{voterId}/point")]
    public async Task<IActionResult> CastVote([FromRoute] string voterId, [FromBody] UpdatedVoteDto updatedVoteDto)
    {
      if (!ModelState.IsValid)
      {
        return ModelState.ContainsKey("voterNotFound")
            ? (IActionResult)NotFound(ModelState.Values.First(v => v.Errors.Count > 0))
            : (IActionResult)BadRequest(ModelState.Values.First(v => v.Errors.Count > 0));
      }

      var voter = _voterService.GetVoterById(voterId);
      if (updatedVoteDto.Point != "" && !voter.Room.Items.Contains(updatedVoteDto.Point))
      {
        return BadRequest(new ErrorMessage("Invalid vote value"));
      }

      voter.Point = updatedVoteDto.Point;
      _voterService.UpdateVoter(voter);

      await _hubContext.Clients.Group(voter.Room.Id).VotingUpdated(_voterService.GetVotersByRoomId(voter.Room.Id));

      return Ok(voter.Point);
    }


    [HttpPut("{voterId}/role")]
    public async Task<IActionResult> ChangeRole([FromRoute] string voterId, [FromBody] UpdatedRoleDto updatedRoleDto)
    {
      if (!ModelState.IsValid)
      {
        return ModelState.ContainsKey("voterNotFound")
            ? (IActionResult)NotFound(ModelState.Values.First(v => v.Errors.Count > 0))
            : (IActionResult)BadRequest(ModelState.Values.First(v => v.Errors.Count > 0));
      }

      var voter = _voterService.GetVoterById(voterId);
      voter.Role = updatedRoleDto.Role;

      _voterService.UpdateVoter(voter);
      await _hubContext.Clients.Group(voter.Room.Id).VotingUpdated(_voterService.GetVotersByRoomId(voter.Room.Id));

      return Ok(voter.Role);
    }

    [HttpDelete("{voterId}")]
    public async Task<IActionResult> RemoveVoter([FromRoute] string voterId, [FromHeader] string connectionId)
    {
      if (!ModelState.IsValid)
      {
        return ModelState.ContainsKey("voterNotFound")
            ? (IActionResult)NotFound(ModelState.Values.First(v => v.Errors.Count > 0))
            : (IActionResult)BadRequest(ModelState.Values.First(v => v.Errors.Count > 0));
      }

      if (string.IsNullOrWhiteSpace(connectionId))
      {
        return BadRequest(new ErrorMessage("Invalid connection id"));
      }

      var voter = _voterService.GetVoterById(voterId);
      _voterService.RemoveVoterById(voterId);

      await _hubContext.Groups.RemoveFromGroupAsync(connectionId, voter.Room.Id);
      await _hubContext.Clients.Group(voter.Room.Id).VotingUpdated(_voterService.GetVotersByRoomId(voter.Room.Id));

      return NoContent();
    }

  }
}