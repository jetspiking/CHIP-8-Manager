using Newtonsoft.Json;
using System;
using System.IO;

namespace CHIP8Manager.Utilities
{
    /// <summary>
    /// Provides a generic implementation for serializing and deserializing to and from JSON.
    /// </summary>
    public static class JSONManager
    {
        public static void SerializeToFile<T>(T obj, String path)
        {
            File.WriteAllText(path, JsonConvert.SerializeObject(obj));
        }

        public static T? DeserializeFromFile<T>(String path)
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(File.ReadAllText(path));
            }
            catch
            {
                return default(T);
            }
        }
    }
}

