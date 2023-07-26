using BMS.Core.Models;
using BMS.Repo.Repositories.GenericRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.Repo.Repositories.Interfaces
{
    public interface IImageRepository : IRepository<Image>
    {
        void RemoveImage(int postId);

        string GetImage(int postId);
    }
}
