namespace PapaMobileX.App.BusinessLogic.Services.Interfaces;

public interface IAccelerationControlService
{
    void UpdateAcceleration(double acceleration);

    void UpdateBreakPosition(bool breakPosition);
}