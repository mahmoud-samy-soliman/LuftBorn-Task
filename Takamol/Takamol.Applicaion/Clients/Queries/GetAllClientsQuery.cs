using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ta3alom.Application.Interfaces;
using Takamol.Domain.Entities;

namespace Takamol.Application.Clients.Query
{
    public class GetAllClientsQuery : IRequest<List<Client>>
    {
        public class Handler : IRequestHandler<GetAllClientsQuery, List<Client>>
        {
            private readonly IContext _context;

            public Handler(IContext context)
            {
                _context = context;
            }

            public async Task<List<Client>> Handle(GetAllClientsQuery request, CancellationToken cancellationToken)
            {
                try
                {
                    var clientsWithNames = await _context.Clients
                        .Where(a => !a.Deleted)
                        .Select(client => new Client
                        {
                            Id = client.Id,
                            Name = client.Name,
                            Address = client.Address,
                            Description = client.Description,
                            JobTitle = client.JobTitle,
                            ClientClassification = client.ClientClassification,
                            ClientSource = client.ClientSource,
                            SalesMan = client.SalesMan,
                            CreatedById = _context.AppUsers.Where(a => a.Id == client.CreatedById).Select(a => a.Name).FirstOrDefault(),
                            CreationDate = client.CreationDate,
                            ModifiedById = _context.AppUsers.Where(a => a.Id == client.ModifiedById).Select(a => a.Name).FirstOrDefault(),
                            ModificationDate = client.ModificationDate
                        })
                        .ToListAsync(cancellationToken);

                    return clientsWithNames;
                }
                catch (Exception exp)
                {
                    throw new Exception(exp.Message);
                }
            }
        }
    }
}
