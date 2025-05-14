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
    public partial class FrmChonMonAn : Form
    {
        private MonAn_BL monAnBL;
        public int MaMonAnDaChon { get; private set; }
        public string TenMonAnDaChon { get; private set; }
        public float DonGiaDaChon { get; private set; }

        public FrmChonMonAn()
        {
            InitializeComponent();
            monAnBL = new MonAn_BL();
        }

        private void FrmChonMonAn_Load(object sender, EventArgs e)
        {
            try
            {
                //tai danh sach mon an
                LoadDanhSachMonAn();

                // tai danh sach loai mon an
                LoadDanhSachLoaiMonAn();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadDanhSachMonAn()
        {
            try
            {
                // lay danh sach mon an tu bl
                List<MonAn_DTO> dsMonAn = monAnBL.GetAll();

                //loc ra cac mon an trang thai true
                dsMonAn = dsMonAn.Where(ma => ma.TrangThai).ToList();
                // Gán dữ liệu cho DataGridView
                dgvMonAn.DataSource = null;
                dgvMonAn.DataSource = dsMonAn;


                if (dsMonAn != null && dsMonAn.Count > 0)
                {
                    dgvMonAn.DataSource = dsMonAn;
                    dgvMonAn.Columns["MaMonAn"].HeaderText = "Mã Món Ăn";
                    dgvMonAn.Columns["TenMonAn"].HeaderText = "Tên Món Ăn";
                    dgvMonAn.Columns["MaLoai"].HeaderText = "Loại món";
                    dgvMonAn.Columns["DonGia"].HeaderText = "Đơn giá";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải danh sách món ăn: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void LoadDanhSachLoaiMonAn()
        {
            try
            {
                // lay danh sach loai mon an tu bl
                List<LoaiMonAn_DTO> dsLoaiMonAn = monAnBL.GetAllLoaiMonAn();

                // tao item all them vao dau danh sach
                LoaiMonAn_DTO itemAll = new LoaiMonAn_DTO(0, "Tất cả", "");

                // them item all vao danh sach
                dsLoaiMonAn.Insert(0, itemAll);
                cboLoaiMonAn.DisplayMember = "TenLoai";
                cboLoaiMonAn.ValueMember = "MaLoai";

                // gán dữ liệu cho ComboBox
                cboLoaiMonAn.DataSource = dsLoaiMonAn;


                // set gia tri mac dinh cho combobox
                cboLoaiMonAn.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải danh sách loại món ăn: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            try
            {
                string tuKhoa = txtTimKiem.Text.Trim().ToLower();

                // Lấy danh sách món ăn từ Business Layer
                List<MonAn_DTO> dsMonAn = monAnBL.GetAll();

                // Lọc theo trạng thái
                dsMonAn = dsMonAn.Where(ma => ma.TrangThai).ToList();

                // Lọc theo loại món ăn (nếu không phải "Tất cả")
                if (cboLoaiMonAn.SelectedIndex > 0)
                {
                    int maLoai = (int)cboLoaiMonAn.SelectedValue;
                    dsMonAn = dsMonAn.Where(ma => ma.MaLoai == maLoai).ToList();
                }

                // Lọc theo từ khóa tìm kiếm (nếu có)
                if (!string.IsNullOrEmpty(tuKhoa))
                {
                    dsMonAn = dsMonAn.Where(ma =>
                        ma.TenMonAn.ToLower().Contains(tuKhoa) ||
                        ma.MaMonAn.ToString().Contains(tuKhoa) ||
                        ma.MaLoai.ToString().Contains(tuKhoa) 
                    ).ToList();
                }

                // Gán dữ liệu đã lọc cho DataGridView
                dgvMonAn.DataSource = null;
                dgvMonAn.DataSource = dsMonAn;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tìm kiếm: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnXacNhan_Click(object sender, EventArgs e)
        {
            if (dgvMonAn.CurrentRow != null)
            {
                // Lấy thông tin món ăn đã chọn
                MaMonAnDaChon = Convert.ToInt32(dgvMonAn.CurrentRow.Cells["MaMonAn"].Value);
                TenMonAnDaChon = dgvMonAn.CurrentRow.Cells["TenMonAn"].Value.ToString();
                DonGiaDaChon = Convert.ToSingle(dgvMonAn.CurrentRow.Cells["DonGia"].Value);
                // Đóng form
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một món ăn.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btnTimKiem_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true; // Ngăn chặn âm thanh "ding" khi nhấn Enter
                btnTimKiem_Click(sender, e);
            }
        }

        private void dgvMonAn_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                btnXacNhan_Click(sender, e);
            }
        }

        private void cboLoaiMonAn_SelectedIndexChanged(object sender, EventArgs e)
        {
            // khi thay đổi loại món ăn, tự động tìm kiếm
            btnTimKiem_Click(sender, e);
        }
    }
}