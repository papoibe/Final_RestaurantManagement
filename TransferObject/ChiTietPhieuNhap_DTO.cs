using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransferObject
{
    public class ChiTietPhieuNhap_DTO
    {
        public int MaChiTiet { get; set; }
        public int MaPhieuNhap { get; set; }
        public int MaNguyenLieu { get; set; }
        public string TenNguyenLieu { get; set; } // hien thi ten nguyen lieu
        public float SoLuong {  get; set; }
        public float DonGia { get; set; }
        public float ThanhTien { get; set; }
        public string GhiChu { get; set; }


        public ChiTietPhieuNhap_DTO() { }

        public ChiTietPhieuNhap_DTO(int maChiTiet, int maPhieuNhap, int maNguyenLieu, string tenNguyenLieu, float soLuong, float donGia, float thanhTien, string ghiChu)
        {
            MaChiTiet = maChiTiet;
            MaPhieuNhap = maPhieuNhap;
            MaNguyenLieu = maNguyenLieu;
            TenNguyenLieu = tenNguyenLieu;
            SoLuong = soLuong;
            DonGia = donGia;
            ThanhTien = thanhTien;
            GhiChu = ghiChu;
        }
    }
}

