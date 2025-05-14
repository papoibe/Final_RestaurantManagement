using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransferObject
{
    public class MonAn_MaDH_DTO
    {
        public int MaChiTiet { get; set; }
        public string TenMonAn { get; set; }
        public int SoLuong { get; set; }
        public float DonGia { get; set; }
        public float ThanhTien { get; set; }
        public string GhiChu { get; set; }
        public MonAn_MaDH_DTO(int maChiTiet, string tenMonAn, int soLuong, float donGia, float thanhTien,string ghiChu)
        {
            MaChiTiet = maChiTiet;
            TenMonAn = tenMonAn;
            SoLuong = soLuong;
            DonGia = donGia;
            ThanhTien = thanhTien;
            GhiChu = ghiChu;
        }
    }
}
