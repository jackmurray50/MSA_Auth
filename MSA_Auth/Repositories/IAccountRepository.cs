using System.Threading;
using System.Threading.Tasks;
using MSA_Auth_API.Entities;

namespace MSA_Auth_API.Repositories
{
    public interface IAccountRepository : IRepository
    {
        Task<bool> AuthenticateAsync(string email, string password, CancellationToken cancellationToken = default);
        Task<bool> AddAccountAsync(Account account, string hash, string salt, CancellationToken cancellationToken = default);
        Task<Account> GetByEmailAsync(string requestEmail, CancellationToken cancellationToken = default);
    }
}
