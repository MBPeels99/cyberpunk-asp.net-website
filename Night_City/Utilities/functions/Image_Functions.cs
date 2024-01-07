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

        public string SaveFile(HttpContext context, string ImageTheme, FileUpload fileUploadControl, string Name)
        {
            if (fileUploadControl.HasFile)
            {
                string fileName = $"{Name}{Path.GetExtension(fileUploadControl.FileName)}";
                string directoryPath = context.Server.MapPath($"~/Pictures/{ImageTheme}/{SanitizeDirectoryName(Name)}/");

                // Will Check if directory for district exists
                Directory.CreateDirectory(directoryPath);

                string savePath = Path.Combine(directoryPath, fileName);

                // Save the file
                fileUploadControl.SaveAs(savePath);

                // Return the relative path as needed (or just the file name if that's all you need)
                return $"~/Pictures/{ImageTheme}/{SanitizeDirectoryName(Name)}/{fileName}";
            }
            return null;
        }

        private string SanitizeDirectoryName(string directoryName)
        {
            foreach (char c in Path.GetInvalidFileNameChars())
            {
                directoryName = directoryName.Replace(c, '_');
            }
            return directoryName;
        }
    }
}