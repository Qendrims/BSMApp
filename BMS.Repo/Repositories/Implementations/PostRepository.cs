using BMS.Core.Models;
using BMS.Repo.Data;
using BMS.Repo.Repositories.GenericRepo;
using BMS.Repo.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.Repo.Repositories.Implementations
{
    public class PostRepository : Repository<Post>, IPostRepository
    {
        public PostRepository(ApplicationDbContext context) : base(context)
        {
        }

        public Post GetPostById(int id)
        {
            return base._context.Posts.FirstOrDefault(p => p.Id == id);
        }

        public async Task<List<Post>> GetSearchPost(string search)
        {
            return await _context.Posts.Where(p => p.Title.Contains(search)).ToListAsync();
        }

        public void UpdatePost(Post post) => _context.Posts.Update(post);
    }
}
