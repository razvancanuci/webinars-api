using Application.Handlers;
using Domain.Entities;
using Domain.Interfaces;
using Moq;

namespace Application.UnitTests.Handlers;

public abstract class RequestHandlerTestsBase<TRequest>
where TRequest : RequestHandlerBase
{    
    protected readonly Mock<IRepository<Webinar>> WebinarRepositoryMock;
    protected readonly Mock<IUnitOfWork> UnitOfWorkMock;
    protected abstract TRequest Handler { get; }

    protected RequestHandlerTestsBase()
    {
        WebinarRepositoryMock = new();
        
        UnitOfWorkMock = new();
        UnitOfWorkMock.Setup(x => x.WebinarRepository).Returns(WebinarRepositoryMock.Object);
    }
}