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

        // Validation method for name
        public bool IsValidName(string name)
        {
            // This checks if name is not null or empty and if name contains only letters and white spaces
            return !string.IsNullOrEmpty(name) && System.Text.RegularExpressions.Regex.IsMatch(name, @"^[a-zA-Z\s]+$");
        }

        // Validation method for email
        public bool IsValidEmail(string email)
        {
            // This checks if email is not null or empty and if email is in a valid email format
            return !string.IsNullOrEmpty(email) && System.Text.RegularExpressions.Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
        }

        // Validation method for phone number
        public bool IsValidPhoneNumber(string phoneNumber, string country)
        {
            if (string.IsNullOrEmpty(phoneNumber))
                return false;

            switch (country)
            {
                case "USA":
                    // USA phone numbers are typically 10 digits
                    return System.Text.RegularExpressions.Regex.IsMatch(phoneNumber, @"^\d{10}$");
                case "Canada":
                    // Canadian numbers are also typically 10 digits
                    return System.Text.RegularExpressions.Regex.IsMatch(phoneNumber, @"^\d{10}$");
                case "Netherlands":
                    // Dutch numbers can vary in length, typically 9 to 10 digits
                    return System.Text.RegularExpressions.Regex.IsMatch(phoneNumber, @"^\d{9,10}$");
                case "South Africa":
                    // South African numbers are typically 10 digits
                    return System.Text.RegularExpressions.Regex.IsMatch(phoneNumber, @"^\d{10}$");
                default:
                    return false;
            }
        }
    }

}