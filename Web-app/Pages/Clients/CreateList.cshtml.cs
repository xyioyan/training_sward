using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;

namespace Web_app.Pages.Clients
{
    public class CreatListModel : PageModel
    {
        private readonly EmailSender _emailSender;

        public CreatListModel(EmailSender _emailSender)
        {
            _emailSender = emailSender;
        }

        [BindProperty]
        public string email { get; set; }

        public ClientInfo clientInfo = new ClientInfo();
        public String errorMessage = "";
        public String successMessage = "";
        private readonly ILogger<CreatListModel> _logger;

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

        public CreatListModel(ILogger<CreatListModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            ClientInfo clientInfo = new ClientInfo();
        }

        public void OnPost()
        {
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
                        "INSERT INTO dbo.customer_list (customer_code,customer_name,contact_no,email,area,address,branch,status) VALUES (@customer_code,@customer_name,@contact_no,@email,@area,@address,@branch,@status)";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
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
                successMessage = "Record created successfullly";
                string subject = "Customer List Created Successfully. Thank you!";
                string message = "Your customer list has been created successfully, and email sent!";
                await _emailSender.SendEmailAsync(email,subject, message);
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
}
