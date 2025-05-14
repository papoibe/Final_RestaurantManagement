using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransferObject
{
    public class KiemTraChatLuong_DTO
    {
        public string MaKiemTra { get; set; }
        public string LoaiKiemTra { get; set; }
        public string DoiTuongKiemTra { get; set; }
        public DateTime NgayKiemTra { get; set; }
        public int NguoiKiemTra { get; set; }
        public string TieuChiKiemTra { get; set; }
        public float? GiaTri { get; set; }
        public string DonVi { get; set; }
        public string KetQua { get; set; }
        public string GhiChu { get; set; }
        public string HinhAnh { get; set; }
        public string TenNguoiKiemTra { get; set; } 


        public KiemTraChatLuong_DTO()
        {
            NgayKiemTra = DateTime.Now;
        }

        public KiemTraChatLuong_DTO(string maKiemTra, string loaiKiemTra, string doiTuongKiemTra, DateTime ngayKiemTra, int nguoiKiemTra, string tieuChiKiemTra, float? giaTri, string donVi, string ketQua, string ghiChu, string hinhAnh, string tenNguoiKiemTra)
        {
            MaKiemTra = maKiemTra;
            LoaiKiemTra = loaiKiemTra;
            DoiTuongKiemTra = doiTuongKiemTra;
            NgayKiemTra = ngayKiemTra;
            NguoiKiemTra = nguoiKiemTra;
            TieuChiKiemTra = tieuChiKiemTra;
            GiaTri = giaTri;
            DonVi = donVi;
            KetQua = ketQua;
            GhiChu = ghiChu;
            HinhAnh = hinhAnh;
            TenNguoiKiemTra = tenNguoiKiemTra;
        }
    }
}
