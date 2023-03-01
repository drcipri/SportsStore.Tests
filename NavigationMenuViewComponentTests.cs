using Microsoft.AspNetCore.Mvc;
using SportsStore.Controllers;
using SportsStore.Models.ViewModels;
using SportsStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SportsStore.Components;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Components;

namespace SportsStore.Tests
{
    [TestFixture]
    internal class NavigationMenuViewComponentTests
    {
        [Test]
        public void Invoke_CanGetCategoriesFromDatabase_ResultValueAreTheSameLikeInTheDatabase()
        {
            //arrange
            var mock = new Mock<IStoreRepository>();
            mock.Setup(m => m.Products).Returns((new Product[]
            {
                new Product{ProductId = 1, Name = "P1", Category = "Apples"},
                new Product{ProductId = 2, Name = "P2", Category = "Oranges"},
                new Product{ProductId = 3, Name = "P3", Category = "Banana"},
                new Product{ProductId = 4, Name = "P4", Category = "Apples"},
                new Product{ProductId = 5, Name = "P5", Category = "Apples"},
                new Product{ProductId = 5, Name = "P5", Category = "Oranges"},
            }).AsQueryable<Product>());
            var navMenu = new NavigationMenuViewComponent(mock.Object);

            //Act
            string[] results = ((IEnumerable<string>?)(navMenu.Invoke() as ViewViewComponentResult)?.ViewData?.Model ?? Enumerable.Empty<string>()).ToArray();


            //assert
            string[] testCollection = new string[] { "Apples", "Banana", "Oranges" };
            CollectionAssert.AreEqual(testCollection, results);
        }
       
        [Test]
        public void Invoke_IndicateSelectedCategory_ReturnSelectedCategory()
        {
            //Arrange
            string categorySelect = "Apples";
            var mock = new Mock<IStoreRepository>();
            mock.Setup(m => m.Products).Returns((new Product[]
            {
                new Product{ProductId = 1, Name = "P1", Category = "Apples"},
                new Product{ProductId = 2, Name = "P2", Category = "Oranges"},
            }).AsQueryable<Product>());
            var navMenu = new NavigationMenuViewComponent(mock.Object);
            navMenu.ViewComponentContext = new ViewComponentContext { ViewContext = new ViewContext() { RouteData = new Microsoft.AspNetCore.Routing.RouteData() } };
            navMenu.RouteData.Values["category"] = categorySelect;

            //Act
            string? result = (string?)(navMenu.Invoke() as ViewViewComponentResult)?.ViewData?["SelectedCategory"];  

            //Assert
            Assert.That(result,Is.EqualTo(categorySelect));

        }
    }
}
