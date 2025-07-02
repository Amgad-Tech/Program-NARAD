using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Program_na_Ryadam
{
    //  Дорогой следующий программист,
    //
    //
    //     Когда я Этот код писал, только я и Бог знали, как он работает.
    //
    //     Теперь пусть Бог будет с тобой — потому что теперь только Он один знает, что тут вообще происходит.
    //
    //                                                  С уважением, программист ЮссеФ ^.^    

    //
    public partial class Form3 : Form
    {
        public string dbPath = Path.Combine(Application.StartupPath, "Database", "Database_XN1.mdb");
        private bool sidebarExpanded = true;
        private const int ExpandedWidth = 250;
        private const int CollapsedWidth = 40;
        private Panel panelContent;
        private Timer sidebarTimer;
        private int sidebarStep = 20;

        private Dictionary<string, UserControl> userControls = new Dictionary<string, UserControl>();

        private Panel headerPanel;
        private Label headerLabel;

        private Panel panelSidebar;
        private Button btnToggle;

        public Form3()
        {
            InitializeComponent();
            InitializeComponents();
            CenterToScreen();
            this.AutoScaleMode = AutoScaleMode.Dpi; // أو Font
            this.AutoSize = true;
        }

        private void InitializeComponents()
        {
            this.Text = "ДЦК-ППН";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.MinimumSize = new Size(800, 600);
            this.Font = new Font("Times New Roman", 10);
            ThemeManager.ApplyTheme(this); // Apply current theme

            // محتوى الواجهة
            panelContent = new Panel();
            panelContent.Dock = DockStyle.Fill;
            this.Controls.Add(panelContent);

            // التري فيو
            treeView1.Dock = DockStyle.Fill;
            treeView1.BorderStyle = BorderStyle.None;
            treeView1.Font = new Font("Times New Roman", 11, FontStyle.Bold);
            treeView1.Indent = 20;
            treeView1.AfterSelect += TreeView1_AfterSelect;

            // الزر الجانبي
            btnToggle = new Button();
            btnToggle.Text = "◀️";
            btnToggle.Font = new Font("Times New Roman", 12, FontStyle.Bold);
            btnToggle.Dock = DockStyle.Top;
            btnToggle.Height = 40;
            btnToggle.FlatStyle = FlatStyle.Flat;
            btnToggle.FlatAppearance.BorderSize = 0;
            btnToggle.Click += BtnToggle_Click;

            // بانل السايدبار
            panelSidebar = new Panel();
            panelSidebar.Dock = DockStyle.Left;
            panelSidebar.Width = ExpandedWidth;
            panelSidebar.Controls.Add(treeView1);
            panelSidebar.Controls.Add(btnToggle);
            this.Controls.Add(panelSidebar);

            panelContent.BringToFront();

            sidebarTimer = new Timer();
            sidebarTimer.Interval = 15;
            sidebarTimer.Tick += SidebarTimer_Tick;

            PopulateTreeView();
            InitializeUserControls();

            ApplyThemeToCustomElements(); // set colors
        }

        private void ApplyThemeToUserControl(Control control)
        {
            ThemeManager.ApplyTheme(control);
            foreach (Control child in control.Controls)
            {
                ApplyThemeToUserControl(child);

                // Handle special container controls
                if (child is Panel || child is GroupBox || child is TabPage)
                {
                    foreach (Control containerChild in child.Controls)
                    {
                        ApplyThemeToUserControl(containerChild);
                    }
                }
            }
        }

        private void ApplyThemeToCustomElements()
        {
            // لون الخلفيات حسب الثيم
            Color sidebarBack = ThemeManager.CurrentTheme == AppTheme.Dark ? Color.FromArgb(51, 51, 76) : Color.Gainsboro;
            Color treeFore = ThemeManager.CurrentTheme == AppTheme.Dark ? Color.White : Color.Black;
            Color treeBack = ThemeManager.CurrentTheme == AppTheme.Dark ? Color.FromArgb(51, 51, 76) : Color.WhiteSmoke;

            panelSidebar.BackColor = sidebarBack;
            treeView1.BackColor = treeBack;
            treeView1.ForeColor = treeFore;

            btnToggle.BackColor = ThemeManager.CurrentTheme == AppTheme.Dark ? Color.FromArgb(39, 39, 58) : Color.LightGray;
            btnToggle.ForeColor = ThemeManager.CurrentTheme == AppTheme.Dark ? Color.White : Color.Black;
        }

        private void InitializeUserControls()
        {
            userControls.Add("ReportView", new ReportView());
            userControls.Add("WorkTypes", new WorkTypes());
            userControls.Add("PlantCulture", new PlantCulture());
            userControls.Add("TankCleaning", new TankCleaning());
            userControls.Add("QualityFields", new QualityFields());
            userControls.Add("recordwork", new recordwork());
            userControls.Add("WorkSettings", new WorkSettings());
            userControls.Add("WorkersDatabase", new WorkersDatabase());
            userControls.Add("DepartmentsPositions", new DepartmentsPositions());
            userControls.Add("DetailedView", new DetailedView());
            userControls.Add("WorkView", new WorkView());
            userControls.Add("SortingView", new SortingView());
            userControls.Add("ForemanWork", new ForemanWork());
            userControls.Add("WorkRecord", new WorkRecord(dbPath));
            userControls.Add("GreenhouseWorkers", new GreenhouseWorkers());
            userControls.Add("SortingWorkers", new SortingWorkers());
            userControls.Add("Message", new Messages());
            userControls.Add("Cutting", new Cutting());
            userControls.Add("SortingReader", new SortingReader());
            userControls.Add("CuttingReader", new CuttingReader());
            userControls.Add("QualityReport", new QualityReport());
            userControls.Add("Fund", new Fund());

            foreach (var uc in userControls.Values)
            {
                ApplyThemeToUserControl(uc);
            }
        }

        private void PopulateTreeView()
        {
            treeView1.Nodes.Clear();

            treeView1.Nodes.Add("Вид отчёта");
            treeView1.Nodes.Add("Виды работ ОБЩАЯ");
            treeView1.Nodes.Add("Культура растений");

            var node = new TreeNode("Мойка баков и час. Работа");
            treeView1.Nodes.Add(node);

            var settingsNode = new TreeNode("Настройки");
            settingsNode.Nodes.Add("Настройка полей в табеле качества");
            settingsNode.Nodes.Add("Настройки работ на запись");
            settingsNode.Nodes.Add("Настройки работы");
            treeView1.Nodes.Add(settingsNode);

            treeView1.Nodes.Add("ОБЩАЯ база данных работников");
            treeView1.Nodes.Add("Подразделения и должности");

            var viewNode = new TreeNode("Просмотр");
            viewNode.Nodes.Add("Подробный просмотр");
            viewNode.Nodes.Add("Просмотр работы");
            viewNode.Nodes.Add("Просмотр сортировки");
            treeView1.Nodes.Add(viewNode);

            treeView1.Nodes.Add("Работа бригады");

            var workRecordNode = new TreeNode("Работа запись");
            workRecordNode.Nodes.Add("Работа запись ОБЩАЯ");
            treeView1.Nodes.Add(workRecordNode);

            treeView1.Nodes.Add("Работники оранжерейного комплекса");
            treeView1.Nodes.Add("Работники сортировки");
            treeView1.Nodes.Add("Сообщения");
            treeView1.Nodes.Add("Срезка");

            var readerNode = new TreeNode("Считыватель");
            readerNode.Nodes.Add("Считыватель сортировка");
            readerNode.Nodes.Add("Считыватель срезка");
            treeView1.Nodes.Add(readerNode);

            treeView1.Nodes.Add("Табель качества работы");
            treeView1.Nodes.Add("ФОНД");
            treeView1.Nodes.Add("EXIT");

            foreach (TreeNode n in treeView1.Nodes)
                n.Tag = n.Text;

            treeView1.ExpandAll();
        }

        private void BtnToggle_Click(object sender, EventArgs e)
        {
            sidebarExpanded = !sidebarExpanded;
            sidebarTimer.Start();
        }

        private void SidebarTimer_Tick(object sender, EventArgs e)
        {
            var panel = treeView1.Parent;

            if (sidebarExpanded)
            {
                if (panel.Width < ExpandedWidth)
                {
                    panel.Width += sidebarStep;
                }
                else
                {
                    sidebarTimer.Stop();
                    treeView1.Visible = true;
                }
            }
            else
            {
                if (panel.Width > CollapsedWidth)
                {
                    panel.Width -= sidebarStep;
                }
                else
                {
                    sidebarTimer.Stop();
                    treeView1.Visible = false;
                }
            }
        }

        private void TreeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            foreach (TreeNode node in treeView1.Nodes)
                ResetNodeColors(node);

            e.Node.BackColor = ThemeManager.CurrentTheme == AppTheme.Dark ? Color.FromArgb(70, 70, 100) : Color.LightBlue;
            e.Node.ForeColor = ThemeManager.CurrentTheme == AppTheme.Dark ? Color.White : Color.Black;

            if (e.Node.Text == "EXIT")
            {
                new Form1().Show();
                this.Hide();
                return;
            }

            ShowContentForNode(e.Node);
        }

        private void ResetNodeColors(TreeNode node)
        {
            node.BackColor = treeView1.BackColor;
            node.ForeColor = treeView1.ForeColor;

            foreach (TreeNode child in node.Nodes)
                ResetNodeColors(child);
        }

        private void ShowContentForNode(TreeNode node)
        {
            // Clear existing controls properly
            panelContent.Controls.Clear();

            // Create header panel if it doesn't exist
            if (headerPanel == null)
            {
                headerPanel = new Panel
                {
                    Dock = DockStyle.Top,
                    Height = 40,
                    Padding = new Padding(0, 5, 0, 5)
                };

                headerLabel = new Label
                {
                    Dock = DockStyle.Fill,
                    Font = new Font("Times New Roman", 14, FontStyle.Bold),
                    TextAlign = ContentAlignment.MiddleLeft,
                    Padding = new Padding(10, 0, 0, 0)
                };
                headerPanel.Controls.Add(headerLabel);
            }

            // Apply theme to header
            headerPanel.BackColor = ThemeManager.CurrentTheme == AppTheme.Dark
                ? Color.FromArgb(70, 70, 100)
                : Color.LightGray;
            headerLabel.ForeColor = ThemeManager.CurrentTheme == AppTheme.Dark
                ? Color.White
                : Color.Black;
            headerLabel.Text = $"📄 {node.Text}";

            // Add header to content panel FIRST
            panelContent.Controls.Add(headerPanel);

            // Create content container panel
            Panel contentContainer = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(0, 50, 0, 0) // Top padding to separate from header
            };
            panelContent.Controls.Add(contentContainer);

            string controlKey = GetControlKeyByText(node.Text);
            if (!string.IsNullOrEmpty(controlKey) && userControls.ContainsKey(controlKey))
            {
                var control = userControls[controlKey];

                // Remove control from any previous parent first
                if (control.Parent != null)
                {
                    control.Parent.Controls.Remove(control);
                }

                control.Dock = DockStyle.Fill;
                contentContainer.Controls.Add(control);
                control.BringToFront();
                ApplyThemeToUserControl(control);
            }
            else
            {
                // Fallback content
                Label fallback = new Label
                {
                    Text = $"❌ No content available for: {node.Text}",
                    Dock = DockStyle.Fill,
                    TextAlign = ContentAlignment.MiddleCenter,
                    Font = new Font("Times New Roman", 14, FontStyle.Bold)
                };
                contentContainer.Controls.Add(fallback);
            }

            // Ensure header stays on top
            headerPanel.BringToFront();
        }

        private string GetControlKeyByText(string russianText)
        {
            if (russianText == "Вид отчёта") return "ReportView";
            if (russianText == "Виды работ ОБЩАЯ") return "WorkTypes";
            if (russianText == "Культура растений") return "PlantCulture";
            if (russianText == "Мойка баков и час. Работа") return "TankCleaning";
            if (russianText == "Настройка полей в табеле качества") return "QualityFields";
            if (russianText == "Настройки работ на запись") return "recordwork";
            if (russianText == "Настройки работы") return "WorkSettings";
            if (russianText == "ОБЩАЯ база данных работников") return "WorkersDatabase";
            if (russianText == "Подразделения и должности") return "DepartmentsPositions";
            if (russianText == "Подробный просмотр") return "DetailedView";
            if (russianText == "Просмотр работы") return "WorkView";
            if (russianText == "Просмотр сортировки") return "SortingView";
            if (russianText == "Работа бригады") return "ForemanWork";
            if (russianText == "Работа запись ОБЩАЯ") return "WorkRecord";
            if (russianText == "Работники оранжерейного комплекса") return "GreenhouseWorkers";
            if (russianText == "Работники сортировки") return "SortingWorkers";
            if (russianText == "Сообщения") return "Message";
            if (russianText == "Срезка") return "Cutting";
            if (russianText == "Считыватель сортировка") return "SortingReader";
            if (russianText == "Считыватель срезка") return "CuttingReader";
            if (russianText == "Табель качества работы") return "QualityReport";
            if (russianText == "ФОНД") return "Fund";

            return "";
        }
        protected override void OnControlAdded(ControlEventArgs e)
        {
            base.OnControlAdded(e);
            Console.WriteLine($"Control added: {e.Control.Name} ({e.Control.GetType().Name})");
        }

        private void Form3_Load(object sender, EventArgs e)
        {

        }
    }
}
