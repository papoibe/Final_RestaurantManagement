using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransferObject
{
    public class NguyenVatLieu_DTO
    {
        public int MaNguyenLieu { get; set; }
        public string TenNguyenLieu { get; set; }
        public string DonViTinh { get; set; }
        public float SoLuongTon { get; set; }
        public string MoTa { get; set; }
        public bool TrangThai { get; set; }

        public NguyenVatLieu_DTO() 
        {
            TrangThai = true; 
        }

        public NguyenVatLieu_DTO(int maNguyenLieu, string tenNguyenLieu, string donViTinh, float soLuongTon, string moTa, bool trangThai)
        {
            MaNguyenLieu = maNguyenLieu;
            TenNguyenLieu = tenNguyenLieu;
            DonViTinh = donViTinh;
            SoLuongTon = soLuongTon;
            MoTa = moTa;
            TrangThai = trangThai;
        }
    }
}
