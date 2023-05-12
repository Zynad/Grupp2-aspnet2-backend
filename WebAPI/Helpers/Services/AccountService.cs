using MailKit;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos.Serialization.HybridRow.Schemas;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Security.Claims;
using System.Security.Policy;
using WebAPI.Helpers.Jwt;
using WebAPI.Helpers.Repositories;
using WebAPI.Models.Dtos;
using WebAPI.Models.Email;
using WebAPI.Models.Entities;
using WebAPI.Models.Schemas;

namespace WebAPI.Helpers.Services;

public class AccountService
{
    #region Properties & Constructors
    private readonly UserProfileRepo _userProfileRepo;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly JwtToken _jwt;
    private readonly MailService _mailService;
    private readonly IConfiguration _configuration;

    public AccountService(JwtToken jwt, RoleManager<IdentityRole> roleManager, SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager, UserProfileRepo userProfileRepo, MailService mailService, IConfiguration configuration)
    {
        _jwt = jwt;
        _roleManager = roleManager;
        _signInManager = signInManager;
        _userManager = userManager;
        _userProfileRepo = userProfileRepo;
        _mailService = mailService;
        _configuration = configuration;
    }
    #endregion
    public async Task<bool> RegisterAsync(RegisterAccountSchema schema)
    {
        try
        {
            if(!await _roleManager.Roles.AnyAsync())
            {
                await _roleManager.CreateAsync(new IdentityRole("admin"));
                await _roleManager.CreateAsync(new IdentityRole("user"));
            }
            if(!await _userManager.Users.AnyAsync())
                schema.RoleName = "admin";

            var identityResult = await _userManager.CreateAsync(schema, schema.Password);

            if(identityResult.Succeeded) 
            {
                var identityUser = await _userManager.FindByEmailAsync(schema.Email);
                var roleResult = await _userManager.AddToRoleAsync(identityUser!, schema.RoleName);

                if(roleResult.Succeeded)
                {
                    UserProfileEntity userProfileEntity = schema;
                    userProfileEntity.UserId = identityUser!.Id;
                    await _userProfileRepo.AddAsync(userProfileEntity);

                    var token = await _userManager.GenerateEmailConfirmationTokenAsync(identityUser);
                    var confirmationLink = $"{_configuration.GetSection("Urls").GetValue<string>("ApiUrl")}api/mail/confirmemail?email={WebUtility.UrlEncode(identityUser.Email)}&token={WebUtility.UrlEncode(token)}";
                    var email = new MailData(new List<string> { identityUser.Email! }, "Confirmation link", $"Press {confirmationLink} to confirm your emailaddress");
                    var result = await _mailService.SendAsync(email, new CancellationToken());
                    return true;
                }
            }
        }
        catch { }

        return false;
    }

    public async Task<string> LogInAsync(LoginAccountSchema schema)
    {
        var identityUser = await _userManager.FindByEmailAsync(schema.Email);
        if(identityUser != null)
        {
            var signInResult = await _signInManager.CheckPasswordSignInAsync(identityUser, schema.Password,false);
            if(signInResult.Succeeded)
            {
                var role = await _userManager.GetRolesAsync(identityUser);
                var claimsIdentity = new ClaimsIdentity(new Claim[]
                {
                    new Claim("id", identityUser.Id.ToString()),
                    new Claim(ClaimTypes.Name, identityUser.Email!),
                    new Claim(ClaimTypes.Role, role[0])
                });
                if(schema.RememberMe == false)              
                    return _jwt.GenerateToken(claimsIdentity, DateTime.Now.AddHours(1));

                else
                    return _jwt.GenerateToken(claimsIdentity, DateTime.Now.AddYears(1));


            }
        }
        return string.Empty;
    }

    public async Task LogOutAsync()
    {
        await _signInManager.SignOutAsync();

        
    }
    public async Task<UserProfileDTO> ReturnProfileAsync(string Id)
    {
        var identityUser = await _userManager.FindByIdAsync(Id);
        var profile = await _userProfileRepo.GetAsync(x => x.UserId == Id);
        if (identityUser == null || profile == null)
        {
            return null!;
        }

        var dto = new UserProfileDTO
        {
            FirstName = profile.FirstName,
            LastName = profile.LastName,
            Email = identityUser.Email,
        };

        if (identityUser.PhoneNumber != null)
        {
            dto.PhoneNumber = identityUser.PhoneNumber;
        }

        if (profile.ImageUrl != null)
        {
            dto.ImageUrl = profile.ImageUrl;
        }

        return dto;
    }

    public async Task<UserProfileDTO> UpdateProfileAsync(UpdateUserSchema schema,string userName)
    {
        try
        {
            IdentityUser identityUser = await _userManager.FindByEmailAsync(userName);
            UserProfileEntity userProfile = await _userProfileRepo.GetAsync(x => x.UserId == identityUser.Id);
            if (userProfile == null || identityUser == null)        
                return null!;

            userProfile.FirstName = schema.FirstName;
            userProfile.LastName = schema.LastName;
            identityUser.Email = schema.Email;
            identityUser.UserName = schema.Email;
            if(schema.PhoneNumber != null)
            {
                identityUser.PhoneNumber = schema.PhoneNumber;
            }
            if (schema.ImageUrl != null)
            {
                userProfile.ImageUrl = schema.ImageUrl;
            }

        
            var identityResult = await _userManager.UpdateAsync(identityUser);
            var profileResult = await _userProfileRepo.UpdateAsync(userProfile);

            if (identityResult.Succeeded && profileResult != null)
            {
                var result = await ReturnProfileAsync(profileResult.UserId);
                return result;
            }
        }
        catch { }
        

        return null!;
    }

    public async Task<UserProfileDTO> GetProfile(string userName)
    {
        try
        {
            IdentityUser identityUser = await _userManager.FindByEmailAsync(userName);
            if (identityUser == null)
                return null!;

            return await ReturnProfileAsync(identityUser.Id);

        }
        catch { }

        return null!;
    }
    public async Task<bool> ResetPassword(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);      
        if (user != null)
        {
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var mailLink = $"{_configuration.GetSection("Urls").GetValue<string>("RecoverPasswordUrl")}?email={WebUtility.UrlEncode(user.Email)}&token={WebUtility.UrlEncode(token)}";
            var passwordMail = new MailData(new List<string> { user.Email }, "Reset password", $"Press {mailLink} to reset your password");
            var result = await _mailService.SendAsync(passwordMail, new CancellationToken());
        }
        return false;
    }
    
}
