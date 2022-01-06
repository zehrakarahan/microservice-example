using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebIdentity
{
    public class AppContext:DbContext
    {
        public AppContext(DbContextOptions<AppContext> db) : base(db)
        {

        }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<IdentityRole> IdentityRoles { get; set; }
    }
}
