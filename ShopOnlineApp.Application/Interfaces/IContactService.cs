using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ShopOnlineApp.Application.Common;
using ShopOnlineApp.Application.ViewModels.Contact;
using ShopOnlineApp.Data.EF.Common;

namespace ShopOnlineApp.Application.Interfaces
{
    public interface IContactService
    {
        void Add(ContactViewModel contactVm);

        void Update(ContactViewModel contactVm);

        void Delete(string id);

        List<ContactViewModel> GetAll();
        BaseReponse<ModelListResult<ContactViewModel>>  GetAllPaging(ContactRequest request);
        ContactViewModel GetById(string id);

        void SaveChanges();
    }
}
