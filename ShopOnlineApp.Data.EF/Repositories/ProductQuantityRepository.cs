using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShopOnlineApp.Data.Entities;
using ShopOnlineApp.Data.IRepositories;

namespace ShopOnlineApp.Data.EF.Repositories
{
    public class ProductQuantityRepository : EFRepository<ProductQuantity, int>, IProductQuantityRepository
    {
        private readonly AppDbContext _context;
        public ProductQuantityRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public ProductQuantity GetByProductId(int productId)
        {
            return _context.ProductQuantities.FirstOrDefault(x => x.ProductId == productId);
        }
    }
}
