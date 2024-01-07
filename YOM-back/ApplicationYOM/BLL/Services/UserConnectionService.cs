using AutoMapper;
using BLL.Interfaces;
using BLL.Models.Users;
using BLL.Validation.Exceptions;
using DAL.Interfaces;

namespace BLL.Services
{
    public class UserConnectionService : IUserConnectionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UserConnectionService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task Disconnect(string userId)
        {
            try
            {
                var userConnection = await _unitOfWork.UserConnectionRepository.GetByParamAsync(u => u.UserId == userId) 
                                     ?? throw new YOMException("Connection with this id not exist");

                userConnection.DateLogout = DateTime.Now;

                _unitOfWork.UserConnectionRepository.Update(userConnection);
                await _unitOfWork.SaveAsync();
            }
            catch (Exception ex)
            {
                throw new ConnectionException(ex.Message);
            }
        }

        public async Task UpdateConnection(UserConnectionInfo model)
        {
            try
            {
                var userConnection = await _unitOfWork.UserConnectionRepository.GetByParamAsync(u => u.UserId == model.UserId)
                                     ?? new DAL.Entities.UserConnectionInfo { DateCreated = DateTime.Now };

                userConnection.UserId = model.UserId;
                userConnection.DateLogin = DateTime.Now;
                userConnection.DateLogout = DateTime.Now;
                userConnection.UserDevice = _mapper.Map<DAL.Entities.UserDevice>(model.UserDevice);

                _unitOfWork.UserConnectionRepository.Update(userConnection);
                await _unitOfWork.SaveAsync();
            }
            catch (Exception ex)
            {
                throw new ConnectionException(ex.Message);
            }
        }

        public async Task<UserConnectionInfo> GetUserConnectionInfo(string userId)
        {
            try
            {
                var userConnection = await _unitOfWork.UserConnectionRepository.GetByParamAsync(u => u.UserId == userId)
                                     ?? throw new YOMException("Connection with this id not exist");
                return _mapper.Map<UserConnectionInfo>(userConnection);
            }
            catch (Exception ex)
            {
                throw new ConnectionException(ex.Message);
            }

        }

        public async Task<IEnumerable<UserConnectionInfo>> GetAllUsersInfo()
        {
            try
            {
                var connections =  await _unitOfWork.UserConnectionRepository.GetAll();
                return _mapper.Map<IEnumerable<UserConnectionInfo>>(connections);
            }
            catch (Exception ex)
            {
                throw new ConnectionException(ex.Message);
            }
        }
    }
}
