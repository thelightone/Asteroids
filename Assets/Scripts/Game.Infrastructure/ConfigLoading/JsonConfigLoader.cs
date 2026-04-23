using System.IO;
using Newtonsoft.Json;
using UnityEngine;

public class JsonConfigLoader
{
    public T Load<T>(string fileName)
    {
        string path = Path.Combine(Application.streamingAssetsPath, "Configs", fileName);
        string json = File.ReadAllText(path);
        return JsonConvert.DeserializeObject<T>(json);
    }
}