using LinkSystem.Data;
using LinkSystem.Helpers;
using LinkSystem.Models;
using LinkSystem.Repository;
using LinkSystem.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<MyDataContext>(Options =>
{
    Options.UseSqlServer(builder.Configuration.GetConnectionString("myConn"));
});

builder.Services.AddIdentity<User, IdentityRole>().AddEntityFrameworkStores<MyDataContext>().AddDefaultTokenProviders();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddScoped<IProdcutService, ProdcutRepository>();
builder.Services.AddScoped<IUserService, UserRepository>();

builder.Services.AddAuthorization(option =>
{
    // option.AddPolicy("Admins", policy => policy.RequireRole("Admin"));
    option.AddPolicy("Admins", policy => policy.RequireClaim(CustomCalimTypes.Permission, "Product.CRUD","SetUp.Roles"));
    option.AddPolicy("Mangers", policy => policy.RequireClaim(CustomCalimTypes.Permission, "Product.CRUD"));

});



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
