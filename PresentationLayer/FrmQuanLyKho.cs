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
using System.Data.SqlClient;

namespace PresentationLayer
{
    public partial class FrmQuanLyKho : Form
    {
        private PhieuNhapKho_BL phieuNhapBL;
        private ChiTietPhieuNhap_BL chiTietPhieuNhapBL;
        private NguyenVatLieu_BL nguyenVatLieuBL;
        private List<NguyenVatLieu_DTO> dsNguyenVatLieu;
        private int maPhieuNhapcurr = 0;

        public FrmQuanLyKho()
        {
            InitializeComponent();
            phieuNhapBL = new PhieuNhapKho_BL();
            chiTietPhieuNhapBL = new ChiTietPhieuNhap_BL();
            nguyenVatLieuBL = new NguyenVatLieu_BL();
        }

        private void FrmQuanLyKho_Load(object sender, EventArgs e)
        {
            // Khởi tạo combobox nguyên liệu
            LoadDanhSachNguyenLieu();

            // Khởi tạo danh sách phiếu nhập
            LoadDanhSachPhieuNhap();

            // Đặt ngày nhập mặc định là ngày hiện tại 
            dtpNgayLap.Value = DateTime.Now;

            // Xóa dữ liệu trong form
            ResetForm();

            // Vô hiệu hóa control chi tiết ban đầu
            EnableEditControls(false);
        }

        // Danh sách nguyên liệu
        private void LoadDanhSachNguyenLieu()
        {
            try
            {
                // Lấy danh sách nguyên liệu
                dsNguyenVatLieu = nguyenVatLieuBL.GetNguyenVatLieuList();

                // Push data lên combobox
                cbo_NguyenLieu.DataSource = null;
                // Thiết lập ValueMember trước khi gán DataSource
                cbo_NguyenLieu.DisplayMember = "TenNguyenLieu";
                cbo_NguyenLieu.ValueMember = "MaNguyenLieu";
                cbo_NguyenLieu.DataSource = dsNguyenVatLieu;

                // Push đơn vị tính theo nguyên liệu
                if (dsNguyenVatLieu.Count > 0)
                {
                    cbo_NguyenLieu.SelectedIndex = 0;
                    cbo_DonViTinh.Text = dsNguyenVatLieu[0].DonViTinh;
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu nguyên liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadDanhSachPhieuNhap()
        {
            try
            {
                // Bước 1: Lấy danh sách phiếu nhập từ Business Layer
                List<PhieuNhap_DTO> dsPhieuNhap = phieuNhapBL.GetPhieuNhapList();

                // Bước 2: Gán DataSource cho DataGridView
                dgv_PhieuNhap.DataSource = null; // Xóa DataSource hiện tại (nếu có)
                dgv_PhieuNhap.DataSource = dsPhieuNhap;

                // Bước 3: Định dạng các cột sau khi đã gán DataSource, với kiểm tra null
                if (dgv_PhieuNhap.Columns.Contains("MaPhieuNhap"))
                    dgv_PhieuNhap.Columns["MaPhieuNhap"].HeaderText = "Mã phiếu";

                if (dgv_PhieuNhap.Columns.Contains("MaNhanVien"))
                    dgv_PhieuNhap.Columns["MaNhanVien"].HeaderText = "Mã NV";

                if (dgv_PhieuNhap.Columns.Contains("NgayNhap"))
                {
                    dgv_PhieuNhap.Columns["NgayNhap"].HeaderText = "Ngày nhập";
                    dgv_PhieuNhap.Columns["NgayNhap"].DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";
                }

                if (dgv_PhieuNhap.Columns.Contains("NhaCungCap"))
                    dgv_PhieuNhap.Columns["NhaCungCap"].HeaderText = "Nhà cung cấp";

                if (dgv_PhieuNhap.Columns.Contains("TongTien"))
                {
                    dgv_PhieuNhap.Columns["TongTien"].HeaderText = "Tổng tiền";
                    dgv_PhieuNhap.Columns["TongTien"].DefaultCellStyle.Format = "N0";
                }

                if (dgv_PhieuNhap.Columns.Contains("GhiChu"))
                    dgv_PhieuNhap.Columns["GhiChu"].HeaderText = "Ghi chú";

                if (dgv_PhieuNhap.Columns.Contains("TrangThai"))
                    dgv_PhieuNhap.Columns["TrangThai"].HeaderText = "Trạng thái";

                // Bước 4: Thiết lập định dạng chung cho DataGridView
                dgv_PhieuNhap.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                // Tùy chọn: Thêm định dạng cho các dòng, màu nền, v.v.
                dgv_PhieuNhap.AllowUserToAddRows = false;
                dgv_PhieuNhap.AllowUserToDeleteRows = false;
                dgv_PhieuNhap.ReadOnly = true;
                dgv_PhieuNhap.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

                // Tùy chọn: Nếu có nhiều dữ liệu, hiển thị dòng đầu tiên
                if (dgv_PhieuNhap.Rows.Count > 0)
                    dgv_PhieuNhap.CurrentCell = dgv_PhieuNhap.Rows[0].Cells[0];
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Lỗi khi tải danh sách phiếu nhập: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                // Xử lý các ngoại lệ khác có thể xảy ra
                MessageBox.Show("Lỗi không xác định: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadChiTietPhieuNhap(int maPhieuNhap)
        {
            try
            {
                if (maPhieuNhap <= 0)
                    return;
                // Lấy chi tiết phiếu nhập
                List<ChiTietPhieuNhap_DTO> dsChiTiet = chiTietPhieuNhapBL.GetChiTiet_ById(maPhieuNhap);

                // Push data vào dgv 
                dgv_ChiTietPhieuNhap.DataSource = null;
                dgv_ChiTietPhieuNhap.DataSource = dsChiTiet;

                // Định dạng hiển thị các cột
                if (dgv_ChiTietPhieuNhap.Columns.Contains("MaChiTiet"))
                    dgv_ChiTietPhieuNhap.Columns["MaChiTiet"].HeaderText = "Mã chi tiết";

                if (dgv_ChiTietPhieuNhap.Columns.Contains("MaPhieuNhap"))
                    dgv_ChiTietPhieuNhap.Columns["MaPhieuNhap"].HeaderText = "Mã phiếu";

                if (dgv_ChiTietPhieuNhap.Columns.Contains("MaNguyenLieu"))
                    dgv_ChiTietPhieuNhap.Columns["MaNguyenLieu"].HeaderText = "Mã nguyên liệu";

                if (dgv_ChiTietPhieuNhap.Columns.Contains("TenNguyenLieu"))
                    dgv_ChiTietPhieuNhap.Columns["TenNguyenLieu"].HeaderText = "Tên nguyên liệu";

                if (dgv_ChiTietPhieuNhap.Columns.Contains("SoLuong"))
                    dgv_ChiTietPhieuNhap.Columns["SoLuong"].HeaderText = "Số lượng";

                if (dgv_ChiTietPhieuNhap.Columns.Contains("DonGia"))
                {
                    dgv_ChiTietPhieuNhap.Columns["DonGia"].HeaderText = "Đơn giá";
                    dgv_ChiTietPhieuNhap.Columns["DonGia"].DefaultCellStyle.Format = "N0";
                }

                if (dgv_ChiTietPhieuNhap.Columns.Contains("ThanhTien"))
                {
                    dgv_ChiTietPhieuNhap.Columns["ThanhTien"].HeaderText = "Thành tiền";
                    dgv_ChiTietPhieuNhap.Columns["ThanhTien"].DefaultCellStyle.Format = "N0";
                }

                if (dgv_ChiTietPhieuNhap.Columns.Contains("GhiChu"))
                    dgv_ChiTietPhieuNhap.Columns["GhiChu"].HeaderText = "Ghi chú";

                // Tự động điều cột
                dgv_ChiTietPhieuNhap.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                // Tính tổng tiền 
                float tongTien = 0;
                foreach (ChiTietPhieuNhap_DTO ct in dsChiTiet)
                {
                    tongTien += ct.ThanhTien;
                }

                // Hiển thị tổng tiền
                lb_TongCong.Text = tongTien.ToString("N0") + " VNĐ";
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Lỗi khi tải chi tiết phiếu nhập: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        

        private void EnableEditControls(bool enable)
        {
            // Điều khiển trạng thái các controls
            cbo_NguyenLieu.Enabled = enable;
            txtSoLuong.Enabled = enable;
            txtDonGia.Enabled = enable;
            cbo_DonViTinh.Enabled = enable;
            txtNhaCungCap.Enabled = enable;
            txtGhiChu.Enabled = enable;

            txtMaNhanVien.Enabled = true;
            btnThem.Enabled = enable;
            btnXoaPhieu.Enabled = enable;
            btnLamMoi.Enabled = enable;
            btnCapNhatPhieu.Enabled = enable;
        }

        private void ResetForm()
        {
            // Reset thông tin nhập
            txtMaPhieuNhap.Text = "";
            txtMaNhanVien.Text = "";
            txtNhaCungCap.Text = ""; // Đảm bảo xóa trắng trường nhà cung cấp
            txtGhiChu.Text = "";
            txtSoLuong.Text = "0";
            txtDonGia.Text = "0";
            dtpNgayLap.Value = DateTime.Now;

            // Reset thông tin chi tiết phiếu nhập
            dgv_ChiTietPhieuNhap.DataSource = null;
            lb_TongCong.Text = "0 VNĐ";

            // Reset mã phiếu hiện tại
            maPhieuNhapcurr = 0;
        }

        private void cbo_NguyenLieu_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Chọn nguyên liệu hiển thị đơn vị tính tương ứng 
            if (cbo_NguyenLieu.SelectedIndex >= 0 && dsNguyenVatLieu != null && dsNguyenVatLieu.Count > 0)
            {
                int maNguyenLieu = Convert.ToInt32(cbo_NguyenLieu.SelectedValue);
                foreach (NguyenVatLieu_DTO nvl in dsNguyenVatLieu)
                {
                    if (nvl.MaNguyenLieu == maNguyenLieu)
                    {
                        cbo_DonViTinh.Text = nvl.DonViTinh;
                        break;
                    }
                }
            }
        }

        // Kiểm tra nhân viên tồn tại
        private bool KiemTraNhanVienTonTai(int maNhanVien)
        {
            try
            {
                // Gọi phương thức từ Business Layer
                return phieuNhapBL.KiemTraNhanVienTonTai(maNhanVien);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi kiểm tra nhân viên: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
        private void btnThemPhieuNhap_Click(object sender, EventArgs e)
        {
            try
            {
                // Lấy và kiểm tra mã nhân viên từ textbox
                int maNhanVien;
                if (!int.TryParse(txtMaNhanVien.Text.Trim(), out maNhanVien) || maNhanVien <= 0)
                {
                    MessageBox.Show("Mã nhân viên phải là số nguyên dương.", "Cảnh báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtMaNhanVien.Focus();
                    return;
                }

                // Kiểm tra mã nhân viên có tồn tại trong CSDL không
                if (!KiemTraNhanVienTonTai(maNhanVien))
                {
                    MessageBox.Show("Mã nhân viên không tồn tại trong hệ thống.", "Cảnh báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtMaNhanVien.Focus();
                    return;
                }

                // Lấy các thông tin khác
                string nhaCungCap = txtNhaCungCap.Text.Trim();
                string ghiChu = txtGhiChu.Text.Trim();

                // Tạo đối tượng phiếu nhập mới
                PhieuNhap_DTO phieuNhap = new PhieuNhap_DTO();
                phieuNhap.MaNhanVien = maNhanVien; // Sử dụng mã nhân viên từ input
                phieuNhap.NgayNhap = dtpNgayLap.Value;
                phieuNhap.NhaCungCap = nhaCungCap;
                phieuNhap.GhiChu = ghiChu;
                phieuNhap.TrangThai = true;

                // Thêm phiếu nhập và lấy mã phiếu nhập mới 
                maPhieuNhapcurr = phieuNhapBL.CreatePhieu(phieuNhap);
                if (maPhieuNhapcurr > 0)
                {
                    MessageBox.Show("Tạo phiếu thành công. Mã phiếu nhập: " + maPhieuNhapcurr,
                        "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Hiển thị mã phiếu nhập và mã nhân viên
                    txtMaPhieuNhap.Text = maPhieuNhapcurr.ToString();
                    txtMaNhanVien.Text = maNhanVien.ToString(); // Hiển thị lại mã nhân viên đã nhập

                    // Hiển thị thông tin nhà cung cấp và ghi chú
                    txtNhaCungCap.Text = nhaCungCap;
                    txtGhiChu.Text = ghiChu;

                    // Enable các control
                    EnableEditControls(true);

                    // Load lại danh sách phiếu nhập 
                    LoadDanhSachPhieuNhap();
                }
                else
                {
                    MessageBox.Show("Không thể tạo phiếu nhập. Vui lòng thử lại.", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Lỗi SQL khi tạo phiếu nhập: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tạo phiếu nhập: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Phương thức chọn dòng trong DataGridView theo mã phiếu nhập
        private void SelectRowByPhieuNhapId(int maPhieuNhap)
        {
            try
            {
                foreach (DataGridViewRow row in dgv_PhieuNhap.Rows)
                {
                    if (Convert.ToInt32(row.Cells["MaPhieuNhap"].Value) == maPhieuNhap)
                    {
                        dgv_PhieuNhap.CurrentCell = row.Cells[0];
                        dgv_PhieuNhap.Rows[row.Index].Selected = true;
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
                // Kiểm tra mã phiếu hiện tại
                if (maPhieuNhapcurr <= 0)
                {
                    MessageBox.Show("Vui lòng tạo phiếu nhập trước khi thêm.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Kiểm tra mã nhân viên tồn tại trước khi thực hiện
                int maNhanVien;
                if (!int.TryParse(txtMaNhanVien.Text.Trim(), out maNhanVien) || maNhanVien <= 0)
                {
                    MessageBox.Show("Mã nhân viên phải là số nguyên dương.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtMaNhanVien.Focus();
                    return;
                }

                // Thêm kiểm tra nhân viên tồn tại
                if (!KiemTraNhanVienTonTai(maNhanVien))
                {
                    MessageBox.Show("Mã nhân viên không tồn tại trong hệ thống.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtMaNhanVien.Focus();
                    return;
                }
                // Kiểm tra dữ liệu nhập
                if (cbo_NguyenLieu.SelectedIndex < 0)
                {
                    MessageBox.Show("Vui lòng chọn nguyên liệu.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    cbo_NguyenLieu.Focus();
                    return;
                }

                float soLuong;
                if (!float.TryParse(txtSoLuong.Text.Trim(), out soLuong) || soLuong <= 0)
                {
                    MessageBox.Show("Số lượng phải là số dương.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtSoLuong.Focus();
                    return;
                }

                float donGia;
                if (!float.TryParse(txtDonGia.Text.Trim(), out donGia) || donGia < 0)
                {
                    MessageBox.Show("Đơn giá phải là số không âm.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtDonGia.Focus();
                    return;
                }

                // Tạo đối tượng chi tiết phiếu nhập
                ChiTietPhieuNhap_DTO chiTiet = new ChiTietPhieuNhap_DTO();
                chiTiet.MaPhieuNhap = maPhieuNhapcurr;
                chiTiet.MaNguyenLieu = Convert.ToInt32(cbo_NguyenLieu.SelectedValue);
                chiTiet.SoLuong = soLuong;
                chiTiet.DonGia = donGia;
                chiTiet.GhiChu = txtGhiChu.Text.Trim();

                // Thêm chi tiết phiếu nhập
                bool result = chiTietPhieuNhapBL.AddChiTietPhieu(chiTiet);
                if (result)
                {
                    // Đồng thời cập nhật thông tin phiếu nhập (nhà cung cấp và ghi chú)
                    PhieuNhap_DTO phieuNhap = new PhieuNhap_DTO();
                    phieuNhap.MaPhieuNhap = maPhieuNhapcurr;
                    phieuNhap.NhaCungCap = txtNhaCungCap.Text.Trim();
                    phieuNhap.GhiChu = txtGhiChu.Text.Trim();

                    // Cập nhật phiếu nhập
                    phieuNhapBL.UpdatePhieuNhap(phieuNhap);

                    MessageBox.Show("Thêm chi tiết phiếu nhập thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Load lại danh sách phiếu nhập để cập nhật tổng tiền và thông tin
                    LoadDanhSachPhieuNhap();

                    // Load lại chi tiết phiếu nhập
                    LoadChiTietPhieuNhap(maPhieuNhapcurr);

                    // Reset số lượng và đơn giá
                    txtSoLuong.Text = "0";
                    txtDonGia.Text = "0";

                    // Chọn nguyên liệu đầu tiên
                    if (cbo_NguyenLieu.Items.Count > 0)
                        cbo_NguyenLieu.SelectedIndex = 0;
                }
                else
                {
                    MessageBox.Show("Không thể thêm chi tiết phiếu nhập.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Lỗi SQL khi thêm chi tiết phiếu nhập: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi thêm chi tiết phiếu nhập: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Thêm button để reset IDENTITY hoặc tự động reset khi xóa phiếu cuối cùng
        private void ResetIdentityAfterDeleteAll()
        {
            try
            {
                // Kiểm tra xem có còn phiếu nào không
                List<PhieuNhap_DTO> dsPhieuNhap = phieuNhapBL.GetPhieuNhapList();
                if (dsPhieuNhap.Count == 0)
                {
                    // Nếu không còn phiếu nào, reset IDENTITY
                    phieuNhapBL.ResetIdentity();
                    MessageBox.Show("Đã reset ID cho các bảng.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi reset ID: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnXoaPhieu_Click(object sender, EventArgs e)
        {
            try
            {
                // Kiểm tra mã phiếu hiện tại
                if (maPhieuNhapcurr <= 0)
                {
                    MessageBox.Show("Vui lòng chọn phiếu cần xóa.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Xác nhận xóa
                DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn xóa phiếu này? Dữ liệu sẽ bị mất vĩnh viễn.", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    // Xóa phiếu nhập
                    bool success = phieuNhapBL.DeletePhieuNhap(maPhieuNhapcurr);
                    if (success)
                    {
                        MessageBox.Show("Xóa phiếu thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // Gọi phương thức ResetIdentityAfterDeleteAll để kiểm tra và reset ID nếu cần
                        ResetIdentityAfterDeleteAll();

                        // Tải lại danh sách phiếu nhập
                        LoadDanhSachPhieuNhap();

                        // Reset form
                        ResetForm();

                        // Vô hiệu hóa các controls
                        EnableEditControls(false);
                    }
                    else
                    {
                        MessageBox.Show("Không thể xóa phiếu nhập.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Lỗi khi xóa phiếu nhập: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            // Reset thông tin nhập liệu
            txtSoLuong.Text = "0";
            txtDonGia.Text = "0";

            // Reset lại combobox nguyên liệu
            if (cbo_NguyenLieu.Items.Count > 0)
                cbo_NguyenLieu.SelectedIndex = 0;

            // Focus vào combobox nguyên liệu
            cbo_NguyenLieu.Focus();
        }

        private void dgv_PhieuNhap_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    // Lấy thông tin phiếu được chọn
                    DataGridViewRow row = dgv_PhieuNhap.Rows[e.RowIndex];
                    maPhieuNhapcurr = Convert.ToInt32(row.Cells["MaPhieuNhap"].Value);

                    // Hiển thị thông tin phiếu nhập
                    txtMaPhieuNhap.Text = maPhieuNhapcurr.ToString();
                    txtMaNhanVien.Text = row.Cells["MaNhanVien"].Value.ToString();
                    dtpNgayLap.Value = Convert.ToDateTime(row.Cells["NgayNhap"].Value);

                    // Đảm bảo xử lý đúng giá trị NULL cho NhaCungCap
                    if (row.Cells["NhaCungCap"].Value != null && row.Cells["NhaCungCap"].Value != DBNull.Value)
                        txtNhaCungCap.Text = row.Cells["NhaCungCap"].Value.ToString();
                    else
                        txtNhaCungCap.Text = "";

                    // Đảm bảo xử lý đúng giá trị NULL cho GhiChu
                    if (row.Cells["GhiChu"].Value != null && row.Cells["GhiChu"].Value != DBNull.Value)
                        txtGhiChu.Text = row.Cells["GhiChu"].Value.ToString();
                    else
                        txtGhiChu.Text = "";

                    // Load chi tiết phiếu nhập
                    LoadChiTietPhieuNhap(maPhieuNhapcurr);

                    // Kiểm tra trạng thái phiếu nhập
                    bool trangThai = Convert.ToBoolean(row.Cells["TrangThai"].Value);
                    EnableEditControls(trangThai);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi chọn phiếu nhập: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Sửa kết nối sự kiện từ dgv_PhieuNhap_CellContentClick sang dgv_PhieuNhap_CellClick
        private void dgv_PhieuNhap_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Gọi phương thức CellClick để xử lý thống nhất
            dgv_PhieuNhap_CellClick(sender, e);
        }

        private void btnCapNhatPhieu_Click_1(object sender, EventArgs e)
        {
            try
            {
                // Kiểm tra mã phiếu hiện tại
                if (maPhieuNhapcurr <= 0)
                {
                    MessageBox.Show("Vui lòng chọn phiếu cần cập nhật.", "Cảnh báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Lấy và kiểm tra mã nhân viên
                int maNhanVien;
                if (!int.TryParse(txtMaNhanVien.Text.Trim(), out maNhanVien) || maNhanVien <= 0)
                {
                    MessageBox.Show("Mã nhân viên phải là số nguyên dương.", "Cảnh báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtMaNhanVien.Focus();
                    return;
                }

                // Kiểm tra mã nhân viên có tồn tại không
                if (!KiemTraNhanVienTonTai(maNhanVien))
                {
                    MessageBox.Show("Mã nhân viên không tồn tại trong hệ thống.", "Cảnh báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtMaNhanVien.Focus();
                    return;
                }

                // 1. Cập nhật thông tin phiếu nhập
                PhieuNhap_DTO phieuNhap = new PhieuNhap_DTO();
                phieuNhap.MaPhieuNhap = maPhieuNhapcurr;
                phieuNhap.NgayNhap = dtpNgayLap.Value;
                phieuNhap.NhaCungCap = txtNhaCungCap.Text.Trim();
                phieuNhap.GhiChu = txtGhiChu.Text.Trim();

                // Gọi phương thức cập nhật phiếu nhập
                bool resultPhieuNhap = phieuNhapBL.UpdatePhieuNhap(phieuNhap);

                // 2. Cập nhật chi tiết phiếu nhập (nếu có dữ liệu mới từ GroupBox "Thông tin hàng hóa")
                // Kiểm tra xem có dữ liệu mới để cập nhật chi tiết không
                if (cbo_NguyenLieu.SelectedIndex >= 0)
                {
                    float soLuong;
                    if (!float.TryParse(txtSoLuong.Text.Trim(), out soLuong) || soLuong <= 0)
                    {
                        MessageBox.Show("Số lượng phải là số dương.", "Cảnh báo",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtSoLuong.Focus();
                        return;
                    }

                    float donGia;
                    if (!float.TryParse(txtDonGia.Text.Trim(), out donGia) || donGia < 0)
                    {
                        MessageBox.Show("Đơn giá phải là số không âm.", "Cảnh báo",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtDonGia.Focus();
                        return;
                    }

                    // Tìm chi tiết đang được chọn (nếu có)
                    int maChiTiet = 0;

                    // Kiểm tra xem có dòng nào được chọn trong bảng chi tiết không
                    if (dgv_ChiTietPhieuNhap.SelectedRows.Count > 0)
                    {
                        DataGridViewRow selectedRow = dgv_ChiTietPhieuNhap.SelectedRows[0];
                        maChiTiet = Convert.ToInt32(selectedRow.Cells["MaChiTiet"].Value);
                    }

                    // Nếu có mã chi tiết (đang chọn một dòng), thực hiện cập nhật chi tiết đó
                    if (maChiTiet > 0)
                    {
                        ChiTietPhieuNhap_DTO chiTiet = new ChiTietPhieuNhap_DTO();
                        chiTiet.MaChiTiet = maChiTiet;
                        chiTiet.MaPhieuNhap = maPhieuNhapcurr;
                        chiTiet.MaNguyenLieu = Convert.ToInt32(cbo_NguyenLieu.SelectedValue);
                        chiTiet.SoLuong = soLuong;
                        chiTiet.DonGia = donGia;
                        chiTiet.GhiChu = txtGhiChu.Text.Trim();

                        // Bạn cần thêm phương thức UpdateChiTietPhieuNhap trong lớp ChiTietPhieuNhap_BL
                        bool resultChiTiet = chiTietPhieuNhapBL.UpdateChiTietPhieu(chiTiet);

                        if (resultChiTiet)
                        {
                            MessageBox.Show("Cập nhật chi tiết phiếu nhập thành công.", "Thông báo",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("Không thể cập nhật chi tiết phiếu nhập.", "Lỗi",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else // Nếu không có chi tiết nào được chọn, thêm mới chi tiết
                    {
                        ChiTietPhieuNhap_DTO chiTiet = new ChiTietPhieuNhap_DTO();
                        chiTiet.MaPhieuNhap = maPhieuNhapcurr;
                        chiTiet.MaNguyenLieu = Convert.ToInt32(cbo_NguyenLieu.SelectedValue);
                        chiTiet.SoLuong = soLuong;
                        chiTiet.DonGia = donGia;
                        chiTiet.GhiChu = txtGhiChu.Text.Trim();

                        bool resultChiTiet = chiTietPhieuNhapBL.AddChiTietPhieu(chiTiet);

                        if (resultChiTiet)
                        {
                            MessageBox.Show("Thêm chi tiết phiếu nhập thành công.", "Thông báo",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("Không thể thêm chi tiết phiếu nhập.", "Lỗi",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }

                // Hiển thị thông báo cập nhật thành công
                if (resultPhieuNhap)
                {
                    MessageBox.Show("Cập nhật phiếu nhập thành công.", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Đảm bảo load lại dữ liệu sau khi cập nhật
                    LoadDanhSachPhieuNhap();

                    // Chọn lại phiếu nhập vừa cập nhật để hiển thị thông tin
                    SelectRowByPhieuNhapId(maPhieuNhapcurr);

                    // Load lại chi tiết phiếu nhập
                    LoadChiTietPhieuNhap(maPhieuNhapcurr);
                }
                else
                {
                    MessageBox.Show("Không thể cập nhật phiếu nhập.", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Lỗi SQL khi cập nhật phiếu nhập: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi cập nhật phiếu nhập: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgv_ChiTietPhieuNhap_CellClick(object sender, DataGridViewCellEventArgs e)
        {
             try
             {
                if (e.RowIndex >= 0)
                {
                    // Lấy thông tin chi tiết phiếu được chọn
                    DataGridViewRow row = dgv_ChiTietPhieuNhap.Rows[e.RowIndex];
            
                    // Xác định mã nguyên liệu
                    int maNguyenLieu = Convert.ToInt32(row.Cells["MaNguyenLieu"].Value);
            
                    // Hiển thị dữ liệu lên các textbox
                    txtSoLuong.Text = row.Cells["SoLuong"].Value.ToString();
                    txtDonGia.Text = row.Cells["DonGia"].Value.ToString();
            
                    // Chọn nguyên liệu tương ứng trong combobox
                    for (int i = 0; i < cbo_NguyenLieu.Items.Count; i++)
                    {
                        NguyenVatLieu_DTO nvl = (NguyenVatLieu_DTO)cbo_NguyenLieu.Items[i];
                        if (nvl.MaNguyenLieu == maNguyenLieu)
                        {
                            cbo_NguyenLieu.SelectedIndex = i;
                            break;
                        }
                    }
                }
             }
             catch (Exception ex)
             {
                MessageBox.Show("Lỗi khi chọn chi tiết phiếu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
             }
        }
    }
}