﻿using WebAPI.Contexts;
using WebAPI.Helpers.Repositories.BaseModels;
using WebAPI.Models.Entities;

namespace WebAPI.Helpers.Repositories;

public class UserCouponRepo : CosmosRepo<UserCouponEntity>
{
    public UserCouponRepo(CosmosContext context) : base(context)
    {
    }
}
