using Api.App.Features.FileSystem;
using Api.App.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Api.App.Policies;

namespace Api.Database;

public class DatabaseContext(DbContextOptions<DatabaseContext> options): DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<Rule> Rules { get; set; }
    public DbSet<Group> Groups { get; set; }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder
        .UseSqlServer(ConfigApp.Get("db.connection"))
        .UseSeeding((context, _) =>
        {
            var fileSystem = new Manager("profile");
            
            var defaultRules = new []{
                "profile.upload.avatar",
                "profile.update.avatar",
                "profile.update.account",
                "account.confirm",
                "personal.session.logged",
                "personal.session.refresh",
                "reset.password",
                "reset.password",
                "account.email.code"
            };
            var adminRules = new[]
            {
                "admin.moderation.users"
            };
            
            var allRules = adminRules.Concat(defaultRules).ToArray();
            
            var user = new User
            {
                Id = new Guid(),
                Avatar = fileSystem.Url("avatar-m.png"),
                Name = ConfigApp.Get("admin.name"),
                Email = ConfigApp.Get("admin.email"),
                Password = BCrypt.Net.BCrypt.HashPassword(ConfigApp.Get("admin.password")),
                Rules = JsonConvert.SerializeObject(adminRules),
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                Active = true,
                Confirmed = true,
                ConfirmationCode = "00000"
            };

            context.Set<User>().Add(user);
            context.SaveChanges();

            var groupAdmin = new Group
            {
                Name = "admin",
                CreatedAt = DateTime.Now,
                CreatedById = user.Id
            };
            var groupDefault = new Group
            {
                Name = "default",
                CreatedAt = DateTime.Now,
                CreatedById = user.Id
            };
            
            context.Set<Group>().Add(groupDefault);
            context.Set<Group>().Add(groupAdmin);
            context.SaveChanges();

            foreach (var rule in allRules)
            {
                context.Set<Rule>().Add(new Rule
                {
                    Name = rule,
                    CreatedAt = DateTime.Now,
                    CreatedById = user.Id,
                    Description = "functionality created by system",
                });
            }

            context.SaveChanges();
        });
}