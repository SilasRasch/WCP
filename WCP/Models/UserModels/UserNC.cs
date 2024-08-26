using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WCPShared.Models.UserModels
{
    /// <summary>
    /// User (Non-confidential - no passwords)
    /// </summary>
   
    public class UserNC
    {
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public string? CVR { get; set; }
        public string Role { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public Organization? Organization { get; set; }
    }
}
