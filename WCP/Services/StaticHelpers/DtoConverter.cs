using System.Data;
using WCPShared.Models;
using WCPShared.Models.DTOs;
using WCPShared.Models.UserModels;

namespace WCPShared.Services.StaticHelpers
{
    public static class DtoConverter
    {
        public static Order CloneOrder(Order obj)
        {
            return new Order()
            {
                BrandId = obj.BrandId,
                Price = obj.Price,
                Status = obj.Status,
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
                Other = obj.Other,
                Brand = obj.Brand,
                Creators = new List<Creator>(obj.Creators),
                Id = obj.Id
            };
        }

        public static Order OrderDtoToOrder(OrderDto obj)
        {
            var order = new Order
            {
                BrandId = obj.BrandId,
                Price = obj.Price,
                Status = obj.Status,
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
