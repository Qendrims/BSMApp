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
    public class TagRepository : Repository<Tag>, ITagRepository
    {
        public TagRepository(ApplicationDbContext context) : base(context)
        {
        }

        public void TagToRemove(int postId)
        {
            var tagtoDelete = base._context.Tags.Where(t => t.PostId == postId).ToList();

            foreach(var tag in tagtoDelete)
            {
                base._context.Tags.Remove(tag);
            }
            base._context.SaveChanges();
        }

    }
}
