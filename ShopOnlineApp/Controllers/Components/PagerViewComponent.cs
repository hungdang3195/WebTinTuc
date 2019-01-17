using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ShopOnlineApp.Data.EF.Common;

namespace ShopOnlineApp.Controllers.Components
{
    public class PagerViewComponent : ViewComponent
    {
        public Task<IViewComponentResult> InvokeAsync(ModelListResult<dynamic> result)
        {
            return Task.FromResult((IViewComponentResult)View("Default", result));
        }
    }
}
