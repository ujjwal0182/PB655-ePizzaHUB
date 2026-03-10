using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ePizzaHub.Infrastructure.Models;
using ePizzaHub.Repositories.Contract;
using Microsoft.EntityFrameworkCore;

namespace ePizzaHub.Repositories.Concrete
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(PB655Context dbcontext) : base(dbcontext)
        {
        }

        //I need to call this method in the service layer (AuthService)
        public async Task<User> FindByUserNameAsync(string userName)
        {
            return await _dbcontext
                .Users
                .Include(x =>x.Roles) //Navigation Property to join with the Roles table
                .FirstOrDefaultAsync(x => x.Email == userName);
        }
    }
}
