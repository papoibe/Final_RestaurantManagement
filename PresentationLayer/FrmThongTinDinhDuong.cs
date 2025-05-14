using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using BusinessLayer;
using TransferObject;

namespace PresentationLayer
{
    public partial class FrmThongTinDinhDuong : Form
    {
        ThongTinDinhDuong_BL bl = new ThongTinDinhDuong_BL();
        public FrmThongTinDinhDuong()
        {
            InitializeComponent();
        }
        private void btnSearch_Click(object sender, EventArgs e)
        {
            string tenMon = txtSearch.Text.Trim();
            List<ThongTinDinhDuong_DTO> list = bl.LoadDinhDuong(tenMon);
            dgvNutrition.DataSource = list;
            HienThiTenCotTiengViet();
        }


        private void dgvNutrition_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvNutrition.CurrentRow != null && dgvNutrition.CurrentRow.Index >= 0)
            {
                try
                {
                    // Lấy dữ liệu dòng đang chọn
                    string tenMon = dgvNutrition.CurrentRow.Cells["TenMonAn"].Value.ToString();
                    double protein = Convert.ToDouble(dgvNutrition.CurrentRow.Cells["Protein"].Value);
                    double carbohydrate = Convert.ToDouble(dgvNutrition.CurrentRow.Cells["Carbohydrate"].Value);
                    double fat = Convert.ToDouble(dgvNutrition.CurrentRow.Cells["Fat"].Value);
                    double fiber = Convert.ToDouble(dgvNutrition.CurrentRow.Cells["Fiber"].Value);
                    double duong = Convert.ToDouble(dgvNutrition.CurrentRow.Cells["Duong"].Value);

                    double total = protein + carbohydrate + fat + fiber + duong;
                    if (total <= 0)
                    {
                        chartPie.Series.Clear();
                        chartPie.Titles.Clear();
                        chartPie.Titles.Add("Không có dữ liệu để hiển thị.");
                        return;
                    }

                    // Cập nhật biểu đồ
                    chartPie.Series.Clear();
                    chartPie.Titles.Clear();

                    var series = new Series("Tỷ lệ dinh dưỡng");
                    series.ChartType = SeriesChartType.Pie;
                    series.Label = "#PERCENT{P0}\n#VALX";
                    series["PieLabelStyle"] = "Outside";
                    series["PieLineColor"] = "Black";

                    series.Points.AddXY("Protein", protein);
                    series.Points.AddXY("Carbohydrate", carbohydrate);
                    series.Points.AddXY("Fat", fat);
                    series.Points.AddXY("Fiber", fiber);
                    series.Points.AddXY("Đường", duong);

                    chartPie.Series.Add(series);
                    chartPie.Titles.Add("TỶ LỆ DINH DƯỠNG: " + tenMon.ToUpper());
                }
                catch
                {
                    // Tránh lỗi nếu dữ liệu chưa sẵn sàng
                }
            }
        }
        private void HienThiTenCotTiengViet()
        {
            dgvNutrition.Columns["TenMonAn"].HeaderText = "Tên món ăn";
            dgvNutrition.Columns["Calo"].HeaderText = "Calo (kcal)";
            dgvNutrition.Columns["Protein"].HeaderText = "Đạm (g)";
            dgvNutrition.Columns["Carbohydrate"].HeaderText = "Tinh bột (g)";
            dgvNutrition.Columns["Fat"].HeaderText = "Chất béo (g)";
            dgvNutrition.Columns["Fiber"].HeaderText = "Chất xơ (g)";
            dgvNutrition.Columns["Duong"].HeaderText = "Đường (g)";
            dgvNutrition.Columns["Natri"].HeaderText = "Natri (mg)";
            dgvNutrition.Columns["ThanhPhanDiUng"].HeaderText = "Dị ứng";
            dgvNutrition.Columns["GhiChu"].HeaderText = "Ghi chú";
        }
    }
}
