using RailwayReservationMVC.Models.DAL;
using RailwayReservationMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Numerics;
using System.Web.Util;
using System.Net.Mail;
using System.Net;
using System.Web.Helpers;

namespace RailwayReservationMVC.Controllers
{
    
    public class PaymentController : Controller
    {
        public string transactionId;
        // GET: Payment
        private IRailwayRepository<Reservation> ResObj;
        private IRailwayRepository<TrainDetails> TrainObj;
        public PaymentController()
        {
            this.ResObj = new RailwayRepository<Reservation>();
            this.TrainObj = new RailwayRepository<TrainDetails>();

        }
        public ActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public ActionResult CreateOrder(Models.OrderModel _requestData)
        {
            Guid guidtrans = Guid.NewGuid();
            BigInteger big = new BigInteger(guidtrans.ToByteArray());
            var bigstr = big.ToString();
            var Id = bigstr.Replace("-", string.Empty);
            var transactionId = Id.ToString().Substring(0, 12);
            Session["transactionId"] = transactionId;

            Razorpay.Api.RazorpayClient client = new Razorpay.Api.RazorpayClient("rzp_test_aJVAJtaSskH7US", "5ug92ZJIU89n0R3SKU7O0RP8");
            Dictionary<string, object> options = new Dictionary<string, object>();
            options.Add("amount", 500 * 100);
            options.Add("receipt", transactionId);
            options.Add("currency", "INR");
            options.Add("payment_capture", "0");
            Razorpay.Api.Order orderResponse = client.Order.Create(options);
            string orderId = orderResponse["id"].ToString();

            Models.OrderModel orderModel = new Models.OrderModel
            {
                orderId = orderResponse.Attributes["id"],
                razorpayKey = "rzp_test_aJVAJtaSskH7US",
                amount = 500 * 100,
                currency = "INR",
                name = _requestData.name,
                email = _requestData.email,
                contactNumber = _requestData.contactNumber,
                address = _requestData.address,

            };
            
            return View("PaymentPage", orderModel);
        }

        


        [HttpPost]
        public ActionResult Complete(Reservation reserve)
        {
           
            string paymentId = Request.Params["rzp_paymentid"];

            string orderId = Request.Params["rzp_orderid"];

            Razorpay.Api.RazorpayClient client = new Razorpay.Api.RazorpayClient("rzp_test_aJVAJtaSskH7US", "5ug92ZJIU89n0R3SKU7O0RP8");

            Razorpay.Api.Payment payment = client.Payment.Fetch(paymentId);

            Dictionary<string, object> options = new Dictionary<string, object>();
            options.Add("amount", payment.Attributes["amount"]);
            Razorpay.Api.Payment paymentCaptured = payment.Capture(options);
            string amt = paymentCaptured.Attributes["amount"];

            //Session["email_to"] = _requestData.email;

            if (paymentCaptured.Attributes["status"] == "captured")
            {
                //sessions
                Guid guid = Guid.NewGuid();
                BigInteger big = new BigInteger(guid.ToByteArray());
                var bigstr = big.ToString();
                var str = bigstr.Replace("-", string.Empty);
                var pnr = str.ToString().Substring(0, 10);
                Session["PNR"] = pnr;
                
                

                List<TrainDetails> trainDetails = TrainObj.GetModel().ToList();

                reserve.PNR_NO = pnr;
                
                reserve.Res_Name = (string)Session["ResName"];
                reserve.Res_Gender = (string)Session["ResGender"];
                reserve.QuotaType= (string)Session["Quota"];
                reserve.User_Id = (int)Session["user_id"];
                reserve.Res_Date = (string)Session["Date"];
                reserve.Seat_No = (int)Session["seatno"];
                
                //var train_Id = from id in  trainDetails where id.SourceStation == reserve.TrainDetails.SourceStation select id;

                reserve.Transaction_Id = (string)Session["transactionId"];
                var train_id = from id in trainDetails where id.SourceStation == (string)Session["SourceStation"] select id.Train_Id;
                var tf = train_id.FirstOrDefault();

                reserve.Train_Id = tf;
                ResObj.InsertModel(reserve);
                ResObj.Save();



                //email notification:
                MailMessage mm = new MailMessage("railwayreservationsystemmail@gmail.com", (string)Session["email"] );

                mm.Subject = "Welcome to Railway Reservation System";
                mm.Body = "Thank you for Booking tickets with us!" + "\n" + "Here are you details:" + "\n" + "Transaction Id :" + Session["transactionId"] + "\n" + "PNR Number" + Session["PNR"] + "\n"+ "Name" + Session["ResName"] +"\n"+ "Source Station"  + Session["SourceStation"] + "\n" + "Destination Station" + Session["DestinationStation"] + "\n" + "Thank you ! Have a safe Journey";
                mm.IsBodyHtml = false;
                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587;
                smtp.EnableSsl = true;

                NetworkCredential nc = new NetworkCredential("railwayreservationsystemmail@gmail.com", "chfxpbtcfjfobhlv");
                smtp.UseDefaultCredentials = true;
                smtp.Credentials = nc;
                smtp.Send(mm);
                //TempData["Message"] = "Ticket Booked !";
               
                //redirect to payment gateway
                return RedirectToAction("Success");
            }
            else
            {
                return RedirectToAction("Failed");
            }
        }

        public ActionResult Success(OrderModel e)
        {
            
           
            return View();
        }

        public ActionResult Failed()
        {
            return View();
        }
    }
}