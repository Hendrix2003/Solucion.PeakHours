using Hangfire;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SolucionPeakHours.Entities;
using SolucionPeakHours.Interfaces;
using SolucionPeakHours.Models;
using SolucionPeakHours.Persistence;
using SolucionPeakHours.Repositories;
using SolucionPeakHours.Services;
using System.Reflection;
using System.Text;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Token security",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[]{}
        }
    });
});

builder.Services.AddDbContext<CndDbContext>(options =>
{
    options.UseSqlServer
        (builder
        .Configuration
        .GetConnectionString("DefaultConnectionString"));
});

builder.Services.AddIdentity<UserIdentityEntity, IdentityRole>()
                .AddEntityFrameworkStores<CndDbContext>();

var tokenValidationParameters = new TokenValidationParameters
{
    ValidateIssuerSigningKey = true,
    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSetting:Key"]!)),
    ValidateIssuer = false,
    ValidateAudience = false,
    ValidateLifetime = true,
    RequireExpirationTime = false,
    ClockSkew = TimeSpan.Zero,
};
builder.Services.AddSingleton(tokenValidationParameters);

builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSetting"));

builder.Services.AddAuthentication(op =>
{
    op.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    op.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    op.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(op =>
{
    op.RequireHttpsMetadata = false;
    op.SaveToken = true;
    op.TokenValidationParameters = tokenValidationParameters;
});



builder.Services.AddScoped(typeof(IBaseCrudRepository<>), typeof(BaseCrudRepository<>));

builder.Services.AddScoped<IUserRepositoy, UserRepositoy>();

builder.Services.AddScoped<IDashboardRepository, DashboardRepository>();

builder.Services.AddScoped<IProgHourRepository, ProgHourRepository>();

builder.Services.AddScoped<IWorkHourRepository, WorkHourRepository>();

builder.Services.AddScoped<IAccountService, AccountService>();

builder.Services.AddScoped<IEmailService, EmailService>();

builder.Services.AddScoped<ITokenService, TokenService>();

builder.Services.AddScoped<IBackgroundTaskService, BackgroundTaskService>();

builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});


builder.Services.AddHangfire(configuration => configuration
        .UseSimpleAssemblyNameTypeSerializer()
        .UseRecommendedSerializerSettings()
        .UseSqlServerStorage(builder.Configuration.GetConnectionString("DefaultConnectionString")));

builder.Services.AddHangfireServer();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");

app.UseHangfireDashboard();

app.UseAuthorization();

app.MapControllers();

app.Run();
