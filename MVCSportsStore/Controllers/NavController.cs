using System.Linq;
using System.Web.Mvc;
using MVCSportsStore.Domain.Abstract;

namespace MVCSportsStore.Controllers
{
    public class NavController : Controller
    {
        private IProductRepository _productRepository;

        public NavController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public PartialViewResult Menu(string category=null)
        {
            ViewBag.SelectedCategory = category;
            var categories = _productRepository.Products.Select(x => x.Category).Distinct().OrderBy(x => x);
            return PartialView("FlexMenu", categories);
        }
    }
}