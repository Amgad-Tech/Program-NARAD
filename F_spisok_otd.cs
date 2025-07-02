using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Windows.Forms;

namespace Program_na_Ryadam
{
    public partial class F_spisok_otd : Form
    {
        private int[] _departmentIDs = new int[50];

        public string SelectedDepartmentIDs { get; private set; } = "";
        public string SelectedDepartmentNames { get; private set; } = "";
        private readonly string _dbPath;

        public F_spisok_otd(string dbPath, string preSelectedIDs = "")
        {
            InitializeComponent();
            _dbPath = dbPath;
            LoadDepartments(preSelectedIDs);
        }

        private void LoadDepartments(string preSelectedIDs)
        {
            listViewDepartments.Items.Clear();
            var selectedIDs = preSelectedIDs.Split(';').ToList();

            using (var conn = new OleDbConnection($"Provider=Microsoft.Jet.OLEDB.4.0;Data Source={_dbPath}"))
            {
                conn.Open();
                var cmd = new OleDbCommand("SELECT ID, Name_otd FROM Name_otd ORDER BY Name_otd", conn);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var id = reader["ID"].ToString();
                        var item = new ListViewItem(reader["Name_otd"].ToString()) { Tag = id };
                        item.Checked = selectedIDs.Contains(id);
                        listViewDepartments.Items.Add(item);
                    }
                }
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            var selectedIDs = new List<string>();
            var selectedNames = new List<string>();

            foreach (ListViewItem item in listViewDepartments.Items)
            {
                if (item.Checked)
                {
                    selectedIDs.Add(item.Tag.ToString());
                    selectedNames.Add(item.Text);
                }
            }

            SelectedDepartmentIDs = string.Join(";", selectedIDs);
            SelectedDepartmentNames = string.Join("; ", selectedNames);
            DialogResult = DialogResult.OK;
        }
    }
}