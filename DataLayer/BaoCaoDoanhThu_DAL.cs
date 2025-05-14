using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using TransferObject;

namespace DataLayer
{
    public class BaoCaoDoanhThu_DAL : DataProvider
    {
        public DataTable GetBaoCaoDoanhThuTheoNgay(DateTime ngayBatDau, DateTime ngayKetThuc)
        {
            try
            {
                string sql = "EXEC SP_BaoCaoDoanhThuTheoNgay @NgayBatDau, @NgayKetThuc";

                // sqlCommand 
                Connect();
                SqlCommand cmd = new SqlCommand(sql, cn);
                cmd.CommandType = CommandType.Text;

                // Thêm tham số
                cmd.Parameters.AddWithValue("@NgayBatDau", ngayBatDau.Date);
                cmd.Parameters.AddWithValue("@NgayKetThuc", ngayKetThuc.Date);

                // Thực thi và lấy kết quả
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                return dt;
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

        public DataTable GetBaoCaoDoanhThuTheoThang(int nam)
        {
            try
            {
                string sql = "EXEC SP_BaoCaoDoanhThuTheoThang @Nam";


                Connect();
                SqlCommand cmd = new SqlCommand(sql, cn);
                cmd.CommandType = CommandType.Text;

                // Thêm tham số
                cmd.Parameters.AddWithValue("@Nam", nam);

                // Thực thi và lấy kết quả
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                return dt;
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

        public DataTable GetBaoCaoDoanhThuTheoLoaiMonAn(DateTime ngayBatDau, DateTime ngayKetThuc)
        {
            try
            {
                string sql = @"SELECT 
                                lma.MaLoai, 
                                lma.TenLoai, 
                                COUNT(DISTINCT dh.MaDonHang) AS SoDonHangLoai, 
                                SUM(ctdh.SoLuong) AS TongSoLuongLoai, 
                                SUM(ctdh.SoLuong * ctdh.DonGia) AS DoanhThuLoai
                              FROM DonHang dh
                              JOIN ChiTietDonHang ctdh ON dh.MaDonHang = ctdh.MaDonHang
                              JOIN MonAn ma ON ctdh.MaMonAn = ma.MaMonAn
                              JOIN LoaiMonAn lma ON ma.MaLoai = lma.MaLoai
                              WHERE dh.NgayTao BETWEEN @NgayBatDau AND @NgayKetThuc
                              GROUP BY lma.MaLoai, lma.TenLoai
                              ORDER BY DoanhThuLoai DESC";

                Connect();
                SqlCommand cmd = new SqlCommand(sql, cn);
                cmd.CommandType = CommandType.Text;

                // Thêm tham số
                cmd.Parameters.AddWithValue("@NgayBatDau", ngayBatDau.Date);
                cmd.Parameters.AddWithValue("@NgayKetThuc", ngayKetThuc.Date);

                // Thực thi và lấy kết quả
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                return dt;
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


        public DataTable GetBaoCaoTonKho()
        {
            try
            {
                string sql = @"SELECT 
                                nvl.MaNguyenLieu, 
                                nvl.TenNguyenLieu, 
                                nvl.DonViTinh, 
                                nvl.SoLuongTon,
                                CASE 
                                    WHEN nvl.SoLuongTon <= 5 THEN N'Cần nhập thêm'
                                    WHEN nvl.SoLuongTon <= 20 THEN N'Sắp hết'
                                    ELSE N'Đủ dùng'
                                END AS TrangThai
                              FROM NguyenVatLieu nvl
                              WHERE nvl.TrangThai = 1
                              ORDER BY nvl.SoLuongTon ASC";

                Connect();
                SqlCommand cmd = new SqlCommand(sql, cn);
                cmd.CommandType = CommandType.Text;

                // Thực thi và lấy kết quả
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                return dt;
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





        public int LuuBaoCaoDoanhThu(BaoCaoDoanhThu baoCao)
        {
            try
            {
                string sql = @"INSERT INTO BaoCaoDoanhThu (LoaiBaoCao, NgayBatDau, NgayKetThuc, TongDoanhThu, GhiChu, NgayTao, NguoiTao)
                         VALUES (@LoaiBaoCao, @NgayBatDau, @NgayKetThuc, @TongDoanhThu, @GhiChu, @NgayTao, @NguoiTao);
                         SELECT SCOPE_IDENTITY();";

                // Tạo và cấu hình SqlCommand
                Connect();
                SqlCommand cmd = new SqlCommand(sql, cn);

                // Thêm tham số
                cmd.Parameters.AddWithValue("@LoaiBaoCao", baoCao.LoaiBaoCao);
                cmd.Parameters.AddWithValue("@NgayBatDau", baoCao.NgayBatDau);
                cmd.Parameters.AddWithValue("@NgayKetThuc", baoCao.NgayKetThuc);
                cmd.Parameters.AddWithValue("@TongDoanhThu", baoCao.TongDoanhThu);
                cmd.Parameters.AddWithValue("@GhiChu", (object)baoCao.GhiChu ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@NgayTao", baoCao.NgayTao);
                cmd.Parameters.AddWithValue("@NguoiTao", baoCao.NguoiTao);

                // Thực thi truy vấn và lấy ID mới được tạo
                object result = cmd.ExecuteScalar();
                if (result != null && result != DBNull.Value)
                {
                    return Convert.ToInt32(result);
                }
                return -1;
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
    }
}
