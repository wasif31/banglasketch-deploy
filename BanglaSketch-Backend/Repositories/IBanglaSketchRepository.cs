using System.Collections.Generic;
using System.Threading.Tasks;
using Model;
using Model.Users;

namespace Repositories
{
    public interface IBanglaSketchRepository
    {
        void Add<T>(T entity) where T : class;
        void Delete<T>(T entity) where T : class;
        Task<bool> SaveAll();
        Task<IEnumerable<User>> GetUsers();
        Task<User> GetUser(int id);
        Task<Photo> GetPhoto(int id);

    }
}