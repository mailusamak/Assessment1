using Assessment1.Models;
using Assessment1.ModelView;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Assessment1.Test.Data
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {

        }
        public DbSet<Batch> Batch { get; set; }
        public DbSet<ACLreadUsers> ACLreadUsers { get; set; }
        public DbSet<ACLreadGroups> ACLreadGroups { get; set; }
        public DbSet<attributes> attributes { get; set; }
    }
}
