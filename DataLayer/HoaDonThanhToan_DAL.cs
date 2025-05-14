using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using TransferObject;

namespace DataLayer
{
    public class HoaDonThanhToan_DAL : DataProvider
    {
        public List<HoaDonThanhToan_DTO> GetAll()
        {
            var list = new List<HoaDonThanhToan_DTO>();
            string sql = @"SELECT hdt.MaHoaDon, hdt.MaDonHang, hdt.NgayDatHang, hdt.NgayThanhToan, hdt.MaNhanVien, hdt.TongTien, hdt.MaHinhThuc, hdt.TrangThai, hdt.GhiChu,
                                b.TenBan, nv.HoTen, ht.TenHinhThuc
                         FROM HoaDonThanhToan hdt
                         LEFT JOIN DonHang dh ON hdt.MaDonHang = dh.MaDonHang
                         LEFT JOIN Ban b ON dh.MaBan = b.MaBan
                         LEFT JOIN NhanVien nv ON hdt.MaNhanVien = nv.MaNhanVien
                         LEFT JOIN HinhThucThanhToan ht ON hdt.MaHinhThuc = ht.MaHinhThuc";
            SqlDataReader dr = MyExecuteReader(sql, CommandType.Text);
            while (dr.Read())
            {
                var dto = new HoaDonThanhToan_DTO
                {
                    MaHoaDon = dr.GetInt32(0),
                    MaDonHang = dr.GetInt32(1),
                    NgayDatHang = dr.GetDateTime(2),
                    NgayThanhToan = dr.IsDBNull(3) ? (DateTime?)null : dr.GetDateTime(3),
                    MaNhanVien = dr.GetInt32(4),
                    TongTien = dr.IsDBNull(5) ? (decimal?)null : dr.GetDecimal(5),
                    MaHinhThuc = dr.IsDBNull(6) ? (int?)null : dr.GetInt32(6),
                    TrangThai = dr.IsDBNull(7) ? null : dr.GetString(7),
                    GhiChu = dr.IsDBNull(8) ? null : dr.GetString(8),
                    TenBan = dr.IsDBNull(9) ? null : dr.GetString(9),
                    HoTen = dr.IsDBNull(10) ? null : dr.GetString(10),
                    TenHinhThuc = dr.IsDBNull(11) ? null : dr.GetString(11)
                };
                list.Add(dto);
            }
            dr.Close();
            return list;
        }

        public HoaDonThanhToan_DTO GetById(int maHoaDon)
        {
            string sql = @"SELECT hdt.MaHoaDon, hdt.MaDonHang, hdt.NgayDatHang, hdt.NgayThanhToan, hdt.MaNhanVien, hdt.TongTien, hdt.MaHinhThuc, hdt.TrangThai, hdt.GhiChu,
                                b.TenBan, nv.HoTen, ht.TenHinhThuc
                         FROM HoaDonThanhToan hdt
                         LEFT JOIN DonHang dh ON hdt.MaDonHang = dh.MaDonHang
                         LEFT JOIN Ban b ON dh.MaBan = b.MaBan
                         LEFT JOIN NhanVien nv ON hdt.MaNhanVien = nv.MaNhanVien
                         LEFT JOIN HinhThucThanhToan ht ON hdt.MaHinhThuc = ht.MaHinhThuc
                         WHERE hdt.MaHoaDon = @MaHoaDon";
            SqlCommand cmd = new SqlCommand(sql, cn);
            cmd.Parameters.AddWithValue("@MaHoaDon", maHoaDon);
            Connect();
            SqlDataReader dr = cmd.ExecuteReader();
            HoaDonThanhToan_DTO dto = null;
            if (dr.Read())
            {
                dto = new HoaDonThanhToan_DTO
                {
                    MaHoaDon = dr.GetInt32(0),
                    MaDonHang = dr.GetInt32(1),
                    NgayDatHang = dr.GetDateTime(2),
                    NgayThanhToan = dr.IsDBNull(3) ? (DateTime?)null : dr.GetDateTime(3),
                    MaNhanVien = dr.GetInt32(4),
                    TongTien = dr.IsDBNull(5) ? (decimal?)null : dr.GetDecimal(5),
                    MaHinhThuc = dr.IsDBNull(6) ? (int?)null : dr.GetInt32(6),
                    TrangThai = dr.IsDBNull(7) ? null : dr.GetString(7),
                    GhiChu = dr.IsDBNull(8) ? null : dr.GetString(8),
                    TenBan = dr.IsDBNull(9) ? null : dr.GetString(9),
                    HoTen = dr.IsDBNull(10) ? null : dr.GetString(10),
                    TenHinhThuc = dr.IsDBNull(11) ? null : dr.GetString(11)
                };
            }
            dr.Close();
            DisConnect();
            return dto;
        }

        public bool Insert(HoaDonThanhToan_DTO hoaDon)
        {
            string sql = @"INSERT INTO HoaDonThanhToan (MaDonHang, NgayDatHang, NgayThanhToan, MaNhanVien, TongTien, MaHinhThuc, TrangThai, GhiChu)
                            VALUES (@MaDonHang, @NgayDatHang, @NgayThanhToan, @MaNhanVien, @TongTien, @MaHinhThuc, @TrangThai, @GhiChu)";
            SqlCommand cmd = new SqlCommand(sql, cn);
            cmd.Parameters.AddWithValue("@MaDonHang", hoaDon.MaDonHang);
            cmd.Parameters.AddWithValue("@NgayDatHang", hoaDon.NgayDatHang);
            cmd.Parameters.AddWithValue("@NgayThanhToan", (object)hoaDon.NgayThanhToan ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@MaNhanVien", hoaDon.MaNhanVien);
            cmd.Parameters.AddWithValue("@TongTien", (object)hoaDon.TongTien ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@MaHinhThuc", (object)hoaDon.MaHinhThuc ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@TrangThai", (object)hoaDon.TrangThai ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@GhiChu", (object)hoaDon.GhiChu ?? DBNull.Value);
            try
            {
                Connect();
                int result = cmd.ExecuteNonQuery();
                return result > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi thêm hóa đơn thanh toán: " + ex.Message);
            }
            finally
            {
                DisConnect();
            }
        }

        public bool Update(HoaDonThanhToan_DTO hoaDon)
        {
            string sql = @"UPDATE HoaDonThanhToan SET NgayThanhToan = @NgayThanhToan, MaNhanVien = @MaNhanVien, TongTien = @TongTien, MaHinhThuc = @MaHinhThuc, TrangThai = @TrangThai, GhiChu = @GhiChu WHERE MaHoaDon = @MaHoaDon";
            SqlCommand cmd = new SqlCommand(sql, cn);
            cmd.Parameters.AddWithValue("@MaHoaDon", hoaDon.MaHoaDon);
            cmd.Parameters.AddWithValue("@NgayThanhToan", (object)hoaDon.NgayThanhToan ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@MaNhanVien", hoaDon.MaNhanVien);
            cmd.Parameters.AddWithValue("@TongTien", (object)hoaDon.TongTien ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@MaHinhThuc", (object)hoaDon.MaHinhThuc ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@TrangThai", (object)hoaDon.TrangThai ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@GhiChu", (object)hoaDon.GhiChu ?? DBNull.Value);
            try
            {
                Connect();
                int result = cmd.ExecuteNonQuery();
                return result > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi cập nhật hóa đơn thanh toán: " + ex.Message);
            }
            finally
            {
                DisConnect();
            }
        }
    }
}