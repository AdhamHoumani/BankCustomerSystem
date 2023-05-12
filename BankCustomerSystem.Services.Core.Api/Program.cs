using AutoMapper;
using BankCustomerSystem.Services.Core.Data.Contracts;
using BankCustomerSystem.Services.Core.Data.Repositories;
using BankCustomerSystem.Services.Core.Service.Interfaces;
using BankCustomerSystem.Services.Core.Service.Interfaces.Clients;
using BankCustomerSystem.Services.Core.Service.Services;
using BankCustomerSystem.Services.Core.Service.Services.Clients;
using BankCustomerSystem.Services.Core.Services.Interfaces;
using BankCustomerSystem.Services.Core.Services.Models.Mapping;
using BankCustomerSystem.Services.Core.Services.Services;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using RepoDb;

var builder = WebApplication.CreateBuilder(args);
var provider = builder.Services.BuildServiceProvider();
var configuration = provider.GetRequiredService<IConfiguration>();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var corsOrigins = configuration.GetSection("CorsOrigins").Get<string[]>();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins(corsOrigins).AllowAnyHeader().AllowAnyMethod();
    });
});

string connectionString = configuration.GetConnectionString("DataBase");
SqlServerBootstrap.Initialize();
GlobalConfiguration
    .Setup()
    .UseSqlServer();

// Repo Dependecies
builder.Services.AddTransient<ICustomerRepository>(s => new CustomerRepository(connectionString));


// Services Dependencies
builder.Services.AddTransient<IRequestInfoService, RequestInfoService>();
builder.Services.AddTransient<ICustomerService, CustomerService>();

// Helpers Dependencies

// Http Client
builder.Services.AddHttpClient<IFinanceClientService, FinanceClientService>().ConfigureHttpClient(client =>
{
    client.BaseAddress = new Uri(configuration.GetConnectionString("FinanceBaseUrl"));
    client.Timeout = TimeSpan.FromMilliseconds(Timeout.Infinite);
});

// Libraries
builder.Services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
var dataProtectionProvider = DataProtectionProvider.Create("BankCustomerSystem.Services.Core.Api");
var protector = dataProtectionProvider.CreateProtector("BankCustomerSystem.Services.Core.Api");
builder.Services.AddSingleton(protector);
var mapperConfig = new MapperConfiguration(mc =>
{
    mc.AddProfile(new MapperProfile(protector));
});
IMapper mapper = mapperConfig.CreateMapper();
builder.Services.AddSingleton(mapper);


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
