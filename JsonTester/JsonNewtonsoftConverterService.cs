using Newtonsoft.Json;
using System;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace JsonTester
{
    public class NewtonsoftConverter<T> : IDataConverter<T> where T : new()
    {

        public NewtonsoftConverter()
        {
        }

        public T Deserialize(byte[] data)
        {
            var movies = new T();
            try
            {
                var stringData = Encoding.Default.GetString(data);
                movies = JsonConvert.DeserializeObject<T>(stringData);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while deserializing data. Error: {ex.Message}, InnerException {ex.InnerException?.Message}, Stack {ex.StackTrace}");
            }
            return movies;
        }
    }

}
