using AutoMapper;
using BLL.Interfaces;
using BLL.Models;
using BLL.Validation;
using BLL.Validation.Exceptions;
using DAL.Entities;
using DAL.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Linq.Expressions;
using BLL.Models.AdminPagination;
using System.Data.Entity.Core.Common.CommandTrees;
using BLL.Helpers;
using BLL.Models.Enums;

namespace BLL.Services
{
    /// <summary>
    /// Service for operations with users
    /// </summary>
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IPhotoService _photoService;
        private readonly Microsoft.AspNetCore.Identity.UserManager<ApplicationUser> _userManager;
        public UserService(IUnitOfWork unitOfWork, IMapper mapper, IPhotoService photoService, Microsoft.AspNetCore.Identity.UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _photoService = photoService;
            _userManager = userManager;
        }

        /// <summary>
        /// Delete user by id
        /// </summary>
        /// <param name="userId">User id></param>
        /// <returns>Task</returns>
        /// <exception cref="YOMException">if something go wrong</exception>
        public async Task DeleteAsync(string userId)
        {
            try
            {
                var user = await _unitOfWork.UserRepository.GetByParamAsync(u => u.Id == userId);
                if (!string.IsNullOrEmpty(user.AvatarPath))
                    await _photoService.DeletePhotoByPathAsync(user.AvatarPath);
                await _unitOfWork.UserRepository.DeleteById(userId)!;
                await _unitOfWork.SaveAsync();
            }
            catch (Exception ex)
            {
                throw new YOMException(ex.Message);
            }
        }

        /// <summary>
        /// Get all users
        /// </summary>
        /// <returns>Collection of user</returns>
        /// <exception cref="YOMException">If something go wrong</exception>
        public async Task<IEnumerable<User?>> GetAll()
        {
            try
            {
                var unmappedResult = await _unitOfWork.UserRepository.GetAll() ?? null;
                var mappedResult = _mapper.Map<IEnumerable<User>>(unmappedResult);
                foreach (var user in mappedResult)
                {
                    user.AvatarPath = await _photoService.GetUrlPhotoPath(user.AvatarPath);
                }
                return mappedResult;
            }
            catch (Exception ex)
            {
                throw new YOMException(ex.Message);
            }
        }

        public async Task<AdminUsers> GetAllPagination(int pageNumber, int? pageSize)
        {
            try
            {
                pageSize ??= 20;
                if (pageNumber <= 0 || pageSize <= 0)
                    throw new YOMException("Incorrect input data");

                var totalCount = await _unitOfWork.UserRepository.GetCount(null);
                var totalPages = (int)Math.Ceiling(totalCount / (decimal)pageSize);

                var unmappedResult = await _unitOfWork.UserRepository.GetAllByQueryAsync(new DAL.Helpers.FilterState { PageNumber = pageNumber, PageSize = pageSize }) ?? null;
                var mappedResult = _mapper.Map<IEnumerable<User>>(unmappedResult);
                foreach (var user in mappedResult)
                {
                    user.AvatarPath = await _photoService.GetUrlPhotoPath(user.AvatarPath);
                }

                return new AdminUsers { Users = mappedResult, TotalPages = totalPages };
            }
            catch (Exception ex)
            {
                throw new YOMException(ex.Message);
            }
        }
        /// <summary>
        /// Get user by id
        /// </summary>
        /// <param name="id">User id</param>
        /// <returns>User</returns>
        /// <exception cref="YOMException">if something go wrong</exception>
        public async Task<User> GetByIdAsync(string id)
        {
            try
            {
                var unmappedResult = await _unitOfWork.UserRepository.GetByParamAsync(user => user.Id == id);
                if (unmappedResult is null) throw new YOMException("User with this id not exist");
                var mappedResult = _mapper.Map<User>(unmappedResult);
                mappedResult.AvatarPath = await _photoService.GetUrlPhotoPath(mappedResult.AvatarPath);
                return mappedResult;
            }
            catch (Exception ex)
            {
                throw new YOMException(ex.Message);
            }
        }

        public async Task UpdateAsync(User user)
        {
            try
            {
                if (UserValidator.CheckUserData(user))
                {
                    var possibleUser = await _userManager.FindByIdAsync(user.Id);
                    if (possibleUser is null) 
                        throw new YOMException("User with this Id not exist");
                    possibleUser.FullName = user.FullName;
                    possibleUser.UserName = user.UserName;
                    possibleUser.Email = user.Email;
                    possibleUser.PhoneNumber = user.PhoneNumber;
                    possibleUser.SecurityStamp = Guid.NewGuid().ToString();
                    await _userManager.UpdateAsync(possibleUser);
                    await _unitOfWork.SaveAsync();
                }
            }
            catch (Exception ex)
            {
                throw new YOMException(ex.Message);
            }
        }

        /// <summary>
        /// Get user by id
        /// </summary>
        /// <param name="id">User id</param>
        /// <returns>User</returns>
        /// <exception cref="YOMException">if something go wrong</exception>
        public async Task<IEnumerable<User>> GetAllUsersByParams(Expression<Func<ApplicationUser, bool>> filterExpression, int pageNumber, int? pageSize)
        {
            try
            {
                var unmappedResult = await _unitOfWork.UserRepository.GetAllByParamsAsync(filterExpression);
                var mappedResult = _mapper.Map<IEnumerable<User>>(unmappedResult);
                foreach (var user in mappedResult)
                {
                    user.AvatarPath = await _photoService.GetUrlPhotoPath(user.AvatarPath);
                }
                return mappedResult;
            }
            catch (Exception ex)
            {
                throw new YOMException(ex.Message);
            }
        }

        public async Task<AdminUsers> GetAllBlockedUsers(int pageNumber, int? pageSize)
        {
            try
            {

                pageSize ??= 20;
                if (pageNumber <= 0 || pageSize <= 0)
                    throw new YOMException("Incorrect input data");

                var filterState = FilterExtension.CreateSingleFilter("IsBlocked", GridFilterOperator.eq, true.ToString(), // filter
                    GridSortDirection.desc, "RefreshTokenExpiryInDays", pageNumber, pageSize); // sort
                var mappedFilterState = _mapper.Map<DAL.Helpers.FilterState>(filterState);
                var unmappedResult = await _unitOfWork.UserRepository.GetAllByQueryAsync(mappedFilterState);
                var mappedResult = _mapper.Map<IEnumerable<User>>(unmappedResult);


                var totalCount = await _unitOfWork.UserRepository.GetCount(null);
                var totalPages = (int)Math.Ceiling(totalCount / (decimal)pageSize);

                foreach (var user in mappedResult)
                {
                    user.AvatarPath = await _photoService.GetUrlPhotoPath(user.AvatarPath);
                }

                return new AdminUsers { Users = mappedResult, TotalPages = totalPages };
            }
            catch (Exception ex)
            {
                throw new YOMException(ex.Message);
            }
        }

        public async Task<IEnumerable<User>> GetAllUsersByFilter(FilterState query)
        {
            var mappedFilter = _mapper.Map<DAL.Helpers.FilterState>(query);
            var result = await _unitOfWork.AdRepository.GetAllByQueryAsync(mappedFilter);
            var mappedResult = _mapper.Map<IEnumerable<User>>(result);
            foreach (var user in mappedResult)
            {
                user.AvatarPath = await _photoService.GetUrlPhotoPath(user.AvatarPath);
            }
            return mappedResult;
        }

        /// <summary>
        /// Update user
        /// </summary>
        /// <param name="user">User</param>
        /// <returns>Task</returns>
        /// <exception cref="YOMException">If one of the user params is invalid</exception>
        public async Task UpdateAsync(User user, IFormFile photo)
        {
            try
            {
                if (UserValidator.CheckUserData(user))
                {
                    var possibleUser = await _userManager.FindByIdAsync(user.Id);
                    if (possibleUser is null)
                        throw new YOMException("User with this Id not exist");
                    possibleUser.FullName = user.FullName;
                    possibleUser.UserName = user.UserName;
                    possibleUser.Email = user.Email;
                    possibleUser.PhoneNumber = user.PhoneNumber;
                    var avatarPath = await _photoService.UploadPhotoAsync(photo, user);
                    possibleUser.AvatarPath = avatarPath;
                    possibleUser.SecurityStamp = Guid.NewGuid().ToString();
                    await _userManager.UpdateAsync(possibleUser);
                    await _unitOfWork.SaveAsync();
                }
            }
            catch (Exception ex)
            {
                throw new YOMException(ex.Message);
            }
        }

        public async Task BlockUserAsync(string userId)
        {
            try
            {
                var possibleUser = await _userManager.FindByIdAsync(userId) ?? throw new YOMException("User with this id not exist");
                if (possibleUser.IsBlocked is true) throw new YOMException("User with this id already blocked");
                possibleUser.IsBlocked = true;
                await _userManager.UpdateAsync(possibleUser);
                await _unitOfWork.SaveAsync();
            }
            catch (Exception ex)
            {
                throw new YOMException(ex.Message);
            }
        }

        public async Task UnBlockUserAsync(string userId)
        {
            try
            {
                var possibleUser = await _userManager.FindByIdAsync(userId) ?? throw new YOMException("User with this id not exist");
                if (possibleUser.IsBlocked is false) throw new YOMException("User with this id not blocked");
                possibleUser.IsBlocked = false;
                await _userManager.UpdateAsync(possibleUser);
                await _unitOfWork.SaveAsync();
            }
            catch (Exception ex)
            {
                throw new YOMException(ex.Message);
            }
        }

        public async Task<int> GetUsersCountAsync()
        {
            return await _unitOfWork.UserRepository.GetCount(null);
        }

        public async Task<int> GetUsersBlockedCountAsync()
        {
            return await _unitOfWork.UserRepository.GetCount(u=> u.IsBlocked == true);
        }
    }
}
