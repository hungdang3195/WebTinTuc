using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using ShopOnlineApp.Application.Common;
using ShopOnlineApp.Application.ViewModels.Product;
using ShopOnlineApp.Data.EF.Common;

namespace ShopOnlineApp.Models.ProductViewModels
{
    public class CatalogViewModel
    {
        public string Keyword { get; set; }
        public ModelListResult<ProductViewModel> Data { get; set; }

        public ProductCategoryViewModel Category { set; get; }

        public string SortType { set; get; }

        public int? PageSize { set; get; }

        public List<SelectListItem> SortTypes { get; } = new List<SelectListItem>
        {
            new SelectListItem(){Value = "lastest",Text = "Lastest"},
            new SelectListItem(){Value = "price",Text = "Price"},
            new SelectListItem(){Value = "name",Text = "Name"},
        };

        public List<SelectListItem> PageSizes { get; } = new List<SelectListItem>
        {
            new SelectListItem(){Value = "12",Text = "12"},
            new SelectListItem(){Value = "24",Text = "24"},
            new SelectListItem(){Value = "48",Text = "48"},
        };
    }
}
