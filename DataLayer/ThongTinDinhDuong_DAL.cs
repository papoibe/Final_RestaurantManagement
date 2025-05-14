using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransferObject; //sử dụng cac lớp của DTO 


namespace DataLayer
{
    public class ThongTinDinhDuong_DAL
    {
        //khởi tạo 1 đối tượng data provider để truy vấn data
        private DataProvider dp = new DataProvider();

        public List<ThongTinDinhDuong_DTO> GetThongTinDinhDuong(string tenMon)
        {
            List<ThongTinDinhDuong_DTO> result = new List<ThongTinDinhDuong_DTO>();  //tạo list kq trả về
            string sql = @"
                SELECT m.TenMonAn, td.Calo, td.Protein, td.Carbohydrate, td.Fat, td.Fiber, td.Duong, td.Natri, td.ThanhPhanDiUng, td.GhiChu
                FROM ThongTinDinhDuong td
                INNER JOIN MonAn m ON td.MaMonAn = m.MaMonAn
                WHERE m.TenMonAn LIKE @TenMon";

            //khai báo và gán gtri cho tham số sql để tìm kiếm theo tên món
            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@TenMon", "%" + tenMon + "%")
            };
            //Thực thi + lấy kq dưới dạng datatable
            DataTable dt = dp.ExecuteQuery(sql, param);
            foreach (DataRow row in dt.Rows)
            {
                result.Add(new ThongTinDinhDuong_DTO
                {
                    TenMonAn = row["TenMonAn"].ToString(),
                    Calo = Convert.ToDouble(row["Calo"]),
                    Protein = Convert.ToDouble(row["Protein"]),
                    Carbohydrate = Convert.ToDouble(row["Carbohydrate"]),
                    Fat = Convert.ToDouble(row["Fat"]),
                    Fiber = Convert.ToDouble(row["Fiber"]),
                    Duong = Convert.ToDouble(row["Duong"]),
                    Natri = Convert.ToDouble(row["Natri"]),
                    ThanhPhanDiUng = row["ThanhPhanDiUng"].ToString(),
                    GhiChu = row["GhiChu"].ToString()
                }); 
            }
            return result;
        }
    }
}
