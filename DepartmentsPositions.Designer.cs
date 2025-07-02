namespace Program_na_Ryadam
{
    partial class DepartmentsPositions
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        private void InitializeComponent()
        {
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.panelLeft = new System.Windows.Forms.Panel();
            this.groupBoxDepartments = new System.Windows.Forms.GroupBox();
            this.gridDepartments = new System.Windows.Forms.DataGridView();
            this.panelDeptDetails = new System.Windows.Forms.Panel();
            this.btnCancelDept = new System.Windows.Forms.Button();
            this.btnEditDept = new System.Windows.Forms.Button();
            this.btnAddDept = new System.Windows.Forms.Button();
            this.txtDeptDesc = new System.Windows.Forms.TextBox();
            this.labelDeptDesc = new System.Windows.Forms.Label();
            this.cmbBrigadeDivision = new System.Windows.Forms.ComboBox();
            this.txtBrigadeCount = new System.Windows.Forms.TextBox();
            this.labelBrigadeCount = new System.Windows.Forms.Label();
            this.labelBrigadeDivision = new System.Windows.Forms.Label();
            this.txtDeptName = new System.Windows.Forms.TextBox();
            this.labelDeptName = new System.Windows.Forms.Label();
            this.groupBoxBrigades = new System.Windows.Forms.GroupBox();
            this.panelBrigadeDetails = new System.Windows.Forms.Panel();
            this.btnSaveBrigade = new System.Windows.Forms.Button();
            this.btnEditBrigade = new System.Windows.Forms.Button();
            this.txtBrigadeName = new System.Windows.Forms.TextBox();
            this.gridBrigades = new System.Windows.Forms.DataGridView();
            this.panelRight = new System.Windows.Forms.Panel();
            this.groupBoxPositions = new System.Windows.Forms.GroupBox();
            this.gridPositions = new System.Windows.Forms.DataGridView();
            this.panelPositionDetails = new System.Windows.Forms.Panel();
            this.btnCancelPosition = new System.Windows.Forms.Button();
            this.btnEditPosition = new System.Windows.Forms.Button();
            this.btnAddPosition = new System.Windows.Forms.Button();
            this.txtPositionDesc = new System.Windows.Forms.TextBox();
            this.labelPositionDesc = new System.Windows.Forms.Label();
            this.txtPositionCount = new System.Windows.Forms.TextBox();
            this.labelPositionCount = new System.Windows.Forms.Label();
            this.txtPositionName = new System.Windows.Forms.TextBox();
            this.labelPositionName = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.panelLeft.SuspendLayout();
            this.groupBoxDepartments.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridDepartments)).BeginInit();
            this.panelDeptDetails.SuspendLayout();
            this.groupBoxBrigades.SuspendLayout();
            this.panelBrigadeDetails.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridBrigades)).BeginInit();
            this.panelRight.SuspendLayout();
            this.groupBoxPositions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridPositions)).BeginInit();
            this.panelPositionDetails.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.panelLeft);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.panelRight);
            this.splitContainer1.Size = new System.Drawing.Size(1004, 554);
            this.splitContainer1.SplitterDistance = 505;
            this.splitContainer1.TabIndex = 0;
            // 
            // panelLeft
            // 
            this.panelLeft.Controls.Add(this.groupBoxBrigades);
            this.panelLeft.Controls.Add(this.groupBoxDepartments);
            this.panelLeft.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelLeft.Location = new System.Drawing.Point(0, 0);
            this.panelLeft.Name = "panelLeft";
            this.panelLeft.Size = new System.Drawing.Size(505, 554);
            this.panelLeft.TabIndex = 0;
            // 
            // groupBoxDepartments
            // 
            this.groupBoxDepartments.Controls.Add(this.panelDeptDetails);
            this.groupBoxDepartments.Controls.Add(this.gridDepartments);
            this.groupBoxDepartments.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBoxDepartments.Location = new System.Drawing.Point(0, 0);
            this.groupBoxDepartments.Name = "groupBoxDepartments";
            this.groupBoxDepartments.Size = new System.Drawing.Size(505, 380);
            this.groupBoxDepartments.TabIndex = 0;
            this.groupBoxDepartments.TabStop = false;
            this.groupBoxDepartments.Text = "Подразделения // бригады";
            // 
            // gridDepartments
            // 
            this.gridDepartments.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridDepartments.Dock = System.Windows.Forms.DockStyle.Top;
            this.gridDepartments.Location = new System.Drawing.Point(3, 16);
            this.gridDepartments.Name = "gridDepartments";
            this.gridDepartments.Size = new System.Drawing.Size(499, 150);
            this.gridDepartments.TabIndex = 0;
            this.gridDepartments.SelectionChanged += new System.EventHandler(this.gridDepartments_SelectionChanged);
            // 
            // panelDeptDetails
            // 
            this.panelDeptDetails.Controls.Add(this.btnCancelDept);
            this.panelDeptDetails.Controls.Add(this.btnEditDept);
            this.panelDeptDetails.Controls.Add(this.btnAddDept);
            this.panelDeptDetails.Controls.Add(this.txtDeptDesc);
            this.panelDeptDetails.Controls.Add(this.labelDeptDesc);
            this.panelDeptDetails.Controls.Add(this.cmbBrigadeDivision);
            this.panelDeptDetails.Controls.Add(this.txtBrigadeCount);
            this.panelDeptDetails.Controls.Add(this.labelBrigadeCount);
            this.panelDeptDetails.Controls.Add(this.labelBrigadeDivision);
            this.panelDeptDetails.Controls.Add(this.txtDeptName);
            this.panelDeptDetails.Controls.Add(this.labelDeptName);
            this.panelDeptDetails.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelDeptDetails.Location = new System.Drawing.Point(3, 166);
            this.panelDeptDetails.Name = "panelDeptDetails";
            this.panelDeptDetails.Size = new System.Drawing.Size(499, 211);
            this.panelDeptDetails.TabIndex = 1;
            // 
            // btnCancelDept
            // 
            this.btnCancelDept.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancelDept.Enabled = false;
            this.btnCancelDept.Location = new System.Drawing.Point(207, 175);
            this.btnCancelDept.Name = "btnCancelDept";
            this.btnCancelDept.Size = new System.Drawing.Size(97, 33);
            this.btnCancelDept.TabIndex = 10;
            this.btnCancelDept.Text = "Отмена";
            this.btnCancelDept.UseVisualStyleBackColor = true;
            this.btnCancelDept.Click += new System.EventHandler(this.btnCancelDept_Click);
            // 
            // btnEditDept
            // 
            this.btnEditDept.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnEditDept.Location = new System.Drawing.Point(304, 175);
            this.btnEditDept.Name = "btnEditDept";
            this.btnEditDept.Size = new System.Drawing.Size(97, 33);
            this.btnEditDept.TabIndex = 9;
            this.btnEditDept.Text = "Изменить";
            this.btnEditDept.UseVisualStyleBackColor = true;
            this.btnEditDept.Click += new System.EventHandler(this.btnEditDept_Click);
            // 
            // btnAddDept
            // 
            this.btnAddDept.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddDept.Location = new System.Drawing.Point(401, 175);
            this.btnAddDept.Name = "btnAddDept";
            this.btnAddDept.Size = new System.Drawing.Size(97, 33);
            this.btnAddDept.TabIndex = 8;
            this.btnAddDept.Text = "Добавить";
            this.btnAddDept.UseVisualStyleBackColor = true;
            this.btnAddDept.Click += new System.EventHandler(this.btnAddDept_Click);
            // 
            // txtDeptDesc
            // 
            this.txtDeptDesc.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDeptDesc.Enabled = false;
            this.txtDeptDesc.Location = new System.Drawing.Point(1, 97);
            this.txtDeptDesc.Multiline = true;
            this.txtDeptDesc.Name = "txtDeptDesc";
            this.txtDeptDesc.Size = new System.Drawing.Size(497, 72);
            this.txtDeptDesc.TabIndex = 7;
            // 
            // labelDeptDesc
            // 
            this.labelDeptDesc.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelDeptDesc.AutoSize = true;
            this.labelDeptDesc.Location = new System.Drawing.Point(3, 81);
            this.labelDeptDesc.Name = "labelDeptDesc";
            this.labelDeptDesc.Size = new System.Drawing.Size(72, 13);
            this.labelDeptDesc.TabIndex = 6;
            this.labelDeptDesc.Text = "Описание:";
            // 
            // cmbBrigadeDivision
            // 
            this.cmbBrigadeDivision.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbBrigadeDivision.Enabled = false;
            this.cmbBrigadeDivision.FormattingEnabled = true;
            this.cmbBrigadeDivision.Items.AddRange(new object[] {
            "Нет",
            "Да"});
            this.cmbBrigadeDivision.Location = new System.Drawing.Point(216, 39);
            this.cmbBrigadeDivision.Name = "cmbBrigadeDivision";
            this.cmbBrigadeDivision.Size = new System.Drawing.Size(70, 21);
            this.cmbBrigadeDivision.TabIndex = 5;
            this.cmbBrigadeDivision.SelectedIndexChanged += new System.EventHandler(this.cmbBrigadeDivision_SelectedIndexChanged);
            // 
            // txtBrigadeCount
            // 
            this.txtBrigadeCount.Enabled = false;
            this.txtBrigadeCount.Location = new System.Drawing.Point(427, 39);
            this.txtBrigadeCount.Name = "txtBrigadeCount";
            this.txtBrigadeCount.Size = new System.Drawing.Size(70, 20);
            this.txtBrigadeCount.TabIndex = 4;
            this.txtBrigadeCount.Text = "0";
            // 
            // labelBrigadeCount
            // 
            this.labelBrigadeCount.AutoSize = true;
            this.labelBrigadeCount.Location = new System.Drawing.Point(313, 42);
            this.labelBrigadeCount.Name = "labelBrigadeCount";
            this.labelBrigadeCount.Size = new System.Drawing.Size(89, 13);
            this.labelBrigadeCount.TabIndex = 3;
            this.labelBrigadeCount.Text = "Количество:";
            // 
            // labelBrigadeDivision
            // 
            this.labelBrigadeDivision.AutoSize = true;
            this.labelBrigadeDivision.Location = new System.Drawing.Point(10, 42);
            this.labelBrigadeDivision.Name = "labelBrigadeDivision";
            this.labelBrigadeDivision.Size = new System.Drawing.Size(157, 13);
            this.labelBrigadeDivision.TabIndex = 2;
            this.labelBrigadeDivision.Text = "Разделение на бригады:";
            // 
            // txtDeptName
            // 
            this.txtDeptName.Enabled = false;
            this.txtDeptName.Location = new System.Drawing.Point(216, 6);
            this.txtDeptName.Name = "txtDeptName";
            this.txtDeptName.Size = new System.Drawing.Size(282, 20);
            this.txtDeptName.TabIndex = 1;
            // 
            // labelDeptName
            // 
            this.labelDeptName.AutoSize = true;
            this.labelDeptName.Location = new System.Drawing.Point(10, 9);
            this.labelDeptName.Name = "labelDeptName";
            this.labelDeptName.Size = new System.Drawing.Size(172, 13);
            this.labelDeptName.TabIndex = 0;
            this.labelDeptName.Text = "Название подразделения:";
            // 
            // groupBoxBrigades
            // 
            this.groupBoxBrigades.Controls.Add(this.panelBrigadeDetails);
            this.groupBoxBrigades.Controls.Add(this.gridBrigades);
            this.groupBoxBrigades.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxBrigades.Location = new System.Drawing.Point(0, 380);
            this.groupBoxBrigades.Name = "groupBoxBrigades";
            this.groupBoxBrigades.Size = new System.Drawing.Size(505, 174);
            this.groupBoxBrigades.TabIndex = 1;
            this.groupBoxBrigades.TabStop = false;
            this.groupBoxBrigades.Text = "Бригады";
            // 
            // panelBrigadeDetails
            // 
            this.panelBrigadeDetails.Controls.Add(this.btnSaveBrigade);
            this.panelBrigadeDetails.Controls.Add(this.btnEditBrigade);
            this.panelBrigadeDetails.Controls.Add(this.txtBrigadeName);
            this.panelBrigadeDetails.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelBrigadeDetails.Location = new System.Drawing.Point(3, 136);
            this.panelBrigadeDetails.Name = "panelBrigadeDetails";
            this.panelBrigadeDetails.Size = new System.Drawing.Size(499, 35);
            this.panelBrigadeDetails.TabIndex = 1;
            // 
            // btnSaveBrigade
            // 
            this.btnSaveBrigade.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSaveBrigade.Enabled = false;
            this.btnSaveBrigade.Location = new System.Drawing.Point(403, 1);
            this.btnSaveBrigade.Name = "btnSaveBrigade";
            this.btnSaveBrigade.Size = new System.Drawing.Size(97, 33);
            this.btnSaveBrigade.TabIndex = 2;
            this.btnSaveBrigade.Text = "Сохранить";
            this.btnSaveBrigade.UseVisualStyleBackColor = true;
            this.btnSaveBrigade.Click += new System.EventHandler(this.btnSaveBrigade_Click);
            // 
            // btnEditBrigade
            // 
            this.btnEditBrigade.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnEditBrigade.Enabled = false;
            this.btnEditBrigade.Location = new System.Drawing.Point(306, 1);
            this.btnEditBrigade.Name = "btnEditBrigade";
            this.btnEditBrigade.Size = new System.Drawing.Size(97, 33);
            this.btnEditBrigade.TabIndex = 1;
            this.btnEditBrigade.Text = "Изменить";
            this.btnEditBrigade.UseVisualStyleBackColor = true;
            this.btnEditBrigade.Click += new System.EventHandler(this.btnEditBrigade_Click);
            // 
            // txtBrigadeName
            // 
            this.txtBrigadeName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtBrigadeName.Enabled = false;
            this.txtBrigadeName.Location = new System.Drawing.Point(1, 3);
            this.txtBrigadeName.Name = "txtBrigadeName";
            this.txtBrigadeName.Size = new System.Drawing.Size(304, 20);
            this.txtBrigadeName.TabIndex = 0;
            // 
            // gridBrigades
            // 
            this.gridBrigades.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridBrigades.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridBrigades.Location = new System.Drawing.Point(3, 16);
            this.gridBrigades.Name = "gridBrigades";
            this.gridBrigades.Size = new System.Drawing.Size(499, 155);
            this.gridBrigades.TabIndex = 0;
            this.gridBrigades.SelectionChanged += new System.EventHandler(this.gridBrigades_SelectionChanged);
            // 
            // panelRight
            // 
            this.panelRight.Controls.Add(this.panelPositionDetails);
            this.panelRight.Controls.Add(this.groupBoxPositions);
            this.panelRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelRight.Location = new System.Drawing.Point(0, 0);
            this.panelRight.Name = "panelRight";
            this.panelRight.Size = new System.Drawing.Size(495, 554);
            this.panelRight.TabIndex = 0;
            // 
            // groupBoxPositions
            // 
            this.groupBoxPositions.Controls.Add(this.gridPositions);
            this.groupBoxPositions.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBoxPositions.Location = new System.Drawing.Point(0, 0);
            this.groupBoxPositions.Name = "groupBoxPositions";
            this.groupBoxPositions.Size = new System.Drawing.Size(495, 279);
            this.groupBoxPositions.TabIndex = 0;
            this.groupBoxPositions.TabStop = false;
            this.groupBoxPositions.Text = "Должности подразделения";
            // 
            // gridPositions
            // 
            this.gridPositions.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridPositions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridPositions.Location = new System.Drawing.Point(3, 16);
            this.gridPositions.Name = "gridPositions";
            this.gridPositions.Size = new System.Drawing.Size(489, 260);
            this.gridPositions.TabIndex = 0;
            this.gridPositions.SelectionChanged += new System.EventHandler(this.gridPositions_SelectionChanged);
            // 
            // panelPositionDetails
            // 
            this.panelPositionDetails.Controls.Add(this.btnCancelPosition);
            this.panelPositionDetails.Controls.Add(this.btnEditPosition);
            this.panelPositionDetails.Controls.Add(this.btnAddPosition);
            this.panelPositionDetails.Controls.Add(this.txtPositionDesc);
            this.panelPositionDetails.Controls.Add(this.labelPositionDesc);
            this.panelPositionDetails.Controls.Add(this.txtPositionCount);
            this.panelPositionDetails.Controls.Add(this.labelPositionCount);
            this.panelPositionDetails.Controls.Add(this.txtPositionName);
            this.panelPositionDetails.Controls.Add(this.labelPositionName);
            this.panelPositionDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelPositionDetails.Location = new System.Drawing.Point(0, 279);
            this.panelPositionDetails.Name = "panelPositionDetails";
            this.panelPositionDetails.Size = new System.Drawing.Size(495, 275);
            this.panelPositionDetails.TabIndex = 1;
            // 
            // btnCancelPosition
            // 
            this.btnCancelPosition.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancelPosition.Enabled = false;
            this.btnCancelPosition.Location = new System.Drawing.Point(55, 235);
            this.btnCancelPosition.Name = "btnCancelPosition";
            this.btnCancelPosition.Size = new System.Drawing.Size(97, 33);
            this.btnCancelPosition.TabIndex = 8;
            this.btnCancelPosition.Text = "Отмена";
            this.btnCancelPosition.UseVisualStyleBackColor = true;
            this.btnCancelPosition.Click += new System.EventHandler(this.btnCancelPosition_Click);
            // 
            // btnEditPosition
            // 
            this.btnEditPosition.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnEditPosition.Enabled = false;
            this.btnEditPosition.Location = new System.Drawing.Point(158, 235);
            this.btnEditPosition.Name = "btnEditPosition";
            this.btnEditPosition.Size = new System.Drawing.Size(97, 33);
            this.btnEditPosition.TabIndex = 7;
            this.btnEditPosition.Text = "Изменить";
            this.btnEditPosition.UseVisualStyleBackColor = true;
            this.btnEditPosition.Click += new System.EventHandler(this.btnEditPosition_Click);
            // 
            // btnAddPosition
            // 
            this.btnAddPosition.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddPosition.Location = new System.Drawing.Point(261, 235);
            this.btnAddPosition.Name = "btnAddPosition";
            this.btnAddPosition.Size = new System.Drawing.Size(97, 33);
            this.btnAddPosition.TabIndex = 6;
            this.btnAddPosition.Text = "Добавить";
            this.btnAddPosition.UseVisualStyleBackColor = true;
            this.btnAddPosition.Click += new System.EventHandler(this.btnAddPosition_Click);
            // 
            // txtPositionDesc
            // 
            this.txtPositionDesc.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPositionDesc.Enabled = false;
            this.txtPositionDesc.Location = new System.Drawing.Point(1, 104);
            this.txtPositionDesc.Multiline = true;
            this.txtPositionDesc.Name = "txtPositionDesc";
            this.txtPositionDesc.Size = new System.Drawing.Size(493, 125);
            this.txtPositionDesc.TabIndex = 5;
            // 
            // labelPositionDesc
            // 
            this.labelPositionDesc.AutoSize = true;
            this.labelPositionDesc.Location = new System.Drawing.Point(4, 88);
            this.labelPositionDesc.Name = "labelPositionDesc";
            this.labelPositionDesc.Size = new System.Drawing.Size(72, 13);
            this.labelPositionDesc.TabIndex = 4;
            this.labelPositionDesc.Text = "Описание:";
            // 
            // txtPositionCount
            // 
            this.txtPositionCount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPositionCount.Enabled = false;
            this.txtPositionCount.Location = new System.Drawing.Point(162, 38);
            this.txtPositionCount.Name = "txtPositionCount";
            this.txtPositionCount.Size = new System.Drawing.Size(73, 20);
            this.txtPositionCount.TabIndex = 3;
            this.txtPositionCount.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtPositionCount_KeyPress);
            // 
            // labelPositionCount
            // 
            this.labelPositionCount.AutoSize = true;
            this.labelPositionCount.Location = new System.Drawing.Point(4, 41);
            this.labelPositionCount.Name = "labelPositionCount";
            this.labelPositionCount.Size = new System.Drawing.Size(240, 13);
            this.labelPositionCount.TabIndex = 2;
            this.labelPositionCount.Text = "Разрешенное количество рабочих:";
            // 
            // txtPositionName
            // 
            this.txtPositionName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPositionName.Enabled = false;
            this.txtPositionName.Location = new System.Drawing.Point(0, 5);
            this.txtPositionName.Name = "txtPositionName";
            this.txtPositionName.Size = new System.Drawing.Size(235, 20);
            this.txtPositionName.TabIndex = 1;
            // 
            // labelPositionName
            // 
            this.labelPositionName.AutoSize = true;
            this.labelPositionName.Location = new System.Drawing.Point(5, 6);
            this.labelPositionName.Name = "labelPositionName";
            this.labelPositionName.Size = new System.Drawing.Size(139, 13);
            this.labelPositionName.TabIndex = 0;
            this.labelPositionName.Text = "Название должности";
            // 
            // DepartmentsPositions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Name = "DepartmentsPositions";
            this.Size = new System.Drawing.Size(1004, 554);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.panelLeft.ResumeLayout(false);
            this.groupBoxDepartments.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridDepartments)).EndInit();
            this.panelDeptDetails.ResumeLayout(false);
            this.panelDeptDetails.PerformLayout();
            this.groupBoxBrigades.ResumeLayout(false);
            this.panelBrigadeDetails.ResumeLayout(false);
            this.panelBrigadeDetails.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridBrigades)).EndInit();
            this.panelRight.ResumeLayout(false);
            this.groupBoxPositions.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridPositions)).EndInit();
            this.panelPositionDetails.ResumeLayout(false);
            this.panelPositionDetails.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Panel panelLeft;
        private System.Windows.Forms.GroupBox groupBoxDepartments;
        private System.Windows.Forms.DataGridView gridDepartments;
        private System.Windows.Forms.Panel panelDeptDetails;
        private System.Windows.Forms.TextBox txtDeptName;
        private System.Windows.Forms.Label labelDeptName;
        private System.Windows.Forms.Label labelBrigadeDivision;
        private System.Windows.Forms.TextBox txtBrigadeCount;
        private System.Windows.Forms.Label labelBrigadeCount;
        private System.Windows.Forms.ComboBox cmbBrigadeDivision;
        private System.Windows.Forms.TextBox txtDeptDesc;
        private System.Windows.Forms.Label labelDeptDesc;
        private System.Windows.Forms.Button btnCancelDept;
        private System.Windows.Forms.Button btnEditDept;
        private System.Windows.Forms.Button btnAddDept;
        private System.Windows.Forms.GroupBox groupBoxBrigades;
        private System.Windows.Forms.Panel panelBrigadeDetails;
        private System.Windows.Forms.Button btnSaveBrigade;
        private System.Windows.Forms.Button btnEditBrigade;
        private System.Windows.Forms.TextBox txtBrigadeName;
        private System.Windows.Forms.DataGridView gridBrigades;
        private System.Windows.Forms.Panel panelRight;
        private System.Windows.Forms.GroupBox groupBoxPositions;
        private System.Windows.Forms.DataGridView gridPositions;
        private System.Windows.Forms.Panel panelPositionDetails;
        private System.Windows.Forms.Button btnCancelPosition;
        private System.Windows.Forms.Button btnEditPosition;
        private System.Windows.Forms.Button btnAddPosition;
        private System.Windows.Forms.TextBox txtPositionDesc;
        private System.Windows.Forms.Label labelPositionDesc;
        private System.Windows.Forms.TextBox txtPositionCount;
        private System.Windows.Forms.Label labelPositionCount;
        private System.Windows.Forms.TextBox txtPositionName;
        private System.Windows.Forms.Label labelPositionName;
    }
}