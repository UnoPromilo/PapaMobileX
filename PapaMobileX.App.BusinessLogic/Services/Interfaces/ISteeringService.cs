namespace PapaMobileX.App.BusinessLogic.Services.Interfaces;

public interface ISteeringService
{
    event EventHandler<double> RotationChanged;
    void StartMonitoring();
    void StopMonitoring();
}