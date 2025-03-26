using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;

namespace Web_app.Pages.Clients
{
    public class Edit : PageModel
    {
        public ClientInfo clientInfo = new ClientInfo();
        public String successMessage = "";
        public String errorMessage = "";
        private readonly ILogger<Edit> _logger;

        public Edit(ILogger<Edit> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            string id = Request.Query["id"];
            try
            {
                String connectionString =
                    "Server=localhost\\SQLEXPRESS;Database=sathi;Trusted_Connection=True;;trustServerCertificate=true";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "SELECT * FROM dbo.customer_list WHERE id = @id";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                clientInfo.id = reader.GetInt32(0);
                                clientInfo.customer_code = reader.GetString(1);
                                clientInfo.customer_name = reader.GetString(2);
                                clientInfo.contact_no = reader.GetString(3);
                                clientInfo.email = reader.GetString(4);
                                clientInfo.area = reader.GetString(5);
                                clientInfo.address = reader.GetString(6);
                                clientInfo.branch = reader.GetString(7);
                                clientInfo.status = reader.GetString(8);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error : " + ex.ToString());
            }
        }

        public void OnPost()
        {
            clientInfo.id = int.Parse(Request.Form["id"]);
            clientInfo.customer_name = Request.Form["customer_name"];
            clientInfo.branch = Request.Form["branch"];
            clientInfo.status = Request.Form["status"];
            clientInfo.contact_no = Request.Form["contact_no"];
            clientInfo.email = Request.Form["email"];
            clientInfo.area = Request.Form["area"];
            clientInfo.address = Request.Form["address"];
            clientInfo.customer_code = Request.Form["customer_code"];
            if (clientInfo.customer_name.Length == 0)
            {
                errorMessage = "All the Fields are required";
                return;
            }

            try
            {
                String connectionString =
                    "Server=localhost\\SQLEXPRESS;Database=sathi;Trusted_Connection=True;;trustServerCertificate=true";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql =
                        "UPDATE dbo.customer_list SET customer_code = @customer_code, customer_name = @customer_name, contact_no = @contact_no, email = @email, area = @area, address = @address, branch = @branch, status = @status WHERE id = @id";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@id", clientInfo.id);
                        command.Parameters.AddWithValue("@customer_code", clientInfo.customer_code);
                        command.Parameters.AddWithValue("@customer_name", clientInfo.customer_name);
                        command.Parameters.AddWithValue("@contact_no", clientInfo.contact_no);
                        command.Parameters.AddWithValue("@email", clientInfo.email);
                        command.Parameters.AddWithValue("@area", clientInfo.area);
                        command.Parameters.AddWithValue("@address", clientInfo.address);
                        command.Parameters.AddWithValue("@branch", clientInfo.branch);
                        command.Parameters.AddWithValue("@status", clientInfo.status);
                        command.ExecuteNonQuery();
                    }
                }
                successMessage = "Record Updated successfullly";
                Response.Redirect("/Clients/Index");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.ToString());
            }
            return;
        }
    }
}
