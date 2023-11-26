using Domain.Requests;
using MediatR;

namespace Application.Commands;

public class RegisterToWebinarCommandHandler : IRequestHandler<RegisterWebinarRequest>
{
    public RegisterToWebinarCommandHandler()
    {
        
    }
    
    public Task Handle(RegisterWebinarRequest request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}