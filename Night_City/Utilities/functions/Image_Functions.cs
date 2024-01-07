using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Night_City.Utilities.functions
{
    public class Image_Functions
    {

        public Image_Functions() { }

        public string SaveFile(HttpContext context, FileUpload fileUploadControl, string Name)
        {
            if (fileUploadControl.HasFile)
            {
                string fileName = $"{Name}{Path.GetExtension(fileUploadControl.FileName)}";
                string directoryPath = context.Server.MapPath($"~/Pictures/District/{SanitizeDirectoryName(Name)}/DistrictImages/");

                // Will Check if directory for district exists
                Directory.CreateDirectory(directoryPath);
                string savePath = Path.Combine(directoryPath, fileName);
                return fileName; // Return the file name to be stored in DB
            }
            return null;
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