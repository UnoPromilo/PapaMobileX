using System.ComponentModel;
using System.Runtime.CompilerServices;
using PapaMobileX.App.BusinessLogic.Services.Interfaces;

namespace PapaMobileX.App.BusinessLogic.Services.Concrete;

public class VideoService : IVideoService
{
    private Stream? _lastFrame;

    public Stream? LastFrame
    {
        get
        {
            lock (this)
            {
                return _lastFrame;
            }
        }
        private set
        {
            lock (this)
            {
                _lastFrame?.Dispose();
                SetField(ref _lastFrame, value);
            }
        } 
    }

    public void UpdateFrame(string image)
    {
        LastFrame = new MemoryStream(Convert.FromBase64String(image));
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value))
            return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }
}