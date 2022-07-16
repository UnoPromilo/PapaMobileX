using System.ComponentModel.DataAnnotations;

namespace PapaMobileX.DTOs.SignalR;

public class UpdateAccelerationDTO
{
    [Range(0, 1)]
    public double Acceleration { get; init; }

    public bool Break { get; init; }
}