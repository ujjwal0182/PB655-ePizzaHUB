using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ePizzaHub.Infrastructure.Models;

//Question is why we need UserRepository even if we have Generic Repository ?
//Bcz generic repo is not fullfill all ur needs, which means I can't pollute this GenericRepository.cs repository with so many methods.
//or so many specific methods. I have to only put down in G.R. - Add, get, commit, delete.

namespace ePizzaHub.Repositories.Contract
{
    public interface IUserRepository : IGenericRepository<User>
    {

    }
}
