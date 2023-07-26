using BMS.Core.Models;
using BMS.Repo.Repositories.GenericRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.Repo.Repositories.Interfaces
{
    public interface ICommentRepository : IRepository<Comment>
    {
     List<Comment> GetCommentsByPostId(int postId);

    }
}
