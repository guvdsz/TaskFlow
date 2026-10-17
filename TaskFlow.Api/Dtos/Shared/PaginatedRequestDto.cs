namespace TaskFlow.Api.Dtos.Shared;

public class PaginatedRequestDto
{
    public int PageSize { get; set; } = 1;
    public int PageNumber { get; set; } = 1;
}
