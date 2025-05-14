using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransferObject;

namespace DataLayer
{
    public class Ban_DAL : DataProvider
    {
        public List<Ban_DTO> GetAll()
        {
            List<Ban_DTO> lst = new List<Ban_DTO>();
            string sql = "SELECT * FROM Ban";

            SqlDataReader dr = MyExecuteReader(sql, System.Data.CommandType.Text);
            while (dr.Read())
            {
                Ban_DTO ban = new Ban_DTO(dr.GetInt32(0), dr.GetString(1), dr.GetInt32(2), dr.GetBoolean(3), dr.IsDBNull(4) ? string.Empty : dr.GetString(4));
                lst.Add(ban);
            }
            dr.Close();
            return lst;
        }
        public bool CapNhatBan(Ban_DTO ban)
        {
            string sql = "UPDATE Ban SET TenBan = @TenBan, TrangThai = @TrangThai, GhiChu = @GhiChu WHERE MaBan = @MaBan";
            try
            {
                SqlCommand cmd = new SqlCommand(sql, cn);
                cmd.Parameters.AddWithValue("@MaBan", ban.MaBan);
                cmd.Parameters.AddWithValue("@TenBan", ban.TenBan);
                cmd.Parameters.AddWithValue("@TrangThai", ban.TrangThai);
                cmd.Parameters.AddWithValue("@GhiChu", ban.GhiChu);

                Connect();
                int rowsAffected = cmd.ExecuteNonQuery();
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi cập nhật bàn: " + ex.Message);
            }
            finally
            {
                DisConnect();
            }
        }
        public bool CapNhatTrangThaiBan(int maBan, bool trangThai)
        {
            string sql = "UPDATE Ban SET TrangThai = @TrangThai WHERE MaBan = @MaBan";
            try
            {
                SqlCommand cmd = new SqlCommand(sql, cn);
                cmd.Parameters.AddWithValue("@MaBan", maBan);
                cmd.Parameters.AddWithValue("@TrangThai", trangThai);
                Connect();
                int rowsAffected = cmd.ExecuteNonQuery();
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi cập nhật trạng thái bàn: " + ex.Message);
            }
            finally
            {
                DisConnect();
            }
        }

        public bool GetTrangThaiBan(int maBan)
        {
            string sql = "SELECT TrangThai FROM Ban WHERE MaBan = @MaBan";
            try
            {
                SqlCommand cmd = new SqlCommand(sql, cn);
                cmd.Parameters.AddWithValue("@MaBan", maBan);
                Connect();
                object result = cmd.ExecuteScalar();
                return result != null && (bool)result;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy trạng thái bàn: " + ex.Message);
            }
            finally
            {
                DisConnect();
            }
        }
        public float GetTienBan(int maBan)
        {
            string sql = "select top 1 TongTien from donhang where maban =@MaBan  order by NgayTao DESC";
            try
            {
                SqlCommand cmd = new SqlCommand(sql, cn);
                cmd.Parameters.AddWithValue("@MaBan", maBan);
                Connect();
                object result = cmd.ExecuteScalar();
                return result != null ? Convert.ToSingle(result) : 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy doanh thu bàn: " + ex.Message);
            }
            finally
            {
                DisConnect();
            }


        }
        public List<Ban_DTO> GetBanTrong()
        {
            List<Ban_DTO> lst = new List<Ban_DTO>();
            string sql = "SELECT * FROM Ban WHERE TrangThai = 1";
            SqlDataReader dr = MyExecuteReader(sql, System.Data.CommandType.Text);
            while (dr.Read())
            {
                Ban_DTO ban = new Ban_DTO(dr.GetInt32(0), dr.GetString(1), dr.GetInt32(2), dr.GetBoolean(3), dr.IsDBNull(4) ? string.Empty : dr.GetString(4));
                lst.Add(ban);
            }
            dr.Close();
            return lst;
        }

    }
}
