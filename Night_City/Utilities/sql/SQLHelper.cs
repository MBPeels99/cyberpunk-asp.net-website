using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Web;
using System.IO;

namespace Night_City.Utilities.sql
{
    public class SQLHelper
    {
        private readonly string _connectionString;
        public SQLHelper()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        }

        public SqlConnection GetConnection()
        {
            return new SqlConnection(_connectionString);
        }

        public SqlCommand InitializeCommand(string query)
        {
            SqlConnection conn = GetConnection();
            SqlCommand cmd = new SqlCommand(query, conn);
            return cmd;
        }

        public DataTable GetAllDataFromTable(string tableName)
        {
            DataTable dataTable = new DataTable();
            // This is a check to see if the incomming table names are valid and existing tables
            var knownTables = new List<string> { "Occupations", "Affiliations" };

            if (!knownTables.Contains(tableName))
            {
                throw new ArgumentException("Invalid table name.");
            }

            using (SqlConnection conn = GetConnection())
            {
                conn.Open();
                string query = $"SELECT * FROM {tableName}";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dataTable);
                }
            }

            return dataTable;
        }

        public bool InsertDistrictIntoDB(string districtName, string description, string imageMap, string backImage, string imageOne, string imageTwo)
        {
            string query = @"INSERT INTO Districts (DistrictName, Description, ImageMap, BackImage, ImageOne, ImageTwo) 
                             VALUES (@DistrictName, @Description, @ImageMap, @BackImage, @ImageOne, @ImageTwo)";

            try
            {
               using (SqlCommand cmd = InitializeCommand(query))
               {
                    cmd.Parameters.AddWithValue("@DistrictName", districtName);
                    cmd.Parameters.AddWithValue("@Description", description);
                    cmd.Parameters.AddWithValue("@ImageMap", imageMap);
                    cmd.Parameters.AddWithValue("@BackImage", backImage ?? (object)DBNull.Value); // Use DBNull for null values
                    cmd.Parameters.AddWithValue("@ImageOne", imageOne ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@ImageTwo", imageTwo ?? (object)DBNull.Value);

                    cmd.Connection.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();

                    // Return true if any row is affected
                    return rowsAffected > 0;
               }
            }
            catch (Exception)
            {

                // Handle exception
                return false;
            }
        }

        public bool ExecuteQuery(string query, List<SqlParameter> parameters)
        {
            try
            {
                using (SqlCommand cmd = InitializeCommand(query))
                {
                    foreach (var param in parameters)
                    {
                        cmd.Parameters.Add(param);
                    }

                    cmd.Connection.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
            catch (Exception)
            {
                // Handle exception
                return false;
            }
        }

        public int ExecuteQueryAndGetId(string query, List<SqlParameter> parameters)
        {
            try
            {
                string fullQuery = query + "; SELECT SCOPE_IDENTITY();"; // Append the scope identity query

                using (SqlCommand cmd = InitializeCommand(fullQuery)) // Use the full query
                {
                    foreach (var param in parameters)
                    {
                        cmd.Parameters.Add(param);
                    }

                    cmd.Connection.Open();
                    int newId = Convert.ToInt32(cmd.ExecuteScalar());
                    return newId;
                }
            }
            catch (Exception ex)
            {
                // Log the exception for debugging
                Log($"Error in ExecuteQueryAndGetId: {ex.Message}");
                return -1; // Indicates an error
            }
        }

        public int StoreUserInfoAndGetId(string fullName, string email, string phoneNumber, string country, DateTime dateOfBirth, string hashedPassword)
        {
            int userId = 0;

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "INSERT INTO Users (FullName, Email, PhoneNumber, Country, DateOfBirth, Password) VALUES (@FullName, @Email, @PhoneNumber, @Country, @DateOfBirth, @Password); SELECT SCOPE_IDENTITY()";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@FullName", fullName);
                command.Parameters.AddWithValue("@Email", email);
                command.Parameters.AddWithValue("@PhoneNumber", phoneNumber);
                command.Parameters.AddWithValue("@Country", country);
                command.Parameters.AddWithValue("@DateOfBirth", dateOfBirth);
                command.Parameters.AddWithValue("@Password", hashedPassword);

                connection.Open();
                object result = command.ExecuteScalar();
                connection.Close();

                if (result != null && int.TryParse(result.ToString(), out int id))
                {
                    userId = id;
                }
            }

            return userId;
        }

        private void Log(string message)
        {
            string logFile = HttpContext.Current.Server.MapPath("~/Logs/log.txt");
            string logEntry = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}\n";

            File.AppendAllText(logFile, logEntry);
        }

    }
}