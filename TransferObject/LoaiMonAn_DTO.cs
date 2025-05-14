using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransferObject
{
    public class LoaiMonAn_DTO
    {
        public int MaLoai { get; set; }
        public string TenLoai { get; set; }
        public string MoTa { get; set; }
        public LoaiMonAn_DTO(int maLoai, string tenLoai, string moTa)
        {
            MaLoai = maLoai;
            TenLoai = tenLoai;
            MoTa = moTa;
        }
    }
}
