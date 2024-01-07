using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Night_City.Utilities.sessions;
using Night_City.Utilities.validation;

namespace Night_City.Pages
{
    public partial class SignIn : System.Web.UI.Page
    {
        string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        private SignInValidation _SignInValidation;
        private SessionManager _SessionManager = new SessionManager();
        
        protected void Page_Load(object sender, EventArgs e)
        {
            _SignInValidation = new SignInValidation();
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
            string email = txtEmailSignIn.Value;
            string password = txtPassword.Value;

            if (!IsValidEmail(email))
            {
                lblError.Text = "Please enter a valid email.";
                lblError.Visible = true;
                return;
            }

            AttemptSignIn(email, password);
        }

        private void AttemptSignIn(string email, string password)
        {
            string hashedPassword = HashPassword(password);

            try
            {
                bool result = _SignInValidation.ValidateUser(email, hashedPassword);
                if (result)
                {
                    result = _SessionManager.ManageUserSession(email);
                    if (result)
                    {
                        RedirectUserBasedOnSecurityLevel();
                    } 
                    else
                    {
                        lblError.Text = "Log In failed. Please Try Again.";
                        lblError.Visible = true;
                    }
                }
                else
                {
                    lblError.Text = "Incorrect email or password.";
                    lblError.Visible = true;
                }
            }
            catch (Exception ex)
            {
                lblError.Text = "An error occurred while signing in. Please try again.";
                lblError.Visible = true;
                // Log the exception
            }
        }

        private void RedirectUserBasedOnSecurityLevel()
        {
            if (Session["SecurityLevel"].ToString() == "0")
            {
                Response.Redirect("~/Pages/AdminPage.aspx");
            }
            else if (Session["SecurityLevel"].ToString() == "-1")
            {
                Response.Redirect("~/Pages/ExplorePage.aspx");
            }
            // Additional conditions as needed
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
                        // Store user details in session
                        Session["FullName"] = fullName;
                        Session["UserId"] = userId;

                        Response.Redirect("~/Pages/ExplorePage.aspx");
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