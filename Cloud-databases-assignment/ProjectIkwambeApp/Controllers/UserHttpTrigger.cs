using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Domain.DTO;
using HttpMultipartParser;
using Infrastructure.Services.Products;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Cloud_databases_assignment.Controllers
{
    public class UserHttpTrigger
    {
        ILogger Logger { get; }
        private readonly IUserService _userService;

        public UserHttpTrigger(ILogger<UserHttpTrigger> Logger, IUserService userService)
        {
            this.Logger = Logger;
            _userService = userService;
        }

        [Function(nameof(UserHttpTrigger.AddUser))]
        public async Task<HttpResponseData> AddUser([HttpTrigger(AuthorizationLevel.Anonymous, "POST", Route = "users")] HttpRequestData req, FunctionContext executionContext)
        {

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            UserDTO userDTO = JsonConvert.DeserializeObject<UserDTO>(requestBody);
            HttpResponseData response = req.CreateResponse(HttpStatusCode.Created);
            await response.WriteAsJsonAsync(await _userService.AddUser(userDTO));
            return response;

        }

        [Function(nameof(UserHttpTrigger.GetUsers))]
        public async Task<HttpResponseData> GetUsers([HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "users")] HttpRequestData req, FunctionContext executionContext)
        {
            {
                HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);

                await response.WriteAsJsonAsync(await _userService.GetAllUsers());

                return response;
            }
        }

        [Function(nameof(UserHttpTrigger.GetUsersById))]
        public async Task<HttpResponseData> GetUsersById([HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "users/{userId}")] HttpRequestData req, string userId, FunctionContext executionContext)
        {

            HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(await _userService.GetUserById(userId));
            return response;

        }

        [Function(nameof(UserHttpTrigger.UpdateUser))]
        public async Task<HttpResponseData> UpdateUser([HttpTrigger(AuthorizationLevel.Anonymous, "PUT", Route = "users/{userId}")] HttpRequestData req, string userId, FunctionContext executionContext)
        {

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            UserDTO userDTO = JsonConvert.DeserializeObject<UserDTO>(requestBody);
            HttpResponseData response = req.CreateResponse(HttpStatusCode.Created);
            await response.WriteAsJsonAsync(await _userService.UpdateUser(userDTO, userId));
            return response;
        }
    }
}
