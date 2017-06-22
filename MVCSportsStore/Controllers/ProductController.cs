using System.Linq;
using System.Web.Mvc;
using MVCSportsStore.Domain.Abstract;
using MVCSportsStore.Models;

namespace MVCSportsStore.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductRepository _productRepository;
        public int PageSize = 4;

        public ProductController(IProductRepository productRepository)
        {
            _productRepository = productRepository;            
        }
        // GET: Product
        public ActionResult Index()
        {
            return View();
        }

        public ViewResult List(string category, int page = 1)
        {
            var model = new ProductListViewModel()
            {
                Products = _productRepository
                .Products
                .Where(x => x.Category == category || category == null)
                .OrderBy(p => p.ProductId)
                .Skip((page - 1) * PageSize)
                .Take(PageSize),

                PagingInfo = new PagingInfo()
                {
                    CurrentPage = page,
                    ItemsPerPage = PageSize,
                    TotalItems = category == null ? _productRepository.Products.Count() : _productRepository.Products.Count(x => x.Category == category)
                },
                CurrentCategory = category
            };

            return View(model);
        }
    }
}