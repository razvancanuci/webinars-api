using Domain.Entities;
using Domain.Interfaces;

namespace DataAccess.Repositories;

public class WebinarRepository : GenericRepository<Webinar>, IWebinarRepository
{
    public WebinarRepository(WebinarContext context) : base(context)
    {
        
    }
}