using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EWarranty.Models;
using System.Data;

using System.Security.Principal;
using System.Web.Security;
using System.DirectoryServices;
using System.Runtime.InteropServices;
using System.Globalization;
using System.Net.Http.Headers;

namespace EWarranty.Controllers
{
    public class EditWarrantyregisterController : Controller
    {
        //
        // GET: /EditWarrantyregister/

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
                string UserPassword = Session["UserPassword"].ToString();
                string id = Session["ID"].ToString();
                ViewBag.UserId = User;
                ViewBag.UserType = UserType;

                ViewBag.UserPassword = UserPassword;
                ViewBag.id = id;

              string Name  =string.Empty;
			  string LastName =string.Empty;
			  string Password =string.Empty;
              string UsrTyp =string.Empty;
			  string Address01 =string.Empty;
			  string Address02 =string.Empty;
			  string Province=string.Empty;
			  string Postcode =string.Empty;
			  string EMail =string.Empty;
			  string Tel  =string.Empty;
		 	  string LineID =string.Empty;
			  string Logon =string.Empty;
			  string LastLogOn =string.Empty;
			  string SessionId =string.Empty;
			  string pwdLastSet =string.Empty;
			  string Insertdate =string.Empty;
			  string LoginFail =string.Empty;
			  string Online =string.Empty;
              string District =string.Empty;
              string SubDistrict = string.Empty;
                var connectionString = ConfigurationManager.ConnectionStrings["CLAIM_ConnectionString"].ConnectionString;
                SqlConnection Connection = new SqlConnection(connectionString);

                var command = new SqlCommand("P_GetRegister_customer_ewaranty", Connection);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@inUsrID ", User);
                Connection.Open();

                SqlDataReader dr = command.ExecuteReader();
                while (dr.Read())
                {

                        Name = dr["Name"].ToString();
                        LastName = dr["Last Name"].ToString();
                        Password = dr["Password"].ToString();
                        UsrTyp = dr["UsrTyp"].ToString();
                        Address01 = dr["Address01"].ToString();
                        Address02 = dr["Address02"].ToString();
                        Province = dr["Province"].ToString();
                        Postcode = dr["Postcode"].ToString();
                        EMail = dr["E-Mail"].ToString();
                        Tel = dr["Tel"].ToString();
                        LineID = dr["LineID"].ToString();
                        SubDistrict = dr["Subdistrict"].ToString();
                        District = dr["District"].ToString();
                       
                }
                dr.Close();
                dr.Dispose();
                command.Dispose();
                Connection.Close();

           
                ViewBag.Name = Name;
                ViewBag.LastName = LastName;
                ViewBag.Password = Password;
                ViewBag.UsrTyp = UsrTyp;
                ViewBag.Address01 = Address01;
                ViewBag.Address02 = Address02;
                ViewBag.Province = Province;
                ViewBag.Postcode = Postcode;
                ViewBag.EMail = EMail;
                ViewBag.Tel = Tel;
                ViewBag.LineID = LineID;
                ViewBag.SubDistrict = SubDistrict;
                ViewBag.District = District;


            }
            return View();
        }

        public JsonResult EditRegister(string b_name, string b_lastname, string b_address1, string b_district, string b_amphoe, string b_province, string b_zipcode, string b_tel, string b_lineid, string b_email)
        {
            string message = string.Empty;
            string no = string.Empty;
            var connectionString = ConfigurationManager.ConnectionStrings["CLAIM_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            try
            {
                Connection.Open();

                var command = new SqlCommand("P_EditRegister_customer_ewaranty", Connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@inUsrID", Session["UserID"].ToString());
                command.Parameters.AddWithValue("@inname", b_name);
                command.Parameters.AddWithValue("@inlastname", b_lastname);
                command.Parameters.AddWithValue("@inaddress1", b_address1);
                //command.Parameters.AddWithValue("@inaddress2", b_address2);
                command.Parameters.AddWithValue("@indistrict", b_district);
                command.Parameters.AddWithValue("@inamphoe", b_amphoe);
                command.Parameters.AddWithValue("@inprovince", b_province);
                command.Parameters.AddWithValue("@inzipcode", b_zipcode);
                command.Parameters.AddWithValue("@intel", b_tel);
                command.Parameters.AddWithValue("@inlineid", b_lineid);
                command.Parameters.AddWithValue("@inemail ", b_email);

                SqlParameter returnValuedoc = new SqlParameter("@outGenstatus", SqlDbType.NVarChar, 100);
                returnValuedoc.Direction = System.Data.ParameterDirection.Output;
                command.Parameters.Add(returnValuedoc);



                command.ExecuteNonQuery();
                message = returnValuedoc.Value.ToString();
                command.Dispose();
                // message = "true";

            }
            catch (Exception ex)
            {
                message = ex.Message;
            }


            Connection.Close();

            return Json(new { message }, JsonRequestBehavior.AllowGet);
        }
    }
}
