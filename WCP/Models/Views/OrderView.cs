using WCPShared.Models.DTOs;
using WCPShared.Models.Entities;

namespace WCPShared.Models.Views
{
    public class OrderView
    {
        #region Base

        public int Id { get; set; }
        public double Price { get; set; }
        public DateTime DeliveryDate { get; set; }
        public int DeliveryTimeFrom { get; set; }
        public int DeliveryTimeTo { get; set; }
        public int Status { get; set; }
        public int? VideographerId { get; set; }
        public CreatorView? Videographer { get; set; }
        public int? EditorId { get; set; }
        public CreatorView? Editor { get; set; }
        public List<CreatorView> Creators { get; set; } = [];
        public List<StaticTemplateView> StaticTemplates { get; set; } = [];
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }

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
        public List<string> Ideas { get; set; } = [];
        public List<string> Products { get; set; } = [];

        #endregion


        public OrderView()
        {
        }

        public OrderView(Order obj)
        {
            Id = obj.Id;
            BrandId = obj.BrandId;
            VideographerId = obj.VideographerId;
            EditorId = obj.EditorId;
            Price = obj.Price;
            Status = obj.Status;
            Content = obj.Content;
            ContentCount = obj.ContentCount;
            ContentLength = obj.ContentLength;
            Delivery = obj.Delivery;
            DeliveryDate = obj.DeliveryDate;
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
            Created = obj.Created;
            Updated = obj.Updated;
        }

        public OrderDto ToDto()
        {
            return new OrderDto()
            {
                BrandId = BrandId,
                VideographerId = VideographerId,
                EditorId = EditorId,
                Price = Price,
                Status = Status,
                Content = Content,
                ContentCount = ContentCount,
                ContentLength = ContentLength,
                Delivery = Delivery,
                DeliveryDate = DeliveryDate,
                DeliveryTimeFrom = DeliveryTimeFrom,
                DeliveryTimeTo = DeliveryTimeTo,
                Email = Email,
                Name = Name,
                Phone = Phone,
                ExtraCreator = ExtraCreator,
                ExtraHook = ExtraHook,
                ExtraNotes = ExtraNotes,
                FocusPoints = FocusPoints,
                Format = Format,
                Ideas = Ideas,
                Platforms = Platforms,
                Products = Products,
                ProjectName = ProjectName,
                ProjectType = ProjectType,
                RelevantFiles = RelevantFiles,
                Scripts = Scripts,
                Other = Other,
                Creators = new List<int>(Creators.Select(x => x.Id)),
                StaticTemplates = new List<int>(StaticTemplates.Select(x => x.Id)),
            };
        }
    }
}
