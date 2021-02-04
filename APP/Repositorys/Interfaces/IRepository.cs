using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APP.Repositorys.Interfaces
{
    public interface IRepository<T> where T : class
    {
        void Create(T model);
        void Delete(T model);
        List<T> GetAll();
        Task<List<T>> GetAllAsync();
        T GetById(int? Id);
        Task<T> GetByIdAsync(int? Id);
        void Update(T model);
        void Save();
        Task SaveAsync();
    }
}
