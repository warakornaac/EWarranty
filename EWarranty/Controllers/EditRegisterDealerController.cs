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
    public class EditRegisterDealerController : Controller
    {
        //
        // GET: /EditRegisterDealer/

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

                string Name = string.Empty;
                string LastName = string.Empty;
                string Password = string.Empty;
                string UsrTyp = string.Empty;
                string Address01 = string.Empty;
                string Logon = string.Empty;
                string LastLogOn = string.Empty;
                string SessionId = string.Empty;
                string pwdLastSet = string.Empty;
                string Insertdate = string.Empty;
                string LoginFail = string.Empty;
                string Online = string.Empty;
                string District = string.Empty;
                string SubDistrict = string.Empty;
                string Cuscode = string.Empty;
                string Shipto = string.Empty;
                string Tel = string.Empty;
                string EMail = string.Empty;
                string Password_Endcode = string.Empty;
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
                    Tel = dr["Tel"].ToString();
                    EMail = dr["E-Mail"].ToString();
                    SubDistrict = dr["Subdistrict"].ToString();
                    District = dr["District"].ToString();
                    Cuscode = dr["CusCode"].ToString();
                    Shipto = dr["ShipTo"].ToString();
                    Password_Endcode = dr["Password_Endcode"].ToString();

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
                ViewBag.SubDistrict = SubDistrict;
                ViewBag.District = District;
                ViewBag.Cuscode = Cuscode;
                ViewBag.Shipto = Shipto;
                ViewBag.Tel = Tel;
                ViewBag.EMail = EMail;
                ViewBag.Password_Endcode = Password_Endcode;


            }
            return View();
        }
        public JsonResult EditRegisterDealer(string b_name, string b_code, string b_password,  string b_address1, string b_tel, string b_email)
        {
            string message = string.Empty;
            string no = string.Empty;
            var connectionString = ConfigurationManager.ConnectionStrings["CLAIM_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            try
            {
                Connection.Open();
                var command = new SqlCommand("P_Edit_Register_dealer_ewaranty", Connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@inname", b_name);
                command.Parameters.AddWithValue("@incuscode", b_code);
                command.Parameters.AddWithValue("@inpassword", b_password);
                command.Parameters.AddWithValue("@inaddress1", b_address1);
                command.Parameters.AddWithValue("@intel", b_tel);
                command.Parameters.AddWithValue("@inemail ", b_email);
                SqlParameter returnValuedoc = new SqlParameter("@outGenstatus", SqlDbType.NVarChar, 100);
                returnValuedoc.Direction = System.Data.ParameterDirection.Output;
                command.Parameters.Add(returnValuedoc);
                command.ExecuteNonQuery();
                message = returnValuedoc.Value.ToString();
                command.Dispose();
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
