using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EWarranty.Models
{
    public class GroupClass : Controller
    {
        //
        // GET: /GroupClass/

        public ActionResult Index()
        {
            return View();
        }
        public class LoginUserViewModel
        {
            [Required]
            //[EmailAddress]
            [StringLength(150)]
            [Display(Name = "Email: ")]
            public string Usre { get; set; }


            [Required]
            [DataType(DataType.Password)]
            [StringLength(150, MinimumLength = 2)]
            [Display(Name = "Password: ")]
            public string Password { get; set; }
        }

        public class ChangepassUserViewModel
        {
            [Required]
            //[EmailAddress]
            [StringLength(150)]
            [Display(Name = "Email: ")]
            public string Userid { get; set; }

            [Required]
            [DataType(DataType.Password)]
            [StringLength(150, MinimumLength = 2)]
            [Display(Name = "Password: ")]
            public string Password { get; set; }


            [Required]
            [DataType(DataType.Password)]
            [StringLength(150, MinimumLength = 2)]
            [Display(Name = "ConfirmPassword: ")]
            public string ConfirmPassword { get; set; }
        }
        public class ForgotPasswordViewModel
        {
            [Required]
            //[EmailAddress]
            [StringLength(150)]
            [Display(Name = "Email: ")]
            public string Email { get; set; }


            
        }
        public class ProductViewModel
        {
            public string ProductName { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
            public Nullable<int> Price { get; set; }
            public Nullable<int> ImageId { get; set; }
            public HttpPostedFileWrapper ImageFile { get; set; }
        }
        public class LookupVehicle
        {
            public string Type { get; set; }
            public string Code { get; set; }
            public string Description { get; set; }
            public string SearchDescription { get; set; }
            public string CodeRelation { get; set; }
            public string YrStart { get; set; }
            public string YrEnd { get; set; }
            public string EngineType { get; set; }
            public string CC { get; set; }
            public string Picture { get; set; }
            public string sort { get; set; }
        }
        public class Inquiry_Customer
        {
             public string No { get; set; }
             public string  Warranty_ID  { get; set; }
             public string  UsrID  { get; set; }
             public string  SN  { get; set; }
             public string  CarMaker  { get; set; }
             public string  CarModel  { get; set; }
             public string  CarYear  { get; set; }
             public string  CarLicense  { get; set; }
             public string  CarMileage  { get; set; }
             public string  Shop  { get; set; }
             public string  Status  { get; set; }
             public string  WarrantyStartDate  { get; set; }
             public string  WarrantyEndDate  { get; set; }
             public string  SNReplacement  { get; set; }
             public string  SNReplacementDate  { get; set; }
             public string  InvoiceNo  { get; set; }
             public string  CUSCOD  { get; set; }
             public string  ItemNo  { get; set; }
             public string  InvoiceDate  { get; set; }
             public string  SalesOrder  { get; set; }
             public string  STKDES  { get; set; }
             public string  CUSNAM  { get; set; }
             public string  SLMCOD  { get; set; }
             public string  ExpectedReceiptDate  { get; set; }
             public string StatusID { get; set; }
             public string WarrantyExpire { get; set; }
             public string Name { get; set; }
             public string Tel { get; set; }
             public string CreateBy{ get; set; }
			 public string CreateDate{ get; set; }
             public string  UpdateBy{ get; set; }
             public string UpdateDate { get; set; }
        }
        public class DefineCode
        {
            public string ID { get; set; }
            public string CODE { get; set; }
            public string FIELD_RELATION { get; set; }
            public string DESCRIPTION_TH { get; set; }
            public string DESCRIPTION_EN { get; set; }
            public string INACTIVE { get; set; }
            public string TYPE { get; set; }
            public string Desc_Thai { get; set; }
            public string Desc_Eng { get; set; }
            public string Symptom_ID { get; set; }
            public string Type { get; set; }
            public string Code { get; set; }
        }
        public class ItemMaker
        {
            public string Maker { get; set; }
			public string Model{ get; set; }
			public string ItemNo{ get; set; }
			public string Company{ get; set; }
        
        }
        // Class Image Files 
        public class ImageFiles
        {
            public string IMAGE_ID { get; set; }
            public string REQ_NO { get; set; }
            public string CLM_NO_SUB { get; set; }
            public string IMAGE_NO { get; set; }
            public string IMAGE_NAME { get; set; }
            public string PATH { get; set; }
        }
        public class ImageFilesListDetail
        {
            public ImageFiles val { get; set; }

        }

        public class Userregister 
        { 
             public string UsrID  { get; set; }
			 public string Name  { get; set; }
			 public string LastName { get; set; }
			 public string Password { get; set; }
             public string UsrTyp { get; set; }
			 public string Address01 { get; set; }
			 public string Address02 { get; set; }
			 public string Province{ get; set; }
			 public string Postcode { get; set; }
			 public string EMail { get; set; }
			 public string Tel  { get; set; }
		 	 public string LineID { get; set; }
			 public string Logon { get; set; }
			 public string LastLogOn { get; set; }
			 public string SessionId { get; set; }
			 public string pwdLastSet { get; set; }
			 public string Insertdate { get; set; }
			 public string LoginFail { get; set; }
			 public string Online { get; set; }
        }
    }
}
