using System.Diagnostics;
using PapaMobileX.App.BusinessLogic.Services.Interfaces;

namespace PapaMobileX.App.Services.Concrete;

public class RotationService : ISteeringService
{
    private const SensorSpeed Speed = SensorSpeed.Game;
    private const double RollLimit = 0.5d;
    
    public event EventHandler<double>? RotationChanged;
    
    public RotationService()
    {
        Accelerometer.ReadingChanged += AccelerometerOnReadingChanged;
    }
    
    public void StartMonitoring()
    {
        if (Accelerometer.IsMonitoring == false)
            Accelerometer.Start(Speed);
    }

    public void StopMonitoring()
    {
        if (Accelerometer.IsMonitoring)
            Accelerometer.Stop();
    }
    
    private void AccelerometerOnReadingChanged(object? sender, AccelerometerChangedEventArgs e)
    {
        AccelerometerData data = e.Reading;

        double roll = Math.Atan(data.Acceleration.Y / Math.Sqrt(Math.Pow(data.Acceleration.X, 2.0) + Math.Pow(data.Acceleration.Z, 2.0)));
        double normalizedRoll = NormalizeRoll(roll);
        RotationChanged?.Invoke(this, normalizedRoll);
    }
    
    private static double NormalizeRoll(double roll)
    {
        if (roll < -RollLimit)
            roll = -RollLimit;
        else if (roll > RollLimit)
            roll = RollLimit;
        return roll / RollLimit;
    }
}