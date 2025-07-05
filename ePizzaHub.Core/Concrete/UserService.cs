using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using ePizzaHub.Core.Contracts;
using ePizzaHub.Infrastructure.Models;
using ePizzaHub.Models.ApiModels.Request;
using ePizzaHub.Repositories.Contract;

namespace ePizzaHub.Core.Concrete
{
    public class UserService : IUserService
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserService(IRoleRepository roleRepository, IUserRepository userRepository, IMapper mapper)
        {
            _roleRepository = roleRepository;
            _userRepository = userRepository;
            _mapper = mapper;
        }
        public async Task<bool> CreateUserRequestAsync(CreateUserRequest createUserRequest)
        {
            // 1. insert record into user and user role tables.
            // 2. Hash passord sending by end user.

            //Find the detail of user where the role is User from the Role table
            var roleDetails = _roleRepository.GetAll().Where(x => x.Name == "User").FirstOrDefault();

            if(roleDetails != null)
            {
                ///I need to convert this (\ePizzaHub.Models\ApiModels\Request\CreateUserRequest.cs) user request into this (\ePizzaHub.Infrastructure\Models\User.cs) User format.
                ///And that can be done at the BAL layer (UserService.cs (CreateUserRequest to user format))

                ///we have use AutoMapper to convert this, 1. Install package AutoMapper [A convention-based object-object mapper.]
                ///2. To use this we have to create a mapping profile. \ePizzaHub.Core\Mappers\UserMappingProfile.cs

                var user = _mapper.Map<User>(createUserRequest);
                user.Roles.Add(roleDetails); //I would be able to insert record in my board table. User & UserRoles tables because of navigation properties.

                user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);

                await _userRepository.AddAsync(user);
                int rowInserted = await _userRepository.CommitAsync();

                return rowInserted > 0;
            }
            return false;
        }
    }
}
