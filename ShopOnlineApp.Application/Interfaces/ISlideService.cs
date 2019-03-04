using System;
using System.Collections.Generic;
using System.Text;
using ShopOnlineApp.Application.ViewModels.Color;
using ShopOnlineApp.Application.ViewModels.Slide;
using ShopOnlineApp.Data.EF.Common;
using ShopOnlineApp.Data.Entities;

namespace ShopOnlineApp.Application.Interfaces
{
    public interface ISlideService
    {
        void Add(SlideViewModel slideVm);
        void Update(SlideViewModel slideVm);
        void Delete(int id);
        List<SlideViewModel> GetAll();
        BaseReponse<ModelListResult<SlideViewModel>> GetAllPaging(SlideRequest request);
        SlideViewModel GetById(int id);
        void SaveChanges();
    }
}
