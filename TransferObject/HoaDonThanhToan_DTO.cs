using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransferObject
{
    public class HoaDonThanhToan_DTO
    {
        public int MaHoaDon { get; set; }
        public int MaDonHang { get; set; }
        public DateTime? NgayThanhToan { get; set; }
        public int MaNhanVien { get; set; }
        public decimal? TongTien { get; set; }
        public int? MaHinhThuc { get; set; }
        public string TrangThai { get; set; }
        public string GhiChu { get; set; }
        public string TenBan { get; set; }
        public string HoTen { get; set; }
        public string TenHinhThuc { get; set; }
        public DateTime NgayDatHang { get; set; }

        public HoaDonThanhToan_DTO() { }
        public HoaDonThanhToan_DTO(int maHoaDon, int maDonHang, DateTime? ngayThanhToan, int maNhanVien, decimal? tongTien, int? maHinhThuc, string trangThai, string ghiChu)
        {
            MaHoaDon = maHoaDon;
            MaDonHang = maDonHang;
            NgayThanhToan = ngayThanhToan;
            MaNhanVien = maNhanVien;
            TongTien = tongTien;
            MaHinhThuc = maHinhThuc;
            TrangThai = trangThai;
            GhiChu = ghiChu;
        }
    }
}