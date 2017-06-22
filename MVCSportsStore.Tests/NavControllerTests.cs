using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MVCSportsStore.Controllers;
using MVCSportsStore.Domain.Abstract;
using MVCSportsStore.Domain.Entities;

namespace MVCSportsStore.Tests
{
    [TestClass]
    public class NavControllerTests
    {
        [TestMethod]
        public void Navigation_Menu_ReturnsCategories()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(x => x.Products).Returns(GetMockProductList);

            NavController controller = new NavController(mock.Object);
            var result = controller.Menu().Model as IEnumerable<string>;

            Assert.IsNotNull(result);
            var resultAsArray = result.ToArray();
            Assert.AreEqual(resultAsArray.Count(), 3);
            Assert.AreEqual(resultAsArray.ToArray()[1], "Chess");
        }

        [TestMethod]
        public void NavController_SelectedCategory_ReturnsCorrectly()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(x => x.Products).Returns(GetMockProductList);

            NavController controller = new NavController(mock.Object);
            var result = controller.Menu("category").ViewBag.SelectedCategory;

            Assert.IsNotNull(result);
            Assert.AreEqual(result,"category");

        }

        private IEnumerable<Product> GetMockProductList()
        {
            return new List<Product>
            {
                new Product { ProductId = 1, Name = "P1", Category = "Soccer"},
                new Product { ProductId = 2, Name = "P2", Category = "Chess"},
                new Product { ProductId = 3, Name = "Football"},
                new Product { ProductId = 4, Name = "Soccer"}
            };
        }
    }
}
