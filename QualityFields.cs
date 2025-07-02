using System;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Program_na_Ryadam
{
    public partial class QualityFields : UserControl
    {
        private string dbPath = Path.Combine(Application.StartupPath, "Database", "Database_XN1.mdb");
        private OleDbConnection connection;
        private DataTable dataTable;
        private int maxOrder = 1;
        private int tempOrder;
        private bool isAdding = false;
        private bool isEditing = false;

        public QualityFields()
        {
            InitializeComponent();
            SetupDatabaseConnection();
            InitializeComponents();
        }

        private void SetupDatabaseConnection()
        {
            string connStr = $"Provider=Microsoft.Jet.OLEDB.4.0;Data Source={dbPath}";
            connection = new OleDbConnection(connStr);
        }

        private void InitializeComponents()
        {
            // Main layout
            Size = new Size(835, 397);
            Dock = DockStyle.Fill;

            // DataGridView setup
            dataGridViewQuality = new DataGridView();
            dataGridViewQuality.Dock = DockStyle.Fill;
            dataGridViewQuality.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewQuality.CellClick += DataGridViewQuality_CellClick;
            dataGridViewQuality.AllowUserToAddRows = false;

            // Bottom Panel
            Panel panelBottom = new Panel();
            panelBottom.Dock = DockStyle.Bottom;
            panelBottom.Height = 186;

            // GroupBoxes
            GroupBox groupBoxLeft = new GroupBox();
            groupBoxLeft.Text = "Изменение данных";
            groupBoxLeft.Dock = DockStyle.Fill;
            groupBoxLeft.Font = new Font("Times New Roman", 12);

            GroupBox groupBoxRight = new GroupBox();
            groupBoxRight.Text = "Выбор";
            groupBoxRight.Dock = DockStyle.Right;
            groupBoxRight.Width = 298;
            groupBoxRight.Font = new Font("Times New Roman", 12);

            // Controls for right groupbox
            Label labelDepartment = new Label();
            labelDepartment.Text = "Подразделение:";
            labelDepartment.Location = new Point(16, 32);
            labelDepartment.Size = new Size(114, 19);

            comboBoxDepartment = new ComboBox();
            comboBoxDepartment.Items.AddRange(new object[] {
                "Оранжерейный комплекс",
                "Сортировочный отдел"
            });
            comboBoxDepartment.SelectedIndex = 0;
            comboBoxDepartment.Location = new Point(64, 57);
            comboBoxDepartment.Size = new Size(225, 27);

            buttonLoad = new Button();
            buttonLoad.Text = "Загрузить";
            buttonLoad.Location = new Point(190, 152);
            buttonLoad.Size = new Size(105, 28);
            buttonLoad.Click += ButtonLoad_Click;

            // Controls for left groupbox
            richTextBoxName = new RichTextBox();
            richTextBoxName.Dock = DockStyle.Top;
            richTextBoxName.Height = 100;

            Label labelPercent = new Label();
            labelPercent.Text = "Максимальное количество Премии в %";
            labelPercent.Location = new Point(16, 127);
            labelPercent.Size = new Size(287, 19);

            textBoxMaxPercent = new TextBox();
            textBoxMaxPercent.Location = new Point(412, 121);
            textBoxMaxPercent.Size = new Size(121, 27);
            textBoxMaxPercent.KeyPress += TextBoxMaxPercent_KeyPress;

            // Button panel
            Panel buttonPanel = new Panel();
            buttonPanel.Dock = DockStyle.Bottom;
            buttonPanel.Height = 30;

            buttonAdd = new Button();
            buttonAdd.Text = "Добавить";
            buttonAdd.Dock = DockStyle.Right;
            buttonAdd.Width = 99;
            buttonAdd.Enabled = false;
            buttonAdd.Click += ButtonAdd_Click;

            buttonSave = new Button();
            buttonSave.Text = "Сохранить";
            buttonSave.Dock = DockStyle.Right;
            buttonSave.Width = 99;
            buttonSave.Enabled = false;
            buttonSave.Click += ButtonSave_Click;

            buttonEdit = new Button();
            buttonEdit.Text = "Изменить";
            buttonEdit.Dock = DockStyle.Right;
            buttonEdit.Width = 99;
            buttonEdit.Enabled = false;
            buttonEdit.Click += ButtonEdit_Click;

            buttonCancel = new Button();
            buttonCancel.Text = "Отмена";
            buttonCancel.Dock = DockStyle.Right;
            buttonCancel.Width = 99;
            buttonCancel.Enabled = false;
            buttonCancel.Click += ButtonCancel_Click;

            // Context Menu
            contextMenuGrid = new ContextMenuStrip();
            ToolStripMenuItem moveUpItem = new ToolStripMenuItem("Вверх в списке");
            moveUpItem.Click += MoveUpItem_Click;
            ToolStripMenuItem moveDownItem = new ToolStripMenuItem("Вниз в списке");
            moveDownItem.Click += MoveDownItem_Click;
            ToolStripMenuItem deleteItem = new ToolStripMenuItem("Удалить");
            deleteItem.Click += DeleteItem_Click;
            
            contextMenuGrid.Items.AddRange(new ToolStripItem[] { moveUpItem, moveDownItem, deleteItem });
            dataGridViewQuality.ContextMenuStrip = contextMenuGrid;

            // Assemble controls
            buttonPanel.Controls.Add(buttonSave);
            buttonPanel.Controls.Add(buttonCancel);
            buttonPanel.Controls.Add(buttonEdit);
            buttonPanel.Controls.Add(buttonAdd);

            groupBoxLeft.Controls.Add(richTextBoxName);
            groupBoxLeft.Controls.Add(labelPercent);
            groupBoxLeft.Controls.Add(textBoxMaxPercent);
            groupBoxLeft.Controls.Add(buttonPanel);

            groupBoxRight.Controls.Add(labelDepartment);
            groupBoxRight.Controls.Add(comboBoxDepartment);
            groupBoxRight.Controls.Add(buttonLoad);

            panelBottom.Controls.Add(groupBoxLeft);
            panelBottom.Controls.Add(groupBoxRight);

            Controls.Add(dataGridViewQuality);
            Controls.Add(panelBottom);
        }

        private void ButtonLoad_Click(object sender, EventArgs e)
        {
            try
            {
                string query = $@"SELECT * FROM Tabel_KACH 
                                WHERE Podr = {comboBoxDepartment.SelectedIndex + 1} 
                                ORDER BY Porad";
                
                OleDbDataAdapter adapter = new OleDbDataAdapter(query, connection);
                dataTable = new DataTable();
                adapter.Fill(dataTable);

                dataGridViewQuality.DataSource = dataTable;
                buttonAdd.Enabled = true;
                buttonCancel.Enabled = false;
                buttonEdit.Enabled = dataTable.Rows.Count > 0;

                // Calculate max order number
                maxOrder = 1;
                foreach (DataRow row in dataTable.Rows)
                {
                    int currentOrder = Convert.ToInt32(row["Porad"]);
                    if (currentOrder >= maxOrder)
                        maxOrder = currentOrder + 1;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки: {ex.Message}");
            }
        }

        private void ButtonAdd_Click(object sender, EventArgs e)
        {
            isAdding = true;
            richTextBoxName.Clear();
            textBoxMaxPercent.Clear();
            buttonSave.Enabled = true;
            buttonCancel.Enabled = true;
            buttonEdit.Enabled = false;
            buttonAdd.Enabled = false;
            tempOrder = dataTable.Rows.Count + 1;
        }

        private void ButtonSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(richTextBoxName.Text))
            {
                MessageBox.Show("Введите название премии");
                return;
            }

            if (!int.TryParse(textBoxMaxPercent.Text, out int maxPercent) || maxPercent <= 0)
            {
                MessageBox.Show("Введите разрешенное количество процентов премии");
                return;
            }

            try
            {
                if (isAdding)
                {
                    DataRow newRow = dataTable.NewRow();
                    newRow["Name_Prem"] = richTextBoxName.Text;
                    newRow["Max_procent"] = maxPercent;
                    newRow["Podr"] = comboBoxDepartment.SelectedIndex + 1;
                    newRow["Porad"] = tempOrder;
                    dataTable.Rows.Add(newRow);
                }
                else if (isEditing)
                {
                    int index = dataGridViewQuality.CurrentRow.Index;
                    dataTable.Rows[index]["Name_Prem"] = richTextBoxName.Text;
                    dataTable.Rows[index]["Max_procent"] = maxPercent;
                }

                SaveToDatabase();
                ResetState();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сохранения: {ex.Message}");
            }
        }

        private void SaveToDatabase()
        {
            using (OleDbDataAdapter adapter = new OleDbDataAdapter("SELECT * FROM Tabel_KACH", connection))
            {
                OleDbCommandBuilder builder = new OleDbCommandBuilder(adapter);
                adapter.Update(dataTable);
            }
        }

        private void ResetState()
        {
            isAdding = false;
            isEditing = false;
            buttonSave.Enabled = false;
            buttonCancel.Enabled = false;
            buttonAdd.Enabled = true;
            buttonEdit.Enabled = dataTable.Rows.Count > 0;
        }

        private void ButtonEdit_Click(object sender, EventArgs e)
        {
            if (dataGridViewQuality.CurrentRow == null) return;

            isEditing = true;
            tempOrder = Convert.ToInt32(
                dataGridViewQuality.CurrentRow.Cells["Porad"].Value);
            buttonSave.Enabled = true;
            buttonCancel.Enabled = true;
            buttonAdd.Enabled = false;
            buttonEdit.Enabled = false;
        }

        private void ButtonCancel_Click(object sender, EventArgs e)
        {
            ResetState();
            dataTable.RejectChanges();
            if (dataTable.Rows.Count > 0)
            {
                buttonEdit.Enabled = true;
            }
        }

        private void DataGridViewQuality_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            richTextBoxName.Text = dataGridViewQuality.Rows[e.RowIndex]
                .Cells["Name_Prem"].Value.ToString();
            textBoxMaxPercent.Text = dataGridViewQuality.Rows[e.RowIndex]
                .Cells["Max_procent"].Value.ToString();
        }

        private void TextBoxMaxPercent_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != '\b')
                e.Handled = true;
        }

        private void MoveUpItem_Click(object sender, EventArgs e)
        {
            MoveRow(-1);
        }

        private void MoveDownItem_Click(object sender, EventArgs e)
        {
            MoveRow(1);
        }

        private void MoveRow(int direction)
        {
            if (dataGridViewQuality.CurrentRow == null) return;

            int currentIndex = dataGridViewQuality.CurrentRow.Index;
            int newIndex = currentIndex + direction;

            if (newIndex < 0 || newIndex >= dataGridViewQuality.Rows.Count)
                return;

            DataRow currentRow = dataTable.Rows[currentIndex];
            DataRow neighborRow = dataTable.Rows[newIndex];

            // Swap order values
            int currentOrder = Convert.ToInt32(currentRow["Porad"]);
            int neighborOrder = Convert.ToInt32(neighborRow["Porad"]);

            currentRow["Porad"] = neighborOrder;
            neighborRow["Porad"] = currentOrder;

            // Save changes
            SaveToDatabase();
            ButtonLoad_Click(null, EventArgs.Empty);
            dataGridViewQuality.CurrentCell = dataGridViewQuality.Rows[newIndex].Cells[0];
        }

        private void DeleteItem_Click(object sender, EventArgs e)
        {
            if (dataGridViewQuality.CurrentRow == null) return;

            dataGridViewQuality.Rows.RemoveAt(dataGridViewQuality.CurrentRow.Index);
            SaveToDatabase();

            // Renumber remaining rows
            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                dataTable.Rows[i]["Porad"] = i + 1;
            }

            SaveToDatabase();
            ButtonLoad_Click(null, EventArgs.Empty);

            if (dataTable.Rows.Count == 0)
            {
                richTextBoxName.Clear();
                textBoxMaxPercent.Clear();
                buttonEdit.Enabled = false;
            }
        }

        // Component declarations
        private DataGridView dataGridViewQuality;
        private ComboBox comboBoxDepartment;
        private Button buttonLoad;
        private RichTextBox richTextBoxName;
        private TextBox textBoxMaxPercent;
        private Button buttonAdd;
        private Button buttonSave;
        private Button buttonEdit;
        private Button buttonCancel;
        private ContextMenuStrip contextMenuGrid;
    }
}