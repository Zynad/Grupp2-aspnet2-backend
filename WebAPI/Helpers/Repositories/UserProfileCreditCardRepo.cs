﻿using WebAPI.Contexts;
using WebAPI.Helpers.Repositories.BaseModels;
using WebAPI.Models.Entities;

namespace WebAPI.Helpers.Repositories;

public class UserProfileCreditCardRepo : Repo<UserProfileCreditCardEntity>
{
    public UserProfileCreditCardRepo(DataContext context) : base(context)
    {
    }
}
