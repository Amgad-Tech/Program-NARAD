using System;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Program_na_Ryadam
{
    public partial class DepartmentsPositions : UserControl
    {
        // Database connection
        private string dbPath = System.IO.Path.Combine(Application.StartupPath, "Database", "Database_XN1.mdb");
        private string connStr;

        // Data tables
        private DataTable tableDepartments = new DataTable();
        private DataTable tablePositions = new DataTable();
        private DataTable tableBrigades = new DataTable();

        // Data views for filtering
        private DataView viewPositions;
        private DataView viewBrigades;

        // State flags
        private bool isAddingDepartment = false;
        private bool isAddingPosition = false;
        private bool isEditingBrigade = false;

        public DepartmentsPositions()
        {
            InitializeComponent();
            connStr = $"Provider=Microsoft.Jet.OLEDB.4.0;Data Source={dbPath}";
            InitializeDatabase();
            InitializeData();
        }

        private void InitializeDatabase()
        {
            try
            {
                using (OleDbConnection conn = new OleDbConnection(connStr))
                {
                    // Load departments
                    OleDbDataAdapter daDept = new OleDbDataAdapter(
                        "SELECT ID, Name_PODR, BRIG_bool, N_BRIG, Komment FROM Table_Departments", conn);
                    daDept.Fill(tableDepartments);

                    // Load positions
                    OleDbDataAdapter daPos = new OleDbDataAdapter(
                        "SELECT ID, ID_PODR, Name_DOLSN, N_all, N_use, Komment FROM Table_Positions", conn);
                    daPos.Fill(tablePositions);

                    // Load brigades
                    OleDbDataAdapter daBrig = new OleDbDataAdapter(
                        "SELECT ID, ID_PODR, Name_Brigad FROM Table_Brigades", conn);
                    daBrig.Fill(tableBrigades);
                }

                // Create filtered views
                viewPositions = new DataView(tablePositions);
                viewBrigades = new DataView(tableBrigades);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Database error: {ex.Message}");
            }
        }

        private void InitializeData()
        {
            // Set up data sources
            gridDepartments.DataSource = tableDepartments;
            gridPositions.DataSource = viewPositions;
            gridBrigades.DataSource = viewBrigades;

            // Configure grids
            gridDepartments.Columns["ID"].Visible = false;
            gridDepartments.Columns["Name_PODR"].HeaderText = "Подразделение";
            gridDepartments.Columns["BRIG_bool"].HeaderText = "Разд. на бригады";
            gridDepartments.Columns["N_BRIG"].HeaderText = "Кол. бриг.";

            gridPositions.Columns["ID"].Visible = false;
            gridPositions.Columns["ID_PODR"].Visible = false;
            gridPositions.Columns["Name_DOLSN"].HeaderText = "Должность";
            gridPositions.Columns["N_all"].HeaderText = "Разрешено";
            gridPositions.Columns["N_use"].HeaderText = "Работают";

            gridBrigades.Columns["ID"].Visible = false;
            gridBrigades.Columns["ID_PODR"].Visible = false;
            gridBrigades.Columns["Name_Brigad"].HeaderText = "Бригада";

            // Initial selection
            if (tableDepartments.Rows.Count > 0)
            {
                gridDepartments.CurrentCell = gridDepartments.Rows[0].Cells[0];
                UpdatePositionView();
                UpdateBrigadeView();
            }
        }

        private void UpdatePositionView()
        {
            if (gridDepartments.CurrentRow != null)
            {
                int deptId = (int)gridDepartments.CurrentRow.Cells["ID"].Value;
                viewPositions.RowFilter = $"ID_PODR = {deptId}";
                btnEditPosition.Enabled = viewPositions.Count > 0;
            }
        }

        private void UpdateBrigadeView()
        {
            if (gridDepartments.CurrentRow != null)
            {
                int deptId = (int)gridDepartments.CurrentRow.Cells["ID"].Value;
                viewBrigades.RowFilter = $"ID_PODR = {deptId}";
                btnEditBrigade.Enabled = viewBrigades.Count > 0;
            }
        }

        private void gridDepartments_SelectionChanged(object sender, EventArgs e)
        {
            if (gridDepartments.CurrentRow != null)
            {
                // Update department details
                txtDeptName.Text = gridDepartments.CurrentRow.Cells["Name_PODR"].Value.ToString();
                txtBrigadeCount.Text = gridDepartments.CurrentRow.Cells["N_BRIG"].Value.ToString();
                txtDeptDesc.Text = gridDepartments.CurrentRow.Cells["Komment"].Value.ToString();

                bool hasBrigades = Convert.ToBoolean(gridDepartments.CurrentRow.Cells["BRIG_bool"].Value);
                cmbBrigadeDivision.SelectedIndex = hasBrigades ? 1 : 0;
                txtBrigadeCount.Enabled = hasBrigades;

                // Update position and brigade views
                UpdatePositionView();
                UpdateBrigadeView();
            }
        }

        private void gridPositions_SelectionChanged(object sender, EventArgs e)
        {
            if (gridPositions.CurrentRow != null)
            {
                txtPositionName.Text = gridPositions.CurrentRow.Cells["Name_DOLSN"].Value?.ToString();
                txtPositionCount.Text = gridPositions.CurrentRow.Cells["N_all"].Value?.ToString();
                txtPositionDesc.Text = gridPositions.CurrentRow.Cells["Komment"].Value?.ToString();
            }
        }

        private void gridBrigades_SelectionChanged(object sender, EventArgs e)
        {
            if (gridBrigades.CurrentRow != null)
            {
                txtBrigadeName.Text = gridBrigades.CurrentRow.Cells["Name_Brigad"].Value?.ToString();
            }
        }

        private void cmbBrigadeDivision_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtBrigadeCount.Enabled = (cmbBrigadeDivision.SelectedIndex == 1);
            if (cmbBrigadeDivision.SelectedIndex == 0)
            {
                txtBrigadeCount.Text = "0";
            }
        }

        private void txtPositionCount_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != '\b')
            {
                e.Handled = true;
            }
        }

        private void btnAddDept_Click(object sender, EventArgs e)
        {
            if (btnAddDept.Text == "Добавить")
            {
                // Enter add mode
                btnAddDept.Text = "Сохранить";
                btnEditDept.Enabled = false;
                isAddingDepartment = true;

                txtDeptName.Enabled = true;
                txtDeptName.Text = "";
                txtBrigadeCount.Text = "0";
                cmbBrigadeDivision.SelectedIndex = 0;
                cmbBrigadeDivision.Enabled = true;
                txtDeptDesc.Enabled = true;
            }
            else
            {
                // Validate input
                if (string.IsNullOrWhiteSpace(txtDeptName.Text))
                {
                    MessageBox.Show("Вы не ввели название подразделения! Введите!");
                    return;
                }

                if (cmbBrigadeDivision.SelectedIndex == 1 &&
                    (string.IsNullOrWhiteSpace(txtBrigadeCount.Text) ||
                    Convert.ToInt32(txtBrigadeCount.Text) == 0))
                {
                    MessageBox.Show("Вы не ввели количество бригад. Введите!");
                    return;
                }

                // Save changes
                try
                {
                    if (isAddingDepartment)
                    {
                        DataRow newRow = tableDepartments.NewRow();
                        newRow["Name_PODR"] = txtDeptName.Text;
                        newRow["BRIG_bool"] = (cmbBrigadeDivision.SelectedIndex == 1);
                        newRow["N_BRIG"] = Convert.ToInt32(txtBrigadeCount.Text);
                        newRow["Komment"] = txtDeptDesc.Text;
                        tableDepartments.Rows.Add(newRow);
                    }
                    else
                    {
                        DataRow row = ((DataRowView)gridDepartments.CurrentRow.DataBoundItem).Row;
                        row.BeginEdit();
                        row["Name_PODR"] = txtDeptName.Text;
                        row["BRIG_bool"] = (cmbBrigadeDivision.SelectedIndex == 1);
                        row["N_BRIG"] = Convert.ToInt32(txtBrigadeCount.Text);
                        row["Komment"] = txtDeptDesc.Text;
                        row.EndEdit();
                    }

                    // Save to database
                    using (OleDbConnection conn = new OleDbConnection(connStr))
                    {
                        OleDbDataAdapter da = new OleDbDataAdapter(
                            "SELECT * FROM Table_Departments", conn);
                        OleDbCommandBuilder cb = new OleDbCommandBuilder(da);
                        da.Update(tableDepartments);
                    }

                    // Create brigades if needed
                    if (cmbBrigadeDivision.SelectedIndex == 1)
                    {
                        int brigadeCount = Convert.ToInt32(txtBrigadeCount.Text);
                        int deptId = Convert.ToInt32(
                            ((DataRowView)gridDepartments.CurrentRow.DataBoundItem)["ID"]);

                        // Get existing brigades
                        var existingBrigades = tableBrigades.Select($"ID_PODR = {deptId}");

                        // Create new brigades if needed
                        if (brigadeCount > existingBrigades.Length)
                        {
                            for (int i = existingBrigades.Length; i < brigadeCount; i++)
                            {
                                DataRow newBrigade = tableBrigades.NewRow();
                                newBrigade["Name_Brigad"] = $"Бригада № {i + 1}";
                                newBrigade["ID_PODR"] = deptId;
                                tableBrigades.Rows.Add(newBrigade);
                            }

                            // Save to database
                            using (OleDbConnection conn = new OleDbConnection(connStr))
                            {
                                OleDbDataAdapter da = new OleDbDataAdapter(
                                    "SELECT * FROM Table_Brigades", conn);
                                OleDbCommandBuilder cb = new OleDbCommandBuilder(da);
                                da.Update(tableBrigades);
                            }
                        }
                    }

                    // Reset UI
                    btnAddDept.Text = "Добавить";
                    txtDeptName.Enabled = false;
                    txtBrigadeCount.Enabled = false;
                    cmbBrigadeDivision.Enabled = false;
                    txtDeptDesc.Enabled = false;
                    btnEditDept.Enabled = true;
                    isAddingDepartment = false;

                    // Refresh views
                    UpdatePositionView();
                    UpdateBrigadeView();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка сохранения: {ex.Message}");
                }
            }
        }

        private void btnEditDept_Click(object sender, EventArgs e)
        {
            isAddingDepartment = false;
            btnEditDept.Enabled = false;
            btnAddDept.Text = "Сохранить";
            btnCancelDept.Enabled = true;

            txtDeptName.Enabled = true;
            cmbBrigadeDivision.Enabled = true;
            txtBrigadeCount.Enabled = (cmbBrigadeDivision.SelectedIndex == 1);
            txtDeptDesc.Enabled = true;
        }

        private void btnCancelDept_Click(object sender, EventArgs e)
        {
            // Reset department UI
            if (gridDepartments.CurrentRow != null)
            {
                txtDeptName.Text = gridDepartments.CurrentRow.Cells["Name_PODR"].Value.ToString();
                txtBrigadeCount.Text = gridDepartments.CurrentRow.Cells["N_BRIG"].Value.ToString();
                txtDeptDesc.Text = gridDepartments.CurrentRow.Cells["Komment"].Value.ToString();
                cmbBrigadeDivision.SelectedIndex =
                    Convert.ToBoolean(gridDepartments.CurrentRow.Cells["BRIG_bool"].Value) ? 1 : 0;
            }

            txtDeptName.Enabled = false;
            txtBrigadeCount.Enabled = false;
            cmbBrigadeDivision.Enabled = false;
            txtDeptDesc.Enabled = false;
            btnCancelDept.Enabled = false;
            btnAddDept.Text = "Добавить";
            btnEditDept.Enabled = true;
        }

        private void btnAddPosition_Click(object sender, EventArgs e)
        {
            if (btnAddPosition.Text == "Добавить")
            {
                // Enter add mode
                btnAddPosition.Text = "Сохранить";
                btnEditPosition.Enabled = false;
                isAddingPosition = true;

                txtPositionName.Enabled = true;
                txtPositionName.Text = "";
                txtPositionCount.Text = "";
                txtPositionDesc.Enabled = true;
            }
            else
            {
                // Validate input
                if (string.IsNullOrWhiteSpace(txtPositionName.Text))
                {
                    MessageBox.Show("Вы не ввели название должности! Введите!");
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtPositionCount.Text) ||
                    Convert.ToInt32(txtPositionCount.Text) == 0)
                {
                    MessageBox.Show("Вы не ввели разрешенное количество людей на должность! Введите!");
                    return;
                }

                // Save changes
                try
                {
                    int deptId = (int)gridDepartments.CurrentRow.Cells["ID"].Value;

                    if (isAddingPosition)
                    {
                        DataRow newRow = tablePositions.NewRow();
                        newRow["ID_PODR"] = deptId;
                        newRow["Name_DOLSN"] = txtPositionName.Text;
                        newRow["N_all"] = Convert.ToInt32(txtPositionCount.Text);
                        newRow["N_use"] = 0;
                        newRow["Komment"] = txtPositionDesc.Text;
                        tablePositions.Rows.Add(newRow);
                    }
                    else
                    {
                        DataRow row = ((DataRowView)gridPositions.CurrentRow.DataBoundItem).Row;
                        row.BeginEdit();
                        row["Name_DOLSN"] = txtPositionName.Text;
                        row["N_all"] = Convert.ToInt32(txtPositionCount.Text);
                        row["Komment"] = txtPositionDesc.Text;
                        row.EndEdit();
                    }

                    // Save to database
                    using (OleDbConnection conn = new OleDbConnection(connStr))
                    {
                        OleDbDataAdapter da = new OleDbDataAdapter(
                            "SELECT * FROM Table_Positions", conn);
                        OleDbCommandBuilder cb = new OleDbCommandBuilder(da);
                        da.Update(tablePositions);
                    }

                    // Reset UI
                    btnAddPosition.Text = "Добавить";
                    txtPositionName.Enabled = false;
                    txtPositionCount.Enabled = false;
                    txtPositionDesc.Enabled = false;
                    btnCancelPosition.Enabled = false;
                    isAddingPosition = false;

                    // Refresh view
                    UpdatePositionView();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка сохранения: {ex.Message}");
                }
            }
        }

        private void btnEditPosition_Click(object sender, EventArgs e)
        {
            isAddingPosition = false;
            btnEditPosition.Enabled = false;
            btnAddPosition.Text = "Сохранить";
            btnCancelPosition.Enabled = true;

            txtPositionName.Enabled = true;
            txtPositionCount.Enabled = true;
            txtPositionDesc.Enabled = true;
        }

        private void btnCancelPosition_Click(object sender, EventArgs e)
        {
            // Reset position UI
            if (gridPositions.CurrentRow != null)
            {
                txtPositionName.Text = gridPositions.CurrentRow.Cells["Name_DOLSN"].Value.ToString();
                txtPositionCount.Text = gridPositions.CurrentRow.Cells["N_all"].Value.ToString();
                txtPositionDesc.Text = gridPositions.CurrentRow.Cells["Komment"].Value.ToString();
            }

            txtPositionName.Enabled = false;
            txtPositionCount.Enabled = false;
            txtPositionDesc.Enabled = false;
            btnCancelPosition.Enabled = false;
            btnAddPosition.Text = "Добавить";
            btnEditPosition.Enabled = true;
        }

        private void btnEditBrigade_Click(object sender, EventArgs e)
        {
            if (btnEditBrigade.Text == "Изменить")
            {
                // Enter edit mode
                btnEditBrigade.Text = "Отмена";
                btnSaveBrigade.Enabled = true;
                txtBrigadeName.Enabled = true;
            }
            else
            {
                // Cancel edit
                btnEditBrigade.Text = "Изменить";
                btnSaveBrigade.Enabled = false;
                txtBrigadeName.Enabled = false;
                if (gridBrigades.CurrentRow != null)
                {
                    txtBrigadeName.Text = gridBrigades.CurrentRow.Cells["Name_Brigad"].Value.ToString();
                }
            }
        }

        private void btnSaveBrigade_Click(object sender, EventArgs e)
        {
            if (gridBrigades.CurrentRow != null)
            {
                try
                {
                    DataRow row = ((DataRowView)gridBrigades.CurrentRow.DataBoundItem).Row;
                    row.BeginEdit();
                    row["Name_Brigad"] = txtBrigadeName.Text;
                    row.EndEdit();

                    // Save to database
                    using (OleDbConnection conn = new OleDbConnection(connStr))
                    {
                        OleDbDataAdapter da = new OleDbDataAdapter(
                            "SELECT * FROM Table_Brigades", conn);
                        OleDbCommandBuilder cb = new OleDbCommandBuilder(da);
                        da.Update(tableBrigades);
                    }

                    // Reset UI
                    btnSaveBrigade.Enabled = false;
                    btnEditBrigade.Text = "Изменить";
                    txtBrigadeName.Enabled = false;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка сохранения: {ex.Message}");
                }
            }
        }
    }
}