namespace Api.App.Features.Pagination;

public record PaginationInput<T>(int Page, int PerPage, int Total, IQueryable<T> Filter);