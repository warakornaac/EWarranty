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
using System.Net.Http;
using System.Net;
using System.Threading.Tasks;
using System.Threading;


namespace EWarranty.Controllers
{
    public class AccountController : Controller
    {
        //
        // GET: /Account/

        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult LogIn()
        {

            return View();

        }
        [HttpGet]
        public ActionResult CheckLoginExternal()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CheckDataLoginExternal(string userId, string email, string displayName)
        {
            string message = string.Empty;
            var connectionString = ConfigurationManager.ConnectionStrings["CLAIM_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            try
            {
                Connection.Open();
                var command = new SqlCommand("P_Check_Login_External", Connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@userId", userId);
                command.Parameters.AddWithValue("@email", email);
                command.Parameters.AddWithValue("@displayName", displayName);

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
        public JsonResult GetDataLoginExternal(string userId, string page)
        {
            this.Session["UserID"] = string.Empty;
            this.Session["UserType"] = string.Empty;
            string message = string.Empty;
            var connectionString = ConfigurationManager.ConnectionStrings["CLAIM_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            Connection.Open();
            try
            {
                SqlCommand cmd = new SqlCommand("select * From UsrTbl where UserIdLine =N'" + userId + "' and  [LoginFail] <> 3", Connection);
                SqlDataReader rev = cmd.ExecuteReader();
                while (rev.Read())
                {
                    if (!string.IsNullOrEmpty(rev["UsrID"].ToString()))
                    {
                        message = "Y";
                        this.Session["UserID"] = rev["UsrID"].ToString();
                        this.Session["UserType"] = rev["UsrTyp"].ToString();
                        //get sesssion
                        string sessionId = string.Empty;
                        string httpCookie = string.Empty;
                        if (Request.ServerVariables["HTTP_COOKIE"] != null)
                        {
                            httpCookie = Request.ServerVariables["HTTP_COOKIE"].Substring(0, (Request.ServerVariables["HTTP_COOKIE"].Length > 399) ? 399 : Request.ServerVariables["HTTP_COOKIE"].Length);
                        }
                        sessionId = httpCookie;

                        if (sessionId != null)
                        {
                            sessionId = sessionId.Substring(sessionId.Length - 24);
                            this.Session["ID"] = sessionId;
                        }
                        else
                        {
                            this.Session["ID"] = "775.333";
                        }
                        //set session id
                        var command = new SqlCommand("P_logSingin_customer_ewaranty", Connection);
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@UsrID", userId);
                        command.Parameters.AddWithValue("@SessionId", sessionId);
                        command.Parameters.AddWithValue("@flag", "external");
                        command.ExecuteReader();
                        command.Dispose();
                    }
                }
                rev.Close();
                rev.Dispose();
                cmd.Dispose();
                Connection.Close();
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            var returnField = new { UserId = this.Session["UserID"], UserType = this.Session["UserType"], ID = this.Session["ID"], page = page, message = message };
            return Json(returnField, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult ForgotPassword()
        {
            return View();
        }
        public ActionResult ForgotPassword(EWarranty.Models.GroupClass.ForgotPasswordViewModel Userforget)
        {

            string Useremail = string.Empty;
            string meass = string.Empty;

            var connectionString = ConfigurationManager.ConnectionStrings["CLAIM_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            try
            {
                Connection.Open();
                string message = string.Empty;
                if (Userforget.Email != null)
                {
                    var cmdup = new SqlCommand("P_ForgotPassword_customer_ewaranty", Connection);
                    cmdup.CommandType = CommandType.StoredProcedure;
                    cmdup.Parameters.AddWithValue("@inemailuser", Userforget.Email);
                    SqlParameter returnValuedoc = new SqlParameter("@outResult", SqlDbType.NVarChar, 100);
                    returnValuedoc.Direction = System.Data.ParameterDirection.Output;
                    cmdup.Parameters.Add(returnValuedoc);
                    cmdup.ExecuteNonQuery();
                    message = returnValuedoc.Value.ToString();

                    cmdup.Dispose();
                    Connection.Close();
                    ModelState.AddModelError("", message);
                }
                else
                {
                    ModelState.AddModelError("", "กรุณาใส่อีเมล");
                }

            }
            catch (COMException ex)
            {

                ModelState.AddModelError("", "กรุณาลองใหม่อีกครั้ง มีข้อผิดพลาด " + ex);

            }
            return View();

        }
        public ActionResult Register()
        {

            return View();

        }
        [HttpGet]
        public ActionResult Changepassword()
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

            }

            return View();

        }
        public ActionResult Changepassword(EWarranty.Models.GroupClass.ChangepassUserViewModel Userchange)
        {
            if (Session["UserID"] == null && Session["UserPassword"] == null)
            {
                return RedirectToAction("LogIn", "Account");
            }
            return View();
        }

        [HttpPost]
        public ActionResult LogIn(EWarranty.Models.GroupClass.LoginUserViewModel User)
        {
            if (ModelState.IsValid)
            {
                string Userlog = string.Empty;
                string Usertype = string.Empty;
                string dateexpire = string.Empty;
                int intdateexpire = 0;
                var connectionString = ConfigurationManager.ConnectionStrings["CLAIM_ConnectionString"].ConnectionString;
                SqlConnection Connection = new SqlConnection(connectionString);
                Connection.Open();
                try
                {

                    string type = string.Empty;
                    string cuscode = string.Empty;
                    int contyp = 0;
                    string LoginFail = string.Empty;
                    this.Session["UserID"] = User.Usre;
                    this.Session["UserPassword"] = User.Password;

                    SqlCommand cmd = new SqlCommand("select * From UsrTbl where (UsrID =N'" + User.Usre + "' or [E-Mail] =N'" + User.Usre + "' or LineID =N'" + User.Usre + "' or Tel =N'" + User.Usre + "') and [dbo].F_decrypt([Password])='" + User.Password + "'", Connection);
                    SqlDataReader rev = cmd.ExecuteReader();
                    while (rev.Read())
                    {

                        this.Session["UserType"] = rev["UsrTyp"].ToString();
                        //this.Session["Contact"] = rev["Contact"].ToString();
                        //this.Session["ContactPhone"] = rev["ContactPhone"].ToString();
                        LoginFail = rev["LoginFail"].ToString();
                        type = rev["UsrTyp"].ToString();
                        cuscode = rev["CusCode"].ToString();
                        this.Session["cuscode"] = rev["CusCode"].ToString();
                        //this.Session["Department"] = rev["Department"].ToString();
                    }
                    rev.Close();
                    rev.Dispose();
                    cmd.Dispose();
                    //ถ้ามีใน UsrTbl มี UsrTyp
                    if (type != "")
                    {
                        contyp = Convert.ToInt32(type);

                        if ((contyp == 14 || contyp == 15) && LoginFail != "3")
                        {
                            string sessionId = string.Empty;
                            string httpCookie = string.Empty;
                            //HttpCookie cookie = Request.Cookies["cookieName"];
                            //string sessionId = Request["ASP.NET_SessionId"];
                            // sessionId = Request["ASP.NET_SessionId"];
                            // sessionId = Request["http_cookie"];

                            if (Request.ServerVariables["HTTP_COOKIE"] != null)
                            {
                                httpCookie = Request.ServerVariables["HTTP_COOKIE"].Substring(0, (Request.ServerVariables["HTTP_COOKIE"].Length > 399) ? 399 : Request.ServerVariables["HTTP_COOKIE"].Length);
                            }
                            else
                            {
                                return RedirectToAction("LogIn", "Account");
                            }
                            sessionId = httpCookie;

                            if (sessionId != null)
                            {
                                sessionId = sessionId.Substring(sessionId.Length - 24);
                                this.Session["ID"] = sessionId;
                            }
                            else
                            {
                                this.Session["ID"] = "775.333";

                            }
                            //  string sessionId = "";


                            var command = new SqlCommand("P_logSingin_customer_ewaranty", Connection);
                            command.CommandType = CommandType.StoredProcedure;
                            //command.Parameters.AddWithValue("@pCusCod", strcustome);
                            command.Parameters.AddWithValue("@UsrID", User.Usre);
                            command.Parameters.AddWithValue("@SessionId", sessionId);
                            command.Parameters.AddWithValue("@flag", "internal");
                            command.ExecuteReader();
                            command.Dispose();
                            return RedirectToAction("Index", "Menubar");
                        }
                        else
                        {
                            ModelState.AddModelError("", "กรอกรหัสผ่านไม่ถูกต้อง ไม่สามารถเข้าสู่ระบบได้กรุณารอ 30 นาทีเพื่อดำเนินการใหม่อีกครั้ง");


                        }
                    }
                    else
                    {
                        //ADSRV01
                        DirectoryEntry entry = new DirectoryEntry("LDAP://ADSRV2016-01/dc=Automotive,dc=com", User.Usre, User.Password);
                        DirectorySearcher search = new DirectorySearcher(entry);
                        search.Filter = "(SAMAccountName=" + User.Usre + ")";
                        search.PropertiesToLoad.Add("cn");

                        SearchResult result = search.FindOne();
                        //result.GetDirectoryEntry();

                        if (null == result)
                        {
                            if (IsValid(User.Usre, User.Password))
                            {

                                this.Session["UsrGrpspecial"] = 0;
                                FormsAuthentication.SetAuthCookie(User.Usre, false);
                                return RedirectToAction("LogIn", "Account");
                            }
                            else
                            {
                                ModelState.AddModelError("", "ชื่อผู้ใช้งาน/อีเมล รหัสผ่านไม่ถูกต้อง");
                            }
                            //throw new SoapException("Error authenticating user.",SoapException.ClientFaultCode);
                        }
                        else
                        {
                            this.Session["UserID"] = User.Usre;
                            this.Session["UserPassword"] = User.Password;
                            this.Session["UsrGrpspecial"] = 0;
                            SqlCommand cmd_v = new SqlCommand("select * From v_UsrTbl where UsrID =N'" + User.Usre + "'", Connection);
                            SqlDataReader rev_ = cmd_v.ExecuteReader();
                            while (rev_.Read())
                            {
                                dateexpire = rev_["Date to Expire"].ToString();
                                this.Session["UserType"] = rev_["UsrTyp"].ToString();
                                // this.Session["ISApprover"] = rev["ISApprover"].ToString();
                                // this.Session["Department"] = rev["Department"].ToString();
                            }

                            rev_.Dispose();
                            cmd_v.Dispose();

                            intdateexpire = Convert.ToInt32(dateexpire);

                            if (intdateexpire <= 15)
                            {
                                this.Session["DatetoExpire"] = "Passwords expire '" + intdateexpire + "' days";
                            }
                            else if (intdateexpire == 0)
                            {
                                this.Session["DatetoExpire"] = "The user's password must be changed password  Changed password on Citrix";
                            }
                            else
                            {

                                this.Session["DatetoExpire"] = "..";
                            }

                            FormsAuthentication.SetAuthCookie(User.Usre, false);


                            return RedirectToAction("Index", "Menubar");


                            //return View();
                        }
                        //  Connection.Close();
                    }

                }
                catch (COMException ex)
                {
                    string type = string.Empty;
                    int contyp = 0;
                    //Connection.Open();
                    this.Session["UserID"] = User.Usre;
                    this.Session["UserPassword"] = User.Password;
                    SqlCommand cmd = new SqlCommand("select * From UsrGrp_special where UsrID =N'" + User.Usre + "' and [dbo].F_decrypt([Password])='" + User.Password + "' and  [LoginFail] <> 3", Connection);
                    SqlDataReader rev = cmd.ExecuteReader();
                    while (rev.Read())
                    {
                        this.Session["UsrCode"] = rev["SLMCOD"].ToString();
                        this.Session["UsrGrpspecial"] = 1;
                        this.Session["UserType"] = rev["UsrTyp"].ToString();
                        this.Session["Contact"] = rev["Contact"].ToString();
                        this.Session["ContactPhone"] = rev["ContactPhone"].ToString();
                        this.Session["ISApprover"] = rev["ISApprover"].ToString();
                        type = rev["UsrTyp"].ToString();
                        this.Session["Department"] = rev["Department"].ToString();
                    }
                    rev.Close();
                    rev.Dispose();
                    cmd.Dispose();
                    if (type != "")
                    {
                        contyp = Convert.ToInt32(type);
                        if (contyp != 14)
                        {
                            return RedirectToAction("Index", "Menubar");
                        }
                        else
                        {

                            string message = string.Empty;
                            var cmdup = new SqlCommand("P_logSingin", Connection);
                            cmdup.CommandType = CommandType.StoredProcedure;
                            cmdup.Parameters.AddWithValue("@UsrID", User.Usre);
                            cmdup.Parameters.AddWithValue("@Password", User.Password);
                            SqlParameter returnValuedoc = new SqlParameter("@outResult", SqlDbType.NVarChar, 100);
                            returnValuedoc.Direction = System.Data.ParameterDirection.Output;
                            cmdup.Parameters.Add(returnValuedoc);
                            cmdup.ExecuteNonQuery();
                            message = returnValuedoc.Value.ToString();

                            cmdup.Dispose();

                            return RedirectToAction("Index", "Menubar");
                        }
                    }
                    else
                    {
                        // PCP 090124 >> ModelState.AddModelError("", "ชื่อผู้ใช้งาน/อีเมล์ ที่ระบุยังไมได้ลงทะเบียนในระบบ");
                        ModelState.AddModelError("", "ชื่อผู้ใช้งาน/อีเมล หรือรหัสผ่าน ไม่ถูกต้อง");
                        //string message = string.Empty;
                        //var cmdup = new SqlCommand("P_CountLoginFail_customer_ewaranty", Connection);
                        //cmdup.CommandType = CommandType.StoredProcedure;
                        //cmdup.Parameters.AddWithValue("@UsrID", User.Usre);
                        //SqlParameter returnValuedoc = new SqlParameter("@outResult", SqlDbType.NVarChar, 100);
                        //returnValuedoc.Direction = System.Data.ParameterDirection.Output;
                        //cmdup.Parameters.Add(returnValuedoc);
                        //cmdup.ExecuteNonQuery();
                        //message = returnValuedoc.Value.ToString();
                        //if (message == "true")
                        //{
                        //    ModelState.AddModelError("", "");
                        //}
                        //else
                        //{
                        //    ModelState.AddModelError("", "กรอกรหัสผ่านไม่ถูกต้อง ไม่สามารถเข้าสู่ระบบได้กรุณารอ 30นาทีเพื่อดำเนินการใหม่อีกครั้ง");
                        //}
                    }
                    // Connection.Close();

                    //  ModelState.AddModelError("", "Login details are wrong.");

                }
                Connection.Close();
                return View();
            }
            else {
                return View();
            }
        }

        public JsonResult AddRegister(string b_name, string b_lastname, string b_password, string b_cfpassword, string b_address1, string b_address2, string b_district, string b_amphoe, string b_province, string b_zipcode, string b_tel, string b_lineid, string b_email, string b_userId, string b_displayName)
        {

            string message = string.Empty;
            string no = string.Empty;
            var connectionString = ConfigurationManager.ConnectionStrings["CLAIM_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            try
            {
                Connection.Open();

                var command = new SqlCommand("P_Register_customer_ewaranty", Connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@inname", b_name);
                command.Parameters.AddWithValue("@inlastname", b_lastname);
                command.Parameters.AddWithValue("@inpassword", b_password);
                command.Parameters.AddWithValue("@inaddress1", b_address1);
                command.Parameters.AddWithValue("@inaddress2", b_address2);
                command.Parameters.AddWithValue("@indistrict", b_district);
                command.Parameters.AddWithValue("@inamphoe", b_amphoe);
                command.Parameters.AddWithValue("@inprovince", b_province);
                command.Parameters.AddWithValue("@inzipcode", b_zipcode);
                command.Parameters.AddWithValue("@intel", b_tel);
                command.Parameters.AddWithValue("@inlineid", b_lineid);
                command.Parameters.AddWithValue("@inemail ", b_email);
                command.Parameters.AddWithValue("@inuserId ", b_userId);
                command.Parameters.AddWithValue("@indisplayName ", b_displayName);

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

        public JsonResult savechangepassword(string b_name, string b_cfpassword)
        {



            string Userid = b_name;

            string Useremail = string.Empty;
            string message = string.Empty;

            var connectionString = ConfigurationManager.ConnectionStrings["CLAIM_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            try
            {
                Connection.Open();


                var cmdup = new SqlCommand("P_Changepassword_customer_ewaranty", Connection);
                cmdup.CommandType = CommandType.StoredProcedure;
                cmdup.Parameters.AddWithValue("@newpassword ", b_cfpassword);
                cmdup.Parameters.AddWithValue("@UsrID ", Userid);

                SqlParameter returnValuedoc = new SqlParameter("@outResult", SqlDbType.NVarChar, 100);
                returnValuedoc.Direction = System.Data.ParameterDirection.Output;
                cmdup.Parameters.Add(returnValuedoc);
                cmdup.ExecuteNonQuery();
                message = returnValuedoc.Value.ToString();

                cmdup.Dispose();
                Connection.Close();
                // ModelState.AddModelError("", message);
                //  ModelState.AddModelError("", "เปลี่ยน Password สำเร็จ");
                message = "Y";
            }
            catch (COMException ex)
            {


                message = "กรุณาลองใหม่อีกครั้ง มีข้อผิดพลาด " + ex;
            }




            return Json(new { message }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult RegisterDealer()
        {
            return View();
        }

        public JsonResult AddRegisterDealer(string b_name, string b_code, string b_password, string b_cfpassword, string b_address1, string b_tel, string b_email)
        {
            string message = string.Empty;
            string no = string.Empty;
            var connectionString = ConfigurationManager.ConnectionStrings["CLAIM_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            try
            {
                Connection.Open();

                var command = new SqlCommand("P_Register_dealer_ewaranty", Connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@inname", b_name);
                command.Parameters.AddWithValue("@incuscode", b_code.Trim());
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

        public JsonResult SearchDataDealer(string textSearch)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["CLAIM_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            Connection.Open();
            string SLMCOD = string.Empty;
            string query = string.Empty;
            //string Company = string.Empty;
            string tableCus = string.Empty;
            tableCus = "v_CUSPROV";
            query = string.Format("select distinct pc.CUSCOD,pc.CUSCOD + ' | ' + pc.CUSNAM  from " + tableCus + " pc    where  pc.CUSCOD LIKE '%{0}%'or pc.CUSNAM  LIKE '%{0}%'", textSearch);
            List<string> Code = new List<string>();
            using (SqlCommand cmd = new SqlCommand(query, Connection))
            {
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Code.Add(reader.GetString(1));
                }
                reader.Close();
                reader.Dispose();
                cmd.Dispose();
            }
            Connection.Close();
            return Json(Code, JsonRequestBehavior.AllowGet);
        }
        public JsonResult SearchShiptoDealer(string cuscode, string shipto)
        {
            string txtSql = string.Empty;
            string query = string.Empty;
            var connectionString = ConfigurationManager.ConnectionStrings["CLAIM_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            Connection.Open();
            if (cuscode != "") {
                txtSql = "SELECT customer, case when [customer] = [Code] then 'S000' else [Code] end as code, [Name], CONCAT([Address], ' ', [Address 2]) as Address1, [Address 2] as Address2, [Phone No_] as Phone, [Contact] FROM v_shiptoAACTAC where ([customer] = '"+ cuscode + "' or [Name] = '" + cuscode + "') and [Code] not like '%999%'";
            }
            if (shipto != "") {
                if (shipto == "S000") {
                    shipto = cuscode;
                }
                txtSql += " and Code = '"+ shipto + "'";
            }
            List<listShipto> List = new List<listShipto>();
            using (SqlCommand cmd = new SqlCommand(txtSql, Connection))
            {
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    List.Add(new listShipto()
                    {
                        CustomerCode = reader["customer"].ToString(),
                        Code = reader["Code"].ToString(),
                        Name = reader["Name"].ToString(),
                        Address1 = reader["Address1"].ToString(),
                        Address2 = reader["Address2"].ToString(),
                        Phone = reader["Phone"].ToString(),
                        Contact = reader["Contact"].ToString(),
                    });
                }
                reader.Close();
                reader.Dispose();
                cmd.Dispose();
            }
            Connection.Close();
            return Json(List, JsonRequestBehavior.AllowGet);
        }

        private bool IsValid(string user, string Password)
        {

            bool IsValid = false;
            if (user == null || Password == null) { IsValid = false; }
            else
            {


            }
            return IsValid;
        }

        public class listShipto
        {
            public string CustomerCode { get; set; }
            public string Code { get; set; }
            public string Name { get; set; }
            public string Address1 { get; set; }
            public string Address2 { get; set; }
            public string Phone { get; set; }
            public string Contact { get; set; }
        }

    }
}
