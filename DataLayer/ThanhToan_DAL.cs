using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using TransferObject;
using System.Data.SqlClient;

namespace DataLayer
{
    public class ThanhToan_DAL : DataProvider
    {
        public List<HinhThucTT_DTO> GetHinhThucTT()
        {
            List<HinhThucTT_DTO> lst = new List<HinhThucTT_DTO>();
            string sql = "SELECT * FROM HinhThucThanhToan";
            SqlDataReader dr = MyExecuteReader(sql, CommandType.Text);
            while (dr.Read())
            {
                HinhThucTT_DTO hinhThucTT = new HinhThucTT_DTO(dr.GetInt32(0), dr.GetString(1), dr.IsDBNull(2) ? string.Empty : dr.GetString(2), dr.GetBoolean(3));
                lst.Add(hinhThucTT);
            }
            dr.Close();
            return lst;
        }
        public List<ThanhToan_DTO> GetAll()
        {
            List<ThanhToan_DTO> lst = new List<ThanhToan_DTO>();
            string sql = "SELECT * FROM ThanhToan";
            SqlDataReader dr = MyExecuteReader(sql, CommandType.Text);
            while (dr.Read())
            {
                ThanhToan_DTO thanhToan = new ThanhToan_DTO(dr.GetInt32(0), dr.GetInt32(1), dr.GetInt32(2), (float)dr.GetDouble(3), dr.GetDateTime(4), dr.IsDBNull(5) ? string.Empty : dr.GetString(5));
                lst.Add(thanhToan);
            }
            dr.Close();
            return lst;
        }


        public bool InsertThanhToan(ThanhToan_DTO thanhToan)
        {
            string sql = "INSERT INTO ThanhToan( MaDonHang, MaNhanVien, MaHinhThuc, SoTien, NgayThanhToan, GhiChu) VALUES( @MaDonHang, @MaNhanVien, @MaHinhThuc, @SoTien, @NgayThanhToan, @GhiChu)";
            SqlCommand cmd = new SqlCommand(sql, cn);
            cmd.Parameters.AddWithValue("@MaDonHang", thanhToan.MaDonHang);
            cmd.Parameters.AddWithValue("@MaNhanVien", thanhToan.MaNhanVien);
            cmd.Parameters.AddWithValue("@MaHinhThuc", thanhToan.MaHinhThucTT);
            cmd.Parameters.AddWithValue("@SoTien", thanhToan.SoTien);
            cmd.Parameters.AddWithValue("@NgayThanhToan", thanhToan.NgayThanhToan);
            cmd.Parameters.AddWithValue("@GhiChu", thanhToan.GhiChu);
            try
            {
                Connect();
                int result = cmd.ExecuteNonQuery();
                if (result > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
                DisConnect();
            }
        }
        public List<HoaDonTT_DTO> GetHD()
        {
            List<HoaDonTT_DTO> lst = new List<HoaDonTT_DTO>();
            string sql = @"
            SELECT 
                DonHang.MaDonHang,
                ThanhToan.MaThanhToan,
                Ban.TenBan,
                NhanVien.HoTen,
                ThanhToan.NgayThanhToan,
                DonHang.MaTrangThai,
                DonHang.TongTien,
                HinhThucThanhToan.TenHinhThuc
            FROM 
                DonHang
                INNER JOIN NhanVien ON DonHang.MaNhanVien = NhanVien.MaNhanVien
                LEFT JOIN ThanhToan ON ThanhToan.MaDonHang = DonHang.MaDonHang
                LEFT JOIN HinhThucThanhToan ON ThanhToan.MaHinhThuc = HinhThucThanhToan.MaHinhThuc
                INNER JOIN Ban ON DonHang.MaBan = Ban.MaBan;";

            using (SqlDataReader dr = MyExecuteReader(sql, CommandType.Text))
            {
                while (dr.Read())
                {
                    var hoaDonTT = new HoaDonTT_DTO(
                        dr.GetInt32(0), // MaDonHang
                        dr.IsDBNull(1) ? (int?)null : dr.GetInt32(1), // MaThanhToan (nullable)
                        dr.IsDBNull(2) ? null : dr.GetString(2), // TenBan
                        dr.IsDBNull(3) ? null : dr.GetString(3), // HoTen
                        dr.IsDBNull(4) ? (DateTime?)null : dr.GetDateTime(4), // NgayThanhToan (nullable)
                        dr.GetInt32(5), // MaTrangThai
                        (float)dr.GetDouble(6), // TongTien (int trong SQL)
                        dr.IsDBNull(7) ? null : dr.GetString(7) // TenHinhThuc
                    );
                    lst.Add(hoaDonTT);
                }
            }

            return lst;
        }



        public List<HoaDonTT_DTO> SearchOrders(int? id = null, DateTime? time = null, string nameBan = null, int? status = null)
        {
            var result = new List<HoaDonTT_DTO>();
            const string sql = @"
        SELECT 
            DonHang.MaDonHang,
            ThanhToan.MaThanhToan,
            Ban.TenBan,
            NhanVien.HoTen,
            ThanhToan.NgayThanhToan,
            DonHang.MaTrangThai,
            DonHang.TongTien,
            HinhThucThanhToan.TenHinhThuc
        FROM 
            DonHang
            INNER JOIN NhanVien ON DonHang.MaNhanVien = NhanVien.MaNhanVien
            LEFT JOIN ThanhToan ON ThanhToan.MaDonHang = DonHang.MaDonHang
            LEFT JOIN HinhThucThanhToan ON ThanhToan.MaHinhThuc = HinhThucThanhToan.MaHinhThuc
            INNER JOIN Ban ON DonHang.MaBan = Ban.MaBan
        WHERE 1=1";

            var parameters = new List<SqlParameter>();
            var query = sql;

            if (id.HasValue)
            {
                query += " AND DonHang.MaDonHang = @Id";
                parameters.Add(new SqlParameter("@Id", id.Value));
            }

            if (time.HasValue)
            {
                query += " AND CAST(ThanhToan.NgayThanhToan AS DATE) = @Date";
                parameters.Add(new SqlParameter("@Date", time.Value.Date));
            }

            if (!string.IsNullOrWhiteSpace(nameBan))
            {
                query += " AND Ban.TenBan = @NameBan";
                parameters.Add(new SqlParameter("@NameBan", nameBan));
            }

            if (status.HasValue)
            {
                query += " AND DonHang.MaTrangThai = @Status";
                parameters.Add(new SqlParameter("@Status", status.Value));
            }

            query += " ORDER BY DonHang.MaDonHang DESC";

            try
            {
                Connect(); // Giả định phương thức mở kết nối

                using (SqlDataReader dr = MyExecuteReader(query, CommandType.Text, parameters.ToArray()))
                {
                    while (dr.Read())
                    {
                        var hoaDonTT = new HoaDonTT_DTO(
                            dr.GetInt32(0), // MaDonHang
                            dr.IsDBNull(1) ? (int?)null : dr.GetInt32(1), // MaThanhToan (nullable)
                            dr.IsDBNull(2) ? null : dr.GetString(2), // TenBan
                            dr.IsDBNull(3) ? null : dr.GetString(3), // HoTen
                            dr.IsDBNull(4) ? (DateTime?)null : dr.GetDateTime(4), // NgayThanhToan (nullable)
                            dr.GetInt32(5), // MaTrangThais
                            (float)dr.GetDouble(6), // TongTien (int trong SQL)
                            dr.IsDBNull(7) ? null : dr.GetString(7) // TenHinhThuc
                        );
                        result.Add(hoaDonTT);
                    }
                }

                return result;
            }
            catch (SqlException ex)
            {
                throw new Exception("Lỗi khi truy vấn cơ sở dữ liệu: " + ex.Message, ex);
            }
            finally
            {
                DisConnect(); // Giả định phương thức đóng kết nối
            }
        }

    }
}
