using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MSA_Auth_API.Configurations;
using MSA_Auth_API.Repositories;
using MSA_Auth_API.Responses;
using MSA_Auth_API.Requests;
using MSA_Auth_API.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace MSA_Auth_API.Services
{
    public class AccountService : IAccountService
    {
        private readonly AuthenticationSettings _authenticationSettings;
        private readonly IAccountRepository _accountRepository;

        public AccountService(IAccountRepository accountRepository, IOptions<AuthenticationSettings> authenticationSettings)
        {
            _accountRepository = accountRepository;
            _authenticationSettings = authenticationSettings.Value;
        }

        public async Task<AccountResponse> GetAccountAsync(GetAccountRequest request, CancellationToken cancellationToken)
        {
            var response = await _accountRepository.GetByEmailAsync(request.Email, cancellationToken);
            return new AccountResponse {Email = response.Email };
        }

        public async Task<AccountResponse> AddAccountAsync(AddAccountRequest request, CancellationToken cancellationToken)
        {
            var account = new Models.Account { Email = request.Email};
            bool isCreated = await _accountRepository.AddAccountAsync(account, request.Hash, request.Salt, cancellationToken);

            return !isCreated ? null : new AccountResponse {Email = request.Email };
        }

        public async Task<TokenResponse> SignInAsync(SignInRequest request, CancellationToken cancellationToken)
        {
            bool isAuthenticated = await _accountRepository.AuthenticateAsync(request.Email, request.Password, cancellationToken);

            return !isAuthenticated ? null : new TokenResponse { Token = GenerateSecurityToken(request) };
        }

        private string GenerateSecurityToken(SignInRequest request)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_authenticationSettings.Secret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Email, request.Email)
                }),
                Expires = DateTime.UtcNow.AddDays(_authenticationSettings.ExpirationDays),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
