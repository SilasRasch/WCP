using WCPShared.Interfaces.DataServices;
using WCPShared.Models.Entities;
using WCPShared.Models.Entities.UserModels;
using WCPShared.Models.Views;

namespace WCPShared.Services.Converters
{
    public class ViewConverter
    {
        public OrderView Convert(Order obj) 
        {
            OrderView view = new OrderView(obj);
            view.Brand = Convert(obj.Brand);
            view.Creators = obj.Participations.Select(x => Convert(x.Creator)).ToList();

            if (obj.Videographer is not null)
                view.Videographer = Convert(obj.Videographer);

            if (obj.Editor is not null)
                view.Editor = Convert(obj.Editor);

            if (obj.StaticTemplates is not null)
                view.StaticTemplates = obj.StaticTemplates.Select(x => Convert(x)).ToList();

            return view;
        }

        public OrganizationView Convert(Organization obj)
        {
            if (obj is null) return null!;
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
                Role = obj.Role.ToString(),
            };
        }

        public StaticTemplateView Convert(StaticTemplate obj)
        {
            return new StaticTemplateView(obj);
        }
    }
}
