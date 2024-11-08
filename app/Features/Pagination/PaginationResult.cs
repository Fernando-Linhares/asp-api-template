namespace Api.App.Features.Pagination;

public record PaginationResult<T>(List<T> Data, object Pagination);