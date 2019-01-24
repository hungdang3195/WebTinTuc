using System;
using System.Collections.Generic;
using System.Linq;
using ShopOnlineApp.Application.Interfaces;
using ShopOnlineApp.Application.ViewModels.Contact;
using ShopOnlineApp.Data.EF.Common;
using ShopOnlineApp.Data.Entities;
using ShopOnlineApp.Infrastructure.Interfaces;
using ShopOnlineApp.Utilities.Enum;

namespace ShopOnlineApp.Application.Implementation
{
    public class ContactService : IContactService
    {
        private readonly IRepository<Contact, string> _contactRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ContactService(IRepository<Contact, string> contactRepository,
            IUnitOfWork unitOfWork)
        {
            _contactRepository = contactRepository;
            _unitOfWork = unitOfWork;
        }

        public void Add(ContactViewModel pageVm)
        {
            var page = new ContactViewModel().Map(pageVm);
            _contactRepository.Add(page);
        }

        public void Delete(string id)
        {
            _contactRepository.Remove(id);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public List<ContactViewModel> GetAll()
        {
            return new ContactViewModel().Map(_contactRepository.FindAll()).ToList();
        }

        public BaseReponse<ModelListResult<ContactViewModel>> GetAllPaging(ContactRequest request)
        {

            var query = _contactRepository.FindAll();
            if (!string.IsNullOrEmpty(request.SearchText))
                query = query.Where(x => x.Name.Contains(request.SearchText));

            int totalRow = query.Count();
            var data = query.OrderByDescending(x => x.Id)
                .Skip((request.PageIndex) * request.PageSize)
                .Take(request.PageSize);

            var items = new ContactViewModel().Map(data).ToList();

            return new BaseReponse<ModelListResult<ContactViewModel>>
            {
                Data = new ModelListResult<ContactViewModel>()
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

        public ContactViewModel GetById(string id)
        {
            return new ContactViewModel().Map(_contactRepository.FindById(id));
        }

        public void SaveChanges()
        {
            _unitOfWork.Commit();
        }

        public void Update(ContactViewModel pageVm)
        {
            var page = new ContactViewModel().Map(pageVm);
            _contactRepository.Update(page);
        }
    }
}
