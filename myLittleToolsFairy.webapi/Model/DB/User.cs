using SqlSugar;

namespace myLittleToolsFairy.webapi.Model.DB
{
    public class User
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public long Id { get; set; }

        [SugarColumn(IsNullable = false)]
        public string Name { get; set; }

        [SugarColumn(IsNullable = false)]
        public string Password { get; set; }
    }
}