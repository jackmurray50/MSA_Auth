using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSA_Auth_API.Models
{
    public class Account
    {
        public Guid Id { get; set; }
        public string Salt { get; set; }
        public string Hash { get; set; }
        public string Email { get; set; }
        public string AccountType { get; set; }

    }
}
