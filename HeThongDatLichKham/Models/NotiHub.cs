using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace HeThongDatLichKham.Models
{
    public class NotiHub : Hub
    {
        public override async Task OnConnectedAsync()
        {
            var user = Context.User;
            if (user != null && user.Identity.IsAuthenticated)
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var isDoctor = user.IsInRole("Doctor");

                if (!string.IsNullOrEmpty(userId) && isDoctor)
                {
                    await Groups.AddToGroupAsync(Context.ConnectionId, $"user-{userId}");
                    Console.WriteLine($"✅ User {userId} đã join group user-{userId}");
                }
            }

            await base.OnConnectedAsync();
        }

    }
}
