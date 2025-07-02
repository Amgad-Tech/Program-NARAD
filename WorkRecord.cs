using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using Program_na_Ryadam.Models;
using System.Linq;
using System.Windows.Forms;

namespace Program_na_Ryadam
{
    public partial class WorkRecord : UserControl
    {
        // Database connection
        private string _dbPath;
        private string _connStr;

        // Data storage
        //
        private List<int> _departmentIDs = new List<int>();
        private List<int> _workerIDs = new List<int>();
        private List<int> _workTypeIDs = new List<int>();
        private List<bool> _workIsHourly = new List<bool>();
        private List<int> _brigadeIDs = new List<int>();
        private List<bool> _isBrigadeDepartment = new List<bool>();

        public int CurrentWorkerID { get; private set; }
        public string CurrentWorkerName { get; private set; }
        private System.Windows.Forms.DateTimePicker dtpQuantity;

        public WorkRecord(string dbPath)
        {
            InitializeComponent();
            _dbPath = dbPath;
            _connStr = $"Provider=Microsoft.Jet.OLEDB.4.0;Data Source={_dbPath}";
            dtpQuantity = new DateTimePicker();
            InitializeDateTimeFormats();
            InitializeGrid();
            SetupPopupControls();
            WireUpEvents();
            LoadData();
            cbBrigade.Visible = false;
        }

        private void WireUpEvents()
        {
            // Combo box events
            cbDepartment.SelectedIndexChanged += cbDepartment_SelectedIndexChanged;
            cbBrigade.SelectedIndexChanged += cbBrigade_SelectedIndexChanged;
            cbWorkType.SelectedIndexChanged += cbWorkType_SelectedIndexChanged;

            // Button events
            btnAddWork.Click += btnAddWork_Click;
            btnSave.Click += btnSave_Click_1;
            btnClearAll.Click += btnClearAll_Click;
            

            // Checkbox event
            CheckBox1.CheckedChanged += CheckBox1_CheckedChanged;

            // Grid events
            dgvWorkRecords.CellClick += DgvWorkRecords_CellClick;
            dgvWorkRecords.CellDoubleClick += dgvWorkRecords_CellDoubleClick;

            // Popup control events
            cbGridConfirm.LostFocus += CbGridConfirm_LostFocus;
            dtpGridDate.LostFocus += DtpGridDate_LostFocus;
            dtpGridTime.LostFocus += DtpGridTime_LostFocus;
        }


        public void LoadData()
        {
            try
            {
                LoadDepartments();
                SetDefaultDateTimeValues();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Initialization error: {ex.Message}");
            }
        }

        private void SetDefaultDateTimeValues()
        {
            dtpWorkDate.Value = DateTime.Today;
            dtpStartTime.Value = DateTime.Today.AddHours(8);
            dtpEndTime.Value = DateTime.Today.AddHours(17);
        }

        // NEW: Proper control enabling
        private void EnableControls(bool enable)
        {
            cbBrigade.Enabled = enable;
            cbWorker.Enabled = enable;
            cbWorkType.Enabled = enable;
            dtpWorkDate.Enabled = enable;
            dtpStartTime.Enabled = enable;
            dtpEndTime.Enabled = enable;
            dtpQuantity.Enabled = enable;
            txtQuantity.Enabled = enable;
            btnAddWork.Enabled = enable;
            btnOpenWorkList.Enabled = enable;
            CheckBox1.Enabled = enable;
        }

        private void InitializeDateTimeFormats()
        {
            dtpWorkDate.Format = DateTimePickerFormat.Custom;
            dtpWorkDate.CustomFormat = "dd.MM.yyyy";
            dtpStartTime.Format = DateTimePickerFormat.Custom;
            dtpStartTime.CustomFormat = "HH:mm";
            dtpEndTime.Format = DateTimePickerFormat.Custom;
            dtpEndTime.CustomFormat = "HH:mm";

            // dtpQuantity now properly initialized
            dtpQuantity.Format = DateTimePickerFormat.Custom;
            dtpQuantity.CustomFormat = "HH:mm";
            dtpQuantity.ShowUpDown = true;
            dtpQuantity.Value = DateTime.Today;
        }

        private DialogResult ShowDuplicatePrompt(string worker, string work, string date, string existingValue)
        {
            return MessageBox.Show($"У работника {worker}\nуже занесено {work} на {date}.\n"
                + $"Существующее значение: {existingValue}\n\n"
                + "Yes - Добавить к существующему\nNo - Перезаписать\nCancel - Пропустить",
                "Дублирующая запись", MessageBoxButtons.YesNoCancel);
        }

        private void SetupPopupControls()
        {

            dtpQuantity = new DateTimePicker();
            dtpQuantity.Format = DateTimePickerFormat.Custom;
            dtpQuantity.CustomFormat = "HH:mm";
            dtpQuantity.ShowUpDown = true;
            dtpQuantity.Visible = false;
            dtpQuantity.Value = DateTime.Today;
            GroupBox2.Controls.Add(dtpQuantity);  // Add to container

            // Position near other controls
            dtpQuantity.Location = new Point(343, 22);
            dtpQuantity.Size = new Size(121, 32);

            // Other popup controls
            dtpGridTime = new DateTimePicker();
            cbGridConfirm = new ComboBox();
            dtpGridDate = new DateTimePicker();

            // Time picker
            dtpGridTime.Format = DateTimePickerFormat.Custom;
            dtpGridTime.CustomFormat = "HH:mm";
            dtpGridTime.ShowUpDown = true;
            dtpGridTime.Visible = false;
            dtpGridTime.LostFocus += DtpGridTime_LostFocus;
            Controls.Add(dtpGridTime);

            // Confirmation combo
            cbGridConfirm.Items.AddRange(new object[] { "Нет", "Да" });
            cbGridConfirm.DropDownStyle = ComboBoxStyle.DropDownList;
            cbGridConfirm.Visible = false;
            cbGridConfirm.LostFocus += CbGridConfirm_LostFocus;
            Controls.Add(cbGridConfirm);

            // Date picker
            dtpGridDate.Format = DateTimePickerFormat.Custom;
            dtpGridDate.CustomFormat = "dd.MM.yyyy";
            dtpGridDate.Visible = false;
            dtpGridDate.LostFocus += DtpGridDate_LostFocus;
            Controls.Add(dtpGridDate);
        }

        private void InitializeGrid()
        {
            dgvWorkRecords.Columns.Clear();

            // Add columns
            dgvWorkRecords.Columns.Add("colIndex", "#");
            dgvWorkRecords.Columns.Add("colWorker", "Фамилия Имя Отчество");
            dgvWorkRecords.Columns.Add("colWorkName", "Название работы");
            dgvWorkRecords.Columns.Add("colQuantity", "КОЛИЧ.");
            dgvWorkRecords.Columns.Add("colWorkType", "Вид Р.");
            dgvWorkRecords.Columns.Add("colConfirmed", "ВД");
            dgvWorkRecords.Columns.Add("colStart", "Начало");
            dgvWorkRecords.Columns.Add("colEnd", "Конец");
            dgvWorkRecords.Columns.Add("colWorkerID", "Worker ID");
            dgvWorkRecords.Columns.Add("colWorkTypeID", "Work Type ID");
            dgvWorkRecords.Columns.Add("colDate", "Дата");
            dgvWorkRecords.Columns.Add("colLocation", "Место");
            dgvWorkRecords.Columns.Add("colDeptIDs", "Department IDs");
            dgvWorkRecords.Columns.Add("colLunch", "Обед");

            // Set column widths
            dgvWorkRecords.Columns["colIndex"].Width = 35;
            dgvWorkRecords.Columns["colWorker"].Width = 270;
            dgvWorkRecords.Columns["colWorkName"].Width = 250;
            dgvWorkRecords.Columns["colQuantity"].Width = 90;
            dgvWorkRecords.Columns["colWorkType"].Width = 90;
            dgvWorkRecords.Columns["colStart"].Width = 75;
            dgvWorkRecords.Columns["colEnd"].Width = 75;
            dgvWorkRecords.Columns["colDate"].Width = 100;
            dgvWorkRecords.Columns["colLocation"].Width = 80;

            // Hide technical columns
            dgvWorkRecords.Columns["colWorkerID"].Visible = false;
            dgvWorkRecords.Columns["colWorkTypeID"].Visible = false;
            dgvWorkRecords.Columns["colDeptIDs"].Visible = false;

            // Set up context menu
            var menu = new ContextMenuStrip();
            var deleteItem = new ToolStripMenuItem("Удалить");
            var deleteAllItem = new ToolStripMenuItem("Удалить все");
            deleteItem.Click += (s, e) => DeleteSelectedRecord();
            deleteAllItem.Click += (s, e) => DeleteAllRecords();
            menu.Items.Add(deleteItem);
            menu.Items.Add(deleteAllItem);
            dgvWorkRecords.ContextMenuStrip = menu;

            // Event handlers
            dgvWorkRecords.CellClick += DgvWorkRecords_CellClick;
            dgvWorkRecords.CellDoubleClick += dgvWorkRecords_CellDoubleClick;
        }

        // UPDATED: Proper error handling
        private void LoadDepartments()
        {
            cbDepartment.Items.Clear();
            _departmentIDs.Clear();
            _isBrigadeDepartment.Clear();

            using (OleDbConnection conn = new OleDbConnection(_connStr))
            {
                conn.Open();
                string sql = "SELECT * FROM Name_PODRASD ORDER BY Name_PODR";
                OleDbCommand cmd = new OleDbCommand(sql, conn);
                using (OleDbDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        cbDepartment.Items.Add(reader["Name_PODR"].ToString());
                        _departmentIDs.Add(Convert.ToInt32(reader["ID"]));
                        _isBrigadeDepartment.Add(Convert.ToBoolean(reader["BRIG_bool"]));
                    }
                }
            }

            if (cbDepartment.Items.Count > 0)
            {
                cbDepartment.SelectedIndex = 0;
            }
            else
            {
                cbDepartment.Text = "Create department first";
                EnableControls(false);
            }
        }

        private void cbDepartment_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbDepartment.SelectedIndex < 0) return;

            int deptID = _departmentIDs[cbDepartment.SelectedIndex];
            bool isBrigade = _isBrigadeDepartment[cbDepartment.SelectedIndex];
            cbBrigade.Visible = isBrigade;
            cbBrigade.Enabled = isBrigade;

            if (!isBrigade)
            {
                // Non-brigade department
                cbBrigade.Enabled = false;
                cbBrigade.SelectedIndex = -1;
                LoadWorkersForDepartment(deptID);
                LoadWorkTypesForDepartment(deptID);
            }
            else
            {
                // Brigade department
                cbBrigade.Enabled = true;
                LoadBrigadesForDepartment(deptID);
            }
        }

        private void LoadWorkersForDepartment(int deptID)
        {
            cbWorker.Items.Clear();
            _workerIDs.Clear();

            using (OleDbConnection conn = new OleDbConnection(_connStr))
            {
                conn.Open();
                string sql = $"SELECT * FROM BD_WORKING_ALL WHERE Podr={deptID} AND NOT fired ORDER BY Fam";
                OleDbCommand cmd = new OleDbCommand(sql, conn);
                using (OleDbDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string name = $"{reader["Fam"]} {reader["Imj"]} {reader["Otc"]}";
                        cbWorker.Items.Add(name);
                        _workerIDs.Add(Convert.ToInt32(reader["ID"]));
                    }
                }
            }

            if (cbWorker.Items.Count > 0)
            {
                cbWorker.SelectedIndex = 0;
                EnableControls(true);
            }
            else
            {
                cbWorker.Text = "Add workers to department";
                EnableControls(false);
            }
        }

        private void LoadWorkTypesForDepartment(int deptID)
        {
            cbWorkType.Items.Clear();
            _workTypeIDs.Clear();
            _workIsHourly.Clear();

            using (OleDbConnection conn = new OleDbConnection(_connStr))
            {
                conn.Open();
                string sql = CheckBox1.Checked ?
                    "SELECT * FROM Vid_RABOT ORDER BY Kultura_str, Name_W" :
                    $"SELECT * FROM Vid_RABOT WHERE Podrasd_N={deptID} ORDER BY Kultura_str, Name_W";

                OleDbCommand cmd = new OleDbCommand(sql, conn);
                using (OleDbDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string workName = string.IsNullOrEmpty(reader["Kultura_str"].ToString()) ?
                            reader["Name_W"].ToString() :
                            $"{reader["Kultura_str"]} {reader["Name_W"]}";

                        cbWorkType.Items.Add(workName);
                        _workTypeIDs.Add(Convert.ToInt32(reader["ID"]));
                        _workIsHourly.Add(Convert.ToBoolean(reader["Vid_r"]));
                    }
                }
            }

            if (cbWorkType.Items.Count > 0)
            {
                cbWorkType.SelectedIndex = 0;
                UpdateQuantityControl();
                EnableControls(true);
            }
            else
            {
                cbWorkType.Text = "Add work types";
                EnableControls(false);
            }
        }

        private void LoadBrigadesForDepartment(int deptID)
        {
            cbBrigade.Items.Clear();
            _brigadeIDs.Clear();

            using (OleDbConnection conn = new OleDbConnection(_connStr))
            {
                conn.Open();
                string sql = $"SELECT * FROM Name_BRIGADA WHERE ID_PODR={deptID}";
                OleDbCommand cmd = new OleDbCommand(sql, conn);
                using (OleDbDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        cbBrigade.Items.Add(reader["Name_Brigad"].ToString());
                        _brigadeIDs.Add(Convert.ToInt32(reader["ID"]));
                    }
                }
            }

            if (cbBrigade.Items.Count > 0)
            {
                cbBrigade.SelectedIndex = 0;
            }
            else
            {
                cbBrigade.Text = "No brigades available";
                cbBrigade.Enabled = false;
                EnableControls(false);
            }
        }


        private void cbBrigade_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbDepartment.SelectedIndex < 0 || cbBrigade.SelectedIndex < 0) return;

            int deptID = _departmentIDs[cbDepartment.SelectedIndex];
            int brigadeID = _brigadeIDs[cbBrigade.SelectedIndex];

            LoadWorkersForBrigade(deptID, brigadeID);
            LoadWorkTypesForBrigade(deptID, brigadeID);
        }

        private void LoadWorkersForBrigade(int deptID, int brigadeID)
        {
            cbWorker.Items.Clear();
            _workerIDs.Clear();

            using (OleDbConnection conn = new OleDbConnection(_connStr))
            {
                conn.Open();
                string sql = $"SELECT * FROM BD_WORKING_ALL WHERE Podr={deptID} AND Brigada={brigadeID} AND NOT fired ORDER BY Fam";
                OleDbCommand cmd = new OleDbCommand(sql, conn);
                using (OleDbDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string name = $"{reader["Fam"]} {reader["Imj"]} {reader["Otc"]}";
                        cbWorker.Items.Add(name);
                        _workerIDs.Add(Convert.ToInt32(reader["ID"]));
                    }
                }
            }

            if (cbWorker.Items.Count > 0)
            {
                cbWorker.SelectedIndex = 0;
                EnableControls(true);
            }
            else
            {
                cbWorker.Text = "Add workers to brigade";
                EnableControls(false);
            }
        }

        private void LoadWorkTypesForBrigade(int deptID, int brigadeID)
        {
            cbWorkType.Items.Clear();
            _workTypeIDs.Clear();
            _workIsHourly.Clear();

            using (OleDbConnection conn = new OleDbConnection(_connStr))
            {
                conn.Open();
                string sql = CheckBox1.Checked ?
                    "SELECT * FROM Vid_RABOT ORDER BY Kultura_str, Name_W" :
                    $"SELECT * FROM Vid_RABOT WHERE Podrasd_N={deptID} AND ID_Brig={brigadeID} ORDER BY Kultura_str, Name_W";

                OleDbCommand cmd = new OleDbCommand(sql, conn);
                using (OleDbDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string workName = string.IsNullOrEmpty(reader["Kultura_str"].ToString()) ?
                            reader["Name_W"].ToString() :
                            $"{reader["Kultura_str"]} {reader["Name_W"]}";

                        cbWorkType.Items.Add(workName);
                        _workTypeIDs.Add(Convert.ToInt32(reader["ID"]));
                        _workIsHourly.Add(Convert.ToBoolean(reader["IsHourly"]));
                    }
                }
            }

            if (cbWorkType.Items.Count > 0)
            {
                cbWorkType.SelectedIndex = 0;
                UpdateQuantityControl();
                EnableControls(true);
            }
            else
            {
                cbWorkType.Text = "Add work types";
                EnableControls(false);
            }
        }

        private void cbWorkType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbWorkType.SelectedIndex >= 0)
            {
                UpdateQuantityControl();
            }
        }


        private void UpdateQuantityControl()
        {
            if (cbWorkType.SelectedIndex < 0 || cbWorkType.SelectedIndex >= _workIsHourly.Count)
                return;

            bool isHourly = _workIsHourly[cbWorkType.SelectedIndex];
            dtpQuantity.Visible = isHourly;
            txtQuantity.Visible = !isHourly;

            if (isHourly)
            {
                dtpQuantity.Value = DateTime.Today;
            }
            else
            {
                txtQuantity.Text = "0";
            }
        }

        private void CheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (cbDepartment.SelectedIndex < 0) return;

            int deptID = _departmentIDs[cbDepartment.SelectedIndex];
            bool isBrigade = _isBrigadeDepartment[cbDepartment.SelectedIndex];

            if (isBrigade && cbBrigade.SelectedIndex >= 0)
            {
                int brigadeID = _brigadeIDs[cbBrigade.SelectedIndex];
                LoadWorkTypesForBrigade(deptID, brigadeID);
            }
            else
            {
                LoadWorkTypesForDepartment(deptID);
            }
        }

        private void btnAddWork_Click(object sender, EventArgs e)
        {
            if (cbWorker.SelectedIndex < 0 || cbWorkType.SelectedIndex < 0)
            {
                MessageBox.Show("Select worker and work type");
                return;
            }

            // Validate input
            if (txtQuantity.Visible && (string.IsNullOrEmpty(txtQuantity.Text) || txtQuantity.Text == "0"))
            {
                MessageBox.Show("Enter quantity!");
                return;
            }

            if (dtpQuantity.Visible && dtpQuantity.Value.ToString("HH:mm") == "00:00")
            {
                MessageBox.Show("Enter work hours!");
                return;
            }

            // Prepare values
            int workerID = _workerIDs[cbWorker.SelectedIndex];
            int workTypeID = _workTypeIDs[cbWorkType.SelectedIndex];
            bool isHourly = _workIsHourly[cbWorkType.SelectedIndex];
            string date = dtpWorkDate.Value.ToString("dd.MM.yyyy");

            // Check for duplicates
            if (IsDuplicateEntry(workerID, workTypeID, date))
            {
                MessageBox.Show($"Worker {cbWorker.Text} already has work {cbWorkType.Text} on {date}");
                return;
            }

            // Add to grid
            int rowIndex = dgvWorkRecords.Rows.Add();
            DataGridViewRow row = dgvWorkRecords.Rows[rowIndex];

            row.Cells["colIndex"].Value = rowIndex + 1;
            row.Cells["colWorker"].Value = cbWorker.Text;
            row.Cells["colWorkName"].Value = cbWorkType.Text;
            row.Cells["colWorkerID"].Value = workerID;
            row.Cells["colWorkTypeID"].Value = workTypeID;
            row.Cells["colDate"].Value = date;
            row.Cells["colStart"].Value = dtpStartTime.Value.ToString("HH:mm");
            row.Cells["colEnd"].Value = dtpEndTime.Value.ToString("HH:mm");
            row.Cells["colConfirmed"].Value = "Нет";
            row.Cells["colLunch"].Value = "00:00";

            if (isHourly)
            {
                row.Cells["colQuantity"].Value = dtpQuantity.Value.ToString("HH:mm");
                row.Cells["colWorkType"].Value = "Часовая";
            }
            else
            {
                row.Cells["colQuantity"].Value = txtQuantity.Text;
                row.Cells["colWorkType"].Value = "Сдельно";
            }

            // Enable action buttons
            btnSave.Enabled = true;
            btnClearAll.Enabled = true;
        }

        public bool IsDuplicateEntry(int workerID, int workTypeID, string date)
        {
            foreach (DataGridViewRow row in dgvWorkRecords.Rows)
            {
                if (row.IsNewRow) continue;

                int rowWorkerID = Convert.ToInt32(row.Cells["colWorkerID"].Value);
                int rowWorkTypeID = Convert.ToInt32(row.Cells["colWorkTypeID"].Value);
                string rowDate = row.Cells["colDate"].Value?.ToString();

                if (rowWorkerID == workerID &&
                    rowWorkTypeID == workTypeID &&
                    rowDate == date)
                {
                    return true;
                }
            }
            return false;
        }

        private void btnSave_Click_1(object sender, EventArgs e)
        {
            if (!ValidateRecords()) return;

            using (OleDbConnection conn = new OleDbConnection(_connStr))
            {
                conn.Open();

                foreach (DataGridViewRow row in dgvWorkRecords.Rows)
                {
                    if (row.IsNewRow) continue;

                    int workerID = Convert.ToInt32(row.Cells["colWorkerID"].Value);
                    int workTypeID = Convert.ToInt32(row.Cells["colWorkTypeID"].Value);
                    string workName = row.Cells["colWorkName"].Value.ToString();
                    DateTime date = DateTime.Parse(row.Cells["colDate"].Value.ToString());
                    bool isConfirmed = row.Cells["colConfirmed"].Value.ToString() == "Да";
                    string location = row.Cells["colLocation"].Value?.ToString() ?? "";
                    string lunch = row.Cells["colLunch"].Value?.ToString() ?? "00:00";
                    string workType = row.Cells["colWorkType"].Value.ToString();
                    string quantityValue = row.Cells["colQuantity"].Value.ToString();

                    // Check if record exists
                    bool recordExists = false;
                    string existingQuantity = "";
                    using (OleDbCommand checkCmd = new OleDbCommand(
                        "SELECT * FROM BD_workin_rab_all " +
                        "WHERE ID_working = @workerID " +
                        "AND ID_work = @workTypeID " +
                        "AND Data = @date", conn))
                    {
                        checkCmd.Parameters.AddWithValue("@workerID", workerID);
                        checkCmd.Parameters.AddWithValue("@workTypeID", workTypeID);
                        checkCmd.Parameters.AddWithValue("@date", date);

                        using (OleDbDataReader reader = checkCmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                recordExists = true;
                                existingQuantity = workType == "Часовая" ?
                                    reader["Kolich_hour"].ToString() :
                                    reader["KOLICH"].ToString();
                            }
                        }
                    }

                    if (recordExists)
                    {
                        var result = ShowDuplicatePrompt(
                            row.Cells["colWorker"].Value.ToString(),
                            workName,
                            date.ToString("dd.MM.yyyy"),
                            existingQuantity
                        );

                        if (result == DialogResult.Yes) // Add to existing
                        {
                            if (workType == "Часовая")
                            {
                                TimeSpan existingTime = TimeSpan.Parse(existingQuantity);
                                TimeSpan newTime = TimeSpan.Parse(quantityValue);
                                quantityValue = (existingTime + newTime).ToString(@"hh\:mm");
                            }
                            else
                            {
                                int existingQty = int.Parse(existingQuantity);
                                int newQty = int.Parse(quantityValue);
                                quantityValue = (existingQty + newQty).ToString();
                            }
                        }
                        else if (result == DialogResult.Cancel) // Skip
                        {
                            continue;
                        }
                    }

                    // Build SQL (same as before)
                    string quantityField = workType == "Часовая" ? "Kolich_hour" : "KOLICH";
                    string formattedQuantity = workType == "Часовая" ?
                        $"#{DateTime.Parse(quantityValue):HH:mm}#" :
                        quantityValue;

                    string sql = $@"
        IF EXISTS (
            SELECT 1 FROM BD_workin_rab_all 
            WHERE ID_working = {workerID} 
            AND ID_work = {workTypeID} 
            AND Data = #{date:MM/dd/yyyy}#
        )
        BEGIN
            UPDATE BD_workin_rab_all SET
                {quantityField} = {quantityValue},
                Vech_day = {(isConfirmed ? 1 : 0)},
                Begin_work = #{row.Cells["colStart"].Value}#,
                End_work = #{row.Cells["colEnd"].Value}#,
                Obed_tim = #{lunch}#,
                Mesto_rab = '{location}'
            WHERE 
                ID_working = {workerID} 
                AND ID_work = {workTypeID} 
                AND Data = #{date:MM/dd/yyyy}#
        END
        ELSE
        BEGIN
            INSERT INTO BD_workin_rab_all (
                ID_working, ID_work, Name_work, Data, 
                Begin_work, End_work, {quantityField}, 
                Vech_day, Mesto_rab, Obed_tim
            ) VALUES (
                {workerID}, {workTypeID}, '{workName}', 
                #{date:MM/dd/yyyy}#, 
                #{row.Cells["colStart"].Value}#, 
                #{row.Cells["colEnd"].Value}#, 
                {quantityValue}, 
                {(isConfirmed ? 1 : 0)}, 
                '{location}', 
                #{lunch}#
            )
        END";

                    new OleDbCommand(sql, conn).ExecuteNonQuery();
                }
            }

            MessageBox.Show("Data saved successfully!");
            dgvWorkRecords.Rows.Clear();
            btnSave.Enabled = false;
            btnClearAll.Enabled = false;
        }


        public bool ValidateRecords()
        {
            foreach (DataGridViewRow row in dgvWorkRecords.Rows)
            {
                if (row.IsNewRow) continue;

                bool isM2 = Convert.ToBoolean(row.Cells["colIsM2"].Value);
                var deptIDs = row.Cells["colDeptIDs"].Value?.ToString().Split(';');

                if (isM2 && (deptIDs == null || deptIDs.Length != 1))
                {
                    MessageBox.Show($"M2 work requires exactly 1 department!\nRow #{row.Index + 1}: {row.Cells["colWorkName"].Value}");
                    return false;
                }

                if (string.IsNullOrEmpty(row.Cells["colQuantity"].Value?.ToString()) ||
                    row.Cells["colQuantity"].Value.ToString() == "0")
                {
                    MessageBox.Show($"Quantity not set for worker {row.Cells["colWorker"].Value} on {row.Cells["colDate"].Value}");
                    return false;
                }
            }
            return true;
        }

        private void btnClearAll_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Delete all records?", "Confirmation",
                MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                dgvWorkRecords.Rows.Clear();
                btnSave.Enabled = false;
                btnClearAll.Enabled = false;
            }
        }
        private void DeleteSelectedRecord()
        {
            // Get mouse position relative to the grid
            Point mousePosition = dgvWorkRecords.PointToClient(Cursor.Position);
            DataGridView.HitTestInfo hitInfo = dgvWorkRecords.HitTest(mousePosition.X, mousePosition.Y);

            if (hitInfo.RowIndex >= 0 && !dgvWorkRecords.Rows[hitInfo.RowIndex].IsNewRow)
            {
                dgvWorkRecords.Rows.RemoveAt(hitInfo.RowIndex);
                UpdateRowNumbers();
                CheckRecordsExist();
            }
        }

        private void DeleteAllRecords()
        {
            if (MessageBox.Show("Вы действительно хотите удалить все записи?",
                "Подтверждение", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                dgvWorkRecords.Rows.Clear();
                btnSave.Enabled = false;
                btnClearAll.Enabled = false;
            }
        }

        private void UpdateRowNumbers()
        {
            for (int i = 0; i < dgvWorkRecords.Rows.Count; i++)
            {
                dgvWorkRecords.Rows[i].Cells["colIndex"].Value = i + 1;
            }
        }

        private void CheckRecordsExist()
        {
            bool hasRecords = dgvWorkRecords.Rows.Count > 0;
            btnSave.Enabled = hasRecords;
            btnClearAll.Enabled = hasRecords;
        }

        private void DgvWorkRecords_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

            var cell = dgvWorkRecords.Rows[e.RowIndex].Cells[e.ColumnIndex];
            Rectangle cellRect = dgvWorkRecords.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false);

            // Handle different column types
            switch (dgvWorkRecords.Columns[e.ColumnIndex].Name)
            {
                case "colConfirmed":
                    cbGridConfirm.Location = new Point(
                        dgvWorkRecords.Location.X + cellRect.Left,
                        dgvWorkRecords.Location.Y + cellRect.Top
                    );
                    cbGridConfirm.Size = new Size(cellRect.Width, cellRect.Height);
                    cbGridConfirm.Text = cell.Value?.ToString() ?? "Нет";
                    cbGridConfirm.Visible = true;
                    cbGridConfirm.BringToFront();
                    break;

                case "colDate":
                    dtpGridDate.Location = new Point(
                        dgvWorkRecords.Location.X + cellRect.Left,
                        dgvWorkRecords.Location.Y + cellRect.Top
                    );
                    dtpGridDate.Size = new Size(cellRect.Width, cellRect.Height);
                    dtpGridDate.Value = DateTime.TryParse(cell.Value?.ToString(), out DateTime date)
                        ? date : DateTime.Now;
                    dtpGridDate.Visible = true;
                    dtpGridDate.BringToFront();
                    break;

                case "colStart":
                case "colEnd":
                case "colQuantity":
                case "colLunch":
                    dtpGridTime.Location = new Point(
                        dgvWorkRecords.Location.X + cellRect.Left,
                        dgvWorkRecords.Location.Y + cellRect.Top
                    );
                    dtpGridTime.Size = new Size(cellRect.Width, cellRect.Height);
                    dtpGridTime.Value = DateTime.TryParse(cell.Value?.ToString(), out DateTime time)
                        ? time : DateTime.Now;
                    dtpGridTime.Visible = true;
                    dtpGridTime.BringToFront();
                    break;
            }
        }

        private void dgvWorkRecords_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == colLocation.Index && e.RowIndex >= 0)
            {
                string preSelected = dgvWorkRecords.Rows[e.RowIndex].Cells["colDeptIDs"].Value?.ToString() ?? "";
                using (var form = new F_spisok_otd(_dbPath, preSelected))
                {
                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        dgvWorkRecords.Rows[e.RowIndex].Cells["colLocation"].Value =
                            string.Join("; ", form.SelectedDepartmentNames);
                        dgvWorkRecords.Rows[e.RowIndex].Cells["colDeptIDs"].Value =
                            string.Join(";", form.SelectedDepartmentIDs);
                    }
                }
            }
        }

        private void CbGridConfirm_LostFocus(object sender, EventArgs e)
        {
            if (dgvWorkRecords.CurrentCell != null)
            {
                dgvWorkRecords.CurrentCell.Value = cbGridConfirm.Text;
            }
            cbGridConfirm.Visible = false;
        }

        private void DtpGridDate_LostFocus(object sender, EventArgs e)
        {
            if (dgvWorkRecords.CurrentCell != null)
            {
                dgvWorkRecords.CurrentCell.Value = dtpGridDate.Value.ToString("dd.MM.yyyy");
            }
            dtpGridDate.Visible = false;
        }

        private void DtpGridTime_LostFocus(object sender, EventArgs e)
        {
            if (dgvWorkRecords.CurrentCell != null)
            {
                // Update the cell value first
                dgvWorkRecords.CurrentCell.Value = dtpGridTime.Value.ToString("HH:mm");

                // Get current row reference
                DataGridViewRow row = dgvWorkRecords.Rows[dgvWorkRecords.CurrentCell.RowIndex];
                string colName = dgvWorkRecords.CurrentCell.OwningColumn.Name;

                // Calculate lunch break duration if start/end time changes
                if (colName == "colStart" || colName == "colEnd")
                {
                    if (TimeSpan.TryParse(row.Cells["colStart"].Value?.ToString(), out TimeSpan start) &&
                        TimeSpan.TryParse(row.Cells["colEnd"].Value?.ToString(), out TimeSpan end))
                    {
                        TimeSpan duration = end - start;
                        if (duration.TotalHours > 6)
                        {
                            row.Cells["colLunch"].Value = "00:45";
                        }
                    }
                }
            }
            dtpGridTime.Visible = false;
        }


        public void AddWorkEntry(string cultureName, string workType, WorkTypeInfo workInfo,
                       string date, List<int> deptIDs, List<string> deptNames)
        {
            int rowIndex = dgvWorkRecords.Rows.Add();
            DataGridViewRow row = dgvWorkRecords.Rows[rowIndex];

            row.Cells["colIndex"].Value = rowIndex + 1;
            row.Cells["colWorker"].Value = CurrentWorkerName;
            row.Cells["colWorkName"].Value = $"{cultureName} {workType}";

            if (workInfo.IsHourly)
            {
                row.Cells["colQuantity"].Value = "00:00";
                row.Cells["colWorkType"].Value = "Часовая";
            }
            else
            {
                row.Cells["colQuantity"].Value = "0";
                row.Cells["colWorkType"].Value = "Сдельно";
            }

            row.Cells["colConfirmed"].Value = "Нет";
            row.Cells["colStart"].Value = dtpStartTime.Value.ToString("HH:mm");
            row.Cells["colEnd"].Value = dtpEndTime.Value.ToString("HH:mm");
            row.Cells["colWorkerID"].Value = CurrentWorkerID;
            row.Cells["colWorkTypeID"].Value = workInfo.ID;
            row.Cells["colDate"].Value = date;
            row.Cells["colLocation"].Value = string.Join("; ", deptNames);
            row.Cells["colDeptIDs"].Value = string.Join(";", deptIDs);
            row.Cells["colLunch"].Value = "00:00";
        }

        public void ShowActionButtons()
        {
            btnSave.Visible = btnSave.Enabled = true;
            btnClearAll.Visible = btnClearAll.Enabled = true;
        }

        public bool IsDuplicateEntry2(int workTypeID, int dateAsInt, string deptIDsStr)
        {
            // Convert int date to string format (dd.MM.yyyy)
            string dateStr = new DateTime(
                dateAsInt / 10000,         // year
                (dateAsInt / 100) % 100,   // month
                dateAsInt % 100             // day
            ).ToString("dd.MM.yyyy");

            foreach (DataGridViewRow row in dgvWorkRecords.Rows)
            {
                if (row.IsNewRow) continue;

                int rowWorkTypeID = Convert.ToInt32(row.Cells["colWorkTypeID"].Value);
                string rowDate = row.Cells["colDate"].Value?.ToString() ?? "";
                string rowDeptIDs = row.Cells["colDeptIDs"].Value?.ToString() ?? "";

                if (rowWorkTypeID == workTypeID &&
                    rowDate == dateStr &&
                    rowDeptIDs == deptIDsStr)
                {
                    return true;
                }
            }
            return false;
        }

        private void btnOpenWorkList_Click(object sender, EventArgs e)
        {
            if (cbWorker.SelectedIndex < 0)
            {
                MessageBox.Show("Сначала выберите работника");
                return;
            }

            CurrentWorkerID = _workerIDs[cbWorker.SelectedIndex];
            CurrentWorkerName = cbWorker.Text;
            using (var workForm = new f_list_view(_dbPath))
            {
                if (workForm.ShowDialog() == DialogResult.OK)
                {
                    foreach (var work in workForm.SelectedWorks)
                    {
                        AddWorkEntry(
                            work.CultureName,
                            work.WorkType,
                            work.WorkInfo,
                            work.Date,
                            work.DepartmentIDs,
                            work.DepartmentNames
                        );
                    }
                    ShowActionButtons();
                }
            }

        }
        private void btnWorkByDepartments_Click(object sender, EventArgs e)
        {
            if (dgvWorkRecords.CurrentRow == null || dgvWorkRecords.CurrentRow.IsNewRow)
            {
                MessageBox.Show("Выберите запись в таблице");
                return;
            }

            var currentRow = dgvWorkRecords.CurrentRow;
            var preSelected = currentRow.Cells["colDeptIDs"].Value?.ToString() ?? "";

            using (var form = new F_spisok_otd(_dbPath, preSelected))
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    currentRow.Cells["colDeptIDs"].Value = form.SelectedDepartmentIDs;
                    currentRow.Cells["colLocation"].Value = form.SelectedDepartmentNames;
                }
            }
        }
        //
    }
}