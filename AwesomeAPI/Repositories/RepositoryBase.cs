using System.Collections.Generic;
using System.Threading.Tasks;
using AwesomeAPI.DatabaseContext;
using AwesomeAPI.Models;
using AwesomeAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AwesomeAPI.Repositories
{
    public abstract class RepositoryBase<TModel> : IRepository<TModel>
        where TModel : ModelBase
    {
        readonly ApiDbContext _dbContext;

        public RepositoryBase(ApiDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<TModel>> GetAll()
        {
            return await _dbContext.Set<TModel>().ToListAsync();
        }

        public async Task<TModel> GetById(int id)
        {
            return await _dbContext.Set<TModel>().FirstOrDefaultAsync(m => m.Id == id);
        }

        public void Add(TModel model)
        {
            _dbContext.Set<TModel>().Add(model);
        }

        public void Update(TModel model)
        {
            _dbContext.Set<TModel>().Update(model);
        }

        public void Remove(TModel model)
        {
            _dbContext.Set<TModel>().Remove(model);
        }

        public async Task<bool> Save()
        {
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> Exists(int id)
        {
            // Use AsNoTracking() to avoid exception when using Update(TModel model) afterwards
            return await _dbContext.Set<TModel>().AsNoTracking().FirstOrDefaultAsync(m => m.Id == id) != null;
        }
    }
}