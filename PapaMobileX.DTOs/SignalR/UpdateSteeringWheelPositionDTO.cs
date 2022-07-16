using System.ComponentModel.DataAnnotations;

namespace PapaMobileX.DTOs.SignalR;

public class UpdateSteeringWheelPositionDTO
{
    [Range(-1, 1)]
    public double SteeringWheelPosition { get; init; }
}