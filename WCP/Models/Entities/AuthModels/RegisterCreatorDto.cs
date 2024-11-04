using WCPShared.Models.DTOs;

namespace WCPShared.Models.Entities.AuthModels
{
    public class RegisterCreatorDto
    {
        public RegisterDto User { get; set; } = new();
        public CreatorDto? Creator { get; set; } = new();
    }
}
