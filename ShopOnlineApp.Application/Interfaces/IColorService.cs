using System.Collections.Generic;
using ShopOnlineApp.Application.ViewModels.Color;
using ShopOnlineApp.Data.EF.Common;
namespace ShopOnlineApp.Application.Interfaces
{
    public interface IColorService
    {
        void Add(ColorViewModel contactVm);
        void Update(ColorViewModel contactVm);
        void Delete(int id);
        List<ColorViewModel> GetAll();
        BaseReponse<ModelListResult<ColorViewModel>> GetAllPaging(ColorRequest request);
        ColorViewModel GetById(int id);
        void SaveChanges();
    }
}
