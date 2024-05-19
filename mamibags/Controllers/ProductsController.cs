using mamibags.Models;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;


namespace mamibags.Controllers
{

    public class ProductsController : Controller
    {
        string dbconnection = @"Data Source = DESKTOP-DT8HUI5; Initial Catalog = mamibags ; Integrated Security = true";
        // GET: Products
        public ActionResult Index()
        {
            return View();
        }

        // GET: Products/Details/5
        public ActionResult Details(int id)
        {
            
            var products = new tbl_Products();

            try
            {
                using (SqlConnection con = new SqlConnection(dbconnection))
                {
                    con.Open();

                    using (SqlCommand cmd = new SqlCommand("sp_getProuctbyId", con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@ProductID", id);

                        var reader = cmd.ExecuteReader();

                        if (reader.Read())
                        {
                            products = new tbl_Products
                            {
                                ProductID = reader.GetInt32(0),
                                Pname = reader.GetString(1),
                                Size = reader.GetString(2),
                                Description = reader.GetString(3),
                                Price = reader.GetDecimal(4),
                                ImagePath = reader.IsDBNull(5)?null:reader.GetString(5)
                            };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error retrieving product details: " + ex.Message;
            }

            return View(products); // Pass the product to the "Details" view
        }
    
        public ActionResult Admin()
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
                            ImagePath = reader.IsDBNull(5)?null:reader.GetString(5)
                        });
                    }
                    con.Close();
                }
            }
            return View(products);
        }
        //public IEnumerable<tbl_Products> GetProducts()
        //{
        //    // Example: If there's a database issue or no products, return an empty list to avoid null references
        //    return new List<tbl_Products>(); // Returns an empty list instead of null
        //}


        // GET: Products/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Products/Create
        [HttpPost]
        public ActionResult Create(tbl_Products tblProductObj, HttpPostedFileBase imageFile)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(dbconnection))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_addProduct", con))
                    {
                        con.Open();
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@Pname", tblProductObj.Pname);
                        cmd.Parameters.AddWithValue("@Size", tblProductObj.Size);
                        cmd.Parameters.AddWithValue("@Description", tblProductObj.Description);
                        cmd.Parameters.AddWithValue("@Price", tblProductObj.Price);

                        // Handle file upload
                        if (imageFile != null && imageFile.ContentLength > 0)
                        {
                            var fileName = Path.GetFileName(imageFile.FileName);  // Get the file name
                            var path = Path.Combine(Server.MapPath("~/Content/Image"), fileName);  // Set the save path
                            imageFile.SaveAs(path);  // Save the image to the specified directory

                            cmd.Parameters.AddWithValue("@ImagePath", fileName);  // Store the image path in the database
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@ImagePath", DBNull.Value);  // If no image, store null
                        }

                        cmd.ExecuteNonQuery();  // Execute the SQL command
                    }
                }

                return RedirectToAction("Admin");  // Redirect to the admin page after successful creation
            }
            catch (Exception ex)  // Handle exceptions
            {
                ViewBag.Error = "An error occurred: " + ex.Message;  // Set an error message in case of failure
                return View(tblProductObj);  // Return to the same view with error feedback
            }
        }


        // GET: Products/Edit/5
        public ActionResult Edit(int id)
        {
            var products = new tbl_Products();

            try
            {
                using (SqlConnection con = new SqlConnection(dbconnection))
                {
                    con.Open();

                    using (SqlCommand cmd = new SqlCommand("sp_getProuctbyId", con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@ProductID", id);

                        var reader = cmd.ExecuteReader();

                        if (reader.Read())
                        {
                            products = new tbl_Products
                            {
                                ProductID = reader.GetInt32(0),
                                Pname = reader.GetString(1),
                                Size = reader.GetString(2),
                                Description = reader.GetString(3),
                                Price = reader.GetDecimal(4),
                                ImagePath = reader.GetString(5),
                            };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error retrieving product details: " + ex.Message;
            }

            return View(products); // Pass the product to the "Details" view
        }
        

        // POST: Products/Edit/5
        [HttpPost]
        public ActionResult Edit(int id ,tbl_Products tblProductObj,HttpPostedFileBase imageFile)
        {
            try
            {
                using (SqlConnection con=new SqlConnection(dbconnection)) 
                {
                    con.Open();
                    using(SqlCommand cmd = new SqlCommand("sp_updateProduct", con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@ProductID", tblProductObj.ProductID);
                        cmd.Parameters.AddWithValue("@Pname", tblProductObj.Pname);
                        cmd.Parameters.AddWithValue("@Size", tblProductObj.Size);
                        cmd.Parameters.AddWithValue("@Description", tblProductObj.Description);
                        cmd.Parameters.AddWithValue("@Price", tblProductObj.Price);

                        if(imageFile !=null && imageFile.ContentLength > 0)
                        {
                            var fileName = Path.GetFileName(imageFile.FileName);
                            var path = Path.Combine(Server.MapPath("~/Content/Image"), fileName);
                          
                            imageFile.SaveAs(path); 

                            cmd.Parameters.AddWithValue("@ImagePath", fileName);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@ImagePath",tblProductObj.ImagePath);
                        }

                        cmd.ExecuteNonQuery();
                        con.Close();

                    }
                }
                // TODO: Add update logic here

                return RedirectToAction("Admin");
            }
            catch
            {
                return View();
            }
        }

        // GET: Products/Delete/5
        public ActionResult Delete(int id)
        {
            var products = new tbl_Products();

            try
            {
                using (SqlConnection con = new SqlConnection(dbconnection))
                {
                    con.Open();

                    using (SqlCommand cmd = new SqlCommand("sp_getProuctbyId", con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@ProductID", id);

                        var reader = cmd.ExecuteReader();

                        if (reader.Read())
                        {
                            products = new tbl_Products
                            {
                                ProductID = reader.GetInt32(0),
                                Pname = reader.GetString(1),
                                Size = reader.GetString(2),
                                Description = reader.GetString(3),
                                Price = reader.GetDecimal(4),
                                ImagePath = reader.GetString(5),
                            };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error retrieving product details: " + ex.Message;
            }

            return View(products); // Pass the product to the "Details" view
        }
    

        // POST: Products/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, tbl_Products tblProductsObj)
        {
            try
            {
            using (SqlConnection con = new SqlConnection(dbconnection))
            {
                    using(SqlCommand cmd = new SqlCommand("sp_deleteproduct", con))
                    {
                        con.Open() ;
                        cmd.CommandType=System.Data.CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@ProductID", id);
                        cmd.ExecuteNonQuery();
                        con.Close();
                    }
            }
                // TODO: Add delete logic here

                return RedirectToAction("Admin");
            }
            catch
            {
                return View();
            }
        }
        public ActionResult ViewOrders()
        {

            List<tbl_Orders> orders = new List<tbl_Orders>();

            using (SqlConnection con = new SqlConnection(dbconnection))
            {
                using (SqlCommand cmd = new SqlCommand("sp_fetchorders", con))
                {
                    con.Open();
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    var reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        orders.Add(new tbl_Orders
                        {
                            OrderID = reader.GetInt32(0),
                            ProductID = reader.GetInt32(1),
                            OrderDate = reader.GetDateTime(2),
                            Quantity = reader.GetInt32(3),
                            CustomerName = reader.GetString(4),
                            CustomerMobile = reader.GetString(5),
                            Address =  reader.GetString(6)
                        });
                    }
                    con.Close();
                }
            }
            return View(orders);
        }
        public ActionResult CreateOrder(int id)
        {
            var product = GetProductById(id);  // Get the product based on id
            if (product == null)
            {
                ViewBag.Error = "Product not found.";
                return View("Error");  // Return an error view or handle the error appropriately
            }
            //return View(product);
            var viewModel = new OrderProductViewModel
            {
                Orders = new tbl_Orders { ProductID = id, OrderDate = DateTime.Now },
                Products = product
            };

            return View(viewModel);  // Return the correct model to the view


        }

        private tbl_Products GetProductById(int id)
        {
            var products = new tbl_Products();

            try
            {
                using (SqlConnection con = new SqlConnection(dbconnection))
                {
                    con.Open();

                    using (SqlCommand cmd = new SqlCommand("sp_getProuctbyId", con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@ProductID", id);

                        var reader = cmd.ExecuteReader();

                        if (reader.Read())
                        {
                            products = new tbl_Products
                            {
                                ProductID = reader.GetInt32(0),
                                Pname = reader.GetString(1),
                                Size = reader.GetString(2),
                                Description = reader.GetString(3),
                                Price = reader.GetDecimal(4),
                                ImagePath = reader.IsDBNull(5) ? null : reader.GetString(5)
                            };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error retrieving product details: " + ex.Message;
            }

            return products; // Pass the product to the "Details" view

        }



        // POST: Orders/CreateOrder
        [HttpPost]
        public ActionResult CreateOrder(OrderProductViewModel OrderProductViewModelObj)
        {
            try
            {
                // TODO: Add insert logic here

                using (SqlConnection con = new SqlConnection(dbconnection))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_createOrder", con))
                    {
                        con.Open();

                        cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@ProductID", OrderProductViewModelObj.Orders.ProductID);
                        cmd.Parameters.AddWithValue("@OrderDate", OrderProductViewModelObj.Orders.OrderDate);
                        cmd.Parameters.AddWithValue("@Quantity", OrderProductViewModelObj.Orders.Quantity);
                        cmd.Parameters.AddWithValue("@CustomerName", OrderProductViewModelObj.Orders.CustomerName);
                        cmd.Parameters.AddWithValue("@CustomerMobile", OrderProductViewModelObj.Orders.CustomerMobile);
                        cmd.Parameters.AddWithValue("@Address", OrderProductViewModelObj.Orders.Address);

                        cmd.ExecuteNonQuery();

                        con.Close();
                    }
                }

                TempData["SuccessMessage"] = "Your Order Placed";

                return View();
            }
            catch
            {
                return View();
            }
        }

    }
}

