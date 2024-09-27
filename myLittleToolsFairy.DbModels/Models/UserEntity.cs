namespace myLittleToolsFairy.DbModels.Models
{
    public class UserEntity
    {
        public int UserId { get; set; }

        public string? Name { get; set; }

        public string? Password { get; set; }

        public int? UserType { get; set; }

        public string? Phone { get; set; }

        public string? Mobile { get; set; }

        public string? Address { get; set; }

        public string? Email { get; set; }

        public int Sex { get; set; }

        public string? ImageUrl { get; set; }

        public DateTime LastLoginTime { get; set; }
    }
}