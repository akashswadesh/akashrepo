using collegedaze.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace collegedaze.Controllers
{
    public class NewsletterController : Controller
    {
        //
        // GET: /Newsletter/

        public ActionResult Index()
        {
            ListMenu();
            return View();
        }

        [HttpPost]
        public ActionResult Index(Property p)
        {
            try
            {
                MailAddress mailfrom = new MailAddress("coverman1965@gmail.com", p.EmailID);
                MailAddress mailto = new MailAddress("coverman1965@gmail.com");

                MailMessage newmsg = new MailMessage(mailfrom, mailto);
                MailAddress SendBCC = new MailAddress("coverman1965@gmail.com");
                newmsg.Bcc.Add(SendBCC);
                newmsg.IsBodyHtml = true;
                newmsg.Subject = "Newsletter Subscribe Request  From " + p.EmailID;
                newmsg.Body = "<Html><head><title></title></head> You have received a Subscribe Request from <a style='color:blue;'href='mailto:" + p.EmailID + "' email id>" + p.EmailID + "</a> <Html>";


                SmtpClient smtp = new SmtpClient("smtpout.europe.secureserver.net", 25);
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential("enquiry@hbitm.in", "Gowebbi@123");
                smtp.EnableSsl = false;
                smtp.Send(newmsg);

                TempData["MSG"] = "Thanks For Subscribing our News Letter";
                ModelState.Clear();
            }
            catch (Exception ex)
            {
                TempData["MSG"] = ex.ToString();
            }
            return Redirect("/Newsletter/Index/");


        }


        public void ListMenu()
        {
            Datalayer dl = new Datalayer();
            DataSet ds = new DataSet();
            Property p1 = new Property();

            p1.Condition1 = "";
            p1.Condition2 = "";
            p1.Condition3 = "";
            p1.onTable = "LIST_MENU";

            ds = dl.FETCH_CONDITIONAL_QUERY(p1);

            List<Property> Menu = new List<Property>();
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    Property p = new Property();
                    p.id = ds.Tables[0].Rows[i]["id"].ToString();

                    p.Menu = ds.Tables[0].Rows[i]["Menu"].ToString();

                    p.URLTitle = ds.Tables[0].Rows[i]["URL"].ToString();
                    Menu.Add(p);
                }
            }
            else
            {
                Property p = new Property();
                p.id = "0";

                p.Menu = "NONE";

                p.URLTitle = "NONE";
                Menu.Add(p);
            }




            ViewBag.MenuList = Menu;

        }

    }
}
