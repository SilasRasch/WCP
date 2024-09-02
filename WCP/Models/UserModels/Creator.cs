namespace WCPShared.Models.UserModels
{
    public class Creator
    {
        public int Id { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Address { get; set; } = string.Empty;
        public string? Speciality { get; set; } = string.Empty;
        public string? ImgURL { get; set; }
        public bool IsEditor { get; set; }
        public int UserId { get; set; }
        public User User { get; set; } = new User();
        public List<Language>? Languages { get; set; } = new List<Language>();
        public List<Order> Orders { get; set; } = new List<Order>();

        public bool Validate()
        {
            if (!IsEditor && (string.IsNullOrWhiteSpace(Address) || string.IsNullOrWhiteSpace(Speciality)))
                return false;

            return true;
        }
    }
}
