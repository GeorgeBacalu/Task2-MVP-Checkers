using Newtonsoft.Json;
using System.IO;

namespace Checkers.Core.Data
{
    public class DataManager
    {
        private readonly string _filePath;

        public DataManager(string filePath) => _filePath = filePath;

        public void SaveData<T>(T data) => File.WriteAllText(_filePath, JsonConvert.SerializeObject(data, Formatting.Indented));

        public T LoadData<T>() => JsonConvert.DeserializeObject<T>(File.ReadAllText(_filePath));
    }
}