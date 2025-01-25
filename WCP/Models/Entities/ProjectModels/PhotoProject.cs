
namespace WCPShared.Models.Entities.ProjectModels
{
    public class PhotoProject : CreatorProject
    {
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
