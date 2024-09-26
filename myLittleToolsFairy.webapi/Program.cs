using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using myLittleToolsFairy.WebApi;
using myLittleToolsFairy.WebCore.SwaggerExtend;
using myLittleToolsFairy.WebCore.CorsExtend;
using SqlSugar;
using System.Reflection.Metadata;
using Microsoft.EntityFrameworkCore;
using myLittleToolsFairy.DbModels;
using myLittleToolsFairy.IBusinessServices;
using myLittleToolsFairy.BusinessServices;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

#region Swagger相關

builder.Services.AddSwaggerExt("My Little Tools Fairy Site - Api Document", "Universal Version CoreApi");

#endregion Swagger相關

builder.Services.AddTransient<ISqlSugarClient>(p =>
{
    SqlSugarClient db = new SqlSugarClient(new ConnectionConfig()
    {
        DbType = DbType.SqlServer,
        ConnectionString = builder.Configuration.GetConnectionString("WebToolDB"),
        IsAutoCloseConnection = true
    });
    return db;
});

#region IOC

// 讀取appsetting中的連線字串
string connectionStr = builder.Configuration.GetConnectionString("WebToolDB");

// AddDbContext<DbContext, {這裡放你的DbContext類別名稱(DbModel專案內)} >()
builder.Services.AddDbContext<DbContext, myLittleToolsFairyDbContext>(buildOption =>
{
    // 讀取appsetting中的連線字串，並將該連線字串作為參數傳入以連線資料庫
    buildOption.UseSqlServer(builder.Configuration.GetConnectionString("WebToolDB"));
});

// 指定生命週期是Transient 短暫性，每次請求都會創建全新的實例，且不共享實例
// AddTransient<{接口},{實作}>
builder.Services.AddTransient<IUserService, UserService>();

#endregion IOC

builder.Services.AddCorsExt();

var app = builder.Build();

app.UseCorsExt();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwaggerExt("My Little Tools Fairy Site");
}

app.UseAuthorization();

app.MapControllers();

app.Run();