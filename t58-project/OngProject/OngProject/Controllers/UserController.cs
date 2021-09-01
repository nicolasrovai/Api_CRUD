using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OngProject.Core.DTOs;
using OngProject.Core.DTOs.Auth;
using OngProject.Core.Interfaces.IServices;
using OngProject.Core.Interfaces.IUnitOfWork;
using OngProject.Core.Models;
using OngProject.Core.Services;
using OngProject.Core.Services.Auth;
using OngProject.Infrastructure.Data;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OngProject.Core.Helper.Pagination;
using OngProject.Infrastructure;

namespace OngProject.Controllers
{
    
    [Produces("application/json")]
    [Authorize]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IAuthService _auth;
        public UserController(IUserService userService, IAuthService auth)
        {
            this._userService = userService;
            this._auth = auth;
        }

        // Get /auth/me
        /// <summary>
        /// Obtaining my information with my access token
        /// </summary>
        /// <returns>return the information of the authenticated user</returns>
        /// <response code="200">Returns the user information</response>
        /// <response code="401">Unauthorized user</response>
        /// <response code="404">Not Found</response> 
        [HttpGet("/auth/me")]
        [ProducesResponseType(typeof(UserInfoDto), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> GetUserData() //UserInfoDto
        {
            GenericResult<UserInfoDto> userModeldto = new GenericResult<UserInfoDto>();
            try
            {
                string authToken = Request.Headers["Authorization"];
                int userId = _auth.GetUserId(authToken);

                userModeldto.data = await _userService.GetUserById(userId);
                userModeldto.IsSuccess = true;
                userModeldto.Message = "Get data successfully.";
                return Ok(userModeldto);
            }
            catch (Exception e)
            {
                userModeldto.IsSuccess = false;
                userModeldto.Message = "Get data failed.";
                userModeldto.Error= e.Message;
                return BadRequest(userModeldto);
            }
        }

        // Post /auth/register
        /// <summary>
        /// Registration of new users to the system
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /auth/register
        ///     {
        ///        "firstName": "Administrador",
        ///        "lastName": "Sistema",
        ///        "email": "mailAdmin@gmail.com",
        ///        "password": "123456"
        ///     }
        /// </remarks>
        /// <returns>A new user created with their access token</returns>
        /// <response code="200">Returns the newly created user</response>
        /// <response code="400">Bad Request</response> 
        [AllowAnonymous]
        [HttpPost("/auth/register")]
        [ProducesResponseType(typeof(UserDto), 200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<GenericResult<UserDto>>> Register([FromForm] RegisterDTO request)
        {
            GenericResult<UserDto> userDtoResult = new GenericResult<UserDto>();
            try
            {
                userDtoResult.data = await this._auth.register(request);

                if (userDtoResult.data != null)
                {
                    userDtoResult.IsSuccess = true;
                    userDtoResult.Message = "Register successully.";
                    return Ok(userDtoResult);
                }
            }
            catch (Exception e)
            {
                userDtoResult.IsSuccess = false;
                userDtoResult.Message = "Error Register.";
                userDtoResult.Error= e.Message;
                return BadRequest(userDtoResult);
            }

            userDtoResult.IsSuccess = false;
            userDtoResult.Message = "Error Register.";
            return BadRequest(userDtoResult);
        }


        // Post /auth/login
        /// <summary>
        /// Login and return access token
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /auth/login
        ///     {
        ///        "email": "mail1@Mail.com",
        ///        "password": "Admin123"
        ///     }
        /// </remarks>
        /// <returns>A user with their access token</returns>
        /// <response code="200">returns a user with his access token</response>
        /// <response code="404">Not found user</response> 
        [AllowAnonymous]
        [HttpPost("/auth/login")]
        [ProducesResponseType(typeof(UserDto), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> Login([FromBody] LoginDTO request)
        {
            var user = await this._auth.login(request);
            GenericResult<UserDto> userDtoResult = new GenericResult<UserDto> ();
            if (user == null)
            {
               // userDtoResult.data = null;
                userDtoResult.IsSuccess = false;
                userDtoResult.Message = "User or password do not exists.";
                return NotFound(userDtoResult);
            }

            userDtoResult.data = user;
            userDtoResult.IsSuccess = true;
            userDtoResult.Message = "Login successfully.";
            return Ok(userDtoResult);

        }

        // Delete /users/10
        /// <summary>
        /// Delete a user from the system
        /// </summary>
        /// <param name="id">User Id</param>
        /// <returns>Message that a user has been successfully deleted</returns>
        /// <response code="200">User deleted successfully</response>
        /// <response code="401">Unauthorized user</response>
        /// <response code="404">Not Found</response> 
        [Authorize]
        [HttpDelete("/users/{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Delete(int id)
        {
            GenericResult<bool> userExistsResult = new GenericResult<bool>();
            userExistsResult.data = _userService.UserExists(id);
            if (!userExistsResult.data)
            {
                userExistsResult.IsSuccess = false;
                userExistsResult.Message = "User not found.";
                return NotFound(userExistsResult);
            }

            userExistsResult.data = await _userService.DeleteUser(id);
            if (userExistsResult.data == true)
            {
                userExistsResult.IsSuccess = true;
                userExistsResult.Message = "Delete user successfully.";
                return Ok(userExistsResult);
            }
            else
            {
                userExistsResult.IsSuccess = false;
                userExistsResult.Message = "Delete user failed.";
                return BadRequest(userExistsResult);
            }
        }

        // Get /users
        /// <summary>
        /// Get the data of all registered users
        /// </summary>
        /// <returns>returns the information of authenticated users</returns>
        /// <response code="200">Returns the users information</response>
        /// <response code="401">Unauthorized user</response>
        [Authorize(Roles ="Admin")]
        [HttpGet("/users")]
        [ProducesResponseType(typeof(UserModel),200)]
        [ProducesResponseType(401)]
        public async Task<ResponsePagination<GenericPagination<UserModel>>> GetUsers(int page = 1, int sizeByPage = 10)
        {
            return await _userService.GetUsers(page, sizeByPage);
        }


        // Patch /users/2
        /// <summary>
        /// Update a user's information
        /// </summary>
        /// <param name="id">User Id</param>
        /// <param name="userUpdateDto">DTO updated user information</param>
        /// <returns>returns the updated information of a user</returns>
        /// <response code="200">Return the user information updated</response>
        /// <response code="401">Unauthorized user</response>
        /// <response code="404">User inexistent</response>
        [Authorize]
        [HttpPatch("users/{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Put(int id, [FromForm] UserUpdateDto userUpdateDto)
        {
            GenericResult<bool> userExistsResult = new GenericResult<bool>();
            try
            {
                userExistsResult.data = _userService.UserExists(id);

                if (!userExistsResult.data)
                {
                    userExistsResult.IsSuccess = false;
                    userExistsResult.Message = "User not found.";
                    return NotFound(userExistsResult);
                }
                else
                {
                    GenericResult<UserModel> userPutResult = new GenericResult<UserModel> ();
                    userPutResult.data = await _userService.Put(userUpdateDto, id);
                    userPutResult.IsSuccess = true;
                    userPutResult.Message = "User update successfully.";
                    return Ok(userPutResult);

                }
            }
            catch (Exception e)
            {
                userExistsResult.IsSuccess = false;
                userExistsResult.Message = "Update user failed.";
                userExistsResult.Error=e.Message;
                return BadRequest(userExistsResult);
            }
        }
    }
}
