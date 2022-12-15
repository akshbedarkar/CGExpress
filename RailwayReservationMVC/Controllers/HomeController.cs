using RailwayReservationMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Text;

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
            // database updation
            var data = obj.Users.Add(u);
            u.Password= Convert.ToBase64String(
            System.Security.Cryptography.SHA256.Create()
            .ComputeHash(Encoding.UTF8.GetBytes(u.Password)));
            obj.SaveChanges();

            //email notification
            MailMessage mm = new MailMessage("railwayreservationsystemmail@gmail.com", u.Email);

            mm.Subject = "Welcome to Railway Reservation System";
            mm.Body = "This is your password :" + u.Password.ToString();
            mm.IsBodyHtml = false;
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.Port = 587;
            smtp.EnableSsl = true;

            NetworkCredential nc = new NetworkCredential("railwayreservationsystemmail@gmail.com", "chfxpbtcfjfobhlv");
            smtp.UseDefaultCredentials = true;
            smtp.Credentials = nc;

            smtp.Send(mm);

            ViewBag.message = "Thank you for Connecting with us!Your password has been sent to your regsitered mail id  ";

            return RedirectToAction("Index");
        }

        
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