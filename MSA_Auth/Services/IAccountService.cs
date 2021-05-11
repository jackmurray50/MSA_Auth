using System.Threading;
using System.Threading.Tasks;
using MSA_Auth_API.Models;
using MSA_Auth_API.Responses;
using MSA_Auth_API.Requests;

namespace MSA_Auth_API.Services
{
    public interface IAccountService
    {
        Task<AccountResponse> GetUserAsync(GetAccountRequest request, CancellationToken cancellationToken = default);
        Task<AccountResponse> SignUpAsync(SignUpRequest request, CancellationToken cancellationToken = default);
        Task<TokenResponse> SignInAsync(SignInRequest request, CancellationToken cancellationToken = default);
    }
}
