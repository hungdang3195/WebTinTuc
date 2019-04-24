using System;
using System.Collections.Generic;
using System.Text;
using ShopOnlineApp.Application.Interfaces;
using ShopOnlineApp.Data.IRepositories;

namespace ShopOnlineApp.Application.Implementation
{
    public class ProductQuantityService : IProductQuantityService
    {
        private readonly IProductQuantityRepository _quantityRepository;
        public ProductQuantityService(IProductQuantityRepository quantityRepository)
        {
            _quantityRepository = quantityRepository;
        }
        public bool SellProduct(int productId, int quantity)
        {
            var productQuantiry = _quantityRepository.GetByProductId(productId);
            if (productQuantiry != null)
            {
                return productQuantiry.Quantity >= quantity;
            }
            return false;
        }

        public void UpdateQuantityProduct(int productId, int quantity)
        {
            var productQuantity = _quantityRepository.GetByProductId(productId);
            if (productQuantity != null)
            {
                productQuantity.Quantity -= quantity;
                _quantityRepository.SaveChanges();
            }

        }
    }
}
