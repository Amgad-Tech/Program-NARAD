using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Program_na_Ryadam
{
    public enum AppTheme
    {
        Dark,
        Light
    }

    public static class ThemeManager
    {
        public static AppTheme CurrentTheme { get; private set; } = AppTheme.Dark;
        public static event EventHandler ThemeChanged;

        public static void ToggleTheme()
        {
            CurrentTheme = CurrentTheme == AppTheme.Dark ? AppTheme.Light : AppTheme.Dark;
            ThemeChanged?.Invoke(null, EventArgs.Empty);
        }

        public static void ApplyTheme(Control root)
        {
            if (root == null) return;

            Color backColor, foreColor;
            Color controlBackColor, controlForeColor;

            if (CurrentTheme == AppTheme.Dark)
            {
                backColor = Color.FromArgb(45, 45, 60);
                foreColor = Color.WhiteSmoke;
                controlBackColor = Color.FromArgb(60, 60, 80);
                controlForeColor = Color.White;
            }
            else
            {
                backColor = Color.WhiteSmoke;
                foreColor = Color.FromArgb(30, 30, 30);
                controlBackColor = Color.White;
                controlForeColor = Color.Black;
            }

            ApplyColorsRecursively(root, backColor, foreColor, controlBackColor, controlForeColor);
        }

        private static void ApplyColorsRecursively(Control control, Color backColor, Color foreColor,
                                                 Color controlBackColor, Color controlForeColor)
        {
            // Apply to the control itself
            if (control is Form || control is Panel || control is GroupBox)
            {
                control.BackColor = backColor;
                control.ForeColor = foreColor;
            }
            else if (control is Label || control is LinkLabel || control is RadioButton || control is CheckBox)
            {
                control.ForeColor = foreColor;
                if (control is Label || control is LinkLabel)
                    control.BackColor = backColor;
            }
            else if (control is TextBox textBox)
            {
                textBox.BackColor = controlBackColor;
                textBox.ForeColor = controlForeColor;
                textBox.BorderStyle = BorderStyle.FixedSingle;
            }
            else if (control is ComboBox comboBox)
            {
                comboBox.BackColor = controlBackColor;
                comboBox.ForeColor = controlForeColor;
                comboBox.FlatStyle = FlatStyle.Flat;
            }
            else if (control is Button button)
            {
                button.BackColor = CurrentTheme == AppTheme.Dark ?
                    Color.FromArgb(80, 80, 110) : Color.LightSteelBlue;
                button.ForeColor = controlForeColor;
                button.FlatStyle = FlatStyle.Flat;
                button.FlatAppearance.BorderColor = CurrentTheme == AppTheme.Dark ?
                    Color.SlateGray : Color.Gray;
            }
            else if (control is DataGridView dataGridView)
            {
                dataGridView.BackgroundColor = backColor;
                dataGridView.GridColor = CurrentTheme == AppTheme.Dark ?
                    Color.Gray : Color.LightGray;
                dataGridView.DefaultCellStyle.BackColor = controlBackColor;
                dataGridView.DefaultCellStyle.ForeColor = controlForeColor;
                dataGridView.ColumnHeadersDefaultCellStyle.BackColor = CurrentTheme == AppTheme.Dark ?
                    Color.FromArgb(70, 70, 90) : Color.LightGray;
                dataGridView.ColumnHeadersDefaultCellStyle.ForeColor = controlForeColor;
                dataGridView.RowHeadersDefaultCellStyle.BackColor = dataGridView.ColumnHeadersDefaultCellStyle.BackColor;
                dataGridView.RowHeadersDefaultCellStyle.ForeColor = controlForeColor;
            }
            else if (control is ListView listView)
            {
                listView.BackColor = controlBackColor;
                listView.ForeColor = controlForeColor;
            }
            else if (control is TreeView treeView)
            {
                treeView.BackColor = controlBackColor;
                treeView.ForeColor = controlForeColor;
            }

            // Apply to child controls
            foreach (Control child in control.Controls)
            {
                ApplyColorsRecursively(child, backColor, foreColor, controlBackColor, controlForeColor);
            }
        }
    }
}