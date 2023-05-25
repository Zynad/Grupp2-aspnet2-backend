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
    private Mock<IUserCouponService> _mockUserCouponService;
    private UserCouponController _controller;

    public UserCouponControllerTests()
    {
        _mockUserCouponService = new Mock<IUserCouponService>();
        _controller = new UserCouponController(_mockUserCouponService.Object);
    }
}
