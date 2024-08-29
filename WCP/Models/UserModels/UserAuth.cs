namespace WCPShared.Models.UserModels
{
    public class UserAuth
    {
        public string PasswordHash { get; set; } = string.Empty;
        public string? PasswordResetToken { get; set; } = string.Empty;
        public string? VerificationToken { get; set; } = string.Empty;
        public DateTime? ResetTokenExpiry { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiry { get; set; }
        public int LoginTries { get; set; }
        public DateTime? LastLoginAttempt { get; set; }
    }
}
