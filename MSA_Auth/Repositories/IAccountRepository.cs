using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MSA_Auth_API.Models;

namespace MSA_Auth_API.Repositories
{
    public interface IAccountRepository : IRepository
    {
        Task<IEnumerable<Account>> GetAsync();
        Task<Account> GetAsync(Guid id);
        Task<Account> Add(Guid id);
    }
}
