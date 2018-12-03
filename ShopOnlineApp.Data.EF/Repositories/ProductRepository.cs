using ShopOnlineApp.Data.Entities;
using ShopOnlineApp.Data.IRepositories;
using TeduCoreApp.Data.EF;

namespace ShopOnlineApp.Data.EF.Repositories
{
    public class ProductRepository : EFRepository<Product,int>, IProductRepository
    {
        public ProductRepository(AppDbContext context) : base(context)
        {

        }
    }
}
