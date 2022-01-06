using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebIdentity
{
    public class User : IdentityUser
    {
        public string Pass { get; set; }
        public string DisplayName { get; set; }
        public DateTime RegisteredTime { get; set; }
    }
}
