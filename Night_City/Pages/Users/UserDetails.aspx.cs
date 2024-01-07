using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Night_City.Pages
{
    public partial class UserDetails : System.Web.UI.Page
    {
        string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=\"E:\\CodingProjects\\Dot Net\\Night_City\\App_Data\\NightCity.mdf\";Integrated Security=True";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                int userId = int.Parse(Request.QueryString["id"]);
                LoadUserDetails(userId);
            }
        }

        private void LoadUserDetails(int userId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT UserId, FullName, Email, PhoneNumber, Country, DateOfBirth   FROM Users WHERE UserId = @UserId";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@UserId", userId);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            lblUserId.Text += reader["UserId"].ToString();
                            txtFullName.Text += reader["FullName"].ToString();
                            txtEmail.Text += reader["Email"].ToString();
                            txtPhoneNumber.Text += reader["PhoneNumber"].ToString();
                            txtCountry.Text += reader["Country"].ToString();
                            txtDateOfBirth.Text += reader["DateOfBirth"].ToString();

                        }
                    }
                }
            }
        }

        private bool UpdateUserDetails(int userId)
        {
            bool userExists = false;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();

                // Check if the user still exists
                string checkQuery = "SELECT COUNT(1) FROM Users WHERE UserId = @UserId";
                using (SqlCommand checkCmd = new SqlCommand(checkQuery, con))
                {
                    checkCmd.Parameters.AddWithValue("@UserId", userId);
                    userExists = (int)checkCmd.ExecuteScalar() > 0;
                }

                // If the user exists, update their details
                if (userExists)
                {
                    string updateQuery = "UPDATE Users SET FullName = @FullName, Email = @Email, PhoneNumber = @PhoneNumber, Country = @Country, DateOfBirth = @DateOfBirth WHERE UserId = @UserId";
                    using (SqlCommand updateCmd = new SqlCommand(updateQuery, con))
                    {
                        updateCmd.Parameters.AddWithValue("@UserId", userId);
                        updateCmd.Parameters.AddWithValue("@FullName", txtFullName.Text);
                        updateCmd.Parameters.AddWithValue("@Email", txtEmail.Text);
                        updateCmd.Parameters.AddWithValue("@PhoneNumber", txtPhoneNumber.Text);
                        updateCmd.Parameters.AddWithValue("@Country", txtCountry.Text);
                        updateCmd.Parameters.AddWithValue("@DateOfBirth", Convert.ToDateTime(txtDateOfBirth.Text));

                        updateCmd.ExecuteNonQuery();
                    }
                }
            }
            return userExists;
        }

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            // Enable TextBoxes for editing
            txtFullName.ReadOnly = false;
            txtEmail.ReadOnly = false;
            txtPhoneNumber.ReadOnly = false;
            txtCountry.ReadOnly = false;
            txtDateOfBirth.ReadOnly = false;
            // Similarly for other TextBoxes

            btnSave.Visible = true;
            btnCancel.Visible = true;
            btnEdit.Visible = false;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            int userId = int.Parse(Request.QueryString["id"]);
            bool userExists = UpdateUserDetails(userId);

            if (userExists)
            {
                // Disable TextBoxes after saving
                txtFullName.ReadOnly = true;
                txtEmail.ReadOnly = true;
                txtPhoneNumber.ReadOnly = true;
                txtCountry.ReadOnly = true;
                txtDateOfBirth.ReadOnly = true;

                // Switch back buttons visibility
                btnSave.Visible = false;
                btnCancel.Visible = false;
                btnEdit.Visible = true;
                // Optionally, show a success message
            }
            else
            {
                // User does not exist
                // Show an error message or handle accordingly
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            // Reload the original data to reset changes
            int userId = int.Parse(Request.QueryString["id"]);
            LoadUserDetails(userId);

            // Disable TextBoxes
            txtFullName.ReadOnly = true;
            txtEmail.ReadOnly = true;
            txtPhoneNumber.ReadOnly = true;
            txtCountry.ReadOnly = true;
            txtDateOfBirth.ReadOnly = true;
            // Switch back buttons visibility
            btnSave.Visible = false;
            btnCancel.Visible = false;
            btnEdit.Visible = true;
        }
    }
}