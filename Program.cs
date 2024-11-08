using Api.App.Providers;

namespace Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        
        AppServiceProvider.Bind(builder);
        
        var app = builder.Build();
        
        app.UseCors("Default");
        app.UseHttpsRedirection();
        app.MapControllers();
        app.UseStaticFiles();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseExceptionHandler("/Error");

        app.Run();
    }
}