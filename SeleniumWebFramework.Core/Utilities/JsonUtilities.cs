using Newtonsoft.Json;

namespace SeleniumWebFramework.Core.Utilities;

public class JsonUtils
{
    public static T Deserialize<T>(string json) where T : class
    {
        var data = JsonConvert.DeserializeObject<T>(json);
        return data 
               ?? throw new InvalidOperationException("Deserialization returned null.");
    }

    public static string Serialize<T>(T obj) where T : class
    {
        ArgumentNullException.ThrowIfNull(obj);
        
        return JsonConvert.SerializeObject(obj);
    }
}