using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Night_City.Pages
{
    public partial class Profile : System.Web.UI.Page
    {
        static List<District> districts;
        HttpCookie districtCookie;

        string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=\"E:\\CodingProjects\\Dot Net\\Night_City\\App_Data\\NightCity.mdf\";Integrated Security=True";

        protected void Page_Load(object sender, EventArgs e)
        {
            // Retrieve the user's full name from the cookie
            HttpCookie userCookie = Request.Cookies["User"];
            districtCookie = Request.Cookies["District"];

            if (userCookie != null)
            {
                string fullName = userCookie["FullName"];

                // Set the welcome message
                welcomeMessage.InnerHtml = $"Welcome to Night City: {fullName}";
            }
            else
            {
                welcomeMessage.InnerHtml = $"Welcome to Night City: Tester";

                // No user cookie found, redirect to sign in page
                //Response.Redirect("SignIn.aspx");
            }

            // Fetch the district data from the database
            districts = GetDistrictsFromDatabase();

            // Bind the district data to the repeaters
            rptDistrictsLeft.DataSource = districts.Take(districts.Count / 2);
            rptDistrictsLeft.DataBind();

            rptDistrictsRight.DataSource = districts.Skip(districts.Count / 2);
            rptDistrictsRight.DataBind();

            // Display the content area of the first district
            if (!IsPostBack)
            {
                if (districtCookie != null && !string.IsNullOrEmpty(districtCookie["districtId"]))
                {
                    int districtId;
                    if (int.TryParse(districtCookie["districtId"], out districtId) && districtId >= 0 && districtId < districts.Count)
                    {
                        DisplayDistrictDetails(districts[districtId]);
                    }
                    else
                    {
                        // Invalid districtId, display details of the first district
                        DisplayDistrictDetails(districts[0]);
                    }
                }
                else
                {
                    districtCookie = new HttpCookie("District");
                    districtCookie.Expires = DateTime.Now.AddDays(30);
                    Response.Cookies.Add(districtCookie);
                    DisplayDistrictDetails(districts[0]);
                }
            }

        }


        protected void updateAccount_Click(object sender, EventArgs e)
        {
            // Redirect to the update account page
            Response.Redirect("~/Pages/UpdateAccount.aspx");
        }

        protected void deleteAccount_Click(object sender, EventArgs e)
        {
            // Redirect to the delete account page
            Response.Redirect("~/Pages/DeleteAccount.aspx");
        }
        protected void btnLogOut_Click(object sender, EventArgs e)
        {
            HttpCookie userCookie = Request.Cookies["User"];
            if (userCookie != null)
            {
                userCookie.Expires = DateTime.Now.AddDays(-1); // Set the expiration date to a past date
                Response.Cookies.Add(userCookie); // Add the updated cookie to the response
            }
            Session.Clear();
            // Redirect to the SignIn page
            Response.Redirect("~/Pages/SignIn.aspx");
        }

        protected void rptDistrictsLeft_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                ImageButton imageButton = e.Item.FindControl("ImageButton1") as ImageButton;
                if (imageButton != null)
                {
                    string imageUrl = (string)DataBinder.Eval(e.Item.DataItem, "ImageOne");
                    District district = (District)e.Item.DataItem; // Retrieve the district object

                    imageButton.ImageUrl = ResolveUrl(imageUrl);
                }
            }
        }

        protected void ImageButton_Click(object sender, ImageClickEventArgs e)
        {
            ImageButton button = (ImageButton)sender;
            string districtIdString = button.CommandArgument.ToString();

            if (!string.IsNullOrEmpty(districtIdString) && int.TryParse(districtIdString, out int districtId))
            {
                // Fetch the details of the selected district using districtId from your database
                District selectedDistrictObject = districts.FirstOrDefault(d => d.ID == districtId.ToString());

                if (selectedDistrictObject != null)
                {
                    // Update the districtId in the districtCookie
                    districtCookie["districtId"] = districtId.ToString();
                    Response.SetCookie(districtCookie);

                    // Update the districtId in the districts list
                    foreach (District district in districts)
                    {
                        if (district.ID == districtId.ToString())
                        {
                            district.ID = districtId.ToString();
                            break;
                        }
                    }

                    DisplayDistrictDetails(selectedDistrictObject);
                }
            }
        }


        private void DisplayDistrictDetails(District district)
        {
            // Find the selectedDistrict control
            HtmlGenericControl selectedDistrict = (HtmlGenericControl)contentArea.FindControl("selectedDistrict");

            if (selectedDistrict != null)
            {
                // Set the visibility of the selected district controls
                selectedDistrict.Style["display"] = "block";

                // Find the selectedDistrictName and selectedDistrictImage controls within the selectedDistrict
                HtmlGenericControl selectedDistrictName = (HtmlGenericControl)selectedDistrict.FindControl("selectedDistrictName");
                Image selectedDistrictImage = (Image)selectedDistrict.FindControl("selectedDistrictImage");
                Image selectedDistrictMap = (Image)selectedDistrict.FindControl("selectedDistrictMap");

                if (selectedDistrictName != null && selectedDistrictImage != null && selectedDistrictMap != null)
                {
                    // Display the selected district details
                    selectedDistrictName.InnerText = district.Name;
                    selectedDistrictImage.ImageUrl = district.Back;
                    selectedDistrictMap.ImageUrl = district.Map;
                }

                selectedDistrictDescription.InnerText = district.Description;
                // Trigger data-binding for the current page
                DataBind();

                // Update the districtId in the districtCookie
                districtCookie["districtId"] = district.ID.ToString(); // Update the districtId as a string
                Response.Cookies.Set(districtCookie);
            }
        }


        private List<District> GetDistrictsFromDatabase()
        {
            List<District> districts = new List<District>();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = "SELECT * FROM Districts";
                    SqlCommand command = new SqlCommand(query, connection);

                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        District district = new District();
                        district.ID = reader["DistrictId"].ToString();
                        district.Name = reader["DistrictName"].ToString();

                        // Modify the ImageOne property to include the district name
                        string districtName = reader["DistrictName"].ToString();
                        // Encode the district name in the image URL
                        string encodedDistrictName = Uri.EscapeDataString(districtName);
                        district.ImageOne = $"~/Pictures/District/{encodedDistrictName}/{reader["ImageOne"]}";
                        district.Back = $"~/Pictures/District/{encodedDistrictName}/{reader["BackImage"]}";
                        district.Map = $"~/Pictures/District/{encodedDistrictName}/{reader["ImageMap"]}";
                        district.Description = reader["Description"].ToString();

                        districts.Add(district);
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle the exception (e.g., log it, show an error message, etc.)
            }

            return districts;
        }

        [WebMethod]
        public static string ConfirmDates(string startDate, string endDate)
        {
            try
            {
                DateTime start;
                DateTime end;
                DateTime currentDate = DateTime.Today;

                if (!DateTime.TryParse(startDate, out start) || !DateTime.TryParse(endDate, out end))
                {
                    return "Please select both 'From' and 'To' dates.";
                }

                if (start >= end)
                {
                    return "Invalid date selection. The 'From' date should be earlier than the 'To' date.";
                }

                if (start < currentDate || end <= currentDate)
                {
                    return "Invalid date selection. Dates should be in the future.";
                }

                // Assume that 'UserId' and 'DistrictId' are stored in session or cookies when the user logs in
                string userId = HttpContext.Current.Request.Cookies["User"]?["UserId"];
                string districtId = HttpContext.Current.Request.Cookies["District"]?["districtId"];

                if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(districtId))
                {
                    return "User information or district selection is missing.";
                }

                // Set the session variables
                HttpContext.Current.Session["StartDate"] = start;
                HttpContext.Current.Session["EndDate"] = end;

                // Add the trip to the database
                AddUserTrip(userId, districtId, start, end);

                // If you need to redirect to another page, you'll have to handle that on the front end after success
                return "Success";
            }
            catch (Exception ex)
            {
                // Log the exception details for debugging
                Console.WriteLine("Exception: " + ex.ToString());
                return "An error occurred while processing your request. Please try again later.";
            }
        }

        private static void AddUserTrip(string userId, string districtId, DateTime startDate, DateTime endDate)
        {
            string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=\"C:\\Users\\Gebruiker\\Documents\\NWU\\2023\\CMPG 212\\Assignments\\Assignment Assessment 2\\Night_City\\App_Data\\NightCity.mdf\";Integrated Security=True";
            string query = @"INSERT INTO UserTrips (UserId, DistrictId, StartDate, EndDate) 
                 VALUES (@UserId, @DistrictId, @StartDate, @EndDate)";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserId", userId);
                    command.Parameters.AddWithValue("@DistrictId", districtId);
                    command.Parameters.AddWithValue("@StartDate", startDate);
                    command.Parameters.AddWithValue("@EndDate", endDate);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
            catch (SqlException ex)
            {
                // Log the SQL exception here
                Console.WriteLine("SQL Exception: " + ex.ToString());
                throw;
            }
        }



        public class District
        {
            public string ID { get; set; }
            public string Name { get; set; }
            public string ImageOne { get; set; }
            public string Back { get; set; }
            public string Description { get; set; }
            public string Map { get; set; }
        }
    }
}