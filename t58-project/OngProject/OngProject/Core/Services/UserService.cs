
using OngProject.Core.Interfaces.IServices;
using OngProject.Core.Interfaces.IUnitOfWork;
using OngProject.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OngProject.Core.DTOs;
using OngProject.Core.Mapper;
using OngProject.Core.Interfaces.IServices.AWS;
using OngProject.Core.Helper.Pagination;
using OngProject.Core.Interfaces.IServices.IUriPaginationService;

namespace OngProject.Core.Services
{
    public class UserService: IUserService
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IImagenService _imagenService;
        private readonly IUriPaginationService _uriPaginationService;

        public UserService(IUnitOfWork unitOfWork, IImagenService imagenService, IUriPaginationService uriPaginationService)
        {
            _unitOfWork = unitOfWork;
            _imagenService = imagenService;
            _uriPaginationService = uriPaginationService;
        }

        public async Task<bool> DeleteUser(int Id)
        {

            try
            {           
                await _unitOfWork.UserRepository.Delete(Id);
                await _unitOfWork.SaveChangesAsync();
                
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        public bool UserExists(int Id)
        {
            return _unitOfWork.UserRepository.EntityExists(Id);
        }

        public async Task<ResponsePagination<GenericPagination<UserModel>>> GetUsers(int page, int sizeByPage)
        {
            string nextRoute = null, previousRoute = null;
            IEnumerable<UserModel> data = await _unitOfWork.UserRepository.GetAll();
            GenericPagination<UserModel> objGenericPagination = GenericPagination<UserModel>.Create(data, page, sizeByPage);
            ResponsePagination<GenericPagination<UserModel>> response = new ResponsePagination<GenericPagination<UserModel>>(objGenericPagination);
            response.CurrentPage = objGenericPagination.CurrentPage;
            response.HasNextPage = objGenericPagination.HasNextPage;
            response.HasPreviousPage = objGenericPagination.HasPreviousPage;
            response.PageSize = objGenericPagination.PageSize;
            response.TotalPages = objGenericPagination.TotalPages;
            response.TotalRecords = objGenericPagination.TotalRecords;
            response.Data = objGenericPagination;

            if (response.HasNextPage)
            {
                nextRoute = $"/users?page={(page + 1)}";
                response.NextPageUrl = _uriPaginationService.GetPaginationUri(page, nextRoute).ToString();
            }
            else
            {
                response.NextPageUrl = "";
            }

            if (response.HasPreviousPage)
            {
                previousRoute = $"/users?page={(page - 1)}";
                response.PreviousPageUrl = _uriPaginationService.GetPaginationUri(page, previousRoute).ToString();
            }
            else
            {
                response.PreviousPageUrl = null;
            }



            return response;
        }

        public async Task<UserInfoDto> GetUserById(int Id)
        {
            UserModel user = await _unitOfWork.UserRepository.GetById(Id);
            EntityMapper mapper = new EntityMapper();
            UserInfoDto userInfoDto = mapper.FromUserModelToUserInfoDto(user);

            return userInfoDto;
        }

        public async Task<UserModel> Put(UserUpdateDto userUpdateDto, int id)
        {
            var mapper = new EntityMapper();

            try
            {
                UserModel user = await _unitOfWork.UserRepository.GetById(id);

                user = mapper.FromUserUpdateDtoToUser(userUpdateDto, user);

                if (userUpdateDto.Photo != null)
                    user.photo = await _imagenService.Save(user.photo, userUpdateDto.Photo);

                await _unitOfWork.UserRepository.Update(user);
                await _unitOfWork.SaveChangesAsync();

                return user;
            }
            catch (Exception e)
            {

                throw new Exception(e.Message);
            }
        }
    }
}
