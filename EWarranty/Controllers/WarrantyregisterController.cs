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
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.IO;
namespace EWarranty.Controllers
{
    public class WarrantyregisterController : Controller
    {
        //
        // GET: /Warrantyregister/

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
                ViewBag.Year = DateTime.Now.Year.ToString();
            }
            return View();
        }
        
       

        [HttpPost]
        public ActionResult UploadFiles(FormCollection formCollection)
        {
            int countPass = 0;
            int countError = 0;
            string uname = string.Empty;
            string Pathimg = string.Empty;
            int count = 0;
            string path = Server.MapPath(@"~\UploadedImage\");
            HttpFileCollectionBase files = Request.Files;
            var connectionString = ConfigurationManager.ConnectionStrings["CLAIM_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            Connection.Open();
            for (int i = 0; i < files.Count; i++)
            {
                bool allowSave = true;
                count = i;
                //string name = formCollection["uploadername"];
                string inCim_No= formCollection["inCim_No"];
              //  string No = formCollection["No"];


                //Pathimg = name + "-" + pussend + ".png";
                var command = new SqlCommand("P_Save_PathImage_customer_ewaranty", Connection);
                command.CommandType = CommandType.StoredProcedure;
               
                command.Parameters.AddWithValue("@in_No", inCim_No);
                command.Parameters.AddWithValue("@in_RunNo", count);

                SqlParameter returnValuedoc = new SqlParameter("@outimagename", SqlDbType.NVarChar, 100);
                returnValuedoc.Direction = System.Data.ParameterDirection.Output;
                command.Parameters.Add(returnValuedoc);
                command.ExecuteNonQuery();
                uname = returnValuedoc.Value.ToString();
                command.Dispose();
                //pus = i;
                //pussend = (pus + 1);
                HttpPostedFileBase file = files[i];
                string fileName = file.FileName;
                string fullPath = Server.MapPath("~/UploadedImage/" + uname);
                file.SaveAs(fullPath);
                int byteCount = file.ContentLength;
                if (file.ContentType.Contains("image"))
                {
                    if (byteCount > 1048576)
                    { //เกิน 1 MB
                        try
                        {
                            System.Web.Helpers.WebImage img = new System.Web.Helpers.WebImage(fullPath);
                            if (img.Width > 1000)
                            {
                                img.Resize(1024, 768);
                                FileInfo fileInfo = new FileInfo(fullPath);
                                long sizeAfter = fileInfo.Length;
                                if (sizeAfter > 1048576)
                                { //เกิน 1 MB
                                    img.Resize(800, 600);
                                }
                                img.Save(fullPath, "png", true);
                            }
                        }
                        catch (Exception ex)
                        {
                            allowSave = false;
                            ++countError;
                            FileInfo fileInfo = new FileInfo(fullPath);
                            fileInfo.Delete();
                            command = new SqlCommand("P_Delete_PathImage_customer_ewaranty", Connection);
                            command.CommandType = CommandType.StoredProcedure;
                            //command.Parameters.AddWithValue("@imageName", uname);
                            command.Parameters.AddWithValue("@imageName", inCim_No);
                            command.ExecuteNonQuery();
                            command.Dispose();


                            var commanderror = new SqlCommand("P_DeleteRegistErerror_customer_ewaranty", Connection);
                            commanderror.CommandType = CommandType.StoredProcedure;


                            commanderror.Parameters.AddWithValue("@inWarranty_ID", inCim_No);


                            SqlParameter returnValue = new SqlParameter("@outGenstatus", SqlDbType.NVarChar, 100);
                            returnValue.Direction = System.Data.ParameterDirection.Output;
                            commanderror.Parameters.Add(returnValue);


                            commanderror.ExecuteNonQuery();
                            commanderror.Dispose();
                        }
                    }
                }
               // file.SaveAs(Server.MapPath(@"~\UploadedImage\" + uname));


            }
            //Connection.Close();
            //return Json(files.Count + " Files Uploaded!");
            Connection.Close();
            countPass = files.Count - countError;
            //return Json(countPass + " Files uploaded!" + " \n" + countError + " Unable upload file!");
            return Json(countError);
        }
       
        public JsonResult AddWarrantyregister(string b_CarMaker, string b_SN, string b_CarModel, string b_CarYear, string b_CarLicense, string b_CarMileage, string b_Shop)
        {



            string message = string.Empty;
            string W_ID = string.Empty;
          
            var connectionString = ConfigurationManager.ConnectionStrings["CLAIM_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            try
            {
                Connection.Open();

                var command = new SqlCommand("P_RegisterWarranty_customer_ewaranty", Connection);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@inUsrID", Session["UserID"]);
                 command.Parameters.AddWithValue("@inSN", b_SN);
                 command.Parameters.AddWithValue("@inCarMaker", b_CarMaker);
                 command.Parameters.AddWithValue("@inCarModel", b_CarModel);
                 command.Parameters.AddWithValue("@inarYear", b_CarYear);
                 command.Parameters.AddWithValue("@inCarLicense", b_CarLicense);
                 command.Parameters.AddWithValue("@inCarMileage", b_CarMileage);
                command.Parameters.AddWithValue("@inShop", b_Shop);
                command.Parameters.AddWithValue("@inWarranty_ID", "0");
				
             
                SqlParameter returnValuedoc = new SqlParameter("@outGenstatus", SqlDbType.NVarChar, 100);
                returnValuedoc.Direction = System.Data.ParameterDirection.Output;
                command.Parameters.Add(returnValuedoc);

                SqlParameter returnno = new SqlParameter("@outoutParm", SqlDbType.NVarChar, 100);
                returnno.Direction = System.Data.ParameterDirection.Output;
                command.Parameters.Add(returnno);
               
                command.ExecuteNonQuery();
                message = returnValuedoc.Value.ToString();
                W_ID = returnno.Value.ToString();
                command.Dispose();
                // message = "true";

            }
            catch (Exception ex)
            {
                message = ex.Message;
            }


            Connection.Close();

            return Json(new { message, W_ID }, JsonRequestBehavior.AllowGet);
        }


        public JsonResult DeleteWarrantyregister(string Warranty_ID)
        {



            string message = string.Empty;
            string W_ID = string.Empty;

            var connectionString = ConfigurationManager.ConnectionStrings["CLAIM_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            try
            {
                Connection.Open();

                var command = new SqlCommand("P_DeleteRegistErerror_customer_ewaranty", Connection);
                command.CommandType = CommandType.StoredProcedure;


                command.Parameters.AddWithValue("@inWarranty_ID", Warranty_ID);


                SqlParameter returnValuedoc = new SqlParameter("@outGenstatus", SqlDbType.NVarChar, 100);
                returnValuedoc.Direction = System.Data.ParameterDirection.Output;
                command.Parameters.Add(returnValuedoc);


                command.ExecuteNonQuery();







                var commandPathImage = new SqlCommand("P_Delete_PathImage_customer_ewaranty", Connection);
                commandPathImage.CommandType = CommandType.StoredProcedure;
                //command.Parameters.AddWithValue("@imageName", uname);
                commandPathImage.Parameters.AddWithValue("@imageName", Warranty_ID);
                commandPathImage.ExecuteNonQuery();
                commandPathImage.Dispose();


                message = returnValuedoc.Value.ToString();
                commandPathImage.Dispose();
                command.Dispose();
                // message = "true";

            }
            catch (Exception ex)
            {
                message = ex.Message;
            }


            Connection.Close();

            return Json(new { message}, JsonRequestBehavior.AllowGet);
        }
       
    }
}
