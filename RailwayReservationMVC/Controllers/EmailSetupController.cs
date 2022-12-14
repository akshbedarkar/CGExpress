using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Web;
using System.Web.Mvc;
using RailwayReservationMVC.Models;

namespace RailwayReservationMVC.Controllers
{
    public class EmailSetupController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(RailwayReservationMVC.Models.User model)
        {
            MailMessage mm = new MailMessage("railwayreservationsystemmail@gmail.com", model.Email);
            
            mm.Subject = "Ticket Notification";
            mm.Body = "thank you for your coordination";
            mm.IsBodyHtml = false;
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.Port = 587;
            smtp.EnableSsl = true;

            NetworkCredential nc = new NetworkCredential("railwayreservationsystemmail@gmail.com", "chfxpbtcfjfobhlv");
            smtp.UseDefaultCredentials = true;
            smtp.Credentials = nc;

            smtp.Send(mm);

            ViewBag.message = "your ticket details have been sent successfully to your mail id ";
            return View(model);
        }

    }
}