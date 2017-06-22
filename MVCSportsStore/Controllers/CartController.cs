using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using MVCSportsStore.Domain.Abstract;
using MVCSportsStore.Domain.Entities;
using MVCSportsStore.Models;

namespace MVCSportsStore.Controllers
{
    public class CartController: Controller
    {
        private readonly IProductRepository _productRepository;

        public CartController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public ViewResult Index(string returnUrl)
        {
            CartIndexViewModel vm = new CartIndexViewModel 
            {
                Cart = GetCart(), 
                ReturnUrl = returnUrl
            };

            return View(vm);
        }

        public RedirectToRouteResult AddToCart(int productId, string returnUrl)
        {
            Product p = _productRepository.Products.FirstOrDefault(x => x.ProductId == productId);

            if(p != null)
            {
                GetCart().AddItem(p, 1);
            }
            return RedirectToAction("Index", new { returnUrl });
        }

        public RedirectToRouteResult RemoveFromCart(int productId, string returnUrl)
        {
            Product p = _productRepository.Products.FirstOrDefault(x => x.ProductId == productId);

            if (p != null)
            {
                GetCart().RemoveLine(p);
            }
            return RedirectToAction("Index", new { returnUrl });
        }

        private Cart GetCart()
        {
            Cart c = Session["cart"] as Cart;
            if (c == null)
            {
                c = new Cart();
                Session["Cart"] = c;
            }
            return c;
        }
    }
}