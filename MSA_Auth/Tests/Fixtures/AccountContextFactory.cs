using System.Collections.Generic;
using System.Linq;
using System.Threading;
using MSA_Auth_API.Entities;
using MSA_Auth_API.Repositories;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace MSA_Auth_API.Tests.Fixtures
{
    public class AccountContextFactory
    {
        private readonly PasswordHasher<Account> _passwordHasher;
        private readonly IList<Account> _accounts;

        public AccountContextFactory()
        {
            _passwordHasher = new PasswordHasher<Account>();

            _accounts = new List<Account>();

            var account = new Account
            {
                Email = "jack.murray50@gmail.com",
                AccountType = "User"
            };
            account.PasswordHash = _passwordHasher.HashPassword(account, "TESTPASSWORD");

            _accounts.Add(account);
        }

        public IAccountRepository InMemoryAccountManager => GetInMemoryAccountManager();

        private IAccountRepository GetInMemoryAccountManager()
        {
            var fakeAccountService = new Mock<IAccountRepository>();

            fakeAccountService.Setup(x => x.AuthenticateAsync(It.IsAny<string>(), It.IsAny<string>(), CancellationToken.None))
                .ReturnsAsync((string email, string password, CancellationToken token) =>
                {
                    var account = _accounts.FirstOrDefault(x => x.Email == email);

                    if (account == null) return false;

                    var result = _passwordHasher.VerifyHashedPassword(account, account.PasswordHash, password);
                    return result == PasswordVerificationResult.Success;
                });

            fakeAccountService.Setup(x => x.GetByEmailAsync(It.IsAny<string>(), CancellationToken.None))
                .ReturnsAsync((string email, CancellationToken token) => _accounts.First(x => x.Email == email));

            fakeAccountService.Setup(x => x.AddAccountAsync(It.IsAny<Account>(), It.IsAny<string>(), It.IsAny<string>(), CancellationToken.None))
                .ReturnsAsync((Account account, string password, CancellationToken token) =>
                {
                    account.PasswordHash = _passwordHasher.HashPassword(account, password);
                    _accounts.Add(account);
                    return true;
                });

            return fakeAccountService.Object;
        }
    }
}
