using System.Threading.Tasks;

namespace PlanningPoker.Interfaces
{
    public interface IHubClient
    {
        Task BroadcastMessage(string type, string payload);
    }
}
