using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SneakerStore.Controllers;
using SneakerStore.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static System.Collections.Specialized.BitVector32;



namespace UnitTest
{
    [TestClass]
    public class ShoppingCartControllerTests : Controller
    {
        private Mock<DBSneakerStoreEntities> mockDatabase;
        private Mock<HttpSessionStateBase> session;
        private Mock<HttpContextBase> httpContext;
        private ShoppingCartController controller;
        private Cart cart;
        [TestInitialize]
        public void Setup()
        {
            mockDatabase = new Mock<SneakerStore.Models.DBSneakerStoreEntities>();

            // Initialize the cart and mock session
            session = new Mock<HttpSessionStateBase>();
            session.Setup(s => s["Cart"]).Returns(new Cart());

            // Mock HttpContext
            httpContext = new Mock<HttpContextBase>();
            httpContext.Setup(c => c.Session).Returns(session.Object);

            // Create controller and set its context
            controller = new ShoppingCartController(mockDatabase.Object)
            {
                ControllerContext = new ControllerContext { HttpContext = httpContext.Object }
            };
        }
        [TestMethod]
        public void TestAddToCart_EmptyCart()
        {
            // Arrange
            var productId = 1;

            // Mock the Products property to return a predefined collection of products
            var products = new List<Product>
        {
            new Product { ProductID = productId, NamePro = "Test Product", Price = 100 }
        }.AsQueryable();

            // Set up the behavior of the Products property
            var mockProducts = new Mock<System.Data.Entity.DbSet<Product>>();
            mockProducts.As<IQueryable<Product>>().Setup(m => m.Provider).Returns(products.Provider);
            mockProducts.As<IQueryable<Product>>().Setup(m => m.Expression).Returns(products.Expression);
            mockProducts.As<IQueryable<Product>>().Setup(m => m.ElementType).Returns(products.ElementType);
            mockProducts.As<IQueryable<Product>>().Setup(m => m.GetEnumerator()).Returns(products.GetEnumerator());
            mockDatabase.Setup(db => db.Products).Returns(mockProducts.Object);

            // Ensure there's no existing cart in the session
            session.Setup(s => s["Cart"]).Returns(null);

            // Act
            var result = controller.AddToCart(productId) as RedirectToRouteResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("ShowCart", result.RouteValues["action"]);
            Assert.AreEqual("ShoppingCart", result.RouteValues["controller"]);

            // Check if the cart in the session contains the added product
            var cart = session.Object["Cart"] as Cart;
            Assert.IsNotNull(cart);
            Assert.AreEqual(1, cart.Items.Count());
            Assert.AreEqual(productId, cart.Items.First()._product.ProductID);
        }
        [TestMethod]
        public void AddToCart_ValidProductId_ProductAddedToCart()
        {
            // Arrange
            var product = new Product { ProductID = 1, NamePro = "Test Product", Price = 100, Quantity = 5 };

            // Mock the DbSet for Products
            var products = new List<Product> { product }.AsQueryable();
            var mockProductSet = new Mock<System.Data.Entity.DbSet<Product>>();
            mockProductSet.As<IQueryable<Product>>().Setup(m => m.Provider).Returns(products.Provider);
            mockProductSet.As<IQueryable<Product>>().Setup(m => m.Expression).Returns(products.Expression);
            mockProductSet.As<IQueryable<Product>>().Setup(m => m.ElementType).Returns(products.ElementType);
            mockProductSet.As<IQueryable<Product>>().Setup(m => m.GetEnumerator()).Returns(products.GetEnumerator());

            mockDatabase.Setup(db => db.Products).Returns(mockProductSet.Object);

            // Act
            var result = controller.AddToCart(1) as RedirectToRouteResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("ShowCart", result.RouteValues["action"]);
            Assert.AreEqual("ShoppingCart", result.RouteValues["controller"]);
            Assert.AreEqual(1, cart.Items.Count());
            Assert.AreSame(product, cart.Items.First()._product);
        }

        [TestMethod]
        public void TestUpdate_Cart_Quantity()
        {
            // Arrange
            var mockDatabase = new Mock<SneakerStore.Models.DBSneakerStoreEntities>();

            // Create a product and add it to the cart
            var productId = 1;
            var product = new Product { ProductID = productId, NamePro = "Test Product", Price = 100 };
            var cart = new Cart();
            cart.Add_Product_Cart(product);
            cart.Update_quantity(productId, 1); // Initialize with quantity 1

            var controller = new ShoppingCartController(mockDatabase.Object);

            // Mock HttpContext and set up its Session property
            var httpContext = new Mock<HttpContextBase>();
            var session = new MockHttpSession();
            httpContext.Setup(c => c.Session).Returns(session);

            // Set the HttpContext for the controller
            var controllerContext = new ControllerContext
            {
                Controller = controller,
                HttpContext = httpContext.Object
            };
            controller.ControllerContext = controllerContext;

            // Put the cart in session
            session["Cart"] = cart;

            // Prepare form data
            var form = new FormCollection
    {
        { "idPro", productId.ToString() },
        { "cartQuantity", "5" }
    };
            // Act
            var result = controller.Update_Cart_Quantity(form) as RedirectToRouteResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("ShowCart", result.RouteValues["action"]);
            Assert.AreEqual("ShoppingCart", result.RouteValues["controller"]);

            // Check if the cart in the session has the updated quantity
            cart = session["Cart"] as Cart;
            Assert.IsNotNull(cart, "Cart is null in session");

            var cartItem = cart.Items.SingleOrDefault(i => i._product.ProductID == productId);
            Assert.IsNotNull(cartItem, "Cart item not found");
            Assert.AreEqual(5, cartItem._quantity, "Cart item quantity mismatch");
        }
        [TestMethod]
        public void RemoveCart_ValidProductId_ProductRemovedFromCart()
        {
            // Arrange
            // Arrange
            var mockDatabase = new Mock<DBSneakerStoreEntities>();
            var controller = new ShoppingCartController(mockDatabase.Object);
            var cart = new Cart();
            cart.Add_Product_Cart(new Product { ProductID = 1 });
            var product = new Product { ProductID = 1, NamePro = "Test Product", Price = 100, Quantity = 5 };
            cart.Add_Product_Cart(product);

            // Act
            var result = controller.RemoveCart(1) as RedirectToRouteResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("ShowCart", result.RouteValues["action"]);
            Assert.AreEqual("ShoppingCart", result.RouteValues["controller"]);
            Assert.AreEqual(0, cart.Items.Count());
            Assert.IsNull(controller.Session["Cart"]);
        }
        [TestMethod]
        public void TestRemoveCart()
        {
            // Arrange
            var mockDatabase = new Mock<DBSneakerStoreEntities>();
            var controller = new ShoppingCartController(mockDatabase.Object);
            var cart = new Cart();
            cart.Add_Product_Cart(new Product { ProductID = 1 });

            // Mock the Session property of the controller
            var controllerContext = new Mock<ControllerContext>();
            var mockSession = new MockHttpSession();
            controllerContext.Setup(c => c.HttpContext.Session).Returns(mockSession);
            controller.ControllerContext = controllerContext.Object;

            // Set the "Cart" session variable
            controller.Session["Cart"] = cart;

            // Act
            controller.RemoveCart(1);

            // Assert
            Assert.IsNull(controller.Session["Cart"]);
        }

        // Mock HttpSessionStateBase for testing session
        public class MockHttpSession : System.Web.HttpSessionStateBase
        {
            private readonly System.Collections.Generic.Dictionary<string, object> _sessionDictionary = new System.Collections.Generic.Dictionary<string, object>();

            public override object this[string name]
            {
                get { return _sessionDictionary.ContainsKey(name) ? _sessionDictionary[name] : null; }
                set { _sessionDictionary[name] = value; }
            }
        }
        [TestMethod]
        public void TestCheckOut_Success_View()
        {
            // Arrange
            var mockDatabase = new Mock<DBSneakerStoreEntities>();

            // Mock the session
            var cart = new Cart();
            var session = new Mock<HttpSessionStateBase>();
            session.Setup(s => s["Cart"]).Returns(cart);

            var httpContext = new Mock<HttpContextBase>();
            httpContext.Setup(c => c.Session).Returns(session.Object);
            var controller = new ShoppingCartController(mockDatabase.Object)
            {
                ControllerContext = new ControllerContext { HttpContext = httpContext.Object }
            };

            // Act
            var result = controller.CheckOut_Success() as ViewResult;

            // Assert
            Assert.IsNotNull(result, "ViewResult is null");
            Assert.AreEqual("CheckOut_Success", result.ViewName);
        }
        [TestMethod]
        public void TestApplyDiscountCode_ValidCode()
        {
            // Arrange
            var mockDatabase = new Mock<DBSneakerStoreEntities>();

            // Mock the session
            var cart = new Cart();
            var session = new Mock<HttpSessionStateBase>();
            session.Setup(s => s["Cart"]).Returns(cart);

            var httpContext = new Mock<HttpContextBase>();
            httpContext.Setup(c => c.Session).Returns(session.Object);
            var controller = new ShoppingCartController(mockDatabase.Object)
            {
                ControllerContext = new ControllerContext { HttpContext = httpContext.Object }
            };

            // Mock form data
            var form = new FormCollection
    {
        { "discountCode", "DISCOUNT123" }
    };

            // Mock the vouchers
            var vouchers = new List<Voucher>
    {
        new Voucher { MaVoucher = "DISCOUNT123", PhanTramDis = 10 }
    }.AsQueryable();
            var mockVoucherSet = new Mock<System.Data.Entity.DbSet<Voucher>>();
            mockVoucherSet.As<IQueryable<Voucher>>().Setup(m => m.Provider).Returns(vouchers.Provider);
            mockVoucherSet.As<IQueryable<Voucher>>().Setup(m => m.Expression).Returns(vouchers.Expression);
            mockVoucherSet.As<IQueryable<Voucher>>().Setup(m => m.ElementType).Returns(vouchers.ElementType);
            mockVoucherSet.As<IQueryable<Voucher>>().Setup(m => m.GetEnumerator()).Returns(vouchers.GetEnumerator());
            mockDatabase.Setup(db => db.Vouchers).Returns(mockVoucherSet.Object);

            // Act
            var result = controller.ApplyDiscountCode(form) as RedirectToRouteResult;

            // Assert
            Assert.IsNotNull(result, "RedirectToRouteResult is null");
            Assert.AreEqual("ShowCart", result.RouteValues["action"]);
            Assert.AreEqual("ShoppingCart", result.RouteValues["controller"]);
            Assert.AreEqual(10, session.Object["perCentDis"]);
        }
        [TestMethod]
        public void BagCart_CartExists_ReturnsPartialViewWithTotalQuantity()
        {
            // Arrange
            var product = new Product { ProductID = 1, NamePro = "Test Product", Price = 100, Quantity = 5 };
            cart.Add_Product_Cart(product);

            // Act
            var result = controller.BagCart() as PartialViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("BagCart", result.ViewName);
            Assert.AreEqual(1, result.ViewBag.QuantityCart);
        }
    }
}




