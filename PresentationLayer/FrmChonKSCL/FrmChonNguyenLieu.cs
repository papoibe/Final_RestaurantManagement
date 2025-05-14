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
using BusinessLayer;

namespace PresentationLayer.FrmChonKSCL
{
    public partial class FrmChonNguyenLieu : Form
    {
        private NguyenVatLieu_BL nguyenVatLieuBL;
        public int MaNguyenLieuDaChon { get; private set; }
        public string TenNguyenLieuDaChon { get; private set; }
        public string DonViTinhDaChon { get; private set; }



        public FrmChonNguyenLieu()
        {
            InitializeComponent();
            nguyenVatLieuBL = new NguyenVatLieu_BL();
        }

        private void FrmChonNguyenLieu_Load(object sender, EventArgs e)
        {
            try
            {
                //load danh sach nguyên liệu
                LoadDanhSachNguyenLieu();

            }
            catch (Exception ex)
            {

                MessageBox.Show("Lỗi khi tải danh sách nguyên liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadDanhSachNguyenLieu()
        {
            try
            {
                List<NguyenVatLieu_DTO> dsNguyenVatLieu = nguyenVatLieuBL.GetNguyenVatLieuList();

                // gán dữ liệu cho DataGridView
                dgvNguyenLieu.DataSource = null;
                dgvNguyenLieu.DataSource = dsNguyenVatLieu;


                if (dsNguyenVatLieu != null && dsNguyenVatLieu.Count > 0)
                {
                    dgvNguyenLieu.DataSource = dsNguyenVatLieu;
                    dgvNguyenLieu.Columns["MaNguyenLieu"].HeaderText = "Mã Nguyên Liệu";
                    dgvNguyenLieu.Columns["TenNguyenLieu"].HeaderText = "Tên Nguyên Liệu";
                    dgvNguyenLieu.Columns["DonViTinh"].HeaderText = "Đơn Vị Tính";
                    dgvNguyenLieu.Columns["SoLuongTon"].HeaderText = "Số Lượng Tồn";
                }
                else
                {
                    MessageBox.Show("Không có nguyên liệu nào trong danh sách.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải danh sách nguyên liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            try
            {
                string tuKhoa = txtTimKiem.Text.Trim().ToLower();

                if (string.IsNullOrEmpty(tuKhoa))
                {
                    // Nếu không có từ khóa tìm kiếm, tải lại danh sách nguyên liệu
                    LoadDanhSachNguyenLieu();
                    return;
                }

                //lay danh sách 
                List<NguyenVatLieu_DTO> dsNguyenVatLieu = nguyenVatLieuBL.GetNguyenVatLieuList();
                // Tìm kiếm nguyên liệu theo từ khóa
                var ketQuaTimKiem = dsNguyenVatLieu.Where(nvl => nvl.TenNguyenLieu.ToLower().Contains(tuKhoa)).ToList();
                if (ketQuaTimKiem.Count > 0)
                {
                    dgvNguyenLieu.DataSource = null;
                    dgvNguyenLieu.DataSource = ketQuaTimKiem;
                }
                else
                {
                    MessageBox.Show("Không tìm thấy nguyên liệu nào với từ khóa: " + tuKhoa, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tìm kiếm nguyên liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnXacNhan_Click(object sender, EventArgs e)
        {
            if (dgvNguyenLieu.CurrentRow != null)
            {
                // Lấy thông tin nguyên liệu đã chọn
                MaNguyenLieuDaChon = Convert.ToInt32(dgvNguyenLieu.CurrentRow.Cells["MaNguyenLieu"].Value);
                TenNguyenLieuDaChon = dgvNguyenLieu.CurrentRow.Cells["TenNguyenLieu"].Value.ToString();
                DonViTinhDaChon = dgvNguyenLieu.CurrentRow.Cells["DonViTinh"].Value.ToString();
                // Đóng form
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một nguyên liệu.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void txtTimKiem_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true; // Ngăn chặn âm thanh "ding" khi nhấn Enter
                btnTimKiem_Click(sender, e);
            }
        }

        private void dgvNguyenLieu_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                btnXacNhan_Click(sender, e);
            }
        }
    }
}
