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
    public async Task AddUserCoupon_ValidSchema_ReturnsOkResult()
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
            .Setup(service => service.CheckDuplicateUserCouponAsync(schema.VoucherCode, userName))
            .ReturnsAsync(new UserCouponEntity());

        _mockUserCouponService
            .Setup(service => service.AddUserCouponAsync(It.IsAny<UserCouponEntity>()))
            .ReturnsAsync(new UserCouponDTO());

        // Act
        var result = await _controller.AddUserCoupon(schema);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
    }

}
