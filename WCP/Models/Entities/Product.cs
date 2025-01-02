using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using WCPShared.Models.Entities.ProjectModels;
using WCPShared.Models.Entities.ProjectModels.Concepts;

namespace WCPShared.Models.Entities
{
    public class Product
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Pains { get; set; } = string.Empty;
        public string Features { get; set; } = string.Empty;
        public string FocusPoints { get; set; } = string.Empty;
        public string ImgUrl { get; set; } = string.Empty;
        public string Link { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string HowToUse { get; set; } = string.Empty;
        public long Value { get; set; }

        public int BrandId { get; set; }
        public Brand Brand { get; set; }

        //public ICollection<Project> Projects { get; set; } = [];
        public ICollection<Concept> Concepts { get; set; } = [];
    }
}