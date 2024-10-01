namespace WCPShared.Models.Entities.AuthModels
{
    public class SelfRegisterDto : RegisterCreatorDto
    {
        public string Password { get; set; } = string.Empty;
        public string VerificationToken { get; set; } = string.Empty;
    }
}
