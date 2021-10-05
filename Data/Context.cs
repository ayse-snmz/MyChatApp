using Lets2Chat.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lets2Chat.Data
{
    public class Context : IdentityDbContext 
    {
        public Context(DbContextOptions<Context> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Message>()
                .HasOne<AppUser>(m => m.appUser)
                .WithMany(t => t.Messages)
                .HasForeignKey(t => t.UserId);
        }

        public DbSet<Message> Message { get; set; }
    }
}
