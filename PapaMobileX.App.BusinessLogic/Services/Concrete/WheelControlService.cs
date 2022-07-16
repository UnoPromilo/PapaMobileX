using PapaMobileX.App.BusinessLogic.Services.Interfaces;
using PapaMobileX.DTOs.SignalR;

namespace PapaMobileX.App.BusinessLogic.Services.Concrete;

public class WheelControlService : IWheelControlService
{
    private readonly ISignalRSender _signalRSender;
    private readonly ISteeringService _steeringService;
    private readonly TimeSpan _updateDelay = TimeSpan.FromSeconds(0.1);
    private readonly object _updateLock = new();
    private double _lastKnownSteeringWheelPosition;
    private Task? _updateTask;

    public WheelControlService(ISteeringService steeringService, ISignalRSender signalRSender)
    {
        _steeringService = steeringService;
        _signalRSender = signalRSender;
    }

    public void StartMonitoring()
    {
        _steeringService.RotationChanged += SteeringServiceOnRotationChanged;
        _steeringService.StartMonitoring();
    }

    public void StopMonitoring()
    {
        _steeringService.StopMonitoring();
        _steeringService.RotationChanged -= SteeringServiceOnRotationChanged;
    }

    private void SteeringServiceOnRotationChanged(object? sender, double e)
    {
        _lastKnownSteeringWheelPosition = e;
        SendUpdate();
    }

    private void SendUpdate()
    {
        lock (_updateLock)
        {
            if (_updateTask is { IsCompleted: false })
                return;

            _updateTask = Task.Delay(_updateDelay);
            _updateTask.ContinueWith(_ =>
            {
                _signalRSender.SendMessage(new UpdateSteeringWheelPositionDTO
                {
                    SteeringWheelPosition = _lastKnownSteeringWheelPosition
                });
            });
        }
    }
}