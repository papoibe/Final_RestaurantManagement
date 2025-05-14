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
using System.IO;
using System.Data.SqlClient;
using PresentationLayer.FrmChonKSCL;
using System.Diagnostics;
using PresentationLayer.Helpers;
using ClosedXML.Excel;

namespace PresentationLayer
{
    public partial class FrmKiemSoatChatLuong : Form
    {
        private KiemTraChatLuong_BL kiemTraBL;
        private string maKiemTraCurrent = "";
        private List<string> loaiKiemTraList = new List<string> { "NguyenLieu", "MonAn"};
        private List<string> ketQuaList = new List<string> { "Đạt", "Không đạt", "Cần kiểm tra lại" };
        private string selectedImagePath = "";
        private bool isNewRecord = true;
        public FrmKiemSoatChatLuong()
        {
            InitializeComponent();
            // Khởi tạo danh sách loại kiểm tra chỉ với hai loại: NguyenLieu và MonAn
            loaiKiemTraList = new List<string> { "NguyenLieu", "MonAn" };
            // Khởi tạo business layer để thao tác với dữ liệu
            kiemTraBL = new KiemTraChatLuong_BL();
            // Khởi tạo danh sách kết quả
            ketQuaList = new List<string> { "Đạt", "Không đạt", "Cần kiểm tra lại" };
        }

        // khai báo class thống kê tab báo cáo và biến danh sách
        private class ThongKeBaoCao
        {
            public string LoaiKiemTra { get; set; }
            public int SoLuong { get; set; }
            public int SoDat { get; set; }
            public int SoKhongDat { get; set; }
            public int SoCanKiemTraLai { get; set; }
            public float TyLeDat { get; set; }
        }

        private List<ThongKeBaoCao> danhSachThongKe = new List<ThongKeBaoCao>();

        private void FrmKiemSoatChatLuong_Load(object sender, EventArgs e)
        {
            // Thiết lập người dùng hiện tại
            lblCurrentUser.Text = "Người dùng: (Mặc định @Admin)";

            // Khởi tạo combobox
            LoadComboBoxes();

            // Khởi tạo danh sách kiểm tra
            LoadDanhSachKiemTra();

            // Đặt ngày kiểm tra mặc định là ngày hiện tại
            dtpNgayKiemTra.Value = DateTime.Now;
            timeNgayKiemTra.Value = DateTime.Now;

            // Thiết lập filter date
            dtpFilterFrom.Value = DateTime.Now.AddDays(-30);
            dtpFilterTo.Value = DateTime.Now;

            // Xóa dữ liệu trong form
            ResetForm();

            // Vô hiệu hóa control chi tiết ban đầu
            EnableEditControls(false);

            // Tải thống kê
            LoadThongKe();

            // Khoi tao tab bao cao 
            LoadTabBaoCaoInitial();

        }

        private void LoadTabBaoCaoInitial()
        {
            //Thiet lap cac gia tri mac dinh
            dtpBaoCaoFrom.Value = DateTime.Now.AddMonths(-1);
            dtpBaoCaoTo.Value = DateTime.Now;
            cboBaoCaoLoai.SelectedIndex = 0;
            cboBaoCaoLoai.Items.Clear();
            cboBaoCaoLoai.Items.Add("Tất cả");
            cboBaoCaoLoai.Items.Add("Nguyên liệu");
            cboBaoCaoLoai.Items.Add("Món ăn");
            cboBaoCaoLoai.SelectedIndex = 0;
            //tạo bảng
            DataTable dtBaoCao = new DataTable();
            dtBaoCao.Columns.Add("Loại Kiểm Tra", typeof(string));
            dtBaoCao.Columns.Add("Số Lượng", typeof(int));
            dtBaoCao.Columns.Add("Số Đạt", typeof(int));
            dtBaoCao.Columns.Add("Số Không Đạt", typeof(int));
            dtBaoCao.Columns.Add("Số Cần Kiểm Tra Lại", typeof(int));
            dtBaoCao.Columns.Add("Tỷ Lệ Đạt (%)", typeof(float));


            //gán nguồn dữ liệu cho DataGridCView
            dgvBaoCao.DataSource = dtBaoCao;

            // Định dạng hiển thị cho DataGridView
            FormatBaoCaoDataGridView();


            //load dữ liệu ban đầu 
            LoadBaoCaoData();
        }
        private void FormatBaoCaoDataGridView()
        {
            foreach (DataGridViewColumn col in dgvBaoCao.Columns)
            {
                col.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                col.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                col.HeaderCell.Style.Font = new Font(dgvBaoCao.Font, FontStyle.Bold);
            }
           
        }

        //Hiển thị dữ liệu lên Dgv
        private void DisplayBaoCaoData()
        {
            DataTable dtBaoCao = new DataTable();

            dtBaoCao.Columns.Add("LoaiKiemTra", typeof(string));
            dtBaoCao.Columns.Add("SoLuong", typeof(int));
            dtBaoCao.Columns.Add("SoDat", typeof(int));
            dtBaoCao.Columns.Add("SoKhongDat", typeof(int));
            dtBaoCao.Columns.Add("SoCanKiemTraLai", typeof(int));
            dtBaoCao.Columns.Add("TyLeDat", typeof(float));

            foreach (var item in danhSachThongKe)
            {
                DataRow row = dtBaoCao.NewRow();
                row["LoaiKiemTra"] = item.LoaiKiemTra;
                row["SoLuong"] = item.SoLuong;
                row["SoDat"] = item.SoDat;
                row["SoKhongDat"] = item.SoKhongDat;
                row["SoCanKiemTraLai"] = item.SoCanKiemTraLai;
                row["TyLeDat"] = item.TyLeDat;

                dtBaoCao.Rows.Add(row);
            }

            // Xóa DataSource cũ và gán mới
            dgvBaoCao.DataSource = null;
            dgvBaoCao.DataSource = dtBaoCao;

            // Đặt tiêu đề cột tiếng Việt
            dgvBaoCao.Columns["LoaiKiemTra"].HeaderText = "Loại Kiểm Tra";
            dgvBaoCao.Columns["SoLuong"].HeaderText = "Số Lượng";
            dgvBaoCao.Columns["SoDat"].HeaderText = "Số Đạt";
            dgvBaoCao.Columns["SoKhongDat"].HeaderText = "Số Không Đạt";
            dgvBaoCao.Columns["SoCanKiemTraLai"].HeaderText = "Số Cần Kiểm Tra Lại";
            dgvBaoCao.Columns["TyLeDat"].HeaderText = "Tỷ Lệ Đạt (%)";

            // Định dạng cột tỷ lệ phần trăm
            dgvBaoCao.Columns["TyLeDat"].DefaultCellStyle.Format = "0.00";

            // Định dạng lại DataGridView 
            foreach (DataGridViewRow row in dgvBaoCao.Rows)
            {
                if (row.Cells["LoaiKiemTra"].Value != null &&
                    row.Cells["LoaiKiemTra"].Value.ToString() == "Tổng cộng")
                {
                    row.DefaultCellStyle.BackColor = Color.LightGray;
                    row.DefaultCellStyle.Font = new Font(dgvBaoCao.Font, FontStyle.Bold);
                }
            }
        }


        private void DrawBaoCaoCharts()
        {
            // Xóa các control cũ trong panel
            pnlPieChart.Controls.Clear();
            pnlBarChart.Controls.Clear();

            // Tạo biểu đồ tròn (pie chart)
            DrawPieChart();

            // Tạo biểu đồ cột (bar chart)
            DrawBarChart();
        }

        private void DrawPieChart()
        {
            // Kiểm tra nếu không có dữ liệu
            if (danhSachThongKe.Count == 0 || danhSachThongKe.Sum(tk => tk.SoLuong) == 0)
            {
                Label lblNoData = new Label
                {
                    Text = "Không có dữ liệu",
                    TextAlign = ContentAlignment.MiddleCenter,
                    Dock = DockStyle.Fill,
                    Font = new Font(Font.FontFamily, 12, FontStyle.Bold)
                };
                pnlPieChart.Controls.Add(lblNoData);
                return;
            }

            // Lấy dữ liệu cho biểu đồ tròn theo kết quả kiểm tra
            int tongSo = danhSachThongKe.Sum(tk => tk.SoLuong);
            int soDat = danhSachThongKe.Sum(tk => tk.SoDat);
            int soKhongDat = danhSachThongKe.Sum(tk => tk.SoKhongDat);
            int soCanKiemTraLai = danhSachThongKe.Sum(tk => tk.SoCanKiemTraLai);

            // Tính phần trăm
            float ptDat = (float)soDat / tongSo * 100;
            float ptKhongDat = (float)soKhongDat / tongSo * 100;
            float ptCanKiemTraLai = (float)soCanKiemTraLai / tongSo * 100;

            // Vẽ biểu đồ tròn đơn giản
            PictureBox picPie = new PictureBox
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White
            };

            picPie.Paint += (sender, e) =>
            {
                Graphics g = e.Graphics;
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

                // Kích thước và vị trí của hình tròn
                int size = Math.Min(picPie.Width, picPie.Height) - 40;
                Rectangle rect = new Rectangle(
                    (picPie.Width - size) / 2,
                    (picPie.Height - size) / 2,
                    size, size);

                // Vẽ các phần của biểu đồ
                float startAngle = 0;

                // Phần "Đạt"
                if (soDat > 0)
                {
                    float sweepAngle = ptDat * 360 / 100;
                    g.FillPie(new SolidBrush(Color.Green), rect, startAngle, sweepAngle);
                    startAngle += sweepAngle;
                }

                // Phần "Không đạt"
                if (soKhongDat > 0)
                {
                    float sweepAngle = ptKhongDat * 360 / 100;
                    g.FillPie(new SolidBrush(Color.Red), rect, startAngle, sweepAngle);
                    startAngle += sweepAngle;
                }

                // Phần "Cần kiểm tra lại"
                if (soCanKiemTraLai > 0)
                {
                    float sweepAngle = ptCanKiemTraLai * 360 / 100;
                    g.FillPie(new SolidBrush(Color.Orange), rect, startAngle, sweepAngle);
                }

                // Vẽ chú thích
                int legendY = rect.Bottom + 10;

                // Chú thích "Đạt"
                g.FillRectangle(new SolidBrush(Color.Green), 10, legendY, 20, 15);
                g.DrawString($"Đạt: {soDat} ({ptDat:0.0}%)",
                    new Font(Font.FontFamily, 8), Brushes.Black, 35, legendY);

                // Chú thích "Không đạt"
                legendY += 20;
                g.FillRectangle(new SolidBrush(Color.Red), 10, legendY, 20, 15);
                g.DrawString($"Không đạt: {soKhongDat} ({ptKhongDat:0.0}%)",
                    new Font(Font.FontFamily, 8), Brushes.Black, 35, legendY);

                // Chú thích "Cần kiểm tra lại"
                legendY += 20;
                g.FillRectangle(new SolidBrush(Color.Orange), 10, legendY, 20, 15);
                g.DrawString($"Cần kiểm tra lại: {soCanKiemTraLai} ({ptCanKiemTraLai:0.0}%)",
                    new Font(Font.FontFamily, 8), Brushes.Black, 35, legendY);
            };

            pnlPieChart.Controls.Add(picPie);
        }

        private void DrawBarChart()
        {
            // Kiểm tra nếu không có dữ liệu
            if (danhSachThongKe.Count <= 1) // Loại bỏ dòng tổng cộng
            {
                Label lblNoData = new Label
                {
                    Text = "Không có dữ liệu",
                    TextAlign = ContentAlignment.MiddleCenter,
                    Dock = DockStyle.Fill,
                    Font = new Font(Font.FontFamily, 12, FontStyle.Bold)
                };
                pnlBarChart.Controls.Add(lblNoData);
                return;
            }

            // Lấy dữ liệu cho biểu đồ cột (loại bỏ dòng tổng cộng)
            var dataForChart = danhSachThongKe.Where(tk => tk.LoaiKiemTra != "Tổng cộng").ToList();

            // Vẽ biểu đồ cột đơn giản
            PictureBox picBar = new PictureBox
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White
            };

            picBar.Paint += (sender, e) =>
            {
                Graphics g = e.Graphics;
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

                // Kích thước và vị trí của biểu đồ
                int margin = 40;
                int left = margin;
                int top = margin;
                int width = picBar.Width - 2 * margin;
                int height = picBar.Height - 2 * margin;

                // Vẽ trục x và trục y
                g.DrawLine(Pens.Black, left, top + height, left + width, top + height); // Trục x
                g.DrawLine(Pens.Black, left, top, left, top + height); // Trục y

                // Xác định giá trị max để tỷ lệ
                int maxValue = dataForChart.Max(tk => tk.SoLuong);
                if (maxValue == 0) maxValue = 1; // Tránh chia cho 0

                // Khoảng cách giữa các cột
                int barWidth = width / (dataForChart.Count * 4);
                int barSpace = barWidth / 2;

                // Vẽ các cột
                for (int i = 0; i < dataForChart.Count; i++)
                {
                    var data = dataForChart[i];

                    int x = left + barSpace + i * (barWidth * 4 + barSpace);

                    // Cột tổng số
                    int barHeight = (int)(height * data.SoLuong / maxValue);
                    g.FillRectangle(new SolidBrush(Color.Blue), x, top + height - barHeight, barWidth, barHeight);
                    g.DrawString(data.SoLuong.ToString(), new Font(Font.FontFamily, 8), Brushes.Black,
                        x, top + height - barHeight - 15);

                    // Cột số đạt
                    x += barWidth + barSpace;
                    barHeight = (int)(height * data.SoDat / maxValue);
                    g.FillRectangle(new SolidBrush(Color.Green), x, top + height - barHeight, barWidth, barHeight);
                    g.DrawString(data.SoDat.ToString(), new Font(Font.FontFamily, 8), Brushes.Black,
                        x, top + height - barHeight - 15);

                    // Cột số không đạt
                    x += barWidth + barSpace;
                    barHeight = (int)(height * data.SoKhongDat / maxValue);
                    g.FillRectangle(new SolidBrush(Color.Red), x, top + height - barHeight, barWidth, barHeight);
                    g.DrawString(data.SoKhongDat.ToString(), new Font(Font.FontFamily, 8), Brushes.Black,
                        x, top + height - barHeight - 15);

                    // Cột số cần kiểm tra lại
                    x += barWidth + barSpace;
                    barHeight = (int)(height * data.SoCanKiemTraLai / maxValue);
                    g.FillRectangle(new SolidBrush(Color.Orange), x, top + height - barHeight, barWidth, barHeight);
                    g.DrawString(data.SoCanKiemTraLai.ToString(), new Font(Font.FontFamily, 8), Brushes.Black,
                        x, top + height - barHeight - 15);

                    // Nhãn trục x
                    g.DrawString(data.LoaiKiemTra, new Font(Font.FontFamily, 8), Brushes.Black,
                        left + barSpace + i * (barWidth * 4 + barSpace) + barWidth, top + height + 5);
                }

                // Chú thích
                int legendY = 10;

                // Chú thích "Tổng số"
                g.FillRectangle(new SolidBrush(Color.Blue), width - 120, legendY, 15, 10);
                g.DrawString("Tổng số", new Font(Font.FontFamily, 8), Brushes.Black, width - 100, legendY);

                // Chú thích "Đạt"
                legendY += 15;
                g.FillRectangle(new SolidBrush(Color.Green), width - 120, legendY, 15, 10);
                g.DrawString("Đạt", new Font(Font.FontFamily, 8), Brushes.Black, width - 100, legendY);

                // Chú thích "Không đạt"
                legendY += 15;
                g.FillRectangle(new SolidBrush(Color.Red), width - 120, legendY, 15, 10);
                g.DrawString("Không đạt", new Font(Font.FontFamily, 8), Brushes.Black, width - 100, legendY);

                // Chú thích "Cần kiểm tra lại"
                legendY += 15;
                g.FillRectangle(new SolidBrush(Color.Orange), width - 120, legendY, 15, 10);
                g.DrawString("Cần KT lại", new Font(Font.FontFamily, 8), Brushes.Black, width - 100, legendY);
            };

            pnlBarChart.Controls.Add(picBar);
        }

        //tính và hiển thị tab bao cao
        private void LoadBaoCaoData()
        {
            try
            {
                lblStatus.Text = "Đang tải dữ liệu báo cáo...";
                progressBar.Visible = true;

                // Lấy thông tin từ các điều khiển
                DateTime tuNgay = dtpBaoCaoFrom.Value.Date;
                DateTime denNgay = dtpBaoCaoTo.Value.Date.AddDays(1).AddSeconds(-1); // Đến cuối ngày
                string loaiFilter = cboBaoCaoLoai.SelectedItem.ToString();

                // Lấy danh sách kiểm tra từ Business Layer
                List<KiemTraChatLuong_DTO> dsKiemTra = kiemTraBL.GetKiemTraChatLuongList();

                // Lọc theo khoảng thời gian //note
                var dsKiemTraLoc = dsKiemTra.Where(kt => kt.NgayKiemTra >= tuNgay && kt.NgayKiemTra <= denNgay);
                if (loaiFilter != "Tất cả")
                {
                    string loaiKiemTra = loaiFilter == "Nguyên liệu" ? "NguyenLieu" : "MonAn";
                    dsKiemTraLoc = dsKiemTraLoc.Where(kt => kt.LoaiKiemTra == loaiKiemTra);
                }
                //Làm mới danh sách thống ke
                danhSachThongKe.Clear();

                // tính toán thống kê theo loại 
                var thongKe = dsKiemTraLoc
                    .GroupBy(kt => kt.LoaiKiemTra)
                    .Select(g => new
                    {
                        LoaiKiemTra = g.Key == "NguyenLieu" ? "Nguyên liệu" : "Món ăn",
                        SoLuong = g.Count(),
                        SoDat = g.Count(kt => kt.KetQua == "Đạt"),
                        SoKhongDat = g.Count(kt => kt.KetQua == "Không đạt"),
                        SoCanKiemTraLai = g.Count(kt => kt.KetQua == "Cần kiểm tra lại")
                    })
                    .ToList();

                //tạo đối tượng ThongKeBaoCao tu kết quả thống kê
                foreach (var item in thongKe)
                {
                    ThongKeBaoCao baoCaoItem = new ThongKeBaoCao
                    {
                        LoaiKiemTra = item.LoaiKiemTra,
                        SoLuong = item.SoLuong,
                        SoDat = item.SoDat,
                        SoKhongDat = item.SoKhongDat,
                        SoCanKiemTraLai = item.SoCanKiemTraLai,
                        TyLeDat = item.SoLuong > 0 ? (float)item.SoDat * 100 / item.SoLuong : 0
                    };

                    danhSachThongKe.Add(baoCaoItem);
                }

                // tổng cộng
                if (danhSachThongKe.Count > 0)
                {
                    ThongKeBaoCao tongCong = new ThongKeBaoCao
                    {
                        LoaiKiemTra = "Tổng cộng",
                        SoLuong = danhSachThongKe.Sum(tk => tk.SoLuong),
                        SoDat = danhSachThongKe.Sum(tk => tk.SoDat),
                        SoKhongDat = danhSachThongKe.Sum(tk => tk.SoKhongDat),
                        SoCanKiemTraLai = danhSachThongKe.Sum(tk => tk.SoCanKiemTraLai),
                    };
                    tongCong.TyLeDat = tongCong.SoLuong > 0 ? (float)tongCong.SoDat * 100 / tongCong.SoLuong : 0;
                    danhSachThongKe.Add(tongCong);
                }

                //hiển thị dữ liệu lên Dgv
                DisplayBaoCaoData();

                //vẽ biểu đồ (Chương Đồ Họa lập trình giao diện) 
                DrawBaoCaoCharts();
                progressBar.Visible = false;
                lblStatus.Text = "Đã tải xong báo cáo thống kê";

            }
            catch (Exception ex)
            {

                progressBar.Visible = false;
                lblStatus.Text = "Lỗi khi tải báo cáo: " + ex.Message;
                MessageBox.Show("Lỗi khi tải dữ liệu báo cáo: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Tải dữ liệu thống kê
        private void LoadThongKe()
        {
            try
            {
                // Đếm số lượng kiểm tra theo kết quả
                int soDat = 0;
                int soKhongDat = 0;
                int soCanKiemTraLai = 0;

                // Đếm số lượng cảnh báo
                int soNguyenLieuCanhBao = 0;
                int soMonAnCanhBao = 0;

                // Lấy danh sách kiểm tra
                List<KiemTraChatLuong_DTO> danhSachKiemTra = kiemTraBL.GetKiemTraChatLuongList();

                foreach (var item in danhSachKiemTra)
                {
                    // Đếm theo kết quả
                    if (item.KetQua == "Đạt")
                        soDat++;
                    else if (item.KetQua == "Không đạt")
                    {
                        soKhongDat++;

                        // Đếm theo loại
                        if (item.LoaiKiemTra == "NguyenLieu")
                            soNguyenLieuCanhBao++;
                        else if (item.LoaiKiemTra == "MonAn")
                            soMonAnCanhBao++;
                    }
                    else if (item.KetQua == "Cần kiểm tra lại")
                        soCanKiemTraLai++;
                }

                // Hiển thị số liệu
                lblSoDat.Text = soDat.ToString();
                lblSoKhongDat.Text = soKhongDat.ToString();
                lblSoCanKiemTra.Text = soCanKiemTraLai.ToString();

                lblSoNguyenLieuCanhBao.Text = soNguyenLieuCanhBao.ToString();
                lblSoMonAnCanhBao.Text = soMonAnCanhBao.ToString();


            }
            catch (Exception ex)
            {
                lblStatus.Text = "Lỗi: " + ex.Message;
            }
        }

        // Tải dữ liệu vào combobox
        private void LoadComboBoxes()
        {
            // Loại kiểm tra
            cboLoaiKiemTra.DataSource = null;
            cboLoaiKiemTra.DataSource = loaiKiemTraList;

            // Kết quả
            cboKetQua.DataSource = null;
            cboKetQua.DataSource = ketQuaList;

            // Filter loại
            cboFilterLoai.SelectedIndex = 0;

            // Filter kết quả
            cboFilterKetQua.SelectedIndex = 0;
        }

        // Tải danh sách kiểm tra
        private void LoadDanhSachKiemTra()
        {
            try
            {
                lblStatus.Text = "Đang tải dữ liệu...";
                progressBar.Visible = true;

                // luu tru lua chon hien tai 
                string selectedMaKiemTra = null;
                if (dgvKiemTra.CurrentRow != null && dgvKiemTra.CurrentRow.Cells["MaKiemTra"] != null)
                {
                    selectedMaKiemTra = dgvKiemTra.CurrentRow.Cells["MaKiemTra"].Value.ToString();
                }


                // Lấy danh sách kiểm tra từ Business Layer
                List<KiemTraChatLuong_DTO> dsKiemTra = kiemTraBL.GetKiemTraChatLuongList();

                // Gán DataSource cho DataGridView
                dgvKiemTra.DataSource = null;

                // neu danh sach rong, khong thuc hien gi 
                if (dsKiemTra == null || dsKiemTra.Count == 0)
                {
                    progressBar.Visible = false;
                    lblStatus.Text = "Không có dữ liệu kiểm tra nào.";
                    return;
                }

                // gan data mới
                dgvKiemTra.DataSource = dsKiemTra;

                // dinh dang cac cot sau khi da gan DataSource
                FormatDataGridView();

                // Chọn lại dòng đã chọn trước đó   
                if (!string.IsNullOrEmpty(selectedMaKiemTra))
                {
                    SelectRowByMaKiemTra(selectedMaKiemTra);
                }

                progressBar.Visible = false;
                lblStatus.Text = "Đã tải " + dsKiemTra.Count + " bản ghi";
            }
            catch (SqlException ex)
            {
                progressBar.Visible = false;
                lblStatus.Text = "Lỗi SQL: " + ex.Message;
                MessageBox.Show("Lỗi khi tải danh sách kiểm tra: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                progressBar.Visible = false;
                lblStatus.Text = "Lỗi: " + ex.Message;
                MessageBox.Show("Lỗi không xác định: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // phuong thuc dinh dang cac cot sau khi da gan datasoucre
        private void FormatDataGridView()
        {
            // Đặt tên cột
            dgvKiemTra.Columns["MaKiemTra"].HeaderText = "Mã Kiểm Tra";
            dgvKiemTra.Columns["LoaiKiemTra"].HeaderText = "Loại Kiểm Tra";
            dgvKiemTra.Columns["DoiTuongKiemTra"].HeaderText = "Đối Tượng Kiểm Tra";
            dgvKiemTra.Columns["NgayKiemTra"].HeaderText = "Ngày Kiểm Tra";
            dgvKiemTra.Columns["NguoiKiemTra"].HeaderText = "Người Kiểm Tra";
            dgvKiemTra.Columns["TieuChiKiemTra"].HeaderText = "Tiêu Chí Kiểm Tra";
            dgvKiemTra.Columns["GiaTri"].HeaderText = "Giá Trị";
            dgvKiemTra.Columns["DonVi"].HeaderText = "Đơn Vị";
            dgvKiemTra.Columns["KetQua"].HeaderText = "Kết Quả";
            dgvKiemTra.Columns["GhiChu"].HeaderText = "Ghi Chú";
            // Ẩn cột không cần thiết
            dgvKiemTra.Columns["HinhAnh"].Visible = false;
            // Đặt chiều rộng cột
            foreach (DataGridViewColumn col in dgvKiemTra.Columns)
            {
                col.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                col.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                col.HeaderCell.Style.Font = new Font(dgvKiemTra.Font, FontStyle.Bold);
                col.HeaderCell.Style.BackColor = Color.LightGray;
                col.HeaderCell.Style.ForeColor = Color.Black;
                col.HeaderCell.Style.SelectionBackColor = Color.Gray;
                col.HeaderCell.Style.SelectionForeColor = Color.White;
            }
        }

        // dinh dang mau cho KetQua
        private void dgvKiemTra_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e == null || dgvKiemTra == null || dgvKiemTra.Columns == null || e.ColumnIndex < 0 || e.ColumnIndex >= dgvKiemTra.Columns.Count)
                return;

            if (dgvKiemTra.Columns[e.ColumnIndex].Name == "KetQua" && e.Value != null)
            {
                string ketQua = e.Value.ToString();
                if (ketQua == "Đạt")
                {
                    e.CellStyle.ForeColor = Color.Green;
                    e.CellStyle.Font = new Font(dgvKiemTra.Font, FontStyle.Bold);
                }
                else if (ketQua == "Không đạt")
                {
                    e.CellStyle.ForeColor = Color.Red;
                    e.CellStyle.Font = new Font(dgvKiemTra.Font, FontStyle.Bold);
                }
                else if (ketQua == "Cần kiểm tra lại")
                {
                    e.CellStyle.ForeColor = Color.DarkOrange;
                    e.CellStyle.Font = new Font(dgvKiemTra.Font, FontStyle.Bold);
                }
            }
        }

        private void EnableEditControls(bool enable)
        {
            // Điều khiển trạng thái các controls
            cboLoaiKiemTra.Enabled = enable;
            txtDoiTuong.Enabled = enable;
            btnChonDoiTuong.Enabled = enable;
            txtMaNhanVien.Enabled = true; // Luôn cho phép nhập mã nhân viên
            txtTieuChi.Enabled = enable;
            txtGiaTri.Enabled = enable;
            txtDonVi.Enabled = enable;
            cboKetQua.Enabled = enable;
            txtGhiChu.Enabled = enable;
            btnChonAnh.Enabled = enable;

            btnThem.Enabled = enable;
            btnXoa.Enabled = enable;
            btnCapNhat.Enabled = enable;
            btnLamMoi.Enabled = enable;
        }


        private void ResetForm()
        {
            // Reset thông tin nhập
            txtMaKiemTra.Text = "";
            txtMaNhanVien.Text = "";
            txtDoiTuong.Text = "";
            txtTieuChi.Text = "";
            txtGiaTri.Text = "0";
            txtDonVi.Text = "";
            txtGhiChu.Text = "";
            dtpNgayKiemTra.Value = DateTime.Now;
            timeNgayKiemTra.Value = DateTime.Now;

            // Reset ảnh
            picAnhMinhHoa.Image = null;
            selectedImagePath = "";

            // Reset mã kiểm tra hiện tại
            maKiemTraCurrent = "";
            isNewRecord = true;
        }

        //kiem tra nhan vien ton tai
        private bool KiemTraNhanVienTonTai(int maNhanVien)
        {
            try
            {
                return kiemTraBL.KiemTraNhanVienTonTai(maNhanVien);
            }
            catch (Exception ex)
            {
                lblStatus.Text = "Lỗi: " + ex.Message;
                MessageBox.Show("Lỗi kiểm tra nhân viên: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        //Xu ly viec them phieu kiem tra
        private void ThemPhieuKiemTra(KiemTraChatLuong_DTO kiemTra)
        {
            try
            {
                // Thêm phiếu kiểm tra vào CSDL
                bool result = kiemTraBL.AddKiemTraChatLuong(kiemTra);
                if (result)
                {
                    MessageBox.Show("Thêm phiếu kiểm tra thành công.", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    //Enable 
                    EnableEditControls(true); 
                    // Tải lại danh sách kiểm tra
                    LoadDanhSachKiemTra();

                    // Hiển thị thông tin phiếu vừa tạo
                    KiemTraChatLuong_DTO kiemTraMoi = kiemTraBL.GetKiemTraById(kiemTra.MaKiemTra);
                    if (kiemTraMoi != null)
                    {
                        maKiemTraCurrent = kiemTraMoi.MaKiemTra;
                        txtMaKiemTra.Text = kiemTraMoi.MaKiemTra;
                        SelectRowByMaKiemTra(kiemTraMoi.MaKiemTra);
                    }

                    // cap nhat trang thai
                    lblStatus.Text = "Đã thêm phiếu kiểm tra mới." + kiemTra.MaKiemTra;

                    // CAP NHAT THONG KE
                    LoadThongKe();
                    // Đánh dấu là không phải bản ghi mới nữa 
                    isNewRecord = false;



                }
                else
                {
                    MessageBox.Show("Không thể thêm phiếu kiểm tra.", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (SqlException ex)
            {
                lblStatus.Text = "Lỗi SQL: " + ex.Message;
                MessageBox.Show("Lỗi khi thêm phiếu kiểm tra: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                lblStatus.Text = "Lỗi: " + ex.Message;
                MessageBox.Show("Lỗi không xác định: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }





        private void btnThemKiemTra_Click(object sender, EventArgs e)
        {
            try
            {
                EnableEditControls(true);
                // Hiển thị thông báo đang xử lý
                lblStatus.Text = "Đang tạo phiếu kiểm tra...";
                progressBar.Visible = true;

                // Lấy và kiểm tra mã nhân viên từ textbox
                int maNhanVien;
                if (!int.TryParse(txtMaNhanVien.Text.Trim(), out maNhanVien) || maNhanVien <= 0)
                {
                    MessageBox.Show("Mã nhân viên phải là số nguyên dương.", "Cảnh báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtMaNhanVien.Focus();
                    progressBar.Visible = false;
                    lblStatus.Text = "Sẵn sàng";
                    return;
                }

                // Kiểm tra mã nhân viên có tồn tại trong CSDL không
                if (!KiemTraNhanVienTonTai(maNhanVien))
                {
                    MessageBox.Show("Mã nhân viên không tồn tại trong hệ thống.", "Cảnh báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtMaNhanVien.Focus();
                    progressBar.Visible = false;
                    lblStatus.Text = "Sẵn sàng";
                    return;
                }

                // Kiểm tra đã chọn loại kiểm tra chưa
                if (cboLoaiKiemTra.SelectedItem == null)
                {
                    MessageBox.Show("Vui lòng chọn loại kiểm tra.", "Cảnh báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    cboLoaiKiemTra.Focus();
                    progressBar.Visible = false;
                    lblStatus.Text = "Sẵn sàng";
                    return;
                }

                // Kiểm tra đã chọn kết quả chưa
                if (cboKetQua.SelectedItem == null)
                {
                    MessageBox.Show("Vui lòng chọn kết quả kiểm tra.", "Cảnh báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    cboKetQua.Focus();
                    progressBar.Visible = false;
                    lblStatus.Text = "Sẵn sàng";
                    return;
                }

                // Kiểm tra đã nhập đối tượng kiểm tra chưa
                if (string.IsNullOrWhiteSpace(txtDoiTuong.Text))
                {
                    MessageBox.Show("Vui lòng chọn đối tượng kiểm tra.", "Cảnh báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    btnChonDoiTuong.Focus();
                    progressBar.Visible = false;
                    lblStatus.Text = "Sẵn sàng";
                    return;
                }

                // Tạo đối tượng kiểm tra mới
                KiemTraChatLuong_DTO kiemTra = new KiemTraChatLuong_DTO();
                kiemTra.LoaiKiemTra = cboLoaiKiemTra.SelectedItem.ToString();
                kiemTra.DoiTuongKiemTra = txtDoiTuong.Text.Trim();

                // Kết hợp ngày và giờ
                DateTime ngayGio = dtpNgayKiemTra.Value.Date.Add(timeNgayKiemTra.Value.TimeOfDay);
                kiemTra.NgayKiemTra = ngayGio;

                kiemTra.NguoiKiemTra = maNhanVien;
                kiemTra.TieuChiKiemTra = txtTieuChi.Text.Trim();

                // Xử lý giá trị
                float giaTri;
                if (float.TryParse(txtGiaTri.Text.Trim(), out giaTri))
                    kiemTra.GiaTri = giaTri;
                else
                    kiemTra.GiaTri = null;

                kiemTra.DonVi = txtDonVi.Text.Trim();
                kiemTra.KetQua = cboKetQua.SelectedItem.ToString();
                kiemTra.GhiChu = txtGhiChu.Text.Trim();

                // Xử lý ảnh nếu có
                if (!string.IsNullOrEmpty(selectedImagePath))
                {
                    // Lưu ảnh hoặc tên file
                    kiemTra.HinhAnh = Path.GetFileName(selectedImagePath);
                }
                else
                {
                    kiemTra.HinhAnh = null;
                }

                // Sử dụng một method riêng để thêm phiếu kiểm tra để dễ xử lý lỗi
                ThemPhieuKiemTra(kiemTra);
            }
            catch (Exception ex)
            {
                progressBar.Visible = false;
                lblStatus.Text = "Lỗi: " + ex.Message;
                MessageBox.Show("Lỗi khi tạo phiếu kiểm tra: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        // Phương thức chọn dòng trong DataGridView theo mã kiểm tra
        private void SelectRowByMaKiemTra(string maKiemTra)
        {
            try
            {
                foreach (DataGridViewRow row in dgvKiemTra.Rows)
                {
                    if (row.Cells["MaKiemTra"].Value.ToString() == maKiemTra)
                    {
                        dgvKiemTra.CurrentCell = row.Cells[0];
                        dgvKiemTra.Rows[row.Index].Selected = true;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi khi chọn dòng: " + ex.Message);
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            try
            {
                // Kiểm tra mã kiểm tra hiện tại
                if (string.IsNullOrEmpty(maKiemTraCurrent))
                {
                    MessageBox.Show("Vui lòng tạo phiếu kiểm tra trước khi thêm chi tiết.", "Cảnh báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Kiểm tra dữ liệu nhập
                if (string.IsNullOrWhiteSpace(txtDoiTuong.Text))
                {
                    MessageBox.Show("Vui lòng nhập đối tượng kiểm tra.", "Cảnh báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtDoiTuong.Focus();
                    return;
                }

                if (string.IsNullOrWhiteSpace(cboKetQua.Text))
                {
                    MessageBox.Show("Vui lòng chọn kết quả kiểm tra.", "Cảnh báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    cboKetQua.Focus();
                    return;
                }

                // Kiểm tra mã nhân viên
                int maNhanVien;
                if (!int.TryParse(txtMaNhanVien.Text.Trim(), out maNhanVien) || maNhanVien <= 0)
                {
                    MessageBox.Show("Mã nhân viên phải là số nguyên dương.", "Cảnh báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtMaNhanVien.Focus();
                    return;
                }

                // Kiểm tra nhân viên tồn tại
                if (!KiemTraNhanVienTonTai(maNhanVien))
                {
                    MessageBox.Show("Mã nhân viên không tồn tại trong hệ thống.", "Cảnh báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtMaNhanVien.Focus();
                    return;
                }

                // Tạo đối tượng kiểm tra
                KiemTraChatLuong_DTO kiemTra = new KiemTraChatLuong_DTO();
                kiemTra.MaKiemTra = maKiemTraCurrent;
                kiemTra.LoaiKiemTra = cboLoaiKiemTra.SelectedItem.ToString();
                kiemTra.DoiTuongKiemTra = txtDoiTuong.Text.Trim();
                kiemTra.NgayKiemTra = dtpNgayKiemTra.Value;
                kiemTra.NguoiKiemTra = maNhanVien;
                kiemTra.TieuChiKiemTra = txtTieuChi.Text.Trim();

                // Xử lý giá trị
                float giaTri;
                if (float.TryParse(txtGiaTri.Text.Trim(), out giaTri))
                    kiemTra.GiaTri = giaTri;
                else
                    kiemTra.GiaTri = null;

                kiemTra.DonVi = txtDonVi.Text.Trim();
                kiemTra.KetQua = cboKetQua.SelectedItem.ToString();
                kiemTra.GhiChu = txtGhiChu.Text.Trim();
                kiemTra.HinhAnh = null; // Xử lý hình ảnh nếu cần

                // Cập nhật phiếu kiểm tra
                bool result = kiemTraBL.UpdateKiemTraChatLuong(kiemTra);
                if (result)
                {
                    MessageBox.Show("Cập nhật thông tin kiểm tra thành công.", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Load lại danh sách kiểm tra
                    LoadDanhSachKiemTra();

                    // Chọn lại phiếu kiểm tra vừa cập nhật
                    SelectRowByMaKiemTra(maKiemTraCurrent);
                }
                else
                {
                    MessageBox.Show("Không thể cập nhật thông tin kiểm tra.", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Lỗi SQL khi cập nhật thông tin kiểm tra: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi cập nhật thông tin kiểm tra: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            try
            {
                // Kiểm tra mã kiểm tra hiện tại
                if (string.IsNullOrEmpty(maKiemTraCurrent))
                {
                    MessageBox.Show("Vui lòng chọn phiếu kiểm tra cần xóa.", "Cảnh báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Xác nhận xóa
                DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn xóa phiếu kiểm tra này?",
                    "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    // Xóa phiếu kiểm tra
                    bool success = kiemTraBL.DeleteKiemTraChatLuong(maKiemTraCurrent);
                    if (success)
                    {
                        MessageBox.Show("Xóa phiếu kiểm tra thành công.", "Thông báo",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // Tải lại danh sách kiểm tra
                        LoadDanhSachKiemTra();

                        // Reset form
                        ResetForm();

                        // Vô hiệu hóa các controls
                        EnableEditControls(false);
                    }
                    else
                    {
                        MessageBox.Show("Không thể xóa phiếu kiểm tra.", "Lỗi",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Lỗi SQL khi xóa phiếu kiểm tra: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xóa phiếu kiểm tra: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCapNhat_Click(object sender, EventArgs e)
        {
            try
            {
                // Kiểm tra mã kiểm tra hiện tại
                if (string.IsNullOrEmpty(maKiemTraCurrent))
                {
                    MessageBox.Show("Vui lòng chọn phiếu kiểm tra cần cập nhật.", "Cảnh báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Kiểm tra mã nhân viên
                int maNhanVien;
                if (!int.TryParse(txtMaNhanVien.Text.Trim(), out maNhanVien) || maNhanVien <= 0)
                {
                    MessageBox.Show("Mã nhân viên phải là số nguyên dương.", "Cảnh báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtMaNhanVien.Focus();
                    return;
                }

                // Kiểm tra nhân viên tồn tại
                if (!KiemTraNhanVienTonTai(maNhanVien))
                {
                    MessageBox.Show("Mã nhân viên không tồn tại trong hệ thống.", "Cảnh báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtMaNhanVien.Focus();
                    return;
                }

                // Tạo đối tượng kiểm tra
                KiemTraChatLuong_DTO kiemTra = new KiemTraChatLuong_DTO();
                kiemTra.MaKiemTra = maKiemTraCurrent;
                kiemTra.LoaiKiemTra = cboLoaiKiemTra.SelectedItem.ToString();
                kiemTra.DoiTuongKiemTra = txtDoiTuong.Text.Trim();
                kiemTra.NgayKiemTra = dtpNgayKiemTra.Value;
                kiemTra.NguoiKiemTra = maNhanVien;
                kiemTra.TieuChiKiemTra = txtTieuChi.Text.Trim();

                // Xử lý giá trị
                float giaTri;
                if (float.TryParse(txtGiaTri.Text.Trim(), out giaTri))
                    kiemTra.GiaTri = giaTri;
                else
                    kiemTra.GiaTri = null;

                kiemTra.DonVi = txtDonVi.Text.Trim();
                kiemTra.KetQua = cboKetQua.SelectedItem.ToString();
                kiemTra.GhiChu = txtGhiChu.Text.Trim();
                kiemTra.HinhAnh = null; // Xử lý hình ảnh nếu cần

                // Cập nhật phiếu kiểm tra
                bool result = kiemTraBL.UpdateKiemTraChatLuong(kiemTra);
                if (result)
                {
                    MessageBox.Show("Cập nhật phiếu kiểm tra thành công.", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Load lại danh sách kiểm tra
                    LoadDanhSachKiemTra();

                    // Chọn lại phiếu kiểm tra vừa cập nhật
                    SelectRowByMaKiemTra(maKiemTraCurrent);
                }
                else
                {
                    MessageBox.Show("Không thể cập nhật phiếu kiểm tra.", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Lỗi SQL khi cập nhật phiếu kiểm tra: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi cập nhật phiếu kiểm tra: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            // Reset form
            ResetForm();
            EnableEditControls(true);
        }

        private void dgvKiemTra_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    // Lấy thông tin phiếu kiểm tra được chọn
                    DataGridViewRow row = dgvKiemTra.Rows[e.RowIndex];
                    maKiemTraCurrent = row.Cells["MaKiemTra"].Value.ToString();

                    // Hiển thị thông tin phiếu kiểm tra
                    txtMaKiemTra.Text = maKiemTraCurrent;
                    txtMaNhanVien.Text = row.Cells["NguoiKiemTra"].Value.ToString();

                    // Đảm bảo chọn đúng loại kiểm tra
                    string loaiKiemTra = row.Cells["LoaiKiemTra"].Value.ToString();
                    for (int i = 0; i < cboLoaiKiemTra.Items.Count; i++)
                    {
                        if (cboLoaiKiemTra.Items[i].ToString() == loaiKiemTra)
                        {
                            cboLoaiKiemTra.SelectedIndex = i;
                            break;
                        }
                    }

                    txtDoiTuong.Text = row.Cells["DoiTuongKiemTra"].Value.ToString();
                    dtpNgayKiemTra.Value = Convert.ToDateTime(row.Cells["NgayKiemTra"].Value);

                    // Xử lý các giá trị có thể null
                    txtTieuChi.Text = row.Cells["TieuChiKiemTra"].Value != null && row.Cells["TieuChiKiemTra"].Value != DBNull.Value
                        ? row.Cells["TieuChiKiemTra"].Value.ToString() : "";

                    txtGiaTri.Text = row.Cells["GiaTri"].Value != null && row.Cells["GiaTri"].Value != DBNull.Value
                        ? row.Cells["GiaTri"].Value.ToString() : "0";

                    txtDonVi.Text = row.Cells["DonVi"].Value != null && row.Cells["DonVi"].Value != DBNull.Value
                        ? row.Cells["DonVi"].Value.ToString() : "";

                    // Đảm bảo chọn đúng kết quả
                    string ketQua = row.Cells["KetQua"].Value.ToString();
                    for (int i = 0; i < cboKetQua.Items.Count; i++)
                    {
                        if (cboKetQua.Items[i].ToString() == ketQua)
                        {
                            cboKetQua.SelectedIndex = i;
                            break;
                        }
                    }

                    txtGhiChu.Text = row.Cells["GhiChu"].Value != null && row.Cells["GhiChu"].Value != DBNull.Value
                        ? row.Cells["GhiChu"].Value.ToString() : "";

                    // Enable các control
                    EnableEditControls(true);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi chọn phiếu kiểm tra: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }




        }

        private void dgvKiemTra_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Gọi phương thức CellClick để xử lý thống nhất
            dgvKiemTra_CellClick(sender, e);
        }

        private void btnChonDoiTuong_Click(object sender, EventArgs e)
        {
            try
            {
                // kiem tra loai kiem tra da duoc chon chua
                if (cboLoaiKiemTra.SelectedItem == null)
                {
                    MessageBox.Show("Vui lòng chọn loại kiểm tra trước.", "Cảnh báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    cboLoaiKiemTra.Focus();
                    return;
                }

                string loaiKiemTra = cboLoaiKiemTra.SelectedItem.ToString();

                // xu ly tuy theo loai kiem tra
                switch (loaiKiemTra)
                {
                    case "NguyenLieu":
                        // Mở form chọn nguyên liệu
                         FrmChonKSCL.FrmChonNguyenLieu frmNguyenLieu = new FrmChonKSCL.FrmChonNguyenLieu();

                        frmNguyenLieu.Dock = DockStyle.Fill;
                        frmNguyenLieu.FormBorderStyle = FormBorderStyle.None;
                        if (frmNguyenLieu.ShowDialog() == DialogResult.OK)
                        {
                            // Lấy giá trị từ form chọn nguyên liệu
                            int maNguyenLieu = frmNguyenLieu.MaNguyenLieuDaChon;
                            string tenNguyenLieu = frmNguyenLieu.TenNguyenLieuDaChon;
                            string donVi = frmNguyenLieu.DonViTinhDaChon;

                            // Gán giá trị vào textbox
                            txtDoiTuong.Text = maNguyenLieu.ToString();
                            // hiển thị thông tin nguyên liệu đã chọn trong ghi chú 
                            txtGhiChu.Text = $"Kiểm tra nguyên liệu: {tenNguyenLieu} ({donVi})";

                            //Tieu chí kiểm tra
                            txtTieuChi.Text = "Kiểm tra chất lượng nguyên liệu: " + tenNguyenLieu;
                            //hiển thị thông tin nguyên liệu đã chọn trong tiêu chí
                            MessageBox.Show($"Tiêu chí kiểm tra: Kiểm tra chất lượng nguyên liệu: {tenNguyenLieu}", "Thông báo",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        break;


                    case "MonAn":
                        FrmChonKSCL.FrmChonMonAn frmMonAn = new FrmChonKSCL.FrmChonMonAn();
                        if (frmMonAn.ShowDialog() == DialogResult.OK)
                        {
                            // lay mon an duoc chon
                            int maMonAn = frmMonAn.MaMonAnDaChon;
                            string tenMonAn = frmMonAn.TenMonAnDaChon;
                            float donVi = frmMonAn.DonGiaDaChon;
                            //gan ma mon an vao textbox
                            txtDoiTuong.Text = maMonAn.ToString();

                            // hien thi thong tin mon an da chon trong ghi chu
                            txtGhiChu.Text = $"Kiểm tra món ăn: {tenMonAn} ({donVi})";
                            // Tieu chí kiểm tra
                            txtTieuChi.Text = "Hương vị";

                            // hien thi thong tin mon an da chon trong tieu chi
                            MessageBox.Show($"Tiêu chí kiểm tra: Hương vị món ăn: {tenMonAn}", "Thông báo",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        break;
                    default:
                        MessageBox.Show("Loại kiểm tra không hợp lệ.", "Cảnh báo",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        break;
                } 
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi chọn đối tượng kiểm tra: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnChonAnh_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.bmp";
                openFileDialog.Title = "Chọn ảnh minh họa";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    selectedImagePath = openFileDialog.FileName;
                    picAnhMinhHoa.Image = Image.FromFile(selectedImagePath);
                }
            }
        }

        private void btnFilter_Click(object sender, EventArgs e)
        {
            try
            {
                lblStatus.Text = "Đang lọc dữ liệu...";
                progressBar.Visible = true;

                // TODO: Implement logic lọc dữ liệu

                progressBar.Visible = false;
                lblStatus.Text = "Đã lọc dữ liệu thành công";
            }
            catch (Exception ex)
            {
                progressBar.Visible = false;
                lblStatus.Text = "Lỗi: " + ex.Message;
                MessageBox.Show("Lỗi khi lọc dữ liệu: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            try
            {
                string tuKhoa = txtTimKiem.Text.Trim();
                if (string.IsNullOrEmpty(tuKhoa))
                {
                    LoadDanhSachKiemTra();
                    return;
                }

                lblStatus.Text = "Đang tìm kiếm...";
                progressBar.Visible = true;

                // TODO: Implement logic tìm kiếm

                progressBar.Visible = false;
                lblStatus.Text = "Đã tìm kiếm thành công";
            }
            catch (Exception ex)
            {
                progressBar.Visible = false;
                lblStatus.Text = "Lỗi: " + ex.Message;
                MessageBox.Show("Lỗi khi tìm kiếm: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnXuatBaoCao_Click(object sender, EventArgs e)
        {
            try
            {
                // Kiểm tra xem có dữ liệu để xuất không
                if (dgvBaoCao.Rows.Count == 0)
                {
                    MessageBox.Show("Không có dữ liệu để xuất báo cáo!", "Cảnh báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                using (var workbook = new XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add("ThongKeKiemTra");

                    // Thêm tiêu đề
                    worksheet.Cell(1, 1).Value = "BÁO CÁO THỐNG KÊ KIỂM TRA CHẤT LƯỢNG";
                    worksheet.Cell(1, 1).Style.Font.Bold = true;
                    worksheet.Cell(1, 1).Style.Font.FontSize = 16;
                    worksheet.Range(1, 1, 1, dgvBaoCao.Columns.Count).Merge();
                    worksheet.Cell(1, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                    // Thêm thông tin thời gian báo cáo
                    worksheet.Cell(2, 1).Value = $"Từ ngày: {dtpBaoCaoFrom.Value.ToString("dd/MM/yyyy")} - Đến ngày: {dtpBaoCaoTo.Value.ToString("dd/MM/yyyy")}";
                    worksheet.Range(2, 1, 2, dgvBaoCao.Columns.Count).Merge();
                    worksheet.Cell(2, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                    // Thêm headers từ DataGridView
                    for (int i = 0; i < dgvBaoCao.Columns.Count; i++)
                    {
                        worksheet.Cell(4, i + 1).Value = dgvBaoCao.Columns[i].HeaderText;
                        worksheet.Cell(4, i + 1).Style.Font.Bold = true;
                        worksheet.Cell(4, i + 1).Style.Fill.BackgroundColor = XLColor.LightGray;
                    }

                    // Thêm dữ liệu từ DataGridView
                    for (int i = 0; i < dgvBaoCao.Rows.Count; i++)
                    {
                        for (int j = 0; j < dgvBaoCao.Columns.Count; j++)
                        {
                            if (dgvBaoCao.Rows[i].Cells[j].Value != null)
                            {
                                worksheet.Cell(i + 5, j + 1).Value = dgvBaoCao.Rows[i].Cells[j].Value.ToString();
                            }
                        }
                    }

                    // Tự động điều chỉnh độ rộng cột
                    worksheet.Columns().AdjustToContents();

                    // Thêm viền cho bảng
                    var tableRange = worksheet.Range(4, 1, dgvBaoCao.Rows.Count + 4, dgvBaoCao.Columns.Count);
                    tableRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    tableRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;

                    // Hiển thị hộp thoại lưu file
                    SaveFileDialog saveDialog = new SaveFileDialog();
                    saveDialog.Filter = "Excel Files|*.xlsx";
                    saveDialog.Title = "Xuất báo cáo kiểm soát chất lượng";
                    saveDialog.FileName = $"BaoCaoKiemSoatChatLuong_{DateTime.Now:yyyyMMdd_HHmmss}";

                    if (saveDialog.ShowDialog() == DialogResult.OK)
                    {
                        workbook.SaveAs(saveDialog.FileName);
                        MessageBox.Show("Xuất báo cáo thành công!", "Thông báo",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }

                MessageBox.Show("Xuất báo cáo thành công!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xuất báo cáo: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void pnlBaoCao_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnBaoCaoLoc_Click(object sender, EventArgs e)
        {
            LoadBaoCaoData();
        }

        private void btnBaoCaoXuat_Click(object sender, EventArgs e)
        {
            btnXuatBaoCao_Click(sender, e);
        }
    }
}
