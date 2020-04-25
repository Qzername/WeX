using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace Database
{
    /// <summary>
    /// Klasa służąca do zczytywania danych typu .json
    /// </summary>
    public static class JSONhandler
    {
        /// <summary>
        /// Służy do zczytywania pliku json. Wyszukiwane jest po id.
        /// </summary>
        /// <param name="file">Który plik ma zostać zczytany</param>
        /// <param name="id">ID obiektu, jeżeli jest jeden to zwróci ten jedyny</param>
        public static object GetElement(JsonFile file, int id)
        {
            if (file == JsonFile.config)
                return JsonConvert.DeserializeObject<Config>(GetFile(file));

            return null;
        }

        /// <summary>
        /// Służy do zczytywania pliku json. Zwraca wszystkie obiekty.
        /// </summary>
        /// <param name="file">Który plik ma zostać zczytany</param>
        public static object[] GetElement(JsonFile file)
        {
            if (file == JsonFile.config)
            {
                List<Config> x = new List<Config>();
                x.Add(JsonConvert.DeserializeObject<Config>(GetFile(file)));
                return x.ToArray();
            }

            return null;
        }

        //Czytanie pliku. Najlepiej nie dotykać bez uzera :p
        static string GetFile(JsonFile file)
        {
            string text = string.Empty;
            string path = string.Empty;

            path = file.ToString() + ".json";

            if (path != null)
                using (StreamReader r = new StreamReader(path))
                    text = r.ReadToEnd();

            return text;
        }
    }

    public enum JsonFile
    {
        config,
        cat
    }
}
