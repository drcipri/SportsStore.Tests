using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using SportsStore.Models;
using SportsStore.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Mvc.Abstractions;

namespace SportsStore.Tests
{
    [TestFixture]
    internal class CartPageTests
    {
        [Test]
        public void OnGet_CanLoadTheCart_ReturnExistingCartObjectsToTheRazorView()
        {
            //Arange
            //-create a mock repository
            var p1 = new Product { ProductId = 1, Name = "P1" };
            var p2 = new Product { ProductId = 2, Name = "P2" };
            Mock<IStoreRepository> mockRepo = new Mock<IStoreRepository>();
            mockRepo.Setup(m => m.Products).Returns((new Product[]
            {
                p1,
                p2
            }).AsQueryable<Product>());

            //create a cart
            Cart testCart = new Cart();
            testCart.AddItem(p1,2);
            testCart.AddItem(p2,2);

            //Action
            CartModel cartModel = new CartModel(mockRepo.Object, testCart);
            cartModel.OnGet("myUrl");

            //assert
            Assert.That(cartModel.Cart?.Lines, Has.Count.EqualTo(2));
            Assert.That(cartModel.ReturnUrl, Is.EqualTo("myUrl"));
        }

        [Test]
        public void OnPost_CanUpdateTheCart_ReturnTheCartWithTheNewObjects()
        {
            //arrange
            Mock<IStoreRepository> mockRepo = new Mock<IStoreRepository>();
            mockRepo.Setup(m => m.Products).Returns((new Product[]
            {
                new Product{ProductId = 1, Name = "P1"},
            }).AsQueryable<Product>());

            Cart? testCart = new Cart();

            //action
            CartModel cartModel = new CartModel(mockRepo.Object, testCart);
            cartModel.OnPost(1, "myUrl");

            //assert
            Assert.That(testCart.Lines, Has.Count.EqualTo(1));
            Assert.That(testCart.Lines.First().Product.Name, Is.EqualTo("P1"));
            Assert.That(testCart.Lines.First().Quantity, Is.EqualTo(1));
        }
    }
}
