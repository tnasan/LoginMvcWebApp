﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using LoginMvcWebApp.Models;

namespace LoginMvcWebApp
{
    public class AccountController : Controller
    {
        //
        // GET: /Login/
        [Authorize(Roles = "Admin, IT")]
        public ActionResult Index()
        {
            ViewBag.IsAdmin = Request.RequestContext.HttpContext.User.IsInRole("Admin");
            ViewBag.IsIT = Request.RequestContext.HttpContext.User.IsInRole("IT");
            ViewBag.IsUser = Request.RequestContext.HttpContext.User.IsInRole("User");

            return View();
        }

        public ActionResult Login()
        {
            return View(new User());
        }

        [HttpPost]
        public ActionResult Login(User user)
        {
            string passwordHash = Helper.GetSHA512Hash(user.Password);

            using (LoginDataModelContainer db = new LoginDataModelContainer())
            {
                var dbUser = db.Users.Where(x => x.Username == user.Username && x.Password == passwordHash).SingleOrDefault();
                if (dbUser != null)
                {
                    FormsAuthentication.SetAuthCookie(user.Username, true);
                    return RedirectToAction("Index");
                }
            }

            ModelState.AddModelError("LoginError", "Login failed");
            return View(user);
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index");
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(User user)
        {
            return RedirectToAction("Index");
        }

        public ActionResult Delete(int id)
        {
            return RedirectToAction("Index");
        }

        public ActionResult Unauthorized()
        {
            return View();
        }
	}
}