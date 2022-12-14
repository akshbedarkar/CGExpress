using RailwayReservationMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RailwayReservationMVC.Controllers
{
    public class HomeController : Controller
    {
        DataContext obj = new DataContext();
        //homepage
        public ActionResult Index()
        {
            var data = obj.TrainDetails.ToList();
            return View(data);
        }

        //signup page
        public ActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SignUp(User u)
        {
           return View();
        }

        //login page
        public ActionResult LogIn()
        {
            return View();
        }

        [HttpPost]
        public ActionResult LogIn(User u)
        {
            return View();
        }


    }
}