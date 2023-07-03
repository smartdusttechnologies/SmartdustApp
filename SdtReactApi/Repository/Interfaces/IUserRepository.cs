using System.Collections.Generic;
using SmartdustApi.Model;
using SmartdustApi.Model;

namespace SmartdustApi.Repository.Interface
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