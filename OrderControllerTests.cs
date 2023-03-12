using SportsStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SportsStore.Controllers;
using Microsoft.AspNetCore.Mvc;
using SportsStore.Models.ViewModels;

namespace SportsStore.Tests
{
    [TestFixture]
    internal class OrderControllerTests
    {
        [Test]
        public void Checkout_CheckOutAnEmptyCart_ReturnInforThatCartIsEmpty()
        {
            //Arrange-repository
            var mockRepository = new Mock<IOrderRepository>();

            //Arrange-cart
            var cart = new Cart();

            //Arrange-order
            var order = new Order();

            var controller = new OrderController(mockRepository.Object,cart);

            //act
           var result = controller.Checkout(order) as ViewResult;

            //assert
            mockRepository.Verify(m => m.SaveOrder(It.IsAny<Order>()), Times.Never());
            Assert.That(result?.ViewName, Is.Null.Or.Empty);
            Assert.That(result?.ViewData.ModelState, Is.Not.True);

        }

        [Test]
        public void Checkout_OrderIsNotValid_ReturnTheDefaultView()
        {
            //Arrange-repository
            var mockRepository = new Mock<IOrderRepository>();

            //Arrange-cart
            var cart = new Cart();
            cart.AddItem(new Product(), 1);

            //Arrange-order
            var order = new Order();

            var controller = new OrderController(mockRepository.Object, cart);
            controller.ModelState.AddModelError("error", "error");

            //act
            var result = controller.Checkout(order) as ViewResult;

            //assert
            mockRepository.Verify(m => m.SaveOrder(It.IsAny<Order>()), Times.Never());
            Assert.That(result?.ViewName, Is.Null.Or.Empty);
            Assert.That(result?.ViewData.ModelState, Is.Not.True);

        }
        [Test]
        public void Checkout_OrderIsSuccesfull_OrderIsSubmited()
        {
            //Arrange-repository
            var mockRepository = new Mock<IOrderRepository>();

            //Arrange-cart
            var cart = new Cart();
            cart.AddItem(new Product(), 1);

            //Arrange-order
            var order = new Order();

            var controller = new OrderController(mockRepository.Object, cart);


            //act
            var result = controller.Checkout(order) as RedirectToPageResult;

            //assert
            mockRepository.Verify(m => m.SaveOrder(It.IsAny<Order>()), Times.Once);
   
            Assert.That(result?.PageName, Is.EqualTo("/Completed"));

        }

    }
}
