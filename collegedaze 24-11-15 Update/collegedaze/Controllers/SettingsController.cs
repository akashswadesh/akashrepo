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
    public class SettingsController : Controller
    {
        //
        // GET: /Settings/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Menu()
        {
            Datalayer dl = new Datalayer();
            DataSet ds = new DataSet();
            Property p1 = new Property();

            p1.Condition1 = "";
            p1.Condition2 = "";
            p1.Condition3 = "";
            p1.onTable = "MAIN_MENU";
            ds = dl.FETCH_CONDITIONAL_QUERY(p1);
            List<Property> Menu = new List<Property>();

            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    Property p = new Property();

                    p.id = ds.Tables[0].Rows[i]["id"].ToString();
                    p.Menu = ds.Tables[0].Rows[i]["Menu"].ToString();
                    p.Position = ds.Tables[0].Rows[i]["Position"].ToString();
                    p.Status = ds.Tables[0].Rows[i]["Status"].ToString();
                    Menu.Add(p);
                }
            }
            else
            {
                Property p = new Property();
                p.id = "0";
                p.Menu = "NONE";
                p.Position = "NONE";
                p.Status = "NONE";

                Menu.Add(p);
            }

            ViewBag.MenuList = Menu;
            return View();
        }


        [HttpPost]
        public ActionResult Menu(Property p)
        {
            Datalayer dl = new Datalayer();
            try
            {
                p.id = "0";

                if (dl.INSERT_MENU(p) > 0)
                {
                    TempData["MSG"] = "Record Saved Successfully!";
                    ModelState.Clear();
                }
            }
            catch (Exception ex)
            {
                TempData["MSG"] = ex.ToString();
            }

            return RedirectToAction("Menu");
        }

        public string Menu_Edit(string id)
        {
            Datalayer dl = new Datalayer();
            DataSet ds = new DataSet();
            Property p1 = new Property();

            p1.Condition1 = id;
            p1.Condition2 = "";
            p1.Condition3 = "";
            p1.onTable = "MAIN_MENU_EDIT";
            ds = dl.FETCH_CONDITIONAL_QUERY(p1);

            List<Property> Data = new List<Property>();
            string custData = "";
            if (ds.Tables[0].Rows.Count > 0)
            {
                custData = ds.Tables[0].Rows[0]["id"].ToString();
                custData += "##" + ds.Tables[0].Rows[0]["Menu"].ToString();
                custData += "##" + ds.Tables[0].Rows[0]["Position"].ToString();
                custData += "##" + ds.Tables[0].Rows[0]["Status"].ToString();
            }
            else
            {
                custData = "0";
                custData += "##" + "0";
                custData += "##" + "0";
                custData += "##" + "0";

            }
            return custData;

        }
        public string Menu_Update(string id, string menu, string pos,string Status)
        {
            Property p = new Property();
            SqlConnection con = new SqlConnection(p.Con);
            con.Open();
            string str = "UPDATE tbl_Menu SET Menu='" + menu + "',Position='" + pos + "', Status='" + Status + "' where id='" + id + "'";
            try
            {
                SqlCommand cmd = new SqlCommand(str, con);
                int i = cmd.ExecuteNonQuery();
                if (i > 0)
                {

                    return "SAVE";

                }
                else
                {
                    //TempData["MSG"] = "Record Not Update.";
                    return "NOTSAVE";
                }
            }
            catch (Exception ex)
            {
                TempData["MSG"] = ex.ToString();
            }
            con.Close();

            return "";
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

            List<SelectListItem> Menu = new List<SelectListItem>();

            Menu.Add(new SelectListItem { Text = "-SELECT-", Value = "0" });

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                Menu.Add(new SelectListItem { Text = ds.Tables[0].Rows[i][1].ToString(), Value = ds.Tables[0].Rows[i][0].ToString() });
            }
            ViewBag.MenuList = new SelectList(Menu, "Value", "Text");

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

            if (Request.QueryString["type"].ToString() == "Menu")
            {
                p1.onTable = "";
                str = "DELETE FROM tbl_Menu WHERE id='" + p1.id + "'";
                ds = dl.FETCH_CONDITIONAL_QUERY(p1);

            }
            
            else if (Request.QueryString["type"].ToString() == "SubMenu")
            {
                p1.onTable = "";
                str = "DELETE FROM tbl_SubMenu WHERE id='" + p1.id + "'";
                ds = dl.FETCH_CONDITIONAL_QUERY(p1);
            }
            else if (Request.QueryString["type"].ToString() == "SubMenuLevel")
            {
                p1.onTable = "";
                str = "DELETE FROM tbl_SubMenu_Level2 WHERE id='" + p1.id + "'";
                ds = dl.FETCH_CONDITIONAL_QUERY(p1);
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


            if (Request.QueryString["type"].ToString() == "Menu")
            {
                return RedirectToAction("Menu", "Settings");
            }
            else if (Request.QueryString["type"].ToString() == "SubMenu")
            {
                return RedirectToAction("SubMenu", "Settings");
            }

            else if (Request.QueryString["type"].ToString() == "SubMenuLevel")
            {
                return RedirectToAction("SubMenuLevel2", "Settings");
            }
            else
            {
                return RedirectToAction("Index", "Admin");
            }

        }







    }
}
