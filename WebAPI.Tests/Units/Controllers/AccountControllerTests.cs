using Moq;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Models.Interfaces;
using WebAPI.Controllers;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using WebAPI.Models.Dtos;

namespace WebAPI.Tests.Units.Controllers;

public class AccountControllerTests
{

	private Mock<IAccountService> _accountService;
	private AccountController _accountController;

	public AccountControllerTests()
	{
		_accountService = new Mock<IAccountService>();
		_accountController = new AccountController(_accountService.Object);
	}

	[Fact]
	public async Task GetProfile__ReturnsOKWithUserObject_WhenCorrect()
	{
		// Arrange
		var userName = "DanNov@gmail.com";
		var userDTO = new UserProfileDTO
		{
			FirstName = "Daniel",
			LastName = "Novacic",
			Email = "DanNov@gmail.com"
		};

		var identity = new ClaimsIdentity(new[]
		{
			new Claim(ClaimTypes.Name, userName)
		});

		var principal = new ClaimsPrincipal(identity);

		_accountController.ControllerContext = new ControllerContext
		{
			HttpContext = new DefaultHttpContext
			{
				User = principal
			}
		};


		_accountService.Setup(x => x.GetProfile(userName)).ReturnsAsync(userDTO);


		// Act
		var result = await _accountController.GetProfile();

		// Assert
		Assert.IsType<OkObjectResult>(result);
	}

	[Fact]
	public async Task GetProfile__ReturnsBadRequest_WhenModelFails()
	{
		// Arrange
		_accountController.ModelState.AddModelError("Error", "Model error");

		// Act
		var result = await _accountController.GetProfile();

		// Assert
		Assert.IsType<BadRequestObjectResult>(result);
	}

	[Fact]
	public async Task GetProfile__ReturnsProblem_WhenNoUserExists()
	{
		// Arrange
		
		var userName = "DanNov@gmail.com";

		var identity = new ClaimsIdentity(new[]
		{
			new Claim(ClaimTypes.Name, userName)
		});

		var principal = new ClaimsPrincipal(identity);

		_accountController.ControllerContext = new ControllerContext
		{
			HttpContext = new DefaultHttpContext
			{
				User = principal
			}
		};

		_accountService.Setup(x => x.GetProfile(userName)).ReturnsAsync((UserProfileDTO) null!);

		// Act
		var result = await _accountController.GetProfile();

		// Assert
		Assert.IsType<ObjectResult>(result);

		var objectResult = (ObjectResult) result;
		Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
		var problemDetails = (ProblemDetails) objectResult.Value!;
		Assert.Equal("Model valid, something else is wrong", problemDetails.Detail);
	}
}
