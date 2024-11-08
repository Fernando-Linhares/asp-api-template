using Api.App.Database;
using Microsoft.EntityFrameworkCore;

namespace Api.App.Features.Database;

public static class DatabaseContextSingleton
{
    private static DatabaseContext? Instance { get; set; }

    public static DatabaseContext GetInstance()
    {
        if (Instance == null)
        {
            var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
            return new DatabaseContext(optionsBuilder.Options);
        }
        
        return Instance;
    }
}