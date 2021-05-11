using System.Threading;
using System.Threading.Tasks;
using MSA_Auth_API.Models;
using MSA_Auth_API.Responses;
using MSA_Auth_API.Requests;

namespace MSA_Auth_API.Services
{
    public interface IUserService
    {
        Task<UserResponse> GetUserAsync(GetUserRequest request, CancellationToken cancellationToken = default);
        Task<UserResponse> SignUpAsync(SignUpRequest request, CancellationToken cancellationToken = default);
        Task<TokenResponse> SignInAsync(SignInRequest request, CancellationToken cancellationToken = default);
    }
}
