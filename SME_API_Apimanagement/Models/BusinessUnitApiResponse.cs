namespace SME_API_Apimanagement.Models
{
    public class BusinessUnitApiResponse
    {
        public List<BusinessUnitModel> Results { get; set; }
    }

    public class BusinessUnitModel
    {
        public string? BusinessUnitId { get; set; }
        public string? BusinessUnitCode { get; set; }
        public int BusinessUnitLevel { get; set; }
        public string? ParentId { get; set; }
        public string? CompanyId { get; set; }
        public DateTime EffectiveDate { get; set; }
        public string? NameTh { get; set; }
        public string? NameEn { get; set; }
        public string? AbbreviationEn { get; set; }
        public string? AbbreviationTh { get; set; }
        public string? DescriptionTh { get; set; }
        public string? DescriptionEn { get; set; }
    }

}
