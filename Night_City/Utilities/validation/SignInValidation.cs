using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Night_City.Utilities.validation
{
    public class SignInValidation
    {
        public bool ValidateUser(string email, string hashedPassword)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = "SELECT Password FROM Users WHERE Email = @Email";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Email", email);

                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return reader["Password"].ToString() == hashedPassword;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as per your application's error handling policy
            }

            return false;
        }
    }

}