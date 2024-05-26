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
        private object controller;
        private object result;

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
            var categoryId = 2;
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

        [TestMethod]
        public void Index_NoSearchString_ReturnsAllCategories()
        {
            // Arrange
            var data = new List<Category>
        {
            new Category { IDCate = 1, NameCate = "Sports" },
            new Category { IDCate = 2, NameCate = "Casual" }
        }.AsQueryable();

            var mockSet = new Mock<DbSet<Category>>();
            mockSet.As<IQueryable<Category>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<Category>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<Category>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<Category>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            var mockContext = new Mock<DBSneakerStoreEntities>();
            mockContext.Setup(m => m.Categories).Returns(mockSet.Object);

            var controller = new CategoriesController { Database = mockContext.Object };
        }

        // 

        [TestMethod]
        public void Index_WithSearchString_ReturnsFilteredCategories()
        {
            // Arrange
            var data = new List<Category>
    {
        new Category { IDCate = 1, NameCate = "Sports" },
        new Category { IDCate = 2, NameCate = "Casual" },
        new Category { IDCate = 3, NameCate = "Running" }
    }.AsQueryable();

            var mockSet = new Mock<DbSet<Category>>();
            mockSet.As<IQueryable<Category>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<Category>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<Category>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<Category>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            var mockContext = new Mock<DBSneakerStoreEntities>();
            mockContext.Setup(m => m.Categories).Returns(mockSet.Object);

            var controller = new CategoriesController { Database = mockContext.Object };


        }

        [TestMethod]
        public void Create_PostValidCategory_RedirectsToIndex()
        {
            // Arrange
            var mockSet = new Mock<DbSet<Category>>();
            var mockContext = new Mock<DBSneakerStoreEntities>();
            mockContext.Setup(m => m.Categories).Returns(mockSet.Object);

            var controller = new CategoriesController { Database = mockContext.Object };

        }

        [TestMethod]
        public void Details_ValidId_ReturnsCategory()
        {
            // Arrange
            var data = new List<Category>
    {
        new Category { IDCate = 1, NameCate = "Sports" },
    }.AsQueryable();

            var mockSet = new Mock<DbSet<Category>>();
            mockSet.As<IQueryable<Category>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<Category>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<Category>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<Category>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            var mockContext = new Mock<DBSneakerStoreEntities>();
            mockContext.Setup(m => m.Categories).Returns(mockSet.Object);

            var controller = new CategoriesController { Database = mockContext.Object };

        }

        //


        [TestMethod]
        public void Edit_PostValidCategory_RedirectsToIndex()
        {
            // Arrange
            var mockSet = new Mock<DbSet<Category>>();
            var mockContext = new Mock<DBSneakerStoreEntities>();
            mockContext.Setup(m => m.Categories).Returns(mockSet.Object);

            var controller = new CategoriesController { Database = mockContext.Object };

        }

        [TestMethod]
        public void Delete_PostValidId_RedirectsToIndex()
        {
            // Arrange
            var mockSet = new Mock<DbSet<Category>>();
            var mockContext = new Mock<DBSneakerStoreEntities>();
            var category = new Category { IDCate = 1, NameCate = "Sports" };
            var data = new List<Category> { category }.AsQueryable();

            mockSet.As<IQueryable<Category>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<Category>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<Category>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<Category>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            mockContext.Setup(m => m.Categories).Returns(mockSet.Object);

            var controller = new CategoriesController { Database = mockContext.Object };


        }

        [TestMethod]
        public void CategoryPartial_ReturnsPartialView()
        {
            // Arrange
            var mockSet = new Mock<DbSet<Category>>();
            var mockContext = new Mock<DBSneakerStoreEntities>();
            var categories = new List<Category>
    {
        new Category { IDCate = 1, NameCate = "Sports" },
        new Category { IDCate = 2, NameCate = "Casual" }
    };
            mockSet.As<IQueryable<Category>>().Setup(m => m.Provider).Returns(categories.AsQueryable().Provider);
            mockSet.As<IQueryable<Category>>().Setup(m => m.Expression).Returns(categories.AsQueryable().Expression);
            mockSet.As<IQueryable<Category>>().Setup(m => m.ElementType).Returns(categories.AsQueryable().ElementType);
            mockSet.As<IQueryable<Category>>().Setup(m => m.GetEnumerator()).Returns(categories.AsQueryable().GetEnumerator());

            mockContext.Setup(m => m.Categories).Returns(mockSet.Object);

            var controller = new CategoriesController { Database = mockContext.Object };

         }

        //


        [TestMethod]
        public void Details_ValidId_ReturnsCategory_2()
        {
            // Arrange
            var data = new List<Category>
    {
        new Category { IDCate = 1, NameCate = "Sports" },
    }.AsQueryable();

            var mockSet = new Mock<DbSet<Category>>();
            mockSet.As<IQueryable<Category>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<Category>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<Category>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<Category>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            var mockContext = new Mock<DBSneakerStoreEntities>();
            mockContext.Setup(m => m.Categories).Returns(mockSet.Object);

            var controller = new CategoriesController { Database = mockContext.Object };


        }

        [TestMethod]
        public void Index_NoSearchString_ReturnsAllCategories_2()
        {
            // Arrange
            var data = new List<Category>
    {
        new Category { IDCate = 1, NameCate = "Sports" },
        new Category { IDCate = 2, NameCate = "Casual" }
    }.AsQueryable();

            var mockSet = new Mock<DbSet<Category>>();
            mockSet.As<IQueryable<Category>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<Category>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<Category>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<Category>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            var mockContext = new Mock<DBSneakerStoreEntities>();
            mockContext.Setup(m => m.Categories).Returns(mockSet.Object);

            var controller = new CategoriesController { Database = mockContext.Object };


        }

        [TestMethod]
        public void Index_WithSearchString_ReturnsFilteredCategories_2()
        {
            // Arrange
            var data = new List<Category>
    {
        new Category { IDCate = 1, NameCate = "Sports" },
        new Category { IDCate = 2, NameCate = "Casual" },
        new Category { IDCate = 3, NameCate = "Running" }
    }.AsQueryable();

            var mockSet = new Mock<DbSet<Category>>();
            mockSet.As<IQueryable<Category>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<Category>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<Category>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<Category>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            var mockContext = new Mock<DBSneakerStoreEntities>();
            mockContext.Setup(m => m.Categories).Returns(mockSet.Object);

            var controller = new CategoriesController { Database = mockContext.Object };


        }


        [TestMethod]
        public void Create_ValidCategory_RedirectsToIndex_3()
        {
            // Arrange
            var mockSet = new Mock<DbSet<Category>>();
            var mockContext = new Mock<DBSneakerStoreEntities>();
            mockContext.Setup(m => m.Categories).Returns(mockSet.Object);

            var controller = new CategoriesController { Database = mockContext.Object };


        }

        [TestMethod]
        public void Create_InvalidCategory_ReturnsErrorMessage_3()
        {
            // Arrange
            var mockSet = new Mock<DbSet<Category>>();
            var mockContext = new Mock<DBSneakerStoreEntities>();
            mockContext.Setup(m => m.Categories).Returns(mockSet.Object);

            var controller = new CategoriesController { Database = mockContext.Object };


        }

        [TestMethod]
        public void Edit_NonExistentId_ReturnsNotFound_3()
        {
            // Arrange
            var mockSet = new Mock<DbSet<Category>>();
            var mockContext = new Mock<DBSneakerStoreEntities>();
            var category = new Category { IDCate = 1, NameCate = "Sports" };
            var data = new List<Category>().AsQueryable();

            mockSet.As<IQueryable<Category>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<Category>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<Category>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<Category>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            mockContext.Setup(m => m.Categories).Returns(mockSet.Object);

            var controller = new CategoriesController { Database = mockContext.Object };


        }

        [TestMethod]
        public void Edit_ValidCategory_RedirectsToIndex_3()
        {
            // Arrange
            var mockSet = new Mock<DbSet<Category>>();
            var mockContext = new Mock<DBSneakerStoreEntities>();
            var category = new Category { IDCate = 1, NameCate = "Sports" };
            var data = new List<Category> { category }.AsQueryable();

            mockSet.As<IQueryable<Category>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<Category>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<Category>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<Category>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            mockContext.Setup(m => m.Categories).Returns(mockSet.Object);

            var controller = new CategoriesController { Database = mockContext.Object };


        }

        [TestMethod]
        public void Delete_ValidCategory_RedirectsToIndex_3()
        {
            // Arrange
            var mockSet = new Mock<DbSet<Category>>();
            var mockContext = new Mock<DBSneakerStoreEntities>();
            var category = new Category { IDCate = 1, NameCate = "Sports" };
            var data = new List<Category> { category }.AsQueryable();

            mockSet.As<IQueryable<Category>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<Category>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<Category>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<Category>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            mockContext.Setup(m => m.Categories).Returns(mockSet.Object);

            var controller = new CategoriesController { Database = mockContext.Object };


        }



        [TestMethod]
        public void Index_NoSearchString_ReturnsAllCategories_4()
        {
            // Arrange
            var data = new List<Category>
        {
            new Category { IDCate = 1, NameCate = "Sports" },
            new Category { IDCate = 2, NameCate = "Casual" }
        }.AsQueryable();

            var mockSet = new Mock<DbSet<Category>>();
            mockSet.As<IQueryable<Category>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<Category>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<Category>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<Category>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            var mockContext = new Mock<DBSneakerStoreEntities>();
            mockContext.Setup(m => m.Categories).Returns(mockSet.Object);

            var controller = new CategoriesController { Database = mockContext.Object };


        }

        [TestMethod]
        public void Index_WithSearchString_ReturnsFilteredCategories_4()
        {
            // Arrange
            var data = new List<Category>
        {
            new Category { IDCate = 1, NameCate = "Sports" },
            new Category { IDCate = 2, NameCate = "Casual" },
            new Category { IDCate = 3, NameCate = "Running" }
        }.AsQueryable();

            var mockSet = new Mock<DbSet<Category>>();
            mockSet.As<IQueryable<Category>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<Category>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<Category>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<Category>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            var mockContext = new Mock<DBSneakerStoreEntities>();
            mockContext.Setup(m => m.Categories).Returns(mockSet.Object);

            var controller = new CategoriesController { Database = mockContext.Object };


        }

        [TestMethod]
        public void Create_ValidCategory_RedirectsToIndex_NewName()
        {
            // Arrange
            var mockSet = new Mock<DbSet<Category>>();
            var mockContext = new Mock<DBSneakerStoreEntities>();
            mockContext.Setup(m => m.Categories).Returns(mockSet.Object);

            var controller = new CategoriesController { Database = mockContext.Object };


        }

        [TestMethod]
        public void Create_DuplicateCategory_ReturnsErrorMessage()
        {
            // Arrange
            var existingCategory = new Category { IDCate = 1, NameCate = "Existing Category" };
            var mockSet = new Mock<DbSet<Category>>();
            mockSet.Setup(m => m.Add(It.IsAny<Category>())).Callback<Category>((s) => existingCategory = s);
            mockSet.Setup(m => m.Find(It.IsAny<object[]>())).Returns<object[]>(ids => existingCategory);

            var mockContext = new Mock<DBSneakerStoreEntities>();
            mockContext.Setup(m => m.Categories).Returns(mockSet.Object);

            var controller = new CategoriesController { Database = mockContext.Object };


        }

        [TestMethod]
        public void Details_NonExistentId_ReturnsNotFound()
        {
            // Arrange
            var mockSet = new Mock<DbSet<Category>>();
            var mockContext = new Mock<DBSneakerStoreEntities>();
            var data = new List<Category>().AsQueryable();

            mockSet.As<IQueryable<Category>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<Category>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<Category>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<Category>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            mockContext.Setup(m => m.Categories).Returns(mockSet.Object);

            var controller = new CategoriesController { Database = mockContext.Object };


        }

        [TestMethod]
        public void Delete_NonExistentId_ReturnsNotFound()
        {
            // Arrange
            var mockSet = new Mock<DbSet<Category>>();
            var mockContext = new Mock<DBSneakerStoreEntities>();
            var data = new List<Category>().AsQueryable();

            mockSet.As<IQueryable<Category>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<Category>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<Category>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<Category>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            mockContext.Setup(m => m.Categories).Returns(mockSet.Object);

            var controller = new CategoriesController { Database = mockContext.Object };


        }

        [TestMethod]
        public void Delete_ValidCategory_RemovesCategoryAndRedirectsToIndex()
        {
            // Arrange
            var categoryToRemove = new Category { IDCate = 1, NameCate = "Category to remove" };
            var mockSet = new Mock<DbSet<Category>>();
            mockSet.Setup(m => m.Find(It.IsAny<object[]>())).Returns<object[]>(ids => categoryToRemove);

            var mockContext = new Mock<DBSneakerStoreEntities>();
            mockContext.Setup(m => m.Categories).Returns(mockSet.Object);

            var controller = new CategoriesController { Database = mockContext.Object };


        }

        [TestMethod]
        public void Edit_NonExistentId_ReturnsNotFound()
        {
            // Arrange
            var mockSet = new Mock<DbSet<Category>>();
            var mockContext = new Mock<DBSneakerStoreEntities>();
            var data = new List<Category>().AsQueryable();

            mockSet.As<IQueryable<Category>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<Category>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<Category>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<Category>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            mockContext.Setup(m => m.Categories).Returns(mockSet.Object);

            var controller = new CategoriesController { Database = mockContext.Object };

  
        }


    }


}




