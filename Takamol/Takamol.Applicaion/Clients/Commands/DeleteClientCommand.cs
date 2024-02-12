using MediatR;
using Microsoft.EntityFrameworkCore;
using Ta3alom.Application.Interfaces;

namespace Takamol.Application.Clients.Commands.Delete
{
    public class DeleteClientCommand : IRequest<string>
    {
        public string ClientId { get; set; }
        public class Handler : IRequestHandler<DeleteClientCommand, string>
        {
            private readonly IContext _context;
            public Handler(IContext context)
            {
                _context = context;
            }
            public async Task<string> Handle(DeleteClientCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    var client = await _context.Clients.Where(a => a.Id == request.ClientId).FirstOrDefaultAsync(cancellationToken);


                    client.Deleted = true;

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
