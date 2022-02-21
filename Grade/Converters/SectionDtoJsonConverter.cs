using Grade.Helpers;
using Grade.Models;
using Grade.Models.Dto;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Grade.Converters
{
    public class SectionDtoJsonConverter : JsonConverter<SectionDto>
    {
        

        public override bool CanConvert(Type typeToConvert) =>
            typeof(SectionDto) == typeToConvert;

        [Obsolete]
        public override SectionDto? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }

        public override void Write(Utf8JsonWriter writer, SectionDto value, JsonSerializerOptions options) =>
            JsonSerializer.Serialize(writer, value as object, options);

           
        
    }
}
