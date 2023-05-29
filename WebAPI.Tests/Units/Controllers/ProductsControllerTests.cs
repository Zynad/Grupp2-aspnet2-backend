using Moq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Controllers;
using WebAPI.Models.Dtos;
using WebAPI.Models.Interfaces;
using WebAPI.Tests.Fixtures;

namespace WebAPI.Tests.Controllers
{
	//Tobbe testar denna
	public class ProductsControllerTest
	{
		private readonly ProductsController _controller;
		private readonly Mock<IProductService> _mockProductService;

		public ProductsControllerTest()
		{
			_mockProductService = new Mock<IProductService>();
			_controller = new ProductsController(_mockProductService.Object);
		}

		[Fact]
		public async Task AddProduct_ShouldCreateProductAndAddToDatabase_ReturnsCreatedResult()
		{
			// Arrange
			var schema = ProductFixture.Schemas[0];
			_mockProductService.Setup(x => x.CreateAsync(schema)).ReturnsAsync(true);

			// Act
			var result = await _controller.AddProduct(schema);

			// Assert
			Assert.IsType<CreatedResult>(result);
			Assert.Equal(StatusCodes.Status201Created, (result as CreatedResult)?.StatusCode);
		}

		[Fact]
		public async Task AddProduct_ServiceFails_ReturnsBadRequest()
		{
			// Arrange
			var schema = ProductFixture.Schemas[0];
			_mockProductService.Setup(x => x.CreateAsync(schema)).ReturnsAsync(false);

			// Act
			var result = await _controller.AddProduct(schema);

			// Assert
			Assert.IsType<BadRequestObjectResult>(result);
			Assert.Equal(StatusCodes.Status400BadRequest, (result as BadRequestObjectResult)?.StatusCode);
			Assert.Equal("Something went wrong, try again!", (result as BadRequestObjectResult)?.Value);
		}
		
		[Fact]
		public async Task GetById_ShouldFetchProductById_ReturnsOkResult()
		{
			// Arrange
			var id = ProductFixture.DTOs[0].Id;
			var product = ProductFixture.DTOs[0];
			_mockProductService.Setup(x => x.GetByIdAsync(id)).ReturnsAsync(product);

			// Act
			var result = await _controller.GetById(id);

			// Assert
			Assert.IsType<OkObjectResult>(result);
			Assert.Equal(StatusCodes.Status200OK, (result as OkObjectResult)?.StatusCode);
			Assert.Equal(product, (result as OkObjectResult)?.Value);
		}
		
		[Fact]
		public async Task GetById_ProductWithIdNotFoundInDatabase_ReturnsNotFoundResult()
		{
			// Arrange
			var id = ProductFixture.DTOs[0].Id;
			_mockProductService.Setup(x => x.GetByIdAsync(id))!.ReturnsAsync((ProductDTO)null!);

			// Act
			var result = await _controller.GetById(id);

			// Assert
			Assert.IsType<NotFoundObjectResult>(result);
			Assert.Equal(StatusCodes.Status404NotFound, (result as NotFoundObjectResult)?.StatusCode);
			Assert.Equal("No product found", (result as NotFoundObjectResult)?.Value);
		}

	}
}