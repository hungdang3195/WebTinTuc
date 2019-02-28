using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShopOnlineApp.Application.Interfaces;
using ShopOnlineApp.Application.ViewModels.Color;
using ShopOnlineApp.Data.EF.Common;
using ShopOnlineApp.Data.IRepositories;
using ShopOnlineApp.Infrastructure.Interfaces;
using ShopOnlineApp.Utilities.Enum;

namespace ShopOnlineApp.Application.Implementation
{
    public class ColorService:IColorService
    {
       
            private readonly IColorRepository _colorRepository;
            private readonly IUnitOfWork _unitOfWork;

            public ColorService(IColorRepository colorRepository,
                IUnitOfWork unitOfWork)
            {
                _colorRepository = colorRepository;
                _unitOfWork = unitOfWork;
            }

            public void Add(ColorViewModel pageVm)
            {
                var page = new ColorViewModel().Map(pageVm);
                _colorRepository.Add(page);
            }

            public void Delete(int id)
            {
                _colorRepository.Remove(id);
            }

            public void Dispose()
            {
                GC.SuppressFinalize(this);
            }

            public List<ColorViewModel> GetAll()
            {
                return new ColorViewModel().Map(_colorRepository.FindAll()).ToList();
            }

            public BaseReponse<ModelListResult<ColorViewModel>> GetAllPaging(ColorRequest request)
            {

                var query = _colorRepository.FindAll();
                if (!string.IsNullOrEmpty(request.SearchText))
                    query = query.Where(x => x.Name.Contains(request.SearchText));

                int totalRow = query.Count();
                var data = query.OrderByDescending(x => x.Id)
                    .Skip((request.PageIndex) * request.PageSize)
                    .Take(request.PageSize);

                var items = new ColorViewModel().Map(data).ToList();

                return new BaseReponse<ModelListResult<ColorViewModel>>
                {
                    Data = new ModelListResult<ColorViewModel>()
                    {
                        Items = items,
                        Message = Message.Success,
                        RowCount = totalRow,
                        PageSize = request.PageSize,
                        PageIndex = request.PageIndex
                    },
                    Message = Message.Success,
                    Status = (int)QueryStatus.Success
                };

            }

            public ColorViewModel GetById(int id)
            {
                return new ColorViewModel().Map(_colorRepository.FindById(id));
            }

            public void SaveChanges()
            {
                _unitOfWork.Commit();
            }

            public void Update(ColorViewModel pageVm)
            {
                var page = new ColorViewModel().Map(pageVm);
                _colorRepository.Update(page);
            }
        }
    
}
