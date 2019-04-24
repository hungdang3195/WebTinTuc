using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ShopOnlineApp.Application.Interfaces;
using ShopOnlineApp.Extensions;

namespace ShopOnlineApp.Controllers
{
    public class OrderController : Controller
    {
        private readonly IBillService _billService;
        public OrderController(IBillService billService)
        {
            _billService = billService;
        }
        [Route("orders-list.html")]
        public IActionResult Index()
        {
            var orderList = _billService.GetOrdersByCustomer(User.GetUserId());
            return View(orderList);
        }

        [Route("order.{id}.html")]
        public IActionResult Detail(int id)
        {
            var orderDetail = _billService.GetDetail(id);
            return View(orderDetail);
        }
    }
}