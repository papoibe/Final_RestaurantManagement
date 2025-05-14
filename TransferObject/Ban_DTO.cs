using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransferObject
{
    public class Ban_DTO
    {
        public int MaBan { get; set; }
        public string TenBan { get; set; }
        public int SucChua { get; set; }
        public bool TrangThai { get; set; }
        public string GhiChu { get; set; }
        public Ban_DTO(int maBan, string tenBan, int sucChua, bool trangThai, string ghiChu)
        {
            MaBan = maBan;
            TenBan = tenBan;
            SucChua = sucChua;
            TrangThai = trangThai;
            GhiChu = ghiChu;
        }
    }
}
