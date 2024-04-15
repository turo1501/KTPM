using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SneakerStore.Models;
using System.Data.Entity;

namespace SneakerStore.Controllers
{
    public class LoginUserController : Controller
    {
        DBSneakerStoreEntities database = new DBSneakerStoreEntities();
        // GET: LoginUser
        public ActionResult FormLogin()
        {
            return View();
        }
        [HttpPost]
        public ActionResult FormLogin(AdminUser _user)
        {
            var check = database.AdminUsers.
                Where(s => s.NameUser == _user.NameUser && s.PasswordUser == _user.PasswordUser).FirstOrDefault();
            if (check == null)
            {
                ViewBag.ErrorInfo = "Sai Info";
                return View("Index");
            }
            else
            {
                database.Configuration.ValidateOnSaveEnabled = false;
                Session["ID"] = _user.ID;
                Session["NameUser"] = _user.NameUser;
                Session["PasswodUser"] = _user.PasswordUser;
                Session["RoleUser"] = check.RoleUser;
                Session["perCentDis"] = 0;


                if (check.RoleUser.ToString() == "Quản lí sản phẩm")
                    return RedirectToAction("Pro", "Product");
                else if (check.RoleUser.ToString() == "Quản lí khách hàng")
                    return RedirectToAction("Index", "Customers");
                else if (check.RoleUser.ToString() == "Quản lí đơn hàng")
                    return RedirectToAction("Index", "OrderProes");
                else if (check.RoleUser.ToString() == "Admin")
                    return RedirectToAction("Admin", "LoginUser");
                return View();
            }
        }
        public ActionResult RegisterUser()
        {
            return View();
        }
        [HttpPost]
        public ActionResult RegisterUser(AdminUser _user)
        {
            if(ModelState.IsValid)
            {
                var check_ID = database.AdminUsers.Where(s => s.ID == _user.ID).FirstOrDefault();
                if(check_ID == null)
                {
                    database.Configuration.ValidateOnSaveEnabled = false;
                    database.AdminUsers.Add(_user);
                    database.SaveChanges();
                    return RedirectToAction("FormLogin");
                }
                else
                {
                    ViewBag.ErrorRegister = "Địa chỉ ID đã tồn tại";
                }
            }
            return View();
        }

        public ActionResult LogOutUser()
        {
            Session.Abandon();
            return RedirectToAction("FormLogin", "LoginUser");
        }

        public ActionResult Admin()
        {
            return View();
        }

    }
}