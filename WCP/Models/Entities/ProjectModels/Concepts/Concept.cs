namespace WCPShared.Models.Entities.ProjectModels.Concepts
{
    public abstract class Concept
    {
        public int Id { get; set; }
        public Project Project { get; set; }
        public int ProjectId { get; set; }
        public string Platforms { get; set; } = string.Empty;
        public int Amount { get; set; } = 1;
        public List<string> Formats { get; set; } = [];
        public int ProductId { get; set; }
        public Product Product { get; set; } = new Product();
    }
}
