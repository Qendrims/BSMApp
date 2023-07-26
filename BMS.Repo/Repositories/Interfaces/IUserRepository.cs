using BMS.Core.Models;
using BMS.Repo.Repositories.GenericRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.Repo.Repositories.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        User GetUserById(string id);

        void UpdateUser(User user);

        User checkEmailExists(string email);

        string GetUserIdByEmail(string email);

    }
}
