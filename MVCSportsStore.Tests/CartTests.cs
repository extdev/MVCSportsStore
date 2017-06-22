using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MVCSportsStore.Domain.Entities;

namespace MVCSportsStore.Tests
{
    [TestClass]
    public class CartTests
    {
        [TestMethod]
        public void CanAddItem()
        {
            Cart c = new Cart();
            
            c.AddItem(GetProductList().First(), 2);
            c.AddItem(GetProductList().Skip(1).Take(1).First(),2);

            var results = c.Lines.ToArray();

            Assert.AreEqual(results.Length, 2);
            Assert.AreEqual(results[0].Product.ProductId, 1);
            Assert.AreEqual(results[1].Product.Name, "Net");

            Assert.AreEqual(c.TotalItems, 4);
            Assert.AreEqual(c.Lines.Count(), 2);
        }

        [TestMethod]
        public void CanAddQuantityForExistingLines()
        {
            Cart c = new Cart();

            c.AddItem(GetProductList().First(), 1);

            var countBeforeAdding = c.Lines.Count(x => x.Product.ProductId == GetProductList().First().ProductId);

            c.AddItem(GetProductList().First(), 2);

            var countAfterAdding = c.Lines.Count(x => x.Product.ProductId == GetProductList().First().ProductId);

            var results = c.Lines.ToArray();

            Assert.AreEqual(results.Length, 1);
            Assert.AreEqual(results[0].Product.ProductId, 1);
            Assert.AreEqual(results[0].Quantity, 3);
            Assert.AreEqual(countBeforeAdding, 1);
            Assert.AreEqual(countAfterAdding, 1);
        }

        [TestMethod]
        public void CartComputeTotalValue()
        {
            Cart c = new Cart();

            c.AddItem(GetProductList().First(), 1);
            c.AddItem(GetProductList().Skip(1).Take(1).First(), 2);

            var results = c.Lines.ToArray();

            Assert.AreEqual(results.Length, 2);

            Assert.AreEqual(c.TotalItems, 3);
            Assert.AreEqual(c.ComputeTotalValue(), 50);
        }

        [TestMethod]
        public void CanRemoveItems()
        {
            Cart c = new Cart();

            c.AddItem(GetProductList().First(), 1);
            c.AddItem(GetProductList().Skip(1).Take(1).First(), 3);

            c.RemoveLine(GetProductList().First());

            var results = c.Lines.ToArray();

            Assert.AreEqual(results.Length, 1);
            Assert.AreEqual(results[0].Product.ProductId, 2);
            Assert.AreEqual(results[0].Product.Name, "Net");

            Assert.AreEqual(c.TotalItems, 3);
            Assert.AreEqual(c.Lines.Count(), 1);
        }

        [TestMethod]
        public void CanClearItems()
        {
            Cart c = new Cart();

            c.AddItem(GetProductList().First(), 2);
            c.AddItem(GetProductList().Skip(1).Take(1).First(), 2);

            var countBeforeDeleting = c.Lines.Count();

            c.Clear();

            var results = c.Lines.ToArray();

            Assert.AreEqual(results.Length, 0);
            Assert.AreEqual(countBeforeDeleting, 2);
            Assert.AreEqual(c.ComputeTotalValue(), 0);
        }


        private IEnumerable<Product> GetProductList()
        {
            return new List<Product>
            {
               new Product
                {
                    Name = "Soccer ball",
                    Price = 10,
                    Category = "Soccer",
                    ProductId = 1
                },
                new Product
                {
                    Name = "Net",
                    Price = 20,
                    Category = "Soccer",
                    ProductId = 2
                }
            };
        }


    }
}
