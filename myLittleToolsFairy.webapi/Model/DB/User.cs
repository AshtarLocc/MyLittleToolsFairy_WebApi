using SqlSugar;

namespace myLittleToolsFairy.WebApi.Model.DB
{
    public class User
    {
        //[SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        //public long Id { get; set; }

        //[SugarColumn(IsNullable = false)]
        //public string Name { get; set; }

        //[SugarColumn(IsNullable = false)]
        //public string Password { get; set; }

        public int UserId { get; set; }

        public string? Name { get; set; }

        public string? Password { get; set; }

        public int UserType { get; set; }

        public string? Phone { get; set; }

        public string? Mobile { get; set; }

        public string? Address { get; set; }

        public string? Email { get; set; }

        public int Sex { get; set; }

        public string? ImageUrl { get; set; }

        public DateTime LastLoginTime { get; set; }
    }
}