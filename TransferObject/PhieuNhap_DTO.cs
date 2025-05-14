using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransferObject
{
    public class PhieuNhap_DTO
    {
        public int MaPhieuNhap { get; set; }
        public int MaNhanVien { get; set; }
        public DateTime NgayNhap { get; set; }
        public string NhaCungCap { get; set; }
        public float TongTien { get; set; }
        public string GhiChu { get; set; }
        public bool TrangThai { get; set; }

        public PhieuNhap_DTO() 
        {
            NgayNhap = DateTime.Now;
            TrangThai = true;
            
        }

        public PhieuNhap_DTO(int maPhieuNhap, int maNhanVien, DateTime ngayNhap, string nhaCungCap, float tongTien, string ghiChu, bool trangThai)
        {
            MaPhieuNhap = maPhieuNhap;
            MaNhanVien = maNhanVien;
            NgayNhap = ngayNhap;
            NhaCungCap = nhaCungCap;
            TongTien = tongTien;
            GhiChu = ghiChu;
            TrangThai = trangThai;
        }
    }
}
