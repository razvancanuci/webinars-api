using Application.Handlers;
using AutoFixture;
using AutoFixture.AutoMoq;
using Domain.Entities;
using Domain.Interfaces;
using Moq;

namespace Application.UnitTests.Handlers;

public abstract class RequestHandlerTestsBase<TRequest>
where TRequest : RequestHandlerBase
{    
    protected readonly Mock<IWebinarRepository> WebinarRepositoryMock;
    protected readonly Mock<IUnitOfWork> UnitOfWorkMock;
    protected abstract TRequest Handler { get; }

    protected IFixture Fixture;

    protected RequestHandlerTestsBase()
    {
        Fixture = new Fixture().Customize(new AutoMoqCustomization());
        WebinarRepositoryMock = Fixture.Freeze<Mock<IWebinarRepository>>();
        
        UnitOfWorkMock = Fixture.Freeze<Mock<IUnitOfWork>>();
        UnitOfWorkMock.Setup(x => x.WebinarRepository).Returns(WebinarRepositoryMock.Object);
    }
}