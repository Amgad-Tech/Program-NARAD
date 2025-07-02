// F_spisok_otd.Designer.cs
namespace Program_na_Ryadam
{
    partial class F_spisok_otd
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

        private void InitializeComponent()
        {
            this.listViewDepartments = new System.Windows.Forms.ListView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnOK = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // listViewDepartments
            // 
            this.listViewDepartments.CheckBoxes = true;
            this.listViewDepartments.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listViewDepartments.Font = new System.Drawing.Font("Times New Roman", 19F);
            this.listViewDepartments.HideSelection = false;
            this.listViewDepartments.Location = new System.Drawing.Point(0, 0);
            this.listViewDepartments.Name = "listViewDepartments";
            this.listViewDepartments.Size = new System.Drawing.Size(440, 319);
            this.listViewDepartments.TabIndex = 1;
            this.listViewDepartments.UseCompatibleStateImageBehavior = false;
            this.listViewDepartments.View = System.Windows.Forms.View.List;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnOK);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 319);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(440, 32);
            this.panel1.TabIndex = 0;
            // 
            // btnOK
            // 
            this.btnOK.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnOK.Font = new System.Drawing.Font("Times New Roman", 19F);
            this.btnOK.Location = new System.Drawing.Point(365, 0);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 32);
            this.btnOK.TabIndex = 0;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // F_spisok_otd
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(440, 351);
            this.Controls.Add(this.listViewDepartments);
            this.Controls.Add(this.panel1);
            this.Name = "F_spisok_otd";
            this.Text = "Выбор отделений";
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.ListView listViewDepartments;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnOK;
    }
}