
using WCPShared.Models.Enums;

namespace WCPShared.Models.Entities.ProjectModels
{
    public class StaticProject : Project
    {
        public List<StaticTemplate> StaticTemplates { get; set; } = [];

        public StaticProject(int id, int brandId, Brand brand, string name, ProjectStatus status, long price, DateTime created, DateTime updated, DateTime deadline, string internalNotes, string platforms, int amount, string formats, int productId, Product product, List<StaticTemplate> staticTemplates)
        : base(id, brandId, brand, name, status, price, created, updated, deadline, internalNotes)
        {
            StaticTemplates = staticTemplates ?? new List<StaticTemplate>();
        }

        public StaticProject(Project existingProject) : base(existingProject)
        {
            if (existingProject is StaticProject staticProject)
            {
                // Deep copy of StaticTemplates list to avoid shared references
                StaticTemplates = new List<StaticTemplate>(staticProject.StaticTemplates);
            }
        }

        public StaticProject() : base()
        {
            
        }
    }
}
