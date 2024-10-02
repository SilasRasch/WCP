using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using WCPShared.Interfaces;

namespace WCPShared.Models.Entities.UserModels
{
    public class Language : IWcpEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<Creator> Speakers { get; set; } = new List<Creator>();
    }
}
