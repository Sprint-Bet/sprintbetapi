using System.Threading.Tasks;

namespace PlanningPoker.Interfaces
{
    public interface IHubClient
    {
        Task BroadcastVote(string name, string point);
    }
}
