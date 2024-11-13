using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WCPShared.Models.Entities.UserModels;
using WCPShared.Models.Enums;

namespace WCPShared.Models.Entities.ProjectModels
{
    public class StaticProject : Project
    {
        public List<StaticTemplate> StaticTemplates { get; set; } = [];

        public StaticProject(int id, int brandId, Brand brand, string name, ProjectStatus status, long price, DateTime created, DateTime updated, DateTime deadline, string internalNotes, string platforms, int amount, string formats, int productId, Product product, List<StaticTemplate> staticTemplates)
        : base(id, brandId, brand, name, status, price, created, updated, deadline, internalNotes, platforms, amount, formats, productId, product)
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
