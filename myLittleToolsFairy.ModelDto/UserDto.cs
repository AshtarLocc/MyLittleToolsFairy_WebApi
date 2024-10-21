using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myLittleToolsFairy.ModelDto
{
    public class UserDto
    {
        public int UserId { get; set; }

        //對應的是UserEntity中的 Name，在Dto中改名為 UserName
        public string? UserName { get; set; }

        public int? UserType { get; set; }

        public string? Phone { get; set; }

        public string? Mobile { get; set; }

        public string? Address { get; set; }

        public string? Email { get; set; }

        public int Sex { get; set; }

        public string? ImageUrl { get; set; }

        public DateTime LastLoginTime { get; set; }

        // //這是一個UserEntity中未包含的欄位，用於後續演示該情況下如何處理而出現的屬性
        public string? IgnoreAtb { get; set; }
    }
}