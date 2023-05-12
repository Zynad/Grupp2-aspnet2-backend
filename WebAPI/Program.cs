using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using WebAPI.Contexts;
using WebAPI.Helpers.Services;
using WebAPI.Helpers.Jwt;
using WebAPI.Helpers.Repositories;
using WebAPI.Models.Email;
using WebAPI.Models.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#region Databases
builder.Services.AddDbContext<DataContext>(x => x.UseSqlServer(builder.Configuration.GetConnectionString("DataDB")));
builder.Services.AddDbContext<CosmosContext>(x => x.UseCosmos(builder.Configuration.GetConnectionString("CosmosDB")!, "grupp2-cosmos"));
#endregion

#region EmailConfig
builder.Services.Configure<MailSettings>(builder.Configuration.GetSection(nameof(MailSettings)));
builder.Services.AddScoped<MailService>();
#endregion

#region Helpers
builder.Services.AddScoped<JwtToken>();
builder.Services.AddScoped<AccountService>();
builder.Services.AddScoped<ProductService>();
#endregion

#region Repositories
builder.Services.AddScoped<UserProfileRepo>();
builder.Services.AddScoped<ProductRepo>();
builder.Services.AddScoped<CategoryRepo>();
builder.Services.AddScoped<TagRepo>();
#endregion

#region Identity
builder.Services.AddIdentity<IdentityUser, IdentityRole>(x =>
{
    x.Password.RequiredLength = 8;
    x.SignIn.RequireConfirmedAccount = false;
    x.User.RequireUniqueEmail = true;

}).AddEntityFrameworkStores<DataContext>().AddDefaultTokenProviders();

builder.Services.Configure<DataProtectionTokenProviderOptions>(x =>
{
    x.TokenLifespan = TimeSpan.FromHours(10);
});
#endregion

#region Authentication
builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
    x.Events = new JwtBearerEvents
    {
        OnTokenValidated = context =>
        {
            if (string.IsNullOrEmpty(context?.Principal?.FindFirst("id")?.Value) || string.IsNullOrEmpty(context?.Principal?.Identity?.Name))
                context?.Fail("Unauthorized");

            return Task.CompletedTask;
        }
    };

    x.RequireHttpsMetadata = true;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = builder.Configuration.GetSection("TokenValidation").GetValue<string>("Issuer")!,
        ValidateAudience = true,
        ValidAudience = builder.Configuration.GetSection("TokenValidation").GetValue<string>("Audience")!,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration.GetSection("TokenValidation").GetValue<string>("SecretKey")!))
    };
});
#endregion

var app = builder.Build();
app.UseCors(x => x.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod());
app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
