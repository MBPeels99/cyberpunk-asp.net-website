using Night_City.Utilities.functions;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Night_City.Pages
{
    public partial class AddDistrict : System.Web.UI.Page
    {
        string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        private Image_Functions _functions;
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnAddDistrict_Click(object sender, EventArgs e)
        {
            string districtName = txtDistrictName.Text.Trim();
            string description = txtDescription.Text.Trim();
            string imageMap = _functions.SaveFile(HttpContext.Current, "District", fuImageMap, districtName+"_Image_Map");
            string backImage = _functions.SaveFile(HttpContext.Current, "District", fuBackImage, districtName + "Back_Image");
            string imageOne = _functions.SaveFile(HttpContext.Current, "District", fuImageOne, districtName + "Image_One");
            string imageTwo = _functions.SaveFile(HttpContext.Current, "District", fuImageTwo, districtName + "Image_Two");

            // Now insert these details into the database
            InsertDistrictIntoDB(districtName, description, imageMap, backImage, imageOne, imageTwo);
        }

        /*private string SaveFile(FileUpload fileUploadControl, string districtName, string imageType)
        {
            if (fileUploadControl.HasFile)
            {
                string fileName = $"{districtName}_{imageType}{Path.GetExtension(fileUploadControl.FileName)}";
                string directoryPath = Server.MapPath($"~/Pictures/District/{SanitizeDirectoryName(districtName)}/DistrictImages/");

                // Will Check if directory for district exists
                Directory.CreateDirectory(directoryPath);
                string savePath = Path.Combine(directoryPath, fileName);
                return fileName; // Return the file name to be stored in DB
            }
            return null;
        }*/

        private void InsertDistrictIntoDB(string districtName, string description, string imageMap, string backImage, string imageOne, string imageTwo)
        {
            string query = @"INSERT INTO Districts (DistrictName, Description, ImageMap, BackImage, ImageOne, ImageTwo) 
                             VALUES (@DistrictName, @Description, @ImageMap, @BackImage, @ImageOne, @ImageTwo)";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@DistrictName", districtName);
                    cmd.Parameters.AddWithValue("@Description", description);
                    cmd.Parameters.AddWithValue("@ImageMap", imageMap);
                    cmd.Parameters.AddWithValue("@BackImage", backImage ?? (object)DBNull.Value); // Use DBNull for null values
                    cmd.Parameters.AddWithValue("@ImageOne", imageOne ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@ImageTwo", imageTwo ?? (object)DBNull.Value);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            // Optionally, redirect to a confirmation page or show a success message
        }

        private string SanitizeDirectoryName(string directoryName)
        {
            // Replace invalid characters with an underscore or another valid character
            // You can expand this method to include other checks as necessary
            foreach (char c in Path.GetInvalidFileNameChars())
            {
                directoryName = directoryName.Replace(c, '_');
            }
            return directoryName;
        }

    }
}