using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSA_Auth_API.Requests
{
    public class SignInRequest
    {
        public string Email { get; set; }
        public string Hash { get; set; }
    }
}
