using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Domain;
using Domain.DTO;
using HttpMultipartParser;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services.Products
{
    public class UserService : IUserService
    {
        private readonly ICosmosReadRepository<User> _userReadRepository;
        private readonly ICosmosWriteRepository<User> _userWriteRepository;

        public UserService(ICosmosReadRepository<User> userReadRepository, ICosmosWriteRepository<User> userWriteRepository)
        {
            _userReadRepository = userReadRepository;
            _userWriteRepository = userWriteRepository;
        }

        public async Task<User> AddUser(UserDTO userDto)
        {
            Guid id = Guid.NewGuid();
            User user = new User();
            user.UserId = id;
            user.FirstName = userDto.FirstName;
            user.LastName = userDto.LastName;
            user.PartitionKey = id;



            return await _userWriteRepository.AddAsync(user);
        }

        public async Task DeleteUserAsync(string userId)
        {
            var id = !string.IsNullOrEmpty(userId) ? userId : throw new ArgumentNullException($"{userId} cannot be null or empty string.");
            User user = await GetUserById(id);

            if (user != null)
            {
                await _userWriteRepository.Delete(user);
            }
            else
            {
                throw new InvalidOperationException($"The user ID {userId} provided is invalid.");
            }
        }

        public async Task<IEnumerable<User>> GetAllUsers()
        {
            return await _userReadRepository.GetAll().ToListAsync();
        }

        public async Task<User> GetUserById(string userId)
        {
            var userGuid = Guid.Parse(userId);
            var user = await _userReadRepository.GetAll().FirstOrDefaultAsync(t => t.UserId == userGuid);
            return user;
        }

        public async Task<User> UpdateUser(UserDTO userDto, string userId)
        {
            var existingUser = await GetUserById(userId);
            if (existingUser != null)
            {
                existingUser.FirstName = userDto.FirstName;
                existingUser.LastName = userDto.LastName;
                return await _userWriteRepository.Update(existingUser);
            }
            else
            {
                throw new InvalidOperationException($"The user ID {userId} provided does not exist");
            }
        }
    }
}
