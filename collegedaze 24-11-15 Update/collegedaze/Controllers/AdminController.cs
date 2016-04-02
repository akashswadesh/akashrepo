using collegedaze.Helpers;
using collegedaze.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace collegedaze.Controllers
{
    public class AdminController : Controller
    {
        //
        // GET: /Admin/
        Datalayer dl = new Datalayer();
        EncryptDecrypt enc = new EncryptDecrypt();
        public ActionResult Index()
        {
           
            return View();
        }


     

     

    }
}
