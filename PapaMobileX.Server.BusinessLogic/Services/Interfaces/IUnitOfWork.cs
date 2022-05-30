namespace PapaMobileX.Server.BusinessLogic.Services.Interfaces;

public interface IUnitOfWork : IDisposable
{
    Task<int> CommitAsync();
}