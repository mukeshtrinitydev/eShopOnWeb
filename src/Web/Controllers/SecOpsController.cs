using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Microsoft.eShopWeb.Web.Controllers;
[Route("api/[controller]")]
[ApiController]
public class SecOpsController : ControllerBase
{
        public IActionResult ReadContentOfURL(string url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url); // Noncompliant

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            string responseFromServer = reader.ReadToEnd();

            reader.Close();
            dataStream.Close();
            response.Close();
            return Content(responseFromServer);
        }

        public IActionResult OnPostAsync(string userName, string password)
        {
            using (SqlConnection sqlConnection = new SqlConnection("Data Source=.;Initial Catalog=MvcBook;Integrated Security=True"))
            {
                string commandText = "SELECT [UserName] FROM dbo.[Login] WHERE [Username] = '"
                    + userName
                    + "' AND [Password]='"
                    + password
                    + "' ";
                try
                {
                    using (SqlCommand sqlCommand = new SqlCommand(commandText, sqlConnection))
                    {
                        sqlConnection.Open();
                        if (sqlCommand.ExecuteScalar() == null)
                        {
                            return Ok();
                        }
                        // Valid Login
                        string Username = sqlCommand.ExecuteScalar().ToString();
                        sqlConnection.Close();
                        return RedirectToPage("./LoginSuccess", new { username = Username });
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
    }
