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
            var result = string.Empty;

            if (_contextAccessor.HttpContext is not null)
            {
                result = _contextAccessor.HttpContext.User.FindFirst(ClaimTypes.Email)!.Value;
            }

            return result!;
        }

        public string GetName()
        {
            var result = string.Empty;

            if (_contextAccessor.HttpContext is not null)
            {
                result = _contextAccessor.HttpContext.User.FindFirst(ClaimTypes.Name)!.Value;
            }

            return result!;
        }

        public int GetMyId()
        {
            var result = string.Empty;

            if (_contextAccessor.HttpContext is not null)
            {
                result = _contextAccessor.HttpContext.User.FindFirst(ClaimTypes.UserData)!.Value;
            }

            return Convert.ToInt16(result!);
        }

        public int GetOrganizationId()
        {
            var result = string.Empty;

            if (_contextAccessor.HttpContext is not null)
            {
                result = _contextAccessor.HttpContext.User.FindFirst("OrganizationId")!.Value;
            }

            return Convert.ToInt16(result!);
        }

        public string? GetPhone()
        {
            var result = string.Empty;

            if (_contextAccessor.HttpContext is not null)
            {
                result = _contextAccessor.HttpContext.User.FindFirst(ClaimTypes.MobilePhone)!.Value;
            }

            return result!;
        }

        public IEnumerable<string> GetRoles()
        {
            var result = new List<string>();

            if (_contextAccessor.HttpContext is not null)
            {
                result = _contextAccessor.HttpContext.User.Claims
                    .Where(c => c.Type == ClaimTypes.Role)
                    .Select(c => c.Value)
                    .ToList();
            }

            return result;
        }
    }
}
