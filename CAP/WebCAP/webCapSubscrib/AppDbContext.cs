using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace webCapSubscrib
{
    public class AppDbContext : DbContext
    {
        public class Student
        {
            public int Id { get; set; }

            public string Name { get; set; }

            public override string ToString()
            {
                return $"Name:{Name}, Id:{Id}";
            }
        }

        
        public AppDbContext(DbContextOptions<AppDbContext> options)
                   : base(options)
        {

        }
      
        public DbSet<Student> Students { get; set; }

    }

   
}
