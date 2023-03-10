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
    internal class HomeControllerTests
    {
        [Test]
        public void Index_SendRepositoryDataToView_ResultsAreTheSameLikeInTheDatabase()
        {
            //arange
            Mock<IStoreRepository> mock = new Mock<IStoreRepository>();
            mock.Setup(m => m.Products).Returns((new Product[]
            {
                new Product{ProductId = 1, Name = "P1"},
                new Product{ProductId = 2, Name = "P2"}
            }).AsQueryable<Product>());

            HomeController controller = new (mock.Object);

            //act
            var result = (controller.Index(null) as ViewResult)?.ViewData.Model as ProductsListViewModel ?? new();

            //Assert
            Product[] productArray = result?.Products.ToArray() ?? Array.Empty<Product>();
            Assert.That(productArray.Length, Is.EqualTo(2));
            Assert.That(productArray[0].Name, Is.EqualTo("P1"));
            Assert.That(productArray[1].Name, Is.EqualTo("P2"));
        }
        
        
        [Test]
        public void Index_PaginateTheDataSendedToView_ResultAreTheSameLikeInTheDatabase()
        {
            //Arrange
            Mock<IStoreRepository> mock = new Mock<IStoreRepository>();
            mock.Setup(m => m.Products).Returns((new Product[]
            {
                new Product{ProductId = 1, Name = "P1"},
                new Product{ProductId = 2, Name = "P2"},
                new Product{ProductId = 3, Name = "P3"},
                new Product{ProductId = 4, Name = "P4"},
                new Product{ProductId = 5, Name = "P5"},
            }).AsQueryable<Product>());

            HomeController controller = new(mock.Object);
            controller.PageSize = 3;

            //act
            var result = (controller.Index(null,2) as ViewResult)?.ViewData.Model as ProductsListViewModel ?? new();

            //assert
            Product[] prodArray = result.Products.ToArray();
            Assert.That(prodArray.Length, Is.EqualTo(2));
            Assert.That(prodArray[0].Name, Is.EqualTo("P4"));
            Assert.That(prodArray[1].Name, Is.EqualTo("P5"));
        }

        [Test]
        public void Index_PaginateDataThroughProductsListViewModelObject_PagingInfoResultsAreTheSameAsInTHeView()
        {
            //Arrage Repository
            var mock = new Mock<IStoreRepository>();
            mock.Setup(m => m.Products).Returns((new Product[]
            {
                new Product{ProductId = 1, Name = "P1"},
                new Product{ProductId = 2, Name = "P2"},
                new Product{ProductId = 3, Name = "P3"},
                new Product{ProductId = 4, Name = "P4"},
                new Product{ProductId = 5, Name = "P5"},
            }).AsQueryable<Product>());

            var homeController = new HomeController(mock.Object) {PageSize = 3};

            //act
            var result = (homeController.Index(null,2) as ViewResult)?.ViewData.Model as ProductsListViewModel ?? new();

            //assert
            Assert.That(result.PagingInfo.CurrentPage, Is.EqualTo(2));
            Assert.That(result.PagingInfo.ItemsPerPage, Is.EqualTo(3));
            Assert.That(result.PagingInfo.TotalPages, Is.EqualTo(2));
            Assert.That(result.PagingInfo.TotalItems, Is.EqualTo(5));
        }

        [Test]
        public void Index_FilterProductsByCategory_ResultsAreTHeSameLikeInTHeDatabase()
        {
            //arrange
            var mock = new Mock<IStoreRepository>();
            mock.Setup(m => m.Products).Returns((new Product[]
            {
                new Product{ProductId = 1, Name = "P1", Category = "Cat1"},
                new Product{ProductId = 2, Name = "P2", Category = "Cat2"},
                new Product{ProductId = 3, Name = "P3", Category = "Cat2"},
                new Product{ProductId = 4, Name = "P4", Category = "Cat1"},
                new Product{ProductId = 5, Name = "P5", Category = "Cat3"},
            }).AsQueryable<Product>());
            var homeController = new HomeController(mock.Object);

            //Act
            var result = (homeController.Index("Cat1") as ViewResult)?.ViewData.Model as ProductsListViewModel ?? new();

            //Arrange
            Product[] products = result.Products.ToArray();
            Assert.That(products.Length, Is.EqualTo(2));
            Assert.That(products[0].Name, Is.EqualTo("P1"));
            Assert.That(products[0].Category, Is.EqualTo("Cat1"));
            Assert.That(products[1].Name, Is.EqualTo("P4"));
            Assert.That(products[1].Category, Is.EqualTo("Cat1"));
        }

        [Test]
        public void Index_GenerateCategorySpecificProducts_ReturnsCountOfSpecifcCategory()
        {
            //Arrange
            var mock = new Mock<IStoreRepository>();
            mock.Setup(m => m.Products).Returns((new Product[]
            {
                new Product{ProductId = 1, Name = "P1", Category = "Cat1"},
                new Product{ProductId = 2, Name = "P2", Category = "Cat2"},
                new Product{ProductId = 3, Name = "P3", Category = "Cat2"},
                new Product{ProductId = 4, Name = "P4", Category = "Cat1"},
                new Product{ProductId = 5, Name = "P5", Category = "Cat3"},
            }).AsQueryable<Product>());
            var homeController = new HomeController(mock.Object);
            homeController.PageSize = 3;

            Func<ViewResult?, ProductsListViewModel?> GetModel = result => result?.ViewData?.Model as ProductsListViewModel;

            //act
            int? res1 = GetModel(homeController.Index("Cat1") as ViewResult)?.PagingInfo.TotalItems;
            int? res2 = GetModel(homeController.Index("Cat2") as ViewResult)?.PagingInfo.TotalItems;
            int? res3 = GetModel(homeController.Index("Cat3") as ViewResult)?.PagingInfo.TotalItems;
            int? resAll = GetModel(homeController.Index(null) as ViewResult)?.PagingInfo.TotalItems;

            //assert
            Assert.That(res1, Is.EqualTo(2));
            Assert.That(res2, Is.EqualTo(2));
            Assert.That(res3, Is.EqualTo(1));
            Assert.That(resAll, Is.EqualTo(5));

        }
    }
}
