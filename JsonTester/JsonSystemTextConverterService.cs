using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Unicode;

namespace JsonTester
{
    public class SystemTextConverter<T> : IDataConverter<T> where T : new()
    {

        public SystemTextConverter()
        {
        }

        public T Deserialize(byte[] data)
        {
            var movies = new T();
            try
            {
                movies = JsonSerializer.Deserialize<T>(data);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while deserializing data. Error: {ex.Message}, InnerException {ex.InnerException?.Message}, Stack {ex.StackTrace}");
            }
            return movies;
        }
    }

    public class GuidConverter : JsonConverter<Guid>
    {
        public override Guid Read(
            ref Utf8JsonReader reader,
            Type typeToConvert,
            JsonSerializerOptions options)
        {
            if (reader.GetString().Length != 36)
                return Guid.ParseExact(reader.GetString(), "N");
            else
                return Guid.ParseExact(reader.GetString(), "D");
        }

        public override void Write(
            Utf8JsonWriter writer,
            Guid id,
            JsonSerializerOptions options) =>
                writer.WriteStringValue(id.ToString(
                    "D"));
    }
}
