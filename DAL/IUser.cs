using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace angularDotnet.DAL
{
    public interface IUser
    {
        List<UserViewModel> GetUsers();
        void PostUser(UserViewModel model);
        void EditUser(UserViewModel model);
    }
}
