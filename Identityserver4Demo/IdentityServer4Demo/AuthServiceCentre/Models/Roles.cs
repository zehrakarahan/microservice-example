using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IdentitySever4.Models
{
    public class Roles
    {
        [Key]
        public int Id { get; set; }
        public int AccountId { get; set; }
        public int SystemId { get; set; }
        public string RoleNames { get; set; }
    }
}
