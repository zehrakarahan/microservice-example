﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IdentitySever4.Models
{
    public class Account
    {
        [Key]
        public int Id { get; set; }
        public string AccountName { get; set; }
        public string Password { get; set; }

       
    }
}
