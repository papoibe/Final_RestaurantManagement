using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransferObject;

namespace DataLayer
{
    public class MonAn_DAL : DataProvider
    {
        public List<MonAn_DTO> GetAll()
        {
            List<MonAn_DTO> lst = new List<MonAn_DTO>();
            string sql = "SELECT * FROM MonAn";

            SqlDataReader dr = MyExecuteReader(sql, System.Data.CommandType.Text);
            while (dr.Read())
            {
                MonAn_DTO monAn = new MonAn_DTO(
                    dr.GetInt32(0),                 // MaMonAn
                    dr.GetString(1),                // TenMonAn
                    dr.GetInt32(2),                 // MaLoai
                    (float)dr.GetDouble(3),         // DonGia
                    dr.IsDBNull(4) ? string.Empty : dr.GetString(4), // MoTa
                    dr.GetBoolean(6)                // TrangThai
                );
                lst.Add(monAn);
            }
            dr.Close();
            return lst;
        }

        public List<MonAn_DTO> GetMonAnByLoai(int maLoai)
        {
            List<MonAn_DTO> lst = new List<MonAn_DTO>();
            string sql = "SELECT * FROM MonAn WHERE MaLoai = @MaLoai AND TrangThai = 1";

            try
            {
                SqlCommand cmd = new SqlCommand(sql, cn);
                cmd.Parameters.AddWithValue("@MaLoai", maLoai);
                Connect();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    MonAn_DTO monAn = new MonAn_DTO(
                        dr.GetInt32(0),                 // MaMonAn
                        dr.GetString(1),                // TenMonAn
                        dr.GetInt32(2),                 // MaLoai
                        (float)dr.GetDouble(3),         // DonGia
                        dr.IsDBNull(4) ? string.Empty : dr.GetString(4), // MoTa
                        dr.GetBoolean(6)                // TrangThai
                    );
                    lst.Add(monAn);
                }
                dr.Close();
                return lst;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy món ăn theo loại: " + ex.Message);
            }
            finally
            {
                DisConnect();
            }
        }
        public MonAn_DTO GetMonAnByID(int maMonAn)
        {
            string sql = "SELECT * FROM MonAn WHERE MaMonAn = @MaMonAn";
            SqlCommand cmd = new SqlCommand(sql, cn);
            cmd.Parameters.AddWithValue("@MaMonAn", maMonAn);
            Connect();
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                MonAn_DTO monAn = new MonAn_DTO(
                    dr.GetInt32(0),                 // MaMonAn
                    dr.GetString(1),                // TenMonAn
                    dr.GetInt32(2),                 // MaLoai
                    (float)dr.GetDouble(3),         // DonGia
                    dr.IsDBNull(4) ? string.Empty : dr.GetString(4), // MoTa
                    dr.GetBoolean(6)                // TrangThai
                );
                dr.Close();
                return monAn;
            }
            else
            {
                dr.Close();
                return null;
            }


        }
        public List<MonAn_MaDH_DTO> GetMonByMaDH(int MaDH)
        {
            List<MonAn_MaDH_DTO> lst = new List<MonAn_MaDH_DTO>();
            string sql = "select ct.MaChiTiet, m.TenMonAn, ct.SoLuong, ct.DonGia, ct.ThanhTien, ct.GhiChu from MonAn m, ChiTietDonHang ct where m.MaMonAn = ct.MaMonAn And ct.MaDonHang = @MaDH";
            try
            {
                SqlCommand cmd = new SqlCommand(sql, cn);
                cmd.Parameters.AddWithValue("@MaDH", MaDH);
                Connect();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    var monAn = new MonAn_MaDH_DTO(
                        dr.GetInt32(0),                 // MaChiTiet
                        dr.GetString(1),                // TenMonAn
                        dr.GetInt32(2),                 // SoLuong
                        (float)dr.GetDouble(3),         // DonGia
                        (float)dr.GetDouble(4),      // TrangThai
                        dr.IsDBNull(5) ? string.Empty : dr.GetString(5) // GhiChu
                    );
                    lst.Add(monAn);
                }
                dr.Close();
                return lst;
            }
            catch (SqlException ex)
            {
                throw new Exception("Lỗi khi lấy món ăn theo mã đơn hàng: " + ex.Message);
            }
            finally
            {
                DisConnect();
            }
        }
        public List<MonAn_DTO> GetMonAnByTen(string tenMonAn)
        {
            List<MonAn_DTO> lst = new List<MonAn_DTO>();
            string sql = "SELECT * FROM MonAn WHERE TenMonAn LIKE @TenMonAn AND TrangThai = 1";
            try
            {
                SqlCommand cmd = new SqlCommand(sql, cn);
                cmd.Parameters.AddWithValue("@TenMonAn", "%" + tenMonAn + "%");
                Connect();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    MonAn_DTO monAn = new MonAn_DTO(
                        dr.GetInt32(0),                 // MaMonAn
                        dr.GetString(1),                // TenMonAn
                        dr.GetInt32(2),                 // MaLoai
                        (float)dr.GetDouble(3),         // DonGia
                        dr.IsDBNull(4) ? string.Empty : dr.GetString(4), // MoTa
                        dr.GetBoolean(6)                // TrangThai
                    );
                    lst.Add(monAn);
                }
                dr.Close();
                return lst;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi tìm món ăn theo tên: " + ex.Message);
            }
            finally
            {
                DisConnect();
            }
        }

        //trongthinh
        public bool UpdateTrangThaiMonAn(int maMonAn, bool trangThai)
        {
            string sql = "UPDATE MonAn SET TrangThai = @TrangThai WHERE MaMonAn = @MaMonAn";
            try
            {
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@MaMonAn", maMonAn),
                    new SqlParameter("@TrangThai", trangThai)
                };

                int rowsAffected = ExecuteNonQuery(sql, parameters);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi cập nhật trạng thái món ăn trong cơ sở dữ liệu: " + ex.Message);
            }
        }
        public bool UpdatePrice(int maMonAn, float donGiaMoi)
        {
            string sql = "UPDATE MonAn SET DonGia = @DonGia WHERE MaMonAn = @MaMonAn";
            try
            {
                SqlParameter[] parameters = new SqlParameter[]
                {
            new SqlParameter("@MaMonAn", maMonAn),
            new SqlParameter("@DonGia", donGiaMoi)
                };

                int rowsAffected = ExecuteNonQuery(sql, parameters);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi cập nhật giá món ăn trong cơ sở dữ liệu: " + ex.Message);
            }
        }
    }
}