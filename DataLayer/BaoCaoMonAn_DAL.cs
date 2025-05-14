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
    public class BaoCaoMonAn_DAL : DataProvider
    {
        public DataTable GetBaoCaoMonAnBanChay(DateTime ngayBatDau, DateTime ngayKetThuc, int soLuong = 10)
        {
            try
            {
                string sql = "EXEC SP_BaoCaoMonAnBanChay @NgayBatDau, @NgayKetThuc, @SoLuong";

                // Tạo và cấu hình SqlCommand
                Connect();
                SqlCommand cmd = new SqlCommand(sql, cn);
                cmd.CommandType = CommandType.Text;

                // Thêm tham số
                cmd.Parameters.AddWithValue("@NgayBatDau", ngayBatDau.Date);
                cmd.Parameters.AddWithValue("@NgayKetThuc", ngayKetThuc.Date);
                cmd.Parameters.AddWithValue("@SoLuong", soLuong);

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

        public int LuuBaoCaoMonAn(BaoCaoMonAn baoCao)
        {
            try
            {
                string sql = @"INSERT INTO BaoCaoMonAn (LoaiBaoCao, NgayBatDau, NgayKetThuc, GhiChu, NgayTao, NguoiTao)
                         VALUES (@LoaiBaoCao, @NgayBatDau, @NgayKetThuc, @GhiChu, GETDATE(), @NguoiTao);
                         SELECT SCOPE_IDENTITY();";

                // Tạo và cấu hình SqlCommand
                Connect();
                SqlCommand cmd = new SqlCommand(sql, cn);
                cmd.CommandType = CommandType.Text;

                // Thêm tham số
                cmd.Parameters.AddWithValue("@LoaiBaoCao", baoCao.LoaiBaoCao);
                cmd.Parameters.AddWithValue("@NgayBatDau", baoCao.NgayBatDau);
                cmd.Parameters.AddWithValue("@NgayKetThuc", baoCao.NgayKetThuc);
                cmd.Parameters.AddWithValue("@GhiChu", (object)baoCao.GhiChu ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@NguoiTao", baoCao.NguoiTao);

                // Thực thi và lấy ID được tạo
                int maBaoCao = Convert.ToInt32(cmd.ExecuteScalar());

                // Lưu chi tiết báo cáo
                if (baoCao.ChiTietBaoCao != null && baoCao.ChiTietBaoCao.Count > 0)
                {
                    foreach (ChiTietBaoCaoMonAn chiTiet in baoCao.ChiTietBaoCao)
                    {
                        LuuChiTietBaoCaoMonAn(maBaoCao, chiTiet);
                    }
                }

                return maBaoCao;
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

        private bool LuuChiTietBaoCaoMonAn(int maBaoCao, ChiTietBaoCaoMonAn chiTiet)
        {
            try
            {
                string sql = @"INSERT INTO ChiTietBaoCaoMonAn (MaBaoCao, MaMonAn, SoLuongDaBan, DoanhThu, TiLe)
                         VALUES (@MaBaoCao, @MaMonAn, @SoLuongDaBan, @DoanhThu, @TiLe)";

                // Tạo và cấu hình SqlCommand
                SqlCommand cmd = new SqlCommand(sql, cn);
                cmd.CommandType = CommandType.Text;

                // Thêm tham số
                cmd.Parameters.AddWithValue("@MaBaoCao", maBaoCao);
                cmd.Parameters.AddWithValue("@MaMonAn", chiTiet.MaMonAn);
                cmd.Parameters.AddWithValue("@SoLuongDaBan", chiTiet.SoLuongDaBan);
                cmd.Parameters.AddWithValue("@DoanhThu", chiTiet.DoanhThu);
                cmd.Parameters.AddWithValue("@TiLe", chiTiet.TiLe);

                // Thực thi
                return (cmd.ExecuteNonQuery() > 0);
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
    }
}
