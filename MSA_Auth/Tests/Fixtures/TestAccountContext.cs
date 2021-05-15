using Microsoft.EntityFrameworkCore;
using MSA_Auth_API.Entities;

namespace MSA_Auth_API.Tests.Fixtures
{
    public class TestAccountContext : AccountContext
    {
        public TestAccountContext(DbContextOptions<AccountContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Seed<Account>("./Data/artist.json");

            base.OnModelCreating(modelBuilder);
        }
    }
}
