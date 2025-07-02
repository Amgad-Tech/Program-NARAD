using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Program_na_Ryadam
{
        public partial class F_add_w_all : Form
        {
            // Form controls
            private TextBox txtLastName, txtFirstName, txtPatronymic, txtCoefficient;
            private ComboBox cmbGender, cmbResidence, cmbDepartment, cmbPosition, cmbBrigade;
            private Button btnSave, btnCancel, btnFire;
            private Label lblTitle, lblLastName, lblFirstName, lblPatronymic, lblGender;
            private Label lblResidence, lblDepartment, lblPosition, lblBrigade, lblCoefficient;

            // Database
            private readonly string connStr;
            private OleDbConnection dbConn;

            // State
            private bool editMode;
            private int workerId;
            private List<ComboboxItem> departmentItems = new List<ComboboxItem>();
            private List<ComboboxItem> positionItems = new List<ComboboxItem>();
            private List<ComboboxItem> brigadeItems = new List<ComboboxItem>();

            public F_add_w_all(string databasePath)
            {
                connStr = $"Provider=Microsoft.Jet.OLEDB.4.0;Data Source={databasePath}";
                InitializeComponents();
                LoadDepartments();
                this.Text = "Добавление работника";
                this.Size = new Size(650, 580);
                this.StartPosition = FormStartPosition.CenterScreen;
                this.FormBorderStyle = FormBorderStyle.FixedDialog;
            }

            public void SetMode(bool isEditMode, int id = 0)
            {
                editMode = isEditMode;
                workerId = id;
                Text = editMode ? "Редактирование работника" : "Добавление нового работника";
                btnFire.Visible = editMode;

                if (editMode)
                {
                    LoadWorkerData();
                }
                else
                {
                    ClearForm();
                }
            }

            private void InitializeComponents()
            {
                // Form setup
                this.Padding = new Padding(10);
                this.Font = new Font("Times New Roman", 12F);

                // Title label
                lblTitle = new Label
                {
                    Text = "Добавить нового работника",
                    Dock = DockStyle.Top,
                    TextAlign = ContentAlignment.MiddleCenter,
                    Font = new Font("Times New Roman", 18, FontStyle.Bold),
                    Height = 50
                };

                // Main table layout
                TableLayoutPanel mainTable = new TableLayoutPanel
                {
                    Dock = DockStyle.Fill,
                    ColumnCount = 2,
                    RowCount = 10,
                    CellBorderStyle = TableLayoutPanelCellBorderStyle.None,
                    Padding = new Padding(5)
                };
                mainTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F));
                mainTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 70F));

                // Add rows
                for (int i = 0; i < 9; i++)
                {
                    mainTable.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
                }
                mainTable.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

                // Last Name
                lblLastName = new Label { Text = "Фамилия:", TextAlign = ContentAlignment.MiddleRight };
                txtLastName = new TextBox();
                mainTable.Controls.Add(lblLastName, 0, 0);
                mainTable.Controls.Add(txtLastName, 1, 0);

                // First Name
                lblFirstName = new Label { Text = "Имя:", TextAlign = ContentAlignment.MiddleRight };
                txtFirstName = new TextBox();
                mainTable.Controls.Add(lblFirstName, 0, 1);
                mainTable.Controls.Add(txtFirstName, 1, 1);

                // Patronymic
                lblPatronymic = new Label { Text = "Отчество:", TextAlign = ContentAlignment.MiddleRight };
                txtPatronymic = new TextBox();
                mainTable.Controls.Add(lblPatronymic, 0, 2);
                mainTable.Controls.Add(txtPatronymic, 1, 2);

                // Gender
                lblGender = new Label { Text = "Пол:", TextAlign = ContentAlignment.MiddleRight };
                cmbGender = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList };
                cmbGender.Items.AddRange(new object[] { "мужской", "женский" });
                cmbGender.SelectedIndex = 0;
                mainTable.Controls.Add(lblGender, 0, 3);
                mainTable.Controls.Add(cmbGender, 1, 3);

                // Residence
                lblResidence = new Label { Text = "Место жительства:", TextAlign = ContentAlignment.MiddleRight };
                cmbResidence = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList };
                cmbResidence.Items.AddRange(new object[] { "Город", "Район" });
                cmbResidence.SelectedIndex = 0;
                mainTable.Controls.Add(lblResidence, 0, 4);
                mainTable.Controls.Add(cmbResidence, 1, 4);

                // Department
                lblDepartment = new Label { Text = "Подразделение:", TextAlign = ContentAlignment.MiddleRight };
                cmbDepartment = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList };
                cmbDepartment.SelectedIndexChanged += DepartmentChanged;
                mainTable.Controls.Add(lblDepartment, 0, 5);
                mainTable.Controls.Add(cmbDepartment, 1, 5);

                // Position
                lblPosition = new Label { Text = "Должность:", TextAlign = ContentAlignment.MiddleRight };
                cmbPosition = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Enabled = false };
                mainTable.Controls.Add(lblPosition, 0, 6);
                mainTable.Controls.Add(cmbPosition, 1, 6);

                // Brigade
                lblBrigade = new Label { Text = "Бригада:", TextAlign = ContentAlignment.MiddleRight, Visible = false };
                cmbBrigade = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Visible = false };
                mainTable.Controls.Add(lblBrigade, 0, 7);
                mainTable.Controls.Add(cmbBrigade, 1, 7);

                // Coefficient
                lblCoefficient = new Label { Text = "Коэффициент:", TextAlign = ContentAlignment.MiddleRight };
                txtCoefficient = new TextBox();
                txtCoefficient.KeyPress += CoefficientKeyPress;
                mainTable.Controls.Add(lblCoefficient, 0, 8);
                mainTable.Controls.Add(txtCoefficient, 1, 8);

                // Button panel
                Panel buttonPanel = new Panel { Dock = DockStyle.Bottom, Height = 50 };
                btnCancel = new Button { Text = "Отмена", Size = new Size(100, 35), Anchor = AnchorStyles.Right };
                btnSave = new Button { Text = "Сохранить", Size = new Size(120, 35), Anchor = AnchorStyles.Right };
                btnFire = new Button { Text = "Уволить", Size = new Size(100, 35), Anchor = AnchorStyles.Left, Visible = false };

                btnCancel.Click += (s, e) => this.DialogResult = DialogResult.Cancel;
                btnSave.Click += SaveWorker;
                btnFire.Click += FireWorker;

                buttonPanel.Controls.Add(btnFire);
                buttonPanel.Controls.Add(btnCancel);
                buttonPanel.Controls.Add(btnSave);
                btnCancel.Left = buttonPanel.Width - btnCancel.Width - 10;
                btnSave.Left = btnCancel.Left - btnSave.Width - 10;
                btnFire.Left = 10;

                // Add controls to form
                this.Controls.Add(mainTable);
                this.Controls.Add(lblTitle);
                this.Controls.Add(buttonPanel);
            }

        private void LoadDepartments()
        {
            using (dbConn = new OleDbConnection(connStr))
            {
                dbConn.Open();
                // Corrected column name: Name_PODR instead of Name_PCOR
                var cmd = new OleDbCommand("SELECT ID, Name_PODR FROM Name_PODRASD", dbConn);

                using (var reader = cmd.ExecuteReader())
                {
                    departmentItems.Clear();
                    cmbDepartment.Items.Clear();

                    while (reader.Read())
                    {
                        var item = new ComboboxItem(
                            reader["Name_PODR"].ToString(),
                            Convert.ToInt32(reader["ID"])
                        );
                        departmentItems.Add(item);
                        cmbDepartment.Items.Add(item);
                    }
                }
            }
            // Set default selection if items exist
            if (cmbDepartment.Items.Count > 0)
                cmbDepartment.SelectedIndex = 0;
        }

        private void DepartmentChanged(object sender, EventArgs e)
            {
                if (cmbDepartment.SelectedItem is ComboboxItem selectedDept)
                {
                    LoadPositions((int)selectedDept.Value);
                    LoadBrigades((int)selectedDept.Value);
                }
            }

            private void LoadPositions(int departmentId)
            {
                positionItems.Clear();
                cmbPosition.Items.Clear();
                cmbPosition.Enabled = false;
                cmbPosition.Text = "Выбор должности";

                using (dbConn = new OleDbConnection(connStr))
                {
                    dbConn.Open();
                    string query = "SELECT ID, Name_DOLSN FROM Dolsn_PODR " +
                                  $"WHERE (N_all > N_use OR ID = {workerId}) " +
                                  $"AND ID_PODR = {departmentId}";

                    var cmd = new OleDbCommand(query, dbConn);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (!reader.HasRows)
                        {
                            cmbPosition.Text = "Нет свободных должностей";
                            return;
                        }

                        while (reader.Read())
                        {
                            var item = new ComboboxItem(
                                reader["Name_DOLSN"].ToString(),
                                Convert.ToInt32(reader["ID"])
                            );
                            positionItems.Add(item);
                            cmbPosition.Items.Add(item);
                        }
                        cmbPosition.Enabled = true;
                        cmbPosition.SelectedIndex = 0;
                    }
                }
            }

            private void LoadBrigades(int departmentId)
            {
                brigadeItems.Clear();
                cmbBrigade.Items.Clear();
                lblBrigade.Visible = cmbBrigade.Visible = false;

                using (dbConn = new OleDbConnection(connStr))
                {
                    dbConn.Open();
                    var cmd = new OleDbCommand(
                        $"SELECT ID, Name_Brigad FROM Name_BRIGADA WHERE ID_PODR = {departmentId}",
                        dbConn);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (!reader.HasRows) return;

                        while (reader.Read())
                        {
                            var item = new ComboboxItem(
                                reader["Name_Brigad"].ToString(),
                                Convert.ToInt32(reader["ID"])
                            );
                            brigadeItems.Add(item);
                            cmbBrigade.Items.Add(item);
                        }
                        lblBrigade.Visible = cmbBrigade.Visible = true;
                        cmbBrigade.SelectedIndex = 0;
                    }
                }
            }

        private void LoadWorkerData()
        {
            using (dbConn = new OleDbConnection(connStr))
            {
                dbConn.Open();
                // Select specific columns to ensure we get all needed data
                var cmd = new OleDbCommand(
                    $"SELECT ID, Fam, Imj, Otc, Pol, Mesto_sitel, Podr, Podr_str, Dolsn, Dolsn_str, Brigada, Brigada_str, coff " +
                    $"FROM BD_WORKING_ALL WHERE ID = {workerId}",
                    dbConn);

                using (var reader = cmd.ExecuteReader(CommandBehavior.SingleRow))
                {
                    if (!reader.Read()) return;

                    // Personal info
                    txtLastName.Text = reader["Fam"].ToString();
                    txtFirstName.Text = reader["Imj"].ToString();
                    txtPatronymic.Text = reader["Otc"].ToString();
                    cmbGender.SelectedIndex = (bool)reader["Pol"] ? 0 : 1;
                    cmbResidence.SelectedIndex = (bool)reader["Mesto_sitel"] ? 0 : 1;
                    txtCoefficient.Text = reader["coff"].ToString();

                    // Department
                    int departmentId = Convert.ToInt32(reader["Podr"]);
                    string deptName = reader["Podr_str"].ToString();

                    // Find or create combobox item
                    var deptItem = departmentItems.FirstOrDefault(i => (int)i.Value == departmentId);
                    if (deptItem == null)
                    {
                        deptItem = new ComboboxItem(deptName, departmentId);
                        departmentItems.Add(deptItem);
                        cmbDepartment.Items.Add(deptItem);
                    }
                    cmbDepartment.SelectedItem = deptItem;

                    // Load dependent data
                    LoadPositions(departmentId);
                    LoadBrigades(departmentId);

                    // Position
                    int positionId = Convert.ToInt32(reader["Dolsn"]);
                    string positionName = reader["Dolsn_str"].ToString();

                    var posItem = positionItems.FirstOrDefault(i => (int)i.Value == positionId);
                    if (posItem == null)
                    {
                        posItem = new ComboboxItem(positionName, positionId);
                        positionItems.Add(posItem);
                        cmbPosition.Items.Add(posItem);
                    }
                    cmbPosition.SelectedItem = posItem;

                    // Brigade
                    if (!reader.IsDBNull(reader.GetOrdinal("Brigada")))
                    {
                        int brigadeId = Convert.ToInt32(reader["Brigada"]);
                        string brigadeName = reader["Brigada_str"].ToString();

                        var brigItem = brigadeItems.FirstOrDefault(i => (int)i.Value == brigadeId);
                        if (brigItem == null)
                        {
                            brigItem = new ComboboxItem(brigadeName, brigadeId);
                            brigadeItems.Add(brigItem);
                            cmbBrigade.Items.Add(brigItem);
                        }
                        cmbBrigade.SelectedItem = brigItem;
                    }
                }
            }
        }

        private void SaveWorker(object sender, EventArgs e)
            {
                // Validation
                if (string.IsNullOrWhiteSpace(txtLastName.Text))
                {
                    ShowError("Вы не ввели Фамилию рабочего!");
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtFirstName.Text))
                {
                    ShowError("Вы не ввели Имя рабочего!");
                    return;
                }

                if (cmbDepartment.SelectedItem == null)
                {
                    ShowError("Выберите подразделение!");
                    return;
                }

                if (cmbPosition.SelectedItem == null)
                {
                    ShowError("Выберите должность!");
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtCoefficient.Text) || !float.TryParse(txtCoefficient.Text, out _))
                {
                    ShowError("Введите корректный коэффициент!");
                    return;
                }

            try
            {
                using (dbConn = new OleDbConnection(connStr))
                {
                    dbConn.Open();
                    OleDbCommand cmd;

                    // Get values
                    string deptName = cmbDepartment.Text;
                    string positionName = cmbPosition.Text;
                    string brigadeName = cmbBrigade.Visible ? cmbBrigade.Text : null;
                    object brigadeId = cmbBrigade.Visible ?
                        ((ComboboxItem)cmbBrigade.SelectedItem).Value : DBNull.Value;
                    object brigadeNameParam = !string.IsNullOrEmpty(brigadeName) ? (object)brigadeName : DBNull.Value;

                    if (editMode)
                    {
                        cmd = new OleDbCommand(
                            "UPDATE BD_WORKING_ALL SET " +
                            "Fam = ?, Imj = ?, Otc = ?, " +
                            "Pol = ?, Mesto_sitel = ?, " +
                            "Podr = ?, Podr_str = ?, " +
                            "Dolsn = ?, Dolsn_str = ?, " +
                            "Brigada = ?, Brigada_str = ?, " +
                            "coff = ? " +
                            "WHERE ID = ?",  // 13 parameters total
                            dbConn);

                        // Add parameters in EXACT order as in SQL
                        cmd.Parameters.AddWithValue("Fam", txtLastName.Text);
                        cmd.Parameters.AddWithValue("Imj", txtFirstName.Text);
                        cmd.Parameters.AddWithValue("Otc", txtPatronymic.Text);
                        cmd.Parameters.AddWithValue("Pol", cmbGender.SelectedIndex == 0);
                        cmd.Parameters.AddWithValue("Mesto_sitel", cmbResidence.SelectedIndex == 0);
                        cmd.Parameters.AddWithValue("Podr", ((ComboboxItem)cmbDepartment.SelectedItem).Value);
                        cmd.Parameters.AddWithValue("Podr_str", deptName);
                        cmd.Parameters.AddWithValue("Dolsn", ((ComboboxItem)cmbPosition.SelectedItem).Value);
                        cmd.Parameters.AddWithValue("Dolsn_str", positionName);
                        cmd.Parameters.AddWithValue("Brigada", brigadeId);
                        cmd.Parameters.AddWithValue("Brigada_str", brigadeNameParam);
                        cmd.Parameters.AddWithValue("coff", Convert.ToSingle(txtCoefficient.Text));
                        cmd.Parameters.AddWithValue("ID", workerId);  // WHERE clause parameter
                    }
                    else
                    {
                        cmd = new OleDbCommand(
                            "INSERT INTO BD_WORKING_ALL " +
                            "(Fam, Imj, Otc, Pol, Mesto_sitel, " +
                            "Podr, Podr_str, Dolsn, Dolsn_str, " +
                            "Brigada, Brigada_str, coff, Fired) " +
                            "VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, false)",
                            dbConn);

                        // Add parameters in EXACT order as in SQL
                        cmd.Parameters.AddWithValue("Fam", txtLastName.Text);
                        cmd.Parameters.AddWithValue("Imj", txtFirstName.Text);
                        cmd.Parameters.AddWithValue("Otc", txtPatronymic.Text);
                        cmd.Parameters.AddWithValue("Pol", cmbGender.SelectedIndex == 0);
                        cmd.Parameters.AddWithValue("Mesto_sitel", cmbResidence.SelectedIndex == 0);
                        cmd.Parameters.AddWithValue("Podr", ((ComboboxItem)cmbDepartment.SelectedItem).Value);
                        cmd.Parameters.AddWithValue("Podr_str", deptName);
                        cmd.Parameters.AddWithValue("Dolsn", ((ComboboxItem)cmbPosition.SelectedItem).Value);
                        cmd.Parameters.AddWithValue("Dolsn_str", positionName);
                        cmd.Parameters.AddWithValue("Brigada", brigadeId);
                        cmd.Parameters.AddWithValue("Brigada_str", brigadeNameParam);
                        cmd.Parameters.AddWithValue("coff", Convert.ToSingle(txtCoefficient.Text));
                    }

                    cmd.ExecuteNonQuery();
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сохранения: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

            private void FireWorker(object sender, EventArgs e)
            {
                if (MessageBox.Show("Вы действительно хотите уволить работника?",
                    "Подтверждение",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    try
                    {
                        using (dbConn = new OleDbConnection(connStr))
                        {
                            dbConn.Open();
                            var cmd = new OleDbCommand(
                                $"UPDATE BD_WORKING_ALL SET Fired = true WHERE ID = {workerId}",
                                dbConn);
                            cmd.ExecuteNonQuery();
                        }
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }

            private void CoefficientKeyPress(object sender, KeyPressEventArgs e)
            {
                if (!char.IsDigit(e.KeyChar) && e.KeyChar != ',' && e.KeyChar != '\b')
                {
                    e.Handled = true;
                }
            }

            private void ShowError(string message)
            {
                MessageBox.Show(message, "Ошибка ввода",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            private void ClearForm()
            {
                txtLastName.Clear();
                txtFirstName.Clear();
                txtPatronymic.Clear();
                cmbGender.SelectedIndex = 0;
                cmbResidence.SelectedIndex = 0;
                txtCoefficient.Clear();
                cmbPosition.Enabled = false;
                lblBrigade.Visible = cmbBrigade.Visible = false;
            }
        }

    public class ComboboxItem
    {
        public string Text { get; set; }
        public object Value { get; set; }

        public ComboboxItem() { }

        public ComboboxItem(string text, object value)
        {
            Text = text;
            Value = value;
        }

        public override string ToString() => Text;
    }


}
