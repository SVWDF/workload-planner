using Microsoft.AspNetCore.SignalR;

namespace WorkloadPlanner.Hubs
{
    public class ScrumboardHub : Hub
    {
        public async Task JoinScrumboard(int scrumboardId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"scrumboard-{scrumboardId}");
        }

        public async Task LeaveScrumboard(int scrumboardId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"scrumboard-{scrumboardId}");
        }
    }
}