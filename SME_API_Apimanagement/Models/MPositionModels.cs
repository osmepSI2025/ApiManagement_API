using System.Text.Json.Serialization;

namespace SME_API_Apimanagement.Models
{
    public class MPositionModels
    {
        public string? Code { get; set; }

        public int? Seq { get; set; }

        public string? ProjectCode { get; set; }

        public string? TypeCode { get; set; }

        public string? Module { get; set; }

        public string? Grouping { get; set; }

        public string? Category { get; set; }

        public string? NameTh { get; set; }

        public string? NameEn { get; set; }

        public string? DescriptionTh { get; set; }

        public string? DescriptionEn { get; set; }
    }
    public class ApiListPositionResponse
    {
        [JsonPropertyName("results")]
        public List<MPositionModels?> Results { get; set; }
    }
    public class ApiPositionResponse
    {
        [JsonPropertyName("results")]
        public MPositionModels? Results { get; set; }
    }
}
