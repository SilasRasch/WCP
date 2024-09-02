using WCPShared.Services.StaticHelpers;

namespace WCPShared.Models.DTOs
{
    public class OrganizationDto
    {
        public string Name { get; set; } = string.Empty;
        public string CVR { get; set; } = string.Empty;

        public bool Validate()
        {
            if (CVR is not null && !Validation.ValidateCVR(CVR))
                return false;

            if (!Validation.ValidateDisplayName(Name))
                return false;

            return true;
        }
    }
}
