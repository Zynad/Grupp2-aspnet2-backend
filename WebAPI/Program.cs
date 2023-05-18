using Azure.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using WebAPI.Contexts;
using WebAPI.Helpers.Jwt;
using WebAPI.Helpers.Repositories;
using WebAPI.Helpers.Services;
using WebAPI.Models.Email;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Configuration.AddAzureKeyVault(new Uri($"{builder.Configuration["VaultUri"]}"),new DefaultAzureCredential());

#region Databases
builder.Services.AddDbContext<DataContext>(x => x.UseSqlServer(builder.Configuration["DataDB"]));
builder.Services.AddDbContext<CosmosContext>(x => x.UseCosmos(builder.Configuration["CosmosDB"]!, "grupp2-cosmos"));
#endregion

#region EmailConfig
builder.Services.Configure<MailSettings>(builder.Configuration.GetSection(nameof(MailSettings)));
builder.Services.AddScoped<MailService>();
#endregion

#region SmsConfig
builder.Services.AddScoped<SmsService>();
#endregion
#region Helpers
builder.Services.AddScoped<JwtToken>();
builder.Services.AddScoped<AccountService>();
builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<AddressService>();
builder.Services.AddScoped<CategoryService>();
builder.Services.AddScoped<TagService>();
builder.Services.AddScoped<ReviewService>();
builder.Services.AddScoped<PaymentService>();
#endregion

#region Repositories
builder.Services.AddScoped<UserProfileRepo>();
builder.Services.AddScoped<ProductRepo>();
builder.Services.AddScoped<CategoryRepo>();
builder.Services.AddScoped<TagRepo>();
builder.Services.AddScoped<AddressRepo>();
builder.Services.AddScoped<AddressItemRepo>();
builder.Services.AddScoped<UserProfileAddressItemRepo>();
builder.Services.AddScoped<ReviewRepo>();
builder.Services.AddScoped<CreditCardRepo>();
builder.Services.AddScoped<UserProfileCreditCardRepo>();
#endregion

#region Identity
builder.Services.AddIdentity<IdentityUser, IdentityRole>(x =>
{
    x.Password.RequiredLength = 8;
    x.SignIn.RequireConfirmedAccount = false;
    x.User.RequireUniqueEmail = true;
})
.AddEntityFrameworkStores<DataContext>()
.AddDefaultTokenProviders();


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
        ValidIssuer = builder.Configuration["TokenIssuer"],
        ValidateAudience = true,
        ValidAudience = builder.Configuration["TokenAudience"],
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["TokenSecretKey"]!))
    };
});
#endregion

#region External Auth

builder.Services.AddAuthentication()
    .AddFacebook(x =>
    {
        x.ClientId = builder.Configuration["FacebookClientId"]!;
        x.ClientSecret = builder.Configuration["FacebookClientSecret"]!;
    })
    .AddGoogle(x =>
    {
        x.ClientId = builder.Configuration["GoogleClientId"]!;
        x.ClientSecret = builder.Configuration["GoogleClientSecret"]!;
    });


#endregion

var app = builder.Build();
app.UseCors(x => x.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod());
app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
