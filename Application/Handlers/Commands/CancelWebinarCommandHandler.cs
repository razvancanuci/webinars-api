using Application.Handlers.Interfaces;
using Application.Requests;
using Application.Services.Interfaces;
using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Application.Handlers.Commands;

public class CancelWebinarCommandHandler : RequestHandlerBase, ICommandHandler<CancelWebinarRequest>
{
    private readonly ICacheService _cacheService;
    public CancelWebinarCommandHandler(ICacheService cacheService, IUnitOfWork unitOfWork) : base(unitOfWork)
    {
        _cacheService = cacheService;
    }
    
    public async Task<IActionResult> Handle(CancelWebinarRequest request, CancellationToken cancellationToken)
    {
        var queryResult = await UnitOfWork.WebinarRepository.GetAsync(
            criteria: w => w.Id == request.WebinarId);

        var webinar = queryResult.FirstOrDefault();

        if (webinar is null)
        {
            return new NotFoundObjectResult("The webinar with specified id is not available");
        }
        
        await _cacheService.DeleteKeyAsync(request.KeyToDelete);
        
        UnitOfWork.WebinarRepository.Delete(webinar);
        await UnitOfWork.SaveAsync();

        return new NoContentResult();
    }
}