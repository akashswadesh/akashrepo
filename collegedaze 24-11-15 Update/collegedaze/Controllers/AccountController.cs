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
  
        public class AccountController : Controller
        {
            //
            // GET: /Account/

            EncryptDecrypt enc = new EncryptDecrypt();
            Datalayer dl = new Datalayer();

            public ActionResult Logout()    
            {
                Session.Abandon();
                Session.Clear();
                HttpCookie loginCookie = Request.Cookies["login_cookie"];
                if (loginCookie != null)
                {
                    loginCookie.Expires = DateTime.Now.AddDays(-1);
                    Response.Cookies.Add(loginCookie);
                }
                HttpCookie lockingCookie = Request.Cookies["locking_cookie"];
                if (lockingCookie != null)
                {
                    lockingCookie.Expires = DateTime.Now.AddDays(-1);
                    Response.Cookies.Add(lockingCookie);
                }
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.Cache.SetExpires(DateTime.UtcNow.AddHours(-1));
                Response.Cache.SetNoStore();
                TempData["Msg"] = "You have logged out successfully!";
                return RedirectToAction("Login", "Account");
               
            }

           

            public ActionResult Login()
            {
               
            return View();
             
            }
            

            [HttpPost]
            public ActionResult Login(Property p)
            {
                DataSet ds = new DataSet();


                ds = dl.FETCH_LOGIN_DETAILS(p);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    if (ds.Tables[0].Rows[0]["UserType"].ToString() == "ADMIN" && ds.Tables[0].Rows[0]["Status"].ToString() == "ACTIVE")
                    {

                        HttpCookie loginCookie = Request.Cookies["login_cookie"];
                        if (loginCookie == null)
                        {
                            loginCookie = new HttpCookie("login_cookie");
                            loginCookie["UserName"] = enc.Encrypt(p.EmailID);
                            loginCookie["UserFullName"] = p.FullName;
                            loginCookie["UserType"] = "ADMIN";
                            loginCookie.Expires = DateTime.Now.AddDays(1);
                            Response.Cookies.Add(loginCookie);
                        }
                        else
                        {

                            loginCookie.Expires = DateTime.Now.AddDays(-1);

                            Response.Cookies.Add(loginCookie);
                            loginCookie = new HttpCookie("login_cookie");
                            loginCookie["UserName"] = enc.Encrypt(p.EmailID);
                            loginCookie["UserFullName"] = p.FullName;
                            loginCookie["UserType"] = "ADMIN";
                            loginCookie.Expires = DateTime.Now.AddDays(1);
                            Response.Cookies.Add(loginCookie);
                        }

                        return Redirect("/Admin");
                    }
                 
                 
                    else
                    {
                        TempData["MSG"] = "Your Account is not Verified!";
                        return View();
                    }
                }
                else
                {
                    TempData["MSG"] = "oops.. Your Email or Password is incorrect";
                    return View();
                }

            }


            public ActionResult Change_Pass()
            {
                return View();
            }

            [HttpPost]
            public ActionResult Change_Pass(Property p)
            {
                SqlConnection con = new SqlConnection(p.Con);
                con.Open();
                HttpCookie loginCookie = Request.Cookies["login_cookie"];
                string UserID = loginCookie.Values["UserName"];
                string str = "Update tbl_Login set Password='" + p.Password + "' where EmailID='" + enc.Decrypt(UserID) + "' and Password='" + p.OldPassword + "'";
                try
                {
                    SqlCommand cmd = new SqlCommand(str, con);
                    int i = cmd.ExecuteNonQuery();
                    if (i > 0)
                    {
                        TempData["Result"] = "Password Changed Successfully!!!";
                    }
                    else
                    {
                        TempData["Result"] = "Password Not Changed.";
                    }
                }
                catch (Exception ex)
                {
                    TempData["Result"] = ex.ToString();
                }
                con.Close();
                return View();
            }
          
            [AllowAnonymous]
            public string EMAILID_CHECK(string Id)
            {
                Datalayer dl = new Datalayer();
                DataSet ds = new DataSet();
                Property p1 = new Property();
                p1.Condition1 = Id;
                p1.Condition2 = "";
                p1.onTable = "EMAIL_CHECK";
                ds = dl.FETCH_CONDITIONAL_QUERY(p1);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    return "Email ID Already Exists.";
                }
                else
                {
                    return "Available";
                }
            }

         
        

           

           


        }
    }
