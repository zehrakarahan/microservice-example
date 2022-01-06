using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebCAP
{
    public class AppDbContext : DbContext
    {
        public class Person
        {
            public int Id { get; set; }

            public string Name { get; set; }

            public override string ToString()
            {
                return $"Name:{Name}, Id:{Id}";
            }
        }

        [Table("cap.published")]
        public class Published
        {
            public long Id { get; set; }

            public string Name { get; set; }
            public string Content { get; set; }
            public string Retries { get; set; }
            public DateTime Added { get; set; }
            public DateTime? ExpiresAt { get; set; }
            public string StatusName { get; set; }

           
        }


        [Table("cap.received")]
        public class Received
        {
            public long Id { get; set; }

            public string Name { get; set; }
            public string Group { get; set; }
            public string Content { get; set; }
            public string Retries { get; set; }
            public DateTime Added { get; set; }
            public DateTime? ExpiresAt { get; set; }
            public string StatusName { get; set; }


        }
        public AppDbContext(DbContextOptions<AppDbContext> options)
                   : base(options)
        {

        }
        public DbSet<Person> Persons { get; set; }
        public DbSet<Published> Publisheds { get; set; }
        public DbSet<Received> Receiveds { get; set; }

    }

   
}
