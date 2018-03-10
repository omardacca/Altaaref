using AltaarefWebAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AltaarefWebAPI.Contexts
{
    public class AltaarefDbContext : DbContext
    {
        public AltaarefDbContext(DbContextOptions<AltaarefDbContext> options) : base(options) { }
        public AltaarefDbContext() { }
        
        public DbSet<Faculty> Faculty { get; set; }
    }
}
