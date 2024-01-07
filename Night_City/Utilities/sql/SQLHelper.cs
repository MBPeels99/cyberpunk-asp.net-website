using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;

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
                using (SqlCommand cmd = InitializeCommand(query))
                {
                    foreach (var param in parameters)
                    {
                        cmd.Parameters.Add(param);
                    }

                    cmd.Connection.Open();
                    query += "; SELECT SCOPE_IDENTITY();"; // To get the last inserted ID
                    int newId = Convert.ToInt32(cmd.ExecuteScalar());
                    return newId;
                }
            }
            catch (Exception)
            {
                // Handle exception
                return -1; // Indicates an error
            }
        }


    }
}