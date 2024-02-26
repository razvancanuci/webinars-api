namespace Domain.Interfaces.Requests;

public interface IPaginatedRequest
{
    public int ItemsPerPage { get; }
    public int Page { get; }
}