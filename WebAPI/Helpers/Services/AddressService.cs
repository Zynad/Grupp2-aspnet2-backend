using Microsoft.AspNetCore.Identity;
using WebAPI.Helpers.Repositories;
using WebAPI.Models.Dtos;
using WebAPI.Models.Entities;
using WebAPI.Models.Schemas;

namespace WebAPI.Helpers.Services;

public class AddressService
{
    private readonly AddressRepo _addressRepo;
    private readonly UserProfileAddressItemRepo _userProfileAddressItemRepo;
    private readonly AddressItemRepo _addressItemRepo;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly UserProfileRepo _userProfileRepo;

    public AddressService(AddressRepo addressRepo, UserProfileAddressItemRepo userProfileAddressItemRepo, AddressItemRepo addressItemRepo, UserManager<IdentityUser> userManager, UserProfileRepo userProfileRepo)
    {
        _addressRepo = addressRepo;
        _userProfileAddressItemRepo = userProfileAddressItemRepo;
        _addressItemRepo = addressItemRepo;
        _userManager = userManager;
        _userProfileRepo = userProfileRepo;
    }

    public async Task<List<AddressItemDTO>> GetUserAddressesAsync(string userName)
    {
        try
        {
            var user = await _userManager.FindByEmailAsync(userName);

            if(user != null)
            {
                var userProfileAddressItemsList = await _userProfileAddressItemRepo.GetListAsync(x => x.UserProfileId == user.Id);
                List<AddressItemDTO> addressItems = new List<AddressItemDTO>();

                foreach (var item in userProfileAddressItemsList)
                {
                    AddressItemDTO dto = await _addressItemRepo.GetFullAddressItemAsync(x => x.Id == item.AddressItemId);
                    addressItems.Add(dto);
                }

                return addressItems;
            }
        }
        catch { }
        return null!;
    }

    public async Task<bool> RegisterAddressAsync(RegisterAddressSchema schema, string userName)
    {
        try
        {
            var user = await _userManager.FindByEmailAsync(userName);

            if(user != null)
            {
                var userProfile = await _userProfileRepo.GetAsync(x => x.UserId == user.Id);

                var userAddressItemList = await _userProfileAddressItemRepo.GetListAsync(x => x.UserProfileId == user.Id);
                List<AddressItemEntity> addresses = new List<AddressItemEntity>();
                foreach(var item in userAddressItemList)
                {
                    addresses.Add(await _addressItemRepo.GetAsync(x => x.Id == item.AddressItemId));
                }

                var existingTitle = addresses.Where(x => x.Title == schema.Title).FirstOrDefault();

                if (existingTitle != null)
                    return false;

                var result = await _addressRepo.GetAsync(x => x.StreetName.ToLower() == schema.StreetName.ToLower() && x.PostalCode.ToLower() == schema.PostalCode.ToLower() && x.City.ToLower() == schema.City.ToLower() && x.Country.ToLower() == schema.Country.ToLower());

                if (result != null)
                {
                    var addressItem = await _addressItemRepo.AddAsync(new AddressItemEntity { Title = schema.Title, AddressId = result.Id, Address = result });
                    await _userProfileAddressItemRepo.AddAsync(new UserProfileAddressItemEntity { AddressItemId = addressItem.Id, AddressItem = addressItem, UserProfileId = user.Id, UserProfile = userProfile });
                }
                else
                {
                    var address = await _addressRepo.AddAsync(schema);
                    var addressItem = await _addressItemRepo.AddAsync(new AddressItemEntity { Title = schema.Title,  AddressId = address.Id, Address = address });
                    await _userProfileAddressItemRepo.AddAsync(new UserProfileAddressItemEntity { AddressItemId = addressItem.Id, AddressItem = addressItem, UserProfileId = user.Id, UserProfile = userProfile });
                }
                return true;
            }
        }
        catch { }

        return false;
    }
    public async Task<AddressItemEntity> UpdateAddressAsync(UpdateAddressSchema schema, string userName)
    {
        try
        {
            var user = await _userManager.FindByEmailAsync(userName);
            if (user != null)
            {
                var addressItem = await _addressItemRepo.GetAsync(x => x.Id == schema.AddressItemId);
                addressItem.Title = schema.Title;

                var existingAddress = await _addressRepo.GetAsync(x => x.StreetName.ToLower() == schema.StreetName.ToLower() && x.PostalCode.ToLower() == schema.PostalCode.ToLower() && x.City.ToLower() == schema.City.ToLower() && x.Country.ToLower() == schema.Country.ToLower());
                
                if (existingAddress != null)
                {
                    addressItem.AddressId = existingAddress!.Id;
                    addressItem.Address = existingAddress;
                }
                else
                {
                    var newAddress = await _addressRepo.AddAsync(schema);
                    addressItem.AddressId = newAddress.Id;
                    addressItem.Address = newAddress;
                }

               return await _addressItemRepo.UpdateAsync(addressItem);                
            }
        }
        catch {  }

        return null!;
    }

    public async Task<bool> DeleteAddressAsync(int addressItemId)
    {
        try
        {
            var address = await _addressItemRepo.GetAsync(x => x.Id == addressItemId);
            if (address == null)
                return false;

            await _addressItemRepo.DeleteAsync(address); 
            return true;
        }
        catch { }

        return false;
    }

}
