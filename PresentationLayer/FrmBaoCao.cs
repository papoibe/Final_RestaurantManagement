using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BusinessLayer; 
using TransferObject;
using System.Windows.Forms.DataVisualization.Charting;
using PresentationLayer.Helpers;


namespace PresentationLayer
{
    public partial class FrmBaoCao: Form
    {

        // khai báo đối tượng bl
        private BaoCaoMonAn_BL baoCaoMonAnBL;
        private BaoCaoDoanhThu_BL baoCaoDoanhThuBL;
        public FrmBaoCao()
        {
            InitializeComponent();
            // Khởi tạo đối tượng bl
            baoCaoMonAnBL = new BaoCaoMonAn_BL();
            baoCaoDoanhThuBL = new BaoCaoDoanhThu_BL();

            // Cấu hình layout cho biểu đồ doanh thu
            pnlChiTietDoanhThu.Controls.Add(chartDoanhThu);
            pnlChiTietDoanhThu.Controls.Add(dgvDoanhThu);
            pnlChiTietDoanhThu.Controls.Add(pnlTongDoanhThu);

            dgvDoanhThu.Dock = DockStyle.Top;
            dgvDoanhThu.Height = 200;

            chartDoanhThu.Dock = DockStyle.Fill;
            pnlTongDoanhThu.Dock = DockStyle.Bottom;
            pnlTongDoanhThu.Height = 100;

            // Cấu hình ngày mặc định
            dtpTuNgay.Value = DateTime.Now.AddDays(-30);
            dtpDenNgay.Value = DateTime.Now;

            // Ẩn các panel chi tiết khi mới mở form
            pnlChiTietDoanhThu.Visible = false;
            pnlChiTietMonAn.Visible = false;
            pnlDoanhThuTheoThang.Visible = false;
            pnlTonKho.Visible = false;
            pnlDoanhThuTheoLoai.Visible = false;
        }

        private void FrmBaoCao_Load(object sender, EventArgs e)
        {
            // tạo giá trị măc định cho control
            dtpTuNgay.Value = DateTime.Now.AddDays(-30); // mặc định 30 ngày trước 
            dtpDenNgay.Value = DateTime.Now; // mặc định ngày hiện tại
            nudNam.Value = DateTime.Now.Year; // Mặc định năm hiện tại


            // Ẩn các panel chi tiết khi mới mở form
            pnlChiTietDoanhThu.Visible = false;
            pnlChiTietMonAn.Visible = false;
            pnlDoanhThuTheoThang.Visible = false;
            pnlTonKho.Visible = false;
            pnlDoanhThuTheoLoai.Visible = false;

        }

        private void btnBaoCaoDoanhThu_Click(object sender, EventArgs e)
        {
            try
            {
                //hiển thị pnl báo cáo doanh thu
                pnlChiTietDoanhThu.Visible = true;
                pnlChiTietMonAn.Visible = false;

                pnlDoanhThuTheoThang.Visible = false;
                pnlTonKho.Visible = false;
                pnlDoanhThuTheoLoai.Visible = false;

                // lấy dữ liệu từ ngày bắt đầu đến ngày kết thúc
                DateTime tuNgay = dtpTuNgay.Value;
                DateTime denNgay = dtpDenNgay.Value;
                
                // kiểm tra điều kiện 
                if (tuNgay > denNgay)
                {
                    MessageBox.Show("Từ ngày phải nhỏ hơn hoặc bằng đến ngày!", "Lỗi",
                       MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                //gọi pt lấy dữ liệu báo cáo doanh thu
                DataTable dtDoanhThu = baoCaoDoanhThuBL.GetBaoCaoDoanhThuTheoNgay(tuNgay, denNgay);

                // Hiển thị dữ liệu lên DataGridView
                dgvDoanhThu.DataSource = dtDoanhThu;

                // Tính tổng doanh thu
                decimal tongDoanhThu = 0;
                foreach (DataRow row in dtDoanhThu.Rows)
                {
                    tongDoanhThu += Convert.ToDecimal(row["TongDoanhThu"]);
                }

                // Hiển thị tổng doanh thu
                lblTongDoanhThu.Text = string.Format("{0:#,##0} VNĐ", tongDoanhThu);

                // Hiển thị biểu đồ 
                HienThiBieuDoDoanhThu(dtDoanhThu);

            }
            catch (Exception ex)
            {

                MessageBox.Show("Lỗi hiển thị báo cáo doanh thu: " + ex.Message, "Lỗi",
                                  MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnBaoCaoMonAn_Click(object sender, EventArgs e)
        {
            try
            {
                // Hiển thị panel báo cáo món ăn và ẩn panel báo cáo doanh thu
                pnlChiTietMonAn.Visible = true;
                pnlChiTietDoanhThu.Visible = false;
                pnlDoanhThuTheoThang.Visible = false;
                pnlTonKho.Visible = false;
                pnlDoanhThuTheoLoai.Visible = false;

                // Lấy thông tin từ ngày đến ngày
                DateTime tuNgay = dtpTuNgay.Value;
                DateTime denNgay = dtpDenNgay.Value;

                // Kiểm tra điều kiện hợp lệ
                if (tuNgay > denNgay)
                {
                    MessageBox.Show("Từ ngày phải nhỏ hơn hoặc bằng đến ngày!", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                // Số lượng món ăn cần hiển thị
                int soLuong = 10; // Mặc định top 10 món ăn bán chạy
                if (!string.IsNullOrEmpty(txtSoLuongHienThi.Text))
                {
                    soLuong = Convert.ToInt32(txtSoLuongHienThi.Text);
                }

                // Gọi phương thức lấy dữ liệu báo cáo món ăn
                DataTable dtMonAn = baoCaoMonAnBL.GetBaoCaoMonAnBanChay(tuNgay, denNgay, soLuong);

                // Hiển thị dữ liệu lên DataGridView
                dgvMonAn.DataSource = dtMonAn;
                // Hiển thị biểu đồ món ăn
                HienThiBieuDoMonAn(dtMonAn);

            }
            catch (Exception ex)
            {

                MessageBox.Show("Lỗi hiển thị báo cáo món ăn: " + ex.Message, "Lỗi",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void HienThiBieuDoDoanhThu(DataTable dtDoanhThu)
        {
            try
            {
                // Xóa dữ liệu cũ
                chartDoanhThu.Series.Clear();
                chartDoanhThu.ChartAreas.Clear();
                chartDoanhThu.Titles.Clear();

                // Tạo ChartArea
                ChartArea chartArea = new ChartArea("ChartArea");
                chartArea.AxisX.Title = "Ngày";
                chartArea.AxisY.Title = "Số đơn hàng";
                chartArea.AxisY2.Title = "Doanh thu (VNĐ)";
                chartArea.AxisY2.Enabled = AxisEnabled.True;
                chartDoanhThu.ChartAreas.Add(chartArea);

                // Tạo Series cho số đơn hàng (dạng cột)
                Series seriesSoDonHang = new Series("Số đơn hàng");
                seriesSoDonHang.ChartType = SeriesChartType.Column;
                seriesSoDonHang.XValueMember = "Ngay";
                seriesSoDonHang.YValueMembers = "SoDonHang";
                chartDoanhThu.Series.Add(seriesSoDonHang);

                // Tạo Series cho doanh thu (dạng đường)
                Series seriesDoanhThu = new Series("Doanh thu");
                seriesDoanhThu.ChartType = SeriesChartType.Line;
                seriesDoanhThu.XValueMember = "Ngay";
                seriesDoanhThu.YValueMembers = "TongDoanhThu";
                seriesDoanhThu.YAxisType = AxisType.Secondary;
                chartDoanhThu.Series.Add(seriesDoanhThu);

                // Thêm tiêu đề
                Title title = new Title("Biểu đồ doanh thu theo ngày");
                title.Font = new Font("Arial", 12, FontStyle.Bold);
                chartDoanhThu.Titles.Add(title);

                // Gán dữ liệu
                chartDoanhThu.DataSource = dtDoanhThu;

                // Cập nhật biểu đồ
                chartDoanhThu.DataBind();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi hiển thị biểu đồ: " + ex.Message);
            }
        }

        private void HienThiBieuDoMonAn(DataTable dtMonAn)
        {
            try
            {
                // Xóa dữ liệu cũ
                chartMonAn.Series.Clear();
                chartMonAn.ChartAreas.Clear();
                chartMonAn.Titles.Clear();

                // Tạo ChartArea
                ChartArea chartArea = new ChartArea("ChartArea");
                chartMonAn.ChartAreas.Add(chartArea);

                // Tạo Series cho số lượng
                Series seriesSoLuong = new Series("Số lượng");
                seriesSoLuong.ChartType = SeriesChartType.Column;
                seriesSoLuong.XValueMember = "TenMonAn";
                seriesSoLuong.YValueMembers = "TongSoLuong";
                chartMonAn.Series.Add(seriesSoLuong);

                // Tạo Series cho doanh thu
                Series seriesDoanhThu = new Series("Doanh thu");
                seriesDoanhThu.ChartType = SeriesChartType.Line;
                seriesDoanhThu.XValueMember = "TenMonAn";
                seriesDoanhThu.YValueMembers = "TongDoanhThu";
                seriesDoanhThu.YAxisType = AxisType.Secondary;
                chartMonAn.Series.Add(seriesDoanhThu);

                // Thêm tiêu đề
                Title title = new Title("Biểu đồ món ăn bán chạy");
                title.Font = new Font("Arial", 12, FontStyle.Bold);
                chartMonAn.Titles.Add(title);

                // Định dạng trục X
                chartArea.AxisX.Title = "Tên món ăn";
                chartArea.AxisX.TitleFont = new Font("Arial", 10, FontStyle.Bold);
                chartArea.AxisX.LabelStyle.Angle = -45;

                // Định dạng trục Y (số lượng)
                chartArea.AxisY.Title = "Số lượng";
                chartArea.AxisY.TitleFont = new Font("Arial", 10, FontStyle.Bold);

                // Định dạng trục Y phụ (doanh thu)
                chartArea.AxisY2.Title = "Doanh thu (VNĐ)";
                chartArea.AxisY2.TitleFont = new Font("Arial", 10, FontStyle.Bold);
                chartArea.AxisY2.LabelStyle.Format = "#,##0";
                chartArea.AxisY2.Enabled = AxisEnabled.True;

                // Gán dữ liệu
                chartMonAn.DataSource = dtMonAn;
                chartMonAn.DataBind();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi hiển thị biểu đồ món ăn: " + ex.Message);
            }
        }


        private void HienThiBieuDoDoanhThuTheoThang(DataTable dtDoanhThuTheoThang)
        {
            try
            {
                // Xóa dữ liệu cũ
                chartDoanhThuTheoThang.Series.Clear();
                chartDoanhThuTheoThang.ChartAreas.Clear();
                chartDoanhThuTheoThang.Titles.Clear();

                // Tạo ChartArea
                ChartArea chartArea = new ChartArea("ChartArea");
                chartDoanhThuTheoThang.ChartAreas.Add(chartArea);

                // Tạo Series
                Series series = new Series("Doanh Thu");
                series.ChartType = SeriesChartType.Column;
                series.XValueMember = "Thang";
                series.YValueMembers = "TongDoanhThu";
                chartDoanhThuTheoThang.Series.Add(series);

                // Thêm series đường xu hướng
                Series seriesTrend = new Series("Xu hướng");
                seriesTrend.ChartType = SeriesChartType.Line;
                seriesTrend.XValueMember = "Thang";
                seriesTrend.YValueMembers = "TongDoanhThu";
                seriesTrend.BorderWidth = 3;
                seriesTrend.Color = Color.Red;
                chartDoanhThuTheoThang.Series.Add(seriesTrend);

                // Thêm tiêu đề
                Title title = new Title("Biểu đồ doanh thu theo tháng");
                title.Font = new Font("Arial", 12, FontStyle.Bold);
                chartDoanhThuTheoThang.Titles.Add(title);

                // Định dạng trục X
                chartArea.AxisX.Title = "Tháng";
                chartArea.AxisX.TitleFont = new Font("Arial", 10, FontStyle.Bold);
                chartArea.AxisX.Interval = 1;

                // Định dạng trục Y
                chartArea.AxisY.Title = "Doanh thu (VNĐ)";
                chartArea.AxisY.TitleFont = new Font("Arial", 10, FontStyle.Bold);
                chartArea.AxisY.LabelStyle.Format = "#,##0";

                // Gán dữ liệu
                chartDoanhThuTheoThang.DataSource = dtDoanhThuTheoThang;
                chartDoanhThuTheoThang.DataBind();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi hiển thị biểu đồ doanh thu theo tháng: " + ex.Message);
            }
        }


        private void HienThiBieuDoDoanhThuTheoLoai(DataTable dtDoanhThuTheoLoai)
        {
            try
            {
                // Xóa dữ liệu cũ
                chartDoanhThuTheoLoai.Series.Clear();
                chartDoanhThuTheoLoai.ChartAreas.Clear();
                chartDoanhThuTheoLoai.Titles.Clear();

                // Tạo ChartArea
                ChartArea chartArea = new ChartArea("ChartArea");
                chartDoanhThuTheoLoai.ChartAreas.Add(chartArea);

                // Tạo Series kiểu pie
                Series series = new Series("Doanh Thu");
                series.ChartType = SeriesChartType.Pie;
                chartDoanhThuTheoLoai.Series.Add(series);

                // Thêm tiêu đề
                Title title = new Title("Biểu đồ doanh thu theo loại món ăn");
                title.Font = new Font("Arial", 12, FontStyle.Bold);
                chartDoanhThuTheoLoai.Titles.Add(title);

                // Thêm dữ liệu vào series
                foreach (DataRow row in dtDoanhThuTheoLoai.Rows)
                {
                    string tenLoai = row["TenLoai"].ToString();
                    decimal doanhThu = Convert.ToDecimal(row["DoanhThuLoai"]);
                    series.Points.AddXY(tenLoai, doanhThu);
                }

                // Định dạng series
                series.IsValueShownAsLabel = true;
                series.LabelFormat = "#,##0";
                series["PieLabelStyle"] = "Outside";
                series["PieLineColor"] = "Black";
                series.Label = "#PERCENT{P0}";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi hiển thị biểu đồ doanh thu theo loại: " + ex.Message);
            }
        }

        private void btnDoanhThuTheoThang_Click(object sender, EventArgs e)
        {
            try
            {
                // Hiển thị panel báo cáo doanh thu theo tháng và ẩn các panel khác
                pnlDoanhThuTheoThang.Visible = true;
                pnlChiTietDoanhThu.Visible = false;
                pnlChiTietMonAn.Visible = false;
                pnlTonKho.Visible = false;
                pnlDoanhThuTheoLoai.Visible = false;

                // Lấy năm từ control
                int nam = (int)nudNam.Value;

                // Gọi phương thức lấy dữ liệu báo cáo doanh thu theo tháng
                DataTable dtDoanhThuTheoThang = baoCaoDoanhThuBL.GetBaoCaoDoanhThuTheoThang(nam);

                // Hiển thị dữ liệu lên DataGridView
                dgvDoanhThuTheoThang.DataSource = dtDoanhThuTheoThang;

                // Tính tổng doanh thu năm
                decimal tongDoanhThuNam = 0;
                foreach (DataRow row in dtDoanhThuTheoThang.Rows)
                {
                    tongDoanhThuNam += Convert.ToDecimal(row["TongDoanhThu"]);
                }

                // Hiển thị tổng doanh thu năm
                lblTongDoanhThuNam.Text = string.Format("{0:#,##0} VNĐ", tongDoanhThuNam);

                // Hiển thị biểu đồ doanh thu theo tháng
                HienThiBieuDoDoanhThuTheoThang(dtDoanhThuTheoThang);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi hiển thị báo cáo doanh thu theo tháng: " + ex.Message, "Lỗi",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnBaoCaoTonKho_Click(object sender, EventArgs e)
        {
            try
            {
                // Hiển thị panel báo cáo tồn kho và ẩn các panel khác
                pnlTonKho.Visible = true;
                pnlDoanhThuTheoThang.Visible = false;
                pnlChiTietDoanhThu.Visible = false;
                pnlChiTietMonAn.Visible = false;
                pnlDoanhThuTheoLoai.Visible = false;

                // Gọi phương thức lấy dữ liệu báo cáo tồn kho
                DataTable dtTonKho = baoCaoDoanhThuBL.GetBaoCaoTonKho();

                // Hiển thị dữ liệu lên DataGridView
                dgvTonKho.DataSource = dtTonKho;

                // Tính tổng số lượng nguyên vật liệu
                int tongSoNguyenLieu = dtTonKho.Rows.Count;
                int soLuongCanNhap = 0;
                int soLuongSapHet = 0;

                foreach (DataRow row in dtTonKho.Rows)
                {
                    string trangThai = row["TrangThai"].ToString();
                    if (trangThai == "Cần nhập thêm")
                        soLuongCanNhap++;
                    else if (trangThai == "Sắp hết")
                        soLuongSapHet++;
                }

                // Hiển thị thông tin tồn kho
                lblTongSoNguyenLieu.Text = tongSoNguyenLieu.ToString();
                lblSoLuongCanNhap.Text = soLuongCanNhap.ToString();
                lblSoLuongSapHet.Text = soLuongSapHet.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi hiển thị báo cáo tồn kho: " + ex.Message, "Lỗi",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDoanhThuTheoLoai_Click(object sender, EventArgs e)
        {
            try
            {
                // Hiển thị panel báo cáo doanh thu theo loại và ẩn các panel khác
                pnlDoanhThuTheoLoai.Visible = true;
                pnlTonKho.Visible = false;
                pnlDoanhThuTheoThang.Visible = false;
                pnlChiTietDoanhThu.Visible = false;
                pnlChiTietMonAn.Visible = false;

                // Lấy thông tin từ ngày đến ngày
                DateTime tuNgay = dtpTuNgay.Value;
                DateTime denNgay = dtpDenNgay.Value;

                // Kiểm tra điều kiện hợp lệ
                if (tuNgay > denNgay)
                {
                    MessageBox.Show("Từ ngày phải nhỏ hơn hoặc bằng đến ngày!", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Gọi phương thức lấy dữ liệu báo cáo doanh thu theo loại món ăn
                DataTable dtDoanhThuTheoLoai = baoCaoDoanhThuBL.GetBaoCaoDoanhThuTheoLoaiMonAn(tuNgay, denNgay);

                // Hiển thị dữ liệu lên DataGridView
                dgvDoanhThuTheoLoai.DataSource = dtDoanhThuTheoLoai;

                // Tính tổng doanh thu
                decimal tongDoanhThu = 0;
                foreach (DataRow row in dtDoanhThuTheoLoai.Rows)
                {
                    tongDoanhThu += Convert.ToDecimal(row["DoanhThuLoai"]);
                }

                // Hiển thị tổng doanh thu
                lblTongDoanhThuLoai.Text = string.Format("{0:#,##0} VNĐ", tongDoanhThu);

                // Hiển thị biểu đồ doanh thu theo loại
                HienThiBieuDoDoanhThuTheoLoai(dtDoanhThuTheoLoai);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi hiển thị báo cáo doanh thu theo loại: " + ex.Message, "Lỗi",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnXuatBaoCao_Click(object sender, EventArgs e)
        {
            try
            {
                // Xác định panel đang hiển thị và xuất báo cáo tương ứng
                if (pnlChiTietDoanhThu.Visible)
                {
                    ExcelHelper.ExportToExcel(dgvDoanhThu, "DoanhThu", "BÁO CÁO DOANH THU");
                }
                else if (pnlChiTietMonAn.Visible)
                {
                    ExcelHelper.ExportToExcel(dgvMonAn, "MonAn", "BÁO CÁO MÓN ĂN BÁN CHẠY");
                }
                else if (pnlDoanhThuTheoThang.Visible)
                {
                    ExcelHelper.ExportToExcel(dgvDoanhThuTheoThang, "DoanhThuTheoThang", "BÁO CÁO DOANH THU THEO THÁNG");
                }
                else if (pnlTonKho.Visible)
                {
                    ExcelHelper.ExportToExcel(dgvTonKho, "TonKho", "BÁO CÁO TỒN KHO");
                }
                else if (pnlDoanhThuTheoLoai.Visible)
                {
                    ExcelHelper.ExportToExcel(dgvDoanhThuTheoLoai, "DoanhThuTheoLoai", "BÁO CÁO DOANH THU THEO LOẠI");
                }
                else
                {
                    MessageBox.Show("Vui lòng chọn loại báo cáo cần xuất!", "Cảnh báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xuất báo cáo: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
