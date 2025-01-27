using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using WCPShared.Models.Enums;
using WCPShared.Services.StaticHelpers;
using WCPShared.Interfaces;
using WCPShared.Models.Entities.ProjectModels.Concepts;

namespace WCPShared.Models.Entities.ProjectModels
{
    public class Project : IWcpEntity, IEquatable<Project?>
    {
        #region Base

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int BrandId { get; set; }
        public Brand Brand { get; set; }
        public string Name { get; set; } = string.Empty;
        public ProjectStatus Status { get; set; }
        public long Price { get; set; }
        public ProjectType Type { get; set; }

        // Management
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public DateTime Deadline { get; set; }
        public string InternalNotes { get; set; } = string.Empty;

        // File storage 
        public List<string> Files { get; set; } = [];

        #endregion

        #region Project
        public List<CreatorParticipation> Participations { get; set; } = [];
        public List<Concept> Concepts { get; set; } = [];

        #endregion

        public Project()
        {
            
        }

        protected Project(int id, int brandId, Brand brand, string name, ProjectStatus status, long price, DateTime created, DateTime updated, DateTime deadline, string internalNotes)
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
        public virtual long CalculatePrice()
        {
            var calculatedPrice = 0;
            
            foreach (var concept in Concepts)
            {
                if (concept is UgcConcept ugcConcept)
                {
                    var videoPrice = concept.Amount * 850;
                    calculatedPrice += videoPrice;
                    var hooksPrice = ugcConcept.ExtraHooks * 200;
                    calculatedPrice += hooksPrice;
                }
                else
                {
                    return concept.Amount * 100;
                }
            }

            return calculatedPrice;
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
                   InternalNotes == other.InternalNotes;
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
