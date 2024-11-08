using Api.App.Features.FileSystem;
using Api.App.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Api.App.Policies;

namespace Api.App.Database;

public class DatabaseContext(DbContextOptions<DatabaseContext> options): DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<Token> Tokens { get; set; }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder
        .UseSqlServer(ConfigApp.Get("db.connection"))
        .UseSeeding((context, _) =>
        {
            var fileSystem = new Manager("profile");
            var policyModerator = new ModerationPolicy();
            var policyPersonal = new AuthenticationPolicy();

            var rules = new List<string>();

            foreach(var r in policyModerator.Rules)
            {
                rules.Add(r);
            }

            foreach(var r in policyPersonal.Rules)
            {
                rules.Add(r);
            }

            var user = new User
            {
                Id = new Guid(),
                Avatar = fileSystem.Url("avatar-m.png"),
                Name = ConfigApp.Get("admin.name"),
                Email = ConfigApp.Get("admin.email"),
                Password = BCrypt.Net.BCrypt.HashPassword(ConfigApp.Get("admin.password")),
                Rules = JsonConvert.SerializeObject(rules),
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                Active = true,
                Confirmed = true,
                ConfirmationCode = "00000"
            };

            context.Set<User>().Add(user);
            context.SaveChanges();
        });
}