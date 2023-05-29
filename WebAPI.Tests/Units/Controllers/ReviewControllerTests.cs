using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WebAPI.Controllers;
using WebAPI.Models.Dtos;
using WebAPI.Models.Entities;
using WebAPI.Models.Interfaces;
using WebAPI.Models.Schemas;

namespace WebAPI.Tests.Units.Controllers
{
    // <--**--> Robin testar denna <--**-->
    public class ReviewControllerTests
    {
        private readonly ReviewController _reviewController;
        private readonly Mock<IReviewService> _reviewServiceMock;
        private readonly AccountController _accountController;
        private readonly Mock<IAccountService> _accountServiceMock;
        private readonly ProductsController _productsController;
        private readonly Mock<IProductService> _productServiceMock;

        public ReviewControllerTests()
        {
            _reviewServiceMock = new Mock<IReviewService>();
            _reviewController = new ReviewController(_reviewServiceMock.Object); 
        }

        [Fact]
        public async Task GetAll_ReturnResultOkIfReviewsExist()
        {
            // Arrange

            // Act
            var result = await _reviewController.GetAllReviews();

            // Assert
            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result);

        }


        [Fact]
        public async Task Add_CreateReview_ReturnCreatedWhenExistingUserLeavesReview()
        {
            // Arrange
            var userName = "TestRobin@example.com";

            ReviewSchema schema = new ReviewSchema();
            schema.ProductId = new Guid();
            schema.Comment = "Robin Testing";
            schema.Rating = 1;

            _reviewController.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new[]
                  {
                        new Claim(ClaimTypes.Name, userName)
                    }))
                }
            };

            _reviewServiceMock.Setup(x => x.CreateAsync(schema, userName)).ReturnsAsync(true);

            // Act
            var result = await _reviewController.AddReview(schema);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<CreatedResult>(result);
        }
    }
}
