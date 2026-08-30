using SeleniumWebFramework.Core.Utilities;

namespace SeleniumWebFramework.Core.Models;

public class ConfigurationModel
{
    private static readonly Lazy<ConfigurationModel> _instance =
        new Lazy<ConfigurationModel>(() => ConfigurationLoader.LoadConfiguration());

    /// <summary>
    /// Thread-safe singleton instance of the configuration model.
    /// </summary>
    public static ConfigurationModel Instance => _instance.Value;

    public string BaseUrl { get; set; } = string.Empty;
    public string ApiBaseUrl { get; set; } = string.Empty;
    public string KeyVaultUrl { get; set; } = string.Empty;
    public int ImplicitWait { get; set; }
    public string Browser { get; set; } = string.Empty;
    public bool IsHeadless { get; set; }
    public GridConfigurationOptions? GridConfigurationOptions { get; set; }
    public DriverConfigurationOptions? DriverConfigurationOptions { get; set; }
}