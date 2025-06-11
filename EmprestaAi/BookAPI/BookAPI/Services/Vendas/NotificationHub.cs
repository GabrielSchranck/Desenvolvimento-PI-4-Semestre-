using Microsoft.AspNetCore.SignalR;

namespace BookAPI.Services.Vendas
{
    public class NotificationHub : Hub
    {
        public async Task NotifySeller(string sellerId, string message)
        {
            await Clients.User(sellerId).SendAsync("ReceiveNotification", message);
        }
    }
}
