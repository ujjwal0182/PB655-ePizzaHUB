using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ePizzaHub.Infrastructure.Models;
using ePizzaHub.Repositories.Contract;

namespace ePizzaHub.Repositories.Concrete
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        // Now this generic repo will interact the db my case its PB655Context
        protected PB655Context _dbcontext;

        public GenericRepository(PB655Context dbcontext)
        {
            _dbcontext = dbcontext;
        }

        //public void Add(T entity) //void and async does not go simultaneously
        //{
        //    _dbcontext.Set<T>().Add(entity); //This means that i am free any of the models. Lets say if i want to insert record into my User table,
        //                                     //I will create a instance of a generic repository with this User.cs and that will insert into my user table 
        //                                     //itself, so this is a generic way of writing down a quote -> _dbcontext.set changes then identity.
        //}

        public async Task<T> AddAsync(T entity) 
        {
            await _dbcontext.Set<T>().AddAsync(entity);
            return entity;
        }

        //public int commit()
        //{
        //    return _dbcontext.SaveChanges();
        //}

        //we can also commit by using the async 
        public async Task<int> CommitAsync()
        {
            return await _dbcontext.SaveChangesAsync();
        }

        public IEnumerable<T> GetAll()
        {
            IQueryable<T> query = _dbcontext.Set<T>();
            return query.ToList();
        }
    }
}
