using System.ComponentModel;

namespace PapaMobileX.App.BusinessLogic.Services.Interfaces;

public interface IVideoService : INotifyPropertyChanged
{
    public Stream? LastFrame { get; }

    public void UpdateFrame(string image);
}