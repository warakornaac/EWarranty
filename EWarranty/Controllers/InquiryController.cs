using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EWarranty.Controllers
{
    public class InquiryController : Controller
    {
        //
        // GET: /Inquiry/

        public ActionResult Index()
        {
            
            if (Session["UserID"] == null && Session["UserPassword"] == null)
            {
                return RedirectToAction("LogIn", "Account");
            }
            else
            {

                string User = Session["UserID"].ToString();
                string UserType = Session["UserType"].ToString();



                List<EWarranty.Models.GroupClass.DefineCode> List = new List<EWarranty.Models.GroupClass.DefineCode>();
                //DefineCode model = null;
                var connectionString = ConfigurationManager.ConnectionStrings["CLAIM_ConnectionString"].ConnectionString;
                SqlConnection Connection = new SqlConnection(connectionString);
                var command = new SqlCommand("P_Get_DefineCode", Connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@DefineID", 67);
                Connection.Open();
                SqlDataReader dr = command.ExecuteReader();
                while (dr.Read())
                {

                    List.Add(new EWarranty.Models.GroupClass.DefineCode()
                    {
                        ID = dr["ID"].ToString(),
                        CODE = dr["CODE"].ToString(),
                        FIELD_RELATION = dr["FIELD_RELATION"].ToString(),
                        DESCRIPTION_TH = dr["DESCRIPTION_TH"].ToString(),
                        DESCRIPTION_EN = dr["DESCRIPTION_EN"].ToString(),
                        INACTIVE = dr["INACTIVE"].ToString()
                    });
                }
                dr.Close();
                dr.Dispose();
                command.Dispose();
                Connection.Close();


                ViewBag.Status = List;
                ViewBag.UserId = User;
                ViewBag.UserType = UserType;
            }
            return View();
        }

    }
}
