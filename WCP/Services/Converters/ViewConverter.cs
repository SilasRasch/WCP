using WCPShared.Interfaces.DataServices;
using WCPShared.Models;
using WCPShared.Models.UserModels;
using WCPShared.Models.Views;

namespace WCPShared.Services.Converters
{
    public class ViewConverter
    {
        public OrderView Convert(Order obj) 
        {
            OrderView view = new OrderView(obj);
            view.Brand = Convert(obj.Brand);
            view.Creators = obj.Creators.Select(x => Convert(x)).ToList();
            view.StaticTemplates = obj.StaticTemplates.Select(x => Convert(x)).ToList();
            return view;
        }

        public OrganizationView Convert(Organization obj)
        {
            if (obj is null) return null;
            return new OrganizationView(obj);
        }

        public BrandView Convert(Brand obj)
        {
            BrandView view = new BrandView(obj);
            view.Organization = Convert(obj.Organization);
            return view;
        }

        public CreatorView Convert(Creator obj)
        {
            CreatorView view = new CreatorView(obj);
            view.User = Convert(obj.User);
            return view;
        }

        public UserView Convert(User obj)
        {
            return new UserView
            {
                Name = obj.Name,
                Email = obj.Email,
                Id = obj.Id,
                IsActive = obj.IsActive,
                Organization = Convert(obj.Organization!),
                Phone = obj.Phone,
                Role = obj.Role
            };
        }

        public StaticTemplateView Convert(StaticTemplate obj)
        {
            return new StaticTemplateView(obj);
        }
    }
}
