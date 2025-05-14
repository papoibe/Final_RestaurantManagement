using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TransferObject;

namespace PresentationLayer
{
    public partial class ucHoaDon : UserControl
    {

        // Thuộc tính để lưu thông tin hóa đơn
        public object HoaDon { get; set; }


        public ucHoaDon()
        {
            InitializeComponent();
        }

        public ucHoaDon(HoaDonTT_DTO hd)
        {
            InitializeComponent();
            HoaDon = hd;
            BindData(hd);

            // Gán sự kiện Click cho UserControl (tùy chọn để truyền lên các thành phần con)
            this.Click += (s, e) => { }; // Đặt rỗng để tránh lỗi, sẽ gán sự kiện bên form
            foreach (Control control in this.Controls)
            {
                control.Click += (s, e) => this.OnClick(e);
            }
        }


        public ucHoaDon(HoaDonThanhToan_DTO hd)
        {
            InitializeComponent();
            HoaDon = hd;
            BindData(hd);

            // Gán sự kiện Click cho UserControl (tùy chọn để truyền lên các thành phần con)
            this.Click += (s, e) => { }; // Đặt rỗng để tránh lỗi, sẽ gán sự kiện bên form
            foreach (Control control in this.Controls)
            {
                control.Click += (s, e) => this.OnClick(e);
            }
        }
        private void BindData(HoaDonTT_DTO hd)
        {
            lb_maHD.Text += hd.MaDonHang.ToString();
            lb_maTT.Text += hd.MaThanhToan.ToString();
            lb_tenBan.Text = string.IsNullOrEmpty(hd.TenBan) ? "Không xác định" : hd.TenBan;

            // Không có thuộc tính TrangThai trong HoaDonTT_DTO, chỉ set màu mặc định
            this.BackColor = Color.LightBlue;

            lb_gioTT.Text = hd.NgayThanhToan.HasValue ? hd.NgayThanhToan.Value.ToString("HH:mm:ss") : "--";
            lb_ngayTT.Text = hd.NgayThanhToan.HasValue ? hd.NgayThanhToan.Value.ToString("dd/MM/yyyy") : "--";

            lb_tenTN.Text = string.IsNullOrEmpty(hd.HoTen) ? "Không xác định" : hd.HoTen;
            lb_tongTien.Text = string.Format("{0:N0} VND", hd.TongTien); // float luôn có giá trị
            lb_phuongthucTT.Text = string.IsNullOrEmpty(hd.TenHinhThuc) ? "Chưa thanh toán" : hd.TenHinhThuc;
        }




        private void BindData(HoaDonThanhToan_DTO hd)
        {
            lb_maHD.Text += hd.MaDonHang.ToString();
            lb_maTT.Text += hd.MaHoaDon.ToString();
            lb_tenBan.Text = string.IsNullOrEmpty(hd.TenBan) ? "Không xác định" : hd.TenBan;

            // Thay đổi màu nền dựa trên TrangThai
            if (hd.TrangThai == "Chưa thanh toán")
                this.BackColor = Color.Blue;
            else if (hd.TrangThai == "Đã thanh toán")
                this.BackColor = Color.Green;
            else
                this.BackColor = Color.Red;

            lb_gioTT.Text = hd.NgayThanhToan.HasValue ? hd.NgayThanhToan.Value.ToString("HH:mm:ss") : "--";
            lb_ngayTT.Text = hd.NgayThanhToan.HasValue ? hd.NgayThanhToan.Value.ToString("dd/MM/yyyy") : "--";

            lb_tenTN.Text = string.IsNullOrEmpty(hd.HoTen) ? "Không xác định" : hd.HoTen;
            lb_tongTien.Text = hd.TongTien.HasValue ? string.Format("{0:N0} VND", hd.TongTien.Value) : "0 VND";
            lb_phuongthucTT.Text = string.IsNullOrEmpty(hd.TenHinhThuc) ? "Chưa thanh toán" : hd.TenHinhThuc;
        }
    }
}
