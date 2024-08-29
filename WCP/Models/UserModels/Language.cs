using WCPShared.Models.UserModels.CreatorModels;

namespace WCPShared.Models.UserModels
{
    public class Language
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<Creator> Speakers { get; set; } = new List<Creator>();
    }
}
