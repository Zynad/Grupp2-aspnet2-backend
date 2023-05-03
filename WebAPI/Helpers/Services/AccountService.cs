using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WebAPI.Helpers.Jwt;
using WebAPI.Helpers.Repositories;
using WebAPI.Models.Entities;
using WebAPI.Models.Schemas;

namespace WebAPI.Helpers.Services;

public class AccountService
{
    private readonly UserProfileRepo _userProfileRepo;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly JwtToken _jwt;

    public AccountService(JwtToken jwt, RoleManager<IdentityRole> roleManager, SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager, UserProfileRepo userProfileRepo)
    {
        _jwt = jwt;
        _roleManager = roleManager;
        _signInManager = signInManager;
        _userManager = userManager;
        _userProfileRepo = userProfileRepo;
    }

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
}
