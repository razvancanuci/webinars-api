namespace Domain.Dtos;

public sealed record WebinarPagedDto(int pages, IEnumerable<WebinarShortInfoDto> webinars);