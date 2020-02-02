using PlanningPoker.Dtos;
using System.Threading.Tasks;

namespace PlanningPoker.Interfaces
{
    public interface IHubClient
    {
        Task BroadcastVote(Vote vote);
        //Task PlayerAdded(string name);
    }
}
