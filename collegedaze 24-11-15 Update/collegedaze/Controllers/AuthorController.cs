using collegedaze.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace collegedaze.Controllers
{
    public class AuthorController : Controller
    {
        //
        // GET: /Author/

        public ActionResult Index(string id)
        {
            HomeContent(id);
            ListMenu();
            return View();
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


        public void HomeContent(string Id)
        {
            Property p = new Property();
            Datalayer dl = new Datalayer();
            DataSet ds = new DataSet();
            p.Condition1 = Id;
            p.Condition2 = "";
            p.Condition3 = "";
            p.onTable = "FETCH_HOME_CONTENT";
            ds = dl.FETCH_CONDITIONAL_QUERY(p);
            List<Property> Detail = new List<Property>();

            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {


                    p.id = ds.Tables[0].Rows[i]["id"].ToString();
                    p.Title = ds.Tables[0].Rows[i]["Title"].ToString();
                    p.Menu = ds.Tables[0].Rows[i]["Menu"].ToString();
                    p.Description = ds.Tables[0].Rows[i]["Description"].ToString();
                    p.ThumbImgURL = ds.Tables[0].Rows[i]["ThumbImgURL"].ToString().Replace("~", "");
                    p.ImgURL = ds.Tables[0].Rows[i]["ImgURL"].ToString().Replace("~", "");


                    Detail.Add(p);
                }
            }
            else
            {

                p.id = "0";
                p.Title = "NONE";
                p.Menu = "NONE";
                p.Description = "NONE";
                Detail.Add(p);
            }

            ViewBag.contentlist = Detail;



        }
      



    }
}
