using WebAPI.Models.Dtos;
using WebAPI.Models.Entities;
using WebAPI.Models.Schemas;

namespace WebAPI.Models.Interfaces
{
    public interface IAddressService
    {
        Task<bool> DeleteAddressAsync(int addressItemId);
        Task<List<AddressItemDTO>> GetUserAddressesAsync(string userName);
        Task<bool> RegisterAddressAsync(RegisterAddressSchema schema, string userName);
        Task<AddressItemEntity> UpdateAddressAsync(UpdateAddressSchema schema, string userName);
    }
}