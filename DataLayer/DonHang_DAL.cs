using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransferObject;
using System.Data.SqlClient;
using System.Data;

namespace DataLayer
{
    public class DonHang_DAL : DataProvider
    {
        public List<DonHang_DTO> GetAll()
        {
            List<DonHang_DTO> lst = new List<DonHang_DTO>();
            string sql = "SELECT * FROM DonHang";
            SqlDataReader dr = MyExecuteReader(sql, System.Data.CommandType.Text);
            while (dr.Read())
            {
                DonHang_DTO donHang = new DonHang_DTO(dr.GetInt32(0), dr.GetInt32(1), dr.GetInt32(2), dr.GetDateTime(3), dr.GetInt32(4), (float)dr.GetDouble(5), (float)dr.GetDouble(6), dr.IsDBNull(7) ? string.Empty : dr.GetString(7));
                lst.Add(donHang);
            }
            dr.Close();
            return lst;
        }
        public DonHang_DTO GetById(int maDonHang)
        {
            string sql = "SELECT * FROM DonHang WHERE MaDonHang = @MaDonHang";
            SqlCommand cmd = new SqlCommand(sql, cn);
            cmd.Parameters.AddWithValue("@MaDonHang", maDonHang);
            Connect();
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                DonHang_DTO donHang = new DonHang_DTO(dr.GetInt32(0), dr.GetInt32(1), dr.GetInt32(2), dr.IsDBNull(3) ? DateTime.Today : dr.GetDateTime(3), dr.GetInt32(4), dr.IsDBNull(5) ? 0 : (float)dr.GetDouble(5), dr.IsDBNull(6) ? 0 : (float)dr.GetDouble(6), dr.IsDBNull(7) ? string.Empty : dr.GetString(7));
                dr.Close();
                return donHang;
            }
            dr.Close();
            return null;
        }

        public bool Insert(DonHang_DTO donHang)
        {
            string sql = "INSERT INTO DonHang (MaBan, MaNhanVien, NgayTao, MaTrangThai, GiamGia, TongTien, GhiChu) VALUES (@MaBan, @MaNV, @NgayTao, @MaTrangThai, @MaGiamGia, @TongTien, @GhiChu)";
            try
            {
                SqlCommand cmd = new SqlCommand(sql, cn);
                cmd.Parameters.AddWithValue("@MaBan", donHang.MaBan);
                cmd.Parameters.AddWithValue("@MaNV", donHang.MaNhanVien);
                cmd.Parameters.AddWithValue("@NgayTao", donHang.NgayLap);
                cmd.Parameters.AddWithValue("@MaTrangThai", donHang.MaTrangThai);
                cmd.Parameters.AddWithValue("@MaGiamGia", donHang.GiamGia);
                cmd.Parameters.AddWithValue("@TongTien", donHang.TongTien);
                cmd.Parameters.AddWithValue("@GhiChu", donHang.GhiChu);
                Connect();
                int rowsAffected = cmd.ExecuteNonQuery();
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi thêm đơn hàng: " + ex.Message);
            }
            finally
            {
                DisConnect();
            }
        }
        public bool Update(DonHang_DTO donHang)
        {
            string sql = "UPDATE DonHang SET MaBan = @MaBan, MaNhanVien = @MaNV, NgayTao = @NgayTao, MaTrangThai = @MaTrangThai, GiamGia = @MaGiamGia, TongTien = @TongTien, GhiChu = @GhiChu WHERE MaDonHang = @MaDonHang";
            try
            {
                SqlCommand cmd = new SqlCommand(sql, cn);
                cmd.Parameters.AddWithValue("@MaBan", donHang.MaBan);
                cmd.Parameters.AddWithValue("@MaNV", donHang.MaNhanVien);
                cmd.Parameters.AddWithValue("@NgayTao", donHang.NgayLap);
                cmd.Parameters.AddWithValue("@MaTrangThai", donHang.MaTrangThai);
                cmd.Parameters.AddWithValue("@MaGiamGia", donHang.GiamGia);
                cmd.Parameters.AddWithValue("@TongTien", donHang.TongTien);
                cmd.Parameters.AddWithValue("@GhiChu", donHang.GhiChu);
                cmd.Parameters.AddWithValue("@MaDonHang", donHang.MaDonHang);
                Connect();
                int rowsAffected = cmd.ExecuteNonQuery();
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi cập nhật đơn hàng: " + ex.Message);
            }
            finally
            {
                DisConnect();
            }
        }
        public int GetLastId(int MaBan)
        {
            string sql = "SELECT TOP 1 MaDonHang FROM DonHang WHERE MaBan = @MaBan ORDER BY DonHang.NgayTao DESC";
            try
            {

                SqlCommand cmd = new SqlCommand(sql, cn);
                cmd.Parameters.AddWithValue("@MaBan", MaBan);
                Connect();
                object result = cmd.ExecuteScalar();
                if (result != DBNull.Value)
                {
                    return Convert.ToInt32(result);
                }
                return 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy ID cuối cùng: " + ex.Message);
            }
            finally
            {
                DisConnect();
            }
        }
        public float GetTongTien(int maDonHang)
        {
            string sql = "SELECT SUM(ThanhTien) FROM ChiTietDonHang WHERE MaDonHang = @MaDonHang";
            try
            {
                SqlCommand cmd = new SqlCommand(sql, cn);
                cmd.Parameters.AddWithValue("@MaDonHang", maDonHang);
                Connect();
                object result = cmd.ExecuteScalar();
                if (result != DBNull.Value)
                {
                    return Convert.ToSingle(result);
                }
                return 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy tổng tiền: " + ex.Message);
            }
            finally
            {
                DisConnect();
            }
        }
    }
}
