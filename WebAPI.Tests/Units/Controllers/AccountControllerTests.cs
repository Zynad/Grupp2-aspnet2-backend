using Moq;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Models.Interfaces;
using WebAPI.Controllers;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using WebAPI.Models.Dtos;
using WebAPI.Models.Schemas;

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




    //UpdateProfileTest

    [Fact]
    public async Task UpdateProfileAsync__ReturnsOk_WhenCorrect()
    {
        // Arrange
        var userName = "jakob.darios@gmail.com";
        var schema = new UpdateUserSchema
        {
            FirstName = "Jakob",
            LastName = "Eliyo",
            Email = "jakob.darios@gmail.com"
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

        //_accountService.Setup(x => x.UpdateProfileAsync(schema,userName)).ReturnsAsync(true);

        // Act
        // När jag debuggar så går den aldrig in i accountservice metoden UpdateProfileAsync och returnerar null direkt så detta test failar
        var result = await _accountController.UpdateProfileAsync(schema);

        // Assert
        Assert.IsType<OkObjectResult>(result);
        var okResult = (OkObjectResult)result;
        Assert.Equal("Update is done", okResult.Value);
    }

    [Fact]
    public async Task UpdateProfileAsync__ReturnsBadRequest_WhenModelFails()
    {
        // Arrange
        _accountController.ModelState.AddModelError("Error", "Model error");

        // Act
        var result = await _accountController.UpdateProfileAsync(new UpdateUserSchema());

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task UpdateProfileAsync__ReturnsProblem_WhenUpdateFails()
    {
        // Arrange
        var userName = "DanNov@gmail.com";
        var schema = new UpdateUserSchema
        {
            FirstName = "UpdatedFirstName",
            LastName = "UpdatedLastName",
            Email = "UpdatedEmail@gmail.com"
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

        //_accountService.Setup(x => x.UpdateProfileAsync(schema, userName)).ReturnsAsync(false);

        // Act
        var result = await _accountController.UpdateProfileAsync(schema);

        // Assert
        Assert.IsType<ObjectResult>(result);
        var objectResult = (ObjectResult)result;
        Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
        var problemDetails = (ProblemDetails)objectResult.Value;
        Assert.Equal("Model valid, something else is wrong", problemDetails.Detail);
    }
}