using System;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Program_na_Ryadam
{
    public partial class PlantCulture : UserControl
    {
        // Database fields
        private string dbPath = Path.Combine(Application.StartupPath, "Database", "Database_XN1.mdb");
        private string connStr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0}";

        // UI Controls
        private SplitContainer splitContainer;
        private DataGridView dgvCultures, dgvDepartments;
        private TextBox txtCultureName, txtDeptName, txtDeptArea;
        private TextBox txtCultureComment, txtDeptComment;
        private Button btnCultAdd, btnCultEdit, btnCultCancel, btnCultSave;
        private Button btnDeptAdd, btnDeptEdit, btnDeptDelete, btnDeptCancel, btnDeptSave;
        private bool addCultureMode, editCultureMode;
        private bool addDeptMode, editDeptMode;

        public PlantCulture()
        {
            InitializeComponent();
            this.Size = new Size(1100, 500);
            InitializeUI();
            EnsureDatabaseSchema();
            LoadData();
            ApplyTheme();
            WireUpEventHandlers();
            CenterSplitter();
        }

        private void CenterSplitter()
        {
            if (splitContainer != null && splitContainer.Width > 0)
            {
                splitContainer.SplitterDistance = (splitContainer.Width - splitContainer.SplitterWidth) / 2;
            }
        }

        private void EnsureDatabaseSchema()
        {
            string connString = string.Format(connStr, dbPath);
            using (var conn = new OleDbConnection(connString))
            {
                conn.Open();

                // Create Kultura table if not exists
                if (!TableExists(conn, "Kultura"))
                {
                    new OleDbCommand(
                        "CREATE TABLE Kultura (ID AUTOINCREMENT PRIMARY KEY, " +
                        "Name_kultur TEXT(255), Komment MEMO)", conn).ExecuteNonQuery();
                }

                // Create Name_otd table if not exists
                if (!TableExists(conn, "Name_otd"))
                {
                    new OleDbCommand(
                        "CREATE TABLE Name_otd (ID AUTOINCREMENT PRIMARY KEY, " +
                        "Name_otd TEXT(255), KV_M DOUBLE, Komment MEMO)", conn).ExecuteNonQuery();
                }
            }
        }

        private bool TableExists(OleDbConnection conn, string tableName)
        {
            var schema = conn.GetSchema("Tables", new string[] { null, null, tableName, "TABLE" });
            return schema.Rows.Count > 0;
        }

        private void InitializeUI()
        {
            // Main split container
            splitContainer = new SplitContainer()
            {
                Dock = DockStyle.Fill,
                SplitterDistance = 480,
                SplitterWidth = 5
            };
            this.Controls.Add(splitContainer);
            splitContainer.Resize += (sender, e) => CenterSplitter();

            // Left panel - Cultures
            var gbCultures = new GroupBox()
            {
                Text = "Добавление/изменение данных культур",
                Dock = DockStyle.Fill,
                Font = new Font("Times New Roman", 14, FontStyle.Bold)
            };
            splitContainer.Panel1.Controls.Add(gbCultures);

            // Right panel - Departments
            var gbDepartments = new GroupBox()
            {
                Text = "Добавление/изменение данных культур", // Same as original
                Dock = DockStyle.Fill,
                Font = new Font("Times New Roman", 14, FontStyle.Bold)
            };
            splitContainer.Panel2.Controls.Add(gbDepartments);

            InitializeCulturePanel(gbCultures);
            InitializeDepartmentPanel(gbDepartments);
        }

        private void InitializeCulturePanel(GroupBox container)
        {
            var containerPanel = new Panel()
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(0, 10, 0, 0) // Add 10px top padding
            };
            container.Controls.Add(containerPanel);

            // Existing table layout setup
            var tableLayout = new TableLayoutPanel()
            {
                Dock = DockStyle.Fill,
                RowCount = 2,
                ColumnCount = 1
            };
            tableLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 50));
            tableLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 50));
            containerPanel.Controls.Add(tableLayout);

            // Culture DataGridView
            dgvCultures = new DataGridView()
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false,
                RowHeadersVisible = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                Font = new Font("Times New Roman", 12)
            };
            tableLayout.Controls.Add(dgvCultures, 0, 0);

            // Details panel
            var panelDetails = new Panel()
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(10)
            };
            tableLayout.Controls.Add(panelDetails, 0, 1);

            // Culture name
            var lblCulture = new Label()
            {
                Text = "Культура",
                Location = new Point(10, 10),
                Size = new Size(100, 25),
                TextAlign = ContentAlignment.MiddleLeft,
                Font = new Font("Times New Roman", 12)
            };
            txtCultureName = new TextBox()
            {
                Location = new Point(120, 10),
                Size = new Size(330, 30),
                Enabled = false,
                Font = new Font("Times New Roman", 12)
            };
            panelDetails.Controls.AddRange(new Control[] { lblCulture, txtCultureName });

            // Culture comment
            var lblComment = new Label()
            {
                Text = "Подробное описание",
                Location = new Point(10, 50),
                Size = new Size(200, 25),
                TextAlign = ContentAlignment.MiddleLeft,
                Font = new Font("Times New Roman", 12)
            };
            txtCultureComment = new TextBox()
            {
                Location = new Point(10, 80),
                Size = new Size(440, 100),
                Multiline = true,
                ScrollBars = ScrollBars.Vertical,
                Enabled = false,
                Font = new Font("Times New Roman", 12)
            };
            panelDetails.Controls.AddRange(new Control[] { lblComment, txtCultureComment });

            // Button panel
            var panelButtons = new Panel()
            {
                Dock = DockStyle.Bottom,
                Height = 40
            };
            panelDetails.Controls.Add(panelButtons);

            btnCultAdd = new Button()
            {
                Text = "Добавить",
                Size = new Size(85, 35),
                Font = new Font("Times New Roman", 12)
            };
            btnCultEdit = new Button()
            {
                Text = "Изменить",
                Size = new Size(85, 35),
                Font = new Font("Times New Roman", 12)
            };
            btnCultCancel = new Button()
            {
                Text = "Отмена",
                Size = new Size(85, 35),
                Enabled = false,
                Font = new Font("Times New Roman", 12)
            };
            btnCultSave = new Button()
            {
                Text = "Сохранить",
                Size = new Size(85, 35),
                Enabled = false,
                Font = new Font("Times New Roman", 12)
            };

            btnCultAdd.Location = new Point(10, 0);
            btnCultEdit.Location = new Point(105, 0);
            btnCultCancel.Location = new Point(200, 0);
            btnCultSave.Location = new Point(295, 0);

            panelButtons.Controls.AddRange(new Control[] {
                btnCultAdd, btnCultEdit, btnCultCancel, btnCultSave
            });
        }

        private void InitializeDepartmentPanel(GroupBox container)
        {
            var containerPanel = new Panel()
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(0, 10, 0, 0) // Add 10px top padding
            };
            container.Controls.Add(containerPanel);

            // Existing table layout setup
            var tableLayout = new TableLayoutPanel()
            {
                Dock = DockStyle.Fill,
                RowCount = 2,
                ColumnCount = 1
            };
            tableLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 50));
            tableLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 50));
            containerPanel.Controls.Add(tableLayout);

            // Department DataGridView
            dgvDepartments = new DataGridView()
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false,
                RowHeadersVisible = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                Font = new Font("Times New Roman", 12)
            };
            tableLayout.Controls.Add(dgvDepartments, 0, 0);

            // Details panel
            var panelDetails = new Panel()
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(10)
            };
            tableLayout.Controls.Add(panelDetails, 0, 1);

            // Department name
            var lblDept = new Label()
            {
                Text = "Отделение/Зона",
                Location = new Point(10, 10),
                Size = new Size(150, 25),
                TextAlign = ContentAlignment.MiddleLeft,
                Font = new Font("Times New Roman", 12)
            };
            txtDeptName = new TextBox()
            {
                Location = new Point(170, 10),
                Size = new Size(330, 30),
                Enabled = false,
                Font = new Font("Times New Roman", 12)
            };
            panelDetails.Controls.AddRange(new Control[] { lblDept, txtDeptName });

            // Department area
            var lblArea = new Label()
            {
                Text = "Площадь",
                Location = new Point(10, 50),
                Size = new Size(150, 25),
                TextAlign = ContentAlignment.MiddleLeft,
                Font = new Font("Times New Roman", 12)
            };
            txtDeptArea = new TextBox()
            {
                Location = new Point(170, 50),
                Size = new Size(150, 30),
                Enabled = false,
                Font = new Font("Times New Roman", 12)
            };
            panelDetails.Controls.AddRange(new Control[] { lblArea, txtDeptArea });

            // Department comment
            var lblComment = new Label()
            {
                Text = "Подробное описание",
                Location = new Point(10, 90),
                Size = new Size(200, 25),
                TextAlign = ContentAlignment.MiddleLeft,
                Font = new Font("Times New Roman", 12)
            };
            txtDeptComment = new TextBox()
            {
                Location = new Point(10, 120),
                Size = new Size(490, 100),
                Multiline = true,
                ScrollBars = ScrollBars.Vertical,
                Enabled = false,
                Font = new Font("Times New Roman", 12)
            };
            panelDetails.Controls.AddRange(new Control[] { lblComment, txtDeptComment });

            // Button panel
            var panelButtons = new Panel()
            {
                Dock = DockStyle.Bottom,
                Height = 40
            };
            panelDetails.Controls.Add(panelButtons);

            btnDeptDelete = new Button()
            {
                Text = "Удалить",
                Size = new Size(85, 35),
                Font = new Font("Times New Roman", 12)
            };
            btnDeptAdd = new Button()
            {
                Text = "Добавить",
                Size = new Size(85, 35),
                Font = new Font("Times New Roman", 12)
            };
            btnDeptEdit = new Button()
            {
                Text = "Изменить",
                Size = new Size(85, 35),
                Font = new Font("Times New Roman", 12)
            };
            btnDeptCancel = new Button()
            {
                Text = "Отмена",
                Size = new Size(85, 35),
                Enabled = false,
                Font = new Font("Times New Roman", 12)
            };
            btnDeptSave = new Button()
            {
                Text = "Сохранить",
                Size = new Size(85, 35),
                Enabled = false,
                Font = new Font("Times New Roman", 12)
            };

            btnDeptDelete.Location = new Point(10, 0);
            btnDeptAdd.Location = new Point(105, 0);
            btnDeptEdit.Location = new Point(200, 0);
            btnDeptCancel.Location = new Point(295, 0);
            btnDeptSave.Location = new Point(390, 0);

            panelButtons.Controls.AddRange(new Control[] {
                btnDeptDelete, btnDeptAdd, btnDeptEdit, btnDeptCancel, btnDeptSave
            });
        }

        private void WireUpEventHandlers()
        {
            // Culture events
            btnCultAdd.Click += BtnCultAdd_Click;
            btnCultEdit.Click += BtnCultEdit_Click;
            btnCultCancel.Click += BtnCultCancel_Click;
            btnCultSave.Click += BtnCultSave_Click;
            dgvCultures.SelectionChanged += DgvCultures_SelectionChanged;

            // Department events
            btnDeptAdd.Click += BtnDeptAdd_Click;
            btnDeptEdit.Click += BtnDeptEdit_Click;
            btnDeptDelete.Click += BtnDeptDelete_Click;
            btnDeptCancel.Click += BtnDeptCancel_Click;
            btnDeptSave.Click += BtnDeptSave_Click;
            dgvDepartments.SelectionChanged += DgvDepartments_SelectionChanged;
            txtDeptArea.KeyPress += TxtDeptArea_KeyPress;
        }

        private void LoadData()
        {
            LoadCultures();
            LoadDepartments();
        }

        #region Database Operations
        private void LoadCultures()
        {
            string connString = string.Format(connStr, dbPath);
            using (var conn = new OleDbConnection(connString))
            {
                conn.Open();
                var adapter = new OleDbDataAdapter(
                    "SELECT ID, Name_kultur AS [Название культуры] FROM Kultura ORDER BY Name_kultur",
                    conn);

                var dt = new DataTable();
                adapter.Fill(dt);
                dgvCultures.DataSource = dt;

                if (dgvCultures.Columns["ID"] != null)
                {
                    dgvCultures.Columns["ID"].HeaderText = "№";
                    dgvCultures.Columns["ID"].Width = 40;
                }

                if (dgvCultures.Columns["Название культуры"] != null)
                {
                    dgvCultures.Columns["Название культуры"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                }
            }
        }

        private void LoadDepartments()
        {
            string connString = string.Format(connStr, dbPath);
            using (var conn = new OleDbConnection(connString))
            {
                conn.Open();
                var adapter = new OleDbDataAdapter(
                    "SELECT ID, Name_otd AS [Отделение/зона], KV_M AS [Площадь] " +
                    "FROM Name_otd ORDER BY Name_otd",
                    conn);

                var dt = new DataTable();
                adapter.Fill(dt);
                dgvDepartments.DataSource = dt;

                if (dgvDepartments.Columns["ID"] != null)
                {
                    dgvDepartments.Columns["ID"].HeaderText = "№";
                    dgvDepartments.Columns["ID"].Width = 35;
                }

                if (dgvDepartments.Columns["Отделение/зона"] != null)
                {
                    dgvDepartments.Columns["Отделение/зона"].Width = 196;
                }

                if (dgvDepartments.Columns["Площадь"] != null)
                {
                    dgvDepartments.Columns["Площадь"].Width = 120;
                }
            }
        }

        private void SaveCulture()
        {
            if (string.IsNullOrWhiteSpace(txtCultureName.Text))
            {
                MessageBox.Show("Введите название культуры", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string connString = string.Format(connStr, dbPath);
            using (var conn = new OleDbConnection(connString))
            {
                conn.Open();
                string sql = addCultureMode ?
                    "INSERT INTO Kultura (Name_kultur, Komment) VALUES (?, ?)" :
                    "UPDATE Kultura SET Name_kultur = ?, Komment = ? WHERE ID = ?";

                using (var cmd = new OleDbCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("Name_kultur", txtCultureName.Text.Trim());
                    cmd.Parameters.AddWithValue("Komment", txtCultureComment.Text);

                    if (!addCultureMode && dgvCultures.SelectedRows.Count > 0)
                    {
                        var id = dgvCultures.SelectedRows[0].Cells["ID"].Value;
                        cmd.Parameters.AddWithValue("ID", id);
                    }

                    cmd.ExecuteNonQuery();
                }
            }

            ResetCultureForm();
            LoadCultures();
            MessageBox.Show("Данные успешно сохранены!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void SaveDepartment()
        {
            if (string.IsNullOrWhiteSpace(txtDeptName.Text))
            {
                MessageBox.Show("Введите название отделения/зоны", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!decimal.TryParse(txtDeptArea.Text, out decimal area))
            {
                MessageBox.Show("Введите корректное значение площади", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string connString = string.Format(connStr, dbPath);
            using (var conn = new OleDbConnection(connString))
            {
                conn.Open();
                string sql = addDeptMode ?
                    "INSERT INTO Name_otd (Name_otd, KV_M, Komment) VALUES (?, ?, ?)" :
                    "UPDATE Name_otd SET Name_otd = ?, KV_M = ?, Komment = ? WHERE ID = ?";

                using (var cmd = new OleDbCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("Name_otd", txtDeptName.Text.Trim());
                    cmd.Parameters.AddWithValue("KV_M", area);
                    cmd.Parameters.AddWithValue("Komment", txtDeptComment.Text);

                    if (!addDeptMode && dgvDepartments.SelectedRows.Count > 0)
                    {
                        var id = dgvDepartments.SelectedRows[0].Cells["ID"].Value;
                        cmd.Parameters.AddWithValue("ID", id);
                    }

                    cmd.ExecuteNonQuery();
                }
            }

            ResetDepartmentForm();
            LoadDepartments();
            MessageBox.Show("Данные успешно сохранены!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void DeleteDepartment()
        {
            if (dgvDepartments.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите отделение для удаления", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var id = dgvDepartments.SelectedRows[0].Cells["ID"].Value;
            string name = dgvDepartments.SelectedRows[0].Cells["Отделение/зона"].Value.ToString();

            if (MessageBox.Show($"Вы действительно хотите удалить отделение: {name}?",
                "Подтверждение удаления", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                string connString = string.Format(connStr, dbPath);
                using (var conn = new OleDbConnection(connString))
                {
                    conn.Open();
                    using (var cmd = new OleDbCommand("DELETE FROM Name_otd WHERE ID = ?", conn))
                    {
                        cmd.Parameters.AddWithValue("ID", id);
                        cmd.ExecuteNonQuery();
                    }
                }

                LoadDepartments();
                MessageBox.Show("Отделение успешно удалено!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        #endregion

        #region Event Handlers
        // Culture events
        private void BtnCultAdd_Click(object sender, EventArgs e)
        {
            addCultureMode = true;
            editCultureMode = false;
            SetCultureFormState(true);
            txtCultureName.Text = "";
            txtCultureComment.Text = "";
        }

        private void BtnCultEdit_Click(object sender, EventArgs e)
        {
            if (dgvCultures.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите культуру для редактирования", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            addCultureMode = false;
            editCultureMode = true;
            SetCultureFormState(true);
        }

        private void BtnCultCancel_Click(object sender, EventArgs e)
        {
            ResetCultureForm();
        }

        private void BtnCultSave_Click(object sender, EventArgs e)
        {
            SaveCulture();
        }

        private void DgvCultures_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvCultures.SelectedRows.Count > 0 && !editCultureMode)
            {
                txtCultureName.Text = dgvCultures.SelectedRows[0].Cells["Название культуры"].Value.ToString();

                // Load comments from database
                var id = dgvCultures.SelectedRows[0].Cells["ID"].Value;
                string connString = string.Format(connStr, dbPath);
                using (var conn = new OleDbConnection(connString))
                {
                    conn.Open();
                    using (var cmd = new OleDbCommand("SELECT Komment FROM Kultura WHERE ID = ?", conn))
                    {
                        cmd.Parameters.AddWithValue("ID", id);
                        var result = cmd.ExecuteScalar();
                        txtCultureComment.Text = result?.ToString() ?? "";
                    }
                }
            }
        }

        // Department events
        private void BtnDeptAdd_Click(object sender, EventArgs e)
        {
            addDeptMode = true;
            editDeptMode = false;
            SetDepartmentFormState(true);
            txtDeptName.Text = "";
            txtDeptArea.Text = "";
            txtDeptComment.Text = "";
        }

        private void BtnDeptEdit_Click(object sender, EventArgs e)
        {
            if (dgvDepartments.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите отделение для редактирования", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            addDeptMode = false;
            editDeptMode = true;
            SetDepartmentFormState(true);
        }

        private void BtnDeptDelete_Click(object sender, EventArgs e)
        {
            DeleteDepartment();
        }

        private void BtnDeptCancel_Click(object sender, EventArgs e)
        {
            ResetDepartmentForm();
        }

        private void BtnDeptSave_Click(object sender, EventArgs e)
        {
            SaveDepartment();
        }

        private void DgvDepartments_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvDepartments.SelectedRows.Count > 0 && !editDeptMode)
            {
                txtDeptName.Text = dgvDepartments.SelectedRows[0].Cells["Отделение/зона"].Value.ToString();
                txtDeptArea.Text = dgvDepartments.SelectedRows[0].Cells["Площадь"].Value.ToString();

                // Load comments from database
                var id = dgvDepartments.SelectedRows[0].Cells["ID"].Value;
                string connString = string.Format(connStr, dbPath);
                using (var conn = new OleDbConnection(connString))
                {
                    conn.Open();
                    using (var cmd = new OleDbCommand("SELECT Komment FROM Name_otd WHERE ID = ?", conn))
                    {
                        cmd.Parameters.AddWithValue("ID", id);
                        var result = cmd.ExecuteScalar();
                        txtDeptComment.Text = result?.ToString() ?? "";
                    }
                }
            }
        }

        private void TxtDeptArea_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Allow only digits, decimal point, and control characters
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != ',' && e.KeyChar != '.' && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }

            // Replace . with , for decimal separator
            if (e.KeyChar == '.')
            {
                e.KeyChar = ',';
            }
        }
        #endregion

        #region Form State Management
        private void SetCultureFormState(bool editing)
        {
            txtCultureName.Enabled = editing;
            txtCultureComment.Enabled = editing;
            btnCultAdd.Enabled = !editing;
            btnCultEdit.Enabled = !editing && dgvCultures.SelectedRows.Count > 0;
            btnCultCancel.Enabled = editing;
            btnCultSave.Enabled = editing;
            dgvCultures.Enabled = !editing;
        }

        private void SetDepartmentFormState(bool editing)
        {
            txtDeptName.Enabled = editing;
            txtDeptArea.Enabled = editing;
            txtDeptComment.Enabled = editing;
            btnDeptAdd.Enabled = !editing;
            btnDeptEdit.Enabled = !editing && dgvDepartments.SelectedRows.Count > 0;
            btnDeptDelete.Enabled = !editing && dgvDepartments.SelectedRows.Count > 0;
            btnDeptCancel.Enabled = editing;
            btnDeptSave.Enabled = editing;
            dgvDepartments.Enabled = !editing;
        }

        private void ResetCultureForm()
        {
            addCultureMode = false;
            editCultureMode = false;
            SetCultureFormState(false);

            if (dgvCultures.SelectedRows.Count > 0)
            {
                txtCultureName.Text = dgvCultures.SelectedRows[0].Cells["Название культуры"].Value.ToString();

                // Reload comments
                var id = dgvCultures.SelectedRows[0].Cells["ID"].Value;
                string connString = string.Format(connStr, dbPath);
                using (var conn = new OleDbConnection(connString))
                {
                    conn.Open();
                    using (var cmd = new OleDbCommand("SELECT Komment FROM Kultura WHERE ID = ?", conn))
                    {
                        cmd.Parameters.AddWithValue("ID", id);
                        var result = cmd.ExecuteScalar();
                        txtCultureComment.Text = result?.ToString() ?? "";
                    }
                }
            }
        }

        private void ResetDepartmentForm()
        {
            addDeptMode = false;
            editDeptMode = false;
            SetDepartmentFormState(false);

            if (dgvDepartments.SelectedRows.Count > 0)
            {
                txtDeptName.Text = dgvDepartments.SelectedRows[0].Cells["Отделение/зона"].Value.ToString();
                txtDeptArea.Text = dgvDepartments.SelectedRows[0].Cells["Площадь"].Value.ToString();

                // Reload comments
                var id = dgvDepartments.SelectedRows[0].Cells["ID"].Value;
                string connString = string.Format(connStr, dbPath);
                using (var conn = new OleDbConnection(connString))
                {
                    conn.Open();
                    using (var cmd = new OleDbCommand("SELECT Komment FROM Name_otd WHERE ID = ?", conn))
                    {
                        cmd.Parameters.AddWithValue("ID", id);
                        var result = cmd.ExecuteScalar();
                        txtDeptComment.Text = result?.ToString() ?? "";
                    }
                }
            }
        }
        #endregion

        private void ApplyTheme()
        {
            var theme = ThemeManager.CurrentTheme;
            var isDark = theme == AppTheme.Dark;

            // Color scheme
            var backColor = isDark ? Color.FromArgb(45, 45, 48) : Color.White;
            var foreColor = isDark ? Color.White : Color.Black;
            var controlBack = isDark ? Color.FromArgb(60, 60, 65) : Color.White;
            var borderColor = isDark ? Color.FromArgb(90, 90, 90) : SystemColors.ControlDark;
            var headerColor = isDark ? Color.FromArgb(70, 70, 80) : Color.LightGray;

            // Apply to controls
            this.BackColor = isDark ? Color.FromArgb(30, 30, 30) : SystemColors.Control;

            // Apply to DataGridViews
            dgvCultures.BackgroundColor = backColor;
            dgvCultures.DefaultCellStyle.BackColor = backColor;
            dgvCultures.DefaultCellStyle.ForeColor = foreColor;
            dgvCultures.ColumnHeadersDefaultCellStyle.BackColor = headerColor;
            dgvCultures.ColumnHeadersDefaultCellStyle.ForeColor = isDark ? Color.White : Color.Black;
            dgvCultures.RowHeadersDefaultCellStyle.BackColor = headerColor;

            dgvDepartments.BackgroundColor = backColor;
            dgvDepartments.DefaultCellStyle.BackColor = backColor;
            dgvDepartments.DefaultCellStyle.ForeColor = foreColor;
            dgvDepartments.ColumnHeadersDefaultCellStyle.BackColor = headerColor;
            dgvDepartments.ColumnHeadersDefaultCellStyle.ForeColor = isDark ? Color.White : Color.Black;
            dgvDepartments.RowHeadersDefaultCellStyle.BackColor = headerColor;

            // Apply to textboxes
            var textBoxes = new TextBox[] {
                txtCultureName, txtCultureComment,
                txtDeptName, txtDeptArea, txtDeptComment
            };

            foreach (var txt in textBoxes)
            {
                txt.BackColor = controlBack;
                txt.ForeColor = foreColor;
            }

            // Apply to buttons
            var buttons = new Button[] {
                btnCultAdd, btnCultEdit, btnCultCancel, btnCultSave,
                btnDeptAdd, btnDeptEdit, btnDeptDelete, btnDeptCancel, btnDeptSave
            };

            foreach (var btn in buttons)
            {
                btn.BackColor = isDark ? Color.FromArgb(70, 70, 70) : SystemColors.ControlLight;
                btn.ForeColor = foreColor;
                btn.FlatAppearance.BorderColor = borderColor;
            }

            // Apply to labels
            foreach (Control c in this.Controls)
            {
                if (c is Label label)
                {
                    label.ForeColor = foreColor;
                }
            }
        }
    }
}