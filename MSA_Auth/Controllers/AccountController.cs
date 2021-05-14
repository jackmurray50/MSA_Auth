using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using MSA_Auth_API.Services;
using MediatR;
using MSA_Auth_API.Requests;

namespace MSA_Auth_API.Controllers
{
    [Route("api/account")]
    [ApiController]
    [JsonException]
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpGet("{email:string}")]
        public async Task<IActionResult> GetById(string email)
        {
            var result = await _accountService.GetAccountAsync(new GetAccountRequest { Email = email });
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Post(AddAccountRequest request)
        {
            var result = await _accountService.AddAccountAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = result.Email }, null);
        }
    }
}
