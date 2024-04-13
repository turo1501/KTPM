using PayPal.Api;
using SneakerStore.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SneakerStore.Controllers
{
    public class ShoppingCartController : Controller
    {
        DBSneakerStoreEntities database = new DBSneakerStoreEntities();
        // GET: ShoppingCart
        public ActionResult ShowCart()
        {
            Cart _cart = Session["Cart"] as Cart;
            if (Session["Cart"] == null)
                return View("EmptyCart");
            return View(_cart);
        }
        //Action tao moi gio hang
        public Cart GetCart()
        {
            Cart cart = Session["Cart"] as Cart;
            if (cart == null || Session["Cart"] == null)
            {
                cart = new Cart();
                Session["Cart"] = cart;
            }
            return cart;
        }
        //Action them product vao gio hang
        public ActionResult AddToCart(int id)
        {
            var _pro = database.Products.SingleOrDefault(s => s.ProductID == id); //Lay san pham theo id
            if (_pro != null)
            {
                GetCart().Add_Product_Cart(_pro);
            }
            return RedirectToAction("ShowCart", "ShoppingCart");
        }
        public ActionResult Update_Cart_Quantity(FormCollection form)
        {
            Cart cart = Session["Cart"] as Cart;
            int id_pro = int.Parse(form["idPro"]);
            //int maxquan = int.Parse(form["maxValue"]);
            int _quantity = int.Parse(form["cartQuantity"]);
            if (_quantity>0)
            {
                cart.Update_quantity(id_pro, _quantity);
            }         
            return RedirectToAction("ShowCart", "ShoppingCart");
        }
        public ActionResult RemoveCart(int id)
        {
            Cart cart = Session["Cart"] as Cart;
            cart.Remove_CartItem(id);
            if(cart.Items.Count() == 0)
            {
                cart = null;
                Session["Cart"]= null;
            }
            return RedirectToAction("ShowCart", "ShoppingCart");
        }
        public PartialViewResult BagCart()
        {
            int total_quantity_item = 0;
            Cart cart = Session["Cart"] as Cart;
            if (cart != null)
            {
                total_quantity_item = cart.Total_quantity();
                ViewBag.QuantityCart = total_quantity_item;
            }
            return PartialView("BagCart");
        }
        //Tao Action Checkout cho món hàng khách hàng thanh toán
        public ActionResult CheckOut(FormCollection form)
        {
            try
            {
                Cart cart = Session["Cart"] as Cart;
                OrderPro _order = new OrderPro();//Bang hoa don cau san pham
                _order.DateOrder = DateTime.Now;
                _order.AddressDeliverry = form["AddressDelivery"];
                _order.IDCus = int.Parse(form["CodeCustomer"]);
                database.OrderProes.Add(_order);
                foreach (var item in cart.Items)
                {
                    OrderDetail _order_detail = new OrderDetail(); //Luu bang san pham vao Chi tiet hoa don
                    _order_detail.IDOrder = _order.ID;
                    _order_detail.IDProduct = item._product.ProductID;
                    _order_detail.UnitPrice = (double)item._product.Price;
                    _order_detail.Quantity = item._quantity;
                    database.OrderDetails.Add(_order_detail);
                    //xu li cap nhat lai so luong ton trong bang Product
                    foreach (var p in database.Products.Where(s => s.ProductID == _order_detail.IDProduct))//lay ID Product dang co trong gio hang
                    {
                        var update_quan_pro = p.Quantity - item._quantity;
                        p.Quantity = update_quan_pro;
                    }
                }
                database.SaveChanges();
                cart.ClearCart();
                return RedirectToAction("CheckOut_Success", "ShoppingCart");

            }
            catch
            {
                return Content("Error checkout .Please check information of Customer. Thanks");
            }
        }
        public ActionResult CheckOut_Success()
        {
            Cart cart = Session["Cart"] as Cart;

            // Kiểm tra nếu giỏ hàng không null
            if (cart != null)
            {
                // Xóa sản phẩm khỏi giỏ hàng sau khi thanh toán
                cart.ClearCart();

                // Abandon session để đảm bảo không còn dữ liệu không mong muốn
                Session.Abandon();
            }

            // Chuyển đến trang thành công
            return View();
        }
        public ActionResult ApplyDiscountCode(FormCollection form)
        {
            Cart cart = Session["Cart"] as Cart;
            string discountCode = (string)form["discountCode"];

            var check = database.Vouchers.
                Where(s => s.MaVoucher == discountCode).FirstOrDefault();

            if (check != null)
            {
                int perCentDis = (int)check.PhanTramDis;
                Session["perCentDis"] = perCentDis;
                cart.Total_price_after_dis();
            }
            return RedirectToAction("ShowCart","ShoppingCart");
        }

        public ActionResult PaymentWithPaypal(string Cancel = null)
        {
            //getting the apiContext  
            APIContext apiContext = PaypalConfiguration.GetAPIContext();
            try
            {
                //A resource representing a Payer that funds a payment Payment Method as paypal  
                //Payer Id will be returned when payment proceeds or click to pay  
                string payerId = Request.Params["PayerID"];
                if (string.IsNullOrEmpty(payerId))
                {
                    //this section will be executed first because PayerID doesn't exist  
                    //it is returned by the create function call of the payment class  
                    // Creating a payment  
                    // baseURL is the url on which paypal sendsback the data.  
                    string baseURI = Request.Url.Scheme + "://" + Request.Url.Authority + "/shoppingcart/PaymentWithPayPal?";
                    //here we are generating guid for storing the paymentID received in session  
                    //which will be used in the payment execution  
                    var guid = Convert.ToString((new Random()).Next(100000));
                    //CreatePayment function gives us the payment approval url  
                    //on which payer is redirected for paypal account payment  
                    var createdPayment = this.CreatePayment(apiContext, baseURI + "guid=" + guid);
                    //get links returned from paypal in response to Create function call  
                    var links = createdPayment.links.GetEnumerator();
                    string paypalRedirectUrl = null;
                    while (links.MoveNext())
                    {
                        Links lnk = links.Current;
                        if (lnk.rel.ToLower().Trim().Equals("approval_url"))
                        {
                            //saving the payapalredirect URL to which user will be redirected for payment  
                            paypalRedirectUrl = lnk.href;
                        }
                    }
                    // saving the paymentID in the key guid  
                    Session.Add(guid, createdPayment.id);
                    return Redirect(paypalRedirectUrl);
                }
                else
                {
                    // This function exectues after receving all parameters for the payment  
                    var guid = Request.Params["guid"];
                    var executedPayment = ExecutePayment(apiContext, payerId, Session[guid] as string);
                    //If executed payment failed then we will show payment failure message to user  
                    if (executedPayment.state.ToLower() != "approved")
                    {
                        return View("FailureView");
                    }
                }
            }
            catch (Exception ex)
            {
                return View("FailureView");
            }
            //on successful payment, show success page to user.  
            return View("CheckOut_Success");
        }
        private PayPal.Api.Payment payment;
        private Payment ExecutePayment(APIContext apiContext, string payerId, string paymentId)
        {
            var paymentExecution = new PaymentExecution()
            {
                payer_id = payerId
            };
            this.payment = new Payment()
            {
                id = paymentId
            };
            return this.payment.Execute(apiContext, paymentExecution);
        }
        private Payment CreatePayment(APIContext apiContext, string redirectUrl)
        {
            //create itemlist and add item objects to it  
            var itemList = new ItemList()
            {
                items = new List<Item>()
            };
            //Adding Item Details like name, currency, price etc  
            itemList.items.Add(new Item()
            {
                name = "Item Name comes here",
                currency = "USD",
                price = "1",
                quantity = "1",
                sku = "sku"
            });
            var payer = new Payer()
            {
                payment_method = "paypal"
            };
            // Configure Redirect Urls here with RedirectUrls object  
            var redirUrls = new RedirectUrls()
            {
                cancel_url = redirectUrl + "&Cancel=true",
                return_url = redirectUrl
            };
            // Adding Tax, shipping and Subtotal details  
            var details = new Details()
            {
                tax = "1",
                shipping = "1",
                subtotal = "1"
            };
            //Final amount with details  
            var amount = new Amount()
            {
                currency = "USD",
                total = "3", // Total must be equal to sum of tax, shipping and subtotal.  
                details = details
            };
            var transactionList = new List<Transaction>();
            // Adding description about the transaction  
            var paypalOrderId = DateTime.Now.Ticks;
            transactionList.Add(new Transaction()
            {
                description = $"Invoice #{paypalOrderId}",
                invoice_number = paypalOrderId.ToString(), //Generate an Invoice No    
                amount = amount,
                item_list = itemList
            });
            this.payment = new Payment()
            {
                intent = "sale",
                payer = payer,
                transactions = transactionList,
                redirect_urls = redirUrls
            };
            // Create a payment using a APIContext  
            return this.payment.Create(apiContext);
        }
    }
}