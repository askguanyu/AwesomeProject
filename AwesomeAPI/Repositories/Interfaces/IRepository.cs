using System.Collections.Generic;
using System.Threading.Tasks;

namespace AwesomeAPI.Repositories.Interfaces
{
    public interface IRepository<TModel>
    {
        Task<IEnumerable<TModel>> GetAll();
        Task<TModel> GetById(int id);
        void Add(TModel model);
        void Update(TModel model);
        void Remove(TModel model);
        Task<bool> Save();
        Task<bool> Exists(int id);
    }
}