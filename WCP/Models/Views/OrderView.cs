using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WCPShared.Models.UserModels;
using WCPShared.Services.StaticHelpers;

namespace WCPShared.Models.Views
{
    public class OrderView
    {
        #region Base

        public int Id { get; set; }
        public double Price { get; set; }
        public int DeliveryTimeFrom { get; set; }
        public int DeliveryTimeTo { get; set; }
        public int Status { get; set; }
        public CreatorView? Videographer { get; set; }
        public CreatorView? Editor { get; set; }
        public List<CreatorView> Creators { get; set; } = new List<CreatorView>();
        public List<StaticTemplateView> StaticTemplates { get; set; } = new List<StaticTemplateView>();
        public Dictionary<int, bool> CreatorDeliveryStatus { get; set; } = new Dictionary<int, bool>();

        // Drive-links
        public string Scripts { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty; // Dump for creator
        public string Delivery { get; set; } = string.Empty; // Finished product
        public string Other { get; set; } = string.Empty;

        #endregion

        #region Page one

        public int BrandId { get; set; }
        public BrandView Brand { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;

        #endregion

        #region Page two

        public string ProjectName { get; set; } = string.Empty;
        public string ProjectType { get; set; } = string.Empty;
        public int ContentCount { get; set; }
        public int? ContentLength { get; set; }
        public string Platforms { get; set; } = string.Empty;
        public string Format { get; set; } = string.Empty;

        #endregion

        #region Page three

        public bool? ExtraCreator { get; set; }
        public int? ExtraHook { get; set; }
        public string? ExtraNotes { get; set; }
        public string? FocusPoints { get; set; }
        public string? RelevantFiles { get; set; }
        public List<string> Ideas { get; set; } = new List<string>();
        public List<string> Products { get; set; } = new List<string>();

        #endregion

        public OrderView(Order obj)
        {
            Id = obj.Id;
            BrandId = obj.BrandId;
            Price = obj.Price;
            Status = obj.Status;
            Content = obj.Content;
            ContentCount = obj.ContentCount;
            ContentLength = obj.ContentLength;
            Delivery = obj.Delivery;
            DeliveryTimeFrom = obj.DeliveryTimeFrom;
            DeliveryTimeTo = obj.DeliveryTimeTo;
            Email = obj.Email;
            Name = obj.Name;
            Phone = obj.Phone;
            ExtraCreator = obj.ExtraCreator;
            ExtraHook = obj.ExtraHook;
            ExtraNotes = obj.ExtraNotes;
            FocusPoints = obj.FocusPoints;
            Format = obj.Format;
            Ideas = obj.Ideas;
            Platforms = obj.Platforms;
            Products = obj.Products;
            ProjectName = obj.ProjectName;
            ProjectType = obj.ProjectType;
            RelevantFiles = obj.RelevantFiles;
            Scripts = obj.Scripts;
            Other = obj.Other;
            
        }
    }
}
