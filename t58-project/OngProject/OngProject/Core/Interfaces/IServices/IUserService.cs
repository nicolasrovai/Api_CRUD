using OngProject.Core.DTOs;
using OngProject.Core.Helper.Pagination;
using OngProject.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OngProject.Core.Interfaces.IServices
{
    public interface IUserService
    {
        public Task<bool> DeleteUser(int Id);
        public Task<ResponsePagination<GenericPagination<UserModel>>> GetUsers(int page, int sizeByPage);
        public bool UserExists(int Id);
        public Task<UserInfoDto> GetUserById(int Id);
        public Task<UserModel> Put(UserUpdateDto userUpdateDto, int id);

    }
}
