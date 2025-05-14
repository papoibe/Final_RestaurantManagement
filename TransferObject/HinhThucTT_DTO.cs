using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransferObject
{
    public class HinhThucTT_DTO
    {
        public int maHinhThucTT { get; set; }
        public string tenHinhThucTT { get; set; }
        public string moTa { get; set; }
        public bool trangThai { get; set; }
        public HinhThucTT_DTO(int maHinhThucTT, string tenHinhThucTT, string moTa, bool trangThai)
        {
            this.maHinhThucTT = maHinhThucTT;
            this.tenHinhThucTT = tenHinhThucTT;
            this.moTa = moTa;
            this.trangThai = trangThai;
        }
    }
}
