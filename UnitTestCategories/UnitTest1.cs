using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SneakerStore.Controllers;
using SneakerStore.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;




namespace SneakerStore.Tests.Controllers
{
    [TestClass]
    public class CategoriesControllerTest
    {


        [TestMethod]
        public void Create_ExceptionThrown_ReturnsContentWithError()
        {
            // Arrange
            var category = new Category { IDCate = 1, NameCate = "" };
            var mockDbContext = new Mock<DBSneakerStoreEntities>();
            mockDbContext.Setup(db => db.Categories.Add(It.IsAny<Category>())).Throws(new Exception());
            var controller = new CategoriesController();
            controller.Database = mockDbContext.Object;

            // Act
            var result = controller.Create(category) as System.Web.Mvc.ContentResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Lỗi tạo mới", result.Content);
        }

        [TestMethod]
        public void Delete_ExceptionThrown_ReturnsContentWithError()
        {
            // Arrange
            var categoryId = 2 ;
            var categoryToRemove = new Category { IDCate = categoryId, NameCate = "Test" };
            var mockDbContext = new Mock<DBSneakerStoreEntities>();
            mockDbContext.Setup(db => db.Categories.Remove(It.IsAny<Category>())).Throws(new Exception());
            var controller = new CategoriesController();
            controller.Database = mockDbContext.Object;

            // Act
            var result = controller.Delete(categoryId, categoryToRemove) as System.Web.Mvc.ContentResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Dữ liệu đang được sử dụng ở nơi khác, Lỗi!", result.Content);
        }

        [TestMethod]
        public void Create_InvalidOrder_ReturnsViewWithError()
        {

            // Arrange
            var mockDb = new Mock<DBSneakerStoreEntities>();
            var controller = new OrdersController();
            controller.Database = mockDb.Object;

            OrderPro order = new OrderPro(); // Missing required properties to make it invalid

            // Act
            var result = controller.Create(order) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(order, result.Model); // Verify that the model is passed back to the view
        }



        public class OrderDetailControllerTests
        {
            private Mock<DBSneakerStoreEntities> _mockContext;
            private Mock<DbSet<OrderDetail>> _mockOrderDetailSet;
            private Mock<DbSet<Product>> _mockProductSet;

            [TestInitialize]
            public void Setup()
            {
                _mockContext = new Mock<DBSneakerStoreEntities>();
                _mockOrderDetailSet = new Mock<DbSet<OrderDetail>>();
                _mockProductSet = new Mock<DbSet<Product>>();

                // Thiết lập dữ liệu mẫu
                var orderDetails = new List<OrderDetail>
        {
            new OrderDetail { IDProduct = 1, Quantity = 10 },
            new OrderDetail { IDProduct = 1, Quantity = 5 },
            new OrderDetail { IDProduct = 2, Quantity = 7 }
        }.AsQueryable();

                var products = new List<Product>
        {
            new Product { ProductID = 1, NamePro = "Product 1", ImagePro = "Image1.jpg", Price = 100 },
            new Product { ProductID = 2, NamePro = "Product 2", ImagePro = "Image2.jpg", Price = 200 }
        }.AsQueryable();

                // Thiết lập Mock cho DbSet<OrderDetail>
                _mockOrderDetailSet.As<IQueryable<OrderDetail>>().Setup(m => m.Provider).Returns(orderDetails.Provider);
                _mockOrderDetailSet.As<IQueryable<OrderDetail>>().Setup(m => m.Expression).Returns(orderDetails.Expression);
                _mockOrderDetailSet.As<IQueryable<OrderDetail>>().Setup(m => m.ElementType).Returns(orderDetails.ElementType);
                _mockOrderDetailSet.As<IQueryable<OrderDetail>>().Setup(m => m.GetEnumerator()).Returns(orderDetails.GetEnumerator());

                // Thiết lập Mock cho DbSet<Product>
                _mockProductSet.As<IQueryable<Product>>().Setup(m => m.Provider).Returns(products.Provider);
                _mockProductSet.As<IQueryable<Product>>().Setup(m => m.Expression).Returns(products.Expression);
                _mockProductSet.As<IQueryable<Product>>().Setup(m => m.ElementType).Returns(products.ElementType);
                _mockProductSet.As<IQueryable<Product>>().Setup(m => m.GetEnumerator()).Returns(products.GetEnumerator());

                // Thiết lập Mock cho DbContext
                _mockContext.Setup(c => c.OrderDetails).Returns(_mockOrderDetailSet.Object);
                _mockContext.Setup(c => c.Products).Returns(_mockProductSet.Object);
            }

            [TestMethod]
            public void GroupByTop_ReturnsTop8Products()
            {
                // Arrange
                var controller = new OrderDetailController
                {
                };

                // Act
                var result = controller.GroupByTop() as ViewResult;
                var model = result.Model as List<ViewModel>;

                // Assert
                Assert.IsNotNull(result);
                Assert.AreEqual(2, model.Count);

                Assert.AreEqual(1, model[0].IDPro);
                Assert.AreEqual("Product 1", model[0].NamePro);
                Assert.AreEqual("Image1.jpg", model[0].ImgPro);
                Assert.AreEqual(100, model[0].PricePro);
                Assert.AreEqual(15, model[0].Sum_Quantity);

                Assert.AreEqual(2, model[1].IDPro);
                Assert.AreEqual("Product 2", model[1].NamePro);
                Assert.AreEqual("Image2.jpg", model[1].ImgPro);
                Assert.AreEqual(200, model[1].PricePro);
                Assert.AreEqual(7, model[1].Sum_Quantity);
            }
        }


    }

    
}
