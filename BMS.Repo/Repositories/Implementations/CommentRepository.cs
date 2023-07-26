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
    public class CommentRepository : Repository<Comment>, ICommentRepository
    {

        public CommentRepository(ApplicationDbContext context) : base(context)
        {
        }

        public List<Comment> GetCommentsByPostId(int postId)
        {
            return base._context.Comments.Where(_context => _context.PostId == postId).ToList();
        }
    }
}
