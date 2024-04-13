using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SneakerStore.Models;

namespace SneakerStore.Controllers
{
    public class CustomersController : Controller
    {
        private DBSneakerStoreEntities db = new DBSneakerStoreEntities();

        // GET: Customers
        public ActionResult Index(string _nameCus)
        {
            if (_nameCus == null)
            {
                return View(db.Customers.ToList());
            }
            else
            {
                return View(db.Customers.Where(s => s.NameCus.Contains(_nameCus)).ToList());
            }
            
        }

        // GET: Customers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // GET: Customers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Customers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IDCus,NameCus,PhoneCus,EmailCus,UserName,Password,ConfirmPass")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                db.Customers.Add(customer);
                db.SaveChanges();
                return RedirectToAction("LoginCus");
            }

            return View(customer);
        }

        // GET: Customers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // POST: Customers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IDCus,NameCus,PhoneCus,EmailCus")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(customer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(customer);
        }

        // GET: Customers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Customer customer = db.Customers.Find(id);
            db.Customers.Remove(customer);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult LoginCus()
        {
            return View();
        }
        [HttpPost]
        public ActionResult LoginCus(Customer cus)
        {
            var check = db.Customers.Where(s => s.UserName == cus.UserName && s.Password == cus.Password).FirstOrDefault();
            if (check == null)
            {
                ViewBag.ErrorInfo = "Tài khoản không tồn tại";
                return View("LoginCus");
            }
            else
            {
                db.Configuration.ValidateOnSaveEnabled = false;
                Session["IDCus"] = check.IDCus;
                Session["Password"] = check.Password;
                Session["NameCus"] = check.NameCus;
                Session["UserName"] = check.UserName;
                Session["PhoneCus"] = check.PhoneCus;

                // Kiểm tra xem có URL trả lại trong chuỗi truy vấn không
                string returnUrl = Request.QueryString["ReturnUrl"];

                if (!string.IsNullOrEmpty(returnUrl))
                {
                    // Nếu có URL trả lại, chuyển hướng đến nó
                    return Redirect(returnUrl);
                }
                else
                {
                    // Nếu không có URL trả lại, chuyển hướng đến hành động mặc định
                    return RedirectToAction("Index", "Product");
                }
            }
        }


        public ActionResult LogOutCus()
        {
            Session.Abandon();
            if (Request.UrlReferrer != null)
            {
                // Lấy đường dẫn trước đó và chuyển hướng
                string returnUrl = Request.UrlReferrer.AbsolutePath;
                return Redirect(returnUrl);
            }
            else
            {
                // Nếu không có đường dẫn trước đó, chuyển hướng mặc định
                return RedirectToAction("Index", "Product");
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
