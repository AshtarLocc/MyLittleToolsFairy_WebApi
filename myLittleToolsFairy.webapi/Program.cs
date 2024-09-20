using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using myLittleToolsFairy.webapi;
using myLittleToolsFairy.webcore.SwaggerExtend;
using SqlSugar;
using System.Reflection.Metadata;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

#region Swagger相關

builder.Services.AddSwaggerExt();

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

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwaggerExt();
    //app.UseSwaggerExt();
}

app.UseAuthorization();

app.MapControllers();

app.Run();