using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

public static class DatabaseHelper
{
    public const string ConnectionString = "Data Source=users.db;Version=3;";

    public static void InitializeDatabase()
    {
        try
        {
            bool dbCreated = !File.Exists("users.db");

            if (dbCreated)
            {
                SQLiteConnection.CreateFile("users.db");
                
            }

            using (var conn = new SQLiteConnection(ConnectionString))
            {
                conn.Open();

                
                string sql = @"
                CREATE TABLE IF NOT EXISTS Users (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Username TEXT NOT NULL UNIQUE,
                    Password TEXT NOT NULL,
                    IsAdmin BOOLEAN NOT NULL DEFAULT 0
                );";

                new SQLiteCommand(sql, conn).ExecuteNonQuery();

               
                sql = "SELECT COUNT(*) FROM Users WHERE Username = 'admin'";
                int count = Convert.ToInt32(new SQLiteCommand(sql, conn).ExecuteScalar());

                if (count == 0)
                {
                    sql = @"INSERT INTO Users (Username, Password, IsAdmin) 
                            VALUES ('Admin', 'admin123', 1)";
                    new SQLiteCommand(sql, conn).ExecuteNonQuery();
                    
                }
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Database Error: {ex.Message}");
        }
    }
}