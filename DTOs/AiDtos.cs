using System.Text.Json.Serialization;


namespace AgriculturalTech.API.DTOs
{
    public class AIResponse
    {
        [JsonPropertyName("class")]
        public string ClassName { get; set; }

        [JsonPropertyName("confidence")]
        public string Confidence { get; set; }

        [JsonPropertyName("class_id")]
        public int ClassId { get; set; }
    }
}
