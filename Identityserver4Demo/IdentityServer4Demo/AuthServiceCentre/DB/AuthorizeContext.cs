using IdentitySever4.Models;
using Microsoft.EntityFrameworkCore;

namespace AuthServiceCentre
{
    public class AuthorizeContext : DbContext
    {
        public AuthorizeContext(DbContextOptions<AuthorizeContext> options)
                : base(options)
        {

        }

        public DbSet<Account> Account { get; set; }
        public DbSet<Roles> Roles { get; set; }
    }
}
