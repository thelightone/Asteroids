using System.IO;
using Newtonsoft.Json;
using UnityEngine;

public class JsonConfigLoader
{
    public T Load<T>(string fileName)
    {
        string path = Path.Combine(Application.streamingAssetsPath, "Configs", fileName);
        if (!File.Exists(path))
        {
            throw new FileNotFoundException($"Config file not found: {path}");
        }

        string json = File.ReadAllText(path);
        T config = JsonConvert.DeserializeObject<T>(json);

        if (config == null)
        {
            throw new JsonSerializationException($"Failed to deserialize config: {path}");
        }

        return config;
    }
}
