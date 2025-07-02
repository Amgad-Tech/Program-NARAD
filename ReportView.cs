using System;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Collections.Generic;

namespace Program_na_Ryadam
{
    public partial class ReportView : UserControl
    {
        private ComboBox comboDepartments;
        private DataGridView dgvWorks;
        private Button btnLoad, btnSave;
        private Label lblHeader;
        private Panel signaturePanel;

        private List<TextBox> positionTextBoxes = new List<TextBox>();
        private List<TextBox> bossTextBoxes = new List<TextBox>();
        private List<CheckBox> signatureCheckBoxes = new List<CheckBox>();

        private readonly string[] columnNames = { "№", "Название работы", "В отчёт", "Выходн.", "Ночные", "Премия", "%", "ID" };

        public ReportView()
        {
            InitializeComponent();
            this.Dock = DockStyle.Fill;
            this.AutoScaleMode = AutoScaleMode.Dpi;
            this.Font = new Font("Times New Roman", 11);
            InitializeLayout();
            LoadDepartments();
        }

        private void InitializeLayout()
        {
            this.BackColor = ThemeManager.CurrentTheme == AppTheme.Dark ? Color.FromArgb(60, 60, 76) : Color.WhiteSmoke;

            lblHeader = new Label
            {
                Text = "📄 Вид отчёта",
                Dock = DockStyle.Top,
                Height = 40,
                TextAlign = ContentAlignment.MiddleLeft,
                Font = new Font("Times New Roman", 14, FontStyle.Bold),
                Padding = new Padding(10),
                ForeColor = ThemeManager.CurrentTheme == AppTheme.Dark ? Color.White : Color.Black
            };
            this.Controls.Add(lblHeader);

            comboDepartments = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Width = 300,
                Left = 10,
                Top = 60,
                Font = new Font("Times New Roman", 12),
                FlatStyle = FlatStyle.Flat
            };
            this.Controls.Add(comboDepartments);

            btnLoad = new Button
            {
                Text = "Загрузить",
                Left = 320,
                Top = 58,
                Width = 120,
                Height = 35,
                Font = new Font("Times New Roman", 12, FontStyle.Bold),
                BackColor = Color.FromArgb(100, 140, 180),
                ForeColor = Color.White
            };
            btnLoad.Click += BtnLoad_Click;
            this.Controls.Add(btnLoad);

            btnSave = new Button
            {
                Text = "Сохранить",
                Left = 450,
                Top = 58,
                Width = 130,
                Height = 35,
                Font = new Font("Times New Roman", 12, FontStyle.Bold),
                BackColor = Color.FromArgb(80, 160, 100),
                ForeColor = Color.White
            };
            btnSave.Click += BtnSave_Click;
            this.Controls.Add(btnSave);

            dgvWorks = new DataGridView
            {
                Top = 110,
                Left = 10,
                Width = this.Width - 20,
                Height = this.Height - 300,
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                Font = new Font("Times New Roman", 11),
                BackgroundColor = this.BackColor,
                ForeColor = Color.Black,
                RowHeadersVisible = false,
                AllowUserToAddRows = false
            };
            this.Controls.Add(dgvWorks);

            for (int i = 0; i < columnNames.Length; i++)
            {
                var col = new DataGridViewTextBoxColumn
                {
                    HeaderText = columnNames[i],
                    Name = columnNames[i],
                    ReadOnly = (i == 0 || i == 1 || i == 7)
                };
                dgvWorks.Columns.Add(col);
            }

            string[] daNetOptions = { "Да", "Нет" };
            for (int i = 2; i <= 5; i++)
            {
                var comboCol = new DataGridViewComboBoxColumn
                {
                    HeaderText = columnNames[i],
                    Name = columnNames[i],
                    DataSource = daNetOptions,
                    FlatStyle = FlatStyle.Flat
                };
                dgvWorks.Columns.RemoveAt(i);
                dgvWorks.Columns.Insert(i, comboCol);
            }

            signaturePanel = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 170,
                Padding = new Padding(10),
                BackColor = this.BackColor
            };
            this.Controls.Add(signaturePanel);

            for (int i = 0; i < 3; i++)
            {
                CreateSignatureRow(i + 1, i * 50 + 10);
            }

            dgvWorks.CellBeginEdit += (s, e) =>
            {
                
                if (e.ColumnIndex == dgvWorks.Columns["%"].Index)
                {
                    var premiumValue = dgvWorks.Rows[e.RowIndex].Cells["Премия"].Value?.ToString();
                    e.Cancel = (premiumValue != "Да");
                }
            };

            
            dgvWorks.EditingControlShowing += (s, e) =>
            {
                if (dgvWorks.CurrentCell.ColumnIndex == dgvWorks.Columns["%"].Index)
                {
                    if (e.Control is TextBox tb)
                    {
                        tb.KeyPress += (sender, args) =>
                        {
                            
                            if (!char.IsDigit(args.KeyChar) && args.KeyChar != (char)Keys.Back)
                            {
                                args.Handled = true;
                            }
                        };
                    }
                }
            };

            dgvWorks.CellValueChanged += (s, e) =>
            {
                if (e.ColumnIndex == dgvWorks.Columns["Премия"].Index)
                {
                    bool premiumEnabled = (dgvWorks.Rows[e.RowIndex].Cells["Премия"].Value?.ToString() == "Да");
                    dgvWorks.Rows[e.RowIndex].Cells["%"].ReadOnly = !premiumEnabled;

                    if (!premiumEnabled)
                    {
                        dgvWorks.Rows[e.RowIndex].Cells["%"].Value = "0";
                    }
                }
            };
        }

        private void ReportView_Load(object sender, EventArgs e) { }

        private void PercentageKeyPressHandler(object sender, KeyPressEventArgs e)
        {
           
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back)
            {
                e.Handled = true;
            }
        }

        private void CreateSignatureRow(int index, int top)
        {
            var label = new Label { Text = $"На подпись {index}:", Left = 10, Top = top, Width = 120 };
            var pos = new TextBox { Left = 130, Top = top - 3, Width = 200, Name = $"position{index}" };
            var boss = new TextBox { Left = 340, Top = top - 3, Width = 200, Name = $"boss{index}" };
            var cb = new CheckBox { Left = 550, Top = top - 1, Width = 120, Text = "Не включать", Name = $"chb{index}" };
            cb.CheckedChanged += (s, e) => cb.Text = cb.Checked ? "Включить" : "Не включать";

            signaturePanel.Controls.Add(label);
            signaturePanel.Controls.Add(pos);
            signaturePanel.Controls.Add(boss);
            signaturePanel.Controls.Add(cb);

            positionTextBoxes.Add(pos);
            bossTextBoxes.Add(boss);
            signatureCheckBoxes.Add(cb);

            cb.CheckedChanged += (s, e) =>
            {
                cb.Text = cb.Checked ? "Не включать" : "Включить";
            };
        }

        private void LoadDepartments()
        {
            string dbPath = Path.Combine(Application.StartupPath, "Database", "Database_XN1.mdb");
            if (!File.Exists(dbPath)) { MessageBox.Show("Файл базы данных не найден."); return; }

            string connStr = $"Provider=Microsoft.Jet.OLEDB.4.0;Data Source={dbPath};";

            using (var conn = new OleDbConnection(connStr))
            {
                conn.Open();
                var cmd = new OleDbCommand("SELECT ID, Name_PODR FROM Name_PODRASD ORDER BY Name_PODR", conn);
                var reader = cmd.ExecuteReader();
                var dt = new DataTable();
                dt.Load(reader);
                comboDepartments.DataSource = dt;
                comboDepartments.DisplayMember = "Name_PODR";
                comboDepartments.ValueMember = "ID";
            }
        }

        private void BtnLoad_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(comboDepartments.SelectedValue?.ToString(), out int selectedDeptId)) return;

            string dbPath = Path.Combine(Application.StartupPath, "Database", "Database_XN1.mdb");
            string connStr = $"Provider=Microsoft.Jet.OLEDB.4.0;Data Source={dbPath};";

            using (var conn = new OleDbConnection(connStr))
            {
                conn.Open();
                var sql = "SELECT * FROM Vid_RABOT WHERE Podrasd_N = @id ORDER BY Name_W";
                var cmd = new OleDbCommand(sql, conn);
                cmd.Parameters.AddWithValue("@id", selectedDeptId);

                var adapter = new OleDbDataAdapter(cmd);
                var dt = new DataTable();
                adapter.Fill(dt);

                dgvWorks.Rows.Clear();
                int rowNum = 1;
                foreach (DataRow row in dt.Rows)
                {
                    dgvWorks.Rows.Add(
                        rowNum++,
                        row["Name_W"].ToString(),
                        Convert.ToBoolean(row["bool_OTCH"]) ? "Да" : "Нет",
                        Convert.ToBoolean(row["bool_UIKEND"]) ? "Да" : "Нет",
                        Convert.ToBoolean(row["bool_NIGHT"]) ? "Да" : "Нет",
                        Convert.ToBoolean(row["bool_PREM"]) ? "Да" : "Нет",
                        row["PROCENT"].ToString(),
                        row["ID"].ToString()
                    );
                }

                var signCmd = new OleDbCommand("SELECT * FROM Vid_OTCHET WHERE ID_PODR = @id ORDER BY Numb_por", conn);
                signCmd.Parameters.AddWithValue("@id", selectedDeptId);
                var reader = signCmd.ExecuteReader();
                int i = 0;
                while (reader.Read() && i < 3)
                {
                    positionTextBoxes[i].Text = reader["Name_DOLSN"].ToString();
                    bossTextBoxes[i].Text = reader["Name_BURGAMESTR"].ToString();
                    signatureCheckBoxes[i].Checked = Convert.ToBoolean(reader["bool_vkl"]);
                    i++;
                }
            }

            foreach (DataGridViewRow row in dgvWorks.Rows)
            {
                bool premiumEnabled = (row.Cells["Премия"].Value?.ToString() == "Да");
                row.Cells["%"].ReadOnly = !premiumEnabled;

                if (!premiumEnabled && row.Cells["%"].Value != null)
                {
                    row.Cells["%"].Value = "0";
                }
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dgvWorks.Rows)
            {
                if (row.Cells["Премия"].Value?.ToString() == "Да")
                {
                    if (!int.TryParse(row.Cells["%"].Value?.ToString(), out int percent) || percent < 0)
                    {
                        MessageBox.Show("Недопустимое значение премии!");
                        return;
                    }
                }
            }

            if (!int.TryParse(comboDepartments.SelectedValue?.ToString(), out int selectedDeptId)) return;

            string dbPath = Path.Combine(Application.StartupPath, "Database", "Database_XN1.mdb");
            string connStr = $"Provider=Microsoft.Jet.OLEDB.4.0;Data Source={dbPath};";

            using (var conn = new OleDbConnection(connStr))
            {
                conn.Open();

                foreach (DataGridViewRow row in dgvWorks.Rows)
                {
                    if (row.Cells["ID"].Value == null) continue;

                    var cmd = new OleDbCommand("UPDATE Vid_RABOT SET bool_OTCH = ?, bool_UIKEND = ?, bool_NIGHT = ?, bool_PREM = ?, PROCENT = ? WHERE ID = ?", conn);
                    cmd.Parameters.AddWithValue("?", row.Cells["В отчёт"].Value?.ToString() == "Да");
                    cmd.Parameters.AddWithValue("?", row.Cells["Выходн."].Value?.ToString() == "Да");
                    cmd.Parameters.AddWithValue("?", row.Cells["Ночные"].Value?.ToString() == "Да");
                    cmd.Parameters.AddWithValue("?", row.Cells["Премия"].Value?.ToString() == "Да");

                    int.TryParse(row.Cells["%"].Value?.ToString(), out int percent);
                    cmd.Parameters.AddWithValue("?", percent);
                    cmd.Parameters.AddWithValue("?", row.Cells["ID"].Value);
                    cmd.ExecuteNonQuery();
                }

                new OleDbCommand($"DELETE FROM Vid_OTCHET WHERE ID_PODR = {selectedDeptId}", conn).ExecuteNonQuery();

                for (int i = 0; i < 3; i++)
                {
                    if (!string.IsNullOrWhiteSpace(positionTextBoxes[i].Text))
                    {
                        var insert = new OleDbCommand("INSERT INTO Vid_OTCHET (ID_PODR, Name_DOLSN, Name_BURGAMESTR, Numb_por, bool_vkl) VALUES (?, ?, ?, ?, ?)", conn);
                        insert.Parameters.AddWithValue("?", selectedDeptId);
                        insert.Parameters.AddWithValue("?", positionTextBoxes[i].Text);
                        insert.Parameters.AddWithValue("?", bossTextBoxes[i].Text);
                        insert.Parameters.AddWithValue("?", i + 1);
                        insert.Parameters.AddWithValue("?", signatureCheckBoxes[i].Checked);
                        insert.ExecuteNonQuery();
                    }
                }
            }

            MessageBox.Show("Данные успешно сохранены!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}