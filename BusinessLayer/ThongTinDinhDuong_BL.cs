using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransferObject; //sử dụng DTO và DAL để truy xuất dữ liệu và truyền đối tượng
using DataLayer;

namespace BusinessLayer
{
    public class ThongTinDinhDuong_BL
    {
        private ThongTinDinhDuong_DAL dal = new ThongTinDinhDuong_DAL(); //khởi tạo dal để gọi hàm truy xuất dữ liệu

        public List<ThongTinDinhDuong_DTO> LoadDinhDuong(string tenMon)  //dùng bên frm
        {
            return dal.GetThongTinDinhDuong(tenMon);
        }
    }
}

