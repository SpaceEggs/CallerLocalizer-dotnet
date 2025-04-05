using Microsoft.Extensions.DependencyInjection;
using PhoneNumberLookup.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 注册手机号码查询服务
builder.Services.AddSingleton<IPhoneNumberService, PhoneNumberService>();

var app = builder.Build();

// 配置 Kestrel 监听所有 IP 地址
//app.Urls.Add("http://*:5000");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// 初始化手机号码查询服务
var phoneNumberService = app.Services.GetRequiredService<IPhoneNumberService>();
await phoneNumberService.InitializeAsync();

app.Run();
