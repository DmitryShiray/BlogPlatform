using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace BlogPlatform.Hubs
{
    public class CommentsHub : Hub<ICommentsHub>
    {
        public async Task RefreshCommentsCount()
        {
            await Clients.All.RefreshCommentsCount();
        }
    }
}
