using PapaMobileX.DTOs.SignalR;

namespace PapaMobileX.Shared.HubDefinitions;

public interface IControlHubServerDefinition
{
    public Task UpdateSteeringWheelPosition(UpdateSteeringWheelPositionDTO updateSteeringWheelPositionDTO);

    public Task UpdateAcceleration(UpdateAccelerationDTO updateAccelerationDTO);
}