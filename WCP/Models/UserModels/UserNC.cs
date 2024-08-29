using WCPShared.Interfaces;

namespace WCPShared.Models.UserModels
{
    /// <summary>
    /// Non-confidential user (no passwords, auth tokens, or information that cannot be shared with the public)
    /// </summary>

    public class UserNC : IUser
    {
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string? Phone { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public Organization? Organization { get; set; }
    }
}
