using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using Program_na_Ryadam.Models;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using static System.Net.WebRequestMethods;

namespace Program_na_Ryadam
{
    public partial class f_list_view : Form
    {
        // Reference to main form
        private WorkRecord _workRecord;

        public class SelectedWork
        {
            public string CultureName { get; set; }
            public string WorkType { get; set; }
            public WorkTypeInfo WorkInfo { get; set; }
            public string Date { get; set; }
            public List<int> DepartmentIDs { get; set; }
            public List<string> DepartmentNames { get; set; }
        }

        private string _connStr;
        public List<SelectedWork> SelectedWorks { get; private set; } = new List<SelectedWork>();

        public f_list_view(string dbPath)
        {
            InitializeComponent();
            this.Load += f_list_view_Load;
            Button1.Click += btnAdd_Click;
            lv2.SelectedIndexChanged += lv2_SelectedIndexChanged;
            lv1.ItemSelectionChanged += lv1_ItemSelectionChanged;
            _connStr = $"Provider=Microsoft.Jet.OLEDB.4.0;Data Source={dbPath}";
            dtp.Value = DateTime.Today;
            AdjustColumnWidths();
        }

        private void AdjustColumnWidths()
        {
            lv2.Columns[0].Width = lv2.ClientSize.Width - 4;
            lv1.Columns[0].Width = lv1.ClientSize.Width - 4;
            lv3.Columns[0].Width = lv3.ClientSize.Width - 4;
        }

        // Load cultures (lv2) and departments (lv3)
        private void LoadCulturesAndDepartments()
        {
            try
            {
                using (OleDbConnection conn = new OleDbConnection(_connStr))
                {
                    conn.Open();

                    // FIXED: Corrected field names
                    using (OleDbCommand cmd = new OleDbCommand("SELECT ID, Name_kultur FROM Kultura ORDER BY Name_kultur", conn))
                    using (OleDbDataReader reader = cmd.ExecuteReader())
                    {
                        lv2.Items.Clear();
                        while (reader.Read())
                        {
                            var item = new ListViewItem(reader["Name_kultur"].ToString());
                            item.Tag = Convert.ToInt32(reader["ID"]);
                            lv2.Items.Add(item);
                        }
                    }

                    // FIXED: Corrected field names
                    using (OleDbCommand cmd = new OleDbCommand("SELECT ID, Name_otd FROM Name_otd ORDER BY Name_otd", conn))
                    using (OleDbDataReader reader = cmd.ExecuteReader())
                    {
                        lv3.Items.Clear();
                        while (reader.Read())
                        {
                            var item = new ListViewItem(reader["Name_otd"].ToString());
                            item.Tag = Convert.ToInt32(reader["ID"]);
                            lv3.Items.Add(item);
                        }
                    }
                }

                if (lv2.Items.Count > 0)
                {
                    lv2.Items[0].Selected = true;
                    LoadWorkTypesForCulture((int)lv2.Items[0].Tag);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Database error: {ex.Message}");
            }
        }

        private void LoadWorkTypesForCulture(int cultureID)
        {
            lv1.Items.Clear();

            try
            {
                // FIXED: Corrected field names and added brackets
                string sql = "SELECT ID, Name_W, IsHourly, [bool_otd] " +
                             "FROM Vid_RABOT " +
                             $"WHERE Kultura = {cultureID} " +
                             "ORDER BY Name_W";

                using (OleDbConnection conn = new OleDbConnection(_connStr))
                using (OleDbCommand cmd = new OleDbCommand(sql, conn))
                {
                    conn.Open();
                    using (OleDbDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // FIXED: Handle DBNull values
                            var item = new ListViewItem(reader["Name_W"].ToString());
                            item.Tag = new WorkTypeInfo
                            {
                                ID = Convert.ToInt32(reader["ID"]),
                                IsHourly = reader["IsHourly"] != DBNull.Value && Convert.ToBoolean(reader["IsHourly"]),
                                RequiresDepartment = reader["bool_otd"] != DBNull.Value && Convert.ToBoolean(reader["bool_otd"])
                            };
                            lv1.Items.Add(item);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading work types: {ex.Message}\n\nSQL");
            }
        }

        // Form Load: Initialize data
        private void f_list_view_Load(object sender, EventArgs e)
        {
            LoadCulturesAndDepartments();
        }

        // Culture selection changed: Load work types
        private void lv2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lv2.SelectedItems.Count > 0)
            {
                int cultureID = (int)lv2.SelectedItems[0].Tag;
                LoadWorkTypesForCulture(cultureID);
            }
        }

        // Work type clicked: Uncheck all departments
        private void lv1_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            foreach (ListViewItem item in lv3.Items)
            {
                item.Checked = false;
            }
        }

        // Add selected items to main form's grid
        private void btnAdd_Click(object sender, EventArgs e)
        {
            

            if (lv2.SelectedItems.Count == 0)
            {
                MessageBox.Show("Please select a culture first");
                return;
            }

            SelectedWorks.Clear();
            var selectedCulture = lv2.SelectedItems[0];

            foreach (ListViewItem workItem in lv1.CheckedItems)
            {
                var workInfo = (WorkTypeInfo)workItem.Tag;
                var departments = lv3.CheckedItems
                    .Cast<ListViewItem>()
                    .Select(i => new {
                        ID = (int)i.Tag,
                        Name = i.Text
                    })
                    .ToList();

                if (workInfo.RequiresDepartment && departments.Count == 0)
                {
                    MessageBox.Show($"Work '{workItem.Text}' requires department selection");
                    continue;
                }

                SelectedWorks.Add(new SelectedWork
                {
                    CultureName = selectedCulture.Text,
                    WorkType = workItem.Text,
                    WorkInfo = workInfo,
                    Date = dtp.Value.ToString("dd.MM.yyyy"),
                    DepartmentIDs = departments.Select(d => d.ID).ToList(),
                    DepartmentNames = departments.Select(d => d.Name).ToList()
                });
            }


            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}