using System;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI.WebControls;

namespace Night_City.Pages
{
    public partial class ConfirmationPage : System.Web.UI.Page
    {
        string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=\"E:\\CodingProjects\\Dot Net\\Night_City\\App_Data\\NightCity.mdf\";Integrated Security=True";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Retrieve the user's full name from the session
                string fullName = GetUserFullName();
                lblFullName.Text = fullName;
                if (Session["StartDate"] != null && Session["EndDate"] != null)
                {
                    lblFromDate.Text = ((DateTime)Session["StartDate"]).ToString("MMM dd, yyyy");
                    lblToDate.Text = ((DateTime)Session["EndDate"]).ToString("MMM dd, yyyy");
                }
                else
                {
                    // Handle the case when session variables are not set
                }
            }
        }


        protected void btnReturn_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Pages/ExplorePage.aspx");
        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            HttpCookie userCookie = Request.Cookies["User"];
            if (userCookie != null)
            {
                userCookie.Expires = DateTime.Now.AddDays(-1); // Set the expiration date to a past date
                Response.Cookies.Add(userCookie); // Add the updated cookie to the response
            }
            Session.Clear();
            // Redirect to the delete account page
            Response.Redirect("~/Pages/SignIn.aspx");
        }

        private string GetUserFullName()
        {
            string fullName = string.Empty;

            // Retrieve the user ID from the user cookie
            HttpCookie userCookie = Request.Cookies["User"];
            if (userCookie != null)
            {
                string userId = userCookie["UserId"];

                try
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        string query = "SELECT FullName FROM Users WHERE UserId = @UserId";
                        SqlCommand command = new SqlCommand(query, connection);
                        command.Parameters.AddWithValue("@UserId", userId);

                        connection.Open();
                        object result = command.ExecuteScalar();

                        if (result != null)
                        {
                            fullName = result.ToString();
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Handle the exception (e.g., log it, show an error message, etc.)
                }
            }

            return fullName;
        }

    }
}
