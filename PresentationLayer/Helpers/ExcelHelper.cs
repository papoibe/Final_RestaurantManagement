using ClosedXML.Excel;
using System;
using System.Data;
using System.Windows.Forms;

namespace PresentationLayer.Helpers
{
    public class ExcelHelper
    {
        public static void ExportToExcel(DataGridView dgv, string sheetName, string title)
        {
            try
            {
                using (var workbook = new XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add(sheetName);

                    // Add title
                    worksheet.Cell(1, 1).Value = title;
                    worksheet.Cell(1, 1).Style.Font.Bold = true;
                    worksheet.Cell(1, 1).Style.Font.FontSize = 16;
                    worksheet.Range(1, 1, 1, dgv.Columns.Count).Merge();
                    worksheet.Cell(1, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                    // Add headers
                    for (int i = 0; i < dgv.Columns.Count; i++)
                    {
                        worksheet.Cell(3, i + 1).Value = dgv.Columns[i].HeaderText;
                        worksheet.Cell(3, i + 1).Style.Font.Bold = true;
                    }

                    // Add data
                    for (int i = 0; i < dgv.Rows.Count; i++)
                    {
                        for (int j = 0; j < dgv.Columns.Count; j++)
                        {
                            if (dgv.Rows[i].Cells[j].Value != null)
                            {
                                worksheet.Cell(i + 4, j + 1).Value = dgv.Rows[i].Cells[j].Value.ToString();
                            }
                        }
                    }

                    // Auto-fit columns
                    worksheet.Columns().AdjustToContents();

                    // Show save dialog
                    SaveFileDialog saveDialog = new SaveFileDialog();
                    saveDialog.Filter = "Excel Files|*.xlsx";
                    saveDialog.Title = "Xuất báo cáo";
                    saveDialog.FileName = $"BaoCao_{sheetName}_{DateTime.Now:yyyyMMdd_HHmmss}";

                    if (saveDialog.ShowDialog() == DialogResult.OK)
                    {
                        workbook.SaveAs(saveDialog.FileName);
                        MessageBox.Show("Xuất báo cáo thành công!", "Thông báo",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi xuất báo cáo: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}