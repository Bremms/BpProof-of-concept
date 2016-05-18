using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ProjectLeague.Models.DAL
{
    public class DbEntitiesContext : DbContext
    {
        public DbEntitiesContext() : base("mysqlCon") { }
        public DbSet<Group> Groups { get; set; }
        public DbSet<ApplicationUser> Users { get; set; }
        public DbSet<Client> Clients { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
            {modelBuilder.Entity<IdentityUserLogin>().HasKey<string>(l => l.UserId);
            modelBuilder.Entity<IdentityRole>().HasKey<string>(r => r.Id);
            modelBuilder.Entity<IdentityUserRole>().HasKey(r => new { r.RoleId, r.UserId });
            modelBuilder.Entity<Group>().HasMany(w => w.Users).WithRequired(b => b.Group).HasForeignKey(w => w.Group_id);
            
            // Add other non-cascading FK declarations here

            //base.OnModelCreating(builder);
        }
        }
}