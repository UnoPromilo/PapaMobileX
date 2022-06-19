using System.ComponentModel;
using PapaMobileX.Shared.Results;
using PapaMobileX.Shared.Results.Errors;

namespace PapaMobileX.App.BusinessLogic.Services.Interfaces;

public interface IVideoService : INotifyPropertyChanged
{
    public byte[]? LastFrame { get; }

    Task<Result<Error>> StartConnectionAsync(Uri baseUrl);

    Task StopConnection();
}