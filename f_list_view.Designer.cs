namespace Program_na_Ryadam
{
    partial class f_list_view
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

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.Panel3= new System.Windows.Forms.Panel();
            this.Panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.Panel3.Location = new System.Drawing.Point(0, 0);
            this.Panel3.Name = "Panel padding";
            this.Panel3.Size = new System.Drawing.Size(1137, 50);
            this.Panel3.TabIndex = 0;

            this.Panel1 = new System.Windows.Forms.Panel();
            this.Button1 = new System.Windows.Forms.Button();
            this.dtp = new System.Windows.Forms.DateTimePicker();
            this.Panel2 = new System.Windows.Forms.Panel();
            this.lv3 = new System.Windows.Forms.ListView();
            this.colDepartment = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Splitter2 = new System.Windows.Forms.Splitter();
            this.lv1 = new System.Windows.Forms.ListView();
            this.colWorkName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Splitter1 = new System.Windows.Forms.Splitter();
            this.lv2 = new System.Windows.Forms.ListView();
            this.colCulture = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Panel1.SuspendLayout();
            this.Panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // Panel1
            // 
            this.Panel1.Controls.Add(this.Button1);
            this.Panel1.Controls.Add(this.dtp);
            this.Panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.Panel1.Location = new System.Drawing.Point(0, 603);
            this.Panel1.Name = "Panel1";
            this.Panel1.Size = new System.Drawing.Size(1137, 50);
            this.Panel1.TabIndex = 0;
            // 
            // Button1
            // 
            this.Button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Button1.Font = new System.Drawing.Font("Times New Roman", 17F);
            this.Button1.Location = new System.Drawing.Point(1014, 9);
            this.Button1.Name = "Button1";
            this.Button1.Size = new System.Drawing.Size(111, 33);
            this.Button1.TabIndex = 0;
            this.Button1.Text = "Добавить";
            this.Button1.UseVisualStyleBackColor = true;
            // 
            // dtp
            // 
            this.dtp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.dtp.CustomFormat = "dd.MM.yyyy";
            this.dtp.Font = new System.Drawing.Font("Times New Roman", 17F);
            this.dtp.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtp.Location = new System.Drawing.Point(872, 9);
            this.dtp.Name = "dtp";
            this.dtp.Size = new System.Drawing.Size(121, 34);
            this.dtp.TabIndex = 1;
            // 
            // Panel2
            // 
            this.Panel2.Controls.Add(this.lv3);
            this.Panel2.Controls.Add(this.Splitter2);
            this.Panel2.Controls.Add(this.lv1);
            this.Panel2.Controls.Add(this.Splitter1);
            this.Panel2.Controls.Add(this.lv2);
            this.Panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Panel2.Location = new System.Drawing.Point(0, 0);
            this.Panel2.Name = "Panel2";
            this.Panel2.Size = new System.Drawing.Size(1137, 603);
            this.Panel2.TabIndex = 1;
            // 
            // lv3
            // 
            this.lv3.CheckBoxes = true;
            this.lv3.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colDepartment});
            this.lv3.Dock = System.Windows.Forms.DockStyle.Right;
            this.lv3.Font = new System.Drawing.Font("Times New Roman", 17F);
            this.lv3.FullRowSelect = true;
            this.lv3.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.lv3.HideSelection = false;
            this.lv3.Location = new System.Drawing.Point(795, 0);
            this.lv3.Name = "lv3";
            this.lv3.Size = new System.Drawing.Size(342, 623);
            this.lv3.TabIndex = 2;
            this.lv3.UseCompatibleStateImageBehavior = false;
            this.lv3.View = System.Windows.Forms.View.Details;
            // 
            // colDepartment
            // 
            this.colDepartment.Text = "Отделение";
            this.colDepartment.Width = 150;
            // 
            // Splitter2
            // 
            this.Splitter2.Location = new System.Drawing.Point(265, 0);
            this.Splitter2.Name = "Splitter2";
            this.Splitter2.Size = new System.Drawing.Size(10, 603);
            this.Splitter2.TabIndex = 1;
            this.Splitter2.TabStop = false;
            // 
            // lv1
            // 
            this.lv1.CheckBoxes = true;
            this.lv1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colWorkName});
            this.lv1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lv1.Font = new System.Drawing.Font("Times New Roman", 17F);
            this.lv1.FullRowSelect = true;
            this.lv1.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.lv1.HideSelection = false;
            this.lv1.Location = new System.Drawing.Point(265, 0);
            this.lv1.Name = "lv1";
            this.lv1.Size = new System.Drawing.Size(872, 603);
            this.lv1.TabIndex = 0;
            this.lv1.UseCompatibleStateImageBehavior = false;
            this.lv1.View = System.Windows.Forms.View.Details;
            // 
            // colWorkName
            // 
            this.colWorkName.Text = "Название работы";
            this.colWorkName.Width = 250;
            // 
            // Splitter1
            // 
            this.Splitter1.Location = new System.Drawing.Point(255, 0);
            this.Splitter1.Name = "Splitter1";
            this.Splitter1.Size = new System.Drawing.Size(10, 603);
            this.Splitter1.TabIndex = 0;
            this.Splitter1.TabStop = false;
            // 
            // lv2
            // 
            this.lv2.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colCulture});
            this.lv2.Dock = System.Windows.Forms.DockStyle.Left;
            this.lv2.Font = new System.Drawing.Font("Times New Roman", 17F);
            this.lv2.FullRowSelect = true;
            this.lv2.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.lv2.HideSelection = false;
            this.lv2.Location = new System.Drawing.Point(0, 0);
            this.lv2.Name = "lv2";
            this.lv2.Size = new System.Drawing.Size(255, 603);
            this.lv2.TabIndex = 1;
            this.lv2.UseCompatibleStateImageBehavior = false;
            this.lv2.View = System.Windows.Forms.View.Details;
            // 
            // colCulture
            // 
            this.colCulture.Text = "Культура";
            this.colCulture.Width = 200;
            // 
            // f_list_view
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1137, 653);
            this.Controls.Add(this.Panel2);
            this.Controls.Add(this.Panel1);
            this.Name = "f_list_view";
            this.Text = "Выбор работ";
            this.Panel1.ResumeLayout(false);
            this.Panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel Panel1;
        private System.Windows.Forms.Panel Panel3;
        private System.Windows.Forms.Button Button1;
        private System.Windows.Forms.DateTimePicker dtp;
        private System.Windows.Forms.Panel Panel2;
        private System.Windows.Forms.Splitter Splitter1;
        private System.Windows.Forms.Splitter Splitter2;
        private System.Windows.Forms.ListView lv1;
        private System.Windows.Forms.ColumnHeader colWorkName;
        private System.Windows.Forms.ListView lv2;
        private System.Windows.Forms.ColumnHeader colCulture;
        private System.Windows.Forms.ListView lv3;
        private System.Windows.Forms.ColumnHeader colDepartment;
    }
}