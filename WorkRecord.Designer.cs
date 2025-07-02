namespace Program_na_Ryadam
{
    partial class WorkRecord
    {
        private System.ComponentModel.IContainer components = null;


        #region Component Designer generated code

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.dtpQuantity = new System.Windows.Forms.DateTimePicker();
            this.Panel1 = new System.Windows.Forms.Panel();
            this.GroupBox1 = new System.Windows.Forms.GroupBox();
            this.Label2 = new System.Windows.Forms.Label();
            this.Label8 = new System.Windows.Forms.Label();
            this.Label1 = new System.Windows.Forms.Label();
            this.Label4 = new System.Windows.Forms.Label();
            this.cbDepartment = new System.Windows.Forms.ComboBox();
            this.cbBrigade = new System.Windows.Forms.ComboBox();
            this.cbWorker = new System.Windows.Forms.ComboBox();
            this.cbWorkType = new System.Windows.Forms.ComboBox();
            this.CheckBox1 = new System.Windows.Forms.CheckBox();
            this.btnOpenWorkList = new System.Windows.Forms.Button();
            this.btnWorkByDepartments = new System.Windows.Forms.Button();
            this.GroupBox2 = new System.Windows.Forms.GroupBox();
            this.Label6 = new System.Windows.Forms.Label();
            this.Label5 = new System.Windows.Forms.Label();
            this.Label7 = new System.Windows.Forms.Label();
            this.Label3 = new System.Windows.Forms.Label();
            this.dtpWorkDate = new System.Windows.Forms.DateTimePicker();
            this.dtpHours = new System.Windows.Forms.DateTimePicker();
            this.btnAddWork = new System.Windows.Forms.Button();
            this.dtpStartTime = new System.Windows.Forms.DateTimePicker();
            this.txtQuantity = new System.Windows.Forms.TextBox();
            this.GroupBox3 = new System.Windows.Forms.GroupBox();
            this.btnClearAll = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.Panel2 = new System.Windows.Forms.Panel();
            this.dgvWorkRecords = new System.Windows.Forms.DataGridView();
            this.colIndex = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colWorker = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colWorkName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colQuantity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colWorkType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colConfirmed = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colStart = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colEnd = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colWorkerID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colWorkTypeID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLocation = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDeptIDs = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLunch = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PopupMenu1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.miDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.miDeleteAll = new System.Windows.Forms.ToolStripMenuItem();
            this.dtpGridTime = new System.Windows.Forms.DateTimePicker();
            this.cbGridConfirm = new System.Windows.Forms.ComboBox();
            this.dtpGridDate = new System.Windows.Forms.DateTimePicker();
            this.dtpEndTime = new System.Windows.Forms.DateTimePicker();
            this.Panel1.SuspendLayout();
            this.GroupBox1.SuspendLayout();
            this.GroupBox2.SuspendLayout();
            this.GroupBox3.SuspendLayout();
            this.Panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvWorkRecords)).BeginInit();
            this.PopupMenu1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dtpQuantity
            // 
            this.dtpQuantity.CustomFormat = "HH:mm";
            this.dtpQuantity.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpQuantity.Location = new System.Drawing.Point(152, 51);
            this.dtpQuantity.Name = "dtpQuantity";
            this.dtpQuantity.ShowUpDown = true;
            this.dtpQuantity.Size = new System.Drawing.Size(85, 32);
            this.dtpQuantity.TabIndex = 4;
            // 
            // Panel1
            // 
            this.Panel1.Controls.Add(this.GroupBox1);
            this.Panel1.Controls.Add(this.GroupBox2);
            this.Panel1.Controls.Add(this.GroupBox3);
            this.Panel1.Controls.Add(this.btnRefresh);
            this.Panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.Panel1.Location = new System.Drawing.Point(0, 429);
            this.Panel1.Name = "Panel1";
            this.Panel1.Size = new System.Drawing.Size(1380, 280);
            this.Panel1.TabIndex = 0;
            // 
            // GroupBox1
            // 
            this.GroupBox1.Controls.Add(this.Label2);
            this.GroupBox1.Controls.Add(this.Label8);
            this.GroupBox1.Controls.Add(this.btnOpenWorkList);
            this.GroupBox1.Controls.Add(this.btnWorkByDepartments);
            this.GroupBox1.Controls.Add(this.Label1);
            this.GroupBox1.Controls.Add(this.Label4);
            this.GroupBox1.Controls.Add(this.cbDepartment);
            this.GroupBox1.Controls.Add(this.cbBrigade);
            this.GroupBox1.Controls.Add(this.cbWorker);
            this.GroupBox1.Controls.Add(this.cbWorkType);
            this.GroupBox1.Controls.Add(this.CheckBox1);
            this.GroupBox1.Font = new System.Drawing.Font("Consolas", 16F);
            this.GroupBox1.Location = new System.Drawing.Point(496, 3);
            this.GroupBox1.Name = "GroupBox1";
            this.GroupBox1.Size = new System.Drawing.Size(651, 263);
            this.GroupBox1.TabIndex = 0;
            this.GroupBox1.TabStop = false;
            this.GroupBox1.Text = "Выбор работника и работы";
            // 
            // Label2
            // 
            this.Label2.AutoSize = true;
            this.Label2.Location = new System.Drawing.Point(6, 34);
            this.Label2.Name = "Label2";
            this.Label2.Size = new System.Drawing.Size(180, 26);
            this.Label2.TabIndex = 0;
            this.Label2.Text = "Подразделение:";
            // 
            // Label8
            // 
            this.Label8.AutoSize = true;
            this.Label8.Location = new System.Drawing.Point(3, 74);
            this.Label8.Name = "Label8";
            this.Label8.Size = new System.Drawing.Size(108, 26);
            this.Label8.TabIndex = 1;
            this.Label8.Text = "Бригада:";
            // 
            // Label1
            // 
            this.Label1.AutoSize = true;
            this.Label1.Location = new System.Drawing.Point(3, 118);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(204, 26);
            this.Label1.TabIndex = 2;
            this.Label1.Text = "Выбор работника:";
            // 
            // Label4
            // 
            this.Label4.AutoSize = true;
            this.Label4.Location = new System.Drawing.Point(1, 153);
            this.Label4.Name = "Label4";
            this.Label4.Size = new System.Drawing.Size(204, 26);
            this.Label4.TabIndex = 3;
            this.Label4.Text = "Название работы:";
            // 
            // cbDepartment
            // 
            this.cbDepartment.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbDepartment.Font = new System.Drawing.Font("Consolas", 16F);
            this.cbDepartment.FormattingEnabled = true;
            this.cbDepartment.Location = new System.Drawing.Point(259, 31);
            this.cbDepartment.Name = "cbDepartment";
            this.cbDepartment.Size = new System.Drawing.Size(392, 32);
            this.cbDepartment.TabIndex = 4;
            // 
            // cbBrigade
            // 
            this.cbBrigade.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbBrigade.Font = new System.Drawing.Font("Consolas", 16F);
            this.cbBrigade.FormattingEnabled = true;
            this.cbBrigade.Location = new System.Drawing.Point(253, 74);
            this.cbBrigade.Name = "cbBrigade";
            this.cbBrigade.Size = new System.Drawing.Size(392, 32);
            this.cbBrigade.TabIndex = 5;
            // 
            // cbWorker
            // 
            this.cbWorker.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbWorker.Font = new System.Drawing.Font("Consolas", 16F);
            this.cbWorker.FormattingEnabled = true;
            this.cbWorker.Location = new System.Drawing.Point(253, 112);
            this.cbWorker.Name = "cbWorker";
            this.cbWorker.Size = new System.Drawing.Size(392, 32);
            this.cbWorker.TabIndex = 6;
            // 
            // cbWorkType
            // 
            this.cbWorkType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbWorkType.Font = new System.Drawing.Font("Consolas", 16F);
            this.cbWorkType.FormattingEnabled = true;
            this.cbWorkType.Location = new System.Drawing.Point(253, 150);
            this.cbWorkType.Name = "cbWorkType";
            this.cbWorkType.Size = new System.Drawing.Size(392, 32);
            this.cbWorkType.TabIndex = 7;
            // 
            // CheckBox1
            // 
            this.CheckBox1.AutoSize = true;
            this.CheckBox1.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.CheckBox1.Location = new System.Drawing.Point(6, 239);
            this.CheckBox1.Name = "CheckBox1";
            this.CheckBox1.Size = new System.Drawing.Size(397, 18);
            this.CheckBox1.TabIndex = 8;
            this.CheckBox1.Text = "Включить все работы независимо подразделений и бригад";
            this.CheckBox1.UseVisualStyleBackColor = true;
            // 
            // btnOpenWorkList
            // 
            this.btnOpenWorkList.Font = new System.Drawing.Font("Consolas", 16F);
            this.btnOpenWorkList.Location = new System.Drawing.Point(338, 189);
            this.btnOpenWorkList.Name = "btnOpenWorkList";
            this.btnOpenWorkList.Size = new System.Drawing.Size(307, 44);
            this.btnOpenWorkList.TabIndex = 9;
            this.btnOpenWorkList.Text = "Работа";
            this.btnOpenWorkList.UseVisualStyleBackColor = true;
            this.btnOpenWorkList.Click += new System.EventHandler(this.btnOpenWorkList_Click);
            // 
            // btnWorkByDepartments
            // 
            this.btnWorkByDepartments.Font = new System.Drawing.Font("Consolas", 16F);
            this.btnWorkByDepartments.Location = new System.Drawing.Point(6, 189);
            this.btnWorkByDepartments.Name = "btnWorkByDepartments";
            this.btnWorkByDepartments.Size = new System.Drawing.Size(326, 44);
            this.btnWorkByDepartments.TabIndex = 10;
            this.btnWorkByDepartments.Text = "Работа по отделениям";
            this.btnWorkByDepartments.UseVisualStyleBackColor = true;
            this.btnWorkByDepartments.Click += new System.EventHandler(this.btnWorkByDepartments_Click);
            // 
            // GroupBox2
            // 
            this.GroupBox2.Controls.Add(this.Label6);
            this.GroupBox2.Controls.Add(this.Label5);
            this.GroupBox2.Controls.Add(this.Label7);
            this.GroupBox2.Controls.Add(this.Label3);
            this.GroupBox2.Controls.Add(this.dtpQuantity);
            this.GroupBox2.Controls.Add(this.dtpEndTime);
            this.GroupBox2.Controls.Add(this.dtpWorkDate);
            this.GroupBox2.Controls.Add(this.dtpHours);
            this.GroupBox2.Controls.Add(this.btnAddWork);
            this.GroupBox2.Controls.Add(this.txtQuantity);
            this.GroupBox2.Controls.Add(this.dtpStartTime);
            this.GroupBox2.Font = new System.Drawing.Font("Consolas", 16F);
            this.GroupBox2.Location = new System.Drawing.Point(1, 1);
            this.GroupBox2.Name = "GroupBox2";
            this.GroupBox2.Size = new System.Drawing.Size(470, 265);
            this.GroupBox2.TabIndex = 1;
            this.GroupBox2.TabStop = false;
            this.GroupBox2.Text = "Выбор рабочего дня и количества";
            // 
            // Label6
            // 
            this.Label6.AutoSize = true;
            this.Label6.Location = new System.Drawing.Point(2, 203);
            this.Label6.Name = "Label6";
            this.Label6.Size = new System.Drawing.Size(180, 26);
            this.Label6.TabIndex = 0;
            this.Label6.Text = "Конец раб. дня";
            // 
            // Label5
            // 
            this.Label5.AutoSize = true;
            this.Label5.Location = new System.Drawing.Point(2, 161);
            this.Label5.Name = "Label5";
            this.Label5.Size = new System.Drawing.Size(192, 26);
            this.Label5.TabIndex = 1;
            this.Label5.Text = "Начало раб. дня";
            // 
            // Label7
            // 
            this.Label7.AutoSize = true;
            this.Label7.Location = new System.Drawing.Point(2, 105);
            this.Label7.Name = "Label7";
            this.Label7.Size = new System.Drawing.Size(288, 26);
            this.Label7.TabIndex = 2;
            this.Label7.Text = "Начало рабочего времени";
            // 
            // Label3
            // 
            this.Label3.AutoSize = true;
            this.Label3.Location = new System.Drawing.Point(2, 51);
            this.Label3.Name = "Label3";
            this.Label3.Size = new System.Drawing.Size(144, 26);
            this.Label3.TabIndex = 3;
            this.Label3.Text = "Количество:";
            // 
            // dtpWorkDate
            // 
            this.dtpWorkDate.CustomFormat = "dd.MM.yyyy";
            this.dtpWorkDate.Font = new System.Drawing.Font("Consolas", 16F);
            this.dtpWorkDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpWorkDate.Location = new System.Drawing.Point(319, 105);
            this.dtpWorkDate.Name = "dtpWorkDate";
            this.dtpWorkDate.Size = new System.Drawing.Size(129, 32);
            this.dtpWorkDate.TabIndex = 5;
            // 
            // dtpHours
            // 
            this.dtpHours.CustomFormat = "HH:mm";
            this.dtpHours.Font = new System.Drawing.Font("Consolas", 16F);
            this.dtpHours.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpHours.Location = new System.Drawing.Point(243, 51);
            this.dtpHours.Name = "dtpHours";
            this.dtpHours.ShowUpDown = true;
            this.dtpHours.Size = new System.Drawing.Size(83, 32);
            this.dtpHours.TabIndex = 6;
            // 
            // btnAddWork
            // 
            this.btnAddWork.Font = new System.Drawing.Font("Consolas", 16F);
            this.btnAddWork.Location = new System.Drawing.Point(300, 161);
            this.btnAddWork.Name = "btnAddWork";
            this.btnAddWork.Size = new System.Drawing.Size(161, 71);
            this.btnAddWork.TabIndex = 7;
            this.btnAddWork.Text = "Добавить";
            this.btnAddWork.UseVisualStyleBackColor = true;
            // 
            // dtpStartTime
            // 
            this.dtpStartTime.CustomFormat = "HH:mm";
            this.dtpStartTime.Font = new System.Drawing.Font("Consolas", 16F);
            this.dtpStartTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpStartTime.Location = new System.Drawing.Point(205, 155);
            this.dtpStartTime.Name = "dtpStartTime";
            this.dtpStartTime.ShowUpDown = true;
            this.dtpStartTime.Size = new System.Drawing.Size(89, 32);
            this.dtpStartTime.TabIndex = 8;
            // 
            // txtQuantity
            // 
            this.txtQuantity.Font = new System.Drawing.Font("Consolas", 16F);
            this.txtQuantity.Location = new System.Drawing.Point(340, 51);
            this.txtQuantity.Name = "txtQuantity";
            this.txtQuantity.Size = new System.Drawing.Size(121, 32);
            this.txtQuantity.TabIndex = 9;
            // 
            // GroupBox3
            // 
            this.GroupBox3.Controls.Add(this.btnClearAll);
            this.GroupBox3.Controls.Add(this.btnSave);
            this.GroupBox3.Font = new System.Drawing.Font("Consolas", 16F);
            this.GroupBox3.Location = new System.Drawing.Point(1153, 1);
            this.GroupBox3.Name = "GroupBox3";
            this.GroupBox3.Size = new System.Drawing.Size(224, 265);
            this.GroupBox3.TabIndex = 2;
            this.GroupBox3.TabStop = false;
            this.GroupBox3.Text = "Очистка и Сохранение";
            // 
            // btnClearAll
            // 
            this.btnClearAll.Font = new System.Drawing.Font("Consolas", 19F);
            this.btnClearAll.Location = new System.Drawing.Point(6, 167);
            this.btnClearAll.Name = "btnClearAll";
            this.btnClearAll.Size = new System.Drawing.Size(212, 66);
            this.btnClearAll.TabIndex = 0;
            this.btnClearAll.Text = "Удалить все";
            this.btnClearAll.UseVisualStyleBackColor = true;
            // 
            // btnSave
            // 
            this.btnSave.Font = new System.Drawing.Font("Consolas", 19F);
            this.btnSave.Location = new System.Drawing.Point(6, 51);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(212, 108);
            this.btnSave.TabIndex = 1;
            this.btnSave.Text = "Сохранить";
            this.btnSave.UseVisualStyleBackColor = true;
            // 
            // btnRefresh
            // 
            this.btnRefresh.Location = new System.Drawing.Point(477, 14);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(10, 251);
            this.btnRefresh.TabIndex = 3;
            this.btnRefresh.UseVisualStyleBackColor = true;
            // 
            // Panel2
            // 
            this.Panel2.Controls.Add(this.dgvWorkRecords);
            this.Panel2.Controls.Add(this.dtpGridTime);
            this.Panel2.Controls.Add(this.cbGridConfirm);
            this.Panel2.Controls.Add(this.dtpGridDate);
            this.Panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Panel2.Location = new System.Drawing.Point(0, 0);
            this.Panel2.Name = "Panel2";
            this.Panel2.Size = new System.Drawing.Size(1380, 429);
            this.Panel2.TabIndex = 1;
            // 
            // dgvWorkRecords
            // 
            this.dgvWorkRecords.AllowUserToAddRows = false;
            this.dgvWorkRecords.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvWorkRecords.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colIndex,
            this.colWorker,
            this.colWorkName,
            this.colQuantity,
            this.colWorkType,
            this.colConfirmed,
            this.colStart,
            this.colEnd,
            this.colWorkerID,
            this.colWorkTypeID,
            this.colDate,
            this.colLocation,
            this.colDeptIDs,
            this.colLunch});
            this.dgvWorkRecords.ContextMenuStrip = this.PopupMenu1;
            this.dgvWorkRecords.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvWorkRecords.Location = new System.Drawing.Point(0, 0);
            this.dgvWorkRecords.Name = "dgvWorkRecords";
            this.dgvWorkRecords.RowHeadersVisible = false;
            this.dgvWorkRecords.Size = new System.Drawing.Size(1380, 429);
            this.dgvWorkRecords.TabIndex = 0;
            // 
            // colIndex
            // 
            this.colIndex.HeaderText = "№";
            this.colIndex.Name = "colIndex";
            this.colIndex.Width = 35;
            // 
            // colWorker
            // 
            this.colWorker.HeaderText = "Фамилия Имя Отчество";
            this.colWorker.Name = "colWorker";
            this.colWorker.Width = 270;
            // 
            // colWorkName
            // 
            this.colWorkName.HeaderText = "Название работы";
            this.colWorkName.Name = "colWorkName";
            this.colWorkName.Width = 250;
            // 
            // colQuantity
            // 
            this.colQuantity.HeaderText = "КОЛИЧ.";
            this.colQuantity.Name = "colQuantity";
            this.colQuantity.Width = 90;
            // 
            // colWorkType
            // 
            this.colWorkType.HeaderText = "Вид Р.";
            this.colWorkType.Name = "colWorkType";
            this.colWorkType.Width = 90;
            // 
            // colConfirmed
            // 
            this.colConfirmed.HeaderText = "ВД";
            this.colConfirmed.Name = "colConfirmed";
            this.colConfirmed.Width = 50;
            // 
            // colStart
            // 
            this.colStart.HeaderText = "Начало";
            this.colStart.Name = "colStart";
            this.colStart.Width = 75;
            // 
            // colEnd
            // 
            this.colEnd.HeaderText = "Конец";
            this.colEnd.Name = "colEnd";
            this.colEnd.Width = 75;
            // 
            // colWorkerID
            // 
            this.colWorkerID.HeaderText = "WorkerID";
            this.colWorkerID.Name = "colWorkerID";
            this.colWorkerID.Visible = false;
            // 
            // colWorkTypeID
            // 
            this.colWorkTypeID.HeaderText = "WorkTypeID";
            this.colWorkTypeID.Name = "colWorkTypeID";
            this.colWorkTypeID.Visible = false;
            // 
            // colDate
            // 
            this.colDate.HeaderText = "Дата";
            this.colDate.Name = "colDate";
            // 
            // colLocation
            // 
            this.colLocation.HeaderText = "Место";
            this.colLocation.Name = "colLocation";
            this.colLocation.Width = 80;
            // 
            // colDeptIDs
            // 
            this.colDeptIDs.HeaderText = "DeptIDs";
            this.colDeptIDs.Name = "colDeptIDs";
            this.colDeptIDs.Visible = false;
            // 
            // colLunch
            // 
            this.colLunch.HeaderText = "Обед";
            this.colLunch.Name = "colLunch";
            this.colLunch.Width = 80;
            // 
            // PopupMenu1
            // 
            this.PopupMenu1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miDelete,
            this.miDeleteAll});
            this.PopupMenu1.Name = "PopupMenu1";
            this.PopupMenu1.Size = new System.Drawing.Size(140, 48);
            // 
            // miDelete
            // 
            this.miDelete.Name = "miDelete";
            this.miDelete.Size = new System.Drawing.Size(139, 22);
            this.miDelete.Text = "Удалить";
            // 
            // miDeleteAll
            // 
            this.miDeleteAll.Name = "miDeleteAll";
            this.miDeleteAll.Size = new System.Drawing.Size(139, 22);
            this.miDeleteAll.Text = "Удалить все";
            // 
            // dtpGridTime
            // 
            this.dtpGridTime.CustomFormat = "HH:mm";
            this.dtpGridTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpGridTime.Location = new System.Drawing.Point(376, 30);
            this.dtpGridTime.Name = "dtpGridTime";
            this.dtpGridTime.ShowUpDown = true;
            this.dtpGridTime.Size = new System.Drawing.Size(73, 20);
            this.dtpGridTime.TabIndex = 1;
            this.dtpGridTime.Visible = false;
            // 
            // cbGridConfirm
            // 
            this.cbGridConfirm.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbGridConfirm.FormattingEnabled = true;
            this.cbGridConfirm.Items.AddRange(new object[] {
            "Нет",
            "Да"});
            this.cbGridConfirm.Location = new System.Drawing.Point(376, 64);
            this.cbGridConfirm.Name = "cbGridConfirm";
            this.cbGridConfirm.Size = new System.Drawing.Size(73, 21);
            this.cbGridConfirm.TabIndex = 2;
            this.cbGridConfirm.Visible = false;
            // 
            // dtpGridDate
            // 
            this.dtpGridDate.CustomFormat = "dd.MM.yyyy";
            this.dtpGridDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpGridDate.Location = new System.Drawing.Point(496, 64);
            this.dtpGridDate.Name = "dtpGridDate";
            this.dtpGridDate.Size = new System.Drawing.Size(114, 20);
            this.dtpGridDate.TabIndex = 3;
            this.dtpGridDate.Visible = false;
            // 
            // dtpEndTime
            // 
            this.dtpEndTime.CustomFormat = "HH:mm";
            this.dtpEndTime.Font = new System.Drawing.Font("Consolas", 16F);
            this.dtpEndTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpEndTime.Location = new System.Drawing.Point(205, 201);
            this.dtpEndTime.Name = "dtpEndTime";
            this.dtpEndTime.ShowUpDown = true;
            this.dtpEndTime.Size = new System.Drawing.Size(89, 32);
            this.dtpEndTime.TabIndex = 4;
            // 
            // WorkRecord
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.Panel2);
            this.Controls.Add(this.Panel1);
            this.Margin = new System.Windows.Forms.Padding(0, 80, 0, 0);
            this.Name = "WorkRecord";
            this.Size = new System.Drawing.Size(1380, 709);
            this.Panel1.ResumeLayout(false);
            this.GroupBox1.ResumeLayout(false);
            this.GroupBox1.PerformLayout();
            this.GroupBox2.ResumeLayout(false);
            this.GroupBox2.PerformLayout();
            this.GroupBox3.ResumeLayout(false);
            this.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvWorkRecords)).EndInit();
            this.PopupMenu1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel Panel1;
        private System.Windows.Forms.GroupBox GroupBox1;
        private System.Windows.Forms.Label Label2;
        private System.Windows.Forms.Label Label8;
        private System.Windows.Forms.Label Label1;
        private System.Windows.Forms.Label Label4;
        private System.Windows.Forms.ComboBox cbDepartment;
        private System.Windows.Forms.ComboBox cbBrigade;
        private System.Windows.Forms.ComboBox cbWorker;
        private System.Windows.Forms.ComboBox cbWorkType;
        private System.Windows.Forms.CheckBox CheckBox1;
        private System.Windows.Forms.Button btnOpenWorkList;
        private System.Windows.Forms.Button btnWorkByDepartments;
        private System.Windows.Forms.GroupBox GroupBox2;
        private System.Windows.Forms.Label Label6;
        private System.Windows.Forms.Label Label5;
        private System.Windows.Forms.Label Label7;
        private System.Windows.Forms.Label Label3;
        private System.Windows.Forms.DateTimePicker dtpWorkDate;
        private System.Windows.Forms.DateTimePicker dtpHours;
        private System.Windows.Forms.Button btnAddWork;
        private System.Windows.Forms.DateTimePicker dtpStartTime;
        private System.Windows.Forms.TextBox txtQuantity;
        private System.Windows.Forms.GroupBox GroupBox3;
        private System.Windows.Forms.Button btnClearAll;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Panel Panel2;
        private System.Windows.Forms.DataGridView dgvWorkRecords;
        private System.Windows.Forms.DateTimePicker dtpGridTime;
        private System.Windows.Forms.ComboBox cbGridConfirm;
        private System.Windows.Forms.DateTimePicker dtpGridDate;
        private System.Windows.Forms.ContextMenuStrip PopupMenu1;
        private System.Windows.Forms.ToolStripMenuItem miDelete;
        private System.Windows.Forms.ToolStripMenuItem miDeleteAll;
        private System.Windows.Forms.DataGridViewTextBoxColumn colIndex;
        private System.Windows.Forms.DataGridViewTextBoxColumn colWorker;
        private System.Windows.Forms.DataGridViewTextBoxColumn colWorkName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colQuantity;
        private System.Windows.Forms.DataGridViewTextBoxColumn colWorkType;
        private System.Windows.Forms.DataGridViewTextBoxColumn colConfirmed;
        private System.Windows.Forms.DataGridViewTextBoxColumn colStart;
        private System.Windows.Forms.DataGridViewTextBoxColumn colEnd;
        private System.Windows.Forms.DataGridViewTextBoxColumn colWorkerID;
        private System.Windows.Forms.DataGridViewTextBoxColumn colWorkTypeID;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLocation;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDeptIDs;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLunch;
        private System.Windows.Forms.DateTimePicker dtpEndTime;
    }
}