using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlTypes;

namespace Night_City.Pages
{
    public partial class AdminPage : System.Web.UI.Page
    {
        string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=\"E:\\CodingProjects\\Dot Net\\Night_City\\App_Data\\NightCity.mdf\";Integrated Security=True";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadDistricts(); // default view
            }
        }

        protected void btnDistricts_Click(object sender, EventArgs e)
        {
            DistrictPanel.Visible = true;
            UserPanel.Visible = false;
            EmployeePanel.Visible = false;

            LoadDistricts();
        }

        protected void btnFigures_Click(object sender, EventArgs e)
        {
            DistrictPanel.Visible = false;
            UserPanel.Visible = false;
            EmployeePanel.Visible = false;

            LoadFigures();
        }

        protected void btnUsers_Click(object sender, EventArgs e)
        {
            DistrictPanel.Visible = false;
            UserPanel.Visible = true;
            EmployeePanel.Visible = false;

            LoadUsers();
        }

        protected void btnEmployees_Click(object sender, EventArgs e)
        {
            DistrictPanel.Visible = false;
            UserPanel.Visible = false;
            EmployeePanel.Visible = true;

            LoadEmployees();
        }

        private void LoadDistricts()
        {
            gvDistricts.DataSource = GetDistrictsFromDb();
            gvDistricts.DataBind();
        }

        private void LoadFigures()
        {
            // Similar to LoadDistricts, but for figures
        }

        private void LoadUsers()
        {
            gvUsers.DataSource = GetUsersFromDb(false);
            gvUsers.DataBind();
        }

        private void LoadEmployees()
        {
            gvEmployees.DataSource = GetUsersFromDb(true);
            gvEmployees.DataBind();
        }

        // Example method to generate a table with View, Edit, Delete buttons
        private Table GenerateTableWithActions()
        {
            Table table = new Table();
            // Add table headers and other rows dynamically
            // For each row, add cells and buttons for View, Edit, Delete
            return table;
        }

        private DataTable GetDistrictsFromDb()
        {
            DataTable dtDistricts = new DataTable();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT DistrictId, DistrictName, Description FROM Districts";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        da.Fill(dtDistricts);
                    }
                }
                catch (Exception ex)
                {
                    // Handle exceptions
                }
            }
            return dtDistricts;
        }

        private DataTable GetUsersFromDb(bool IsEmployee)
        {
            DataTable dtUsers = new DataTable();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT UserId,FullName,Email FROM Users WHERE IsEmployee = @IsEmployee";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@IsEmployee", IsEmployee);

                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        da.Fill(dtUsers);
                    }
                }
                catch (Exception ex)
                {
                    // Handle exceptions
                }
            }
            return dtUsers;
        }

        protected void DeleteDistrict(object sender, EventArgs e)
        {
            LinkButton lnkDelete = (LinkButton)sender;
            int districtId = Convert.ToInt32(lnkDelete.CommandArgument);
            // Code to delete the district from the database using districtId
            // After deletion, reload districts
            LoadDistricts();
        }

        protected void ViewDistrict(object sender, EventArgs e)
        {
            LinkButton lnkView = (LinkButton)sender;
            int districtId = Convert.ToInt32(lnkView.CommandArgument);
            Response.Redirect($"ViewDistrict.aspx?id={districtId}");
        }

        protected void EditDistrict(object sender, EventArgs e)
        {
            LinkButton lnkEdit = (LinkButton)sender;
            int districtId = Convert.ToInt32(lnkEdit.CommandArgument);
            Response.Redirect($"EditDistrict.aspx?id={districtId}");
        }

        protected void DeleteUser(object sender, EventArgs e)
        {
            LinkButton lnkDelete = (LinkButton)sender;
            int UserId = Convert.ToInt32(lnkDelete.CommandArgument);
            // Code to delete the district from the database using districtId
            // After deletion, reload districts
            LoadUsers();
        }

        protected void ViewUser(object sender, EventArgs e)
        {
            LinkButton lnkView = (LinkButton)sender;
            int UserId = Convert.ToInt32(lnkView.CommandArgument);
            Response.Redirect($"UserDetails.aspx?id={UserId}");
        }

        protected void EditUser(object sender, EventArgs e)
        {
            LinkButton lnkEdit = (LinkButton)sender;
            int UserId = Convert.ToInt32(lnkEdit.CommandArgument);
            Response.Redirect($"EditDistrict.aspx?id={UserId}");
        }

    }
}