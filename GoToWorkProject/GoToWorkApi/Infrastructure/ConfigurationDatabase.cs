using GoToWorkContracts.Infrastructure;

namespace GoToWorkApi.Infrastructure;

public class ConfigurationDatabase(IConfiguration configuration) : IConfigurationDatabase
{
    private readonly Lazy<DatabaseSettings> _settings = new(() =>
        configuration.GetValue<DatabaseSettings>("DatabaseSettings")
        ?? throw new InvalidDataException(nameof(DatabaseSettings)));

    public string ConnectionString => _settings.Value.ConnectionString;
}