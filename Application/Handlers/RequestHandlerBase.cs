using Domain.Interfaces;

namespace Application.Handlers;

public abstract class RequestHandlerBase
{
    protected readonly IUnitOfWork UnitOfWork;

    protected RequestHandlerBase(IUnitOfWork unitOfWork)
    {
        UnitOfWork = unitOfWork;
    }
}