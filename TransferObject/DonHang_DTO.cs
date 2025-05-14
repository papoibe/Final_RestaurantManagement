using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransferObject
{
    public class DonHang_DTO
    {
        public int MaDonHang { get; set; }
        public int MaBan { get; set; }
        public int MaNhanVien { get; set; }
        public DateTime NgayLap { get; set; }
        public int MaTrangThai { get; set; }
        public float GiamGia { get; set; }
        public float TongTien { get; set; }
        public string GhiChu { get; set; }
        public DonHang_DTO() { }
        public DonHang_DTO(int maDonHang, int maBan, int maNhanVien, DateTime ngayLap, int maTrangThai, float giamGia, float tongTien, string ghiChu)
        {
            MaDonHang = maDonHang;
            MaBan = maBan;
            MaNhanVien = maNhanVien;
            NgayLap = ngayLap;
            MaTrangThai = maTrangThai;
            GiamGia = giamGia;
            TongTien = tongTien;
            GhiChu = ghiChu;
        }
    }
}
