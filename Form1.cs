using System;
using System.Data.SQLite;
using System.Drawing;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Forms;



namespace Program_na_Ryadam
{
    public partial class Form1 : Form
    {
        public static string Currentuser { get; private set; }
        public static bool IsAdmin {  get; private set; }
        public Form1()
        {
            
            this.StartPosition = FormStartPosition.CenterScreen;
            InitializeComponent();
            StyleForm();
            this.Shown += (s, e) => InitializeApp();
            textBox1.PasswordChar = '*';
            textBox1.UseSystemPasswordChar = false;
            DatabaseHelper.InitializeDatabase();
            LoadUsers();
            
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData  == Keys.Enter)
            {
                button1.PerformClick();
                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }
        private void StyleForm()
        {
            this.Font = new Font("Times New Roman", 12);
            ThemeManager.ApplyTheme(this);

            textBox1.Font = new Font("Times New Roman", 14);
            comboUsername.Font = new Font("Times New Roman", 14);
            checkBox1.Font = new Font("Times New Roman", 12);

            StyleButton(button1);
            StyleButton(button2);
            StyleButton(button3);
        }


        private void StyleButton(Button btn)
        {
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.Font = new Font("Times New Roman", 11, FontStyle.Bold);
            btn.Cursor = Cursors.Hand;
            btn.UseVisualStyleBackColor = false;

            btn.BackColor = ThemeManager.CurrentTheme == AppTheme.Dark
                ? Color.FromArgb(100, 140, 180)
                : Color.Gainsboro;

            btn.ForeColor = ThemeManager.CurrentTheme == AppTheme.Dark
                ? Color.White
                : Color.Black;
        }

        private void InitializeApp()
        {
            try
            {
                DatabaseHelper.InitializeDatabase();
                LoadUsers();

                string dbPath = Path.GetFullPath("users.db");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Initialization Error: {ex.Message}");
            }
        }
        private void LoadUsers()
        {
            comboUsername.Items.Clear(); 

            try
            {
                using (var conn = new SQLiteConnection(DatabaseHelper.ConnectionString))
                {
                    conn.Open();
                    string sql = "SELECT Username FROM Users";
                    using (var cmd = new SQLiteCommand(sql, conn))
                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        int count = 0;
                        while (reader.Read())
                        {
                            string username = reader["Username"].ToString();
                            comboUsername.Items.Add(username);
                            count++;
                        }

                       
                        if (comboUsername.Items.Count > 0)
                            comboUsername.SelectedIndex = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Load Users Error: {ex.Message}");
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            textBox1.PasswordChar = '*';
        }
        private void button1_Click(object sender, EventArgs e)
        {
            Login(false);
        }
        private void button2_Click(object sender, EventArgs e)
        {
            Login(true);
        }
        private void Login(bool isAdminRequset)
        {
            string username = comboUsername.SelectedItem?.ToString();
            string password = textBox1.Text;

            if(AuthenticateUser(username, password))
            {
                if (isAdminRequset && IsAdmin)
                {
                    new Form2().Show();
                    this.Hide();
                }
                else if (!isAdminRequset)
                {
                    new Form3().Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Для доступа администратора требуются учетные данные администратора");
                }
            }
            else
            {
                MessageBox.Show("Неверные учетные данные");
            }
        }
        private bool AuthenticateUser(string username, string password)
        {
            if (string.IsNullOrEmpty(username)) return false;

            using (var conn = new SQLiteConnection(DatabaseHelper.ConnectionString))
            {
                conn.Open();
                string sql = "SELECT IsAdmin FROM Users WHERE Username = @username AND Password = @password";
                using (var cmd = new SQLiteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Parameters.AddWithValue("@password", password);

                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            Currentuser = username;
                            IsAdmin = Convert.ToBoolean(reader["IsAdmin"]);
                            return true;
                        }
                    }
                }
            }
            return false;
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                textBox1.PasswordChar = '\0';
                checkBox1.Text = "Не показать пароль";
                textBox1.Focus();
                textBox1.SelectionStart = textBox1.Text.Length;

                var timer = new System.Windows.Forms.Timer();
                timer.Interval = 50000;
                timer.Tick += (s, args) =>
                {
                    checkBox1.Checked = false;
                    timer.Stop();
                    timer.Dispose();
                };
                timer.Start();
            }
            else
            {
                textBox1.PasswordChar = '*';
                checkBox1.Text = "Показать пароль";
            }
        }
        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void pictureBox1_Click_1(object sender, EventArgs e)
        {
            ThemeManager.ToggleTheme(); // غيّر الوضع
            StyleForm(); // طبّق الستايل الجديد
        }
    }
}
