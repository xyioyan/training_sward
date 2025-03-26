using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
//using System.Data.SqlClient;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;

namespace Web_app.Pages.Clients
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        [BindProperty(SupportsGet = true)]
        public String branch { get; set; }

        public List<ClientInfo> listClients { get; set; } = new List<ClientInfo>();

        public void OnGet()
        {
            try
            {
                String connectionString =
                    "Server=localhost\\SQLEXPRESS;Database=sathi;Trusted_Connection=True;;trustServerCertificate=true";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "SELECT * FROM dbo.customer_list";
                    if (!String.IsNullOrEmpty(branch))
                    {
                        sql = sql + " WHERE branch = @branch";
                    }
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        if (!String.IsNullOrEmpty(branch))
                        {
                            command.Parameters.AddWithValue("@branch", branch);
                        }
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ClientInfo clientInfo = new ClientInfo();
                                clientInfo.id = reader.GetInt32(0);
                                clientInfo.customer_code = reader.GetString(1);
                                clientInfo.customer_name = reader.GetString(2);
                                clientInfo.contact_no = reader.GetString(3);
                                clientInfo.email = reader.GetString(4);
                                clientInfo.area = reader.GetString(5);
                                clientInfo.address = reader.GetString(6);
                                clientInfo.branch = reader.GetString(7);
                                clientInfo.status = reader.GetString(8);
                                clientInfo.created_at = reader.GetDateTime(9);
                                listClients.Add(clientInfo);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.ToString());
            }
        }
    }

    public class ClientInfo
    {
        public int id { get; set; }
        public string customer_code { get; set; }
        public string customer_name { get; set; }
        public string contact_no { get; set; }
        public string email { get; set; }
        public string area { get; set; }
        public string address { get; set; }
        public string branch { get; set; }
        public string status { get; set; }
        public DateTime created_at { get; set; }
    }
}
