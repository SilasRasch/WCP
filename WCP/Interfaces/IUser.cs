using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WCPShared.Models;

namespace WCPShared.Interfaces
{
    public interface IUser
    {
        int Id { get; set; }
        string Email { get; set; }
        string Name { get; set; }
        string? Phone { get; set; }
        string Role { get; set; }
        bool IsActive { get; set; }
        Organization? Organization { get; set; }
    }
}
