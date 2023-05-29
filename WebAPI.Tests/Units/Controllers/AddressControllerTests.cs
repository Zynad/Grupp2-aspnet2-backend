using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Security.Claims;
using WebAPI.Controllers;
using WebAPI.Models.Dtos;
using WebAPI.Models.Entities;
using WebAPI.Models.Interfaces;

namespace WebAPI.Tests.Units.Controllers;

//Gabriella testar denna
public class AddressControllerTests
{
    private readonly AddressController _addressController;
    private readonly Mock<IAddressService> _addressServiceMock;

    public AddressControllerTests()
    {
        _addressServiceMock = new Mock<IAddressService>();
        _addressController = new AddressController(_addressServiceMock.Object);
    }


    [Fact]
    public async Task GetAllUserAddresses__ShouldGetListOfUsersAddressItems_ReturnOkWithList()
    {
        //Arrange
        var userName = "test@domain.com";

        _addressController.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext
            {
                User = new ClaimsPrincipal(new ClaimsIdentity(new[]
                {
                        new Claim(ClaimTypes.Name, userName)
                }))
            }
        };

        var addresses = new List<AddressItemDTO>()
        {
            new AddressItemDTO{ Id = 1, Title = "Home", Address = new AddressEntity { Id = 1, StreetName = "Ringvägen 3", PostalCode = "12345", City = "Stockholm", Country = "Sweden" } },
            new AddressItemDTO{ Id = 2, Title = "Arbete", Address = new AddressEntity { Id = 2, StreetName = "Kungsgatan 3", PostalCode = "12875", City = "Stockholm", Country = "Sweden" } }
        };

        _addressServiceMock.Setup(x => x.GetUserAddressesAsync(userName)).ReturnsAsync(addresses);


        //Act
        var result = await _addressController.GetUserAddresses();


        //Assert
        Assert.NotNull(result);
        var response = Assert.IsType<OkObjectResult>(result);
        var returnedAddresses = Assert.IsType<List<AddressItemDTO>>(response.Value);
        Assert.Equal(addresses, returnedAddresses);
    }

    [Fact]
    public async Task RemoveAddress__ShouldRemoveAddressItem_ReturnOKWithString()
    {
        //Arrange
        int addressId = 10;
        string okStatusMessage = "Address removed";

        _addressServiceMock.Setup(x => x.DeleteAddressAsync(addressId)).ReturnsAsync(true);


        //Act
        var result = await _addressController.RemoveAddress(addressId);


        //Assert
        var response = Assert.IsType<OkObjectResult>(result);
        var value = Assert.IsType<string>(response.Value);
        Assert.Equal(okStatusMessage, value);
    }
}
