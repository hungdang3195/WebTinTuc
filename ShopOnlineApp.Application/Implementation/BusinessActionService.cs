using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore.Internal;
using ShopOnlineApp.Application.Interfaces;
using ShopOnlineApp.Application.ViewModels.Business;
using ShopOnlineApp.Application.ViewModels.BusinessAction;
using ShopOnlineApp.Data.IRepositories;
using ShopOnlineApp.Infrastructure.Interfaces;

namespace ShopOnlineApp.Application.Implementation
{
    public class BusinessActionService : IBusinessActionService
    {
        private readonly IBusinessActionRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        public BusinessActionService(IBusinessActionRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }
        public List<BusinessActionViewModel>  GetByBusinessIds(string businessId)
        {
            var items = _repository.FindAll(x => x.BusinessId == businessId).GroupBy(x=>x.Name).Select(x=>x.FirstOrDefault());

            return new BusinessActionViewModel().Map(items).ToList();
        }

        public BusinessActionViewModel GetByActionId(int id)
        {
            return new BusinessActionViewModel().Map(_repository.FindById(id));
        }

        public void Update(BusinessActionViewModel businessVm)
        {
            var currentBusiness = _repository.FindById(businessVm.Id);

            currentBusiness.Name = businessVm.Name;
            _repository.Update(currentBusiness);
            _unitOfWork.Commit();
        }
    }
}
