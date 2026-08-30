using Azure;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using dotenv.net;

namespace SeleniumWebFramework.Core.Utilities;

public class KeyVaultReader
{ 
    private readonly SecretClient _secretClient;

    static KeyVaultReader()
    {
        DotEnv.Load(options: new DotEnvOptions(
            ignoreExceptions: true, // Prevents crashing if .env is missing
            probeForEnv: true,      // Searches parent directories for .env
            probeLevelsToSearch: 6  // How many levels up to search for .env
        ));
    }

    public KeyVaultReader(string vaultUrl)
    {
        if (string.IsNullOrWhiteSpace(vaultUrl))
        {
            throw new ArgumentNullException(nameof(vaultUrl), "Vault URL cannot be null or empty.");
        }

        _secretClient = new SecretClient(new Uri(vaultUrl), new DefaultAzureCredential());
    }

    /// <summary>
    /// Method to return secret value
    /// </summary>
    /// <param name="secretName">The key in Azure</param>
    public string GetSecretValue(string secretName)
    {
        try
        {
            KeyVaultSecret secret = _secretClient.GetSecret(secretName);
            return secret.Value;
        }
        catch (RequestFailedException e)
        {
            throw new Exception($"Azure Key Vault failed to find secret: [{secretName}]", e);
        }
        catch (Exception e)
        {
            throw new Exception($"An unexpected error occurred while fetching secret: [{secretName}]", e);
        }
    }
}