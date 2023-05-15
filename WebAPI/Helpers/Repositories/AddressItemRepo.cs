using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using WebAPI.Contexts;
using WebAPI.Helpers.Repositories.BaseModels;
using WebAPI.Models.Entities;

namespace WebAPI.Helpers.Repositories;

public class AddressItemRepo : Repo<AddressItemEntity>
{
    private readonly DataContext _context;
    public AddressItemRepo(DataContext context) : base(context)
    {
        _context = context;
    }

    public async Task<AddressItemEntity> GetFullAddressItemAsync(Expression<Func<AddressItemEntity, bool>> predicate)
    {

        var address = await _context.Set<AddressItemEntity>().Include("Address").FirstOrDefaultAsync(predicate);
        if (address != null)
            return address;

        return null!;
    }
}
