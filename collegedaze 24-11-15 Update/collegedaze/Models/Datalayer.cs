using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using collegedaze.Helpers;

namespace collegedaze.Models
{
    public class Datalayer
    {
        public static byte[] pImage;

        public int Int_Process(String Storp, string[] parametername, string[] parametervalue)
        {
            int a = 0;
            Property p = new Property();
            SqlConnection con = new SqlConnection(p.Con);
            SqlCommand cmd = new SqlCommand(Storp, con);
            cmd.CommandType = CommandType.StoredProcedure;
            for (int i = 0; i < parametername.Length; i++)
            {
                if (parametername[i] == "@img")
                {
                    cmd.Parameters.AddWithValue(parametername[i], pImage);
                }
                else
                {
                    cmd.Parameters.AddWithValue(parametername[i], parametervalue[i]);
                }
            }
            con.Open();

            a = cmd.ExecuteNonQuery();
            con.Dispose();
            return a;
        }
        public DataSet Ds_Process(String Storp, string[] parametername, string[] parametervalue)
        {
            try
            {
                Property p = new Property();
                SqlConnection con = new SqlConnection(p.Con);
                SqlCommand cmd = new SqlCommand(Storp, con);
                cmd.CommandType = CommandType.StoredProcedure;
                for (int i = 0; i < parametername.Length; i++)
                {
                    cmd.Parameters.AddWithValue(parametername[i], parametervalue[i]);
                }
                con.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds);
                da.Dispose();
                con.Dispose();
                return ds;
            }
            catch (Exception ex)
            {
                DataSet ds = null;
                return ds;
            }

        }
        public DataSet MyDs_Process(String Storp)
        {

            Property p = new Property();
            SqlConnection con = new SqlConnection(p.Con);
            SqlCommand cmd = new SqlCommand(Storp, con);
            cmd.CommandType = CommandType.StoredProcedure;

            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            da.Dispose();
            con.Dispose();
            return ds;

        }

        //----------------------Data Access Layer Work---------------------------

        EncryptDecrypt enc = new EncryptDecrypt();

        public DataSet FETCH_LOGIN_DETAILS(Property p)
        {
            try
            {
                string[] paname = { "@EmailID", "@Password" };
                string[] pvalue = { p.EmailID, p.Password };
                return Ds_Process("FETCH_LOGIN_DETAILS", paname, pvalue);
            }
            catch
            {
                throw;
            }
        }

        public DataSet FETCH_CONDITIONAL_QUERY(Property p)
        {
            try
            {
                string[] paname = { "@Condition1", "@Condition2", "@Condition3", "@onTable" };
                string[] pvalue = { p.Condition1, p.Condition2, p.Condition3, p.onTable };
                return Ds_Process("FETCH_CONDITIONAL_QUERY", paname, pvalue);
            }
            catch
            {
                throw;
            }
        }

        public int INSERT_MENU(Property p)
        {
            try
            {
                string[] paname = { "@id", "@Menu", "@Position", "@Status" };
                string[] pvalue = { p.id, p.Menu, p.Position, p.Status };
                return Int_Process("INSERT_MENU", paname, pvalue);
            }
            catch
            {
                throw;
            }
        }

        public int INSERT_UPDATE_CONTENT(Property p)
        {
            try
            {
                string[] paname = { "@id", "@Title", "@Description", "@ThumbImgURL", "@ImgURL", "@Menu", "@SubMenu", "@SubMenuLevel", "@Status", "@URL" };
                string[] pvalue = { p.id, p.Title, p.Description, p.ThumbImgURL, p.ImgURL, p.Menu, p.SubMenu, p.SubMenuLevel2, p.Status, p.URLTitle };
                return Int_Process("INSERT_UPDATE_CONTENT", paname, pvalue);
            }
            catch
            {
                throw;
            }
        }
     
    }
}