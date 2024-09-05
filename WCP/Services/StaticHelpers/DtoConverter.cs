using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using WCPShared.Models;
using WCPShared.Models.DTOs;
using WCPShared.Models.UserModels;

namespace WCPShared.Services.StaticHelpers
{
    public static class DtoConverter
    {
        public static CreatorDto CreatorToDto(Creator creator)
        {
            var dto = new CreatorDto()
            {
                Address = creator.Address,
                DateOfBirth = creator.DateOfBirth,
                ImgURL = creator.ImgURL,
                IsEditor = creator.IsEditor,
                Speciality = creator.Speciality,
                UserId = creator.UserId
            };

            if (creator.Languages is not null)
                dto.Languages = creator.Languages.Select(x => x.Name).ToList();

            return dto;
        }

        public static UserNC UserToNCUser(User user)
        {
            return new UserNC
            {
                Name = user.Name,
                Email = user.Email,
                Id = user.Id,
                IsActive = user.IsActive,
                Organization = user.Organization,
                Phone = user.Phone,
                Role = user.Role
            };
        }

        public static CreatorUser UserCreatorToCreatorUser(User user, Creator creator)
        {
            return new CreatorUser
            {
                Id = creator.Id,
                Creator = CreatorToDto(creator),
                User = UserToNCUser(user),
            };
        }

        public static OrderDto OrderToDto(Order obj)
        {
            var dto = new OrderDto
            {
                BrandId = obj.BrandId,
                Price = obj.Price,
                Category = 0,
                State = 0,
                Content = obj.Content,
                ContentCount = obj.ContentCount,
                ContentLength = obj.ContentLength,
                Delivery = obj.Delivery,
                DeliveryTimeFrom = obj.DeliveryTimeFrom,
                DeliveryTimeTo = obj.DeliveryTimeTo,
                Email = obj.Email,
                Name = obj.Name,
                Phone = obj.Phone,
                ExtraCreator = obj.ExtraCreator,
                ExtraHook = obj.ExtraHook,
                ExtraNotes = obj.ExtraNotes,
                FocusPoints = obj.FocusPoints,
                Format = obj.Format,
                Ideas = obj.Ideas,
                Platforms = obj.Platforms,
                Products = obj.Products,
                ProjectName = obj.ProjectName,
                ProjectType = obj.ProjectType,
                RelevantFiles = obj.RelevantFiles,
                Scripts = obj.Scripts,
                Other = obj.Other
            };

            return dto;
        }

        public static Order OrderDtoToOrder(OrderDto obj)
        {
            var order = new Order
            {
                BrandId = obj.BrandId,
                Price = obj.Price,
                Category = 0,
                State = 0,
                Content = obj.Content,
                ContentCount = obj.ContentCount,
                ContentLength = obj.ContentLength,
                Delivery = obj.Delivery,
                DeliveryTimeFrom = obj.DeliveryTimeFrom,
                DeliveryTimeTo = obj.DeliveryTimeTo,
                Email = obj.Email,
                Name = obj.Name,
                Phone = obj.Phone,
                ExtraCreator = obj.ExtraCreator,
                ExtraHook = obj.ExtraHook,
                ExtraNotes = obj.ExtraNotes,
                FocusPoints = obj.FocusPoints,
                Format = obj.Format,
                Ideas = obj.Ideas,
                Platforms = obj.Platforms,
                Products = obj.Products,
                ProjectName = obj.ProjectName,
                ProjectType = obj.ProjectType,
                RelevantFiles = obj.RelevantFiles,
                Scripts = obj.Scripts,
                Other = obj.Other
        };

            return order;
        }
    }
}
