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

        public CartController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
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

        public ViewResult Checkout()
        {
            return View(new ShippingDetails());
        }
    }
}