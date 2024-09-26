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

#region Swagger����

builder.Services.AddSwaggerExt("My Little Tools Fairy Site - Api Document", "Universal Version CoreApi");

#endregion Swagger����

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

// Ū��appsetting�����s�u�r��
string connectionStr = builder.Configuration.GetConnectionString("WebToolDB");

// AddDbContext<DbContext, {�o�̩�A��DbContext���O�W��(DbModel�M�פ�)} >()
builder.Services.AddDbContext<DbContext, myLittleToolsFairyDbContext>(buildOption =>
{
    // Ū��appsetting�����s�u�r��A�ñN�ӳs�u�r��@���ѼƶǤJ�H�s�u��Ʈw
    buildOption.UseSqlServer(builder.Configuration.GetConnectionString("WebToolDB"));
});

// ���w�ͩR�g���OTransient �u�ȩʡA�C���ШD���|�Ыإ��s����ҡA�B���@�ɹ��
// AddTransient<{���f},{��@}>
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