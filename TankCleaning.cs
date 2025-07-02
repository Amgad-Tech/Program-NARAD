using System;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Windows.Forms;

namespace Program_na_Ryadam
{
    public partial class TankCleaning : UserControl
    {
        // Database connection
        private string dbPath = Path.Combine(Application.StartupPath, "Database", "Database_XN1.mdb");
        private string connStr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0}";

        // UI Controls
        private DataGridView dgv;
        private ComboBox cbWorker, cbWorkType;
        private DateTimePicker dtpDate;
        private Button btnAdd, btnSave;
        private DateTimePicker timePicker;
        private ContextMenuStrip contextMenu;
        private ToolStripMenuItem deleteMenuItem;

        // Data
        private int[] workerIds = new int[50];
        private float nightStart, nightEnd, lunchStart, lunchEnd;

        public TankCleaning()
        {
            InitializeComponent();
            this.Size = new Size(1423, 785);
            InitializeUI();
            LoadSettings();
            LoadWorkers();
            ApplyTheme();
            WireUpEventHandlers();
        }

        private void InitializeUI()
        {
            this.SuspendLayout();
            this.BackColor = ThemeManager.CurrentTheme == AppTheme.Dark ?
                Color.FromArgb(30, 30, 30) : SystemColors.Control;

            // Main container
            var mainContainer = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(0, 40, 0, 0)  // Top padding for header
            };
            this.Controls.Add(mainContainer);

            // Panel for DataGridView
            var gridContainer = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(10)
            };
            mainContainer.Controls.Add(gridContainer);

            // DataGridView
            dgv = new DataGridView
            {
                Dock = DockStyle.Fill,
                AllowUserToAddRows = false,
                RowHeadersWidth = 30,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                Font = new Font("Times New Roman", 12),
                SelectionMode = DataGridViewSelectionMode.FullRowSelect
            };
            gridContainer.Controls.Add(dgv);

            // Bottom Panel
            var bottomPanel = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 120,
                Padding = new Padding(20)
            };
            mainContainer.Controls.Add(bottomPanel);

            // Worker selection
            var lblWorker = new Label
            {
                Text = "Работник",
                Location = new Point(20, 20),
                Font = new Font("Times New Roman", 14),
                AutoSize = true
            };

            cbWorker = new ComboBox
            {
                Location = new Point(20, 50),
                Width = 300,
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Times New Roman", 12)
            };

            // Work Type selection
            var lblWorkType = new Label
            {
                Text = "Вид работы",
                Location = new Point(340, 20),
                Font = new Font("Times New Roman", 14),
                AutoSize = true
            };

            cbWorkType = new ComboBox
            {
                Location = new Point(340, 50),
                Width = 200,
                Font = new Font("Times New Roman", 12)
            };
            cbWorkType.Items.AddRange(new object[] { "Мойка баков", "Часовые", "Приём баков" });
            cbWorkType.SelectedIndex = 0;

            // Date selection
            var lblDate = new Label
            {
                Text = "Дата работы",
                Location = new Point(560, 20),
                Font = new Font("Times New Roman", 14),
                AutoSize = true
            };

            dtpDate = new DateTimePicker
            {
                Location = new Point(560, 50),
                Width = 150,
                Format = DateTimePickerFormat.Custom,
                CustomFormat = "dd.MM.yyyy",
                Font = new Font("Times New Roman", 12)
            };

            // Add Button
            btnAdd = new Button
            {
                Text = "Добавить",
                Location = new Point(730, 45),
                Size = new Size(100, 35),
                Font = new Font("Times New Roman", 12)
            };

            // Save Button
            btnSave = new Button
            {
                Text = "Сохранить все",
                Location = new Point(850, 45),
                Size = new Size(150, 35),
                Font = new Font("Times New Roman", 12, FontStyle.Bold)
            };

            // Time Picker for in-cell editing
            timePicker = new DateTimePicker
            {
                Format = DateTimePickerFormat.Custom,
                CustomFormat = "HH:mm",
                ShowUpDown = true,
                Visible = false,
                Width = 100
            };

            // Context Menu
            contextMenu = new ContextMenuStrip();
            deleteMenuItem = new ToolStripMenuItem("Удалить");
            contextMenu.Items.Add(deleteMenuItem);
            dgv.ContextMenuStrip = contextMenu;

            // Add controls to bottom panel
            bottomPanel.Controls.Add(lblWorker);
            bottomPanel.Controls.Add(cbWorker);
            bottomPanel.Controls.Add(lblWorkType);
            bottomPanel.Controls.Add(cbWorkType);
            bottomPanel.Controls.Add(lblDate);
            bottomPanel.Controls.Add(dtpDate);
            bottomPanel.Controls.Add(btnAdd);
            bottomPanel.Controls.Add(btnSave);
            gridContainer.Controls.Add(timePicker); // Add to grid container

            InitializeDataGrid();
            this.ResumeLayout(false);
        }

        private void InitializeDataGrid()
        {
            dgv.Columns.Clear();
            dgv.ReadOnly = false; // Enable editing

            // Create columns
            dgv.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "№",
                Width = 40,
                ReadOnly = true
            });

            dgv.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Фамилия Имя Отчество",
                Width = 250,
                ReadOnly = true
            });

            dgv.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Количество",
                Width = 100,
                ReadOnly = false
            });

            // ComboBox column for "Вых. день"
            var comboCol = new DataGridViewComboBoxColumn
            {
                HeaderText = "Вых. день",
                Width = 90,
                FlatStyle = FlatStyle.Flat
            };
            comboCol.Items.AddRange("Нет", "Да");
            dgv.Columns.Add(comboCol);

            dgv.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Начало раб.",
                Width = 100,
                ReadOnly = true
            });

            dgv.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Конец раб.",
                Width = 100,
                ReadOnly = true
            });

            dgv.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Вид работы",
                Width = 120,
                ReadOnly = true
            });

            dgv.Columns.Add(new DataGridViewTextBoxColumn
            {
                Visible = false  // Hidden ID
            });

            dgv.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Дата работы",
                Width = 120,
                ReadOnly = true,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Format = "dd.MM.yyyy"
                }
            });

            dgv.RowHeadersVisible = true;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void WireUpEventHandlers()
        {
            btnAdd.Click += BtnAdd_Click;
            btnSave.Click += BtnSave_Click;
            deleteMenuItem.Click += DeleteMenuItem_Click;
            dgv.CellClick += Dgv_CellClick;
            timePicker.ValueChanged += TimePicker_ValueChanged;
            timePicker.Leave += TimePicker_Leave;
            dgv.EditingControlShowing += Dgv_EditingControlShowing;
        }

        private void LoadWorkers()
        {
            cbWorker.Items.Clear();
            string connString = string.Format(connStr, dbPath);

            try
            {
                using (var conn = new OleDbConnection(connString))
                {
                    conn.Open();
                    var cmd = new OleDbCommand("SELECT ID, Fam, Imj, Otch FROM Working_sort ORDER BY Fam", conn);
                    var reader = cmd.ExecuteReader();

                    int index = 0;
                    while (reader.Read() && index < workerIds.Length)
                    {
                        workerIds[index] = Convert.ToInt32(reader["ID"]);
                        string fullName = $"{reader["Fam"]} {reader["Imj"]} {reader["Otch"]}";
                        cbWorker.Items.Add(fullName);
                        index++;
                    }
                }

                if (cbWorker.Items.Count > 0)
                    cbWorker.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки работников: {ex.Message}",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadSettings()
        {
            string connString = string.Format(connStr, dbPath);

            try
            {
                using (var conn = new OleDbConnection(connString))
                {
                    conn.Open();
                    var cmd = new OleDbCommand("SELECT * FROM Setting_sort", conn);
                    var reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        nightStart = TimeToHours(reader["Time_night_begin"]);
                        nightEnd = TimeToHours(reader["Time_night_end"]);
                        lunchStart = TimeToHours(reader["Obed_begin"]);
                        lunchEnd = TimeToHours(reader["Obed_end"]);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки настроек: {ex.Message}",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private float TimeToHours(object timeValue)
        {
            if (timeValue is DateTime dt)
            {
                return dt.Hour + dt.Minute / 60f;
            }
            return 0;
        }

        private void ApplyTheme()
        {
            var theme = ThemeManager.CurrentTheme;
            var isDark = theme == AppTheme.Dark;

            var backColor = isDark ? Color.FromArgb(45, 45, 48) : Color.White;
            var foreColor = isDark ? Color.White : Color.Black;
            var headerColor = isDark ? Color.FromArgb(70, 70, 80) : Color.LightGray;

            dgv.BackgroundColor = backColor;
            dgv.DefaultCellStyle.BackColor = backColor;
            dgv.DefaultCellStyle.ForeColor = foreColor;
            dgv.ColumnHeadersDefaultCellStyle.BackColor = headerColor;
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = isDark ? Color.White : Color.Black;
            dgv.RowHeadersDefaultCellStyle.BackColor = headerColor;
        }

        #region Event Handlers
        private void BtnAdd_Click(object sender, EventArgs e)
        {
            if (cbWorker.SelectedIndex == -1)
            {
                MessageBox.Show("Выберите работника", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int workerId = workerIds[cbWorker.SelectedIndex];
            string workType = cbWorkType.Text;
            string workDate = dtpDate.Value.ToString("dd.MM.yyyy");

            foreach (DataGridViewRow row in dgv.Rows)
            {
                if (row.Cells[7].Value?.ToString() == workerId.ToString() &&
                    row.Cells[6].Value?.ToString() == workType &&
                    row.Cells[8].Value?.ToString() == workDate)
                {
                    MessageBox.Show($"У работника {cbWorker.Text}\nуже добавлена {workType} на дату {workDate}.",
                        "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }

            int rowIndex = dgv.Rows.Add();
            dgv.Rows[rowIndex].Cells[0].Value = dgv.Rows.Count;
            dgv.Rows[rowIndex].Cells[1].Value = cbWorker.Text;
            dgv.Rows[rowIndex].Cells[2].Value = (cbWorkType.SelectedIndex == 1) ? "00:00" : "0";
            dgv.Rows[rowIndex].Cells[3].Value = "Нет";
            dgv.Rows[rowIndex].Cells[4].Value = "08:00";
            dgv.Rows[rowIndex].Cells[5].Value = "17:00";
            dgv.Rows[rowIndex].Cells[6].Value = workType;
            dgv.Rows[rowIndex].Cells[7].Value = workerId;
            dgv.Rows[rowIndex].Cells[8].Value = workDate;

            btnSave.Enabled = true;
        }

        private void Dgv_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

            // Handle time cells
            if (e.ColumnIndex == 4 || e.ColumnIndex == 5)
            {
                DataGridViewCell cell = dgv.Rows[e.RowIndex].Cells[e.ColumnIndex];
                Rectangle cellRect = dgv.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, true);


                // Calculate position relative to DataGridView
                Point dgvScreenPos = dgv.PointToScreen(Point.Empty);
                timePicker.Location = new Point(
                    dgvScreenPos.X + cellRect.Left - 250,
                    dgvScreenPos.Y + cellRect.Top - 50
                );
                timePicker.Size = new Size(cellRect.Width, cellRect.Height);

                if (DateTime.TryParseExact(cell.Value?.ToString(), "HH:mm",
                    CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime time))
                {
                    timePicker.Value = time;
                }
                else
                {
                    timePicker.Value = DateTime.Today.AddHours(8);
                }

                timePicker.Tag = new Point(e.ColumnIndex, e.RowIndex);
                timePicker.Visible = true;
                timePicker.BringToFront();
            }
            else
            {
                timePicker.Visible = false;
            }
        }

        private void TimePicker_ValueChanged(object sender, EventArgs e)
        {
            if (timePicker.Tag is Point cellLocation)
            {
                dgv.Rows[cellLocation.Y].Cells[cellLocation.X].Value =
                    timePicker.Value.ToString("HH:mm");
            }
        }

        private void TimePicker_Leave(object sender, EventArgs e)
        {
            timePicker.Visible = false;
        }

        private void Dgv_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            // Numeric validation for quantity column (non-hourly work)
            if (dgv.CurrentCell.ColumnIndex == 2 &&
                dgv.CurrentRow.Cells[6].Value?.ToString() != "Часовые")
            {
                if (e.Control is TextBox tb)
                {
                    tb.KeyPress += (s, args) =>
                    {
                        if (!char.IsDigit(args.KeyChar) &&
                            args.KeyChar != (char)Keys.Back)
                        {
                            args.Handled = true;
                        }
                    };
                }
            }
        }

        private void DeleteMenuItem_Click(object sender, EventArgs e)
        {
            if (dgv.SelectedRows.Count > 0)
            {
                dgv.Rows.RemoveAt(dgv.SelectedRows[0].Index);
                btnSave.Enabled = dgv.Rows.Count > 0;

                for (int i = 0; i < dgv.Rows.Count; i++)
                {
                    dgv.Rows[i].Cells[0].Value = i + 1;
                }
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (ValidateData())
            {
                SaveData();
                MessageBox.Show("Данные успешно сохранены!", "Успех",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                btnSave.Enabled = false;
            }
        }
        #endregion

        #region Data Operations
        private bool ValidateData()
        {
            foreach (DataGridViewRow row in dgv.Rows)
            {
                string workType = row.Cells[6].Value?.ToString();
                string quantity = row.Cells[2].Value?.ToString();

                if ((workType == "Мойка баков" || workType == "Приём баков") &&
                    (string.IsNullOrWhiteSpace(quantity) || quantity == "0"))
                {
                    string worker = row.Cells[1].Value?.ToString();
                    string date = row.Cells[8].Value?.ToString();

                    if (MessageBox.Show($"У работника {worker}\nне добавлено количество на дату {date}.\n" +
                        "Изменить запись?\nДа - перейти к исправлению\nНет - удалить запись",
                        "Предупреждение", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                    {
                        dgv.CurrentCell = row.Cells[2];
                        return false;
                    }
                    else
                    {
                        dgv.Rows.Remove(row);
                    }
                }
            }
            return true;
        }

        private void SaveData()
        {
            string connString = string.Format(connStr, dbPath);

            using (var conn = new OleDbConnection(connString))
            {
                conn.Open();

                foreach (DataGridViewRow row in dgv.Rows)
                {
                    int workerId = Convert.ToInt32(row.Cells[7].Value);
                    string tableName = $"Working_sort_{workerId}";
                    DateTime workDate = DateTime.ParseExact(
                        row.Cells[8].Value.ToString(),
                        "dd.MM.yyyy",
                        CultureInfo.InvariantCulture
                    );
                    string workType = row.Cells[6].Value.ToString();
                    string quantity = row.Cells[2].Value.ToString();
                    bool isOutput = row.Cells[3].Value.ToString() == "Да";

                    float startTime = TimeToHours(row.Cells[4].Value.ToString());
                    float endTime = TimeToHours(row.Cells[5].Value.ToString());
                    var (totalHours, nightHours) = CalculateWorkHours(startTime, endTime);

                    bool recordExists = false;
                    var checkCmd = new OleDbCommand(
                        $"SELECT COUNT(*) FROM [{tableName}] WHERE date_work = @workDate", conn);
                    checkCmd.Parameters.AddWithValue("@workDate", workDate);
                    recordExists = (int)checkCmd.ExecuteScalar() > 0;

                    string fieldName, outputFieldName;
                    bool isHourly = workType == "Часовые";

                    if (workType == "Мойка баков")
                    {
                        fieldName = "Metio_BAK";
                        outputFieldName = "Metio_BAK_VECH";
                    }
                    else if (workType == "Часовые")
                    {
                        fieldName = "DR_work";
                        outputFieldName = "DR_work_vech";
                    }
                    else
                    {
                        fieldName = "Priem_BAK";
                        outputFieldName = "Priem_BAK_vech";
                    }

                    OleDbCommand cmd;
                    if (recordExists)
                    {
                        cmd = new OleDbCommand(
                            $"UPDATE [{tableName}] SET " +
                            $"[{fieldName}] = [{fieldName}] + @quantity, " +
                            $"[{outputFieldName}] = [{outputFieldName}] + @output, " +
                            "[time_all] = [time_all] + @total, " +
                            "[time_night] = [time_night] + @night " +
                            "WHERE date_work = @workDate", conn);
                    }
                    else
                    {
                        cmd = new OleDbCommand(
                            $"INSERT INTO [{tableName}] (date_work, at_work, {fieldName}, {outputFieldName}, " +
                            "time_all, time_night) VALUES (@workDate, true, @quantity, @output, @total, @night)", conn);
                    }

                    cmd.Parameters.AddWithValue("@quantity", isHourly ? TimeToHours(quantity) : Convert.ToInt32(quantity));
                    cmd.Parameters.AddWithValue("@output", isHourly ? (isOutput ? TimeToHours(quantity) : 0) : (isOutput ? Convert.ToInt32(quantity) : 0));
                    cmd.Parameters.AddWithValue("@total", totalHours);
                    cmd.Parameters.AddWithValue("@night", nightHours);
                    cmd.Parameters.AddWithValue("@workDate", workDate);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        private float TimeToHours(string time)
        {
            if (DateTime.TryParseExact(time, "HH:mm", CultureInfo.InvariantCulture,
                DateTimeStyles.None, out DateTime result))
            {
                return result.Hour + result.Minute / 60f;
            }
            return 0;
        }

        private (float totalHours, float nightHours) CalculateWorkHours(float start, float end)
        {
            if (end < start) end += 24;

            float total = end - start;

            if (start < lunchEnd && end > lunchStart)
            {
                total -= (lunchEnd - lunchStart);
            }

            float night = 0;
            if (end > nightStart)
            {
                float adjustedNightEnd = nightEnd < nightStart ? nightEnd + 24 : nightEnd;

                float nightStartPoint = Math.Max(start, nightStart);
                float nightEndPoint = Math.Min(end, adjustedNightEnd);

                if (nightEndPoint > nightStartPoint)
                {
                    night = nightEndPoint - nightStartPoint;

                    if (lunchStart > nightStartPoint && lunchEnd < nightEndPoint)
                    {
                        night -= (lunchEnd - lunchStart);
                    }
                }
            }

            return (total, night);
        }
        #endregion
    }
}