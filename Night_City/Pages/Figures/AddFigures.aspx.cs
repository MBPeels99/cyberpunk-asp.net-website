using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Night_City.Utilities.sql;
using Night_City.Utilities.functions;
using System.Data.SqlTypes;
using System.Data.SqlClient;
using System.Security.Cryptography.X509Certificates;
using System.Web.Services;
using System.IO;

namespace Night_City.Pages.Figures
{
    public partial class AddFigures : System.Web.UI.Page
    {
        private SQLHelper _sqlHelper = new SQLHelper();
        private Image_Functions _functions = new Image_Functions();
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                BindOccupations();
                BindAffiliations();
            }
        }

        protected void BindOccupations()
        {
            // Retrieve Occupations
            DataTable occupations =  _sqlHelper.GetAllDataFromTable("Occupations");
            // Bind Data Retrieved to DropDownList
            Occupation.DataSource = occupations;
            Occupation.DataTextField = "OccupationName";
            Occupation.DataValueField = "OccupationID";
            Occupation.DataBind();


            // Add a default item if needed
            Occupation.Items.Insert(0, new ListItem("--Select Occupation--", "0"));
        }

        protected void BindAffiliations()
        {
            // Retrieve Affiliations 
            DataTable affiliations = _sqlHelper.GetAllDataFromTable("Affiliations");

            // Bind Data Retrieved to DropDownList
            Affiliation.DataSource = affiliations;
            Affiliation.DataTextField = "Name";
            Affiliation.DataValueField = "AffiliationID";
            Affiliation.DataBind();

            // Add a default item if needed
            Affiliation.Items.Insert(0, new ListItem("--Select Affiliation--", "0"));
        }

        protected void btnAddFigure_Click(object sender, EventArgs e)
        {
            int figureId = addFigure();
            if (figureId > 0) // Check if the figure was added successfully
            {
                addFigureOccupation(figureId);
                addFigureAffiliation(figureId);
                // Redirect to success page or display a success message
            }
            else
            {
                // Handle the error case
            }
        }

        private int addFigure()
        {
            try
            {
                Log("addFigure method started.");
                /*
                if (!ValidateForm())
                {
                    return 0;
                }*/

                string figureImage = null;

                // Handle file upload for the figure image
                if (fuFigureImage.HasFile)
                {
                    Log("File upload detected, attempting to save file.");
                    figureImage = _functions.SaveFile(HttpContext.Current,"Figures", fuFigureImage, txtFullName.Value);
                    // Note: Make sure the SaveFile method actually saves the file and returns the path
                    Log($"File saved: {figureImage}");
                }

                // Construct SQL query for insertion
                string insertQuery = "INSERT INTO Figures (FullName, StageName, DateOfBirth, DateOfDeath, PlaceOfBirth, PlaceOfDeath, Status, Gender, HairColor, EyeColor, KnownFor, Background, Partner, VoicedBy, Image) VALUES (@FullName, @StageName, @DateOfBirth, @DateOfDeath, @PlaceOfBirth, @PlaceOfDeath, @Status, @Gender, @HairColor, @EyeColor, @KnownFor, @Background, @Partner, @VoicedBy, @Image)";

                // Add parameters to the query
                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    new SqlParameter("@FullName", txtFullName.Value),
                    new SqlParameter("@StageName",  txtStageName.Value),
                    new SqlParameter("@DateOfBirth", txtDateOfBirth.Value),
                    new SqlParameter("@DateOfDeath", txtDateOfDeath.Value),
                    new SqlParameter("@PlaceOfBirth", txtPlaceOfBirth.Value),
                    new SqlParameter("@PlaceOfDeath", txtPlaceOfDeath.Value),
                    new SqlParameter("@Status", txtStatus.Value),
                    new SqlParameter("@Gender", ddlGender.SelectedValue),
                    new SqlParameter("@HairColor", txtHairColor.Value),
                    new SqlParameter("@EyeColor", txtEyeColor.Value),
                    new SqlParameter("@KnownFor", txtKnownFor.Text),
                    new SqlParameter("@Background", txtBackground.Text),
                    new SqlParameter("@Partner", txtPartner.Value),
                    new SqlParameter("@VoicedBy", txtVoicedBy.Value),
                    new SqlParameter("@Image", figureImage),
                };

                // Execute the query
                int result = _sqlHelper.ExecuteQueryAndGetId(insertQuery, parameters);

                Log($"Figure added with ID: {result}");

                return result;

            }
            catch (Exception ex)
            {
                Log($"Error in addFigure: {ex.Message}");
                // Log error and display error message
                return 0;
            }
        }

        private bool ValidateForm()
        {
            string fullName = txtFullName.Value;
            string stageName = txtStageName.Value;
            string dateOfBirth = txtDateOfBirth.Value;
            string dateOfDeath = txtDateOfDeath.Value;
            string placeOfBirth = txtPlaceOfBirth.Value;
            string status = txtStatus.Value;
            string gender = ddlGender.SelectedValue;
            string hairColor = txtHairColor.Value;
            string eyeColor = txtEyeColor.Value;
            string occupation = Occupation.SelectedValue;
            string affiliation = Affiliation.SelectedValue;
            string knownFor = txtKnownFor.Text;
            string background = txtBackground.Text;
            string partner = txtPartner.Value;
            string voicedBy = txtVoicedBy.Value;

            // Full Name Validation
            if (string.IsNullOrWhiteSpace(fullName)){ return false; }
            if (string.IsNullOrWhiteSpace(placeOfBirth)){ return false; }
            if (string.IsNullOrWhiteSpace(status)){ return false; }
            if (string.IsNullOrWhiteSpace(gender)){ return false; }
            if (string.IsNullOrWhiteSpace(hairColor)){ return false; }
            if (string.IsNullOrWhiteSpace(eyeColor)){ return false; }
            if (string.IsNullOrWhiteSpace(knownFor)){ return false; }
            if (string.IsNullOrWhiteSpace(background)){ return false; }
            if (string.IsNullOrWhiteSpace(partner)){ return false; }
            if (string.IsNullOrWhiteSpace(voicedBy)){ return false; }

            DateTime dob;
            if (!DateTime.TryParse(dateOfBirth, out dob)){ return false; }
            // Not Always required
            if (!string.IsNullOrWhiteSpace(dateOfDeath))
            { 
                DateTime dod;
                if (!DateTime.TryParse(dateOfDeath, out dod))
                {
                    // Set an error message or flag
                    return false;
                }
            }

            // Gender Validation (if you have specific set of values)
            if (!new List<string> { "Male", "Female", "Other" }.Contains(gender))
            {
                // Set an error message or flag
                return false;
            }

            // Occupation and Affiliation Validation (should not be default or empty values)
            if (occupation == "0" || affiliation == "0")
            {
                // Set an error message or flag
                return false;
            }

            // If all validations pass
            return true;
        }


        private void addFigureOccupation(int figureId)
        {
            foreach (ListItem item in Occupation.Items)
            {
                if (item.Selected)
                {
                    string occupationId = item.Value;
                    string insertQuery = "INSERT INTO FiguresOccupations (FigureId, OccupationId) VALUES (@FigureId, @OccupationId)";
                    List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@FigureId", figureId),
                new SqlParameter("@OccupationId", occupationId)
            };

                    _sqlHelper.ExecuteQuery(insertQuery, parameters);
                }
            }
        }

        private void addFigureAffiliation(int figureId)
        {
            foreach (ListItem item in Affiliation.Items)
            {
                if (item.Selected)
                {
                    string affiliationId = item.Value;
                    string insertQuery = "INSERT INTO FigureAffiliations (FigureId, AffiliationId) VALUES (@FigureId, @AffiliationId)";
                    List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@FigureId", figureId),
                new SqlParameter("@AffiliationId", affiliationId)
            };

                    _sqlHelper.ExecuteQuery(insertQuery, parameters);
                }
            }
        }


        [WebMethod]
        public static string AddOccupation(string occupationName)
        {
            if (string.IsNullOrWhiteSpace(occupationName))
            {
                return "Occupation name cannot be empty.";
            }

            try
            {
                SQLHelper _sqlHelper = new SQLHelper(); // Assuming SQLHelper is your class for database operations

                // Construct the SQL query to insert the new occupation
                string insertQuery = "INSERT INTO Occupations (OccupationName) VALUES (@OccupationName)";
                List<SqlParameter> parameters = new List<SqlParameter>
        {
            new SqlParameter("@OccupationName", occupationName)
        };

                // Execute the query
                bool result = _sqlHelper.ExecuteQuery(insertQuery, parameters);

                if (result)
                {
                    return "Occupation added successfully.";
                }
                else
                {
                    return "Failed to add occupation.";
                }
            }
            catch (Exception ex)
            {
                // Log the exception
                return "Error: " + ex.Message; // In a production environment, you might want to return a more generic error message
            }
        }

        [WebMethod]
        public static string AddAffiliation(string AffiliationName, string AffiliationDescription)
        {
            if (string.IsNullOrWhiteSpace(AffiliationName))
            {
                return "Affiliation name cannot be empty.";
            }

            if (string.IsNullOrWhiteSpace(AffiliationDescription))
            {
                return "Affiliation description cannot be empty.";
            }

            try
            {
                SQLHelper _sqlHelper = new SQLHelper(); // Assuming SQLHelper is your class for database operations

                // Construct the SQL query to insert the new occupation
                string insertQuery = "INSERT INTO Affiliations (Name, Description) VALUES (@AffiliationName,@AffiliationDescription)";
                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    new SqlParameter("@AffiliationName", AffiliationName),
                    new SqlParameter("@AffiliationDescription",AffiliationDescription)
                };

                // Execute the query
                bool result = _sqlHelper.ExecuteQuery(insertQuery, parameters);

                if (result)
                {
                    return "Affiliation added successfully.";
                }
                else
                {
                    return "Failed to add Affiliation.";
                }
            }
            catch (Exception ex)
            {
                // Log the exception
                return "Error: " + ex.Message; // In a production environment, you might want to return a more generic error message
            }
        }

        private void Log(string message)
        {
            string logFile = HttpContext.Current.Server.MapPath("~/Logs/log.txt");
            string logEntry = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}\n";

            File.AppendAllText(logFile, logEntry);
        }


    }
}