using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransferObject
{
    public class ChiTietDonHang_DTO
    {
        public int MaChiTietDonHang { get; set; }
        public int MaDonHang { get; set; }
        public int MaMonAn { get; set; }
        public int SoLuong { get; set; }
        public float DonGia { get; set; }
        public float ThanhTien { get; set; }
        public string GhiChu { get; set; }
        public ChiTietDonHang_DTO(int maChiTietDonHang, int maDonHang, int maMonAn, int soLuong, float donGia, float thanhTien, string ghiChu)
        {
            MaChiTietDonHang = maChiTietDonHang;
            MaDonHang = maDonHang;
            MaMonAn = maMonAn;
            SoLuong = soLuong;
            DonGia = donGia;
            ThanhTien = soLuong * donGia;
            GhiChu = ghiChu;
        }
        public ChiTietDonHang_DTO() { }
    }
}
