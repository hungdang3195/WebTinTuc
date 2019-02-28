using System.Collections.Generic;
using ShopOnlineApp.Application.ViewModels.Size;
using ShopOnlineApp.Data.EF.Common;

namespace ShopOnlineApp.Application.Interfaces
{
    public interface ISizeService
    {
        void Add(SizeViewModel contactVm);
        void Update(SizeViewModel contactVm);
        void Delete(int id);
        List<SizeViewModel> GetAll();
        BaseReponse<ModelListResult<SizeViewModel>> GetAllPaging(SizeRequest request);
        SizeViewModel GetById(int id);
        void SaveChanges();
    }
}
