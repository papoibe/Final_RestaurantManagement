using BusinessLayer;
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
using TransferObject;
using PresentationLayer.Helpers;

namespace PresentationLayer
{
    public partial class FrmQuanLyHoaDon : Form
    {
        private List<DonHang_DTO> listDonHang;
        private DonHang_BL DonHang_BL = new DonHang_BL();
        private List<ChiTietDonHang_DTO> listChiTietDonHang;
        private ThanhToan_BL thanhToan_BL = new ThanhToan_BL();
        private MonAn_BL monAnBL = new MonAn_BL();
        private Ban_BL ban_BL = new Ban_BL();
        private HoaDonThanhToan_BL hoaDonThanhToan_BL = new HoaDonThanhToan_BL();
        public FrmQuanLyHoaDon()
        {
            InitializeComponent();
        }

        private void btn_close_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FrmQuanLyHoaDon_Load(object sender, EventArgs e)
        {
            InitFilterControls();
            //LoadHoaDonByList(thanhToan_BL.GetHD());

        }
        /// <summary>
        /// Khởi tạo các điều khiển lọc (DateTimePicker, ComboBox bàn, trạng thái)
        /// </summary>
        private void InitFilterControls()
        {
            // Thiết lập DateTimePicker
            dateTimePicker.Value = DateTime.Now;

            // Thiết lập ComboBox bàn
            SetupTableComboBox();

            // Thiết lập ComboBox trạng thái
            SetupStatusComboBox();
        }

        /// <summary>
        /// Thiết lập ComboBox bàn với danh sách bàn từ ban_BL
        /// </summary>
        private void SetupTableComboBox()
        {
            cboTableFilter.Items.Clear();
            cboTableFilter.Items.Add(new { Text = "Tất cả", Value = 0 });

            List<Ban_DTO> tables = ban_BL.GetAll();
            foreach (Ban_DTO table in tables)
            {
                cboTableFilter.Items.Add(new { Text = table.TenBan, Value = table.MaBan });
            }

            cboTableFilter.DisplayMember = "Text";
            cboTableFilter.ValueMember = "Value";
            cboTableFilter.SelectedIndex = 0;
        }

        /// <summary>
        /// Thiết lập ComboBox trạng thái với các giá trị trạng thái cố định
        /// </summary>
        private void SetupStatusComboBox()
        {
            cboStatusFilter.Items.Clear();
            cboStatusFilter.Items.Add(new { Text = "Tất cả", Value = -1 }); // -1 để biểu thị "Tất cả"
            cboStatusFilter.Items.Add(new { Text = "Chưa thanh toán", Value = 0 });
            cboStatusFilter.Items.Add(new { Text = "Đã thanh toán", Value = 4 });
            cboStatusFilter.Items.Add(new { Text = "Đã hủy", Value = 5 });

            cboStatusFilter.DisplayMember = "Text";
            cboStatusFilter.ValueMember = "Value";
            cboStatusFilter.SelectedIndex = 0;
        }

        /// <summary>
        /// Xử lý sự kiện in hóa đơn (tích hợp ứng dụng bên thứ 3)
        /// </summary>
        private void btn_InHD_Click(object sender, EventArgs e)
        {
            try
            {
                // Kiểm tra xem có hóa đơn được chọn không
                if (string.IsNullOrEmpty(lb_MD.Text))
                {
                    MessageBox.Show("Vui lòng chọn hóa đơn cần in!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Lấy mã đơn hàng
                int maDonHang = Convert.ToInt32(lb_MD.Text);


                // Lấy thông tin hóa đơn
                var hoaDon = thanhToan_BL.GetHD().FirstOrDefault(h => h.MaDonHang == maDonHang);
                if (hoaDon == null)
                {
                    MessageBox.Show("Không tìm thấy thông tin hóa đơn!", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Tạo đối tượng PrintHelper và in hóa đơn
                PrintHelper printHelper = new PrintHelper(hoaDon, dgv_chiTiet);
                printHelper.Print();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi in hóa đơn: {ex.Message}", "Lỗi",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Xử lý sự kiện thay đổi ngày trên DateTimePicker
        /// </summary>
        private void dateTimePicker_ValueChanged(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Xử lý sự kiện tìm kiếm khi nhấn nút Search
        /// </summary>
        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                // Lấy tham số tìm kiếm
                DateTime? time = dateTimePicker.Value.Date;
                string nameBan = null;
                if (cboTableFilter.SelectedIndex > 0)
                {
                    nameBan = cboTableFilter.Text.Trim();
                }
                string status = null;
                if (cboStatusFilter.SelectedIndex >= 0 && cboStatusFilter.SelectedValue != null)
                {
                    var selectedStatus = cboStatusFilter.SelectedValue.ToString();
                    if (selectedStatus != "-1")
                    {
                        status = selectedStatus;
                    }
                }
                // Lấy tất cả hóa đơn
                var allHoaDon = hoaDonThanhToan_BL.GetAll();

                // Nếu nhập mã đơn hàng thì ưu tiên tìm theo mã đơn hàng
                if (!string.IsNullOrWhiteSpace(txtSearch.Text))
                {
                    int maDon;
                    if (int.TryParse(txtSearch.Text.Trim(), out maDon))
                    {
                        var ketQua = allHoaDon.Where(hd => hd.MaDonHang == maDon).ToList();
                        if (ketQua.Count == 0)
                        {
                            MessageBox.Show("Không tìm thấy hóa đơn với mã đơn này!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        LoadHoaDonByList(ketQua);
                        return;
                    }
                    else
                    {
                        MessageBox.Show("Vui lòng nhập mã đơn hợp lệ!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }


                // Lọc theo các tiêu chí
                var filtered = allHoaDon;
                if (!string.IsNullOrEmpty(nameBan))
                {
                    filtered = filtered.FindAll(hd => hd.TenBan == nameBan);
                }
                if (time.HasValue)
                {
                    filtered = filtered.FindAll(hd => hd.NgayThanhToan.HasValue && hd.NgayThanhToan.Value.Date == time.Value.Date);
                }
                if (!string.IsNullOrEmpty(status))
                {
                    string statusText = status == "4" ? "Đã thanh toán" : status == "0" ? "Chưa thanh toán" : "Đã hủy";
                    filtered = filtered.FindAll(hd => hd.TrangThai == statusText);
                }
                // Hiển thị kết quả
                LoadHoaDonByList(filtered);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tìm kiếm hóa đơn: {ex.Message}", "Lỗi",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void LoadHoaDonByList_DonHangThanhToan(List<HoaDonTT_DTO> danhSachHoaDon = null)
        {
            try
            {
                fLP_HD.Controls.Clear();
                if (danhSachHoaDon == null)
                {
                    danhSachHoaDon = thanhToan_BL.GetHD();
                }
                foreach (var hd in danhSachHoaDon)
                {
                    ucHoaDon uc = new ucHoaDon(hd);
                    uc.Click += (s, e2) =>
                    {
                        if (s is ucHoaDon clickedUc)
                        {
                            var hoaDon = clickedUc.HoaDon;
                            if (hoaDon is HoaDonTT_DTO hoaDonTT)
                            {
                                var donhang = DonHang_BL.GetById(hoaDonTT.MaDonHang);
                                lb_GiamGia.Text = donhang.GiamGia.ToString();
                                lb_Tong.Text = donhang.TongTien.ToString();
                                lb_MD.Text = donhang.MaDonHang.ToString();
                                switch ((int)donhang.MaTrangThai)
                                {
                                    case 1:
                                    case 2:
                                    case 3:
                                        lb_TrangThai.Text = "Chưa thanh toán";
                                        break;
                                    case 4:
                                        lb_PPTT.Text = hoaDonTT.TenHinhThuc;
                                        lb_TrangThai.Text = "Đã thanh toán";
                                        break;
                                    case 5:
                                        lb_TrangThai.Text = "Đã hủy";
                                        break;
                                    default:
                                        lb_TrangThai.Text = "Không xác định";
                                        break;
                                }
                                var chiTiet = monAnBL.GetMonAnByMaDH(hoaDonTT.MaDonHang);
                                dgv_chiTiet.Rows.Clear();
                                foreach (var monAn in chiTiet)
                                {
                                    dgv_chiTiet.Rows.Add(monAn.MaChiTiet, monAn.SoLuong, monAn.TenMonAn, monAn.ThanhTien);
                                }
                            }
                        }
                    };
                    fLP_HD.Controls.Add(uc);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải danh sách hóa đơn: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }





        private void LoadHoaDonByList(List<HoaDonThanhToan_DTO> danhSachHoaDon = null)
        {
            try
            {
                fLP_HD.Controls.Clear();
                foreach (var hd in danhSachHoaDon)
                {
                    ucHoaDon uc = new ucHoaDon(hd);
                    uc.Click += (s, e2) =>
                    {
                        if (s is ucHoaDon clickedUc)
                        {
                            var hoaDon = clickedUc.HoaDon;
                            if (hoaDon is HoaDonTT_DTO hoaDonTT)
                            {
                                var donhang = DonHang_BL.GetById(hoaDonTT.MaDonHang);
                                lb_GiamGia.Text = donhang.GiamGia.ToString();
                                lb_Tong.Text = donhang.TongTien.ToString();
                                lb_MD.Text = donhang.MaDonHang.ToString();
                                switch ((int)donhang.MaTrangThai)
                                {
                                    case 1:
                                    case 2:
                                    case 3:
                                        lb_TrangThai.Text = "Chưa thanh toán";
                                        break;
                                    case 4:
                                        lb_PPTT.Text = hoaDonTT.TenHinhThuc;
                                        lb_TrangThai.Text = "Đã thanh toán";
                                        break;
                                    case 5:
                                        lb_TrangThai.Text = "Đã hủy";
                                        break;
                                    default:
                                        lb_TrangThai.Text = "Không xác định";
                                        break;
                                }
                                var chiTiet = monAnBL.GetMonAnByMaDH(hoaDonTT.MaDonHang);
                                dgv_chiTiet.Rows.Clear();
                                foreach (var monAn in chiTiet)
                                {
                                    dgv_chiTiet.Rows.Add(monAn.MaChiTiet, monAn.SoLuong, monAn.TenMonAn, monAn.ThanhTien);
                                }
                            }
                            else if (hoaDon is HoaDonThanhToan_DTO hoaDonTTT)
                            {
                                var donhang = DonHang_BL.GetById(hoaDonTTT.MaDonHang);
                                lb_GiamGia.Text = donhang.GiamGia.ToString();
                                lb_Tong.Text = donhang.TongTien.ToString();
                                lb_MD.Text = donhang.MaDonHang.ToString();
                                switch ((int)donhang.MaTrangThai)
                                {
                                    case 1:
                                    case 2:
                                    case 3:
                                        lb_TrangThai.Text = "Chưa thanh toán";
                                        break;
                                    case 4:
                                        lb_PPTT.Text = hoaDonTTT.MaHinhThuc.HasValue ? hoaDonTTT.MaHinhThuc.Value.ToString() : "";
                                        lb_TrangThai.Text = "Đã thanh toán";
                                        break;
                                    case 5:
                                        lb_TrangThai.Text = "Đã hủy";
                                        break;
                                    default:
                                        lb_TrangThai.Text = "Không xác định";
                                        break;
                                }
                                var chiTiet = monAnBL.GetMonAnByMaDH(hoaDonTTT.MaDonHang);
                                dgv_chiTiet.Rows.Clear();
                                foreach (var monAn in chiTiet)
                                {
                                    dgv_chiTiet.Rows.Add(monAn.MaChiTiet, monAn.SoLuong, monAn.TenMonAn, monAn.ThanhTien);
                                }
                            }
                        }
                    };
                    fLP_HD.Controls.Add(uc);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải danh sách hóa đơn: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cboTableFilter_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btnChiTietHoaDon_Click(object sender, EventArgs e)
        {
            // Lấy danh sách từ HoaDonThanhToan
            List<HoaDonThanhToan_DTO> danhSachChiTiet = hoaDonThanhToan_BL.GetAll();
            LoadHoaDonByList(danhSachChiTiet);
        }

        private void btnTatCaHoaDon_Click(object sender, EventArgs e)
        {
            LoadHoaDonByList_DonHangThanhToan();

        }

        private void fLP_HD_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
