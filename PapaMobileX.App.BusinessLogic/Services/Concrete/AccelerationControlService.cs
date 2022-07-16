using PapaMobileX.App.BusinessLogic.Services.Interfaces;
using PapaMobileX.DTOs.SignalR;

namespace PapaMobileX.App.BusinessLogic.Services.Concrete;

public class AccelerationControlService : IAccelerationControlService
{
    private readonly ISignalRSender _signalRSender;
    private readonly TimeSpan _updateDelay = TimeSpan.FromSeconds(0.1);
    private readonly object _updateLock = new();
    private double _lastKnownAcceleration;
    private bool _lastKnownBreakPosition;

    private Task? _updateAccelerationTask;

    public AccelerationControlService(ISignalRSender signalRSender)
    {
        _signalRSender = signalRSender;
    }

    public void UpdateAcceleration(double acceleration)
    {
        _lastKnownAcceleration = acceleration;
        SendUpdate();
    }

    public void UpdateBreakPosition(bool breakPosition)
    {
        _lastKnownBreakPosition = breakPosition;
        SendUpdate();
    }

    private void SendUpdate()
    {
        lock (_updateLock)
        {
            if (_updateAccelerationTask is { IsCompleted: false })
                return;

            _updateAccelerationTask = Task.Delay(_updateDelay);
            _updateAccelerationTask.ContinueWith(_ =>
            {
                _signalRSender.SendMessage(new UpdateAccelerationDTO
                {
                    Acceleration = _lastKnownAcceleration,
                    Break = _lastKnownBreakPosition
                });
            });
        }
    }
}