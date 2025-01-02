using WCPShared.Models.Entities.UserModels;

namespace WCPShared.Models.Entities.ProjectModels.Concepts
{
    public abstract class CreatorConcept : Concept
    {
        public int CreatorLanguageId { get; set; }
        public Language CreatorLanguage { get; set; }
        public string CreatorGender { get; set; } = string.Empty;
        public int[] CreatorAge { get; set; } = new int[2];
        public long[] CreatorBudget { get; set; } = new long[2];
        public int CreatorCount { get; set; } = 1;
        public int CreativesPerCreator { get; set; } = 1;
        public bool CreatorKeepsProduct { get; set; }
        public IEnumerable<string> Tags { get; set; } = [];
    }
}
