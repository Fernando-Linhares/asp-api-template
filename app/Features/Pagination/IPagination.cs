namespace Api.App.Features.Pagination;

public interface IPagination<T>
{
    public Task<PaginationResult<T>> Paginate(PaginationInput<T> input);
}