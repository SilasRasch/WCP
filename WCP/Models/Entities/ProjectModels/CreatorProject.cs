using WCPShared.Models.Entities.UserModels;
using WCPShared.Models.Enums;

namespace WCPShared.Models.Entities.ProjectModels
{
    public abstract class CreatorProject : Project
    {
        public int CreatorLanguageId { get; set; }
        public Language CreatorLanguage { get; set; }
        public string CreatorGender { get; set; } = string.Empty;
        public int[] CreatorAge { get; set; } = new int[2];
        public long[] CreatorBudget { get; set; } = new long[2];
        public int CreatorCount { get; set; } = 1;
        public int CreativesPerCreator { get; set; } = 1;
        public bool CreatorKeepsProduct { get; set; }
        public List<string> Tags { get; set; } = [];

        public List<string> CreatorVisuals { get; set; } = [];
        public List<string> CreatorAudio { get; set; } = [];
        public List<string> Scripts { get; set; } = [];

        public List<CreatorParticipation> Participations { get; set; } = [];

        protected CreatorProject(int id, int brandId, Brand brand, string name, ProjectStatus status, long price, DateTime created, DateTime updated, DateTime deadline, string internalNotes, string platforms, int amount, string formats, int productId, Product product,
                      int creatorLanguageId, Language creatorLanguage, string creatorGender, int[] creatorAge, long[] creatorBudget, int creatorCount, int creativesPerCreator, List<string> tags, List<CreatorParticipation> participations)
        : base(id, brandId, brand, name, status, price, created, updated, deadline, internalNotes, platforms, amount, formats, productId, product)
        {
            CreatorLanguageId = creatorLanguageId;
            CreatorLanguage = creatorLanguage;
            CreatorGender = creatorGender;
            CreatorAge = creatorAge ?? new int[2];
            CreatorBudget = creatorBudget ?? new long[2];
            CreatorCount = creatorCount;
            CreativesPerCreator = creativesPerCreator;
            Tags = tags ?? new List<string>();
            Participations = participations ?? new List<CreatorParticipation>();
        }

        protected CreatorProject(Project existingProject) : base(existingProject)
        {
            if (existingProject is CreatorProject ugcProject)
            {
                CreatorLanguageId = ugcProject.CreatorLanguageId;
                CreatorLanguage = ugcProject.CreatorLanguage;
                CreatorGender = ugcProject.CreatorGender;
                CreatorAge = (int[])ugcProject.CreatorAge.Clone(); // Deep copy for array
                CreatorBudget = (long[])ugcProject.CreatorBudget.Clone(); // Deep copy for array
                CreatorCount = ugcProject.CreatorCount;
                CreativesPerCreator = ugcProject.CreativesPerCreator;
                Tags = new List<string>(ugcProject.Tags); // Deep copy for list
                Participations = new List<CreatorParticipation>(ugcProject.Participations); // Deep copy for list
            }
        }

        protected CreatorProject() : base()
        {
            
        }
    }
}
