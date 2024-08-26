using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace WCPShared.Services
{
    public class UserContextService
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public UserContextService(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        public string GetEmail()
        {
            return GetClaimValue(ClaimTypes.Email);
        }

        public string GetName()
        {
            return GetClaimValue(ClaimTypes.Name);
        }

        public int GetId()
        {
            return Convert.ToInt16(GetClaimValue(ClaimTypes.UserData));
        }

        public int GetOrganizationId()
        {
            return Convert.ToInt16(GetClaimValue("OrganizationId"));
        }

        public string? GetPhone()
        {
            return GetClaimValue(ClaimTypes.MobilePhone);
        }

        public IEnumerable<string> GetRoles()
        {
            var result = new List<string>();

            if (_contextAccessor.HttpContext.User.Claims.Any())
            {
                result = _contextAccessor.HttpContext.User.Claims
                    .Where(c => c.Type == ClaimTypes.Role)
                    .Select(c => c.Value)
                    .ToList();
            }

            return result;
        }

        private string GetClaimValue(string claimType)
        {
            string result = null!;

            if (_contextAccessor.HttpContext.User.Claims.Any())
                result = _contextAccessor.HttpContext.User.FindFirst(claimType)!.Value;

            return result!;
        }
    }
}
