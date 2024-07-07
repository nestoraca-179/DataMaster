﻿using DataMaster.Models;
using System.Web.Mvc;

namespace DataMaster.Controllers
{
	public class UserController : Controller
    {
        public ActionResult Index()
        {
			if (!Request.IsAuthenticated)
				return RedirectToAction("Index", "Home", new { message = "Debes iniciar sesión" });
			else if (Session["USER"] == null)
				return RedirectToAction("Logout", "Account", new { msg = "Debes iniciar sesión" });

			ViewBag.usuario = (Session["USER"] as Usuario);
			ViewBag.users = MyUser.GetAllUsers();
			return View();
		}

		public ActionResult Agregar()
		{
			return View();
		}

		public ActionResult Editar()
		{
			return View();
		}
    }
}