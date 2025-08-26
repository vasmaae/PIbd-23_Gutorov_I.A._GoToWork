using GoToWorkContracts.Infrastructure;

namespace GoToWorkApi.Infrastructure;

public class ConfigurationDatabase(IConfiguration configuration) : IConfigurationDatabase
{
    private readonly Lazy<DatabaseSettings> _settings = new(() =>
        configuration.GetValue<DatabaseSettings>("DatabaseSettings")
        ?? throw new InvalidDataException(nameof(DatabaseSettings)));

    public string ConnectionString => "Host=127.0.0.1;Port=5432;Database=GoToWork;Username=postgres;Password=postgres;";
}