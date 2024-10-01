using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WCPShared.Models.Entities
{
    public class StaticTemplate
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public string TemplateImgOne { get; set; } = string.Empty;
        public string TemplateImgTwo { get; set; } = string.Empty;
        public string ExampleImg { get; set; } = string.Empty;
        public List<Order> Orders { get; set; } = [];
    }
}
