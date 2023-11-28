using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories;

public class WebinarRepository : GenericRepository<Webinar>
{
    public WebinarRepository(WebinarContext context) : base(context)
    {
        
    }
}