using myLittleToolsFairy.DbModels;
using myLittleToolsFairy.DbModels.Models;

namespace myLittleToolsFairy.InitDataBase;

internal class Program
{
    private static void Main(string[] args)
    {
        string ConnectionStr = "Server=localhost;Database=myLittleToolsFairyDb;User Id=AshUserAdmin;Password=006240;TrustServerCertificate=True";

        // 透過context初始化資料庫
        using (myLittleToolsFairyDbContext context = new myLittleToolsFairyDbContext(ConnectionStr))
        {
            // 若該資料庫已經存在就刪除
            context.Database.EnsureDeleted();
            // 若該資料庫還未存在就創建
            context.Database.EnsureCreated();

            var addUser = new DbModels.Models.UserEntity()
            {
                Name = "Ash",
                Password = "123",
                UserType = 3,
                Phone = "072222222",
                Mobile = "0966666666",
                Address = "高雄市",
                Email = "kaouuenne@gmail.com",
                Sex = 1,
                ImageUrl = "01.jpg",
                LastLoginTime = DateTime.Now
            };

            context.UserEntities.Add(addUser);
            context.SaveChanges();

            UserEntity user = context.UserEntities.OrderByDescending(x => x.UserId).FirstOrDefault();

            user.Name = "Ashtar";
            context.SaveChanges();

            context.Remove(user);
            context.SaveChanges();
        }
    }
}