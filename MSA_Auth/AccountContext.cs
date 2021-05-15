using System.Threading;
using System.Threading.Tasks;
using MSA_Auth_API.Entities;
using MSA_Auth_API.Repositories;
using MSA_Auth_API.SchemaDefinitions;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MSA_Auth_API
{
    public class AccountContext : IdentityDbContext<Account>, IUnitOfWork
    {
        public const string DEFAULT_SCHEMA = "account";

        public AccountContext(DbContextOptions<AccountContext> options) : base(options)
        {
        }

        public DbSet<Account> Accounts { get; set; }

        public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default)
        {
            await SaveChangesAsync(cancellationToken);
            return true;
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new AccountEntitySchemaConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}
