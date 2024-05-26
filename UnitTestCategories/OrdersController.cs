using SneakerStore.Models;
using System;
using System.Web.Mvc;

namespace SneakerStore.Tests.Controllers
{
    internal class OrdersController
    {
        public OrdersController()
        {
        }

        public DBSneakerStoreEntities Database { get; internal set; }
        public object ModelState { get; internal set; }

        internal ViewResult Create(OrderPro order)
        {
            throw new NotImplementedException();
        }
    }
}