using BMS.Repo.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.Repo.Repositories.GenericRepo
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected ApplicationDbContext _context;
        private DbSet<T> dbSet;
        public Repository(ApplicationDbContext context)
        {
            _context = context;
            dbSet = _context.Set<T>();
        }
        public void Add(T entity) => _context.Add(entity);

       

        public async Task<List<T>> GetAll()
        {
            List<T> list = dbSet.ToList();
            return list;
        }

        public void Remove(T entity) => _context.Remove(entity);

        public void Save()
        {
           _context.SaveChanges();
        }
    }
}
