using WCPShared.Services.StaticHelpers;

namespace WCPShared.Models.DTOs
{
    public class OrganizationDto
    {
        public string Name { get; set; } = string.Empty;
        public string CVR { get; set; } = string.Empty;
        public int LanguageId { get; set; }

        public bool Validate()
        {
            if (CVR is not null && !Validation.ValidateCVR(CVR))
                return false;

            if (!Validation.ValidateDisplayName(Name))
                return false;

            if (LanguageId == default)
                return false;

            return true;
        }
    }
}
