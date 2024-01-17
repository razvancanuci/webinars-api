namespace Application.Requests.Interfaces;

public interface IPaginatedRequest
{
    public int ItemsPerPage { get; }
    public int Page { get; }
}