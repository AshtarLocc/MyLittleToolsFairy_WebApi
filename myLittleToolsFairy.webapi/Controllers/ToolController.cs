using Microsoft.AspNetCore.Mvc;
using myLittleToolsFairy.webapi.Model.DB;
using myLittleToolsFairy.webapi.Model.DTO;
using SqlSugar;
using System.Reflection;

namespace myLittleToolsFairy.webapi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ToolController : ControllerBase
    {
        private readonly ILogger<ToolController> _logger;
        private readonly ISqlSugarClient _sqlSugarClient;

        public ToolController(ILogger<ToolController> logger, ISqlSugarClient sqlSugarClient)
        {
            _logger = logger;
            _sqlSugarClient = sqlSugarClient;
        }

        [HttpGet("/api/tool/code-first")]
        public string CodeFirst()
        {
            try
            {
                _sqlSugarClient.DbMaintenance.CreateDatabase();

                var tlist = _sqlSugarClient.DbMaintenance.GetTableInfoList();
                if (tlist != null && tlist.Count > 0)
                {
                    tlist.ForEach(p =>
                    {
                        _sqlSugarClient.DbMaintenance.DropTable(p.Name);
                    });
                }

                // myLittleToolsFairy.webapi.dll 是解決方案的名稱後加上.dll
                // myLittleToolsFairy.webapi.Model 設置初始化資料表時要依循的model路徑
                Type[] ass = Assembly.LoadFrom(AppContext.BaseDirectory + "myLittleToolsFairy.webapi.dll").GetTypes().Where(t => t.Namespace == "myLittleToolsFairy.webapi.Model.DB").ToArray();

                // 資料庫連線並且設置模式為CodeFirst，使用ass的配置來初始化資料表
                _sqlSugarClient.CodeFirst.SetStringDefaultLength(200).InitTables(ass);

                User users = new User()
                {
                    Name = "Admin",
                    Password = "123456"
                };
                _sqlSugarClient.Insertable(users).ExecuteCommand();

                Menu m1 = new Menu()
                {
                    Name = "一級菜單",
                    Index = "/first",
                    FilePath = "../views/first/first.vue",
                    ParentId = 0,
                    Order = 1
                };
                Menu m2 = new Menu()
                {
                    Name = "一級菜單2",
                    Index = "/second",
                    FilePath = "../views/second/second.vue",
                    ParentId = 0,
                    Order = 1
                };

                // 使用Insertable指令將m1寫入資料表，並且使用ExecuteReturnBigIdentity來回傳該筆資料的Id，然後將Id賦值給 m1Id
                // 補充: ExecuteReturnBigIdentity是返回long類型，若是int類型的，需改為使用ExecuteReturnIntIdentity
                long m1Id = _sqlSugarClient.Insertable(m1).ExecuteReturnBigIdentity();
                _sqlSugarClient.Insertable(m2).ExecuteReturnBigIdentity();

                // 創建一個list內部寫入一級菜單下的子菜單(二級菜單)的內容，並將ParentId(父級元素Id)設置為 m1Id
                List<Menu> list = new List<Menu>()
                {
                    new Menu()
                    {
                    Name = "二級菜單",
                    Index = "/first2-1",
                    FilePath = "../views/first/first2-1/first2-1.vue",
                    ParentId = m1Id,
                    Order = 1
                    },
                    new Menu()
                    {
                    Name = "二級菜單2",
                    Index = "/first2-2",
                    FilePath = "../views/first/first2-2/first2-2.vue",
                    ParentId = m1Id,
                    Order = 1
                    },
                };
                _sqlSugarClient.Insertable(list).ExecuteCommand();
                return "OK";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        [HttpGet("/api/tool/tree-menus")]
        public List<TreeMenuDTO> GetTreeMenus()
        {
            // 查詢Menu資料表中的內容，並且將其重組為TreeMenu的格式回傳
            return _sqlSugarClient.Queryable<Menu>().Select(u => new TreeMenuDTO
            {
                Id = u.Id,
                Name = u.Name,
                Index = u.Index,
                FilePath = u.FilePath,
                ParentId = u.ParentId,
                Order = u.Order
            }).ToTree(it => it.Children, it => it.ParentId, 0);

            // ToTree是 SqlSugar 終將平面結構組織為樹狀結構(嵌套)的方法，ToTree(it => it.Children, it => it.ParentId, 0)中，it.Children是一個TreeMenu內部定義的TreeMenu結構的List，是所有子節點依循的的model結構，而it => it.ParentId則代表指定了Menu中的ParentId為父節點，最後設置的0是代表根節點的值，通常根結點會設置為0，代表該筆資料沒有父級物件
        }

        [HttpGet("/api/tool/menus")]
        public List<Menu> GetMenus()
        {
            return _sqlSugarClient.Queryable<Menu>().ToList();
        }

        [HttpGet("/api/tool/user-id")]
        public long GetUserId(string userName, string userPassword)
        {
            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(userPassword))
            {
                return 0;
            }
            var info = _sqlSugarClient.Queryable<User>().Where(p => p.Name == userName && p.Password == userPassword).First();
            if (info == null)
            {
                return 0;
            }
            return info.Id;
        }

        [HttpGet("/api/tool/users")]
        public List<User> GetUsers()
        {
            var data = _sqlSugarClient.Queryable<User>().ToList();
            if (data == null)
            {
                return new List<User> { };
            }
            return data;
        }
    }
}