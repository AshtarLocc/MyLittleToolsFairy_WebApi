using SqlSugar;

namespace myLittleToolsFairy.webapi.Model.DTO
{
    public class TreeMenuDTO
    {
        [SugarColumn(IsPrimaryKey = true)]
        public long Id { get; set; }

        public string Name { get; set; }
        public string Index { get; set; }
        public string FilePath { get; set; }
        public long ParentId { get; set; }
        public int Order { get; set; }
        public List<TreeMenuDTO> Children { get; set; }
    }
}