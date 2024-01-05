using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Night_City.Pages
{
    public partial class Default : System.Web.UI.Page
    {
        string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=\"E:\\CodingProjects\\Dot Net\\Night_City\\App_Data\\NightCity.mdf\";Integrated Security=True";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                UpdateDeathCount();
            }
        }

        protected void UpdateDeathCount()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT DeathCount FROM Deaths WHERE Date=@Date";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Date", DateTime.Today);

                    int deathCount = 0;

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            deathCount = (int)reader["DeathCount"];
                            lblDeathCount.Text = deathCount.ToString();
                        }
                    }

                    if (deathCount == 0)
                    {
                        Random rnd = new Random();
                        int randomDeathCount = rnd.Next(10, 80);

                        // Generate a unique identifier for the Id column
                        Guid uniqueId = Guid.NewGuid();

                        query = "INSERT INTO Deaths (DeathCount, Date) VALUES (@DeathCount, @Date)";
                        SqlCommand cmd = new SqlCommand(query, connection);
                        cmd.Parameters.AddWithValue("@DeathCount", randomDeathCount);
                        cmd.Parameters.AddWithValue("@Date", DateTime.Today);
                        cmd.ExecuteNonQuery();

                        lblDeathCount.Text = randomDeathCount.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the exception message for debugging purposes
                System.Diagnostics.Trace.WriteLine("Error occurred: " + ex.Message);

                // Handle the exception appropriately, e.g., display a user-friendly message
                lblErrorMessage.Text = "An error occurred while updating the death count. Please try again later.";
            }
        }



    }
}
