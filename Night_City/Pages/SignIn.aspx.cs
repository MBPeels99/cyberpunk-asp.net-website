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
using Night_City.Utilities.sql;
using Night_City.Utilities.validation;

namespace Night_City.Pages
{
    public partial class SignIn : System.Web.UI.Page
    {
        string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        private SignInValidation _SignInValidation;
        private SessionManager _SessionManager = new SessionManager();
        private SQLHelper _SQLHelper = new SQLHelper();
        
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

            if (!_SignInValidation.IsValidEmail(email))
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
            if (!AreSignUpFieldsValid(fullName, email, phoneNumber, country, dateOfBirthStr, password, confirmPassword))
            {
                return; // Error message is set within AreSignUpFieldsValid
            }

            DateTime dateOfBirth = DateTime.Parse(txtDateOfBirth.Value);
            // Hash the password before storing it into the database
            string hashedPassword = HashPassword(password);

            try
            {
                // Store the user information in the database and retrieve the user ID
                int userId = _SQLHelper.StoreUserInfoAndGetId(fullName, email, phoneNumber, country, dateOfBirth, hashedPassword);

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

        private bool AreSignUpFieldsValid(string fullName, string email, string phoneNumber, string country, string dateOfBirthStr, string password, string confirmPassword)
        {
            if (string.IsNullOrEmpty(fullName) || string.IsNullOrEmpty(email) ||
                string.IsNullOrEmpty(phoneNumber) || string.IsNullOrEmpty(country) ||
                string.IsNullOrEmpty(dateOfBirthStr) || string.IsNullOrEmpty(password) ||
                string.IsNullOrEmpty(confirmPassword))
            {
                lblError.Text = "All fields must be filled.";
                return false;
            }

            if (!_SignInValidation.IsValidName(fullName))
            {
                lblError.Text = "Please enter a valid name.";
                return false;
            }

            if (!_SignInValidation.IsValidEmail(email))
            {
                lblError.Text = "Please enter a valid email.";
                return false;
            }

            if (!_SignInValidation.IsValidPhoneNumber(phoneNumber, country))
            {
                lblError.Text = "Please enter a valid phone number for your selected country.";
                return false;
            }

            if (password != confirmPassword)
            {
                lblError.Text = "Password and password confirmation do not match.";
                return false;
            }

            return true;
        }
    }
}