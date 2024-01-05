using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Night_City.Pages
{
    public partial class UpdateAccount : System.Web.UI.Page
    {
        string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=\"E:\\CodingProjects\\Dot Net\\Night_City\\App_Data\\NightCity.mdf\";Integrated Security=True";

        protected void Page_Load(object sender, EventArgs e)
        {
            HttpCookie userCookie = Request.Cookies["User"];
            if (!IsPostBack)
            {
                // Retrieve the user's information from the database or any other source
                string userId = userCookie["UserId"]; // Assuming you have the user ID stored in the session

                // Retrieve the user's information based on the user ID
                User user = GetUserFromDatabase(userId);

                // Set the placeholders or text values for the input fields
                txtFullName.Value = user.FullName;
                txtEmail.Value = user.Email;
                txtPhone.Value = user.PhoneNumber;
                ddlCountry.SelectedValue = user.Country;
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            // Retrieve the user's information from the input fields
            HttpCookie userCookie = Request.Cookies["User"];
            string userId = userCookie["UserId"]; // Assuming you have the user ID stored in the session
            string fullName = txtFullName.Value.Trim();
            if (userCookie != null)
            {
                // Modify the value of the cookie
                userCookie["FullName"] = fullName;

                // Set the cookie to expire in 30 days
                userCookie.Expires = DateTime.Now.AddDays(30);

                // Update the cookie in the response
                Response.Cookies.Set(userCookie);
            }
            string email = txtEmail.Value.Trim();
            string phoneNumber = txtPhone.Value.Trim();
            string country = ddlCountry.SelectedValue;

            // Validate inputs
            if (!IsValidName(fullName))
            {
                // Show an error message that name is not valid
                lblError.Text = "Please enter a valid name.";
                lblError.Visible = true;
                return;
            }

            if (!IsValidEmail(email))
            {
                // Show an error message that email is not valid
                lblError.Text = "Please enter a valid email.";
                lblError.Visible = true;
                return;
            }

            if (!IsValidPhoneNumber(phoneNumber, country))
            {
                // Show an error message that phone number is not valid
                lblError.Text = "Please enter a valid phone number for your selected country.";
                lblError.Visible = true;
                return;
            }

            // Update the user's information in the database or any other source
            UpdateUserInDatabase(userId, fullName, email, phoneNumber, country);

            // Redirect to the profile page or any other desired page
            Response.Redirect("~/Pages/Profile.aspx");
        }

        private User GetUserFromDatabase(string userId)
        {
            User user = null;

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = "SELECT FullName, Email, PhoneNumber, Country FROM Users WHERE UserId = @UserId";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@UserId", userId);

                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        user = new User();
                        user.FullName = reader["FullName"].ToString();
                        user.Email = reader["Email"].ToString();
                        user.PhoneNumber = reader["PhoneNumber"].ToString();
                        user.Country = reader["Country"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle the exception (e.g., log it, show an error message, etc.)
            }

            return user;
        }

        private void UpdateUserInDatabase(string userId, string fullName, string email, string phoneNumber, string country)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = "UPDATE Users SET FullName = @FullName, Email = @Email, PhoneNumber = @PhoneNumber, Country = @Country WHERE UserId = @UserId";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@FullName", fullName);
                    command.Parameters.AddWithValue("@Email", email);
                    command.Parameters.AddWithValue("@PhoneNumber", phoneNumber);
                    command.Parameters.AddWithValue("@Country", country);
                    command.Parameters.AddWithValue("@UserId", userId);

                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected <= 0)
                    {
                        throw new Exception("No rows were affected. The user information could not be updated in the database.");
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle the exception (e.g., log it, show an error message, etc.)
            }
        }

        // Validation method for name
        private bool IsValidName(string name)
        {
            // This checks if name is not null or empty and if name contains only letters and white spaces
            return !string.IsNullOrEmpty(name) && System.Text.RegularExpressions.Regex.IsMatch(name, @"^[a-zA-Z\s]+$");
        }

        // Validation method for email
        private bool IsValidEmail(string email)
        {
            // This checks if email is not null or empty and if email is in a valid email format
            return !string.IsNullOrEmpty(email) && System.Text.RegularExpressions.Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
        }

        // Validation method for phone number
        private bool IsValidPhoneNumber(string phoneNumber, string country)
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

    public class User
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Country { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}
