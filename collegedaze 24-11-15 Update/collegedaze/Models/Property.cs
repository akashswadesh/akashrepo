using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace collegedaze.Models
{
    public class Property
    {
       //   private string con = "Data Source=WEB2-PC\\AKASH;Initial Catalog=JadoCreation;User ID=sa;Password=sql@2008";
        private string con = "data source=64.16.214.16,1986; user id=sa;password=;initial catalog=collegedaze";
        public string Con
        {
            get
            {
                return con;
            }
        }
        public string id { get; set; }
        public string Password { get; set; }
        public string CnfPassword { get; set; }
        public string FileLocation { get; set; }
        public string Status { get; set; }
        public string FullName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ContactNo { get; set; }
        public string Condition1 { get; set; }
        public string Condition2 { get; set; }
        public string Condition3 { get; set; }
        public string onTable{ get; set; }
        public string onDate { get; set; }
        public string EmailID { get; set; }
        public string OldPassword { get; set; }
        public string ImgURL { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
	    public string Country { get; set; }

        public string Menu { get; set; }
        public string SubMenu { get; set; }
        public string SubMenuLevel2 { get; set; }
        public string Position { get; set; }
        public string ThumbImgURL { get; set; }
        public string Title { get; set; }
        public string URLTitle { get; set; }
        public string Description { get; set; }
        public string SID { get; set; }
        public string Message { get; set; }

        public string Modal { get; set; }
        public string Suburb { get; set; }
        public string Type { get; set; }
        public string Subject { get; set; }

        
      

      
    }
}