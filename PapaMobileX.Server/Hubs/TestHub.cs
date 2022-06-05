using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using PapaMobileX.DTOs.SignalR;

namespace PapaMobileX.Server.Hubs;

[Authorize]
public class TestHub : Hub
{
    public async Task SendMessage(TestDTO testDTO)
    {
        await Clients.All.SendAsync("ReceiveMessage", testDTO);
    }
}