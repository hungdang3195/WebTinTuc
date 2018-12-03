using System;
using System.Collections.Generic;
using System.Text;
using ShopOnlineApp.Data.Entities;
using ShopOnlineApp.Data.IRepositories;
using TeduCoreApp.Data.EF;

namespace ShopOnlineApp.Data.EF.Repositories
{
    public class FunctionRepository: EFRepository<Function, string>, IFunctionRepository
    {
        public FunctionRepository(AppDbContext context) : base(context)
        {

        }
    }
}
