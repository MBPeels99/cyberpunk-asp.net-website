using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Night_City.Pages
{
    public partial class PlanTrip : System.Web.UI.Page
    {
        string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=\"E:\\CodingProjects\\Dot Net\\Night_City\\App_Data\\NightCity.mdf\";Integrated Security=True";
        protected void Page_Load(object sender, EventArgs e)
        {
            lblError.Text = "";
            lblError.Visible = true;
        }

        protected void btnSignIn_Click(object sender, EventArgs e)
        {
            lblError.Visible = false;
            divSignIn.Visible = true;
            divSignUp.Visible = false;
        }

        protected void btnSignUp_Click(object sender, EventArgs e)
        {
            lblError.Visible = true;
            divSignIn.Visible = false;
            divSignUp.Visible = true;
        }

        protected void btnConfirmSignIn_Click(object sender, EventArgs e)
        {
            // Retrieve the values from the textboxes
            string email = txtEmailSignIn.Value;
            string password = txtPassword.Value;

            // Validate the email
            if (!IsValidEmail(email))
            {
                // Show an error message that email is not valid
                lblError.Text = "Please enter a valid email.";
                return;
            }

            // Hash the provided password to match the stored hashed password
            string hashedPassword = HashPassword(password);

            // Retrieve the user's information from the database
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = "SELECT Password,FullName,UserId,IsEmployee FROM Users WHERE Email = @Email";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Email", email);

                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        // Compare the hashed password from the database with the hashed password the user entered
                        if (reader["Password"].ToString() == hashedPassword)
                        {   
                            //Check if user is employee
                            bool isEmployee = Convert.ToBoolean(reader["IsEmployee"]);

                            // Create a new cookie for the user's full name and empl
                            // oyee status
                            HttpCookie userCookie = new HttpCookie("User");
                            userCookie["FullName"] = reader["FullName"].ToString();
                            userCookie["UserId"] = reader["UserId"].ToString();
                            userCookie["IsEmployee"] = isEmployee.ToString();
                            userCookie.Expires = DateTime.Now.AddDays(30); // Set the cookie to expire in 30 days
                            Response.Cookies.Add(userCookie); // Add the cookie to the response

                            // Passwords match, sign-in successful.
                            // Redirect to the admin page if the user is an employee
                            if (isEmployee)
                            {
                                Response.Redirect("~/Pages/AdminPage.aspx");
                            }
                            else
                            {
                                Response.Redirect("~/Pages/Profile.aspx");
                            }
                        }
                        else
                        {
                            // Passwords do not match, show an error message
                            lblError.Text = "Incorrect email or password.";
                            lblError.Visible = true;
                        }
                    }
                    else
                    {
                        // Email not found in the database, show an error message
                        lblError.Text = "Incorrect email or password.";
                        lblError.Visible = true;
                    }

                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                // Handle the exception (e.g., log it, show an error message, etc.)
                lblError.Text = "An error occurred while signing in. Please try again.";
                lblError.Visible = true;
            }
        }

        protected void btnConfirmSignUp_Click(object sender, EventArgs e)
        {
            // Retrieve the values from the input fields
            string fullName = txtFullName.Value;
            string email = txtEmail.Value;
            string phoneNumber = txtPhone.Value;
            string country = ddlCountry.SelectedValue;
            string dateOfBirthStr = txtDateOfBirth.Value;

            string password = txtPasswordSignUp.Value;
            string confirmPassword = txtConfirmPassword.Value;

            // Check if all fields are filled
            if (string.IsNullOrEmpty(fullName) || string.IsNullOrEmpty(email) ||
                string.IsNullOrEmpty(phoneNumber) || string.IsNullOrEmpty(country) ||
                string.IsNullOrEmpty(dateOfBirthStr) || string.IsNullOrEmpty(password) ||
                string.IsNullOrEmpty(confirmPassword))
            {
                // Show an error message that all fields should be filled
                lblError.Text = $"All fields must be filled. name: {fullName}, phone: {phoneNumber}, DOB: {dateOfBirthStr}, country: {country}, email: {email}, password: {password}";
                
                return;
            }

            DateTime dateOfBirth = DateTime.Parse(txtDateOfBirth.Value);

            // Validate inputs
            if (!IsValidName(fullName))
            {
                // Show an error message that name is not valid
                lblError.Text = "Please enter a valid name.";
                
                return;
            }

            if (!IsValidEmail(email))
            {
                // Show an error message that email is not valid
                lblError.Text = "Please enter a valid email.";
                return;
            }

            if (!IsValidPhoneNumber(phoneNumber, ddlCountry.SelectedValue))
            {
                // Show an error message that phone number is not valid
                lblError.Text = "Please enter a valid phone number for your selected country.";
                return;
            }

            if (password == confirmPassword)
            {
                // Hash the password before storing it into the database
                string hashedPassword = HashPassword(password);

                try
                {
                    // Store the user information in the database and retrieve the user ID
                    int userId = StoreUserInfoAndGetId(fullName, email, phoneNumber, country, dateOfBirth, hashedPassword);

                    if (userId > 0)
                    {
                        // Create a new cookie for the user's full name and user ID
                        HttpCookie userCookie = new HttpCookie("User");
                        userCookie["FullName"] = fullName;
                        userCookie["UserId"] = userId.ToString();
                        // Set the cookie to expire in 30 days
                        userCookie.Expires = DateTime.Now.AddDays(30);
                        // Add the cookie to the response
                        Response.Cookies.Add(userCookie);

                        Response.Redirect("~/Pages/Profile.aspx");
                    }
                    else
                    {
                        // Error occurred while storing user information
                        lblError.Text = "An error occurred while creating the account. Please try again.";
                    }
                }
                catch (Exception ex)
                {
                    // Handle the exception (e.g., log it, show an error message, etc.)
                    lblError.Text = "An error occurred while creating the account. Please try again.";
                }
            }
            else
            {
                lblError.Text = "Password and password confirmation do not match.";
            }
        }

        private int StoreUserInfoAndGetId(string fullName, string email, string phoneNumber, string country, DateTime dateOfBirth, string hashedPassword)
        {
            int userId = 0;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO Users (FullName, Email, PhoneNumber, Country, DateOfBirth, Password) VALUES (@FullName, @Email, @PhoneNumber, @Country, @DateOfBirth, @Password); SELECT SCOPE_IDENTITY()";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@FullName", fullName);
                command.Parameters.AddWithValue("@Email", email);
                command.Parameters.AddWithValue("@PhoneNumber", phoneNumber);
                command.Parameters.AddWithValue("@Country", country);
                command.Parameters.AddWithValue("@DateOfBirth", dateOfBirth);
                command.Parameters.AddWithValue("@Password", hashedPassword);

                connection.Open();
                object result = command.ExecuteScalar();
                connection.Close();

                if (result != null && int.TryParse(result.ToString(), out int id))
                {
                    userId = id;
                }
            }

            return userId;
        }


        //Hash Password
        private string HashPassword(string password)
        {
            // Create a SHA256 hash object.
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // Compute and return the hash as a hexadecimal string.
                return BitConverter.ToString(sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password))).Replace("-", String.Empty).ToLowerInvariant();
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
}