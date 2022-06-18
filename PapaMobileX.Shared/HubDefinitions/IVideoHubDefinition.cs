using PapaMobileX.Shared.Models;

namespace PapaMobileX.Shared.HubDefinitions;

public interface IVideoHubDefinition
{
    public Task Frame(VideoData data);
}