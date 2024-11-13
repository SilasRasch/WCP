using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WCPShared.Models.Entities.UserModels;

namespace WCPShared.Models.Entities.ProjectModels
{
    public class PhotoProject : CreatorProject
    {
        #region Product

        public bool CreatorKeepsProduct { get; set; }

        #endregion

        public PhotoProject(Project existingProject) : base(existingProject)
        {
            if (existingProject is PhotoProject photoProject)
            {
                CreatorKeepsProduct = photoProject.CreatorKeepsProduct;
            }
        }

        public PhotoProject() : base()
        {
            
        }
    }
}
