﻿
using AutoMapper;
using BLL.Helpers;
using BLL.Interfaces;
using BLL.Models;
using BLL.Models.Enums;
using BLL.Validation.Exceptions;
using DAL.Interfaces;

namespace BLL.Services
{
    public class LastViewedService : ILastViewedService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPhotoService _photoService;

        public LastViewedService(IMapper mapper, IUnitOfWork unitOfWork, IPhotoService photoService)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _photoService = photoService;
        }

        public async Task AddAdToLastViewedAsync(LastViewedHistory model)
        {
            try
            {
                var possibleLastView = await _unitOfWork.LastViewedHistoryRepository.GetByParamAsync(lv =>
                    lv.AdId == model.AdId && lv.UserId == model.UserId);

                var filterState = FilterExtension.CreateSingleFilter("UserId", GridFilterOperator.eq, model.UserId, // filter
                    GridSortDirection.desc, "AdPopularity", 1); // sort
                var mappedFilter = _mapper.Map<DAL.Helpers.FilterState>(filterState);
                var lastViews = await _unitOfWork.LastViewedHistoryRepository.GetAllByQueryAsync(mappedFilter);

                if (lastViews.Count() >= 10)
                {
                    // Remove the last item if the count is 10 or more
                    var oldestItem = lastViews.Last();
                    await _unitOfWork.LastViewedHistoryRepository.DeleteById(oldestItem.Id);
                }

                if (possibleLastView is not null)
                {
                    var mappedLastView = _mapper.Map<LastViewedHistory>(possibleLastView);
                    await UpdateAdLastViewedAsync(mappedLastView);
                    return;
                }

                var mappedViewed = _mapper.Map<DAL.Entities.LastViewedHistory>(model);
                mappedViewed.LastSeenAt = DateTime.Now;
                _unitOfWork.LastViewedHistoryRepository.Add(mappedViewed);
                await _unitOfWork.SaveAsync();
            }
            catch (Exception ex)
            {
                throw new YOMException(ex.Message);
            }
        }

        public async Task UpdateAdLastViewedAsync(LastViewedHistory model)
        {
            model.LastSeenAt = DateTime.Now;
            var mappedLastView = _mapper.Map<DAL.Entities.LastViewedHistory>(model);
            _unitOfWork.LastViewedHistoryRepository.Update(mappedLastView);
            await _unitOfWork.SaveAsync();
        }

        public async Task RemoveAdFromLastViewedAsync(LastViewedHistory model)
        {
            try
            {
                var lastViewed = await
                    _unitOfWork.LastViewedHistoryRepository.GetByParamAsync(lv =>
                        lv.AdId == model.AdId && lv.UserId == model.UserId);
                await _unitOfWork.LastViewedHistoryRepository.DeleteById(lastViewed.Id);
                await _unitOfWork.SaveAsync();
            }
            catch (Exception ex)
            {
                throw new YOMException(ex.Message);
            }
        }

        public async Task<IEnumerable<LastViewedHistory>> GetLastViewedAdsAsync(string userId, int? pageNumber, int? pageSize)
        {
            var filterState = FilterExtension.CreateSingleFilter("UserId", GridFilterOperator.eq, userId.ToString(), // filter
                GridSortDirection.asc, "LastSeenAt", pageNumber, pageSize); // sort
            var mappedFilterState = _mapper.Map<DAL.Helpers.FilterState>(filterState);
            var history = await _unitOfWork.LastViewedHistoryRepository.GetAllByQueryAsync(mappedFilterState);
            var mappedResult =  _mapper.Map<IEnumerable<LastViewedHistory>>(history);
            foreach (var lv in mappedResult)
            {
                lv.PhotoPaths = await _photoService.GetUrlPhotoPath(lv.PhotoPaths);
            }

            return mappedResult;
        }
    }
}
