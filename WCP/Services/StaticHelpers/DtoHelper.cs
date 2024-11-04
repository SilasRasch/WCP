using WCPShared.Models.DTOs;
using WCPShared.Models.Entities;
using WCPShared.Models.Entities.UserModels;
using WCPShared.Models.Enums;

namespace WCPShared.Services.StaticHelpers
{
    public static class DtoHelper
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
                Id = obj.Id,
                Created = obj.Created,
                Participations = new List<CreatorParticipation>(obj.Participations),
                Editor = obj.Editor,
                EditorId = obj.EditorId,
                StaticTemplates = obj.StaticTemplates,
                Updated = obj.Updated,
                Videographer = obj.Videographer,
                VideographerId = obj.VideographerId 
            };
        }

        public static Order MapProperties(OrderDto input, Order output)
        {
            output.Price = input.Price;
            output.Status = Enum.IsDefined(typeof(ProjectStatus), input.Status) ? (ProjectStatus)input.Status : output.Status;
            output.Content = input.Content;
            output.ContentCount = input.ContentCount;
            output.ContentLength = input.ContentLength;
            output.Delivery = input.Delivery;
            output.DeliveryTimeFrom = input.DeliveryTimeFrom;
            output.DeliveryTimeTo = input.DeliveryTimeTo;
            output.DeliveryDate = input.DeliveryDate;
            output.Email = input.Email;
            output.Name = input.Name;
            output.Phone = input.Phone;
            output.ExtraCreator = input.ExtraCreator;
            output.ExtraHook = input.ExtraHook;
            output.ExtraNotes = input.ExtraNotes;
            output.FocusPoints = input.FocusPoints;
            output.Format = input.Format;
            output.Ideas = input.Ideas.Select(x => new Idea { Text = x }).ToList();
            output.Products = input.Products.Select(x => new Product { Link = x }).ToList();
            output.Platforms = input.Platforms;
            output.ProjectName = input.ProjectName;
            output.ProjectType = (ProjectType)Enum.Parse(typeof(ProjectType), input.ProjectType);
            output.RelevantFiles = input.RelevantFiles;
            output.Scripts = input.Scripts;
            output.Other = input.Other;
            return output;
        }

        public static Order OrderDtoToOrder(OrderDto obj)
        {
            var order = new Order
            {
                BrandId = obj.BrandId,
                Price = obj.Price,
                Status = Enum.IsDefined(typeof(ProjectStatus), obj.Status) ? (ProjectStatus)obj.Status : ProjectStatus.Unconfirmed,
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
                Ideas = obj.Ideas.Select(x => new Idea { Text = x }).ToList(),
                Platforms = obj.Platforms,
                Products = obj.Products.Select(x => new Product { Link = x}).ToList(),
                ProjectName = obj.ProjectName,
                ProjectType = (ProjectType)Enum.Parse(typeof(ProjectType), obj.ProjectType),
                RelevantFiles = obj.RelevantFiles,
                Scripts = obj.Scripts,
                Other = obj.Other,
                EditorId = obj.EditorId,
                VideographerId = obj.VideographerId,
            };

            return order;
        }
    }
}
