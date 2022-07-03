namespace PapaMobileX.App.Controls;

public interface IVideoImage : IView
{
    public Stream? VideoFrame { get; set; }
}