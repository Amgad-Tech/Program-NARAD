using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Program_na_Ryadam
{ 
    public partial class Form2 : Form
    {
        private const string ConnectionString = "Data Source=users.db;Version=3;";
        public Form2()
        {
            this.StartPosition = FormStartPosition.CenterScreen;
            InitializeComponent();
            LoadNonAdminUsers();
        }
        private void LoadNonAdminUsers()
        {
            comboBox1.Items.Clear();

            using (var conn = new SQLiteConnection(ConnectionString))
            {
                conn.Open();
                string sql = "SELECT Username FROM Users WHERE Username != 'admin'";
                using (var cmd = new SQLiteCommand(sql, conn))
                using (SQLiteDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        comboBox1.Items.Add(reader["Username"].ToString());
                    }
                }
            }

            if (comboBox1.Items.Count > 0)
                comboBox1.SelectedIndex = 0;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string username = textBox1.Text.Trim();
            string password = textBox2.Text;

            if (string.IsNullOrWhiteSpace(username))
            {
                MessageBox.Show("Пожалуйста, введите имя пользователя");
                return;
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Пожалуйста, введите пароль");
                return;
            }

            try
            {
                using (var conn = new SQLiteConnection(ConnectionString))
                {
                    conn.Open();
                    string sql = "INSERT INTO Users (Username, Password, IsAdmin) VALUES (@username, @password, 0)";

                    using (var cmd = new SQLiteCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@username", username);
                        cmd.Parameters.AddWithValue("@password", password);
                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Пользователь успешно добавлен!");
                textBox1.Clear();
                textBox2.Clear();
                LoadNonAdminUsers();
            }
            catch (SQLiteException ex)
            {
                if (ex.ResultCode == SQLiteErrorCode.Constraint)
                    MessageBox.Show("Имя пользователя уже существует");
                else
                    MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem == null)
            {
                MessageBox.Show("Пожалуйста, выберите пользователя для удаления");
                return;
            }

            string username = comboBox1.SelectedItem.ToString();

            if (username == "admin")
            {
                MessageBox.Show("Cannot delete admin user");
                return;
            }

            if (MessageBox.Show($"Delete user '{username}'?", "Confirm Delete",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                using (var conn = new SQLiteConnection(ConnectionString))
                {
                    conn.Open();
                    string sql = "DELETE FROM Users WHERE Username = @username";

                    using (var cmd = new SQLiteCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@username", username);
                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Пользователь успешно удален!");
                LoadNonAdminUsers(); 
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            new Form1().Show();
            this.Hide();
        }
    }
}
  
