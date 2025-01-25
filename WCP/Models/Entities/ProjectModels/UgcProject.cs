
namespace WCPShared.Models.Entities.ProjectModels
{
    public class UgcProject : CreatorProject
    {
        #region Project

        public int LengthInSeconds { get; set; } = 15;
        public int ExtraHooks { get; set; }

        #endregion

        //public long[] CalculateUgcPrice()
        //{
        //    long[] priceInterval = new long[2];

        //    if (CreatorBudget.Length == 2)
        //    {
        //        priceInterval[0] = Amount * CreatorBudget[0];
        //        priceInterval[1] = Amount * CreatorBudget[1];

        //        if (ExtraHooks > 0)
        //        {
        //            priceInterval[0] += 200 * Amount;
        //            priceInterval[1] += 200 * Amount;
        //        }
        //    }

        //    if (CreatorBudget.Length == 1)
        //    {
        //        priceInterval[0] = Amount * CreatorBudget[0];

        //        if (ExtraHooks > 0)
        //            priceInterval[0] += 200 * Amount;
        //    }

        //    return priceInterval;
        //}

        public UgcProject(Project existingProject) : base(existingProject)
        {
            if (existingProject is UgcProject ugcProject)
            {
                LengthInSeconds = ugcProject.LengthInSeconds;
                ExtraHooks = ugcProject.ExtraHooks;
                CreatorKeepsProduct = ugcProject.CreatorKeepsProduct;
            }
        }

        public UgcProject() : base()
        {
            
        }
    }
}
