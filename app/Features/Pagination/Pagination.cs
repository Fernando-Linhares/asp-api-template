using Microsoft.EntityFrameworkCore;

namespace Api.App.Features.Pagination;

public class Pagination<T> : IPagination<T>
{
    public async Task<PaginationResult<T>> Paginate(PaginationInput<T> input)
    {
        var data = await input.Filter
            .Skip((input.Page - 1) * input.PerPage)
            .Take(input.PerPage)
            .ToListAsync(); 
        
        int pages = (int) Math.Ceiling((decimal) input.Total /  input.PerPage);
        
        return new PaginationResult<T>(data, new
        {
            page = input.Page,
            next = input.Page + (input.Page != pages ? 1 : 0),
            count = data.Count,
            pages = pages,
            total = input.Total
        });
    }
}