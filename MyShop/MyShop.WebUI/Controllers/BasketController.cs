using MyShop.Core.Contracts;
using MyShop.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyShop.WebUI.Controllers
{
    public class BasketController : Controller
    {
        IBasketService basketService;
        IOrderService Orderservice;
        IRepository<Customer> customers;
        // GET: Basket

            public BasketController(IBasketService BasketService, IOrderService Orderservice, IRepository<Customer> customers)
        {
            this.basketService = BasketService;
            this.Orderservice = Orderservice;
            this.customers = customers;
        }
        public ActionResult Index()
        {
            var model = basketService.GetBasketItems(this.HttpContext);
            return View(model);
        }

        public ActionResult AddToBasket(string Id)
        {
            basketService.AddToBasket(this.HttpContext, Id);

            return RedirectToAction("Index");
        }

        public ActionResult RemoveFromBasket(string Id)
        {
            basketService.RemoveFromBasket(this.HttpContext, Id);

            return RedirectToAction("Index");
        }


        public PartialViewResult BasketSummary()
        {
            var model = basketService.GetBasketSummary(this.HttpContext);

            return PartialView(model);
        }

        [Authorize]
        public ActionResult Checkout()
        {
            Customer customer = customers.Collection().FirstOrDefault(c => c.Email == User.Identity.Name);

            if(customer != null)
            {
                Order order = new Order()
                {
                    Email = customer.Email,
                    City = customer.City,
                    State = customer.State,
                    Street = customer.Street,
                    FirstName = customer.FirstName,
                    Surname = customer.LastName,
                    ZipCode = customer.ZipCode

                };

                 return View(order);
            }
            else
            {
                return RedirectToAction("Error");
            }
            
        }

        [HttpPost]
        [Authorize]
        public ActionResult Checkout(Order order) 
        {
            var basketItems = basketService.GetBasketItems(this.HttpContext);
            order.OrderStatus = "Order Created";
            order.Email = User.Identity.Name;

            //Process Payment

            order.OrderStatus = "Payment Processed";
            Orderservice.CreateOrder(order, basketItems);
            basketService.ClearBasket(this.HttpContext);

            return RedirectToAction("Thankyou", new { OrderId = order.Id });
        }


        public ActionResult Thankyou(string OrderId)
        {
            ViewBag.OrderId = OrderId;
            return View();
        }




        }
}