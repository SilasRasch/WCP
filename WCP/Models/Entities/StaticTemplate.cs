using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WCPShared.Interfaces;
using WCPShared.Models.Entities.ProjectModels;
using WCPShared.Models.Entities.ProjectModels.Concepts;

namespace WCPShared.Models.Entities
{
    public class StaticTemplate : IWcpEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public string TemplateImgOne { get; set; } = string.Empty;
        public string TemplateImgTwo { get; set; } = string.Empty;
        public string ExampleImg { get; set; } = string.Empty;
        public List<StaticConcept> Concepts { get; set; } = [];
        //public List<StaticProject> Projects { get; set; } = [];
    }
}
