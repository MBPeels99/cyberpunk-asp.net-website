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

namespace Night_City.Pages.Figures
{
    public partial class AddFigures : System.Web.UI.Page
    {
        private SQLHelper _sqlHelper = new SQLHelper();
        private Image_Functions _functions;
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
                // Retrieve value from form controls
                string fullName = txtFullName.Value;
                string stageName = txtStageName.Value;
                string dateOfBirth = txtDateOfBirth.Value;
                string dateOfDeath = txtDateOfDeath.Value;
                string placeOfBirth = txtPlaceOfBirth.Value;
                string placeOfDeath = txtPlaceOfDeath.Value;
                string status = txtStatus.Value;
                string gender = txtGender.Value;
                string hairColor = txtHairColor.Value;
                string eyeColor = txtEyeColor.Value;
                string occupation = Occupation.SelectedValue;
                string affiliation = Affiliation.SelectedValue;
                string knownFor = txtKnownFor.Text.ToString();
                string background = txtBackground.Text.ToString();
                string partner = txtPartner.Value;
                string voicedBy = txtVoicedBy.Value;
                string figureImage = null;
                //string figureImage = _functions.SaveFile(HttpContext.Current, fuFigureImage, fullName);

                // Validate input data
                if (string.IsNullOrEmpty(fullName))
                {
                    // Handle validation error
                    // Display error message or set some validation flag
                }
                // ... Continue validation for other fields

                // Handle file upload for the figure image
                if (fuFigureImage.HasFile)
                {
                    figureImage = _functions.SaveFile(HttpContext.Current, fuFigureImage, fullName);
                    // Note: Make sure the SaveFile method actually saves the file and returns the path
                }

                // Construct SQL query for insertion
                string insertQuery = "INSERT INTO Figures (FullName, StageName, DateOfBirth, DateOfDeath, PlaceOfBirth, PlaceOfDeath, Status, Gender, HairColor, EyeColor, KnownFor, Background, Partner, VoicedBy, FigureImage) VALUES (@FullName, @StageName, @DateOfBirth, @DateOfDeath, @PlaceOfBirth, @PlaceOfDeath, @Status, @Gender, @HairColor, @EyeColor, @KnownFor, @Background, @Partner, @VoicedBy, @FigureImage)";

                // Add parameters to the query
                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    new SqlParameter("@FullName", fullName),
                    new SqlParameter("@StageName", stageName),
                    new SqlParameter("@DateOfBirth", dateOfBirth),
                    new SqlParameter("@DateOfDeath", dateOfDeath),
                    new SqlParameter("@PlaceOfBirth", placeOfBirth),
                    new SqlParameter("@PlaceOfDeath", placeOfDeath),
                    new SqlParameter("@Status", status),
                    new SqlParameter("@Gender", gender),
                    new SqlParameter("@HairColor", hairColor),
                    new SqlParameter("@EyeColor", eyeColor),
                    new SqlParameter("@KnownFor", knownFor),
                    new SqlParameter("@Background", background),
                    new SqlParameter("@Partner", partner),
                    new SqlParameter("@VoicedBy", voicedBy),
                    new SqlParameter("@FigureImage", figureImage),
                };

                // Execute the query
                return _sqlHelper.ExecuteQueryAndGetId(insertQuery, parameters);

            }
            catch (Exception ex)
            {
                // Handle exceptions
                // Log error and display error message
                return 0;
            }
        }

        private void addFigureOccupation(int figureId)
        {
            string occupationId = Occupation.SelectedValue;
            string insertQuery = "INSERT INTO FiguresOccupations (FigureId, OccupationId) VALUES (@FigureId, @OccupationId)";
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@FigureId", figureId),
                new SqlParameter("@OccupationId", occupationId)
            };

            _sqlHelper.ExecuteQuery(insertQuery, parameters);
        }

        [WebMethod]
        public static string AddOccupation(string occupationName)
        {
            // Your logic to add occupation
            // Return a response as a string or JSON
            return null;
        }

        private void addFigureAffiliation(int figureId)
        {
            string affiliationId = Affiliation.SelectedValue;
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