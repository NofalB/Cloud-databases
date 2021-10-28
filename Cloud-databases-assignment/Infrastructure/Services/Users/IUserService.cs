using Domain;
using Domain.DTO;
using HttpMultipartParser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services.Products
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetAllUsers();

        Task<User> GetUserById(string userId);

        Task<User> AddUser(UserDTO userDto);

        Task<User> UpdateUser(UserDTO userDto, string userId);
    }
}
