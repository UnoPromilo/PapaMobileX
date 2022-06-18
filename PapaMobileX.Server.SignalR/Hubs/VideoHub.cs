using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using PapaMobileX.Shared.HubDefinitions;

namespace PapaMobileX.Server.SignalR.Hubs;

[Authorize]
public class VideoHub : Hub<IVideoHubDefinition>
{
}