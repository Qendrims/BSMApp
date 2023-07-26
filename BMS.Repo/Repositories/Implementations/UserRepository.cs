using BMS.Core.Models;
using BMS.Repo.Data;
using BMS.Repo.Repositories.GenericRepo;
using BMS.Repo.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.Repo.Repositories.Implementations
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(ApplicationDbContext context) : base(context)
        {

        }

        public User GetUserById(string id) => base._context.Users.Where(u => u.Id == id).FirstOrDefault();
        
        public void UpdateUser(User user) => base._context.Users.Update(user);

        public User checkEmailExists(string email)
        {
            var userbyEmail = base._context.Users.FirstOrDefault(x=> x.Email == email);
            if(userbyEmail != null)
            {
                return userbyEmail;
            }
            return null;
        }
        public string GetUserIdByEmail(string email)
        {
            return base._context.Users.FirstOrDefault(x=> x.Email == email).Id;
        }

    }
}
