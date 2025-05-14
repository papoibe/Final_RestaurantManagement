using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransferObject
{
    public class BaoCaoDoanhThu
    {
        public int MaBaoCao { get; set; }
        public string LoaiBaoCao { get; set; }
        public DateTime NgayBatDau { get; set; }
        public DateTime NgayKetThuc { get; set; }
        public decimal TongDoanhThu { get; set; }
        public string GhiChu { get; set; }
        public DateTime NgayTao { get; set; }
        public int NguoiTao { get; set; }

        public BaoCaoDoanhThu()
        {
        }

        public BaoCaoDoanhThu(string loaiBaoCao, DateTime ngayBatDau, DateTime ngayKetThuc, decimal tongDoanhThu, string ghiChu, int nguoiTao)
        {
            LoaiBaoCao = loaiBaoCao;
            NgayBatDau = ngayBatDau;
            NgayKetThuc = ngayKetThuc;
            TongDoanhThu = tongDoanhThu;
            GhiChu = ghiChu;
            NgayTao = DateTime.Now;
            NguoiTao = nguoiTao;
        }
    }

    public class BaoCaoMonAn
    {
        public int MaBaoCao { get; set; }
        public string LoaiBaoCao { get; set; }
        public DateTime NgayBatDau { get; set; }
        public DateTime NgayKetThuc { get; set; }
        public string GhiChu { get; set; }
        public DateTime NgayTao { get; set; }
        public int NguoiTao { get; set; }
        public List<ChiTietBaoCaoMonAn> ChiTietBaoCao { get; set; }

        public BaoCaoMonAn()
        {
            ChiTietBaoCao = new List<ChiTietBaoCaoMonAn>();
        }

        public BaoCaoMonAn(string loaiBaoCao, DateTime ngayBatDau, DateTime ngayKetThuc, string ghiChu, int nguoiTao)
        {
            LoaiBaoCao = loaiBaoCao;
            NgayBatDau = ngayBatDau;
            NgayKetThuc = ngayKetThuc;
            GhiChu = ghiChu;
            NgayTao = DateTime.Now;
            NguoiTao = nguoiTao;
            ChiTietBaoCao = new List<ChiTietBaoCaoMonAn>();
        }
    }

    public class ChiTietBaoCaoMonAn
    {
        public int MaChiTiet { get; set; }
        public int MaBaoCao { get; set; }
        public int MaMonAn { get; set; }
        public string TenMonAn { get; set; }
        public int SoLuongDaBan { get; set; }
        public decimal DoanhThu { get; set; }
        public decimal TiLe { get; set; }

        public ChiTietBaoCaoMonAn()
        {
        }

        public ChiTietBaoCaoMonAn(int maMonAn, string tenMonAn, int soLuongDaBan, decimal doanhThu, decimal tiLe)
        {
            MaMonAn = maMonAn;
            TenMonAn = tenMonAn;
            SoLuongDaBan = soLuongDaBan;
            DoanhThu = doanhThu;
            TiLe = tiLe;
        }
    }
}
