using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace Program_na_Ryadam
{
    internal class Excel_Library : IDisposable
    {
        private Excel.Application excelApp;
        private Excel.Workbook workbook;
        private Excel.Worksheet worksheet;
        private Excel.Application _excelApp;
        private Excel.Workbook _workbook;
        private Excel.Worksheet _worksheet;
        private bool _isStarted;
        private readonly string[] _monthNames = {
            "январь", "февраль", "март", "апрель", "май", "июнь",
            "июль", "август", "сентябрь", "октябрь", "ноябрь", "декабрь"
        };

        public Excel_Library()
        {
            excelApp = new Excel.Application();
            workbook = excelApp.Workbooks.Add();
            worksheet = (Excel.Worksheet)workbook.Sheets[1];
        }
        public void NewBook(int sheetsCount = 1)
        {
            try
            {
                _excelApp = new Excel.Application();
                _excelApp.SheetsInNewWorkbook = sheetsCount;
                _workbook = _excelApp.Workbooks.Add();
                _worksheet = (Excel.Worksheet)_workbook.Sheets[1];
                _isStarted = true;
            }
            catch (Exception ex)
            {
                throw new Exception("Error creating Excel workbook: " + ex.Message);
            }
        }

        public void SelectSheet(int sheetNumber)
        {
            if (!_isStarted) return;
            _worksheet = (Excel.Worksheet)_workbook.Sheets[sheetNumber]; // Fixed cast
        }

        public void SetSheetName(string name)
        {
            if (!_isStarted) return;
            _worksheet.Name = name;
        }

        public void WriteCell(int row, int column, string value, bool bold = false, int fontSize = 12)
        {
            if (!_isStarted) return;
            Excel.Range cell = _worksheet.Cells[row, column];
            cell.Value = value;
            cell.Font.Bold = bold;
            cell.Font.Size = fontSize;
            Marshal.ReleaseComObject(cell);
        }

        public void WriteCell(int row, int column, float value, bool bold = false, int fontSize = 12)
        {
            WriteCell(row, column, value.ToString(), bold, fontSize);
        }

        public void MergeCells(string range)
        {
            if (!_isStarted) return;
            Excel.Range mergeRange = _worksheet.Range[range];
            mergeRange.Merge();
            Marshal.ReleaseComObject(mergeRange);
        }

        public void SetRowHeight(int row, double height)
        {
            if (!_isStarted) return;
            ((Excel.Range)_worksheet.Rows[row]).RowHeight = height;
        }

        public void SetColumnWidth(int column, double width)
        {
            if (!_isStarted) return;
            ((Excel.Range)_worksheet.Columns[column]).ColumnWidth = width;
        }

        public void SetCellFormat(int row, int column, double height, double width, int fontSize, bool bold, string text)
        {
            SetRowHeight(row, height);
            SetColumnWidth(column, width);
            WriteCell(row, column, text, bold, fontSize);
        }

        public void SetBorders(string range, Excel.XlLineStyle lineStyle, Excel.XlBorderWeight weight, int colorIndex, int borderType)
        {
            if (!_isStarted) return;

            Excel.Range borderRange = _worksheet.Range[range];
            Excel.Borders borders = borderRange.Borders;

            var borderIndexes = new List<Excel.XlBordersIndex>();
            switch (borderType)
            {
                case 0: // Around cell
                    borderIndexes.Add(Excel.XlBordersIndex.xlEdgeLeft);
                    borderIndexes.Add(Excel.XlBordersIndex.xlEdgeTop);
                    borderIndexes.Add(Excel.XlBordersIndex.xlEdgeBottom);
                    borderIndexes.Add(Excel.XlBordersIndex.xlEdgeRight);
                    break;
                case 1: // Around + inside
                    borderIndexes.Add(Excel.XlBordersIndex.xlEdgeLeft);
                    borderIndexes.Add(Excel.XlBordersIndex.xlEdgeTop);
                    borderIndexes.Add(Excel.XlBordersIndex.xlEdgeBottom);
                    borderIndexes.Add(Excel.XlBordersIndex.xlEdgeRight);
                    borderIndexes.Add(Excel.XlBordersIndex.xlInsideVertical);
                    borderIndexes.Add(Excel.XlBordersIndex.xlInsideHorizontal);
                    break;
                case 2: // Left + right
                    borderIndexes.Add(Excel.XlBordersIndex.xlEdgeLeft);
                    borderIndexes.Add(Excel.XlBordersIndex.xlEdgeRight);
                    break;
                case 3: // Inside vertical
                    borderIndexes.Add(Excel.XlBordersIndex.xlInsideVertical);
                    break;
                case 4: // Inside horizontal
                    borderIndexes.Add(Excel.XlBordersIndex.xlInsideHorizontal);
                    break;
                case 5: // Inside both
                    borderIndexes.Add(Excel.XlBordersIndex.xlInsideVertical);
                    borderIndexes.Add(Excel.XlBordersIndex.xlInsideHorizontal);
                    break;
                case 6: // Bottom + inside vertical
                    borderIndexes.Add(Excel.XlBordersIndex.xlEdgeBottom);
                    borderIndexes.Add(Excel.XlBordersIndex.xlInsideVertical);
                    break;
                case 7: // Top only
                    borderIndexes.Add(Excel.XlBordersIndex.xlEdgeTop);
                    break;
            }

            foreach (var borderIndex in borderIndexes)
            {
                borders[borderIndex].LineStyle = lineStyle;
                borders[borderIndex].Weight = weight;
                borders[borderIndex].ColorIndex = colorIndex;
            }

            Marshal.ReleaseComObject(borders);
            Marshal.ReleaseComObject(borderRange);
        }

        public string FormatFloatToTime(float timeValue)
        {
            int hours = (int)timeValue;
            int minutes = (int)Math.Round((timeValue - hours) * 60);
            return $"{hours}:{minutes:00}";
        }

        public void MakeVisible(bool visible = true)
        {
            if (!_isStarted) return;
            _excelApp.Visible = visible;
        }

        public void SetPageSetup(bool landscape = true, int fitToPagesWide = 1, int fitToPagesTall = 1)
        {
            if (!_isStarted) return;
            _worksheet.PageSetup.Orientation = landscape ?
                Excel.XlPageOrientation.xlLandscape :
                Excel.XlPageOrientation.xlPortrait;

            _worksheet.PageSetup.Zoom = false;
            _worksheet.PageSetup.FitToPagesWide = fitToPagesWide;
            _worksheet.PageSetup.FitToPagesTall = fitToPagesTall;
        }

        public void SaveAs(string fileName)
        {
            if (!_isStarted) return;
            _workbook.SaveAs(fileName);
        }

        public void Dispose()
        {
            FreeExcel();
            GC.SuppressFinalize(this);
        }

        public void FreeExcel()
        {
            if (!_isStarted) return;

            try
            {
                if (_workbook != null)
                {
                    _workbook.Close(false);
                    Marshal.ReleaseComObject(_workbook);
                }

                if (_excelApp != null)
                {
                    _excelApp.Quit();
                    Marshal.ReleaseComObject(_excelApp);
                }

                if (_worksheet != null)
                {
                    Marshal.ReleaseComObject(_worksheet);
                }
            }
            finally
            {
                _worksheet = null;
                _workbook = null;
                _excelApp = null;
                _isStarted = false;
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }

        // Specific report methods
        public void GenerateTabelWorkingReport()
        {
            try
            {
                NewBook();
                WriteCell(1, 1, "Employee Report", true, 14);
                MergeCells("A1:D1");
                SetBorders("A1:D10", Excel.XlLineStyle.xlContinuous,
                          Excel.XlBorderWeight.xlThin, 1, 0);
                MakeVisible(true);
            }
            catch (Exception ex)
            {
                throw new Exception("Report generation failed: " + ex.Message);
            }
        }

        public void GenerateOtchetSort(Dictionary<string, float> data, DateTime startDate, DateTime endDate)
        {
            try
            {
                NewBook(data.Count);
                int sheetIndex = 1;

                foreach (var employee in data)
                {
                    SelectSheet(sheetIndex);
                    SetSheetName(employee.Key.Substring(0, Math.Min(employee.Key.Length, 31)));

                    // Header
                    WriteCell(2, 5, $"Приложение № 1 к приказу №         от", false, 12);
                    MergeCells("E2:H2");
                    WriteCell(4, 2, $"Наряд №       с {startDate:dd.MM.yyyy} по {endDate:dd.MM.yyyy}г.", true, 15);
                    MergeCells("B4:C4");

                    // Employee data
                    WriteCell(7, 2, "Подразделение");
                    WriteCell(7, 4, "Бригада");
                    WriteCell(8, 2, "Оранжерейный комплекс");
                    WriteCell(8, 4, "Сортировки");

                    // Table headers
                    string[] headers = {
                        "Сотрудник (ФИО)", "Наим-ие операции", "Вид оплаты",
                        "Колич. шт.", "Расценка руб.", "Начисл. руб.", "Подпись"
                    };

                    for (int i = 0; i < headers.Length; i++)
                    {
                        WriteCell(11, i + 2, headers[i]);
                    }

                    // Add actual data
                    int row = 12;
                    float total = 0;
                    WriteCell(row, 2, employee.Key);
                    // Add other data cells here...
                    total += employee.Value;

                    // Footer
                    WriteCell(row + 1, 7, "Итого:", true);
                    WriteCell(row + 1, 8, total.ToString("0.00"), true);

                    SetPageSetup();
                    sheetIndex++;
                }

                MakeVisible(true);
            }
            catch (Exception ex)
            {
                throw new Exception("Sort report failed: " + ex.Message);
            }
        }

        public void GenerateNaradRabochimReport(WorkersDatabase.TeamReportData data, string periodLabel, bool includeHours)
        {
            try
            {
                NewBook(data.Workers.Count);
                int sheetIndex = 1;

                foreach (var worker in data.Workers)
                {
                    SelectSheet(sheetIndex);
                    SetSheetName($"{sheetIndex}_{worker.LastName}_{worker.FirstName[0]}");

                    // Header
                    WriteCell(2, 3, $"Наряд №________ {periodLabel}", false, 12);
                    WriteCell(3, 3, "Подразделение");
                    WriteCell(3, 4, worker.Department);
                    WriteCell(4, 3, "Бригада");
                    WriteCell(4, 4, worker.Team);
                    WriteCell(5, 3, "Работник");
                    WriteCell(5, 4, $"{worker.LastName} {worker.FirstName} {worker.MiddleName}");

                    // Table headers
                    string[] headers = includeHours ?
                        new[] { "№", "Название работы", "Часы", "Количество", "Расценка", "Сумма", "Подпись" } :
                        new[] { "№", "Название работы", "Количество", "Расценка", "Сумма", "Подпись" };

                    for (int i = 0; i < headers.Length; i++)
                    {
                        WriteCell(7, i + 2, headers[i]);
                    }

                    // Work items
                    int row = 8;
                    int counter = 1;
                    float totalAmount = 0;

                    foreach (var work in worker.WorkItems)
                    {
                        WriteCell(row, 2, counter.ToString());
                        WriteCell(row, 3, work.WorkName);

                        int colOffset = 0;
                        if (includeHours)
                        {
                            WriteCell(row, 4, FormatFloatToTime(work.Hours));
                            colOffset = 1;
                        }

                        WriteCell(row, 4 + colOffset, work.Quantity.ToString());
                        WriteCell(row, 5 + colOffset, work.Rate.ToString("0.0000"));

                        float amount = work.Quantity * work.Rate;
                        WriteCell(row, 6 + colOffset, amount.ToString("0.00"));
                        WriteCell(row, 7 + colOffset, ""); // Empty column for signature

                        totalAmount += amount;
                        row++;
                        counter++;
                    }

                    // Total row
                    WriteCell(row, 3, "Итого:", true);
                    int totalCol = includeHours ? 6 : 5;
                    WriteCell(row, totalCol, totalAmount.ToString("0.00"), true);

                    // Signatures
                    row += 3;
                    WriteCell(row, 2, "Бригадир: ____________________");
                    row += 2;
                    WriteCell(row, 2, "Начальник участка: ____________________");
                    row += 2;
                    WriteCell(row, 2, "Бухгалтер: ____________________");

                    SetPageSetup();
                    sheetIndex++;
                }

                MakeVisible(true);
            }
            catch (Exception ex)
            {
                throw new Exception("Narad report failed: " + ex.Message);
            }
        }

        public void GenerateNaradRabochimNotHourReport(WorkersDatabase.TeamReportData data, string periodLabel)
        {
            GenerateNaradRabochimReport(data, periodLabel, false);
        }

        public void GenerateTabelWorkingReport(DateTime startDate, DateTime endDate)
        {
            try
            {
                NewBook();
                WriteCell(1, 1, "ТАБЕЛЬ УЧЁТА РАБОЧЕГО ВРЕМЕНИ", true, 16);
                MergeCells("A1:J1");

                WriteCell(3, 1, $"За период: {startDate:dd.MM.yyyy} - {endDate:dd.MM.yyyy}");
                MergeCells("A3:J3");

                // Table headers
                string[] headers = { "№", "ФИО работника", "Должность", "1", "2", "3", "4", "5", "6", "Итого часов" };
                for (int i = 0; i < headers.Length; i++)
                {
                    WriteCell(5, i + 1, headers[i], true);
                }

                // Sample data row
                WriteCell(6, 1, "1");
                WriteCell(6, 2, "Иванов И.И.");
                WriteCell(6, 3, "Рабочий");
                WriteCell(6, 4, "8");
                WriteCell(6, 5, "8");
                WriteCell(6, 6, "8");
                WriteCell(6, 7, "8");
                WriteCell(6, 8, "8");
                WriteCell(6, 9, "40");

                SetBorders("A5:J20", Excel.XlLineStyle.xlContinuous,
                          Excel.XlBorderWeight.xlThin, 1, 1);

                // Signature section
                WriteCell(22, 1, "Ответственный: ____________________");
                WriteCell(23, 1, "Дата: ____________________");

                SetPageSetup();
                MakeVisible(true);
            }
            catch (Exception ex)
            {
                throw new Exception("Tabel report failed: " + ex.Message);
            }
        }

        public void GenerateAccountingReport(DateTime startDate, DateTime endDate)
        {
            try
            {
                NewBook();
                WriteCell(1, 1, "СВОДНЫЙ ОТЧЁТ ДЛЯ БУХГАЛТЕРИИ", true, 16);
                MergeCells("A1:J1");

                WriteCell(3, 1, $"За период: {startDate:dd.MM.yyyy} - {endDate:dd.MM.yyyy}");
                MergeCells("A3:J3");

                // Table headers
                string[] headers = { "№", "ФИО работника", "Табельный №", "Отработано часов", "Начислено", "НДФЛ", "К выплате" };
                for (int i = 0; i < headers.Length; i++)
                {
                    WriteCell(5, i + 1, headers[i], true);
                }

                // Sample data row
                WriteCell(6, 1, "1");
                WriteCell(6, 2, "Иванов И.И.");
                WriteCell(6, 3, "12345");
                WriteCell(6, 4, "168");
                WriteCell(6, 5, "42000.00");
                WriteCell(6, 6, "5460.00");
                WriteCell(6, 7, "36540.00");

                SetBorders("A5:J20", Excel.XlLineStyle.xlContinuous,
                          Excel.XlBorderWeight.xlThin, 1, 1);

                SetPageSetup();
                MakeVisible(true);
            }
            catch (Exception ex)
            {
                throw new Exception("Accounting report failed: " + ex.Message);
            }
        }

        public void GenerateWorkerReport(WorkersDatabase.WorkerReportData data, DateTime startDate, DateTime endDate)
        {
            try
            {
                // Header
                worksheet.Cells[1, 1] = "Индивидуальный отчет";
                worksheet.Cells[2, 1] = $"Период: {startDate:dd.MM.yyyy} - {endDate:dd.MM.yyyy}";
                worksheet.Cells[3, 1] = $"Работник: {data.LastName} {data.FirstName} {data.MiddleName}";
                worksheet.Cells[4, 1] = $"Подразделение: {data.Department}";
                worksheet.Cells[5, 1] = $"Бригада: {data.Team}";

                // Table headers
                int row = 7;
                worksheet.Cells[row, 1] = "Вид работы";
                worksheet.Cells[row, 2] = "Часы";
                worksheet.Cells[row, 3] = "Количество";
                worksheet.Cells[row, 4] = "Ставка";
                worksheet.Cells[row, 5] = "Сумма";

                // Data rows
                decimal total = 0;
                foreach (var item in data.WorkItems)
                {
                    row++;
                    worksheet.Cells[row, 1] = item.WorkName;
                    worksheet.Cells[row, 2] = item.Hours;
                    worksheet.Cells[row, 3] = item.Quantity;
                    worksheet.Cells[row, 4] = item.Rate;

                    decimal amount = (decimal)(item.Hours * item.Rate * item.Quantity);
                    worksheet.Cells[row, 5] = amount;
                    total += amount;
                }

                // Total row
                row++;
                worksheet.Cells[row, 4] = "Итого:";
                worksheet.Cells[row, 5] = total;

                // Formatting
                Excel.Range range = worksheet.Range["A7", $"E{row}"];
                range.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;

                Excel.Range headerRange = worksheet.Range["A7", "E7"];
                headerRange.Interior.Color = Color.LightGray.ToArgb();

                worksheet.Columns.AutoFit();

                // Save and open
                string fileName = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                    $"Отчет_{data.LastName}_{startDate:yyyyMMdd}-{endDate:yyyyMMdd}.xlsx");

                workbook.SaveAs(fileName);
                excelApp.Visible = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка генерации отчёта: {ex.Message}");
            }
        }

        public void GenerateTeamReport(WorkersDatabase.TeamReportData data, DateTime startDate, DateTime endDate)
        {
            try
            {
                // Header
                worksheet.Cells[1, 1] = "Командный отчет";
                worksheet.Cells[2, 1] = $"Период: {startDate:dd.MM.yyyy} - {endDate:dd.MM.yyyy}";
                worksheet.Cells[3, 1] = $"Подразделение: {data.Department}";
                worksheet.Cells[4, 1] = $"Бригада: {data.Team}";

                int row = 6;
                foreach (var worker in data.Workers)
                {
                    // Worker header
                    worksheet.Cells[row, 1] = $"{worker.LastName} {worker.FirstName} {worker.MiddleName}";
                    row++;

                    // Table headers
                    worksheet.Cells[row, 1] = "Вид работы";
                    worksheet.Cells[row, 2] = "Часы";
                    worksheet.Cells[row, 3] = "Количество";
                    worksheet.Cells[row, 4] = "Ставка";
                    worksheet.Cells[row, 5] = "Сумма";

                    // Data rows
                    decimal workerTotal = 0;
                    foreach (var item in worker.WorkItems)
                    {
                        row++;
                        worksheet.Cells[row, 1] = item.WorkName;
                        worksheet.Cells[row, 2] = item.Hours;
                        worksheet.Cells[row, 3] = item.Quantity;
                        worksheet.Cells[row, 4] = item.Rate;

                        decimal amount = (decimal)(item.Hours * item.Rate * item.Quantity);
                        worksheet.Cells[row, 5] = amount;
                        workerTotal += amount;
                    }

                    // Worker total
                    row++;
                    worksheet.Cells[row, 4] = "Итого:";
                    worksheet.Cells[row, 5] = workerTotal;
                    row += 2;
                }

                // Formatting
                Excel.Range range = worksheet.Range["A6", $"E{row}"];
                range.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;

                worksheet.Columns.AutoFit();

                // Save and open
                string fileName = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                    $"Отчет_{data.Department}_{data.Team}_{startDate:yyyyMMdd}-{endDate:yyyyMMdd}.xlsx");

                workbook.SaveAs(fileName);
                excelApp.Visible = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка генерации отчёта: {ex.Message}");
            }
        }

        // In Excel_Library.cs
        public string FormatTimeSpan(TimeSpan time)
        {
            if (time.TotalHours > 24)
                return $"{(int)time.TotalHours}:{time.Minutes:00}";
            return time.ToString(@"hh\:mm");
        }


        // Helper classes for report data
        public class EmployeeReportData
        {
            public string LastName { get; set; }
            public string FirstName { get; set; }
            public string MiddleName { get; set; }
            public string Department { get; set; }
            public string Team { get; set; }
            public float PremiumPercent { get; set; }
            public List<WorkItem> WorkItems { get; set; } = new List<WorkItem>();
        }

        public class WorkItem
        {
            public string WorkName { get; set; }
            public string Unit { get; set; } // "часы" or "сдельно"
            public float RegularQty { get; set; }
            public float HolidayQty { get; set; }
            public float Rate { get; set; }
        }
    }
}