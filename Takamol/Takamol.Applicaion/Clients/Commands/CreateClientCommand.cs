using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Ta3alom.Application.Interfaces;
using Takamol.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Takamol.Application.Clients.CreateClientCommand
{
    public class CreateClientCommand : IRequest<string>
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string Description { get; set; }
        public string JobTitle { get; set; }
        public string ClientClassification { get; set; }
        public string ClientSource { get; set; }
        public string SalesMan { get; set; }
        public string CreatedById { get; set; }
        public class Handler : IRequestHandler<CreateClientCommand, string>
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

            public async Task<string> Handle(CreateClientCommand request, CancellationToken cancellationToken)
            {
                try
                {
                        var client = new Client
                        {
                            Id = Guid.NewGuid().ToString(),
                            Name = request.Name,
                            Address = request.Address,
                            Description = request.Description,
                            JobTitle = request.JobTitle,
                            ClientClassification = request.ClientClassification,
                            ClientSource = request.ClientSource,
                            SalesMan = request.SalesMan,
                            CreationDate = DateTime.Now,
                            CreatedById = request.CreatedById,
                        };

                        await _context.Clients.AddAsync(client);
                        await _context.SaveChangesAsync(cancellationToken);
                        return client.Id;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                    return null;
                }
            }
        }
    }
}
