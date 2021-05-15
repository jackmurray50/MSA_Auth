using System;
using Microsoft.AspNetCore.Identity;

namespace MSA_Auth_API.Entities
{
    public class Account : IdentityUser
    {
        public string AccountType { get; set; }

    }
}
