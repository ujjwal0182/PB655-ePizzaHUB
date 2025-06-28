using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ePizzaHub.Core.Contracts;
using ePizzaHub.Models.ApiModels.Request;
using ePizzaHub.Repositories.Contract;

namespace ePizzaHub.Core.Concrete
{
    public class UserService : IUserService
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IUserRepository _userRepository;

        public UserService(IRoleRepository roleRepository, IUserRepository userRepository)
        {
            _roleRepository = roleRepository;
            _userRepository = userRepository;
        }
        public Task<bool> CreateUserRequestAsync(CreateUserRequest createUserRequest)
        {
            // 1. insert record into user and user role tables.
            // 2. Hash passord sending by end user.

            //Find the detail of user where the role is User from the Role table
            var roleDetails = _roleRepository.GetAll().Where(x => x.Name == "User").FirstOrDefault();

            if(roleDetails != null)
            {

            }

            throw new NotImplementedException();
        }
    }
}
