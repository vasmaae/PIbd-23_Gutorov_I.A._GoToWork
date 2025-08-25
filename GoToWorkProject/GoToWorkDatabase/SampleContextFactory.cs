using Microsoft.EntityFrameworkCore.Design;

namespace GoToWorkDatabase;

internal class SampleContextFactory : IDesignTimeDbContextFactory<GoToWorkDbContext>
{
    public GoToWorkDbContext CreateDbContext(string[] args)
    {
        return new GoToWorkDbContext(new DefaultConfigurationDatabase());
    }
}