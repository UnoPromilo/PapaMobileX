using PapaMobileX.Server.BusinessLogic.Services.Interfaces;
using PapaMobileX.Server.DataSource;

namespace PapaMobileX.Server.BusinessLogic.Services.Concrete;

public class UnitOfWork : IUnitOfWork
{
    private readonly CommonContext _context;

    public UnitOfWork(CommonContext context)
    {
        _context = context;
    }

    public Task<int> CommitAsync()
    {
        return _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context?.Dispose();
    }
}