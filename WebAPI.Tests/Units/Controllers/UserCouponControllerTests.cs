using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using WebAPI.Controllers;
using WebAPI.Models.Schemas;
using WebAPI.Models.Interfaces;
using Xunit;
using Microsoft.AspNetCore.Http;
using FluentAssertions;
using WebAPI.Models.Entities;
using WebAPI.Models.Dtos;

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
