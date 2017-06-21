using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MVCSportsStore.Controllers;
using MVCSportsStore.Domain.Abstract;
using MVCSportsStore.Domain.Entities;
using MVCSportsStore.HtmlHelpers;
using MVCSportsStore.Models;

namespace MVCSportsStore.Tests
{
    [TestClass]
    public class ProductControllerTests
    {
        [TestMethod]
        public void List_Can_Paginate()
        {
            //Arrange
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(x => x.Products).Returns(GetMockProductList());

            ProductController controller = new ProductController(mock.Object);
            controller.PageSize = 3;

            //Act
            var result = controller.List(2).Model as IEnumerable<Product>;

            //Assert
            var prodArray = result.ToArray();
            Assert.IsTrue(prodArray.Length == 2);
            Assert.AreEqual(prodArray[0].Name, "P4");
            Assert.AreEqual(prodArray[1].Name, "P5");
        }

        [TestMethod]
        public void Can_Generate_Page_Links()
        {
            //Arrange
            var pagingInfo = new PagingInfo()
            {
                CurrentPage = 2,
                TotalItems = 28,
                ItemsPerPage = 10
            };

            HtmlHelper htmlHelper = null;

            Func<int, string> func = i => "Page" + i;

            //Act
            var result = htmlHelper.PageLinks(pagingInfo, func);

            // Assert
            Assert.AreEqual(@"<a class=""btn btn-default"" href=""Page1"">1</a>"+ 
                                @"<a class=""btn btn-default btn-primary selected"" href=""Page2"">2</a>" + 
                                @"<a class=""btn btn-default"" href=""Page3"">3</a>", result.ToString());
        }

        private IEnumerable<Product> GetMockProductList()
        {
            return new List<Product>
            {
                new Product { ProductId = 1, Name = "P1"},
                new Product { ProductId = 2, Name = "P2"},
                new Product { ProductId = 3, Name = "P3"},
                new Product { ProductId = 4, Name = "P4"},
                new Product { ProductId = 5, Name = "P5"}
            };
        }
    }
}
