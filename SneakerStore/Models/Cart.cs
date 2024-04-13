using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace SneakerStore.Models
{
    public class CartItem
    {
        public Product _product { get; set; }
        public int _quantity { get; set; }

    }

    public class Cart
    {
        List<CartItem> items = new List<CartItem>();

        public IEnumerable<CartItem> Items
        {
            get { return items; }
        }



        public void Add_Product_Cart(Product _pro, int _quan = 1)
        {
            var item = Items.FirstOrDefault(s => s._product.ProductID == _pro.ProductID);
            if (item == null) //neu gio hang trong thi them dong hang moi vao gio
                items.Add(new CartItem
                {
                    _product = _pro,
                    _quantity = _quan
                });
            else
                item._quantity += _quan;//Tong so luong trong gio hang dc con don

        }
        //Viet ham tinh tong so luong trong gio hang
        public int Total_quantity()
        {
            return items.Sum(s => s._quantity);
        }
        //Viet ham tinh thanh tien cho moi dong san pham
        public decimal Total_money()
        {
            var total = items.Sum(s => s._quantity * (s._product.Price));
            return (decimal)total;
        }

        public decimal Total_price_after_dis() 
        {
            decimal totalPrice = Total_money();

            if (System.Web.HttpContext.Current.Session != null && System.Web.HttpContext.Current.Session["perCentDis"] != null)
            {
                int? intValue = System.Web.HttpContext.Current.Session["perCentDis"] as int?;
                if (intValue.HasValue)
                {
                    int perCentDis = (int)System.Web.HttpContext.Current.Session["perCentDis"];
                    decimal totalAfterDiscount = totalPrice - (totalPrice * perCentDis / 100);

                    return (decimal)totalAfterDiscount;
                }

            }
            return (decimal)totalPrice;
        }


        //VIet ham cap nhat lai so luong san pham o moi dong san pham khi khach hang muon dat mua them
        public void Update_quantity(int id, int _new_quan)
        {
            var item = items.Find(s => s._product.ProductID == id);
            if (item != null)
            {
                if (items != null) //neu sl nho hon sl ton
                    item._quantity = _new_quan; // thi chap nhan luong mua
                else
                    item._quantity = 0;//nguoc lai, thi so luong mua tra ve 1
            }

        }
        //Viet ham xoa san pham trong gio hang
        public void Remove_CartItem(int id)
        {
            items.RemoveAll(s => s._product.ProductID == id);
        }
        //Viet ham xoa gio hang sau khi Khach hang thuc hien thanh toan
        public void ClearCart()
        {
            items.Clear();
        }

    }
}