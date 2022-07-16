using PapaMobileX.Shared.Enums;

namespace PapaMobileX.Shared.HubDefinitions;

public interface IControlHubClientDefinition
{
    Task StopConnection(DisconnectReason reason);
}