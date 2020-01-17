using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using PlanningPoker.Dtos;
using PlanningPoker.Hubs;
using PlanningPoker.Interfaces;
using System;

namespace PlanningPoker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private IHubContext<NotifyHub, IHubClient> _hubContext;

        public MessageController(IHubContext<NotifyHub, IHubClient> hubContext)
        {
            _hubContext = hubContext;
        }

        [HttpPost]
        public string Post([FromBody]Message message)
        {
            string returnMessage;

            try
            {
                _hubContext.Clients.All.BroadcastMessage(message.Type, message.Payload);
                returnMessage = "Success";
            }
            catch (Exception error)
            {
                returnMessage = error.ToString();
            }

            return returnMessage;
        }
    }
}