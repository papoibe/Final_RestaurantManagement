using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransferObject
{
    public class ThanhToan_DTO
    {
        public int MaDonHang { get; set; }
        public int MaNhanVien { get; set; }
        public int MaHinhThucTT { get; set; }
        public float SoTien { get; set; }
        public DateTime NgayThanhToan { get; set; }
        public string GhiChu { get; set; }

        public ThanhToan_DTO() { }
        public ThanhToan_DTO(int maDonHang, int maNhanVien, int maHinhThucTT, float soTien, DateTime ngayThanhToan, string ghiChu)
        {
            MaDonHang = maDonHang;
            MaNhanVien = maNhanVien;
            MaHinhThucTT = maHinhThucTT;
            SoTien = soTien;
            NgayThanhToan = ngayThanhToan;
            GhiChu = ghiChu;
        }
    }
}
