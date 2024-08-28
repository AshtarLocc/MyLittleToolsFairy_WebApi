using SqlSugar;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<ISqlSugarClient>(p =>
{
    SqlSugarClient db = new SqlSugarClient(new ConnectionConfig()
    {
        DbType=DbType.SqlServer,
        ConnectionString=builder.Configuration.GetConnectionString("WebToolDB"),
        IsAutoCloseConnection=true
    });
    return db;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
