using collegedaze.Helpers;
using collegedaze.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace collegedaze.Controllers
{
    public class ContentController : Controller
    {
        //
        // GET: /Content/

        public ActionResult Index()
        {
            return View();
        }
        Datalayer dl = new Datalayer();
        public void ListMenu()
        {
            Datalayer dl = new Datalayer();
            DataSet ds = new DataSet();
            Property p1 = new Property();

            p1.Condition1 = "";
            p1.Condition2 = "";
            p1.Condition3 = "";
            p1.onTable = "LIST_MENU_DROPDOWN";

            ds = dl.FETCH_CONDITIONAL_QUERY(p1);

            List<SelectListItem> Menu = new List<SelectListItem>();

            Menu.Add(new SelectListItem { Text = "-SELECT-", Value = "0" });

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                Menu.Add(new SelectListItem { Text = ds.Tables[0].Rows[i][1].ToString(), Value = ds.Tables[0].Rows[i][0].ToString() });
            }
            ViewBag.MenuList = new SelectList(Menu, "Value", "Text");

        }

        public JsonResult SubMenuList(string Id)
        {
            Datalayer dl = new Datalayer();
            DataSet ds = new DataSet();
            Property p1 = new Property();
            p1.Condition1 = Id;
            p1.Condition2 = "";
            p1.Condition3 = "";
            p1.onTable = "SUB_MENU_DROPDOWN";
            ds = dl.FETCH_CONDITIONAL_QUERY(p1);

            List<SelectListItem> DropDown = new List<SelectListItem>();
            DropDown.Add(new SelectListItem { Text = "-select-", Value = "0" });
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                DropDown.Add(new SelectListItem { Text = ds.Tables[0].Rows[i][1].ToString(), Value = ds.Tables[0].Rows[i][0].ToString() });
            }

            return Json(new SelectList(DropDown, "Value", "Text"), JsonRequestBehavior.AllowGet);

        }

        public JsonResult SubMenuLevel(string Id)
        {
            Datalayer dl = new Datalayer();
            DataSet ds = new DataSet();
            Property p1 = new Property();
            p1.Condition1 = Id;
            p1.Condition2 = "";
            p1.Condition3 = "";
            p1.onTable = "SUB_LEVEL_DROPDOWN";
            ds = dl.FETCH_CONDITIONAL_QUERY(p1);

            List<SelectListItem> DropDown = new List<SelectListItem>();
            DropDown.Add(new SelectListItem { Text = "-select-", Value = "0" });
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                DropDown.Add(new SelectListItem { Text = ds.Tables[0].Rows[i][1].ToString(), Value = ds.Tables[0].Rows[i][0].ToString() });
            }

            return Json(new SelectList(DropDown, "Value", "Text"), JsonRequestBehavior.AllowGet);

        }

        public ActionResult Add()
        {
            ListMenu();
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Add(Property p)
        {
            int check=0;
            SqlConnection con = new SqlConnection(p.Con);
            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand("select Menu from tbl_Content where Menu='" + p.Menu + "'", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            if (ds.Tables[0].Rows.Count > 0)
            {
              check =Convert.ToInt32(ds.Tables[0].Rows[0]["Menu"].ToString());
            }

            if (check !=1 && check !=2 && check !=3 )
            {
             try
            {

                string fileLocation = "";
                string ThumbfileLocation = "";
                string ItemUploadFolderPath = "~/DataImages/Content/";
                string ThumbItemUploadFolderPath = "~/DataImages/Content/Thumbnail/";

                foreach (string item in Request.Files)
                {
                    HttpPostedFileBase file = Request.Files[item] as HttpPostedFileBase;
                    if (file.ContentLength == 0)
                        continue;

                    if (file.ContentLength > 0)
                    {
                        fileLocation = HelperFunctions.renameUploadFile(file, ItemUploadFolderPath);
                        ThumbfileLocation = HelperFunctions.ThumbrenameUploadFile(file, ThumbItemUploadFolderPath, 1); //Thumbnail
                    }
                }
                if (fileLocation == "")
                {
                    fileLocation = "";
                    ThumbfileLocation = "";
                }
                p.id = "0";
                if(p.Menu==null)
                {
                    p.Menu="";
                }
                if (p.SubMenu == null)
                {
                    p.SubMenu ="";
                }
                if (p.SubMenuLevel2 == null)
                {
                    p.SubMenuLevel2 = "";
                }
                if (p.ThumbImgURL == null)
                {
                    p.ThumbImgURL = "";
                }
                if (p.ImgURL == null)
                {
                    p.ImgURL = "";
                }
                if (p.Title == null)
                {
                    p.Title = "";
                }
                if (p.URLTitle == null)
                {
                    p.URLTitle = "";
                }
                p.ImgURL = fileLocation;
                p.ThumbImgURL = ThumbfileLocation;

                if (dl.INSERT_UPDATE_CONTENT(p) > 0)
                {
                    TempData["MSG"] = "Record Saved Successfully!!";
                    ModelState.Clear();
                }
                else
                {
                    TempData["MSG"] = "Record not Saved";
                }
            }
            catch (Exception ex)
            {
                TempData["MSG"] = ex.ToString();
            }
            }
            else{

                TempData["MSG"] = "You Have Already Insert Content In About Or Our Creation Or Contact Or Logo Or Head Social Section Please Go For Only Update..";
            }
            ListMenu();
            return View();
        }

        [ValidateInput(false)]
        public ActionResult List()
        {

            Property p1 = new Property();
            DataSet ds = new DataSet();
            p1.Condition1 = "";
            p1.Condition2 = "";
            p1.Condition3 = "";
            p1.onTable = "CONTENT_LIST";
            ds = dl.FETCH_CONDITIONAL_QUERY(p1);
            List<Property> Detail = new List<Property>();
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    Property p = new Property();
                    p.id = ds.Tables[0].Rows[i]["id"].ToString();
                    p.Title = ds.Tables[0].Rows[i]["Title"].ToString();
                    p.Menu = ds.Tables[0].Rows[i]["Menu"].ToString();
                    p.Status = ds.Tables[0].Rows[i]["Status"].ToString();
                    Detail.Add(p);
                }
            }
            else
            {
                Property p = new Property();
                p.id = "0";
                p.Title = "NONE";
                p.Menu = "NONE";
                p.Status = "NONE";
                Detail.Add(p);
            }

            ViewBag.DetailList = Detail;

            return View();
        }

        public ActionResult Edit()
        {
            ListMenu();
            Property p1 = new Property();
            DataSet ds = new DataSet();
            List<Property> Upload = new List<Property>();
            try
            {
                p1.id = Request.QueryString["ID"].ToString();
                if (p1.id == "0")
                {
                    p1.id = "";
                }
            }
            catch
            {
                p1.id = "";

            }

            p1.Condition1 = p1.id;
            p1.Condition2 = "";
            p1.Condition3 = "";

            p1.onTable = "CONTENT_EDIT";

            ds = dl.FETCH_CONDITIONAL_QUERY(p1);

            if (ds.Tables[0].Rows.Count > 0)
            {
                var info = new collegedaze.Models.Property()
                {
                    id = ds.Tables[0].Rows[0]["id"].ToString(),
                    Menu = ds.Tables[0].Rows[0]["Menu"].ToString(),
                    Title = ds.Tables[0].Rows[0]["Title"].ToString(),
                    Description = ds.Tables[0].Rows[0]["Description"].ToString(),
                    ThumbImgURL = ds.Tables[0].Rows[0]["ThumbImgURL"].ToString().Replace("~", ""),
                    Status = ds.Tables[0].Rows[0]["Status"].ToString(),
                    URLTitle = ds.Tables[0].Rows[0]["URL"].ToString(),
                };

               return View(info);
            }

            else
            {
                return RedirectToAction("index");
            }
            return View();


        }


        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(Property p)
        {
            try
            {

                if (p.Title == null)
                {
                    p.Title = "";
                }

               
                if (p.Menu == null)
                {

                    p.Menu = "";

                }
                if (p.SubMenu == null)
                {
                    p.SubMenu = "";
                }
                if (p.SubMenuLevel2 == null)
                {
                    p.SubMenuLevel2 = "";
                }
                if (p.Status == null)
                {
                    p.Status = "";
                }
                if (p.URLTitle == null)
                {
                    p.URLTitle = "";
                }
                p.ImgURL = "";
                p.ThumbImgURL = "";



                if (dl.INSERT_UPDATE_CONTENT(p) > 0)
                {
                    TempData["MSG"] = "Your Record Updated Successfully ";
                    //ModelState.Clear();
                }
                else
                {
                    TempData["MSG"] = "Your Record Not Inserted";
                }
            }
            catch (Exception ex)
            {
                TempData["MSG"] = ex.ToString();
            }
            return RedirectToAction("List");
        }


        [HttpPost]
        public ActionResult Edit_Pic(Property p)
        {
            string fileLocation = "";
            string ThumbfileLocation = "";
            string ItemUploadFolderPath = "~/DataImages/Content/";
            string ThumbItemUploadFolderPath = "~/DataImages/Content/Thumbnail/";

            foreach (string item in Request.Files)
            {
                HttpPostedFileBase file = Request.Files[item] as HttpPostedFileBase;
                if (file.ContentLength == 0)
                    continue;

                if (file.ContentLength > 0)
                {
                    fileLocation = HelperFunctions.renameUploadFile(file, ItemUploadFolderPath);
                    ThumbfileLocation = HelperFunctions.ThumbrenameUploadFile(file, ThumbItemUploadFolderPath, 1); //Thumbnail
                }
            }

            if (fileLocation == "")
            {
                TempData["MSG"] = "Record Not Updated.";
            }
            else
            {
                p.ImgURL = fileLocation;
                p.ThumbImgURL = ThumbfileLocation;

                SqlConnection con = new SqlConnection(p.Con);
                con.Open();

                string str = "update tbl_Content set ImgURL='" + p.ImgURL + "', ThumbImgURL='" + p.ThumbImgURL + "' where id='" + p.id + "'";
                try
                {
                    SqlCommand cmd = new SqlCommand(str, con);
                    int i = cmd.ExecuteNonQuery();
                    if (i > 0)
                    {
                        TempData["MSG"] = "Record Updated Successfully!!!";
                    }
                    else
                    {
                        TempData["MSG"] = "Record Not Updated.";
                    }
                }
                catch (Exception ex)
                {
                    TempData["MSG"] = ex.ToString();
                }
                con.Close();
            }

            return RedirectToAction("List");
        }

        public ActionResult Delete()
        {
            Datalayer dl = new Datalayer();
            DataSet ds = new DataSet();
            Property p1 = new Property();

            string str = "";
            p1.ImgURL = "";
            p1.ThumbImgURL = "";
            p1.id = Request.QueryString["id"].ToString();
            p1.Condition1 = Request.QueryString["id"].ToString();
            p1.Condition2 = "";
            p1.Condition3 = "";

           if (Request.QueryString["type"].ToString() == "Content")
            {
                p1.onTable = "CONTENT_EDIT";
                str = "DELETE FROM tbl_Content WHERE id='" + p1.id + "'";
                ds = dl.FETCH_CONDITIONAL_QUERY(p1);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    p1.ImgURL = ds.Tables[0].Rows[0]["ImgURL"].ToString();
                    p1.ThumbImgURL = ds.Tables[0].Rows[0]["ThumbImgURL"].ToString();

                }
                string completIMG = Server.MapPath(p1.ImgURL);
                string completeThumb = Server.MapPath(p1.ThumbImgURL);
                try
                {
                    if (System.IO.File.Exists(completIMG))
                    {
                        System.IO.File.Delete(completIMG);

                    }
                    if (System.IO.File.Exists(completeThumb))
                    {
                        System.IO.File.Delete(completeThumb);

                    }
                }
                catch (Exception ex)
                {

                    TempData["MSG"] = ex.ToString();

                }
            }
           else if (Request.QueryString["type"].ToString() == "Contact")
           {
               p1.onTable = "CONTENT_EDIT";
               str = "DELETE FROM tbl_Contact WHERE id='" + p1.id + "'";
               ds = dl.FETCH_CONDITIONAL_QUERY(p1);
           }
           else if (Request.QueryString["type"].ToString() == "Multiple")
           {
               p1.onTable = "MULTIPLE_URL";
               str = "DELETE FROM tbl_Multiple WHERE id='" + p1.id + "'";
               ds = dl.FETCH_CONDITIONAL_QUERY(p1);

               if (ds.Tables[0].Rows.Count > 0)
               {
                   p1.ImgURL = ds.Tables[0].Rows[0]["ImgURL"].ToString();
                   p1.ThumbImgURL = ds.Tables[0].Rows[0]["ThumbImgURL"].ToString();

               }
               string completIMG = Server.MapPath(p1.ImgURL);
               string completeThumb = Server.MapPath(p1.ThumbImgURL);
               try
               {
                   if (System.IO.File.Exists(completIMG))
                   {
                       System.IO.File.Delete(completIMG);

                   }
                   if (System.IO.File.Exists(completeThumb))
                   {
                       System.IO.File.Delete(completeThumb);

                   }
               }
               catch (Exception ex)
               {

                   TempData["MSG"] = ex.ToString();

               }
           }
           else
           {
               p1.onTable = "";
               str = "";

           }

            SqlConnection con = new SqlConnection(p1.Con);
            con.Open();
            try
            {
                SqlCommand cmd = new SqlCommand(str, con);
                int i = cmd.ExecuteNonQuery();
                if (i > 0)
                {
                    TempData["MSG"] = "Data Deleted Successfully!!!";
                }
                else
                {
                    TempData["MSG"] = "Record Not Deleted.";
                }

            }
            catch (Exception ex)
            {
                TempData["MSG"] = ex.ToString();
            }
            con.Close();


            if (Request.QueryString["type"].ToString() == "Content")
            {
                return RedirectToAction("List", "Content");
            }
            else if (Request.QueryString["type"].ToString() == "Contact")
            {
                return RedirectToAction("Contact", "Content");
            }

            else if (Request.QueryString["type"].ToString() == "Multiple")
            {
                return RedirectToAction("Multiple", "Content");
            }
            else
            {
                return RedirectToAction("Index", "Admin");
            }

        }

        public ActionResult Home()
        {
            Property p = new Property();
            p.Condition1 ="HOME";
            p.Condition2 = "";
            p.Condition3 = "";
            p.onTable = "HOME_CONTENT";

            Datalayer dl = new Datalayer();
            DataSet ds = new DataSet();
            ds = dl.FETCH_CONDITIONAL_QUERY(p);

            var home = new collegedaze.Models.Property()
            {
                id = ds.Tables[0].Rows[0]["id"].ToString(),
                Title = ds.Tables[0].Rows[0]["Title"].ToString(),
                Description = ds.Tables[0].Rows[0]["Description"].ToString(),
            };
            return View(home);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Home(Property p)
        {

            SqlConnection con = new SqlConnection(p.Con);
            con.Open();
            string str = "UPDATE tbl_Content SET Title='" + p.Title.Replace("'", "''") + "', Description='" + p.Description.Replace("'", "''") + "' where Type='HOME'";
            try
            {
                SqlCommand cmd = new SqlCommand(str, con);
                int i = cmd.ExecuteNonQuery();
                if (i > 0)
                {
                    TempData["MSG"] = "Record Update Successfully";
                }
                else
                {
                    TempData["MSG"] = "Record Not Update.";

                }
            }
            catch (Exception ex)
            {
                TempData["MSG"] = ex.ToString();
            }
            con.Close();

            return Redirect("/Content/Home/");
        }

        public ActionResult Contact()
        {
            Property p1 = new Property();
            DataSet ds = new DataSet();
            p1.Condition1 = "";
            p1.Condition2 = "";
            p1.Condition3 = "";
            p1.onTable = "CONTACT_LIST";
            ds = dl.FETCH_CONDITIONAL_QUERY(p1);
            List<Property> Contact = new List<Property>();
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    Property p = new Property();

                    p.id = ds.Tables[0].Rows[i]["id"].ToString();
                    p.FullName = ds.Tables[0].Rows[i]["FullName"].ToString();
                    p.EmailID = ds.Tables[0].Rows[i]["EmailID"].ToString();
                    p.Message = ds.Tables[0].Rows[i]["Message"].ToString();
                    p.onDate = ds.Tables[0].Rows[i]["onDate"].ToString();
                  
                     Contact.Add(p);
                }
            }
            else
            {
                Property p = new Property();
                p.id = "0";

                Contact.Add(p);
            }

            ViewBag.ContactList = Contact;
            return View();
            
        }

        public ActionResult Quote()
        {
            Property p1 = new Property();
            DataSet ds = new DataSet();
            p1.Condition1 = "";
            p1.Condition2 = "";
            p1.Condition3 = "";
            p1.onTable = "QUOTE_LIST";
            ds = dl.FETCH_CONDITIONAL_QUERY(p1);
            List<Property> Quote = new List<Property>();
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    Property p = new Property();

                    p.id = ds.Tables[0].Rows[i]["id"].ToString();
                    p.FullName = ds.Tables[0].Rows[i]["FullName"].ToString();
                    p.EmailID = ds.Tables[0].Rows[i]["EmailID"].ToString();
                    p.Message = ds.Tables[0].Rows[i]["Message"].ToString();
                    p.ContactNo = ds.Tables[0].Rows[i]["ContactNo"].ToString();
                    p.Modal = ds.Tables[0].Rows[i]["Modal"].ToString();
                    p.Suburb = ds.Tables[0].Rows[i]["Suburb"].ToString();
                    p.onDate = ds.Tables[0].Rows[i]["onDate"].ToString();

                    Quote.Add(p);
                }
            }
            else
            {
                Property p = new Property();
                p.id = "0";

                Quote.Add(p);
            }

            ViewBag.QuoteList = Quote;
            return View();
            
        }

        public void ListTitle()
        {
            Datalayer dl = new Datalayer();
            DataSet ds = new DataSet();
            Property p1 = new Property();

            p1.Condition1 = "";
            p1.Condition2 = "";
            p1.Condition3 = "";
            p1.onTable = "GALLERY_DROPDOWN";

            ds = dl.FETCH_CONDITIONAL_QUERY(p1);

            List<SelectListItem> Title = new List<SelectListItem>();

            Title.Add(new SelectListItem { Text = "-SELECT-", Value = "0" });

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                Title.Add(new SelectListItem { Text = ds.Tables[0].Rows[i]["Title"].ToString(), Value = ds.Tables[0].Rows[i]["id"].ToString() });
            }
            ViewBag.TitleList = new SelectList(Title, "Value", "Text");

        }
   
        public JsonResult SearchList(string title)
        {
            Datalayer dl = new Datalayer();
            DataSet ds = new DataSet();
            Property p = new Property();

            p.Condition1 = title;
            p.Condition2 = "";
            p.Condition3 = "";
            p.onTable = "MULTIPLE_LIST";
            ds = dl.FETCH_CONDITIONAL_QUERY(p);

            List<Property> Data = new List<Property>();
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    Property p1 = new Property();
                    p1.id = ds.Tables[0].Rows[i]["id"].ToString();
                    p1.Title=ds.Tables[0].Rows[i]["Gallery"].ToString();
                    p1.ThumbImgURL = ds.Tables[0].Rows[i]["ThumbImgURL"].ToString().Replace("~", "");
                   
                    Data.Add(p1);
                }

            }
            else
            {
                Property p1 = new Property();
                p1.id = "";
                Data.Add(p1);
            }
            return Json(Data, JsonRequestBehavior.AllowGet);
        }


        


    }
}
