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
    public partial class FrmLapOrder : Form
    {
        public Ban_DTO selectedBan;
        private Ban_BL banBL;
        public DonHang_DTO donHang = new DonHang_DTO();
        private DonHang_BL donHangBL = new DonHang_BL();
        private NhanVien_BL nhanVienBL = new NhanVien_BL();

        public FrmLapOrder(Ban_DTO ban)
        {
            InitializeComponent();
            banBL = new Ban_BL();
            selectedBan = ban;
            donHangBL = new DonHang_BL();
        }

        private void FrmLapOrder_Load(object sender, EventArgs e)
        {
            lb_tenBan.Text = selectedBan.TenBan;
            nUD_SoKhach.Value = selectedBan.SucChua;
            cbox_PhucVuChinh.DataSource = nhanVienBL.LayDanhSachNhanVien();
            cbox_PhucVuChinh.DisplayMember = "HoTen"; // Hiển thị tên nhân viên
            cbox_PhucVuChinh.ValueMember = "MaNhanVien"; // Giá trị của nhân viên
            // chổ này meger code làm tiếp ( load nhân viên vào cbox_PhucVuChinh )
            // cbox_PhucVuChinh.DataSource = donHangBL.GetAllNhanVien();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();   
        }

        private void btn_OK_Click(object sender, EventArgs e)
        {
            selectedBan.SucChua = (int)nUD_SoKhach.Value; // Cập nhật số khách
            selectedBan.GhiChu = rtxt_Note.Text + "\n Phục vụ chính: " + cbox_PhucVuChinh.Text + "\n Loại Khách: " + cbox_LoaiKhach.Text; // Cập nhật ghi chú
                                                                                                                                          // Gọi lớp Business Layer để cập nhật trạng thái bà
            donHang.MaNhanVien = (int)cbox_PhucVuChinh.SelectedValue; // Giả định mã nhân viên là 1, bạn có thể thay đổi theo yêu cầu
            donHang.MaBan = selectedBan.MaBan; // Gán mã bàn cho đơn hàng
            donHang.NgayLap = DateTime.Now; // Gán ngày lập đơn hàng
            donHang.MaTrangThai = 1; // Giả định trạng thái đơn hàng là 1 (chưa thanh toán), bạn có thể thay đổi theo yêu cầu
            donHang.GhiChu = selectedBan.GhiChu; // Gán ghi chú cho đơn hàng
            banBL.CapNhatBan(selectedBan);
            FrmDonHang frmDonHang = new FrmDonHang(donHang, selectedBan);
            DialogResult result = frmDonHang.ShowDialog();
            if (result == DialogResult.OK)
            {
                frmDonHang.ShowDialog();
            }
            else
            {
                this.Close();
            }

        }
    }

}
