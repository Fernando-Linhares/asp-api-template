using Api.App.Features.Pagination;
using Api.App.Models;

namespace Api.App.Providers;

public static class ModerationServiceProvider
{
    public static void Bind(WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IPagination<User>, Pagination<User>>();
    }
}