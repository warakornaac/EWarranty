﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EWarranty.Controllers
{
    public class ClaimEditController : Controller
    {
        //
        // GET: /ClaimEdit/

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
                ViewBag.UserId = User;
                ViewBag.UserType = UserType;

                string Doc = string.Empty;
                // string Docsub = string.Empty;
                string Docdisplay = string.Empty;
                string Docwords = string.Empty;

                string CarMaker = string.Empty;
                string CarModel = string.Empty;
                string CarYear = string.Empty;
                string CarLicense = string.Empty;
                string CarMileage = string.Empty;
                string WarrantyStartDate = string.Empty;
                string WarrantyEndDate = string.Empty;
                string ItemNo = string.Empty;
                string STKDES = string.Empty;
                string UsrID_string = string.Empty;
                string SN = string.Empty;
                string ExpectedReceiptDate = string.Empty;
                string Warranty_ID = string.Empty;
                string Claim_ID = string.Empty;
                string Symptomname = string.Empty;
                string MileageClaim = string.Empty;
                string Symptom_ID = string.Empty;
                string Note = string.Empty;
                string Result = string.Empty;
                string ResultDate = string.Empty;
                string ReplacementSN = string.Empty;
                string ClaimStatus = string.Empty;
                string ClaimShop = string.Empty;
                string CustomerNote = string.Empty;
                string Name = string.Empty;
                string LastName = string.Empty;
                string Tel = string.Empty;
                string LineID = string.Empty;
                string EMail = string.Empty;
                string Shop = string.Empty;
                string ShopTel = string.Empty;
                string ClaimNo = string.Empty;
                Docdisplay = Request.QueryString["SnNUM"];
                List<EWarranty.Models.GroupClass.DefineCode> List_Sympto = new List<EWarranty.Models.GroupClass.DefineCode>();
                List<EWarranty.Models.GroupClass.DefineCode> List = new List<EWarranty.Models.GroupClass.DefineCode>();
                if (Docdisplay != null)
                {

                    string[] words = Docdisplay.Split('/');
                    Docwords = words[0];
                    byte[] data = System.Convert.FromBase64String(Docwords);
                    Doc = System.Text.ASCIIEncoding.ASCII.GetString(data);
                    string Sn_1 = string.Empty;
                    var connectionString = ConfigurationManager.ConnectionStrings["CLAIM_ConnectionString"].ConnectionString;
                    SqlConnection Connection = new SqlConnection(connectionString);
                    Connection.Open();
                    SqlCommand command_Warranty = new SqlCommand("select [SN] From Warranty where [Warranty_ID]  ='" + Doc + "'", Connection);
                    SqlDataReader dr_Warranty = command_Warranty.ExecuteReader();

                    while (dr_Warranty.Read())
                    {
                        Sn_1 = dr_Warranty["SN"].ToString();
                        //  PROD = dr["Code"].ToString(),
                        // PRODNAM = dr["Description"].ToString()


                    }

                    dr_Warranty.Close();
                    dr_Warranty.Dispose();
                    command_Warranty.Dispose();
                    //var command = new SqlCommand("P_Search_LookupVehicleRelation", Connection);
                    //command.CommandType = CommandType.StoredProcedure;
                    //command.Parameters.AddWithValue("@Sty", sty);
                    //command.Parameters.AddWithValue("@Type", Name);
                    var command = new SqlCommand("P_Search_Inquiry_customer_ewaranty", Connection);
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@in_SN", Sn_1);
                    command.Parameters.AddWithValue("@in_Status", "0");
                    command.Parameters.AddWithValue("@in_Customername", "");
                    command.Parameters.AddWithValue("@in_Customerid", "");
                    command.Parameters.AddWithValue("@in_Carlicense", "");


                    SqlDataReader dr = command.ExecuteReader();
                    while (dr.Read())
                    {

                        // List.Add(new EWarranty.Models.GroupClass.Inquiry_Customer()
                        // {
                        // No = dr["No"].ToString(),
                        // Warranty_ID = dr["Warranty_ID"].ToString(),
                        UsrID_string = dr["UsrID"].ToString();
                        SN = dr["SN"].ToString();
                        CarMaker = dr["Car Maker"].ToString();
                        CarModel = dr["Car Model"].ToString();
                        CarYear = dr["Car Year"].ToString();
                        CarLicense = dr["Car License"].ToString();
                        CarMileage = dr["Car Mileage"].ToString();
                        Shop = dr["Shop"].ToString();
                        ShopTel = dr["ShopTel"].ToString();
                        // Status = dr["Status"].ToString(),
                        WarrantyStartDate = dr["Warranty Start Date"].ToString();
                        WarrantyEndDate = dr["Warranty End Date"].ToString();
                        // SNReplacement = dr["SN Replacement"].ToString(),
                        // SNReplacementDate = dr["SN Replacement Date"].ToString(),
                        // InvoiceNo = dr["Invoice No"].ToString(),
                        // CUSCOD = dr["CUSCOD"].ToString(),
                        ItemNo = dr["Item No"].ToString();
                        //  InvoiceDate = dr["Invoice Date"].ToString(),
                        // SalesOrder = dr["Sales Order"].ToString(),
                        STKDES = dr["STKDES"].ToString();
                        // CUSNAM = dr["CUSNAM"].ToString(),
                        // SLMCOD = dr["SLMCOD"].ToString(),
                        ExpectedReceiptDate = dr["Expected Receipt Date"].ToString();
                        Warranty_ID = dr["Warranty_ID"].ToString();
                        Claim_ID = dr["Claim_ID"].ToString();
                        Symptomname = dr["Symptomname"].ToString();
                        MileageClaim = dr["Mileage"].ToString();
                        Symptom_ID = dr["Symptom_ID"].ToString();
                        Note = dr["Note"].ToString();
                        Result = dr["Result"].ToString();
                        ResultDate = dr["Result Date"].ToString();
                        ReplacementSN = dr["Replacement SN"].ToString();
                        ClaimStatus = dr["ClaimStatus"].ToString();
                        ClaimShop = dr["ClaimShop"].ToString();
                        CustomerNote = dr["CustomerNote"].ToString();
                        Name = dr["Name"].ToString();
                        LastName = dr["Last Name"].ToString();
                        Tel = dr["Tel"].ToString();
                        LineID = dr["LineID"].ToString();
                        EMail = dr["EMail"].ToString();
                        ClaimNo = dr["Claim_no"].ToString();

                    }
                    dr.Close();
                    dr.Dispose();
                    command.Dispose();


                    var command_Symptom = new SqlCommand("P_Get_Symptom", Connection);
                    command_Symptom.CommandType = CommandType.StoredProcedure;
                    command_Symptom.Parameters.AddWithValue("@initem", ItemNo);

                    SqlDataReader dr_Sympto = command_Symptom.ExecuteReader();
                    while (dr_Sympto.Read())
                    {

                        List_Sympto.Add(new EWarranty.Models.GroupClass.DefineCode()
                        {
                            ID = dr_Sympto["Symptom_ID"].ToString(),
                            CODE = dr_Sympto["Code"].ToString(),
                            TYPE = dr_Sympto["Type"].ToString(),
                            Desc_Thai = dr_Sympto["Desc_Thai"].ToString(),
                            Desc_Eng = dr_Sympto["Desc_Eng"].ToString(),

                        });
                    }
                    dr_Sympto.Close();
                    dr_Sympto.Dispose();
                    command_Symptom.Dispose();

                    var command_status = new SqlCommand("P_Get_DefineCode", Connection);
                    command_status.CommandType = CommandType.StoredProcedure;
                    command_status.Parameters.AddWithValue("@DefineID", 67);
                    //Connection.Open();
                    SqlDataReader dr_status = command_status.ExecuteReader();
                    while (dr_status.Read())
                    {

                        List.Add(new EWarranty.Models.GroupClass.DefineCode()
                        {
                            ID = dr_status["ID"].ToString(),
                            CODE = dr_status["CODE"].ToString(),
                            FIELD_RELATION = dr_status["FIELD_RELATION"].ToString(),
                            DESCRIPTION_TH = dr_status["DESCRIPTION_TH"].ToString(),
                            DESCRIPTION_EN = dr_status["DESCRIPTION_EN"].ToString(),
                            INACTIVE = dr_status["INACTIVE"].ToString()
                        });
                    }
                    dr_status.Close();
                    dr_status.Dispose();
                    command_status.Dispose();

                    Connection.Close();
                }

                ViewBag.Snno = Doc;
                ViewBag.CarMaker = CarMaker;
                ViewBag.CarModel = CarModel;
                ViewBag.CarYear = CarYear;
                ViewBag.CarLicense = CarLicense;
                ViewBag.CarMileage = CarMileage;
                ViewBag.WarrantyStartDate = WarrantyStartDate;
                ViewBag.WarrantyEndDate = WarrantyEndDate;
                ViewBag.ItemNo = ItemNo;
                ViewBag.STKDES = STKDES;
                ViewBag.UsrID_string = UsrID_string;
                ViewBag.SN = SN;
                ViewBag.ExpectedReceiptDate = ExpectedReceiptDate;
                ViewBag.Warranty_ID = Warranty_ID;
                ViewBag.Claim_ID = Claim_ID;
                ViewBag.List_Sympto = List_Sympto;
                ViewBag.Symptomname = Symptomname;
                ViewBag.MileageClaim = MileageClaim;

                ViewBag.Symptom_ID = Symptom_ID;
                ViewBag.Note = Note;
                ViewBag.Result = Result;
                ViewBag.ResultDate = ResultDate;
                ViewBag.ReplacementSN = ReplacementSN;
                ViewBag.Status = List;
                ViewBag.ClaimStatus = ClaimStatus;
                ViewBag.ClaimShop = ClaimShop;
                ViewBag.CustomerNote = CustomerNote;
                ViewBag.Name = Name;
                ViewBag.LastName = LastName;
                ViewBag.Tel = Tel;
                ViewBag.LineID = LineID;
                ViewBag.EMail = EMail;
                ViewBag.Shop = Shop;
                ViewBag.ShopTel = ShopTel;
                ViewBag.ClaimNo = ClaimNo;
            }
            return View();
        }


    }
}
