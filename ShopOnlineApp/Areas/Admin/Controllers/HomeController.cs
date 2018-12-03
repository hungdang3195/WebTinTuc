using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopOnlineApp.Extensions;

namespace ShopOnlineApp.Areas.Admin.Controllers
{
   
    public class HomeController : BaseController
    {
        public IActionResult Index()
        {
            var getInfo = User.GetSpecificDefault("Email");

            return View();
        }
    }
}