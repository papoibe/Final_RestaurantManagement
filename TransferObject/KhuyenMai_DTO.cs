using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransferObject
{
        public class KhuyenMai_DTO
        {
            public int MaKhuyenMai { get; set; }
            public string TenKhuyenMai { get; set; }
            public string NoiDung { get; set; }
            public string TrangThaiHienThi
            {
            get { return TrangThai ? "Đang hoạt động" : "Đã kết thúc"; }
            }
            public float PhanTramGiam { get; set; }
            public DateTime NgayBatDau { get; set; }
            public DateTime NgayKetThuc { get; set; }
            public bool TrangThai { get; set; }
            public string GhiChu { get; set; }
        }
}
