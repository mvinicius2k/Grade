using Grade.Helpers;
using Grade.Models;
using Grade.Models.Dto;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Grade.Converters
{
    public class SectionDtoJsonConverter : JsonConverter<SectionDto>
    {
        

        public override bool CanConvert(Type typeToConvert) =>
            typeof(SectionDto).IsAssignableFrom(typeToConvert);

        //public override SectionDto Read(
        //    ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        //{
        //    if (reader.TokenType != JsonTokenType.StartObject)
        //    {
        //        throw new JsonException();
        //    }

        //    reader.Read();
        //    if (reader.TokenType != JsonTokenType.PropertyName)
        //    {
        //        throw new JsonException();
        //    }

        //    string? propertyName = reader.GetString();
        //    if (propertyName != "TypeDiscriminator")
        //    {
        //        throw new JsonException();
        //    }

        //    reader.Read();
        //    if (reader.TokenType != JsonTokenType.Number)
        //    {
        //        throw new JsonException();
        //    }

        //    TypeDiscriminator typeDiscriminator = (TypeDiscriminator)reader.GetInt32();
        //    SectionDto person = typeDiscriminator switch
        //    {
        //        TypeDiscriminator.Weekly => new WeeklySectionDetailsDto(),
        //        TypeDiscriminator.Loose => new LooseSectionDetailsDto(),
        //        _ => throw new JsonException()
        //    };

        //    while (reader.Read())
        //    {
        //        if (reader.TokenType == JsonTokenType.EndObject)
        //        {
        //            return person;
        //        }

        //        if (reader.TokenType == JsonTokenType.PropertyName)
        //        {
        //            propertyName = reader.GetString();
        //            reader.Read();
        //            switch (propertyName)
        //            {
        //                case "CreditLimit":
        //                    decimal creditLimit = reader.GetDecimal();
        //                    ((Customer)person).CreditLimit = creditLimit;
        //                    break;
        //                case "OfficeNumber":
        //                    string? officeNumber = reader.GetString();
        //                    ((Employee)person).OfficeNumber = officeNumber;
        //                    break;
        //                case "Name":
        //                    string? name = reader.GetString();
        //                    person.Name = name;
        //                    break;
        //            }
        //        }
        //    }

        //    throw new JsonException();
        //}

        public override SectionDto? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            
            if (JsonDocument.TryParseValue(ref reader, out var doc))
            {
                if (doc.RootElement.TryGetProperty("type", out var type))
                {
                    var typeValue = type.GetString();
                    var rootElement = doc.RootElement.GetRawText();

                    return typeValue switch
                    {
                        nameof(ESectionDtoDetailsType.Weekly) => JsonSerializer.Deserialize<WeeklySectionDetailsDto>(rootElement, options),
                        nameof(ESectionDtoDetailsType.Loose) => JsonSerializer.Deserialize<LooseSectionDetailsDto>(rootElement, options),
                        _ => throw new JsonException($"{typeValue} has not been mapped to a custom type yet!")
                    };
                }

                throw new JsonException("Failed to extract type property, it might be missing?");
            }

            throw new JsonException("Failed to parse JsonDocument");
        }

        [Obsolete("Não implementado")]
#pragma warning disable CS0809 // O membro obsoleto substitui o membro não obsoleto
        public override void Write(Utf8JsonWriter writer, SectionDto value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
#pragma warning restore CS0809 // O membro obsoleto substitui o membro não obsoleto
    }
}
