using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using PapaMobileX.DTOs.SignalR;
using PapaMobileX.Shared.Enums;
using PapaMobileX.Shared.HubDefinitions;

namespace PapaMobileX.Server.SignalR.Hubs;

[Authorize]
public class ControlHub : Hub<IControlHubClientDefinition>, IControlHubServerDefinition
{
    private static readonly SemaphoreSlim ConnectionIdSemaphore = new(1);
    private static volatile string? _connectionId;

    public async Task UpdateSteeringWheelPosition(UpdateSteeringWheelPositionDTO updateSteeringWheelPositionDTO)
    {
        if (VerifyConnection())
        {
            //TODO
        }
    }

    public async Task UpdateAcceleration(UpdateAccelerationDTO updateAccelerationDTO)
    {
        if (VerifyConnection())
        {
            //TODO
        }
    }

    public override async Task OnConnectedAsync()
    {
        //Prevent multiple clients
        await ConnectionIdSemaphore.WaitAsync();
        try
        {
            if (_connectionId is null || (_connectionId == Context.ConnectionId))
                _connectionId = Context.ConnectionId;
            else
                _ = Clients.Caller.StopConnection(DisconnectReason.ConnectionLimit);
        }
        finally
        {
            ConnectionIdSemaphore.Release();
        }

        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        await ConnectionIdSemaphore.WaitAsync();
        try
        {
            if (Context.ConnectionId == _connectionId)
                _connectionId = null;
        }
        finally
        {
            ConnectionIdSemaphore.Release();
        }

        await base.OnDisconnectedAsync(exception);
    }


    private bool VerifyConnection()
    {
        if (Context.ConnectionId == _connectionId)
            return true;

        Clients.Caller.StopConnection(DisconnectReason.ConnectionLimit);
        return false;
    }
}