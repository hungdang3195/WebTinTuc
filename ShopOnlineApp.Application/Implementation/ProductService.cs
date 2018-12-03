using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ShopOnlineApp.Application.Interfaces;
using ShopOnlineApp.Application.ViewModels.Product;
using ShopOnlineApp.Data.EF.Common;
using ShopOnlineApp.Data.IRepositories;
using ShopOnlineApp.Utilities.Enum;
using Status = ShopOnlineApp.Data.Enums.Status;

namespace ShopOnlineApp.Application.Implementation
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productionRepository;
        public ProductService(IProductRepository productionRepository)
        {
            _productionRepository = productionRepository;
        }
        public async Task<List<ProductViewModel>> GetAll()
        {
            try
            {
                var response = await _productionRepository.FindAll().ToListAsync();

                return response.Any() ? new ProductViewModel().Map(response).ToList() : new List<ProductViewModel>();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<BaseReponse<ModelListResult<ProductViewModel>>> GetAllPaging(ProductRequest request)
        {
            var response = _productionRepository.FindAll(x => x.Status == Status.Active).AsNoTracking();

            if (!string.IsNullOrEmpty(request.SearchText))
            {
                response = response.Where(x => x.Name.Contains(request.SearchText));
            }
            else if (!string.IsNullOrEmpty(request.Name))
            {
                response = response.Where(x => x.Name.Contains(request.Name));
            }

            var totalCount = await response.CountAsync();

            if (request.IsPaging)
            {
                response = response.OrderByDescending(x => x.DateCreated)

                    .Skip((request.PageIndex - 1) * request.PageSize).Take(request.PageSize);
            }

            var items = new ProductViewModel().Map(response).ToList();

            return new BaseReponse<ModelListResult<ProductViewModel>>()
            {
                Data = new ModelListResult<ProductViewModel>()
                {
                    Items = items,
                    Message = Message.Success,
                    RowCount = totalCount,
                    PageSize = request.PageSize,
                    PageIndex = request.PageIndex
                },
                Message = Message.Success,
                Status= (int)QueryStatus.Success
            };
        }


        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
