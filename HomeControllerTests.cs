using SportsStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SportsStore.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace SportsStore.Tests
{
    [TestFixture]
    internal class HomeControllerTests
    {
        [Test]
        public void HomeController_CanUseRepository_MultipleScenarios()
        {
            //arange
            Mock<IStoreRepository> mock = new Mock<IStoreRepository>();
            mock.Setup(m => m.Products).Returns((new Product[]
            {
                new Product{ProductId = 1, Name = "P1"},
                new Product{ProductId = 2, Name = "P2"}
            }).AsQueryable<Product>());

            HomeController controller = new HomeController(mock.Object);

            //act
            IEnumerable<Product>? result = (controller.Index() as ViewResult)?.ViewData.Model as IEnumerable<Product>;

            //Assert
            Product[] productArray = result?.ToArray() ?? Array.Empty<Product>();
            Assert.That(productArray.Length, Is.EqualTo(2));
            Assert.That(productArray[0].Name, Is.EqualTo("P1"));
            Assert.That(productArray[1].Name, Is.EqualTo("P2"));
        }
    }
}
