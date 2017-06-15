using System.Linq;
using Microsoft.AspNet.SignalR;

namespace Micro.Hubs
{
    public class NotificationHub : Hub
    {
        public void Hello()
        {
            Clients.All.hello();
            
        }
    }
}