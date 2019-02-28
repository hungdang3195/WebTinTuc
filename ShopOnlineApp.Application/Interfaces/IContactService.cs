using System.Collections.Generic;
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
