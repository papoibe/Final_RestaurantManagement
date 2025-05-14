using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransferObject
{
    public class HoaDonTT_DTO
    {
        public int MaDonHang { get; set; }
        public int? MaThanhToan { get; set; }
        public string TenBan { get; set; }
        public string HoTen { get; set; }
        public DateTime? NgayThanhToan { get; set; }
        public int MaTrangThai { get; set; }
        public float TongTien { get; set; }
        public string TenHinhThuc { get; set; }

        // Constructor
        public HoaDonTT_DTO(int maDonHang, int? maThanhToan, string tenBan, string hoTen,
                            DateTime? ngayThanhToan, int maTrangThai, float tongTien, string tenHinhThuc)
        {
            MaDonHang = maDonHang;
            MaThanhToan = maThanhToan;
            TenBan = tenBan;
            HoTen = hoTen;
            NgayThanhToan = ngayThanhToan;
            MaTrangThai = maTrangThai;
            TongTien = tongTien;
            TenHinhThuc = tenHinhThuc;
        }
    }
}
