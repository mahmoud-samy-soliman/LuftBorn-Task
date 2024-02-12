using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Takamol.Domain.Entities;

namespace Ta3alom.Application.Interfaces
{
    public interface IContext
    {
         DbSet<AppUser> AppUsers { get; set; }
         DbSet<Client> Clients { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
