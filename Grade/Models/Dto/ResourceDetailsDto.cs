using System.Text.Json.Serialization;

namespace Grade.Models.Dto
{
    public class ResourceDetailsDto : ResourceDto
    {
        [JsonPropertyName("id")]
        public int ResourceId => base.Id;
        public DateTime UploadedAt { get; set; }
    }
}
