using SneakerStore.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;

namespace SneakerStore.Tests.Controllers
{
    internal class MockDbSet<T>
    {
        private List<Product> products;
        private List<OrderDetail> orderDetails;

        public MockDbSet(List<Product> products)
        {
            this.products = products;
        }

        public MockDbSet(List<OrderDetail> orderDetails)
        {
            this.orderDetails = orderDetails;
        }

        internal DbSet<Product> Object()
        {
            throw new NotImplementedException();
        }
    }
}