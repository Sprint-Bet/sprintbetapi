using Microsoft.AspNetCore.SignalR;
using PlanningPoker.Interfaces;

namespace PlanningPoker.Hubs
{
    public class NotifyHub: Hub<IHubClient>
    {
    }
}
