using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Program_na_Ryadam
{
    public partial class recordwork : UserControl
    {
        // Database connection
        private string dbPath = System.IO.Path.Combine(Application.StartupPath, "Database", "Database_XN1.mdb");
        private string connStr;

        // Data structures
        private List<Department> departments = new List<Department>();
        private List<Brigade> brigades = new List<Brigade>();

        private class Department
        {
            public int ID { get; set; }
            public string Name { get; set; }
            public bool HasBrigade { get; set; }
        }

        private class Brigade
        {
            public int ID { get; set; }
            public string Name { get; set; }
        }

        // UI Controls
        private GroupBox groupBox1;
        private DataGridView dataGridView1;
        private ComboBox comboBoxDepartment;
        private ComboBox comboBoxBrigade;
        private Button buttonRefresh;
        private Button buttonSave;
        private ComboBox comboBoxGrid;
        private Label label1;
        private Label label2;

        // Padding constants
        private const int TopPadding = 30;  // Added top padding
        private const int VerticalSpacing = 35; // Spacing between controls

        public recordwork()
        {
            InitializeComponent();
            connStr = $"Provider=Microsoft.Jet.OLEDB.4.0;Data Source={dbPath}";
            SetupControls();
            InitializeDataGridView();
        }

        private void SetupControls()
        {
            // Main layout
            this.Size = new Size(925, 412);

            // GroupBox (Actions panel)
            groupBox1 = new GroupBox();
            groupBox1.Text = "Действия";
            groupBox1.Font = new Font("Times New Roman", 12F);
            groupBox1.Dock = DockStyle.Right;
            groupBox1.Size = new Size(405, 412);
            this.Controls.Add(groupBox1);

            // DataGridView
            dataGridView1 = new DataGridView();
            dataGridView1.Dock = DockStyle.Fill;
            dataGridView1.Font = new Font("Times New Roman", 12F);
            dataGridView1.ReadOnly = true;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToDeleteRows = false;
            dataGridView1.AllowUserToResizeRows = false;
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.ScrollBars = ScrollBars.Vertical;
            dataGridView1.CellClick += DataGridView1_CellClick;
            this.Controls.Add(dataGridView1);

            // Department label and combo
            label1 = new Label();
            label1.Text = "Подразделение:";
            label1.Location = new Point(6, TopPadding);  // Added top padding
            label1.Size = new Size(114, 19);
            groupBox1.Controls.Add(label1);

            comboBoxDepartment = new ComboBox();
            comboBoxDepartment.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxDepartment.Enabled = false;
            comboBoxDepartment.Location = new Point(142, TopPadding - 3);  // Added top padding
            comboBoxDepartment.Size = new Size(251, 27);
            comboBoxDepartment.SelectedIndexChanged += ComboBoxDepartment_SelectedIndexChanged;
            groupBox1.Controls.Add(comboBoxDepartment);

            // Brigade label and combo
            label2 = new Label();
            label2.Text = "Бригада:";
            label2.Location = new Point(6, TopPadding + VerticalSpacing);  // Added vertical spacing
            label2.Size = new Size(61, 19);
            groupBox1.Controls.Add(label2);

            comboBoxBrigade = new ComboBox();
            comboBoxBrigade.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxBrigade.Enabled = false;
            comboBoxBrigade.Location = new Point(142, TopPadding + VerticalSpacing - 3);  // Added vertical spacing
            comboBoxBrigade.Size = new Size(251, 27);
            comboBoxBrigade.SelectedIndexChanged += ComboBoxBrigade_SelectedIndexChanged;
            groupBox1.Controls.Add(comboBoxBrigade);

            // Refresh button
            buttonRefresh = new Button();
            buttonRefresh.Text = "Обновить данные";
            buttonRefresh.Location = new Point(17, TopPadding + VerticalSpacing * 2);  // Added vertical spacing
            buttonRefresh.Size = new Size(368, 34);
            buttonRefresh.Click += ButtonRefresh_Click;
            groupBox1.Controls.Add(buttonRefresh);

            // Save button
            buttonSave = new Button();
            buttonSave.Text = "Сохранить изменения";
            buttonSave.Location = new Point(17, TopPadding + VerticalSpacing * 3);  // Added vertical spacing
            buttonSave.Size = new Size(368, 34);
            buttonSave.Enabled = false;
            buttonSave.Click += ButtonSave_Click;
            groupBox1.Controls.Add(buttonSave);

            // In-grid combo box
            comboBoxGrid = new ComboBox();
            comboBoxGrid.Items.AddRange(new object[] { "Нет", "Да" });
            comboBoxGrid.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxGrid.Visible = false;
            comboBoxGrid.SelectedIndexChanged += ComboBoxGrid_SelectedIndexChanged;
            dataGridView1.Controls.Add(comboBoxGrid);
        }

        private void InitializeDataGridView()
        {
            dataGridView1.Columns.Clear();

            // Column setup
            DataGridViewTextBoxColumn colIndex = new DataGridViewTextBoxColumn();
            colIndex.HeaderText = "№";
            colIndex.Width = 40;
            colIndex.ReadOnly = true;

            DataGridViewTextBoxColumn colWork = new DataGridViewTextBoxColumn();
            colWork.HeaderText = "Название работы";
            colWork.Width = 320;
            colWork.ReadOnly = true;

            DataGridViewTextBoxColumn colVisible = new DataGridViewTextBoxColumn();
            colVisible.HeaderText = "Включить";
            colVisible.Width = 100;

            DataGridViewTextBoxColumn colID = new DataGridViewTextBoxColumn();
            colID.Visible = false;

            dataGridView1.Columns.AddRange(new[] { colIndex, colWork, colVisible, colID });
        }

        // ===== Event Handlers =====
        private void ButtonRefresh_Click(object sender, EventArgs e)
        {
            LoadDepartments();
        }

        private void ComboBoxDepartment_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxDepartment.SelectedIndex < 0) return;

            Department dept = departments[comboBoxDepartment.SelectedIndex];
            if (dept.HasBrigade)
            {
                LoadBrigades(dept.ID);
                LoadWorkTypes(true);
            }
            else
            {
                comboBoxBrigade.Enabled = false;
                comboBoxBrigade.Items.Clear();
                brigades.Clear();
                LoadWorkTypes(false);
            }
        }

        private void ComboBoxBrigade_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadWorkTypes(true);
        }

        private void DataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex != 2 || dataGridView1.Rows.Count == 0) return;

            Rectangle cellRect = dataGridView1.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false);
            comboBoxGrid.Size = new Size(cellRect.Width, cellRect.Height);
            comboBoxGrid.Location = new Point(
                dataGridView1.Left + cellRect.Left,
                dataGridView1.Top + cellRect.Top
            );

            string currentValue = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value?.ToString();
            comboBoxGrid.SelectedIndex = currentValue == "Да" ? 1 : 0;
            comboBoxGrid.Visible = true;
            comboBoxGrid.BringToFront();
        }

        private void ComboBoxGrid_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentCell != null)
            {
                dataGridView1.CurrentCell.Value = comboBoxGrid.Text;
            }
            comboBoxGrid.Visible = false;
        }

        private void ButtonSave_Click(object sender, EventArgs e)
        {
            SaveVisibilityChanges();
        }

        // ===== Database Operations =====
        private void LoadDepartments()
        {
            departments.Clear();
            comboBoxDepartment.Items.Clear();

            using (OleDbConnection conn = new OleDbConnection(connStr))
            {
                conn.Open();
                string query = "SELECT * FROM Name_PODRASD ORDER BY Name_PODR";
                OleDbCommand cmd = new OleDbCommand(query, conn);
                OleDbDataReader reader = cmd.ExecuteReader();

                if (!reader.HasRows)
                {
                    MessageBox.Show("Нет ни одного подразделения и бригады.\nДля начала создайте подразделения.",
                        "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                while (reader.Read())
                {
                    departments.Add(new Department
                    {
                        ID = Convert.ToInt32(reader["ID"]),
                        Name = reader["Name_PODR"].ToString(),
                        HasBrigade = Convert.ToBoolean(reader["BRIG_bool"])
                    });
                    comboBoxDepartment.Items.Add(reader["Name_PODR"].ToString());
                }
            }

            comboBoxDepartment.Enabled = true;
            comboBoxDepartment.SelectedIndex = 0;
        }

        private void LoadBrigades(int deptID)
        {
            brigades.Clear();
            comboBoxBrigade.Items.Clear();

            using (OleDbConnection conn = new OleDbConnection(connStr))
            {
                conn.Open();
                string query = $"SELECT * FROM Name_BRIGADA WHERE ID_PODR={deptID}";
                OleDbCommand cmd = new OleDbCommand(query, conn);
                OleDbDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    brigades.Add(new Brigade
                    {
                        ID = Convert.ToInt32(reader["ID"]),
                        Name = reader["Name_Brigad"].ToString()
                    });
                    comboBoxBrigade.Items.Add(reader["Name_Brigad"].ToString());
                }
            }

            comboBoxBrigade.Enabled = brigades.Count > 0;
            if (comboBoxBrigade.Enabled) comboBoxBrigade.SelectedIndex = 0;
        }

        private void LoadWorkTypes(bool hasBrigade)
        {
            dataGridView1.Rows.Clear();
            buttonSave.Enabled = false;

            if (comboBoxDepartment.SelectedIndex < 0) return;
            int deptID = departments[comboBoxDepartment.SelectedIndex].ID;
            int brigadeID = hasBrigade && comboBoxBrigade.SelectedIndex >= 0
                ? brigades[comboBoxBrigade.SelectedIndex].ID
                : -1;

            string query = hasBrigade
                ? $"SELECT * FROM Vid_RABOT WHERE Podrasd_N={deptID} AND ID_Brig={brigadeID} ORDER BY Name_W"
                : $"SELECT * FROM Vid_RABOT WHERE Podrasd_N={deptID} ORDER BY Name_W";

            using (OleDbConnection conn = new OleDbConnection(connStr))
            {
                conn.Open();
                OleDbCommand cmd = new OleDbCommand(query, conn);
                OleDbDataReader reader = cmd.ExecuteReader();

                int rowIndex = 0;
                while (reader.Read())
                {
                    dataGridView1.Rows.Add();
                    dataGridView1.Rows[rowIndex].Cells[0].Value = (rowIndex + 1).ToString();
                    dataGridView1.Rows[rowIndex].Cells[1].Value = reader["Name_W"].ToString();
                    dataGridView1.Rows[rowIndex].Cells[2].Value =
                        Convert.ToBoolean(reader["VKL_SAPIC"]) ? "Да" : "Нет";
                    dataGridView1.Rows[rowIndex].Cells[3].Value = reader["ID"].ToString();
                    rowIndex++;
                }
                buttonSave.Enabled = rowIndex > 0;
            }
        }

        private void SaveVisibilityChanges()
        {
            if (comboBoxDepartment.SelectedIndex < 0) return;
            int deptID = departments[comboBoxDepartment.SelectedIndex].ID;
            bool hasBrigade = departments[comboBoxDepartment.SelectedIndex].HasBrigade;
            int brigadeID = hasBrigade && comboBoxBrigade.SelectedIndex >= 0
                ? brigades[comboBoxBrigade.SelectedIndex].ID
                : -1;

            using (OleDbConnection conn = new OleDbConnection(connStr))
            {
                conn.Open();
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if (row.Cells[3].Value == null) continue;

                    int workID = Convert.ToInt32(row.Cells[3].Value);
                    bool isVisible = row.Cells[2].Value?.ToString() == "Да";

                    string updateQuery = $"UPDATE Vid_RABOT SET VKL_SAPIC = {(isVisible ? "True" : "False")} " +
                                        $"WHERE ID = {workID}";

                    if (hasBrigade)
                    {
                        updateQuery += $" AND ID_Brig = {brigadeID}";
                    }

                    new OleDbCommand(updateQuery, conn).ExecuteNonQuery();
                }
            }
            MessageBox.Show("Изменения сохранены успешно!", "Сохранено",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}