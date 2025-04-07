using Microsoft.EntityFrameworkCore;
using mpit.DataAccess;
using mpit.Extensions;
using mpit.mpit.Application.Interfaces.Auth;
using mpit.mpit.Application.Interfaces.Repositories;
using mpit.mpit.Application.Interfaces.Services;
using mpit.mpit.Application.Services;
using mpit.mpit.DataAccess.DbContexts;
using mpit.mpit.DataAccess.Repositories;
using mpit.mpit.Infastructure.Auth;
using mpit.mpit.Infastructure.Chat.Hubs;
using mpit.mpit.Infastructure.Mapping;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;
var configuration = builder.Configuration;
var environment = builder.Environment;

services.Configure<JwtOptions>(configuration.GetSection(nameof(JwtOptions)));
services.Configure<AuthorizationOptions>(configuration.GetSection(nameof(AuthorizationOptions)));

services.AddSwaggerGen();

services.AddCors(option =>
{
    option.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("https://localhost:3000", "http://localhost:3000");
        policy.AllowCredentials();
        policy.AllowAnyHeader();
        policy.AllowAnyMethod();
    });
});

services.AddMvc();

// Add services to the container.
services.AddScoped<IUsersRepository, UsersRepository>();
services.AddScoped<IRoleRepository, RoleRepository>();

services.AddScoped<IUsersService, UsersService>();
services.AddScoped<IPermissionService, PermissionService>();

services.AddScoped<IJwtProvider, JwtProvider>();
services.AddScoped<IPasswordHasher, PasswordHasher>();
services.AddSignalR();

services.AddAutoMapper(typeof(AppAutoMapperProfile));

services.AddAuthentication(configuration);

services.AddControllers();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
services.AddOpenApi();

services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(configuration.GetConnectionString(nameof(ApplicationDbContext)))
);

var app = builder.Build();

using var scope = app.Services.CreateScope();
await using var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
await dbContext.Database.EnsureCreatedAsync();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}

app.UseStaticFiles();
app.UseHttpsRedirection();
app.UseRouting();

app.MapControllers();

app.UseCors();

app.UseAuthentication();
app.UseAuthorization();

app.MapHub<ChatHub>("/chat");

app.Run();
