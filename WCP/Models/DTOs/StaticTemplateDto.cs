using WCPShared.Services.StaticHelpers;

namespace WCPShared.Models.DTOs
{
    public class StaticTemplateDto
    {
        public string Name { get; set; } = string.Empty;
        public string TemplateImgOne { get; set; } = string.Empty;
        public string TemplateImgTwo { get; set; } = string.Empty;
        public string ExampleImg { get; set; } = string.Empty;

        public bool Validate()
        {
            if (!Validation.ValidateDisplayName(Name))
                return false;

            if (String.IsNullOrWhiteSpace(TemplateImgOne) || String.IsNullOrWhiteSpace(TemplateImgTwo) || String.IsNullOrWhiteSpace(ExampleImg))
                return false;

            return true;
        }
    }
}
