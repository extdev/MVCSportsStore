using System;
using System.Linq;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MVCSportsStore.Controllers;
using MVCSportsStore.Domain.Abstract;
using MVCSportsStore.Domain.Entities;
using MVCSportsStore.Models;

namespace MVCSportsStore.Tests
{
    [TestClass]
    public class CartControllerTests
    {
        [TestMethod]
        public void AddToCart_AddsProduct()
        {
            Cart cart = new Cart();
            Mock<IProductRepository> repo = new Mock<IProductRepository>();
            repo.Setup(x => x.Products).Returns(new []
            {
                new Product
                {
                    ProductId = 1,
                    Category = "Soccer",
                    Price = 20
                }
            });

            //Act
            CartController c = new CartController(repo.Object, null);
            c.AddToCart(cart, 1, "test");

            //Assert
            Assert.AreEqual(cart.Lines.Count(), 1);
            Assert.AreEqual(cart.Lines.ToArray()[0].Product.ProductId, 1);
        }

        [TestMethod]
        public void AddToCart_OnSuccess_RedirectsToIndexView()
        {
            Cart cart = new Cart();
            Mock<IProductRepository> repo = new Mock<IProductRepository>();
            //repo.Setup(x => x.Products).Returns(new[]
            //{
            //    new Product
            //    {
            //        ProductId = 1,
            //        Category = "Soccer",
            //        Price = 20
            //    }
            //});

            CartController c = new CartController(repo.Object, null);
            var result = c.AddToCart(cart, 1, "test");

            Assert.AreEqual(result.RouteValues["action"], "Index");
            Assert.AreEqual(result.RouteValues["returnUrl"], "test");
        }

        [TestMethod]
        public void Can_View_Cart_Contents()
        {
            // Arrange - create a Cart
            Cart cart = new Cart();
            CartController c = new CartController(null, null);

            // Act - call the Index action method
            CartIndexViewModel result = (CartIndexViewModel)c.Index(cart, "myUrl").ViewData.Model;

            // Assert
            Assert.AreSame(result.Cart, cart);
            Assert.AreEqual(result.ReturnUrl, "myUrl");
        }

        [TestMethod]
        public void Cannot_Checkout_Empty_Cart()
        {
            // Arrange - create a mock order processor
            Mock<IOrderProcessor> mock = new Mock<IOrderProcessor>();
            // Arrange - create an empty cart
            Cart cart = new Cart();
            // Arrange - create shipping details
            ShippingDetails shippingDetails = new ShippingDetails();
            // Arrange - create an instance of the controller
            CartController target = new CartController(null, mock.Object);

            // Act
            ViewResult result = target.Checkout(cart, shippingDetails);

            // Assert - check that the order hasn't been passed on to the processor
            mock.Verify(m => m.ProcessOrder(It.IsAny<Cart>(), It.IsAny<ShippingDetails>()),Times.Never());
            // Assert - check that the method is returning the default view
            Assert.AreEqual("", result.ViewName);
            // Assert - check that I am passing an invalid model to the view
            Assert.AreEqual(false, result.ViewData.ModelState.IsValid);
        }

        [TestMethod]
        public void Cannot_Checkout_Invalid_ShippingDetails()
        {
            // Arrange - create a mock order processor
            Mock<IOrderProcessor> mock = new Mock<IOrderProcessor>();

            // Arrange - create a cart with an item
            Cart cart = new Cart();
            cart.AddItem(new Product(), 1);

            // Arrange - create an instance of the controller
            CartController target = new CartController(null, mock.Object);
            // Arrange - add an error to the model
            target.ModelState.AddModelError("error", "error");

            // Act - try to checkout
            ViewResult result = target.Checkout(cart, new ShippingDetails());
            // Assert - check that the order hasn't been passed on to the processor
            mock.Verify(m => m.ProcessOrder(It.IsAny<Cart>(), It.IsAny<ShippingDetails>()),Times.Never());
            // Assert - check that the method is returning the default view
            Assert.AreEqual("", result.ViewName);
            // Assert - check that I am passing an invalid model to the view
            Assert.AreEqual(false, result.ViewData.ModelState.IsValid);
        }

        [TestMethod]
        public void Can_Checkout_And_Submit_Order()
        {
            // Arrange - create a mock order processor
            Mock<IOrderProcessor> mock = new Mock<IOrderProcessor>();

            // Arrange - create a cart with an item
            Cart cart = new Cart();
            cart.AddItem(new Product(), 1);

            // Arrange - create an instance of the controller
            CartController target = new CartController(null, mock.Object);

            // Act - try to checkout
            ViewResult result = target.Checkout(cart, new ShippingDetails());

            // Assert - check that the order has been passed on to the processor
            mock.Verify(m => m.ProcessOrder(It.IsAny<Cart>(), It.IsAny<ShippingDetails>()),Times.Once());
            // Assert - check that the method is returning the Completed view
            Assert.AreEqual("Completed", result.ViewName);
            // Assert - check that I am passing a valid model to the view
            Assert.AreEqual(true, result.ViewData.ModelState.IsValid);
        }
    }
}
