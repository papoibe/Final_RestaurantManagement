using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using TransferObject;

namespace DataLayer
{
    public class ChiTietDonHang_DAL : DataProvider
    {
        public List<ChiTietDonHang_DTO> GetAll()
        {
            List<ChiTietDonHang_DTO> lst = new List<ChiTietDonHang_DTO>();
            string sql = "SELECT * FROM ChiTietDonHang";
            try
            {
                Connect();
                SqlCommand cmd = new SqlCommand(sql, cn);
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    ChiTietDonHang_DTO chiTiet = new ChiTietDonHang_DTO(
                        dr.GetInt32(0),
                        dr.GetInt32(1),
                        dr.GetInt32(2),
                        dr.GetInt32(3),
                        dr.IsDBNull(4) ? 0f : (float)dr.GetDouble(4),
                        dr.IsDBNull(5) ? 0f : (float)dr.GetDouble(5),
                        dr.IsDBNull(6) ? string.Empty : dr.GetString(6)
                    );
                    lst.Add(chiTiet);
                }
                dr.Close();
                return lst;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy danh sách chi tiết đơn hàng: " + ex.Message);
            }
            finally
            {
                DisConnect();
            }
        }

        public List<ChiTietDonHang_DTO> GetByMaDonHang(int maDonHang)
        {
            List<ChiTietDonHang_DTO> lst = new List<ChiTietDonHang_DTO>();
            string sql = "SELECT * FROM ChiTietDonHang WHERE MaDonHang = @MaDonHang";
            try
            {
                Connect();
                SqlCommand cmd = new SqlCommand(sql, cn);
                cmd.Parameters.AddWithValue("@MaDonHang", maDonHang);
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    ChiTietDonHang_DTO chiTiet = new ChiTietDonHang_DTO(
                        dr.GetInt32(0),
                        dr.GetInt32(1),
                        dr.GetInt32(2),
                        dr.GetInt32(3),
                        dr.IsDBNull(4) ? 0f : (float)dr.GetDouble(4),
                        dr.IsDBNull(5) ? 0f : (float)dr.GetDouble(5),
                        dr.IsDBNull(6) ? string.Empty : dr.GetString(6)
                    );
                    lst.Add(chiTiet);
                }
                dr.Close();
                return lst;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy chi tiết đơn hàng theo mã đơn hàng: " + ex.Message);
            }
            finally
            {
                DisConnect();
            }
        }

        public bool Insert(ChiTietDonHang_DTO chiTiet)
        {
            string sql = "INSERT INTO ChiTietDonHang (MaDonHang, MaMonAn, SoLuong, DonGia, ThanhTien, GhiChu) " +
                         "VALUES (@MaDonHang, @MaMonAn, @SoLuong, @DonGia, @ThanhTien, @GhiChu)";
            try
            {
                Connect();
                SqlCommand cmd = new SqlCommand(sql, cn);
                cmd.Parameters.AddWithValue("@MaDonHang", chiTiet.MaDonHang);
                cmd.Parameters.AddWithValue("@MaMonAn", chiTiet.MaMonAn);
                cmd.Parameters.AddWithValue("@SoLuong", chiTiet.SoLuong);
                cmd.Parameters.AddWithValue("@DonGia", chiTiet.DonGia);
                cmd.Parameters.AddWithValue("@ThanhTien", chiTiet.ThanhTien);
                cmd.Parameters.AddWithValue("@GhiChu", (object)chiTiet.GhiChu ?? DBNull.Value);

                int rowsAffected = cmd.ExecuteNonQuery();
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi thêm chi tiết đơn hàng: " + ex.Message);
            }
            finally
            {
                DisConnect();
            }
        }

        public bool Delete(ChiTietDonHang_DTO chiTiet)
        {
            string sql = "DELETE FROM ChiTietDonHang WHERE MaChiTiet = @MaChiTiet";
            try
            {
                Connect();
                SqlCommand cmd = new SqlCommand(sql, cn);
                cmd.Parameters.AddWithValue("@MaChiTiet", chiTiet.MaChiTietDonHang);

                int rowsAffected = cmd.ExecuteNonQuery();
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi xóa chi tiết đơn hàng: " + ex.Message);
            }
            finally
            {
                DisConnect();
            }
        }

        public bool Change(ChiTietDonHang_DTO chiTiet)
        {
            string sql = "UPDATE ChiTietDonHang SET SoLuong = @SoLuongMoi, ThanhTien = DonGia * @SoLuongMoi WHERE MaChiTiet = @MaChiTiet";
            try
            {
                Connect();
                SqlCommand cmd = new SqlCommand(sql, cn);
                cmd.Parameters.AddWithValue("@SoLuongMoi", chiTiet.SoLuong);
                cmd.Parameters.AddWithValue("@MaChiTiet", chiTiet.MaChiTietDonHang);

                int rowsAffected = cmd.ExecuteNonQuery();
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi cập nhật số lượng món: " + ex.Message);
            }
            finally
            {
                DisConnect();
            }
        }
        public bool Update(ChiTietDonHang_DTO chiTiet)
        {
            string sql = "UPDATE ChiTietDonHang SET MaMonAn = @MaMonAn, SoLuong = @SoLuong, DonGia = @DonGia, ThanhTien = @ThanhTien, GhiChu = @GhiChu WHERE MaChiTiet = @MaChiTiet";
            try
            {
                Connect();
                SqlCommand cmd = new SqlCommand(sql, cn);
                cmd.Parameters.AddWithValue("@MaMonAn", chiTiet.MaMonAn);
                cmd.Parameters.AddWithValue("@SoLuong", chiTiet.SoLuong);
                cmd.Parameters.AddWithValue("@DonGia", chiTiet.DonGia);
                cmd.Parameters.AddWithValue("@ThanhTien", chiTiet.ThanhTien);
                cmd.Parameters.AddWithValue("@GhiChu", (object)chiTiet.GhiChu ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@MaChiTiet", chiTiet.MaChiTietDonHang);
                int rowsAffected = cmd.ExecuteNonQuery();
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi cập nhật chi tiết đơn hàng: " + ex.Message);
            }
            finally
            {
                DisConnect();
            }
        }
        public bool UpdateMaDH(ChiTietDonHang_DTO chiTiet)
        {
            string sql = "UPDATE ChiTietDonHang SET MaDonHang = @MaDonHang WHERE MaChiTiet = @MaChiTiet";
            try
            {
                Connect();
                SqlCommand cmd = new SqlCommand(sql, cn);
                cmd.Parameters.AddWithValue("@MaDonHang", chiTiet.MaDonHang);
                cmd.Parameters.AddWithValue("@MaChiTiet", chiTiet.MaChiTietDonHang);
                int rowsAffected = cmd.ExecuteNonQuery();
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi cập nhật chi tiết đơn hàng: " + ex.Message);
            }
            finally
            {
                DisConnect();
            }
        }
        public ChiTietDonHang_DTO GetChiTietDonHang(int maChiTiet)
        {
            string sql = "SELECT * FROM ChiTietDonHang WHERE MaChiTiet = @MaChiTiet";
            try
            {
                Connect();
                SqlCommand cmd = new SqlCommand(sql, cn);
                cmd.Parameters.AddWithValue("@MaChiTiet", maChiTiet);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    ChiTietDonHang_DTO chiTiet = new ChiTietDonHang_DTO(
                        dr.GetInt32(0),
                        dr.GetInt32(1),
                        dr.GetInt32(2),
                        dr.GetInt32(3),
                        dr.IsDBNull(4) ? 0f : (float)dr.GetDouble(4),
                        dr.IsDBNull(5) ? 0f : (float)dr.GetDouble(5),
                        dr.IsDBNull(6) ? string.Empty : dr.GetString(6)
                    );
                    return chiTiet;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy chi tiết đơn hàng theo mã chi tiết: " + ex.Message);
            }
            finally
            {
                DisConnect();
            }
        }
    }
}