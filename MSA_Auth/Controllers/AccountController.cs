using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using MSA_Auth_API.Services;
using MSA_Auth_API.Requests;
using MSA_Auth_API.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MSA_Auth_API.Controllers
{
    [Authorize]
    [Route("api/account")]
    [ApiController]
    [JsonException]
    public class UserController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public UserController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var claim = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email);

            if (claim == null) return Unauthorized();

            var token = await _accountService.GetAccountAsync(new GetAccountRequest { Email = claim.Value });
            return Ok(token);
        }

        [AllowAnonymous]
        [HttpPost("auth")]
        public async Task<IActionResult> SignIn(SignInRequest request)
        {
            var token = await _accountService.SignInAsync(request);

            if (token == null) return BadRequest();
            return Ok(token);
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> AddAccount(AddAccountRequest request)
        {
            var user = await _accountService.AddAccountAsync(request);

            if (user == null) return BadRequest();
            return CreatedAtAction(nameof(Get), new { }, null);
        }
    }
}
