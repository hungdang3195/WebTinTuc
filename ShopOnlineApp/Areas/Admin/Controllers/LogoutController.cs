﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ShopOnlineApp.Areas.Admin.Controllers
{
    public class LogoutController : BaseController
    {
        public IActionResult Index()
        {
            return RedirectToAction("Index", "Login");
        }
    }
}