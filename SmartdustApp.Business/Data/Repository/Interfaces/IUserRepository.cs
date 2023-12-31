﻿using System.Collections.Generic;
using SmartdustApp.Business.Core.Model;

namespace SmartdustApp.Business.Data.Repository.Interfaces
{
    public interface IUserRepository
    {
        List<string> Get();
        UserModel Get(int id);
        UserModel Get(string userName);
        int Insert(UserModel user, PasswordLogin passwordLogin);
        int Update(ChangePasswordModel newpassword);
    }
}