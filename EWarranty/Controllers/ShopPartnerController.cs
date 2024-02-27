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
    public class ShopPartnerController : Controller
    {
        //
        // GET: /ShopPartner/

        public ActionResult Index()
        {
            List<EWarranty.Models.GroupClass.ListShopPartner> List = new List<EWarranty.Models.GroupClass.ListShopPartner>();
            var connectionString = ConfigurationManager.ConnectionStrings["CLAIM_ConnectionString"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            var command = new SqlCommand("P_search_partner_ewaranty", Connection);
            command.CommandType = CommandType.StoredProcedure;
            Connection.Open();
            SqlDataReader dr = command.ExecuteReader();
            while (dr.Read())
            {
                List.Add(new EWarranty.Models.GroupClass.ListShopPartner()
                {
                    Name = dr["Name"].ToString(),
                    Address = dr["Address"].ToString(),
                    Telephone = dr["Telephone"].ToString()
                });
            }
            ViewBag.listShop = List;
            return View();
        }

    }
}
