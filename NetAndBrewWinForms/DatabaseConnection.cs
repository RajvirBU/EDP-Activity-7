using System;
using System.Data;
using MySql.Data.MySqlClient;

namespace NetAndBrewWinForms
{
    // Activity 5 Requirement: "Your MYSQL connection must be a public class."
    public class DatabaseConnection
    {
        private readonly string connectionString;

        public DatabaseConnection()
        {
            // Adjust based on your local MySQL setup (e.g. XAMPP default)
            connectionString = "Server=localhost;Database=coffee_shop_db;Uid=root;Pwd=;";
        }

        public MySqlConnection GetConnection()
        {
            return new MySqlConnection(connectionString);
        }

        // Helper for queries that return a DataTable (e.g., SELECT)
        public DataTable ExecuteQuery(string query, MySqlParameter[] parameters = null)
        {
            using (var connection = GetConnection())
            {
                using (var command = new MySqlCommand(query, connection))
                {
                    if (parameters != null)
                    {
                        command.Parameters.AddRange(parameters);
                    }
                    var adapter = new MySqlDataAdapter(command);
                    var dt = new DataTable();
                    adapter.Fill(dt);
                    return dt;
                }
            }
        }

        // Helper for Non-Query statements (e.g., INSERT, UPDATE, DELETE)
        public int ExecuteNonQuery(string query, MySqlParameter[] parameters = null)
        {
            using (var connection = GetConnection())
            {
                using (var command = new MySqlCommand(query, connection))
                {
                    if (parameters != null)
                    {
                        command.Parameters.AddRange(parameters);
                    }
                    connection.Open();
                    return command.ExecuteNonQuery();
                }
            }
        }
    }
}
