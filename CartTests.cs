using SportsStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsStore.Tests
{
    [TestFixture]
    internal class CartTests
    {
        private Cart _cart;
        [SetUp]
        public void Setup()
        {
            _cart = new Cart();
        }
        [Test]
        public void AddItem_ProductIsNotInTheCart_ProductIsAddedToCart()
        {
            //arrange
            var product1 = new Product { ProductId = 1, Name = "P1" };
            var product2 = new Product { ProductId = 2, Name = "P2" };

            _cart.AddItem(product1, 1);
            _cart.AddItem(product2, 1);

            //act
            var result = _cart.Lines.ToArray();

            //Assert
            Assert.Multiple(() =>
                {
                    Assert.That(result[0].Product.Name, Is.EqualTo("P1"));
                    Assert.That(result[1].Product.Name, Is.EqualTo("P2"));

                    Assert.That(result[0].Quantity, Is.EqualTo(1));
                    Assert.That(result[1].Quantity, Is.EqualTo(1));
                }
            );

        }

        [Test]
        public void AddItem_ProductIsInTheCart_QuantityIsIncreased()
        {
            //arrange
            var product1 = new Product { ProductId = 1, Name = "P1" };
            var product2 = new Product { ProductId = 2, Name = "P2" };


            _cart.AddItem(product1, 1);
            _cart.AddItem(product2, 1);

            //act
            _cart.AddItem(product1, 1);
            _cart.AddItem(product2, 1);
            _cart.AddItem(product2, 2);
            var result = _cart.Lines.ToArray();

            //Assert
            Assert.Multiple(() =>
                {
                    Assert.That(result, Has.Length.EqualTo(2));
                    Assert.That(result[0].Product.Name, Is.EqualTo("P1"));
                    Assert.That(result[1].Product.Name, Is.EqualTo("P2"));

                    Assert.That(result[0].Quantity, Is.EqualTo(2));
                    Assert.That(result[1].Quantity, Is.EqualTo(4));
                }
            );

        }
        [Test]
        public void RemoveLine_RemoveSelectedProduct_ProductIsNotInCart()
        {
            //arrange
            var product1 = new Product { ProductId = 1, Name = "P1" };
            var product2 = new Product { ProductId = 2, Name = "P2" };
            var product3 = new Product { ProductId = 3, Name = "P3" };
            var product4 = new Product { ProductId = 4, Name = "P4" };

            _cart.AddItem(product1, 2);
            _cart.AddItem(product2, 3);
            _cart.AddItem(product3, 5);
            _cart.AddItem(product4, 10);

            //act
            _cart.RemoveLine(product2);
            var result = _cart.Lines.ToArray();

            //Assert
            Assert.Multiple(() =>
                {
                    Assert.That(result, Has.Length.EqualTo(3));
                    Assert.That(result[0].Quantity, Is.EqualTo(2));
                    Assert.That(result[1].Quantity, Is.EqualTo(5));
                    Assert.That(result[2].Quantity, Is.EqualTo(10));
                    Assert.That(result.Where(x => x.Product == product2), Is.Empty);
                }
            );

        }

        [Test]
        public void ComputedValue_CalculateFinalPriceForCart_ReturnFinalPriceOfAllProductsAndTheirQuanityt()
        {
            //arrange
            var product1 = new Product { ProductId = 1, Name = "P1", Price = 20.50m };
            var product2 = new Product { ProductId = 2, Name = "P2", Price = 50.20m };
            var product3 = new Product { ProductId = 3, Name = "P3", Price = 35.33m };
            var product4 = new Product { ProductId = 4, Name = "P4", Price = 5.67m };

            _cart.AddItem(product1, 2);
            _cart.AddItem(product2, 3);
            _cart.AddItem(product3, 5);
            _cart.AddItem(product4, 10);

            //act
            var result = _cart.ComputeTotalValue();
            var expectedPrice = (20.50m * 2) + (50.20m * 3) + (35.33m * 5) + (5.67m * 10);

            //Assert
            Assert.That(result, Is.EqualTo(expectedPrice));
        }

        [Test]
        public void Clear_ClearTheCart_CartIsEmpty()
        {
            //arrange
            var product1 = new Product { ProductId = 1, Name = "P1", Price = 20.50m };
            var product2 = new Product { ProductId = 2, Name = "P2", Price = 50.20m };
            var product3 = new Product { ProductId = 3, Name = "P3", Price = 35.33m };
            var product4 = new Product { ProductId = 4, Name = "P4", Price = 5.67m };

            _cart.AddItem(product1, 2);
            _cart.AddItem(product2, 3);
            _cart.AddItem(product3, 5);
            _cart.AddItem(product4, 10);

            //act
            _cart.Clear();

            //Assert
            Assert.That(_cart.Lines, Has.Count.EqualTo(0));
        }
    }
}
