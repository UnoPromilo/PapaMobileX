using Microsoft.EntityFrameworkCore;
using PapaMobileX.Server.DataSource.Models;

namespace PapaMobileX.Server.DataSource.Repository;

public class AccountsRepository
{
    private readonly CommonContext _context;

    public AccountsRepository(CommonContext context)
    {
        _context = context;
    }

    public Task<Account?> GetAccountByUserName(string userName)
    {
        return _context.Accounts.Where(u => u.UserName == userName).SingleOrDefaultAsync();
    }

    public void AddAccount(Account account)
    {
        _context.Accounts.Add(account);
    }
}