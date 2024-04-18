using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.OleDb;

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
        //set attribute to session
        public ActionResult SetExportDataInquiry(string b_SN, string Status, string Customername, string Customerid, string Carlicense, string Cuscode)
        {
            this.Session["S_b_SN"] = b_SN;
            this.Session["S_Status"] = Status;
            this.Session["S_Customername"] = Customername;
            this.Session["S_Customerid"] = Customerid;
            this.Session["S_Carlicense"] = Carlicense;
            this.Session["S_Cuscode"] = Cuscode;
            return new EmptyResult();
        }
        protected DataTable GetDataTableInquiry(string b_SN, string Status, string Customername, string Customerid, string Carlicense, string Cuscode)
        {
            string User = Session["UserID"].ToString();
            string UserType = Session["UserType"].ToString();
            string S_User = String.Empty;
            if (UserType == "14")
            {
                S_User = User;
            }
            else
            {
                S_User = Customerid;

            }
            DataTable dt = new DataTable();
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = ConfigurationManager.ConnectionStrings["CLAIM_ConnectionString"].ConnectionString;
            conn.Open();
            var command = new SqlCommand("P_Search_Inquiry_customer_ewaranty", conn);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@in_SN", b_SN);
            command.Parameters.AddWithValue("@in_Status", Status);
            command.Parameters.AddWithValue("@in_Customername", Customername);
            command.Parameters.AddWithValue("@in_Customerid", S_User);
            command.Parameters.AddWithValue("@in_Carlicense", Carlicense);
            command.Parameters.AddWithValue("@in_Cuscode", Cuscode);
            command.Parameters.AddWithValue("@in_User", User);

            SqlDataAdapter da = new SqlDataAdapter(command);
            da.Fill(dt);
            conn.Close();
            return dt;
        }
        //get file export excel
        [HttpPost]
        [ActionName("Index")]
        public ActionResult ExportDataInquiry(string b_SN, string Status, string Customername, string Customerid, string Carlicense, string Cuscode)
        {
            b_SN = this.Session["S_b_SN"].ToString();
            Status = this.Session["S_Status"].ToString();
            Customername = this.Session["S_Customername"].ToString();
            Customerid = this.Session["S_Customerid"].ToString();
            Carlicense = this.Session["S_Carlicense"].ToString();
            Cuscode = this.Session["S_Cuscode"].ToString();

            var dayMonth = DateTime.Today.ToString("ddMM");
            var yearTH = DateTime.Today.ToString("yyyy");
            int yearCS = Convert.ToInt32(yearTH) - 543;

            Response.AddHeader("content-disposition", "attachment;filename=DataInquiry_" + dayMonth +  yearCS + ".csv");
            Response.ContentType = "application/ms-excel";
            Response.ContentEncoding = System.Text.Encoding.Unicode;
            Response.BinaryWrite(System.Text.Encoding.Unicode.GetPreamble());
            System.IO.StringWriter sw = new System.IO.StringWriter();
            DataTable dt = GetDataTableInquiry(b_SN, Status, Customername, Customerid, Carlicense, Cuscode);
            string str = string.Empty;
            foreach (DataColumn dtcol in dt.Columns)
            {
                Response.Write(str + dtcol.ColumnName);
                str = "\t";
            }
            Response.Write("\n");
            foreach (DataRow dr in dt.Rows)
            {
                str = "";
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    Response.Write(str + Convert.ToString(dr[j]));
                    str = "\t";
                }
                Response.Write("\n");
            }
            Response.End();

            List<EWarranty.Models.GroupClass.DefineCode> List = new List<EWarranty.Models.GroupClass.DefineCode>();
            //DefineCode model = null;
            var connectionString = ConfigurationManager.ConnectionStrings["CLAIM_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            var command = new SqlCommand("P_Get_DefineCode", Connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@DefineID", 67);
            Connection.Open();
            SqlDataReader drStatus = command.ExecuteReader();
            while (drStatus.Read())
            {

                List.Add(new EWarranty.Models.GroupClass.DefineCode()
                {
                    ID = drStatus["ID"].ToString(),
                    CODE = drStatus["CODE"].ToString(),
                    FIELD_RELATION = drStatus["FIELD_RELATION"].ToString(),
                    DESCRIPTION_TH = drStatus["DESCRIPTION_TH"].ToString(),
                    DESCRIPTION_EN = drStatus["DESCRIPTION_EN"].ToString(),
                    INACTIVE = drStatus["INACTIVE"].ToString()
                });
            }
            drStatus.Close();
            drStatus.Dispose();
            command.Dispose();
            Connection.Close();


            ViewBag.Status = List;
            return View();
        }
    }
}
