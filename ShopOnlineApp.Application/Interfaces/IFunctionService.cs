using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ShopOnlineApp.Application.ViewModels.Function;

namespace ShopOnlineApp.Application.Interfaces
{
    public  interface IFunctionService:IDisposable
    {
        Task< List<FunctionViewModel>> GetAll();
        Task<List<FunctionViewModel>>  GetAllByPermission(Guid userId);
    }
}
