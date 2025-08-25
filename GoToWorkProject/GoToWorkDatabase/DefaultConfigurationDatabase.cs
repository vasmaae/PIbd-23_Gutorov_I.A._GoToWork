using GoToWorkContracts.Infrastructure;

namespace GoToWorkDatabase;

internal class DefaultConfigurationDatabase : IConfigurationDatabase
{
    public string ConnectionString => "Host=127.0.0.1;Port=5432;Database=GoToWork;Username=postgres;Password=postgres;";
}