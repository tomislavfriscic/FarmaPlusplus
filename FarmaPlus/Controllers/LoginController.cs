using FarmaPlus.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FarmaPlus.Controllers
{
    public class LogInController : Controller
    {
        FarmaPlusEntities db = new FarmaPlusEntities();

        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult CheckValidUser(tblUsrPsw model)
        {
            string result = "Fail";
            var DataItem = db.tblUsrPsw.Where(x => x.Username == model.Username && x.Password == model.Password).SingleOrDefault();
            if (DataItem != null)
            {
                Session["UserID"] = DataItem.ID.ToString();
                Session["UserName"] = DataItem.Username.ToString();
                result = "Success";
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Registar()
        {
            if (Session["UserID"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Registar");
            }
        }

        public ActionResult Logout()
        {
            Session.Clear();
            Session.Abandon();
            return RedirectToAction("Index");
        }
    }
}