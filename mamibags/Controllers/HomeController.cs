using mamibags.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;

namespace mamibags.Controllers
{
    public class HomeController : Controller
    {
        string dbconnection = @"Data Source = DESKTOP-DT8HUI5; Initial Catalog = mamibags ; Integrated Security = true";
        public ActionResult Index()
        {

            List<tbl_Products> products = new List<tbl_Products>();

            using (SqlConnection con = new SqlConnection(dbconnection))
            {
                using (SqlCommand cmd = new SqlCommand("sp_getProduct", con))
                {
                    con.Open();
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    var reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        products.Add(new tbl_Products
                        {
                            ProductID = reader.GetInt32(0),
                            Pname = reader.GetString(1),
                            Size = reader.GetString(2),
                            Description = reader.GetString(3),
                            Price = reader.GetDecimal(4),
                            ImagePath = reader.IsDBNull(5) ? null : reader.GetString(5)
                        });
                    }
                    con.Close();
                }
            }
            return View(products);

        }

        public ActionResult Shop()
        {
            ViewBag.Message = "Your application description page.";
            List<tbl_Products> products = new List<tbl_Products>();

            using (SqlConnection con = new SqlConnection(dbconnection))
            {
                using (SqlCommand cmd = new SqlCommand("sp_getProduct", con))
                {
                    con.Open();
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    var reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        products.Add(new tbl_Products
                        {
                            ProductID = reader.GetInt32(0),
                            Pname = reader.GetString(1),
                            Size = reader.GetString(2),
                            Description = reader.GetString(3),
                            Price = reader.GetDecimal(4),
                            ImagePath = reader.IsDBNull(5) ? null : reader.GetString(5)
                        });
                    }
                    con.Close();
                }
            }
            return View(products);

        }



        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        [HttpPost]
        public ActionResult Contact(tbl_feedback tblfeedbackObj)
        {
            try
            {
                if (string.IsNullOrEmpty(tblfeedbackObj.Name) ||
                 string.IsNullOrEmpty(tblfeedbackObj.Mobilenumber) || string.IsNullOrEmpty(tblfeedbackObj.Message))
                {
                    ViewBag.ErrorMessage = "Please enter the all Credential";
                }
                using (SqlConnection con = new SqlConnection(dbconnection))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("sp_insertfeedback", con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@Name", tblfeedbackObj.Name);
                        cmd.Parameters.AddWithValue("@Mobilenumber", tblfeedbackObj.Mobilenumber);
                        cmd.Parameters.AddWithValue("@Message", tblfeedbackObj.Message);

                        cmd.ExecuteNonQuery();
                    }
                    con.Close();

                    TempData["SuccessMessage"] = "Thank you for your feedback.";

                    return View();

                }
            }
            catch
            {
                return View();
            }

       
        }
    

        public ActionResult Why()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult Register()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        [HttpPost]
        public ActionResult Register(tbl_regCustomer tblregCustomerObj)
        {
            try
            {
                if (string.IsNullOrEmpty(tblregCustomerObj.Name) || !tblregCustomerObj.Mobile.HasValue ||
                 string.IsNullOrEmpty(tblregCustomerObj.Email) || string.IsNullOrEmpty(tblregCustomerObj.Address))
                {
                    ViewBag.ErrorMessage = "Please enter the all Credential";
                }
                using (SqlConnection con = new SqlConnection(dbconnection))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("sp_tbl_regCustomer", con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@name", tblregCustomerObj.Name);
                        cmd.Parameters.AddWithValue("@email", tblregCustomerObj.Email);
                        cmd.Parameters.AddWithValue("@mobile", tblregCustomerObj.Mobile);
                        cmd.Parameters.AddWithValue("@address", tblregCustomerObj.Address);

                        cmd.ExecuteNonQuery();
                    }
                    con.Close();

                    TempData["registersuccess"] = "Register Successfully";

                    return View();

                }

            }
            catch
            {
                return View();
            }


        }
        public ActionResult Login()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        [HttpPost]
        public ActionResult Login(string name, int? mobile)
        {
 if (string.IsNullOrEmpty(name) || !mobile.HasValue)  // Server-side validation for empty fields
    {
        ViewBag.ErrorMessage = "Invalid Credential"; 
        return View();  
    }

    bool isUser = CheckUserCredentials(name, mobile);
    bool isAdmin = CheckAdminCredentials(name, mobile);

    if (isUser)
    {
        return RedirectToAction("Index");
    }
    else if (isAdmin)
    {
        return RedirectToAction("Admin", "Products");
    }

  
    return View();

        }
        public bool CheckUserCredentials(string name, int? mobile)
        {
          
                using (SqlConnection con = new SqlConnection(dbconnection))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("sp_login", con))
                    {

                        cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@name", name);
                        cmd.Parameters.AddWithValue("@mobile", mobile);

                        var result = cmd.ExecuteScalar();
                        con.Close();
                        return result != null;
                    }

                }
          
           
            
        }
        public bool CheckAdminCredentials(string name, int? mobile)
        {
            using (SqlConnection con = new SqlConnection(dbconnection))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("sp_adminlogin", con))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@adminname", name);
                    cmd.Parameters.AddWithValue("@adminmobile", mobile);

                    var result2 = cmd.ExecuteScalar();
                    con.Close();
                    return result2 != null;
                }

            }
        }

    }
}