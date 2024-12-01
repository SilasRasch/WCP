using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using WCPShared.Models.Enums;
using Microsoft.SqlServer.Server;
using WCPShared.Services.StaticHelpers;
using WCPShared.Interfaces;

namespace WCPShared.Models.Entities.ProjectModels
{
    public abstract class Project : IWcpEntity, IEquatable<Project?>
    {
        #region Base

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int BrandId { get; set; }
        public Brand Brand { get; set; }
        public string Name { get; set; } = string.Empty;
        public ProjectStatus Status { get; set; }
        public long Price { get; set; }

        // Management
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public DateTime Deadline { get; set; }
        public string InternalNotes { get; set; } = string.Empty;

        // File storage 
        public List<string> FinalContent { get; set; }
        public List<string> OtherFiles { get; set; }

        #endregion

        #region Project

        public string Platforms { get; set; } = string.Empty;
        public int Amount { get; set; } = 1;
        public string Formats { get; set; } = string.Empty;

        #endregion

        #region Product

        public int ProductId { get; set; }
        public Product Product { get; set; } = new Product();

        #endregion

        protected Project(int id, int brandId, Brand brand, string name, ProjectStatus status, long price, DateTime created, DateTime updated, DateTime deadline, string internalNotes, string platforms, int amount, string formats, int productId, Product product)
        {
            Id = id;
            BrandId = brandId;
            Brand = brand;
            Name = name;
            Status = status;
            Price = price;
            Created = created;
            Updated = updated;
            Deadline = deadline;
            InternalNotes = internalNotes;
            Platforms = platforms;
            Amount = amount;
            Formats = formats;
            ProductId = productId;
            Product = product;
        }

        protected Project(Project existingProject)
        {
            Id = existingProject.Id;
            BrandId = existingProject.BrandId;
            Brand = existingProject.Brand;
            Name = existingProject.Name;
            Status = existingProject.Status;
            Price = existingProject.Price;
            Created = existingProject.Created;
            Updated = existingProject.Updated;
            Deadline = existingProject.Deadline;
            InternalNotes = existingProject.InternalNotes;
            Platforms = existingProject.Platforms;
            Amount = existingProject.Amount;
            Formats = existingProject.Formats;
            ProductId = existingProject.ProductId;
            Product = existingProject.Product;
        }

        protected Project()
        {
            
        }

        public virtual bool Validate()
        {
            if (!Validation.ValidateDisplayName(Name))
                return false;

            if (!Brand.Validate())
                return false;

            return true;
        }

        /// <summary>
        /// Calculates an estimated price
        /// </summary>
        /// <returns>100 dkk per content amount</returns>
        public virtual long CalculatePrice()
        {
            return Amount * 100;
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as Project);
        }

        public bool Equals(Project? other)
        {
            return other is not null &&
                   Id == other.Id &&
                   BrandId == other.BrandId &&
                   EqualityComparer<Brand>.Default.Equals(Brand, other.Brand) &&
                   Name == other.Name &&
                   Status == other.Status &&
                   Price == other.Price &&
                   Created == other.Created &&
                   Updated == other.Updated &&
                   Deadline == other.Deadline &&
                   InternalNotes == other.InternalNotes &&
                   Platforms == other.Platforms &&
                   Amount == other.Amount &&
                   Formats == other.Formats &&
                   ProductId == other.ProductId &&
                   EqualityComparer<Product>.Default.Equals(Product, other.Product);
        }

        public override int GetHashCode()
        {
            HashCode hash = new HashCode();
            hash.Add(Id);
            hash.Add(BrandId);
            hash.Add(Brand);
            hash.Add(Name);
            hash.Add(Status);
            hash.Add(Price);
            hash.Add(Created);
            hash.Add(Updated);
            hash.Add(Deadline);
            hash.Add(InternalNotes);
            hash.Add(Platforms);
            hash.Add(Amount);
            hash.Add(Formats);
            hash.Add(ProductId);
            hash.Add(Product);
            return hash.ToHashCode();
        }

        public static bool operator ==(Project? left, Project? right)
        {
            return EqualityComparer<Project>.Default.Equals(left, right);
        }

        public static bool operator !=(Project? left, Project? right)
        {
            return !(left == right);
        }
    }
}
