using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Security.Claims;
using WebAPI.Controllers;
using WebAPI.Models.Dtos;
using WebAPI.Models.Entities;
using WebAPI.Models.Interfaces;
using WebAPI.Models.Schemas;

namespace WebAPI.Tests.Controllers;

//Alex testar denna
public class UserCouponControllerTests
{
    private readonly UserCouponController _controller;
    private readonly Mock<IUserCouponService> _mockUserCouponService;

    public UserCouponControllerTests()
    {
        _mockUserCouponService = new Mock<IUserCouponService>();
        _controller = new UserCouponController(_mockUserCouponService.Object);
    }

    [Fact]
    public async Task AddUserCoupons_ShouldAddUserCoupon_ReturnOkWithObject()
    {
        // Arrange
        var schema = new UserCouponSchema { VoucherCode = "ABC123" };
        var userName = "testuser";

        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext
            {
                User = new ClaimsPrincipal(new ClaimsIdentity(new[]
                {
                        new Claim(ClaimTypes.Name, userName)
                    }))
            }
        };

        _mockUserCouponService
            .Setup(x => x.CheckDuplicateUserCouponAsync(schema.VoucherCode, userName))
            .ReturnsAsync(new UserCouponEntity());

        _mockUserCouponService
            .Setup(x => x.AddUserCouponAsync(It.IsAny<UserCouponEntity>()))
            .ReturnsAsync(new UserCouponDTO());

        // Act
        var result = await _controller.AddUserCoupon(schema);

        // Assert
        Assert.NotNull(result);
        result.Should().BeOfType<OkObjectResult>();
    }
    [Fact]
    public async Task AddUserCoupons_ShouldInsertAlreadyAddedUserCoupon_ReturnOkWithConflict()
    {
        // Arrange
        var schema = new UserCouponSchema { VoucherCode = "ABC123" };
        var userName = "testuser";

        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext
            {
                User = new ClaimsPrincipal(new ClaimsIdentity(new[]
                {
                        new Claim(ClaimTypes.Name, userName)
                    }))
            }
        };

        _mockUserCouponService
            .Setup(x => x.CheckDuplicateUserCouponAsync(schema.VoucherCode, userName))
            .ReturnsAsync((UserCouponEntity)null!);

        //Act 
        var result = await _controller.AddUserCoupon(schema);

        //assert
        Assert.NotNull(result);
        result.Should().BeOfType<ConflictResult>();

    }
}
