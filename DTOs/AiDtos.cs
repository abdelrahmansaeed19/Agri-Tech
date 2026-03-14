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

    public class CropRecommendationRequestDto
    {
        public double Nitrogen { get; set; }
        public double Phosphorous { get; set; }
        public double Potassium { get; set; }
        public double Temperature { get; set; }
        public double Humidity { get; set; }
        public double Ph { get; set; }
        public double Rainfall { get; set; }
    }

    public class CropResponseDto
    {
        public string RecommendedCrop { get; set; }
    }
}
