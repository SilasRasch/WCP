
namespace WCPShared.Models.Entities.ProjectModels.Concepts
{
    public class UgcConcept : CreatorConcept
    {
        public int LengthInSeconds { get; set; } = 15;
        public int ExtraHooks { get; set; }
    }
}