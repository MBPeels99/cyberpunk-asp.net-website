using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;

namespace Night_City.Utilities.sessions
{
    public class SessionManager
    {
        public bool ManageUserSession(string email)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT FullName, UserId, SecurityLevel FROM Users WHERE Email = @Email";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Email", email);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    HttpContext.Current.Session["FullName"] = reader["FullName"].ToString();
                    HttpContext.Current.Session["UserId"] = reader["UserId"].ToString();
                    HttpContext.Current.Session["SecurityLevel"] = reader["SecurityLevel"].ToString();

                    return true;
                }
            }

            return false;
        }
    }
}