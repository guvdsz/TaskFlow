using System;

namespace TaskFlow.Api.Dtos.Shared;

public class PaginatedResponseDto<T>
{
    public IEnumerable<T> Data { get; set; } = [];
    public required int CurrentPage { get; set; }
    public required int PageSize { get; set; }
    public required int TotalPages { get; set; }
    public required int TotalItems { get; set; }
}
