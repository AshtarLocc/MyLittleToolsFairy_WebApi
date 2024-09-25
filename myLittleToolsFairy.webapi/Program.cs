using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using myLittleToolsFairy.webapi;
using myLittleToolsFairy.webcore.SwaggerExtend;
using myLittleToolsFairy.webcore.CorsExtend;
using SqlSugar;
using System.Reflection.Metadata;

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