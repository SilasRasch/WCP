using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using WCPShared.Interfaces.DataServices.Utility;
using WCPShared.Models;
using WCPShared.Models.UserModels;
using WCPShared.Models.Views;

namespace WCPShared.Interfaces.DataServices
{
    public interface IUserService : IDatabaseService<User>, IDtoExtensions<RegisterDto, User>, IObjectViewService<User, UserView>
    {
        Task<User> AddObject(User user);
        Task<User?> GetObjectBy(Expression<Func<User, bool>> predicate);
    }
}
