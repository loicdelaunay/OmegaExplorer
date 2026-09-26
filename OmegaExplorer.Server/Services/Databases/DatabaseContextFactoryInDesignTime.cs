using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace OmegaExplorer.Server.Services.Databases;

public class DatabaseContextFactoryInDesignTime : IDesignTimeDbContextFactory<DatabaseContext>
{
    public DatabaseContext CreateDbContext(string[] args)
    {
        var connectionString = "Filename=database.sqlite";

        var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
        optionsBuilder.UseSqlite(connectionString);

        return new DatabaseContext(optionsBuilder.Options);
    }
}