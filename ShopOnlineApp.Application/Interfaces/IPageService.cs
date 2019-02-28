using System;
using System.Collections.Generic;
using ShopOnlineApp.Application.Common;
using ShopOnlineApp.Application.ViewModels.Page;
using ShopOnlineApp.Data.EF.Common;

namespace ShopOnlineApp.Application.Interfaces
{
    public interface IPageService : IDisposable
    {
        void Add(PageViewModel pageVm);

        void Update(PageViewModel pageVm);

        void Delete(int id);

        List<PageViewModel> GetAll();

        BaseReponse<ModelListResult<PageViewModel>> GetAllPaging(PageRequest request);

        PageViewModel GetByAlias(string alias);

        PageViewModel GetById(int id);

        void SaveChanges();

    }
}
