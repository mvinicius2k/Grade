using System.Text.Json;
using System.Text.Json.Serialization;

namespace Grade.Converters
{
    public class TimeOnlyConverter : JsonConverter<TimeOnly>
    {

        private readonly string serializationFormat;

        public TimeOnlyConverter(string serializationFormat)
        {
            
            this.serializationFormat = serializationFormat;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="typeToConvert"></param>
        /// <param name="options"></param>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="FormatException"/>
        /// <returns></returns>
        public override TimeOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var value = reader.GetString();
            return TimeOnly.Parse(value!);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="value"></param>
        /// <param name="options"></param>]
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="ArgumentException"/>
        public override void Write(Utf8JsonWriter writer, TimeOnly value, JsonSerializerOptions options) =>
            writer.WriteStringValue(value.ToString(serializationFormat));

    }
}
