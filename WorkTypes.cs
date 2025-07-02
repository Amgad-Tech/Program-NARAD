using System;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace Program_na_Ryadam
{
    //  Дорогой следующий программист,
    //
    //
    //     Когда я Этот код писал, только я и Бог знали, как он работает.
    //
    //     Теперь пусть Бог будет с тобой — потому что теперь только Он один знает, что тут вообще происходит.
    //
    // 
    public partial class WorkTypes : UserControl
    {
        // Layout constants
        private const int GridMargin = 50;
        private const int ControlMargin = 15;
        private const int ButtonWidth = 80;
        private const int ButtonHeight = 32;
        private const int ComboBoxWidth = 220;
        private const int TextBoxWidth = 200;
        private const int LabelWidth = 120;
        private readonly int rowHeight = 30;
        private const int FontSize = 12;
        private readonly Font controlFont = new Font("Times New Roman", FontSize);

        // Database fields
        private string dbPath = Path.Combine(Application.StartupPath, "Database", "Database_XN1.mdb");
        private string connStr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0}";

        // Data storage
        private int[] departmentIds = new int[50];
        private int[] brigadeIds = new int[20];
        private int[] cultureIds = new int[30];
        private int all_ism = 0; // 0=none, 1=add, 2=edit

        // UI Controls
        private DataGridView dgvWorks;
        private ComboBox cbWorkType, cbCulture, cbDepartment, cbBrigade, cbFilterCulture;
        private TextBox txtName, txtPrice, txtSpen, txtGradki, txtKrug, txtSum, txtOtdel;
        private CheckBox chbComboWork, chbSquare, chbCalcType, chbHasOtdel, chbGlobal;
        private Button btnAdd, btnEdit, btnDelete, btnSave, btnDetailsCancel, btnRefresh, btnExcel, btnKoef;
        private Panel panelDetails, toolbarPanel;
        private Button btnToolbarCancel;
        private TextBox txtWeek;
        private ComboBox cbCirclePeriod;

        public WorkTypes()
        {
            InitializeComponent();
            this.Resize += WorkTypes_Resize;
            this.AutoSize = false;
            this.AutoSizeMode = AutoSizeMode.GrowOnly;
            InitializeUI();
            LoadInitialData();
            ApplyTheme();
            WireUpEventHandlers();
        }

        //fixing the problem with size minimizing everytime
        private void WorkTypes_Resize(object sender, EventArgs e)
        {
            if (dgvWorks != null)
            {
                dgvWorks.Width = this.Width - (2 * GridMargin);

                toolbarPanel.Width = this.Width - (2 * GridMargin);
                panelDetails.Width = this.Width - (2 * GridMargin);
                panelDetails.Height = 600;
            }
            
        }
        private void InitializeUI()
        {
            this.SuspendLayout();
            this.BackColor = ThemeManager.CurrentTheme == AppTheme.Dark ?
                Color.FromArgb(30, 30, 30) : SystemColors.Control;

            int leftCol1 = ControlMargin;
            int leftCol2 = leftCol1 + ComboBoxWidth + ControlMargin;

            // 1. Main DataGridView
            dgvWorks = new DataGridView()
            {
                Location = new Point(GridMargin, GridMargin),
                Size = new Size(this.Width - (2 * GridMargin), 255),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
                Height = 255,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false,
                RowHeadersVisible = false
            };
            this.Controls.Add(dgvWorks);

            // 2. Toolbar Panel
            toolbarPanel = new Panel()
            {
                Location = new Point(GridMargin, dgvWorks.Bottom + ControlMargin),
                Size = new Size(this.Width - (2 * GridMargin), ButtonHeight + 10),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };

            // Toolbar buttons
            int buttonLeft = 0;
            btnAdd = CreateButton("Добавить", ref buttonLeft, toolbarPanel);
            btnEdit = CreateButton("Изменить", ref buttonLeft, toolbarPanel);
            btnDelete = CreateButton("Удалить", ref buttonLeft, toolbarPanel);
            btnKoef = CreateButton("Коэф.", ref buttonLeft, toolbarPanel);
            btnToolbarCancel = CreateButton("Отмена", ref buttonLeft, toolbarPanel);
            btnExcel = CreateButton("Excel", ref buttonLeft, toolbarPanel);
            btnRefresh = CreateButton("Обновить", ref buttonLeft, toolbarPanel);

            // Culture filter
            cbFilterCulture = new ComboBox()
            {
                Location = new Point(buttonLeft + ControlMargin, 5),
                Size = new Size(ComboBoxWidth, ButtonHeight),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            toolbarPanel.Controls.Add(cbFilterCulture);
            this.Controls.Add(toolbarPanel);

            // 3. Details Panel
            panelDetails = new Panel()
            {
                Location = new Point(GridMargin, toolbarPanel.Bottom + ControlMargin),
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right,
                BackColor = ThemeManager.CurrentTheme == AppTheme.Dark ?
                    Color.FromArgb(45, 45, 48) : SystemColors.Window
            };

            dgvWorks.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvWorks.RowPrePaint += DgvWorks_RowPrePaint;
            dgvWorks.CellFormatting += DgvWorks_CellFormatting;

            txtSum = new TextBox()
            {
                Location = new Point(leftCol1 + 900, ControlMargin + 900 + rowHeight * 10),
                Size = new Size(100, ButtonHeight),
                Enabled = false,
                Visible = true
            };
            panelDetails.Controls.Add(txtSum);

            InitializeDetailsPanel();
            this.Controls.Add(panelDetails);
            this.ResumeLayout(false);
        }

        private void DgvWorks_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                if (dgvWorks.Rows[e.RowIndex].Selected)
                {
                    e.CellStyle.SelectionBackColor = Color.LightBlue;
                    e.CellStyle.SelectionForeColor = Color.Black;
                }
            }
        }

        private void DgvWorks_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var row = dgvWorks.Rows[e.RowIndex];
                if (row.Selected)
                {
                    row.DefaultCellStyle.BackColor = Color.LightBlue;
                    row.DefaultCellStyle.ForeColor = Color.Black;
                }
                else
                {
                    row.DefaultCellStyle.BackColor = dgvWorks.DefaultCellStyle.BackColor;
                    row.DefaultCellStyle.ForeColor = dgvWorks.DefaultCellStyle.ForeColor;
                }
            }
        }

        private Button CreateButton(string text, ref int left, Panel parent)
        {
            var btn = new Button()
            {
                Text = text,
                Location = new Point(left, 5),
                Size = new Size(ButtonWidth, ButtonHeight),
                Tag = text
            };
            parent.Controls.Add(btn);
            left += ButtonWidth + ControlMargin;
            return btn;
        }

        private void InitializeDetailsPanel()
        {
            // Define column positions
            int leftCol1 = ControlMargin;
            int leftCol2 = leftCol1 + ComboBoxWidth + ControlMargin;
            int leftCol3 = leftCol2 + ComboBoxWidth + ControlMargin;
            int leftCol4 = leftCol3 + ComboBoxWidth + ControlMargin;
            int currentTop = ControlMargin;

            // Row 1: Work Type and Calculation Type
            AddLabel("Тип работы:", leftCol1, currentTop, controlFont);
            cbWorkType = new ComboBox()
            {
                Location = new Point(leftCol1, currentTop + rowHeight),
                Size = new Size(ComboBoxWidth, ButtonHeight),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Items = { "Поштучно", "Площадь" },
                Font = controlFont
            };
            panelDetails.Controls.Add(cbWorkType);

            AddLabel("Штука/Часовая:", leftCol2, currentTop, controlFont);
            var cbCalculationType = new ComboBox()
            {
                Name = "cbCalculationType",
                Location = new Point(leftCol2, currentTop + rowHeight),
                Size = new Size(ComboBoxWidth, ButtonHeight),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Items = { "Штука", "Часовая" },
                Font = controlFont
            };
            panelDetails.Controls.Add(cbCalculationType);

            // Row 1: Work Name (Column 3)
            AddLabel("Название работы:", leftCol3, currentTop, controlFont);
            txtName = new TextBox()
            {
                Location = new Point(leftCol3, currentTop + rowHeight),
                Size = new Size(ComboBoxWidth, ButtonHeight),
                Font = controlFont
            };
            panelDetails.Controls.Add(txtName);

            // Row 1: Price (Column 4)
            AddLabel("Расценка:", leftCol4, currentTop, controlFont);
            txtPrice = new TextBox()
            {
                Location = new Point(leftCol4, currentTop + rowHeight),
                Size = new Size(100, ButtonHeight),
                Font = controlFont
            };
            panelDetails.Controls.Add(txtPrice);

            currentTop += rowHeight * 2 + ControlMargin;

            // Row 2: Culture and Department
            AddLabel("Культура:", leftCol1, currentTop, controlFont);
            cbCulture = new ComboBox()
            {
                Location = new Point(leftCol1, currentTop + rowHeight),
                Size = new Size(ComboBoxWidth, ButtonHeight),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = controlFont
            };
            panelDetails.Controls.Add(cbCulture);

            AddLabel("Подразделение:", leftCol2, currentTop, controlFont);
            cbDepartment = new ComboBox()
            {
                Location = new Point(leftCol2, currentTop + rowHeight),
                Size = new Size(ComboBoxWidth, ButtonHeight),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = controlFont
            };
            panelDetails.Controls.Add(cbDepartment);

            // Row 2: Brigade and Fund
            AddLabel("Бригада:", leftCol3, currentTop, controlFont);
            cbBrigade = new ComboBox()
            {
                Location = new Point(leftCol3, currentTop + rowHeight),
                Size = new Size(ComboBoxWidth, ButtonHeight),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = controlFont
            };
            panelDetails.Controls.Add(cbBrigade);

            AddLabel("Разрешенный фонд:", leftCol4, currentTop, controlFont);
            txtSum = new TextBox()
            {
                Location = new Point(leftCol4, currentTop + rowHeight),
                Size = new Size(100, ButtonHeight),
                Text = "0.00",
                Font = controlFont
            };
            panelDetails.Controls.Add(txtSum);

            currentTop += rowHeight * 2 + ControlMargin;

            // Checkboxes (Column 4)
            chbComboWork = new CheckBox()
            {
                Text = "Использовать подробную статистику",
                Location = new Point(leftCol4, currentTop),
                Size = new Size(ComboBoxWidth, ButtonHeight),
                Font = controlFont
            };

            chbSquare = new CheckBox()
            {
                Text = "Использовать учёт квадратных метр",
                Location = new Point(leftCol4, currentTop + rowHeight),
                Size = new Size(ComboBoxWidth, ButtonHeight),
                Font = controlFont
            };

            chbCalcType = new CheckBox()
            {
                Text = "Использовать расчет колич",
                Location = new Point(leftCol4, currentTop + rowHeight * 2),
                Size = new Size(ComboBoxWidth, ButtonHeight),
                Font = controlFont
            };

            chbHasOtdel = new CheckBox()
            {
                Text = "Использовать отделение систему",
                Location = new Point(leftCol4, currentTop + rowHeight * 3),
                Size = new Size(ComboBoxWidth, ButtonHeight),
                Font = controlFont
            };

            chbGlobal = new CheckBox()
            {
                Text = "Глобальный",
                Location = new Point(leftCol4, currentTop + rowHeight * 4),
                Size = new Size(ComboBoxWidth, ButtonHeight),
                Font = controlFont
            };

            panelDetails.Controls.Add(chbComboWork);
            panelDetails.Controls.Add(chbSquare);
            panelDetails.Controls.Add(chbCalcType);
            panelDetails.Controls.Add(chbHasOtdel);
            panelDetails.Controls.Add(chbGlobal);

            // Combo work details
            int detailTop = currentTop;

            // Column 1: Spens
            AddLabel("Кол-во спенов:", leftCol1, detailTop, controlFont);
            txtSpen = new TextBox()
            {
                Location = new Point(leftCol1, detailTop + rowHeight),
                Size = new Size(120, ButtonHeight),
                Text = "0",
                Font = controlFont
            };
            panelDetails.Controls.Add(txtSpen);

            // Column 2: Gradki
            AddLabel("Кол-во грядок:", leftCol2, detailTop, controlFont);
            txtGradki = new TextBox()
            {
                Location = new Point(leftCol2, detailTop + rowHeight),
                Size = new Size(120, ButtonHeight),
                Text = "0",
                Font = controlFont
            };
            panelDetails.Controls.Add(txtGradki);

            // Column 3: Krug
            AddLabel("Грядки/столбы:", leftCol3, detailTop, controlFont);
            txtKrug = new TextBox()
            {
                Location = new Point(leftCol3, detailTop + rowHeight),
                Size = new Size(120, ButtonHeight),
                Text = "0",
                Font = controlFont
            };
            panelDetails.Controls.Add(txtKrug);

            // Circle period
            AddLabel("Круг за дней:", leftCol1, detailTop + rowHeight * 2, controlFont);
            cbCirclePeriod = new ComboBox()
            {
                Location = new Point(leftCol1, detailTop + rowHeight * 3),
                Size = new Size(120, ButtonHeight),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Items = { "7", "14", "21", "28" },
                Font = controlFont
            };
            panelDetails.Controls.Add(cbCirclePeriod);

            // Week
            AddLabel("Неделя:", leftCol2, detailTop + rowHeight * 2, controlFont);
            txtWeek = new TextBox()
            {
                Location = new Point(leftCol2, detailTop + rowHeight * 3),
                Size = new Size(80, ButtonHeight),
                Text = "0",
                Font = controlFont
            };
            panelDetails.Controls.Add(txtWeek);

            currentTop += rowHeight * 4 + ControlMargin;

            // Action buttons
            btnSave = new Button()
            {
                Text = "Сохранить",
                Location = new Point(leftCol2, currentTop + rowHeight - 10),
                Size = new Size(ButtonWidth, ButtonHeight),
                Font = controlFont
            };

            btnDetailsCancel = new Button()
            {
                Text = "Отмена",
                Location = new Point(leftCol2 + ControlMargin + 90, currentTop + rowHeight - 10),
                Size = new Size(ButtonWidth, ButtonHeight),
                Font = controlFont
            };

            panelDetails.Controls.Add(btnSave);
            panelDetails.Controls.Add(btnDetailsCancel);

            // Department selection
            AddLabel("Отделы:", leftCol1, currentTop, controlFont);
            txtOtdel = new TextBox()
            {
                Location = new Point(leftCol1, currentTop + rowHeight),
                Size = new Size(180, ButtonHeight),
                Font = controlFont
            };
            panelDetails.Controls.Add(txtOtdel);

            currentTop += rowHeight * 2 + ControlMargin;
        }

        private void AddLabel(string text, int left, int top, Font font)
        {
            panelDetails.Controls.Add(new Label()
            {
                Text = text,
                Location = new Point(left, top),
                AutoSize = true,
                Font = font
            });
        }

        private void WireUpEventHandlers()
        {
            btnAdd.Click += BtnAdd_Click;
            btnEdit.Click += BtnEdit_Click;
            btnDelete.Click += BtnDelete_Click;
            btnExcel.Click += BtnExcel_Click;
            btnRefresh.Click += BtnRefresh_Click;
            btnSave.Click += BtnSave_Click;
            btnToolbarCancel.Click += BtnCancel_Click;
            btnDetailsCancel.Click += BtnCancel_Click;
            cbFilterCulture.SelectedIndexChanged += CbFilterCulture_SelectedIndexChanged;
            txtOtdel.DoubleClick += TxtOtdel_DoubleClick;
            chbComboWork.CheckedChanged += ChbComboWork_CheckedChanged;
            chbGlobal.CheckedChanged += ChbGlobal_CheckedChanged;
            cbDepartment.SelectedIndexChanged += CbDepartment_SelectedIndexChanged;
            dgvWorks.CellClick += DgvWorks_CellClick;
            txtPrice.KeyPress += NumericOnly_KeyPress;
            txtSpen.KeyPress += NumericOnly_KeyPress;
            txtGradki.KeyPress += NumericOnly_KeyPress;
            txtKrug.KeyPress += NumericOnly_KeyPress;
            btnKoef.Click += BtnKoef_Click;
        }

        private void NumericOnly_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != ',' && e.KeyChar != (char)Keys.Back)
            {
                e.Handled = true;
            }
        }

        #region Event Handlers
        private void BtnAdd_Click(object sender, EventArgs e)
        {
            if (cbDepartment.Items.Count == 0 || cbDepartment.Items[0].ToString() == "Создайте подразделения!")
            {
                MessageBox.Show("Для начала создайте подразделение!\nПодразделения создаются в разделе\nПодразделения и должности.",
                    "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            all_ism = 1; // Add mode
            ClearForm();
            SetFormState(true);
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            if (dgvWorks.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите запись для редактирования",
                                "Внимание",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                return;
            }

            all_ism = 2; // Edit mode
            try
            {
                LoadSelectedWorkType();
                SetFormState(true);
            }
            catch (InvalidCastException ex)
            {
                MessageBox.Show($"Ошибка загрузки данных: {ex.Message}\nПроверьте данные в базе.",
                                "Ошибка",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
            }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {

            if (dgvWorks.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите запись для удаления",
                                "Внимание",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                return;
            }

            // Only THEN access the selected row
            var id = Convert.ToInt32(dgvWorks.SelectedRows[0].Cells["ID"].Value);
            string workName = dgvWorks.SelectedRows[0].Cells["Название"].Value?.ToString() ?? "Unknown Work";

            if (MessageBox.Show($"Вы уверены, что хотите удалить работу:\n{workName}?",
                                "Подтверждение удаления",
                                MessageBoxButtons.YesNo,
                                MessageBoxIcon.Question) != DialogResult.Yes)
            {
                return;
            }

            string connString = string.Format(connStr, dbPath);

            using (var conn = new OleDbConnection(connString))
            {
                conn.Open();

                // Check if work exists in worker records
                var checkCmd = new OleDbCommand(
                    $"SELECT COUNT(*) FROM BD_workin_rab_all WHERE ID_work={id}", conn);
                int count = (int)checkCmd.ExecuteScalar();

                if (count > 0)
                {
                    var result = MessageBox.Show(
                        $"Эта работа используется в {count} записях работников.\nВсе равно удалить?",
                        "Предупреждение", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                    if (result != DialogResult.Yes) return;

                    // Delete from worker records
                    new OleDbCommand($"DELETE FROM BD_workin_rab_all WHERE ID_work={id}", conn).ExecuteNonQuery();
                }

                // Delete the work type
                new OleDbCommand($"DELETE FROM Vid_RABOT WHERE ID={id}", conn).ExecuteNonQuery();
            }

            LoadWorkTypes();
            MessageBox.Show("Работа успешно удалена!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            // Validation
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                ShowError("Введите название работы!");
                return;
            }

            if (string.IsNullOrWhiteSpace(txtPrice.Text))
            {
                ShowError("Вы не ввели расценку! Введите.");
                return;
            }

            if (chbComboWork.Checked && (txtSpen.Text == "0" || string.IsNullOrWhiteSpace(txtSpen.Text)))
            {
                ShowError("Вы не ввели количество спенов. Введите");
                return;
            }

            // Additional validations
            if (chbHasOtdel.Checked && string.IsNullOrWhiteSpace(txtOtdel.Text))
            {
                ShowError("Выберите отделения!");
                return;
            }

            if (chbComboWork.Checked && cbCirclePeriod.SelectedIndex == -1)
            {
                ShowError("Выберите период для кругов!");
                return;
            }

            if (!double.TryParse(txtPrice.Text, out double price))
            {
                ShowError("Неверный формат расценки. Введите число.");
                return;
            }

            if (!int.TryParse(txtSpen.Text, out int spen))
            {
                ShowError("Неверный формат количества спенов. Введите целое число.");
                return;
            }

            if (!int.TryParse(txtGradki.Text, out int gradki))
            {
                ShowError("Неверный формат количества грядок. Введите целое число.");
                return;
            }

            if (!int.TryParse(txtKrug.Text, out int krug))
            {
                ShowError("Неверный формат количества кругов. Введите целое число.");
                return;
            }

            if (!double.TryParse(txtSum.Text, out double sumRub))
            {
                ShowError("Неверный формат суммы. Введите число.");
                return;
            }

            if (!int.TryParse(txtWeek.Text, out int week))
            {
                ShowError("Неверный формат недели. Введите целое число.");
                return;
            }

            string connString = string.Format(connStr, dbPath);
            using (var conn = new OleDbConnection(connString))
            {
                conn.Open();
                string sql = all_ism == 1 ?
                    @"INSERT INTO Vid_RABOT (
                Name_W, Racenka, IsHourly, Kultura, Kultura_str, M2, 
                Podrasd_N, Podrasd_str, tip_rasch, ID_Brig, STR_Brig, Komb_WORK, 
                N_spen, N_gradki, N_krug, KOL_Krug, Sum_RUB, bool_otd, N_otd, 
                Week, Koef
            ) VALUES (?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?)" :
                    @"UPDATE Vid_RABOT SET 
                Name_W=?, Racenka=?, IsHourly=?, Kultura=?, Kultura_str=?, M2=?, 
                Podrasd_N=?, Podrasd_str=?, tip_rasch=?, ID_Brig=?, STR_Brig=?, 
                Komb_WORK=?, N_spen=?, N_gradki=?, N_krug=?, KOL_Krug=?, Sum_RUB=?, 
                bool_otd=?, N_otd=?, Week=?, Koef=? 
            WHERE ID=?";

                using (var cmd = new OleDbCommand(sql, conn))
                {
                    // Get calculation type
                    var cbCalculationType = panelDetails.Controls.Find("cbCalculationType", true).FirstOrDefault() as ComboBox;
                    bool isHourly = cbCalculationType != null && cbCalculationType.SelectedIndex == 1;

                    int brigadeIdValue = 0;
                    if (cbBrigade.SelectedIndex >= 0)
                    {
                        brigadeIdValue = brigadeIds[cbBrigade.SelectedIndex];
                    }

                    cmd.Parameters.AddWithValue("Name_W", txtName.Text);
                    cmd.Parameters.AddWithValue("Racenka", price);
                    cmd.Parameters.AddWithValue("IsHourly", isHourly);
                    cmd.Parameters.AddWithValue("Kultura", cultureIds[cbCulture.SelectedIndex]);
                    cmd.Parameters.AddWithValue("Kultura_str", cbCulture.Text);
                    cmd.Parameters.AddWithValue("M2", chbSquare.Checked);
                    cmd.Parameters.AddWithValue("Podrasd_N", departmentIds[cbDepartment.SelectedIndex]);
                    cmd.Parameters.AddWithValue("Podrasd_str", cbDepartment.Text);
                    cmd.Parameters.AddWithValue("tip_rasch", chbCalcType.Checked ? 1 : 0);

                    cmd.Parameters.AddWithValue("ID_Brig", brigadeIdValue);
                    cmd.Parameters.AddWithValue("STR_Brig", cbBrigade.Text);
                    cmd.Parameters.AddWithValue("Komb_WORK", chbComboWork.Checked ? 3 : 0);
                    cmd.Parameters.AddWithValue("N_spen", spen);
                    cmd.Parameters.AddWithValue("N_gradki", gradki);
                    cmd.Parameters.AddWithValue("N_krug", cbCirclePeriod.SelectedIndex + 1);
                    cmd.Parameters.AddWithValue("KOL_Krug", krug);
                    cmd.Parameters.AddWithValue("Sum_RUB", sumRub);
                    cmd.Parameters.AddWithValue("bool_otd", chbHasOtdel.Checked);
                    cmd.Parameters.AddWithValue("N_otd", txtOtdel.Text);
                    cmd.Parameters.AddWithValue("Week", week);
                    cmd.Parameters.AddWithValue("Koef", chbGlobal.Checked ? 1.0 : 1.0);

                    if (all_ism == 2)
                    {
                        var id = dgvWorks.SelectedRows[0].Cells["ID"].Value;
                        cmd.Parameters.AddWithValue("ID", id);
                    }

                    cmd.ExecuteNonQuery();
                }
            }

            SetFormState(false);
            LoadWorkTypes();
            MessageBox.Show("Данные успешно сохранены!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            if (cbDepartment.Items.Count > 0 && cbDepartment.Items[0].ToString() == "Создайте подразделения!")
            {
                MessageBox.Show("Для начала создайте подразделение!\nПодразделения создаются в разделе\nПодразделения и должности.",
                    "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            SetFormState(false);
            ClearForm();
            all_ism = 0;
            if (cbDepartment.Items.Count > 0) cbDepartment.SelectedIndex = 0;
            dgvWorks.ClearSelection();
        }

        private void BtnKoef_Click(object sender, EventArgs e)
        {
            if (dgvWorks.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите работу для установки коэффициента!",
                                "Внимание",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                return;
            }

            var selectedId = Convert.ToInt32(dgvWorks.SelectedRows[0].Cells["ID"].Value);
            string workName = dgvWorks.SelectedRows[0].Cells["Название"].Value.ToString();
            string connString = string.Format(connStr, dbPath);
            double currentKoef = 1.0;

            // Get current coefficient from database
            using (var conn = new OleDbConnection(connString))
            {
                conn.Open();
                var cmd = new OleDbCommand("SELECT Koef FROM Vid_RABOT WHERE ID = ?", conn);
                cmd.Parameters.AddWithValue("ID", selectedId);
                var result = cmd.ExecuteScalar();
                if (result != null && result != DBNull.Value)
                {
                    currentKoef = Convert.ToDouble(result);
                }
            }

            using (var koefForm = new Form())
            {
                koefForm.Text = $"Коэффициент для: {workName}";
                koefForm.Size = new Size(350, 180);
                koefForm.StartPosition = FormStartPosition.CenterParent;
                koefForm.FormBorderStyle = FormBorderStyle.FixedDialog;
                koefForm.MaximizeBox = false;

                NumericUpDown numKoef = new NumericUpDown()
                {
                    Minimum = 0.01m,
                    Maximum = 10.0m,
                    DecimalPlaces = 2,
                    Increment = 0.1m,
                    Value = (decimal)currentKoef,
                    Location = new Point(20, 60),
                    Size = new Size(100, 25)
                };

                Label lblInfo = new Label()
                {
                    Text = $"Текущий коэффициент: {currentKoef:F2}\nУстановите новое значение:",
                    Location = new Point(20, 20),
                    AutoSize = true
                };

                Button btnApply = new Button()
                {
                    Text = "Применить",
                    Location = new Point(20, 100),
                    Size = new Size(80, 30),
                    DialogResult = DialogResult.OK
                };

                Button btnCancel = new Button()
                {
                    Text = "Отмена",
                    Location = new Point(120, 100),
                    Size = new Size(80, 30),
                    DialogResult = DialogResult.Cancel
                };

                btnApply.Click += (s, ev) =>
                {
                    using (var conn = new OleDbConnection(connString))
                    {
                        conn.Open();
                        var cmd = new OleDbCommand(
                            "UPDATE Vid_RABOT SET Koef = ? WHERE ID = ?", conn);
                        cmd.Parameters.AddWithValue("Koef", numKoef.Value);
                        cmd.Parameters.AddWithValue("ID", selectedId);
                        cmd.ExecuteNonQuery();
                    }
                    koefForm.DialogResult = DialogResult.OK;
                    koefForm.Close();
                };

                koefForm.Controls.Add(lblInfo);
                koefForm.Controls.Add(numKoef);
                koefForm.Controls.Add(btnApply);
                koefForm.Controls.Add(btnCancel);

                if (koefForm.ShowDialog() == DialogResult.OK)
                {
                    LoadWorkTypes();
                    MessageBox.Show("Коэффициент успешно обновлен!",
                                    "Успех",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Information);
                }
            }
        }

        private void ChbComboWork_CheckedChanged(object sender, EventArgs e)
        {
            txtSpen.Enabled = chbComboWork.Checked;
            txtGradki.Enabled = chbComboWork.Checked;
            cbCirclePeriod.Enabled = chbComboWork.Checked;
            chbGlobal.Enabled = chbComboWork.Checked;
        }

        private void ChbGlobal_CheckedChanged(object sender, EventArgs e)
        {
            btnKoef.Enabled = chbGlobal.Checked;
        }

        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            LoadWorkTypes();
        }

        private void BtnExcel_Click(object sender, EventArgs e)
        {
            try
            {
                var excelApp = new Excel.Application();
                excelApp.Visible = true;
                Excel.Workbook workbook = excelApp.Workbooks.Add();
                Excel.Worksheet worksheet = workbook.ActiveSheet;

                // Headers
                for (int i = 0; i < dgvWorks.Columns.Count; i++)
                {
                    worksheet.Cells[1, i + 1] = dgvWorks.Columns[i].HeaderText;
                    worksheet.Cells[1, i + 1].Font.Bold = true;
                    worksheet.Cells[1, i + 1].Interior.Color = Color.LightGray.ToArgb();
                }

                // Data
                for (int i = 0; i < dgvWorks.Rows.Count; i++)
                {
                    for (int j = 0; j < dgvWorks.Columns.Count; j++)
                    {
                        worksheet.Cells[i + 2, j + 1] = dgvWorks.Rows[i].Cells[j].Value?.ToString();
                    }
                }

                // Formatting
                worksheet.Columns.AutoFit();
                worksheet.UsedRange.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка экспорта в Excel:\n{ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CbFilterCulture_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbFilterCulture.SelectedIndex == 0)
            {
                ((DataTable)dgvWorks.DataSource).DefaultView.RowFilter = "";
            }
            else
            {
                int cultureId = cultureIds[cbFilterCulture.SelectedIndex - 1];
                ((DataTable)dgvWorks.DataSource).DefaultView.RowFilter = $"Kultura_ID = {cultureId}";
            }
        }

        private void TxtOtdel_DoubleClick(object sender, EventArgs e)
        {
            if (chbHasOtdel.Checked)
            {
                using (var form = new Form()
                {
                    Text = "Выбор отделов",
                    Width = 500,
                    Height = 400,
                    StartPosition = FormStartPosition.CenterParent
                })
                {
                    var listBox = new CheckedListBox()
                    {
                        Dock = DockStyle.Fill,
                        CheckOnClick = true
                    };

                    // Load all departments
                    var departments = new System.Collections.Generic.List<string>();
                    string connString = string.Format(connStr, dbPath);
                    using (var conn = new OleDbConnection(connString))
                    {
                        conn.Open();
                        var cmd = new OleDbCommand("SELECT Name_PODR FROM Name_PODRASD", conn);
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                departments.Add(reader["Name_PODR"].ToString());
                            }
                        }
                    }

                    // Add departments to list
                    foreach (var dept in departments)
                    {
                        listBox.Items.Add(dept);
                    }

                    // Check currently selected departments
                    var currentDepartments = txtOtdel.Text.Split(new[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
                    for (int i = 0; i < listBox.Items.Count; i++)
                    {
                        if (Array.Exists(currentDepartments, d => d.Equals(listBox.Items[i].ToString())))
                        {
                            listBox.SetItemChecked(i, true);
                        }
                    }

                    // Add buttons
                    var btnPanel = new Panel { Dock = DockStyle.Bottom, Height = 40 };
                    var btnOK = new Button { Text = "OK", DialogResult = DialogResult.OK, Size = new Size(80, 30) };
                    var btnCancel = new Button { Text = "Отмена", DialogResult = DialogResult.Cancel, Size = new Size(80, 30) };

                    btnOK.Click += (s, ev) => form.DialogResult = DialogResult.OK;
                    btnCancel.Click += (s, ev) => form.DialogResult = DialogResult.Cancel;

                    btnPanel.Controls.Add(btnOK);
                    btnPanel.Controls.Add(btnCancel);
                    btnOK.Left = (btnPanel.Width - btnOK.Width) / 2 - 50;
                    btnCancel.Left = (btnPanel.Width - btnCancel.Width) / 2 + 50;
                    btnOK.Top = (btnPanel.Height - btnOK.Height) / 2;
                    btnCancel.Top = (btnPanel.Height - btnCancel.Height) / 2;

                    form.Controls.Add(listBox);
                    form.Controls.Add(btnPanel);

                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        var selected = new System.Text.StringBuilder();
                        foreach (var item in listBox.CheckedItems)
                        {
                            if (selected.Length > 0) selected.Append(", ");
                            selected.Append(item.ToString());
                        }
                        txtOtdel.Text = selected.ToString();
                    }
                }
            }
        }

        private void CbDepartment_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbDepartment.SelectedIndex >= 0)
            {
                LoadBrigades(departmentIds[cbDepartment.SelectedIndex]);
            }
        }

        private void DgvWorks_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                try
                {
                    LoadSelectedWorkType();
                }
                catch (InvalidCastException ex)
                {
                    MessageBox.Show($"Ошибка загрузки данных: {ex.Message}\nВозможно, в базе есть пустые значения.",
                                    "Ошибка",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);
                }
            }
        }
        #endregion

        #region Database Methods
        private void EnsureDatabaseSchema()
        {
            string connString = string.Format(connStr, dbPath);
            using (var conn = new OleDbConnection(connString))
            {
                conn.Open();

                bool columnExists = false;
                var schema = conn.GetSchema("Columns", new string[4] { null, null, "Vid_RABOT", null });

                foreach (DataRow row in schema.Rows)
                {
                    if (row["COLUMN_NAME"].ToString() == "IsHourly")
                    {
                        columnExists = true;
                        break;
                    }
                }

                // Add column if missing
                if (!columnExists)
                {
                    new OleDbCommand("ALTER TABLE Vid_RABOT ADD COLUMN IsHourly BIT DEFAULT False", conn).ExecuteNonQuery();
                }

                foreach (DataRow row in schema.Rows)
                {
                    if (row["COLUMN_NAME"].ToString() == "Koef")
                    {
                        columnExists = true;
                        break;
                    }
                }

                // Add column if missing
                if (!columnExists)
                {
                    new OleDbCommand("ALTER TABLE Vid_RABOT ADD COLUMN Koef DOUBLE DEFAULT 1.0", conn).ExecuteNonQuery();
                }

                // Check if Week column exists
                bool weekColumnExists = false;
                foreach (DataRow row in schema.Rows)
                {
                    if (row["COLUMN_NAME"].ToString() == "Week")
                    {
                        weekColumnExists = true;
                        break;
                    }
                }

                if (!weekColumnExists)
                {
                    new OleDbCommand("ALTER TABLE Vid_RABOT ADD COLUMN Week INTEGER DEFAULT 0", conn).ExecuteNonQuery();
                }
            }
        }

        private void LoadInitialData()
        {
            EnsureDatabaseSchema();
            LoadCultures();
            LoadDepartments();
            LoadWorkTypes();
            SetFormState(false);
        }

        private void LoadCultures()
        {
            string connString = string.Format(connStr, dbPath);
            using (var conn = new OleDbConnection(connString))
            {
                conn.Open();
                var cmd = new OleDbCommand("SELECT ID, Name_kultur FROM Kultura ORDER BY Name_kultur", conn);
                var reader = cmd.ExecuteReader();

                cbCulture.Items.Clear();
                cbFilterCulture.Items.Clear();

                cbCulture.Items.Add("Без культуры");
                cbFilterCulture.Items.Add("Все культуры");
                cbFilterCulture.Items.Add("Без культуры");

                cultureIds = new int[50]; // Ensure proper size
                int index = 1;
                while (reader.Read())
                {
                    cbCulture.Items.Add(reader["Name_kultur"].ToString());
                    cbFilterCulture.Items.Add(reader["Name_kultur"].ToString());
                    cultureIds[index] = Convert.ToInt32(reader["ID"]);
                    index++;
                }

                cbCulture.SelectedIndex = 0;
                cbFilterCulture.SelectedIndex = 0;
            }
        }

        private void LoadDepartments()
        {
            string connString = string.Format(connStr, dbPath);
            using (var conn = new OleDbConnection(connString))
            {
                conn.Open();
                var cmd = new OleDbCommand("SELECT ID, Name_PODR FROM Name_PODRASD ORDER BY Name_PODR", conn);
                var reader = cmd.ExecuteReader();

                cbDepartment.Items.Clear();
                int index = 0;
                while (reader.Read())
                {
                    cbDepartment.Items.Add(reader["Name_PODR"].ToString());
                    departmentIds[index] = Convert.ToInt32(reader["ID"]);
                    index++;
                }

                if (cbDepartment.Items.Count > 0)
                    cbDepartment.SelectedIndex = 0;
                else
                    cbDepartment.Items.Add("Создайте подразделения!");
            }

            if (cbDepartment.Items.Count > 0 && cbDepartment.Items[0].ToString() != "Создайте подразделения!")
            {
                LoadBrigades(departmentIds[0]);
            }
        }

        private void LoadBrigades(int departmentId)
        {
            string connString = string.Format(connStr, dbPath);
            using (var conn = new OleDbConnection(connString))
            {
                conn.Open();
                var cmd = new OleDbCommand(
                    $"SELECT ID, Name_Brigad FROM Name_BRIGADA WHERE ID_PODR={departmentId}", conn);
                var reader = cmd.ExecuteReader();

                cbBrigade.Items.Clear();
                brigadeIds = new int[20]; // Reset IDs
                int index = 0;
                while (reader.Read() && index < brigadeIds.Length)
                {
                    cbBrigade.Items.Add(reader["Name_Brigad"].ToString());
                    brigadeIds[index] = Convert.ToInt32(reader["ID"]);
                    index++;
                }

                if (cbBrigade.Items.Count > 0)
                    cbBrigade.SelectedIndex = 0;
                else
                    cbBrigade.Items.Add("У подраз. нет бригады");
            }
        }

        private void LoadSelectedWorkType()
        {
            if (dgvWorks.SelectedRows.Count == 0) return;

            var id = Convert.ToInt32(dgvWorks.SelectedRows[0].Cells["ID"].Value);
            string connString = string.Format(connStr, dbPath);

            using (var conn = new OleDbConnection(connString))
            {
                conn.Open();
                var cmd = new OleDbCommand("SELECT * FROM Vid_RABOT WHERE ID = ?", conn);
                cmd.Parameters.AddWithValue("?", id);

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        // Clear previous selection
                        dgvWorks.ClearSelection();

                        // Highlight selected row
                        foreach (DataGridViewRow row in dgvWorks.Rows)
                        {
                            if (Convert.ToInt32(row.Cells["ID"].Value) == id)
                            {
                                row.Selected = true;
                                dgvWorks.CurrentCell = row.Cells[0];
                                break;
                            }
                        }

                        // Main fields
                        txtName.Text = SafeGetString(reader, "Name_W");
                        txtPrice.Text = SafeGetDouble(reader, "Racenka").ToString("F2");

                        // Culture selection
                        int cultureId = SafeGetInt(reader, "Kultura");
                        int cultureIndex = -1;
                        for (int i = 0; i < cultureIds.Length; i++)
                        {
                            if (cultureIds[i] == cultureId)
                            {
                                cultureIndex = i;
                                break;
                            }
                        }
                        if (cultureIndex >= 0 && cultureIndex < cbCulture.Items.Count)
                            cbCulture.SelectedIndex = cultureIndex;

                        // Calculation type (Штука/Часовая)
                        var cbCalculationType = panelDetails.Controls.Find("cbCalculationType", true).FirstOrDefault() as ComboBox;
                        if (cbCalculationType != null)
                        {
                            bool isHourly = SafeGetBool(reader, "IsHourly"); // You'll need to add this field to your database
                            cbCalculationType.SelectedIndex = isHourly ? 1 : 0;
                        }
                        // Department selection
                        int deptId = SafeGetInt(reader, "Podrasd_N");
                        int deptIndex = -1;
                        for (int i = 0; i < departmentIds.Length; i++)
                        {
                            if (departmentIds[i] == deptId)
                            {
                                deptIndex = i;
                                break;
                            }
                        }
                        if (deptIndex >= 0 && deptIndex < cbDepartment.Items.Count)
                            cbDepartment.SelectedIndex = deptIndex;

                        // Brigade selection
                        int brigadeId = SafeGetInt(reader, "ID_Brig");
                        int brigadeIndex = -1;
                        for (int i = 0; i < brigadeIds.Length; i++)
                        {
                            if (brigadeIds[i] == brigadeId)
                            {
                                brigadeIndex = i;
                                break;
                            }
                        }
                        if (brigadeIndex >= 0 && brigadeIndex < cbBrigade.Items.Count)
                            cbBrigade.SelectedIndex = brigadeIndex;

                        // Checkboxes
                        chbSquare.Checked = SafeGetBool(reader, "M2");
                        chbCalcType.Checked = SafeGetBool(reader, "tip_rasch");
                        chbComboWork.Checked = SafeGetInt(reader, "Komb_WORK") == 3;
                        chbHasOtdel.Checked = SafeGetBool(reader, "bool_otd");

                        // Handle Koef (global) safely
                        double koefValue = SafeGetDouble(reader, "Koef");
                        chbGlobal.Checked = Math.Abs(koefValue - 1.0) > 0.001;

                        // Combo work details
                        txtSpen.Text = SafeGetInt(reader, "N_spen").ToString();
                        txtGradki.Text = SafeGetInt(reader, "N_gradki").ToString();
                        txtKrug.Text = SafeGetInt(reader, "KOL_Krug").ToString();
                        txtSum.Text = SafeGetDouble(reader, "Sum_RUB").ToString("F2");

                        // Week field
                        txtWeek.Text = SafeGetInt(reader, "Week").ToString();

                        // Department system
                        txtOtdel.Text = SafeGetString(reader, "N_otd");

                        // Circle of the day
                        int circlePeriod = SafeGetInt(reader, "N_krug");
                        if (circlePeriod >= 1 && circlePeriod <= 4)
                            cbCirclePeriod.SelectedIndex = circlePeriod - 1;
                    }
                }
            }
        }

        private string SafeGetString(OleDbDataReader reader, string column)
        {
            try
            {
                int colIndex = reader.GetOrdinal(column);
                return reader.IsDBNull(colIndex) ? string.Empty : reader.GetString(colIndex);
            }
            catch
            {
                return string.Empty;
            }
        }

        private int SafeGetInt(OleDbDataReader reader, string column)
        {
            try
            {
                int colIndex = reader.GetOrdinal(column);
                return reader.IsDBNull(colIndex) ? 0 : Convert.ToInt32(reader[colIndex]);
            }
            catch
            {
                return 0;
            }
        }

        private double SafeGetDouble(OleDbDataReader reader, string column)
        {
            try
            {
                int colIndex = reader.GetOrdinal(column);
                return reader.IsDBNull(colIndex) ? 0.0 : Convert.ToDouble(reader[colIndex]);
            }
            catch
            {
                return 0.0;
            }
        }

        private bool SafeGetBool(OleDbDataReader reader, string column)
        {
            try
            {
                int colIndex = reader.GetOrdinal(column);
                return !reader.IsDBNull(colIndex) && Convert.ToBoolean(reader[colIndex]);
            }
            catch
            {
                return false;
            }
        }

        private void LoadWorkTypes()
        {
            string connString = string.Format(connStr, dbPath);
            using (var conn = new OleDbConnection(connString))
            {
                conn.Open();
                var adapter = new OleDbDataAdapter(
                    @"SELECT 
                ID, 
                Name_W AS [Название], 
                IIF(IsHourly = True, 'Часовая', 'Штука') AS [Тип расчета],
                Racenka AS [Расценка],
                Kultura_str AS [Культура],
                Kultura AS [Kultura_ID],
                Podrasd_str AS [Подразделение], 
                STR_Brig AS [Бригада], 
                Koef AS [Коэффициент] 
              FROM Vid_RABOT 
              ORDER BY Name_W",
                    conn);

                var dt = new DataTable();
                adapter.Fill(dt);
                dgvWorks.DataSource = dt;

                dgvWorks.Columns["Kultura_ID"].Visible = false;

                dgvWorks.Font = new Font("Times New Roman", 12);
                foreach (DataGridViewColumn col in dgvWorks.Columns)
                {
                    col.HeaderCell.Style.Font = new Font("Times New Roman", 12, FontStyle.Bold);
                }

                // Add row numbers
                dgvWorks.RowHeadersVisible = true;
                foreach (DataGridViewRow row in dgvWorks.Rows)
                {
                    row.HeaderCell.Value = (row.Index + 1).ToString();
                }
            }
        }
        #endregion

        #region Helper Methods
        private void SetFormState(bool editing)
        {
            dgvWorks.Enabled = !editing;
            btnAdd.Enabled = !editing;
            btnEdit.Enabled = !editing && dgvWorks.SelectedRows.Count > 0;
            btnEdit.Enabled = !editing && dgvWorks.SelectedRows.Count > 0;
            btnDelete.Enabled = !editing && dgvWorks.SelectedRows.Count > 0;
            btnRefresh.Enabled = !editing;
            btnKoef.Enabled = !editing && dgvWorks.SelectedRows.Count > 0 && chbGlobal.Checked;

            // Details panel
            txtName.Enabled = editing;
            txtPrice.Enabled = editing;
            cbWorkType.Enabled = editing;
            cbCulture.Enabled = editing;
            cbDepartment.Enabled = editing;
            cbBrigade.Enabled = editing;
            chbComboWork.Enabled = editing;
            chbSquare.Enabled = editing;
            chbCalcType.Enabled = editing;
            chbHasOtdel.Enabled = editing;
            chbGlobal.Enabled = editing;
            txtSpen.Enabled = editing && chbComboWork.Checked;
            txtGradki.Enabled = editing && chbComboWork.Checked;
            txtKrug.Enabled = editing && chbComboWork.Checked;
            cbCirclePeriod.Enabled = editing && chbComboWork.Checked;
            txtOtdel.Enabled = editing && chbHasOtdel.Checked;
            btnSave.Enabled = editing;
            btnDetailsCancel.Enabled = editing;
            btnToolbarCancel.Enabled = editing;
            txtWeek.Enabled = editing;
        }

        private void ClearForm()
        {
            txtName.Text = "";
            txtPrice.Text = "";
            txtSpen.Text = "0";
            txtGradki.Text = "0";
            txtKrug.Text = "0";
            txtOtdel.Text = "";
            cbWorkType.SelectedIndex = 0;
            cbCulture.SelectedIndex = 0;
            if (cbDepartment.Items.Count > 0) cbDepartment.SelectedIndex = 0;
            cbBrigade.Items.Clear();
            chbComboWork.Checked = false;
            chbSquare.Checked = false;
            chbCalcType.Checked = false;
            chbHasOtdel.Checked = false;
            chbGlobal.Checked = false;
            txtWeek.Text = "0";
            cbCirclePeriod.SelectedIndex = -1;
        }

        private void ShowError(string message)
        {
            MessageBox.Show(message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void ApplyTheme()
        {
            var theme = ThemeManager.CurrentTheme;
            var isDark = theme == AppTheme.Dark;

            // Color scheme
            var backColor = isDark ? Color.FromArgb(45, 45, 48) : Color.White;
            var foreColor = isDark ? Color.White : Color.Black;
            var controlBack = isDark ? Color.FromArgb(60, 60, 65) : Color.White;
            var borderColor = isDark ? Color.FromArgb(90, 90, 90) : SystemColors.ControlDark;

            // Apply to controls
            this.BackColor = isDark ? Color.FromArgb(30, 30, 30) : SystemColors.Control;
            dgvWorks.BackgroundColor = backColor;
            dgvWorks.DefaultCellStyle.BackColor = backColor;
            dgvWorks.DefaultCellStyle.ForeColor = foreColor;
            dgvWorks.ColumnHeadersDefaultCellStyle.BackColor = isDark ?
                Color.FromArgb(70, 70, 80) : Color.LightGray;
            panelDetails.BackColor = backColor;

            // Style all child controls
            foreach (Control c in this.Controls)
            {
                c.ForeColor = foreColor;
                if (c is TextBox || c is ComboBox)
                {
                    c.BackColor = controlBack;
                    if (c is ComboBox cb) cb.FlatStyle = FlatStyle.Flat;
                }
                else if (c is Button btn)
                {
                    btn.BackColor = isDark ? Color.FromArgb(70, 70, 70) : SystemColors.ControlLight;
                    btn.FlatAppearance.BorderColor = borderColor;
                }
                else if (c is CheckBox chk)
                {
                    chk.ForeColor = foreColor;
                    chk.BackColor = backColor;
                }
            }

            foreach (Control c in panelDetails.Controls)
            {
                c.ForeColor = foreColor;
                if (c is TextBox || c is ComboBox)
                {
                    c.BackColor = controlBack;
                }
                else if (c is Button btn)
                {
                    btn.BackColor = isDark ? Color.FromArgb(70, 70, 70) : SystemColors.ControlLight;
                }
                else if (c is CheckBox chk)
                {
                    chk.ForeColor = foreColor;
                    chk.BackColor = backColor;
                }
                else if (c is Label lbl)
                {
                    lbl.BackColor = backColor;
                }
            }
        }
        #endregion
    }
}