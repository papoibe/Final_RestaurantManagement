using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransferObject
{
    public class MonAn_DTO
    {
        public int MaMonAn { get; set; }
        public string TenMonAn { get; set; }
        public int MaLoai { get; set; }
        public float DonGia { get; set; }
        public string MoTa { get; set; }
        public bool TrangThai { get; set; }

        public MonAn_DTO(int maMonAn, string tenMonAn, int maLoai, float donGia, string moTa, bool trangThai)
        {
            MaMonAn = maMonAn;
            TenMonAn = tenMonAn;
            MaLoai = maLoai;
            DonGia = donGia;
            MoTa = moTa;
            TrangThai = trangThai;
        }
    }
}
