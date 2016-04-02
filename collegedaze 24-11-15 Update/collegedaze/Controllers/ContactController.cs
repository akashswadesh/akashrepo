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
    public class ContactController : Controller
    {
        //
        // GET: /Contact/

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
                MailAddress mailfrom = new MailAddress("coverman1965@gmail.com", p.FullName);
                MailAddress mailto = new MailAddress("coverman1965@gmail.com");

                MailMessage newmsg = new MailMessage(mailfrom, mailto);
                MailAddress SendCC = new MailAddress("coverman1965@gmail.com");
                newmsg.CC.Add(SendCC);

                newmsg.Subject = "Contact Request From" + " " + p.FullName;
                newmsg.IsBodyHtml = true;
                newmsg.Body = "<html><head><title></title></head><body style='font-family: Arial, Helvetica, sans-serif; color: #000; margin: 0;'><table width='100%' height='100%' cellspacing='0' cellpadding='0' border='0' align='center' style='height: 15px!important; max-width: 624px!important; border-collapse: collapse; padding-bottom: 0px; padding-top: 0px; padding-left: 0px; margin: 0px; padding-right: 0px; margin: 0px auto;'><tbody><tr><td></td></tr></tbody></table><table width='100%' height='100%' cellspacing='0' cellpadding='0' border='0' align='center' style='height: 100%!important; max-width: 624px!important; border-collapse: collapse; padding-bottom: 0px; padding-top: 0px; padding-left: 0px; margin: 0px; padding-right: 0px; margin: 0px auto;'><tbody><tr><td valign='top'><table width='100%' border='0'><tr><td>&nbsp;</td><td width='50%' align='left'><img src='http://demo14.gowebbi.com/images/logo.png' alt='Logo' /></td><td width='50%' align='right'><table border='0'><tr><td></td><td></td><td></td></tr></table></td><td width='10'>&nbsp;</td></tr></table><tr><td style='border-top: 1px solid #808080;'></td></tr></td></tr></tbody></table><table width='100%' height='100%' cellspacing='0' cellpadding='0' border='0' align='center' style='height: 100%!important; max-width: 624px!important; border-collapse: collapse; padding-bottom: 0px; padding-top: 0px; padding-left: 0px; margin: 0px; padding-right: 0px; margin: 0px auto;'><tbody><tr><td valign='top'><table width='100%' border='0'><tr><td>&nbsp;</td><td width='100%' align='left' valign='top' style='font-size: 15px; line-height: 24px;'>Dear <b>College Daze</b>,<br />You have received a contact request from <a style='color:blue;'href='mailto:" + p.EmailID + "'>" + p.FullName + "</a><br></td></tr></table></td></tr></tbody></table><br/><table width='100%' height='100%' cellspacing='0' cellpadding='0' border='0' align='center' style='height: 100%!important; max-width: 624px!important; border-collapse: collapse; padding-bottom: 0px; padding-top: 0px; padding-left: 0px; margin: 0px; padding-right: 0px; margin: 0px auto;'><tbody><tr><td style='font-size: 13px;background-color:#e5e5e5;padding:7px;'><b>Full Name:</b></td><td style='font-size: 15px;background-color:#e5e5e5;'>" + p.FullName + "</td></tr><tr><td style='font-size: 13px; background-color:white;padding:7px;'><b>Contact Number:</b></td><td style='font-size: 15px; background-color:white;'>" + p.ContactNo + "</td></tr><tr><td style='font-size: 13px; background-color:#e5e5e5;padding:7px;'><b>Email Address:</b></td><td style='font-size: 15px; background-color:#e5e5e5;'><a style='color:blue;'href='#' target='_blank'>" + p.EmailID + "</a></td></tr><tr><td style='font-size: 13px; background-color:white;padding:7px;'><b>Message:</b></td><td style='font-size: 15px; background-color:white;'>" + p.Message + "</td></tr><tr></tbody></table><table width='100%' height='100%' cellspacing='0' cellpadding='0' border='0' align='center' style='height: 100%!important; max-width: 624px!important; border-collapse: collapse; padding-bottom: 0px; padding-top: 0px; padding-left: 0px; margin: 0px; padding-right: 0px; margin: 0px auto;'><tbody><tr><td style='border-bottom: 1px solid #808080;'><tr></tbody></table></body></html>";

                SmtpClient smtp = new SmtpClient("smtpout.europe.secureserver.net", 25);
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential("enquiry@hbitm.in", "Gowebbi@123");
                smtp.EnableSsl = false;
                smtp.Send(newmsg);

                TempData["MSG"] = "Your request has been successfully sent. We will be in touch soon.";
                ModelState.Clear();
            }
            catch (Exception ex)
            {
                TempData["MSG"] = ex.ToString();
            }

            return Redirect("/Contact/Index/");
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
