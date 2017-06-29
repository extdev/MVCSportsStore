using System.Linq;
using System.Web.Mvc;
using MVCSportsStore.Domain.Abstract;
using MVCSportsStore.Domain.Entities;
using MVCSportsStore.Models;

namespace MVCSportsStore.Controllers
{
    public class CartController: Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly IOrderProcessor _orderProcessor;

        public CartController(IProductRepository productRepository, IOrderProcessor orderProcessor)
        {
            _productRepository = productRepository;
            _orderProcessor = orderProcessor;
        }

        public ViewResult Index(Cart cart, string returnUrl)
        {
            CartIndexViewModel vm = new CartIndexViewModel 
            {
                Cart = cart, 
                ReturnUrl = returnUrl
            };

            return View(vm);
        }

        public RedirectToRouteResult AddToCart(Cart cart, int productId, string returnUrl)
        {
            Product p = _productRepository.Products.FirstOrDefault(x => x.ProductId == productId);

            if(p != null)
            {
                cart.AddItem(p, 1);
            }
            return RedirectToAction("Index", new { returnUrl });
        }

        public RedirectToRouteResult RemoveFromCart(Cart cart, int productId, string returnUrl)
        {
            Product p = _productRepository.Products.FirstOrDefault(x => x.ProductId == productId);

            if (p != null)
            {
                cart.RemoveLine(p);
            }
            return RedirectToAction("Index", new { returnUrl });
        }

        public PartialViewResult Summary(Cart cart)
        {
            return PartialView(cart);
        }

        public ViewResult Checkout(Cart cart)
        {
           return View(new ShippingDetails());
        }

        [HttpPost]
        public ViewResult Checkout(Cart cart, ShippingDetails shippingDetails)
        {
            if (!cart.Lines.Any())
            {
                ModelState.AddModelError("", "Sorry, your cart is empty!");
            }

            if (ModelState.IsValid)
            {
                _orderProcessor.ProcessOrder(cart, shippingDetails);
                cart.Clear();
                return View("Completed");
            }

            return View(shippingDetails);
        }
    }
}