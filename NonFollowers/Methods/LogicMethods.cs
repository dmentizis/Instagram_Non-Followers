using Newtonsoft.Json;
using System.Diagnostics;

namespace NonFollowers.Methods
{
    public static class LogicMethods
    {
        public static void OpenInBrowser(string? url)
        {
            try
            {
                ProcessStartInfo psi = new()
                {
                    FileName = url,
                    UseShellExecute = true
                };
                Process.Start(psi);
            }
            catch (Exception ex)
            {
                throw new Exception("Could not open URL in browser.", ex);
            }
        }

        public static bool IsNullOrEmpty<T>(List<T>? list)
        {
            return list == null || list.Count == 0;
        }

        public static bool IsNullObject(object? obj)
        {
            return obj == null;
        }

        public static List<T>? LoadJsonToObjectList<T>(string filePath)
        {
            if (!File.Exists(filePath))
                throw new Exception("Invalid File Path");


            using StreamReader r = new(filePath);
            string json = r.ReadToEnd();
            List<T>? items = JsonConvert.DeserializeObject<List<T>>(json);

            return items;
        }

        public static T? LoadJsonToObject<T>(string filePath)
        {
            if (!File.Exists(filePath))
                throw new Exception("Invalid File Path");

            using StreamReader r = new(filePath);
            string json = r.ReadToEnd();
            T? item = JsonConvert.DeserializeObject<T>(json);

            return item;
        }
    }

}
