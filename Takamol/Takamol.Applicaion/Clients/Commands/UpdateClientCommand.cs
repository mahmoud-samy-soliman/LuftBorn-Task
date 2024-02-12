using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Ta3alom.Application.Interfaces;
using Takamol.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace Takamol.Application.Clients.UpdateClientCommand
{
    public class UpdateClientCommand : IRequest<string>
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Description { get; set; }
        public string JobTitle { get; set; }
        public string ClientClassification { get; set; }
        public string ClientSource { get; set; }
        public string SalesMan { get; set; }
        public string ModifiedById { get; set; }
        public class Handler : IRequestHandler<UpdateClientCommand, string>
        {
            private readonly IContext _context;
            private readonly IHttpContextAccessor _httpContextAccessor;
            private readonly UserManager<AppUser> _userManager;

            public Handler(IContext context, IHttpContextAccessor httpContextAccessor, UserManager<AppUser> userManager)
            {
                _context = context;
                _httpContextAccessor = httpContextAccessor;
                _userManager = userManager;
            }

            public async Task<string> Handle(UpdateClientCommand request, CancellationToken cancellationToken)
            {
                var client = await _context.Clients.Where(a => a.Id == request.Id && !a.Deleted).FirstOrDefaultAsync();
                if (client != null)
                {
                    try
                    {
                        client.Id = request.Id;
                        client.SalesMan = request.SalesMan;
                        client.Name = request.Name;
                        client.Description = request.Description;
                        client.JobTitle = request.JobTitle;
                        client.ClientClassification = request.ClientClassification;
                        client.ClientSource = request.ClientSource;
                        client.Address = request.Address;
                        client.ModifiedById = request.ModifiedById;
                        client.ModificationDate = DateTime.Now;

                        await _context.SaveChangesAsync(cancellationToken);
                        return client.Id;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"An error occurred: {ex.Message}");
                        return null;
                    }
                }
                return null;
            }
        }
    }
}
