using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;


//we write down the common used code in a single location and that common use code is then further used down the line
//for other classes, for other tables.
namespace ePizzaHub.Repositories.Contract
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T> AddAsync(T entity);
        IEnumerable<T> GetAll();
        Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> filter = null);
        Task<int> CommitAsync();
        void Update(T entity);
    }
}
