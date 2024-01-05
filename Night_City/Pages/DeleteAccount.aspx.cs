using System;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI;

namespace Night_City.Pages
{
    public partial class DeleteAccount : System.Web.UI.Page
    {
        string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=\"E:\\CodingProjects\\Dot Net\\Night_City\\App_Data\\NightCity.mdf\";Integrated Security=True";

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            string password = txtPassword.Value.ToString();

            if (!string.IsNullOrEmpty(password))
            {
                try
                {
                    HttpCookie userCookie = new HttpCookie("User");
                    String userId = userCookie["UserId"];
                    // Verify the password before deleting the account
                    if (VerifyPassword(userId, password))
                    {
                        // Delete the user account from the database
                        DeleteUserAccount(userId);

                        // Clear the session and redirect to the sign-in page
                        Session.Clear();
                        Response.Redirect("~/Pages/SignIn.aspx");
                    }
                    else
                    {
                        lblError.Text = "Incorrect password. Please enter the correct password to delete your account.";
                        lblError.Visible = true;
                    }
                }
                catch (Exception ex)
                {
                    lblError.Text = "An error occurred while deleting the account. Please try again.";
                    lblError.Visible = true;
                }
            }
            else
            {
                lblError.Text = "Please enter your password to delete your account.";
                lblError.Visible = true;
            }
        }

        private bool VerifyPassword(string userId, string password)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT Password FROM Users WHERE UserId = @UserId";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@UserId", userId);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    string hashedPassword = reader["Password"].ToString();
                    return VerifyHashedPassword(password, hashedPassword);
                }

                return false;
            }
        }

        private bool VerifyHashedPassword(string password, string hashedPassword)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                string enteredPasswordHash = builder.ToString();

                return hashedPassword.Equals(enteredPasswordHash);
            }
        }

        private void DeleteUserAccount(string userId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "DELETE FROM Users WHERE UserId = @UserId";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@UserId", userId);

                connection.Open();
                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected <= 0)
                {
                    throw new Exception("No rows were affected. The user account could not be deleted.");
                }
            }
        }
    }
}
