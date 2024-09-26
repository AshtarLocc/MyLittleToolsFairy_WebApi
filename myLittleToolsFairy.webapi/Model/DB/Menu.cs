using SqlSugar;

namespace myLittleToolsFairy.WebApi.Model.DB
{
    public class Menu
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public long Id { get; set; }

        [SugarColumn(IsNullable = false)]
        public string Name { get; set; }

        [SugarColumn(IsNullable = false)]
        public string Index { get; set; }

        [SugarColumn(IsNullable = false)]
        public string FilePath { get; set; }

        [SugarColumn(IsNullable = false)]
        public long ParentId { get; set; }

        [SugarColumn(IsNullable = false)]
        public int Order { get; set; }
    }
}