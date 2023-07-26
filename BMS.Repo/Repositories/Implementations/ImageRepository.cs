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
    public class ImageRepository : Repository<Image>, IImageRepository
    {
        public ImageRepository(ApplicationDbContext context) : base(context)
        {

        }

        public string? GetImage(int postId)
        {
            var img = base._context.Images.FirstOrDefault(x => x.PostId == postId).ImageType;
            if (img == null) return null;
            return img;
        }

        public void RemoveImage(int postId)
        {
            var imageToDelete = base._context.Images.Where(x=>x.PostId == postId).ToList();

            foreach (var imageToDeleteItem in imageToDelete)
            {
                base._context.Images.Remove(imageToDeleteItem);
            }
            base._context.SaveChanges();
        }
    }
}
