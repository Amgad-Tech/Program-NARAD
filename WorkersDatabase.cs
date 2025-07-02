using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace Program_na_Ryadam
{
    public partial class WorkersDatabase : UserControl
    {
        // Database connection
        private string dbPath = Path.Combine(Application.StartupPath, "Database", "Database_XN1.mdb");
        private string connStr;
        private OleDbConnection connection;

        // Forms
        private F_add_w_all addEditWorkerForm;
        private F_O_R reportViewerForm;
        private list_view_otd departmentReportForm;
        private list_view_SCHEMA cultureReportForm;
        private Unit15 workReportForm;

        // UI Controls
        private DateTimePicker dtpStart;
        private DateTimePicker dtpEnd;
        private DataGridView workersDataGridView;
        private ComboBox cbDepartment;
        private ComboBox cbTeam;
        private ComboBox cbPosition;
        private CheckBox chkHideFired;
        private ComboBox cbReportType;
        private Button btnAddWorker;
        private Button btnUpdate;
        private Button btnEditWorker;
        private Button btnFireWorker;
        private Button btnTabel;
        private Button btnAccounting;
        private Button btnGenerateReport;
        private Button btnViewWithoutReport;
        private Button btnReportSettings;
        private Button btnTotalHours;
        private Button btnDepartmentReport;
        private Button btnCultureReport;
        private Button btnGeneralReport;
        private Button btnWorkReport;
        private CheckBox chkReportWithHours;
        private Panel panelLeft;
        private Panel panelRight;
        private Panel buttonPanel;

        // Data structures
        private DataTable workersTable = new DataTable();
        private DataTable departmentsTable = new DataTable();
        private DataView workersView;
        private List<int> departmentIds = new List<int>();

        public WorkersDatabase()
        {
            InitializeComponent();
            connStr = $"Provider=Microsoft.Jet.OLEDB.4.0;Data Source={dbPath}";
            this.Padding = new Padding(0, 0, 0, 0);
            this.Load += WorkersDatabase_Load;
        }

        private void WorkersDatabase_Load(object sender, EventArgs e)
        {
            SetupUI();
            InitializeDatabase();
        }

        private void InitializeDatabase()
        {
            try
            {
                connection = new OleDbConnection(connStr);
                connection.Open();
                LoadDepartments();
                LoadWorkers();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Database error: {ex.Message}");
            }
        }

        private void SetupUI()
        {
            panelLeft = new Panel { Dock = DockStyle.Left, Width = this.Width - 290 };
            panelRight = new Panel { Dock = DockStyle.Right, Width = 420 };
            this.Controls.Add(panelLeft);
            this.Controls.Add(panelRight);
            panelLeft.AutoScroll = true;
            panelRight.AutoScroll = true;

            // Worker DataGridView
            workersDataGridView = new DataGridView
            {
                Dock = DockStyle.Fill,
                AutoGenerateColumns = false,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                RowHeadersVisible = false,
                Font = new Font("Consolas", 12)
            };

            // Configure columns
            workersDataGridView.Columns.AddRange(
                new DataGridViewColumn[]
                {
                    new DataGridViewTextBoxColumn { Name = "Fam", HeaderText = "Фамилия", DataPropertyName = "Fam", Width = 150 },
                    new DataGridViewTextBoxColumn { Name = "Imj", HeaderText = "Имя", DataPropertyName = "Imj", Width = 120 },
                    new DataGridViewTextBoxColumn { Name = "Otc", HeaderText = "Отчество", DataPropertyName = "Otc", Width = 150 },
                    new DataGridViewTextBoxColumn {
                    Name = "Pol",
                    HeaderText = "Пол",
                    DataPropertyName = "Pol",
                    Width = 70,
                    ValueType = typeof(bool),
                    DefaultCellStyle = new DataGridViewCellStyle {
                    NullValue = "Не указан",
                    FormatProvider = CultureInfo.CurrentCulture
                    }
                    },
                    new DataGridViewTextBoxColumn { Name = "Podr_str", HeaderText = "Подразделение", DataPropertyName = "Podr_str", Width = 200 },
                    new DataGridViewTextBoxColumn { Name = "Brigada_str", HeaderText = "Бригада", DataPropertyName = "Brigada_str", Width = 150 },
                    new DataGridViewTextBoxColumn { Name = "Dolsn_str", HeaderText = "Должность", DataPropertyName = "Dolsn_str", Width = 150 },
                    new DataGridViewTextBoxColumn { Name = "Mesto_sitel", HeaderText = "Место жит.", DataPropertyName = "Mesto_sitel", Width = 100 },
                    new DataGridViewCheckBoxColumn { Name = "Fired", HeaderText = "Работает", DataPropertyName = "Fired", Width = 70 },
                    new DataGridViewTextBoxColumn { Name = "ID", DataPropertyName = "ID", Visible = false }
                }
            );

            // Configure headers
            workersDataGridView.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
            {
                Font = new Font("Consolas", 12, FontStyle.Bold)
            };

            panelLeft.Controls.Add(workersDataGridView);

            // Action buttons at bottom
            buttonPanel = new Panel { Dock = DockStyle.Bottom, Height = 60 };
            panelLeft.Controls.Add(buttonPanel);

            btnUpdate = new Button
            {
                Text = "Обновить",
                Dock = DockStyle.Right,
                Width = 120,
                Height = 50,
                Font = new Font("Times New Roman", 14),
                Margin = new Padding(0, 0, 10, 0)
            };
            btnUpdate.Click += btnUpdate_Click;

            btnAddWorker = new Button
            {
                Text = "Добавить",
                Dock = DockStyle.Right,
                Width = 120,
                Height = 50,
                Font = new Font("Times New Roman", 14),
                Margin = new Padding(10)
            };
            btnEditWorker = new Button
            {
                Text = "Изменить",
                Dock = DockStyle.Right,
                Width = 120,
                Height = 50,
                Font = new Font("Times New Roman", 14),
                Margin = new Padding(0, 10, 0, 10)
            };
            btnFireWorker = new Button
            {
                Text = "Уволить",
                Dock = DockStyle.Right,
                Width = 120,
                Height = 50,
                Font = new Font("Times New Roman", 14),
                Margin = new Padding(10)
            };

            btnAddWorker.Click += btnAddWorker_Click;
            btnEditWorker.Click += btnEditWorker_Click;
            btnFireWorker.Click += btnFireWorker_Click;
            workersDataGridView.CellFormatting += workersDataGridView_CellFormatting;

            buttonPanel.Controls.AddRange(new Control[] {
                btnAddWorker,
                btnEditWorker,
                btnUpdate,
                btnFireWorker
            });

            // Worker Selection GroupBox
            var gbWorkerSelection = new GroupBox
            {
                Text = "Выбор работников",
                Font = new Font("Consolas", 12, FontStyle.Bold),
                Dock = DockStyle.Top,
                Height = 450
            };
            panelRight.Controls.Add(gbWorkerSelection);

            // Department selection
            var lblDepartment = new Label
            {
                Text = "Подразделение:",
                Location = new Point(20, 40),
                AutoSize = true,
                Font = new Font("Consolas", 12)
            };
            cbDepartment = new ComboBox
            {
                Location = new Point(20, 70),
                Width = 350,
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Consolas", 12)
            };
            cbDepartment.SelectedIndexChanged += cbDepartment_SelectedIndexChanged;
            gbWorkerSelection.Controls.Add(lblDepartment);
            gbWorkerSelection.Controls.Add(cbDepartment);

            // Team selection
            var lblTeam = new Label
            {
                Text = "Бригада:",
                Location = new Point(20, 120),
                AutoSize = true,
                Font = new Font("Consolas", 12)
            };
            cbTeam = new ComboBox
            {
                Location = new Point(20, 150),
                Width = 350,
                Enabled = false,
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Consolas", 12)
            };
            cbTeam.SelectedIndexChanged += cbTeam_SelectedIndexChanged;
            gbWorkerSelection.Controls.Add(lblTeam);
            gbWorkerSelection.Controls.Add(cbTeam);

            // Position selection
            var lblPosition = new Label
            {
                Text = "Должность:",
                Location = new Point(20, 190),
                AutoSize = true,
                Font = new Font("Consolas", 12)
            };
            cbPosition = new ComboBox
            {
                Location = new Point(20, 220),
                Width = 350,
                Enabled = false,
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Consolas", 12)
            };
            cbPosition.SelectedIndexChanged += cbPosition_SelectedIndexChanged;
            gbWorkerSelection.Controls.Add(lblPosition);
            gbWorkerSelection.Controls.Add(cbPosition);

            // Hide fired workers checkbox
            chkHideFired = new CheckBox
            {
                Text = "Не показывать уволенных",
                Location = new Point(20, 260),
                AutoSize = true,
                Font = new Font("Consolas", 12)
            };
            chkHideFired.CheckedChanged += chkHideFired_CheckedChanged;
            gbWorkerSelection.Controls.Add(chkHideFired);

            // Report Preparation GroupBox
            var gbReportPreparation = new GroupBox
            {
                Text = "Подготовка отчёта",
                Font = new Font("Consolas", 12, FontStyle.Bold),
                Dock = DockStyle.Top,
                Height = 450
            };
            panelRight.Controls.Add(gbReportPreparation);

            // Date range
            var lblStart = new Label
            {
                Text = "Начало:",
                Location = new Point(20, 40),
                AutoSize = true,
                Font = new Font("Consolas", 12)
            };
            var lblEnd = new Label
            {
                Text = "Конец:",
                Location = new Point(200, 40),
                AutoSize = true,
                Font = new Font("Consolas", 12)
            };
            dtpStart = new DateTimePicker
            {
                Location = new Point(20, 70),
                Width = 150,
                Format = DateTimePickerFormat.Custom,
                CustomFormat = "dd.MM.yyyy",
                Font = new Font("Consolas", 12)
            };
            dtpEnd = new DateTimePicker
            {
                Location = new Point(200, 70),
                Width = 150,
                Format = DateTimePickerFormat.Custom,
                CustomFormat = "dd.MM.yyyy",
                Font = new Font("Consolas", 12)
            };
            gbReportPreparation.Controls.Add(lblStart);
            gbReportPreparation.Controls.Add(lblEnd);
            gbReportPreparation.Controls.Add(dtpStart);
            gbReportPreparation.Controls.Add(dtpEnd);

            // Report type
            var lblReportType = new Label
            {
                Text = "Вариант отчёта:",
                Location = new Point(20, 120),
                AutoSize = true,
                Font = new Font("Consolas", 12)
            };
            cbReportType = new ComboBox
            {
                Location = new Point(20, 150),
                Width = 350,
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Consolas", 12)
            };
            cbReportType.Items.AddRange(new[] { "Выделенного работника", "На всех" });
            cbReportType.SelectedIndex = 0;
            gbReportPreparation.Controls.Add(lblReportType);
            gbReportPreparation.Controls.Add(cbReportType);

            // Report options
            chkReportWithHours = new CheckBox
            {
                Text = "Отчёт с часами",
                Location = new Point(20, 190),
                AutoSize = true,
                Font = new Font("Consolas", 12)
            };
            gbReportPreparation.Controls.Add(chkReportWithHours);

            // Report buttons
            btnGenerateReport = new Button
            {
                Text = "Наряд в EXCEL",
                Location = new Point(20, 230),
                Width = 350,
                Height = 35,
                Font = new Font("Consolas", 12)
            };
            btnViewWithoutReport = new Button
            {
                Text = "Просмотр без отчета в EXCEL",
                Location = new Point(20, 275),
                Width = 350,
                Height = 35,
                Font = new Font("Consolas", 12)
            };
            btnReportSettings = new Button
            {
                Text = "Настройки выбора работ в отчете",
                Location = new Point(20, 320),
                Width = 350,
                Height = 35,
                Font = new Font("Consolas", 12)
            };
            btnTotalHours = new Button
            {
                Text = "Общие часы",
                Location = new Point(20, 365),
                Width = 350,
                Height = 35,
                Font = new Font("Consolas", 12)
            };

            btnGenerateReport.Click += btnGenerateReport_Click;
            btnViewWithoutReport.Click += btnViewWithoutReport_Click;
            btnReportSettings.Click += btnReportSettings_Click;
            btnTotalHours.Click += btnTotalHours_Click;

            gbReportPreparation.Controls.AddRange(new Control[] {
                btnGenerateReport, btnViewWithoutReport, btnReportSettings, btnTotalHours
            });

            // Additional Reports GroupBox
            var gbAdditionalReports = new GroupBox
            {
                Text = "Дополнительные отчёты",
                Dock = DockStyle.Top,
                Height = 220
            };
            panelRight.Controls.Add(gbAdditionalReports);

            // Additional report buttons
            btnDepartmentReport = new Button
            {
                Text = "Отчёт по отделению",
                Location = new Point(20, 40),
                Width = 350,
                Height = 35,
                Font = new Font("Consolas", 12)
            };
            btnCultureReport = new Button
            {
                Text = "Отчёт по культуре",
                Location = new Point(20, 85),
                Width = 350,
                Height = 35,
                Font = new Font("Consolas", 12)
            };
            btnGeneralReport = new Button
            {
                Text = "Общий наряд",
                Location = new Point(20, 130),
                Width = 350,
                Height = 35,
                Font = new Font("Consolas", 12)
            };
            btnWorkReport = new Button
            {
                Text = "Отчёт по работе",
                Location = new Point(20, 175),
                Width = 350,
                Height = 35,
                Font = new Font("Consolas", 12)
            };

            // Add buttons to group box
            gbAdditionalReports.Controls.AddRange(new Control[] {
                btnDepartmentReport, btnCultureReport, btnGeneralReport, btnWorkReport
            });

            // Add Tabel and Accounting buttons to LEFT PANEL
            btnTabel = new Button
            {
                Text = "Табель рабочего времени",
                Dock = DockStyle.Bottom,
                Height = 35,
                Font = new Font("Consolas", 12),
                Margin = new Padding(5, 5, 5, 0)
            };
            btnAccounting = new Button
            {
                Text = "Для бухгалтерии",
                Dock = DockStyle.Bottom,
                Height = 35,
                Font = new Font("Consolas", 12),
                Margin = new Padding(5, 5, 5, 5)
            };

            btnTabel.Click += btnTabel_Click;
            btnAccounting.Click += btnAccounting_Click;

            panelLeft.Controls.Add(btnTabel);
            panelLeft.Controls.Add(btnAccounting);

            // Initialize date range
            int currentDay = DateTime.Now.Day;
            if (currentDay >= 15)
            {
                dtpStart.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                dtpEnd.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month,
                    DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month));
            }
            else
            {
                DateTime prevMonth = DateTime.Now.AddMonths(-1);
                dtpStart.Value = new DateTime(prevMonth.Year, prevMonth.Month, 1);
                dtpEnd.Value = new DateTime(prevMonth.Year, prevMonth.Month,
                    DateTime.DaysInMonth(prevMonth.Year, prevMonth.Month));
            }
        }

        private void LoadDepartments()
        {
            using (OleDbCommand cmd = new OleDbCommand(
                "SELECT ID, Name_PODR FROM Name_PODRASD", connection))
            {
                departmentsTable.Clear();
                using (OleDbDataReader reader = cmd.ExecuteReader())
                {
                    departmentsTable.Load(reader);
                }

                cbDepartment.Items.Clear();
                cbDepartment.Items.Add("Все работники");

                departmentIds.Clear();
                departmentIds.Add(-1); // For "All workers" option

                foreach (DataRow row in departmentsTable.Rows)
                {
                    departmentIds.Add(Convert.ToInt32(row["ID"]));
                    cbDepartment.Items.Add(row["Name_PODR"]);
                }

                if (cbDepartment.Items.Count > 0)
                    cbDepartment.SelectedIndex = 0;
            }
        }

        private void LoadWorkers()
        {
            using (OleDbCommand cmd = new OleDbCommand(
                "SELECT ID, Fam, Imj, Otc, Pol, Podr, Podr_str, Brigada_str, Dolsn_str, Mesto_sitel, Fired FROM BD_WORKING_ALL", connection))
            {
                workersTable.Clear();
                using (OleDbDataReader reader = cmd.ExecuteReader())
                {
                    workersTable.Load(reader);
                }

                DataTable newTable = new DataTable();
                using (OleDbDataReader reader = cmd.ExecuteReader())
                {
                    newTable.Load(reader);
                }

                // Replace the existing workersTable with the new data
                workersTable = newTable;

                workersView = new DataView(workersTable);
                workersDataGridView.DataSource = workersView;
            }
        }

        private void UpdateFilter()
        {
            if (workersView == null) return;

            string filter = "";
            if (cbDepartment.SelectedIndex > 0)
            {
                int deptId = departmentIds[cbDepartment.SelectedIndex];
                filter = $"Podr = {deptId}";

                if (cbTeam.SelectedIndex > 0 && !string.IsNullOrEmpty(cbTeam.Text) && cbTeam.Text != "Все бригады")
                {
                    string escapedTeam = cbTeam.Text.Replace("'", "''");
                    filter += $" AND Brigada_str = '{escapedTeam}'";
                }

                if (cbPosition.SelectedIndex > 0 && !string.IsNullOrEmpty(cbPosition.Text) && cbPosition.Text != "Все должности")
                {
                    string escapedPosition = cbPosition.Text.Replace("'", "''");
                    filter += $" AND Dolsn_str = '{escapedPosition}'";
                }
            }

            if (chkHideFired.Checked)
            {
                if (!string.IsNullOrEmpty(filter)) filter += " AND ";
                filter += "Fired = false";
            }

            workersView.RowFilter = filter;
        }

        private void cbDepartment_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbTeam.Items.Clear();
            cbPosition.Items.Clear();
            cbTeam.Enabled = false;
            cbPosition.Enabled = false;

            if (cbDepartment.SelectedIndex > 0)
            {
                int deptId = departmentIds[cbDepartment.SelectedIndex];

                // Load teams
                cbTeam.Items.Add("Все бригады");
                using (OleDbCommand cmd = new OleDbCommand(
                    "SELECT DISTINCT Name_Brigad FROM Name_BRIGADA WHERE ID_PODR = @ID_PODR",
                    connection))
                {
                    cmd.Parameters.AddWithValue("@ID_PODR", deptId);
                    using (OleDbDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            cbTeam.Items.Add(reader["Name_Brigad"].ToString());
                        }
                    }
                }
                cbTeam.SelectedIndex = 0;
                cbTeam.Enabled = true;

                // Load positions
                cbPosition.Items.Add("Все должности");
                using (OleDbCommand cmd = new OleDbCommand(
                    "SELECT DISTINCT Name_DOLSN FROM Dolsn_PODR WHERE ID_PODR = @ID_PODR",
                    connection))
                {
                    cmd.Parameters.AddWithValue("@ID_PODR", deptId);
                    using (OleDbDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            cbPosition.Items.Add(reader["Name_DOLSN"].ToString());
                        }
                    }
                }
                cbPosition.SelectedIndex = 0;
                cbPosition.Enabled = true;
            }

            UpdateFilter();
        }

        #region Event Handlers
        private void cbTeam_SelectedIndexChanged(object sender, EventArgs e) => UpdateFilter();
        private void cbPosition_SelectedIndexChanged(object sender, EventArgs e) => UpdateFilter();
        private void chkHideFired_CheckedChanged(object sender, EventArgs e) => UpdateFilter();

        private void btnTabel_Click(object sender, EventArgs e)
        {
            try
            {
                using (var excelLib = new Excel_Library())
                {
                    excelLib.GenerateTabelWorkingReport(dtpStart.Value, dtpEnd.Value);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка генерации табеля: {ex.Message}");
            }
        }

        private void btnAccounting_Click(object sender, EventArgs e)
        {
            try
            {
                using (var excelLib = new Excel_Library())
                {
                    excelLib.GenerateAccountingReport(dtpStart.Value, dtpEnd.Value);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка генерации бухгалтерского отчёта: {ex.Message}");
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            LoadWorkers();
            MessageBox.Show("Данные работников обновлены!", "Обновление",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private void btnAddWorker_Click(object sender, EventArgs e)
        {
            addEditWorkerForm = new F_add_w_all(dbPath);
            addEditWorkerForm.SetMode(false); // Add mode
            if (addEditWorkerForm.ShowDialog() == DialogResult.OK)
            {
                LoadWorkers(); // Refresh data
            }
        }

        private void btnEditWorker_Click(object sender, EventArgs e)
        {
            if (workersDataGridView.CurrentRow == null) return;

            int workerId = (int)workersDataGridView.CurrentRow.Cells["ID"].Value;
            if (Convert.ToBoolean(workersDataGridView.CurrentRow.Cells["Fired"].Value))
            {
                MessageBox.Show("Работник уволен!");
                return;
            }

            addEditWorkerForm = new F_add_w_all(dbPath);
            addEditWorkerForm.SetMode(true, workerId); // Edit mode
            if (addEditWorkerForm.ShowDialog() == DialogResult.OK)
            {
                LoadWorkers(); // Refresh data
            }
        }

        private void btnFireWorker_Click(object sender, EventArgs e)
        {
            if (workersDataGridView.CurrentRow == null) return;

            int workerId = (int)workersDataGridView.CurrentRow.Cells["ID"].Value;
            string workerName = $"{workersDataGridView.CurrentRow.Cells["Fam"].Value} " +
                               $"{workersDataGridView.CurrentRow.Cells["Imj"].Value} " +
                               $"{workersDataGridView.CurrentRow.Cells["Otc"].Value}";

            if (MessageBox.Show($"Вы действительно хотите уволить работника\n{workerName}?",
                "Подтверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                using (OleDbCommand cmd = new OleDbCommand(
                    "UPDATE BD_WORKING_ALL SET Fired = true WHERE ID = @id", connection))
                {
                    cmd.Parameters.AddWithValue("@id", workerId);
                    cmd.ExecuteNonQuery();
                }
                LoadWorkers(); // Refresh data
            }
        }

        private void btnGenerateReport_Click(object sender, EventArgs e)
        {
            try
            {
                // Get current filter settings
                string department = cbDepartment.SelectedIndex > 0 ? cbDepartment.Text : "Все подразделения";
                string team = cbTeam.SelectedIndex > 0 ? cbTeam.Text : "Все бригады";

                // Build period label
                string periodLabel = $" от {dtpStart.Value:dd.MM.yyyy} по {dtpEnd.Value:dd.MM.yyyy}";

                // Prepare report data
                var reportData = new TeamReportData
                {
                    Department = department,
                    Team = team
                };

                // Collect workers and their work items
                foreach (DataRowView row in workersView)
                {
                    var worker = new WorkerReportData
                    {
                        LastName = row["Fam"].ToString(),
                        FirstName = row["Imj"].ToString(),
                        MiddleName = row["Otc"].ToString(),
                        Department = row["Podr_str"].ToString(),
                        Team = row["Brigada_str"].ToString()
                    };

                    // Get work items for this worker
                    using (OleDbCommand cmd = new OleDbCommand(
                        "SELECT w.WorkName, w.Hours, w.Quantity, w.Rate " +
                        "FROM WorkRecords w " +
                        "WHERE w.WorkerID = @id AND w.WorkDate BETWEEN @start AND @end",
                        connection))
                    {
                        cmd.Parameters.AddWithValue("@id", row["ID"]);
                        cmd.Parameters.AddWithValue("@start", dtpStart.Value);
                        cmd.Parameters.AddWithValue("@end", dtpEnd.Value);

                        using (OleDbDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                worker.WorkItems.Add(new WorkItem
                                {
                                    WorkName = reader["WorkName"].ToString(),
                                    Hours = Convert.ToSingle(reader["Hours"]),
                                    Quantity = Convert.ToInt32(reader["Quantity"]),
                                    Rate = Convert.ToSingle(reader["Rate"])
                                });
                            }
                        }
                    }
                    reportData.Workers.Add(worker);
                }

                // Generate report
                using (var excelLib = new Excel_Library())
                {
                    if (chkReportWithHours.Checked)
                    {
                        excelLib.GenerateNaradRabochimReport(reportData, periodLabel, true);
                    }
                    else
                    {
                        excelLib.GenerateNaradRabochimNotHourReport(reportData, periodLabel);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка генерации отчёта: {ex.Message}");
            }
        }

        private void workersDataGridView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (workersDataGridView.Columns[e.ColumnIndex].Name == "Pol" && e.Value != null)
            {
                e.Value = (bool)e.Value ? "Муж" : "Жен";
                e.FormattingApplied = true;
            }
        }

        private void btnViewWithoutReport_Click(object sender, EventArgs e)
        {
            if (cbReportType.SelectedIndex != 0)
            {
                MessageBox.Show("Данный вид отчёта возможен только для конкретного работника!");
                return;
            }

            if (workersDataGridView.CurrentRow == null)
            {
                MessageBox.Show("Выберите работника!");
                return;
            }

            int workerId = (int)workersDataGridView.CurrentRow.Cells["ID"].Value;
            string workerName = $"{workersDataGridView.CurrentRow.Cells["Fam"].Value} " +
                                $"{workersDataGridView.CurrentRow.Cells["Imj"].Value} " +
                                $"{workersDataGridView.CurrentRow.Cells["Otc"].Value}";

            // Create and show report form without Excel
            //reportViewerForm = new F_O_R(dbPath);
            //reportViewerForm.SetReportData(workerId, workerName, dtpStart.Value, dtpEnd.Value, false);
            reportViewerForm.Show();
        }

        private void btnReportSettings_Click(object sender, EventArgs e)
        {
            // Show report settings form
            MessageBox.Show("Настройки выбора работ в отчете будут реализованы позже", "Настройки",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnTotalHours_Click(object sender, EventArgs e)
        {
            if (cbReportType.SelectedIndex != 0)
            {
                MessageBox.Show("Данный вид отчёта возможен только для конкретного работника!");
                return;
            }

            if (workersDataGridView.CurrentRow == null)
            {
                MessageBox.Show("Выберите работника!");
                return;
            }

            int workerId = (int)workersDataGridView.CurrentRow.Cells["ID"].Value;
            string workerName = $"{workersDataGridView.CurrentRow.Cells["Fam"].Value} " +
                                $"{workersDataGridView.CurrentRow.Cells["Imj"].Value} " +
                                $"{workersDataGridView.CurrentRow.Cells["Otc"].Value}";

            // Show total hours report
            //reportViewerForm = new F_O_R(dbPath);
            //reportViewerForm.ShowTotalHours(workerId, workerName, dtpStart.Value, dtpEnd.Value);
            reportViewerForm.Show();
        }

        private void btnDepartmentReport_Click(object sender, EventArgs e)
        {
            departmentReportForm = new list_view_otd();
            departmentReportForm.Show();
        }

        private void btnCultureReport_Click(object sender, EventArgs e)
        {
            cultureReportForm = new list_view_SCHEMA();
            cultureReportForm.Show();
        }

        private void btnGeneralReport_Click(object sender, EventArgs e)
        {
            // Create and show general report
            //reportViewerForm = new F_O_R(dbPath);
            //reportViewerForm.ShowGeneralReport(dtpStart.Value, dtpEnd.Value);
            reportViewerForm.Show();
        }

        private void btnWorkReport_Click(object sender, EventArgs e)
        {
            workReportForm = new Unit15();
            workReportForm.Show();
        }
        #endregion

        private TeamReportData GetAllWorkersReportData(DateTime startDate, DateTime endDate)
        {
            var reportData = new TeamReportData();

            // Get department and team from filter
            reportData.Department = cbDepartment.SelectedIndex > 0 ? cbDepartment.Text : "Все подразделения";
            reportData.Team = cbTeam.SelectedIndex > 0 ? cbTeam.Text : "Все бригады";

            // Get all workers in current filter
            foreach (DataRowView row in workersView)
            {
                var worker = new WorkerReportData
                {
                    LastName = row["Fam"].ToString(),
                    FirstName = row["Imj"].ToString(),
                    MiddleName = row["Otc"].ToString(),
                    Department = row["Podr_str"].ToString(),
                    Team = row["Brigada_str"].ToString()
                };

                // Get work items for this worker
                using (OleDbCommand cmd = new OleDbCommand(
                    "SELECT WorkName, Hours, Quantity, Rate " +
                    "FROM WorkRecords WHERE WorkerID = @id AND WorkDate BETWEEN @start AND @end",
                    connection))
                {
                    cmd.Parameters.AddWithValue("@id", row["ID"]);
                    cmd.Parameters.AddWithValue("@start", startDate);
                    cmd.Parameters.AddWithValue("@end", endDate);

                    using (OleDbDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            worker.WorkItems.Add(new WorkItem
                            {
                                WorkName = reader["WorkName"].ToString(),
                                Hours = Convert.ToSingle(reader["Hours"]),
                                Quantity = Convert.ToInt32(reader["Quantity"]),
                                Rate = Convert.ToSingle(reader["Rate"])
                            });
                        }
                    }
                }

                reportData.Workers.Add(worker);
            }

            return reportData;
        }

        #region Report Data Classes
        public class WorkerReportData
        {
            public string LastName { get; set; }
            public string FirstName { get; set; }
            public string MiddleName { get; set; }
            public string Department { get; set; }
            public string Team { get; set; }
            public List<WorkItem> WorkItems { get; set; } = new List<WorkItem>();
        }

        public class TeamReportData
        {
            public string Department { get; set; }
            public string Team { get; set; }
            public List<WorkerReportData> Workers { get; set; } = new List<WorkerReportData>();
        }

        public class WorkItem
        {
            public string WorkName { get; set; }
            public float Hours { get; set; }
            public int Quantity { get; set; }
            public float Rate { get; set; }
        }
        #endregion
    }
    #region Additional Forms

    //prosmotr.cpp
    //Form10
    public class list_view_otd: Form
    {
        public list_view_otd()
        {
            this.Text = "Отчет по отделению";
            this.Size = new Size(700, 500);
            this.StartPosition = FormStartPosition.CenterScreen;
        }
    }

    //Form11
    public class list_view_SCHEMA : Form
    {
        public list_view_SCHEMA()
        {
            this.Text = "Отчет по культуре";
            this.Size = new Size(700, 500);
            this.StartPosition = FormStartPosition.CenterScreen;
        }
    }

    public class Unit15 : Form
    {
        public Unit15()
        {
            this.Text = "Отчет по работе";
            this.Size = new Size(700, 500);
            this.StartPosition = FormStartPosition.CenterScreen;
        }
    }
    #endregion
}