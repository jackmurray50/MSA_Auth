using System.Threading;
using System.Threading.Tasks;
using MSA_Auth_API.Models;

namespace MSA_Auth_API.Repositories
{
    public interface IAccountRepository : IRepository
    {
        Task<bool> AuthenticateAsync(string email, string password, CancellationToken cancellationToken = default);
        Task<bool> SignUpAsync(Account account, string password, CancellationToken cancellationToken = default);
        Task<Account> GetByEmailAsync(string requestEmail, CancellationToken cancellationToken = default);
    }
}
