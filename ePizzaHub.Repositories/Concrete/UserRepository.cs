using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ePizzaHub.Infrastructure.Models;
using ePizzaHub.Repositories.Contract;

namespace ePizzaHub.Repositories.Concrete
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(PB655Context dbcontext) : base(dbcontext)
        {
        }
    }
}
