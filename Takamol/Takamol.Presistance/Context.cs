using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Ta3alom.Application.Interfaces;
using Takamol.Domain.Entities;

namespace Takamol.Persistence
{
    public partial class Context : IdentityDbContext<AppUser>, IContext
    {
        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<Client> Clients { get; set; }

        public Context(DbContextOptions<Context> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(Context).Assembly);
            base.OnModelCreating(modelBuilder);
        }

        public override int SaveChanges()
        {
            return base.SaveChanges();
        }
    }
}
