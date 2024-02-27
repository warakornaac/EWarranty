using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace EWarranty.Models
{
    public class GetdataCenterController : Controller
    {
        //
        // GET: /GetdataCenter/

        public ActionResult Index()
        {
            return View();
        }
        public JsonResult GetdataSessionlogin(string UsrID, string SessionId)
        {
            string StrStstuslogin = string.Empty;
            string message = string.Empty;
            var connectionString = ConfigurationManager.ConnectionStrings["CLAIM_ConnectionString"].ConnectionString;
            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();

            try
            {
                SqlCommand cmd = new SqlCommand("P_Update_SessionId_customer_ewaranty", conn);
                cmd.Connection = conn;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UsrID", UsrID);
                cmd.Parameters.AddWithValue("@SessionId", SessionId);
                SqlParameter returnValue = new SqlParameter("@outResult", SqlDbType.NVarChar, 100);

                returnValue.Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters.Add(returnValue);
                cmd.ExecuteReader();
                StrStstuslogin = returnValue.Value.ToString();

                cmd.Dispose();

                conn.Close();
                //}
            }
            catch (Exception ex)
            {
                message = ex.Message + '/' + ex.Source + '/' + ex.HelpLink + '/' + ex.HResult;
                //return -1;
            }


            return Json(new { message, StrStstuslogin }, JsonRequestBehavior.AllowGet);

        }
        public JsonResult GetdateLookupVehicle(string Name)
        {
            List<EWarranty.Models.GroupClass.LookupVehicle> List = new List<EWarranty.Models.GroupClass.LookupVehicle>();
            //DefineCode model = null;
            var connectionString = ConfigurationManager.ConnectionStrings["CLAIM_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            var command = new SqlCommand("P_Search_LookupVehicle", Connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@Type", Name);
            Connection.Open();
            SqlDataReader dr = command.ExecuteReader();
            while (dr.Read())
            {

                List.Add(new EWarranty.Models.GroupClass.LookupVehicle()
                {
                    //Type = dr["Type"].ToString(),
                    Code = dr["Maker"].ToString(),
                    //Description = dr["Description"].ToString(),
                    //SearchDescription = dr["Search Description"].ToString(),
                    //CodeRelation = dr["Code Relation"].ToString(),
                    //YrStart = dr["Yr Start"].ToString(),
                    //YrEnd = dr["Yr End"].ToString(),
                    //EngineType = dr["Engine Type"].ToString(),
                    //CC = dr["CC"].ToString(),

                });
            }
            dr.Close();
            dr.Dispose();
            command.Dispose();
            Connection.Close();
            return Json(List, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetdateItemMaker(string Name)
        {
            List<EWarranty.Models.GroupClass.ItemMaker> List = new List<EWarranty.Models.GroupClass.ItemMaker>();
            //DefineCode model = null;
            var connectionString = ConfigurationManager.ConnectionStrings["CLAIM_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            var command = new SqlCommand("P_Get_ItemMaker", Connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@item", Name);

            Connection.Open();
            SqlDataReader dr = command.ExecuteReader();
            while (dr.Read())
            {

                List.Add(new EWarranty.Models.GroupClass.ItemMaker()
                {

                    Maker = dr["Maker"].ToString(),
                    Model = dr["Model"].ToString(),
                    ItemNo = dr["Item No"].ToString(),
                    Company = dr["Company"].ToString(),


                });
            }
            dr.Close();
            dr.Dispose();
            command.Dispose();
            Connection.Close();
            return Json(List, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetdateLookupSymptom(string Name)
        {
            List<EWarranty.Models.GroupClass.DefineCode> List = new List<EWarranty.Models.GroupClass.DefineCode>();
            //DefineCode model = null;
            var connectionString = ConfigurationManager.ConnectionStrings["CLAIM_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            var command = new SqlCommand("P_Get_Symptom", Connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@item", Name);

            Connection.Open();
            SqlDataReader dr = command.ExecuteReader();
            while (dr.Read())
            {

                List.Add(new EWarranty.Models.GroupClass.DefineCode()
                {

                    Symptom_ID = dr["[Symptom_ID"].ToString(),
                    Type = dr["Type"].ToString(),
                    Code = dr["Code"].ToString(),
                    Desc_Thai = dr["Desc_Thai"].ToString(),
                    Desc_Eng = dr["Desc_Eng"].ToString(),


                });
            }
            dr.Close();
            dr.Dispose();
            command.Dispose();
            Connection.Close();
            return Json(List, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetdateRelation(string Name, string sty)
        {
            List<EWarranty.Models.GroupClass.LookupVehicle> List = new List<EWarranty.Models.GroupClass.LookupVehicle>();
            //DefineCode model = null;
            var connectionString = ConfigurationManager.ConnectionStrings["CLAIM_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            //var command = new SqlCommand("P_Search_LookupVehicleRelation", Connection);
            //command.CommandType = CommandType.StoredProcedure;
            //command.Parameters.AddWithValue("@Sty", sty);
            //command.Parameters.AddWithValue("@Type", Name);
            var command = new SqlCommand("P_Search_LookupVehicle", Connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@Type", Name);
            Connection.Open();
            SqlDataReader dr = command.ExecuteReader();
            while (dr.Read())
            {

                List.Add(new EWarranty.Models.GroupClass.LookupVehicle()
                {
                    // Type = dr["Type"].ToString(),
                    Code = dr["Model"].ToString(),
                    //Description = dr["Search Description"].ToString(),
                    // SearchDescription = dr["Search Description"].ToString(),
                    // CodeRelation = dr["Code Relation"].ToString(),
                    //YrStart = dr["Yr Start"].ToString(),
                    //YrEnd = dr["Yr End"].ToString(),
                    //EngineType = dr["Engine Type"].ToString(),
                    //CC = dr["CC"].ToString(),

                });
            }
            dr.Close();
            dr.Dispose();
            command.Dispose();
            Connection.Close();
            return Json(List, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Getdata_Sales_History_SN(string b_SN)
        {
            string message = string.Empty;
            string Description = string.Empty;
            string ItemNo = string.Empty;
            var connectionString = ConfigurationManager.ConnectionStrings["CLAIM_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            try
            {
                Connection.Open();

                var command = new SqlCommand("P_Sales_History_SN_customer_ewaranty", Connection);
                command.CommandType = CommandType.StoredProcedure;

                //command.Parameters.AddWithValue("@inUsrID", Session["UserID"]);
                command.Parameters.AddWithValue("@in_SN", b_SN);



                SqlParameter returnValuedoc = new SqlParameter("@outgenmessage", SqlDbType.NVarChar, 100);
                returnValuedoc.Direction = System.Data.ParameterDirection.Output;
                command.Parameters.Add(returnValuedoc);

                SqlParameter returnItemNo = new SqlParameter("@outItemNo", SqlDbType.NVarChar, 100);
                returnItemNo.Direction = System.Data.ParameterDirection.Output;
                command.Parameters.Add(returnItemNo);

                SqlParameter returnSTKDES = new SqlParameter("@outSTKDES", SqlDbType.NVarChar, 100);
                returnSTKDES.Direction = System.Data.ParameterDirection.Output;
                command.Parameters.Add(returnSTKDES);

                command.ExecuteNonQuery();
                message = returnValuedoc.Value.ToString();
                Description = returnSTKDES.Value.ToString();
                ItemNo = returnItemNo.Value.ToString();
                command.Dispose();
                // message = "true";

            }
            catch (Exception ex)
            {
                message = ex.Message;
            }


            Connection.Close();

            return Json(new { message, Description, ItemNo }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetdateStatus(string Name)
        {
            List<EWarranty.Models.GroupClass.DefineCode> List = new List<EWarranty.Models.GroupClass.DefineCode>();
            //DefineCode model = null;
            var connectionString = ConfigurationManager.ConnectionStrings["CLAIM_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            var command = new SqlCommand("P_Get_DefineCode", Connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@DefineID", Convert.ToInt32(Name));
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
            return Json(List, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetdateSymptom(string Name)
        {
            List<EWarranty.Models.GroupClass.DefineCode> List = new List<EWarranty.Models.GroupClass.DefineCode>();
            //DefineCode model = null;
            var connectionString = ConfigurationManager.ConnectionStrings["CLAIM_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            var command = new SqlCommand("P_Get_Symptom", Connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@initem", Convert.ToInt32(Name));
            Connection.Open();
            SqlDataReader dr = command.ExecuteReader();
            while (dr.Read())
            {

                List.Add(new EWarranty.Models.GroupClass.DefineCode()
                {
                    ID = dr["Symptom_ID"].ToString(),
                    CODE = dr["Code"].ToString(),
                    TYPE = dr["Type"].ToString(),
                    Desc_Thai = dr["Desc_Thai"].ToString(),
                    Desc_Eng = dr["Desc_Eng"].ToString(),

                });
            }
            dr.Close();
            dr.Dispose();
            command.Dispose();
            Connection.Close();
            return Json(List, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetdateInquiry(string b_SN, string Status, string Customername, string Customerid, string Carlicense, string Cuscode)
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

            List<EWarranty.Models.GroupClass.Inquiry_Customer> List = new List<EWarranty.Models.GroupClass.Inquiry_Customer>();

            var connectionString = ConfigurationManager.ConnectionStrings["CLAIM_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);

            var command = new SqlCommand("P_Search_Inquiry_customer_ewaranty", Connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@in_SN", b_SN);
            command.Parameters.AddWithValue("@in_Status", Status);
            command.Parameters.AddWithValue("@in_Customername", Customername);
            command.Parameters.AddWithValue("@in_Customerid", S_User);
            command.Parameters.AddWithValue("@in_Carlicense", Carlicense);
            command.Parameters.AddWithValue("@in_Cuscode", Cuscode);
            command.Parameters.AddWithValue("@in_User", User);
            Connection.Open();

            SqlDataReader dr = command.ExecuteReader();
            while (dr.Read())
            {

                List.Add(new EWarranty.Models.GroupClass.Inquiry_Customer()
                {
                    No = dr["No"].ToString(),
                    Warranty_ID = dr["Warranty_ID"].ToString(),
                    UsrID = dr["UsrID"].ToString(),
                    SN = dr["SN"].ToString(),
                    CarMaker = dr["Car Maker"].ToString(),
                    CarModel = dr["Car Model"].ToString(),
                    CarYear = dr["Car Year"].ToString(),
                    CarLicense = dr["Car License"].ToString(),
                    CarMileage = dr["Car Mileage"].ToString(),
                    Shop = dr["Shop"].ToString(),
                    ShopTel = dr["ShopTel"].ToString(),
                    Status = dr["Status"].ToString(),
                    WarrantyStartDate = dr["Warranty Start Date"].ToString(),
                    WarrantyEndDate = dr["Warranty End Date"].ToString(),
                    ClaimNo = dr["Claim_no"].ToString(),
                    SNReplacement = dr["SN Replacement"].ToString(),
                    SNReplacementDate = dr["SN Replacement Date"].ToString(),
                    InvoiceNo = dr["Invoice No"].ToString(),
                    CUSCOD = dr["CUSCOD"].ToString(),
                    ItemNo = dr["Item No"].ToString(),
                    InvoiceDate = dr["Invoice Date"].ToString(),
                    SalesOrder = dr["Sales Order"].ToString(),
                    STKDES = dr["STKDES"].ToString(),
                    CUSNAM = dr["CUSNAM"].ToString(),
                    SLMCOD = dr["SLMCOD"].ToString(),
                    StatusID = dr["StatusID"].ToString(),
                    WarrantyExpire = dr["WarrantyExpire"].ToString(),
                    ExpectedReceiptDate = dr["Expected Receipt Date"].ToString(),
                    Name = dr["Name"].ToString(),
                    Tel = dr["Tel"].ToString(),
                    CreateBy = dr["Create By"].ToString(),
                    CreateDate = dr["Create Date"].ToString(),
                    UpdateBy = dr["Update By"].ToString(),
                    UpdateDate = dr["Update Date"].ToString(),
                    ClaimRound = dr["claim_round"].ToString(),
                });
            }
            dr.Close();
            dr.Dispose();
            command.Dispose();
            Connection.Close();
            return Json(List, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetdateSNCode(string Name)
        {

            List<string> SN = new List<string>();


            var connectionString = ConfigurationManager.ConnectionStrings["CLAIM_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            var command = new SqlCommand("P_Search_SN_customer_ewaranty", Connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@inSearch", Name);

            Connection.Open();
            SqlDataReader dr = command.ExecuteReader();
            while (dr.Read())
            {

                SN.Add(dr.GetString(0));

            }
            dr.Close();
            dr.Dispose();
            command.Dispose();
            Connection.Close();
            //}
            return Json(SN, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetdateSNeplacement(string b_SN)
        {



            string message = string.Empty;
            var connectionString = ConfigurationManager.ConnectionStrings["CLAIM_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            Connection.Open();

            var command = new SqlCommand("P_SN_Replacement_customer_ewaranty", Connection);
            command.CommandType = CommandType.StoredProcedure;

            //command.Parameters.AddWithValue("@inUsrID", Session["UserID"]);
            command.Parameters.AddWithValue("@in_SN", b_SN);



            SqlParameter returnValuedoc = new SqlParameter("@outgenmessage", SqlDbType.NVarChar, 100);
            returnValuedoc.Direction = System.Data.ParameterDirection.Output;
            command.Parameters.Add(returnValuedoc);
            command.ExecuteNonQuery();

            message = returnValuedoc.Value.ToString();

            command.Dispose();
            Connection.Close();
            //}
            return Json(message, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetPathImage(string inWarranty_ID, string infoldertype)
        {

            var connectionString = ConfigurationManager.ConnectionStrings["CLAIM_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            List<EWarranty.Models.GroupClass.ImageFilesListDetail> Getdata = new List<EWarranty.Models.GroupClass.ImageFilesListDetail>();
            EWarranty.Models.GroupClass.ImageFiles model = null;
            Connection.Open();
            // var root = @"\Warranty\ImgUpload\";
            //infoldertype =1 UploadedImage
            //infoldertype =2 ImageClaim
            //infoldertype =3 ImageCarMileage
            var root = "";
            if (infoldertype == "1")
            {
                root = @"..\UploadedImage\";
            }
            else if (infoldertype == "2")
            {
                root = @"..\ImageClaim\";
            }
            else if (infoldertype == "3")
            {
                root = @"..\ImageCarMileage\";
            }

            var command = new SqlCommand("P_GetPathImage_customer_ewaranty", Connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@in_ID", inWarranty_ID);
            command.Parameters.AddWithValue("@infoldertype", infoldertype);
            //Connection.Open();
            SqlDataReader dr = command.ExecuteReader();
            while (dr.Read())
            {
                model = new EWarranty.Models.GroupClass.ImageFiles();
                model.IMAGE_ID = dr["ID"].ToString();

                model.IMAGE_NAME = dr["Pic"].ToString();
                //model.PATH = dr["PATH"].ToString();
                //  model.PATH = Server.MapPath(@"~\ImgUpload\" + dr["IMAGE_NAME"].ToString());
                model.PATH = Path.Combine(root, dr["Pic"].ToString());
                //model.PATH = "D:\\Projects\\work spaces\\ClaimWap\\ClaimWap\\ImgUpload\\CM18110012-GDB7224YO-CM18110012-01-01.png";
                Getdata.Add(new EWarranty.Models.GroupClass.ImageFilesListDetail { val = model });
            }
            dr.Close();
            dr.Dispose();
            command.Dispose();
            Connection.Close();
            return Json(new { Getdata }, JsonRequestBehavior.AllowGet);

        }

    }
}
