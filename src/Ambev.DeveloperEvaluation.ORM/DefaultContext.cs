using Ambev.DeveloperEvaluation.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace Ambev.DeveloperEvaluation.ORM;

public class DefaultContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Sale> Sales { get; set; }

    public DefaultContext(DbContextOptions<DefaultContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        modelBuilder.Entity<Sale>().OwnsMany(v => v.Items);
        base.OnModelCreating(modelBuilder);

    }
}
public class YourDbContextFactory : IDesignTimeDbContextFactory<DefaultContext>
{
    public DefaultContext CreateDbContext(string[] args)
    {
        // DEBUG: Exibir argumentos recebidos
        System.Console.WriteLine($"[DbContextFactory] args: {string.Join(", ", args ?? System.Array.Empty<string>())}");

        string? connectionString = null;
        if (args != null && args.Length > 0)
        {
            for (int i = 0; i < args.Length; i++)
            {
                var arg = args[i];
                if (arg.StartsWith("--connection="))
                {
                    connectionString = arg.Substring("--connection=".Length);
                    break;
                }
                else if (arg == "--connection" && i < args.Length - 1)
                {
                    connectionString = args[i + 1];
                    break;
                }
                else if (!arg.StartsWith("--") && string.IsNullOrWhiteSpace(connectionString))
                {
                    // fallback: se vier só a string pura
                    connectionString = arg;
                }
            }
        }
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            var configuration = new Microsoft.Extensions.Configuration.ConfigurationBuilder()
                .SetBasePath(System.IO.Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
            connectionString = configuration.GetConnectionString("DefaultConnection")!;
        }
        // DEBUG: Exibir a string de conexão escolhida
        System.Console.WriteLine($"[DbContextFactory] connectionString: {connectionString}");
        var builder = new Microsoft.EntityFrameworkCore.DbContextOptionsBuilder<DefaultContext>();
        builder.UseNpgsql(
            connectionString,
            b => b.MigrationsAssembly("Ambev.DeveloperEvaluation.ORM")
        );
        return new DefaultContext(builder.Options);
    }
}