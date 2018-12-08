using System;
using System.Collections.Generic;
using System.Text;
using ShopOnlineApp.Data.EF.Common;
using ShopOnlineApp.Utilities;

namespace ShopOnlineApp.Application.ViewModels.Product
{
    public class ProductRequest: BaseRequest
    {
        public string Name { get; set; }
        public int CategoryId { get; set; }

    }
}
