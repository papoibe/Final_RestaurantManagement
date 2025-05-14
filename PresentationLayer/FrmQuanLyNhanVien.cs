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
using TransferObject;

namespace PresentationLayer
{
    public partial class FrmQuanLyNhanVien : Form
    {
        private NhanVien_BL nhanVienBL = new NhanVien_BL();
        private int selectedMaNV = -1, selectedMaTK = -1;

        public FrmQuanLyNhanVien()
        {
            InitializeComponent();
            dgv1.CellClick += dgv1_CellClick;
            Load += FrmQuanLyNhanVien_Load;
        }

        private void FrmQuanLyNhanVien_Load(object sender, EventArgs e)
        {
            LoadNhanVienToGrid();
        }

        private void LoadNhanVienToGrid()
        {
            try
            {
                dgv1.DataSource = nhanVienBL.LayDanhSachNhanVien();
                dgv1.Columns["MaNhanVien"].HeaderText = "Mã NV";
                dgv1.Columns["HoTen"].HeaderText = "Họ tên";
                dgv1.Columns["GioiTinh"].HeaderText = "Giới tính";
                dgv1.Columns["NgaySinh"].HeaderText = "Ngày sinh";
                dgv1.Columns["DiaChi"].HeaderText = "Địa chỉ";
                dgv1.Columns["SDT"].HeaderText = "SĐT";
                dgv1.Columns["Email"].HeaderText = "Email";
                dgv1.Columns["NgayVaoLam"].HeaderText = "Ngày vào làm";
                dgv1.Columns["TrangThai"].Visible = false;
                dgv1.Columns["MaTaiKhoan"].Visible = false;
                dgv1.Columns["Username"].HeaderText = "Tên đăng nhập";
                dgv1.Columns["Password"].HeaderText = "Mật khẩu";
                dgv1.Columns["DisplayName"].HeaderText = "Tên hiển thị";
                dgv1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu: " + ex.Message);
            }
        }

        private void dgv1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var row = dgv1.Rows[e.RowIndex];
            selectedMaNV = Convert.ToInt32(row.Cells["MaNhanVien"].Value);
            selectedMaTK = Convert.ToInt32(row.Cells["MaTaiKhoan"].Value);
            txtTaiKhoan.Text = row.Cells["Username"].Value?.ToString();
            txtMatKhau.Text = row.Cells["Password"].Value?.ToString();
            txtTenHienThi.Text = row.Cells["DisplayName"].Value?.ToString();
            txtTen.Text = row.Cells["HoTen"].Value?.ToString();
            cbGioiTinh.Text = row.Cells["GioiTinh"].Value?.ToString();
            txtDiaChi.Text = row.Cells["DiaChi"].Value?.ToString();
            txtSDT.Text = row.Cells["SDT"].Value?.ToString();
            txtEmail.Text = row.Cells["Email"].Value?.ToString();
            DTPBirth.Value = Convert.ToDateTime(row.Cells["NgaySinh"].Value);
            DTPIn.Value = Convert.ToDateTime(row.Cells["NgayVaoLam"].Value);
        }

 

        private void ClearInputs()
        {
            txtTaiKhoan.Text = txtMatKhau.Text = txtTenHienThi.Text = txtTen.Text = txtDiaChi.Text = txtSDT.Text = txtEmail.Text = "";
            cbGioiTinh.SelectedIndex = -1;
            DTPBirth.Value = DTPIn.Value = DateTime.Now;
            selectedMaNV = selectedMaTK = -1;
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(txtTaiKhoan.Text) || string.IsNullOrWhiteSpace(txtMatKhau.Text) ||
                string.IsNullOrWhiteSpace(txtTenHienThi.Text) || string.IsNullOrWhiteSpace(txtTen.Text))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin bắt buộc!");
                return false;
            }
            return true;
        }

        

        

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (selectedMaNV == -1 || selectedMaTK == -1)
            {
                MessageBox.Show("Hãy chọn một nhân viên để xóa.");
                return;
            }
            if (MessageBox.Show("Bạn có chắc muốn xóa?", "Xác nhận", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                if (nhanVienBL.XoaNhanVien(selectedMaNV, selectedMaTK))
                {
                    MessageBox.Show("Đã xóa thành công.");
                    LoadNhanVienToGrid();
                    ClearInputs();
                }
                else
                    MessageBox.Show("Xóa thất bại.");
            }
        }

        private void btnCapNhat_Click(object sender, EventArgs e)
        {
            if (selectedMaNV == -1 || selectedMaTK == -1)
            {
                MessageBox.Show("Hãy chọn một nhân viên để cập nhật.");
                return;
            }
            if (!ValidateInput()) return;
            var nv = new NhanVien_DTO
            {
                MaNhanVien = selectedMaNV,
                MaTaiKhoan = selectedMaTK,
                Username = txtTaiKhoan.Text,
                Password = txtMatKhau.Text,
                DisplayName = txtTenHienThi.Text,
                HoTen = txtTen.Text,
                GioiTinh = cbGioiTinh.Text,
                NgaySinh = DTPBirth.Value,
                DiaChi = txtDiaChi.Text,
                SDT = txtSDT.Text,
                Email = txtEmail.Text,
                NgayVaoLam = DTPIn.Value,
                TrangThai = true
            };
            if (nhanVienBL.CapNhatNhanVien(nv))
            {
                MessageBox.Show("Cập nhật thành công!");
                LoadNhanVienToGrid();
            }
            else
                MessageBox.Show("Cập nhật thất bại.");
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            if (!ValidateInput()) return;
            var nv = new NhanVien_DTO
            {
                Username = txtTaiKhoan.Text,
                Password = txtMatKhau.Text,
                DisplayName = txtTenHienThi.Text,
                HoTen = txtTen.Text,
                GioiTinh = cbGioiTinh.Text,
                NgaySinh = DTPBirth.Value,
                DiaChi = txtDiaChi.Text,
                SDT = txtSDT.Text,
                Email = txtEmail.Text,
                NgayVaoLam = DTPIn.Value,
                TrangThai = true
            };

            try
            {
                if (nhanVienBL.ThemNhanVien(nv))
                {
                    MessageBox.Show("Thêm nhân viên thành công!");
                    LoadNhanVienToGrid();
                    ClearInputs();
                }
                else
                    MessageBox.Show("Thêm thất bại. Hãy kiểm tra lại dữ liệu đầu vào hoặc xem file log lỗi.");
            }
            catch (Exception ex)
            {
                // Ghi log lỗi chi tiết ra file
                System.IO.File.AppendAllText("error_log.txt", DateTime.Now + " - Lỗi thêm nhân viên: " + ex.Message + "\n" + ex.StackTrace + "\n");
                MessageBox.Show("Lỗi: " + ex.Message + (ex.InnerException != null ? "\nChi tiết: " + ex.InnerException.Message : "") + "\nStackTrace: " + ex.StackTrace);
            }

        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            ClearInputs();
            dgv1.ClearSelection();
        }


  
    }
}
