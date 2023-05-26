using Microsoft.AspNetCore.Identity;
using Microsoft.Azure.Cosmos.Serialization.HybridRow;
using MailKit;
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
    private readonly SmsService _smsService;

    public AccountService(JwtToken jwt, RoleManager<IdentityRole> roleManager, SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager, UserProfileRepo userProfileRepo, MailService mailService, IConfiguration configuration, SmsService smsService)
    {
        _jwt = jwt;
        _roleManager = roleManager;
        _signInManager = signInManager;
        _userManager = userManager;
        _userProfileRepo = userProfileRepo;
        _mailService = mailService;
        _configuration = configuration;
        _smsService = smsService;
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

				await _signInManager.SignInAsync(identityUser, isPersistent: schema.RememberMe);

				if (schema.RememberMe == false)
                    return _jwt.GenerateToken(claimsIdentity, DateTime.Now.AddHours(1));

                else
                    return _jwt.GenerateToken(claimsIdentity, DateTime.Now.AddYears(1));
			}
        }
        return string.Empty;
    }

	public async Task<string> LogInExternalAsync(ExternalLoginSchema externalUser)
	{
		// Attempt login with external info to connected local account
		var signInResult = await _signInManager.ExternalLoginSignInAsync(externalUser.LoginProvider, externalUser.ProviderKey, isPersistent: false);
		if (signInResult.Succeeded)
		{
			var user = await _userManager.FindByLoginAsync(externalUser.LoginProvider, externalUser.ProviderKey);
			var role = await _userManager.GetRolesAsync(user!);
			var claimsIdentity = new ClaimsIdentity(new Claim[]
			{
					new Claim("id", user!.Id.ToString()),
					new Claim(ClaimTypes.Name, user.Email!),
					new Claim(ClaimTypes.Role, role[0])
			});

			await _signInManager.SignInAsync(user, isPersistent: false);

			return _jwt.GenerateToken(claimsIdentity, DateTime.Now.AddHours(1));
		}
		else
		{
			// No local account connected, create a new account
			// Extract necessary user information from the external login
			var email = externalUser.Email;

			// Create a new local identity
			var newIdentityUser = new IdentityUser { UserName = email, Email = email };
			var newIdentityUserResult = await _userManager.CreateAsync(newIdentityUser);

			if (newIdentityUserResult.Succeeded)
			{
				UserProfileEntity newUser = new UserProfileEntity
				{
					FirstName = externalUser.FirstName,
					LastName = externalUser.LastName,
					UserId = newIdentityUser!.Id
				};
				await _userProfileRepo.AddAsync(newUser);


				// Add the external login to the new identity
				var addLoginResult = await _userManager.AddLoginAsync(newIdentityUser, externalUser);
				var roleResult = await _userManager.AddToRoleAsync(newIdentityUser!, "user");

				if (addLoginResult.Succeeded)
				{
					// Sign in the user with the newly created identity
					await _signInManager.SignInAsync(newIdentityUser, isPersistent: false);

					// Generate JWT token for the signed-in user
					var role = await _userManager.GetRolesAsync(newIdentityUser);
					var claimsIdentity = new ClaimsIdentity(new Claim[]
					{
						new Claim("id", newIdentityUser.Id.ToString()),
						new Claim(ClaimTypes.Name, newIdentityUser.Email!),
						new Claim(ClaimTypes.Role, role[0])
					});

					return _jwt.GenerateToken(claimsIdentity, DateTime.Now.AddHours(1));
				}
			}

			return string.Empty; // Failed to create and sign in
		}
	}

	public async Task<string> LogInExternalAsyncCopy(ExternalLoginInfo externalUser)
    {
        // Attempt login with external info to connected local account
		var signInResult = await _signInManager.ExternalLoginSignInAsync(externalUser.LoginProvider, externalUser.ProviderKey, isPersistent: false);
		if (signInResult.Succeeded)
		{
			var user = await _userManager.FindByLoginAsync(externalUser.LoginProvider, externalUser.ProviderKey);
			var role = await _userManager.GetRolesAsync(user!);
			var claimsIdentity = new ClaimsIdentity(new Claim[]
			{
					new Claim("id", user!.Id.ToString()),
					new Claim(ClaimTypes.Name, user.Email!),
					new Claim(ClaimTypes.Role, role[0])
			});

			await _signInManager.SignInAsync(user, isPersistent: false);
				
            return _jwt.GenerateToken(claimsIdentity, DateTime.Now.AddHours(1));
		}
        else
        {
            // No local account connected, create a new account
			// Extract necessary user information from the external login
			var email = externalUser.Principal.FindFirstValue(ClaimTypes.Email);

			// Create a new local identity
			var newIdentityUser = new IdentityUser { UserName = email, Email = email };
			var newIdentityUserResult = await _userManager.CreateAsync(newIdentityUser);

			if (newIdentityUserResult.Succeeded)
			{
                // Create new local user entity
                // Principal.Claims array looks different for Google/Facebook
                if (externalUser.LoginProvider == "Google")
                {
                    UserProfileEntity newUser = new UserProfileEntity
                    {
                        FirstName = externalUser.Principal.Claims.ToArray()[2].Value,
                        LastName = externalUser.Principal.Claims.ToArray()[3].Value,
					    UserId = newIdentityUser!.Id
			        };
					await _userProfileRepo.AddAsync(newUser);
				}
                else if (externalUser.LoginProvider == "Facebook")
                {
					UserProfileEntity newUser = new UserProfileEntity
					{
						FirstName = externalUser.Principal.Claims.ToArray()[3].Value,
						LastName = externalUser.Principal.Claims.ToArray()[4].Value,
						UserId = newIdentityUser!.Id
					};
					await _userProfileRepo.AddAsync(newUser);
				}
                else
                {
                    throw new Exception();
                }



				// Add the external login to the new identity
				var addLoginResult = await _userManager.AddLoginAsync(newIdentityUser, externalUser);
				var roleResult = await _userManager.AddToRoleAsync(newIdentityUser!, "user");

				if (addLoginResult.Succeeded)
				{
					// Sign in the user with the newly created identity
					await _signInManager.SignInAsync(newIdentityUser, isPersistent: false);

					// Generate JWT token for the signed-in user
					var role = await _userManager.GetRolesAsync(newIdentityUser);
					var claimsIdentity = new ClaimsIdentity(new Claim[]
					{
					    new Claim("id", newIdentityUser.Id.ToString()),
					    new Claim(ClaimTypes.Name, newIdentityUser.Email!),
					    new Claim(ClaimTypes.Role, role[0])
					});

					return _jwt.GenerateToken(claimsIdentity, DateTime.Now.AddHours(1));
				}
			}

			return string.Empty; // Failed to create and sign in
		}
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
            Email = identityUser.Email!,
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
            var identityUser = await _userManager.FindByEmailAsync(userName);
            UserProfileEntity userProfile = await _userProfileRepo.GetAsync(x => x.UserId == identityUser!.Id);
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
            var identityUser = await _userManager.FindByEmailAsync(userName);
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
            var passwordMail = new MailData(new List<string> { user.Email! }, "Reset password", $"Press {mailLink} to reset your password");
            var result = await _mailService.SendAsync(passwordMail, new CancellationToken());
            if (result)
            {
                return true;
            }
        }
        return false;
    }
    public async Task<bool> ChangePassword(RecoverPasswordSchema schema)
    {
        try
        {
            var user = await _userManager.FindByEmailAsync(schema.Email);
            if (user != null)
            {
                var result = await _userManager.ResetPasswordAsync(user,schema.Token,schema.Password);
                if(result.Succeeded)
                {                   
                    return true;
                }
            }
        }
        catch { } 
        return false;
    }
    public async Task<bool> ChangePassword(ChangePasswordSchema schema,string email)
    {
        try
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                var result = await _userManager.ChangePasswordAsync(user, schema.CurrentPassword, schema.NewPassword);
                if (result.Succeeded)
                {
                    return true;
                }
            }
        } catch { } return false;
        
    }
    public async Task<ConfirmPhoneDTO> ConfirmPhone(string phoneNo,string email)
    {
        var dto = new ConfirmPhoneDTO();
        try
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                dto.Message = "Can't find the user in the database";
                return dto;
            }

            if (user.PhoneNumber != phoneNo)
            {
                dto.Message = "The number you entered don't match the Phone number in our database";
                return dto;
            }

            if (!user.PhoneNumberConfirmed)
            {
                var Code = await _userManager.GenerateChangePhoneNumberTokenAsync(user, phoneNo);
                
                var result = await _smsService.SendSmsAsync(phoneNo,$"Your code is : {Code}");
                if (result)
                {
                    dto.Code = Code;
                    dto.Message = "Success";
                    return dto;
                }
                dto.Message = "Something went wrong with sending the sms, try again later";
                return dto;
            }

            dto.Message = "Your number is already confirmed in our database";
            return dto;
        }
        catch { }
        dto.Message = "Something went wrong";
        return dto;
       
    }
    public async Task<bool> VerifyPhone(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if(user != null)
        {
            user.PhoneNumberConfirmed = true;
            var result = await _userManager.UpdateAsync(user);
            if(result.Succeeded)
            {
                return true;
            }
        }
        return false;
    }
    
}
