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
    public class KiemTraChatLuong_DAL : DataProvider
    {
        // Lấy danh sách kiểm tra chất lượng
        public List<KiemTraChatLuong_DTO> GetKiemTraChatLuongList()
        {
            string sql = @"SELECT kt.MaKiemTra, kt.LoaiKiemTra, kt.DoiTuongKiemTra, 
                       kt.NgayKiemTra, kt.NguoiKiemTra, nv.HoTen as TenNguoiKiemTra,
                       kt.TieuChiKiemTra, kt.GiaTri, kt.DonVi, kt.KetQua, kt.GhiChu, kt.HinhAnh
                FROM KiemTraChatLuong kt
                INNER JOIN NhanVien nv ON kt.NguoiKiemTra = nv.MaNhanVien
                ORDER BY kt.NgayKiemTra DESC";

            List<KiemTraChatLuong_DTO> danhSachKiemTra = new List<KiemTraChatLuong_DTO>();

            try
            {
                Connect();
                SqlDataReader reader = MyExecuteReader(sql, CommandType.Text);

                while (reader.Read())
                {
                    KiemTraChatLuong_DTO kiemTra = new KiemTraChatLuong_DTO();
                    kiemTra.MaKiemTra = reader["MaKiemTra"].ToString();
                    kiemTra.LoaiKiemTra = reader["LoaiKiemTra"].ToString();
                    kiemTra.DoiTuongKiemTra = reader["DoiTuongKiemTra"].ToString();
                    kiemTra.NgayKiemTra = Convert.ToDateTime(reader["NgayKiemTra"]);
                    kiemTra.NguoiKiemTra = Convert.ToInt32(reader["NguoiKiemTra"]);
                    kiemTra.TenNguoiKiemTra = reader["TenNguoiKiemTra"].ToString();

                    // Xử lý trường null
                    kiemTra.TieuChiKiemTra = reader["TieuChiKiemTra"] == DBNull.Value ? null : reader["TieuChiKiemTra"].ToString();
                    kiemTra.GiaTri = reader["GiaTri"] == DBNull.Value ? null : (float?)Convert.ToSingle(reader["GiaTri"]);
                    kiemTra.DonVi = reader["DonVi"] == DBNull.Value ? null : reader["DonVi"].ToString();
                    kiemTra.KetQua = reader["KetQua"].ToString();
                    kiemTra.GhiChu = reader["GhiChu"] == DBNull.Value ? null : reader["GhiChu"].ToString();
                    kiemTra.HinhAnh = reader["HinhAnh"] == DBNull.Value ? null : reader["HinhAnh"].ToString();

                    danhSachKiemTra.Add(kiemTra);
                }

                reader.Close();
                return danhSachKiemTra;
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

        // Lấy thông tin theo mã kiểm tra
        public KiemTraChatLuong_DTO GetKiemTraById(string maKiemTra)
        {
            string sql = @"SELECT kt.MaKiemTra, kt.LoaiKiemTra, kt.DoiTuongKiemTra, 
                       kt.NgayKiemTra, kt.NguoiKiemTra, nv.HoTen as TenNguoiKiemTra,
                       kt.TieuChiKiemTra, kt.GiaTri, kt.DonVi, kt.KetQua, kt.GhiChu, kt.HinhAnh
                FROM KiemTraChatLuong kt
                INNER JOIN NhanVien nv ON kt.NguoiKiemTra = nv.MaNhanVien
                WHERE kt.MaKiemTra = @MaKiemTra";

            try
            {
                Connect();
                SqlCommand cmd = new SqlCommand(sql, cn);
                cmd.Parameters.AddWithValue("@MaKiemTra", maKiemTra);
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    KiemTraChatLuong_DTO kiemTra = new KiemTraChatLuong_DTO();
                    kiemTra.MaKiemTra = reader["MaKiemTra"].ToString();
                    kiemTra.LoaiKiemTra = reader["LoaiKiemTra"].ToString();
                    kiemTra.DoiTuongKiemTra = reader["DoiTuongKiemTra"].ToString();
                    kiemTra.NgayKiemTra = Convert.ToDateTime(reader["NgayKiemTra"]);
                    kiemTra.NguoiKiemTra = Convert.ToInt32(reader["NguoiKiemTra"]);
                    kiemTra.TenNguoiKiemTra = reader["TenNguoiKiemTra"].ToString();

                    // Xử lý trường null
                    kiemTra.TieuChiKiemTra = reader["TieuChiKiemTra"] == DBNull.Value ? null : reader["TieuChiKiemTra"].ToString();
                    kiemTra.GiaTri = reader["GiaTri"] == DBNull.Value ? null : (float?)Convert.ToSingle(reader["GiaTri"]);
                    kiemTra.DonVi = reader["DonVi"] == DBNull.Value ? null : reader["DonVi"].ToString();
                    kiemTra.KetQua = reader["KetQua"].ToString();
                    kiemTra.GhiChu = reader["GhiChu"] == DBNull.Value ? null : reader["GhiChu"].ToString();
                    kiemTra.HinhAnh = reader["HinhAnh"] == DBNull.Value ? null : reader["HinhAnh"].ToString();

                    reader.Close();
                    return kiemTra;
                }

                reader.Close();
                return null;
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

        // Thêm phiếu kiểm tra chất lượng 
        public bool AddKiemTraChatLuong(KiemTraChatLuong_DTO kiemTra)
        {
            try
            {
                // Tạo mã kiểm tra mới
                kiemTra.MaKiemTra = CreateNewMa();
                Console.WriteLine("Ma kiem tra moi: " + kiemTra.MaKiemTra);

                // Sử dụng ExecuteNonQuery thay vì ExecuteScalar để thêm dữ liệu
                Connect();
                string sql = @"INSERT INTO KiemTraChatLuong (
                        MaKiemTra, LoaiKiemTra, DoiTuongKiemTra, NgayKiemTra, 
                        NguoiKiemTra, TieuChiKiemTra, GiaTri, DonVi, 
                        KetQua, GhiChu, HinhAnh)
                    VALUES (
                        @MaKiemTra, @LoaiKiemTra, @DoiTuongKiemTra, @NgayKiemTra, 
                        @NguoiKiemTra, @TieuChiKiemTra, @GiaTri, @DonVi, 
                        @KetQua, @GhiChu, @HinhAnh)";

                SqlCommand cmd = new SqlCommand(sql, cn);
                cmd.Parameters.AddWithValue("@MaKiemTra", kiemTra.MaKiemTra);
                cmd.Parameters.AddWithValue("@LoaiKiemTra", kiemTra.LoaiKiemTra);
                cmd.Parameters.AddWithValue("@DoiTuongKiemTra", kiemTra.DoiTuongKiemTra);
                cmd.Parameters.AddWithValue("@NgayKiemTra", kiemTra.NgayKiemTra);
                cmd.Parameters.AddWithValue("@NguoiKiemTra", kiemTra.NguoiKiemTra);

                // Xử lý các giá trị null
                if (string.IsNullOrWhiteSpace(kiemTra.TieuChiKiemTra))
                    cmd.Parameters.AddWithValue("@TieuChiKiemTra", DBNull.Value);
                else
                    cmd.Parameters.AddWithValue("@TieuChiKiemTra", kiemTra.TieuChiKiemTra);

                if (kiemTra.GiaTri.HasValue)
                    cmd.Parameters.AddWithValue("@GiaTri", kiemTra.GiaTri);
                else
                    cmd.Parameters.AddWithValue("@GiaTri", DBNull.Value);

                if (string.IsNullOrWhiteSpace(kiemTra.DonVi))
                    cmd.Parameters.AddWithValue("@DonVi", DBNull.Value);
                else
                    cmd.Parameters.AddWithValue("@DonVi", kiemTra.DonVi);

                cmd.Parameters.AddWithValue("@KetQua", kiemTra.KetQua);

                if (string.IsNullOrWhiteSpace(kiemTra.GhiChu))
                    cmd.Parameters.AddWithValue("@GhiChu", DBNull.Value);
                else
                    cmd.Parameters.AddWithValue("@GhiChu", kiemTra.GhiChu);

                if (string.IsNullOrWhiteSpace(kiemTra.HinhAnh))
                    cmd.Parameters.AddWithValue("@HinhAnh", DBNull.Value);
                else
                    cmd.Parameters.AddWithValue("@HinhAnh", kiemTra.HinhAnh);

                int result = cmd.ExecuteNonQuery();
                return result > 0;
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Loi SQL trong AddKiemTraChatLuong: " + ex.Message);
                Console.WriteLine("SQL Error Number: " + ex.Number);
                throw ex;
            }
            finally
            {
                DisConnect();
            }
        }

        // Cập nhật phiếu kiểm tra chất lượng
        public bool UpdateKiemTraChatLuong(KiemTraChatLuong_DTO kiemTra)
        {
            string sql = @"UPDATE KiemTraChatLuong SET 
                       LoaiKiemTra = @LoaiKiemTra, 
                       DoiTuongKiemTra = @DoiTuongKiemTra, 
                       NgayKiemTra = @NgayKiemTra, 
                       NguoiKiemTra = @NguoiKiemTra, 
                       TieuChiKiemTra = @TieuChiKiemTra, 
                       GiaTri = @GiaTri, 
                       DonVi = @DonVi, 
                       KetQua = @KetQua, 
                       GhiChu = @GhiChu, 
                       HinhAnh = @HinhAnh
                    WHERE MaKiemTra = @MaKiemTra";

            try
            {
                Connect();
                SqlCommand cmd = new SqlCommand(sql, cn);
                cmd.Parameters.AddWithValue("@MaKiemTra", kiemTra.MaKiemTra);
                cmd.Parameters.AddWithValue("@LoaiKiemTra", kiemTra.LoaiKiemTra);
                cmd.Parameters.AddWithValue("@DoiTuongKiemTra", kiemTra.DoiTuongKiemTra);
                cmd.Parameters.AddWithValue("@NgayKiemTra", kiemTra.NgayKiemTra);
                cmd.Parameters.AddWithValue("@NguoiKiemTra", kiemTra.NguoiKiemTra);

                // Xử lý các giá trị null
                if (string.IsNullOrWhiteSpace(kiemTra.TieuChiKiemTra))
                    cmd.Parameters.AddWithValue("@TieuChiKiemTra", DBNull.Value);
                else
                    cmd.Parameters.AddWithValue("@TieuChiKiemTra", kiemTra.TieuChiKiemTra);

                if (kiemTra.GiaTri.HasValue)
                    cmd.Parameters.AddWithValue("@GiaTri", kiemTra.GiaTri);
                else
                    cmd.Parameters.AddWithValue("@GiaTri", DBNull.Value);

                if (string.IsNullOrWhiteSpace(kiemTra.DonVi))
                    cmd.Parameters.AddWithValue("@DonVi", DBNull.Value);
                else
                    cmd.Parameters.AddWithValue("@DonVi", kiemTra.DonVi);

                cmd.Parameters.AddWithValue("@KetQua", kiemTra.KetQua);

                if (string.IsNullOrWhiteSpace(kiemTra.GhiChu))
                    cmd.Parameters.AddWithValue("@GhiChu", DBNull.Value);
                else
                    cmd.Parameters.AddWithValue("@GhiChu", kiemTra.GhiChu);

                if (string.IsNullOrWhiteSpace(kiemTra.HinhAnh))
                    cmd.Parameters.AddWithValue("@HinhAnh", DBNull.Value);
                else
                    cmd.Parameters.AddWithValue("@HinhAnh", kiemTra.HinhAnh);

                int result = cmd.ExecuteNonQuery();
                return result > 0;
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

        // Xóa phiếu kiểm tra chất lượng
        public bool DeleteKiemTraChatLuong(string maKiemTra)
        {
            string sql = @"DELETE FROM KiemTraChatLuong WHERE MaKiemTra = @MaKiemTra";
            try
            {
                Connect();
                SqlCommand cmd = new SqlCommand(sql, cn);
                cmd.Parameters.AddWithValue("@MaKiemTra", maKiemTra);
                int result = cmd.ExecuteNonQuery();
                return result > 0;
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

        // Tạo mã kiểm tra mới tự động 
        public string CreateNewMa()
        {
            try
            {
                Connect();

                // Sử dụng stored procedure để tạo mã mới
                SqlCommand cmd = new SqlCommand("SP_TaoMaKiemTraMoi", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                // Thực thi stored procedure và lấy kết quả từ SqlDataReader
                SqlDataReader reader = cmd.ExecuteReader();
                string newMaKiemTra = "KT001"; // Giá trị mặc định

                if (reader.Read())
                {
                    newMaKiemTra = reader["MaKiemTraMoi"].ToString();
                }

                reader.Close();
                return newMaKiemTra;
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

        // Kiểm tra nhân viên tồn tại
        public bool KiemTraNhanVienTonTai(int maNhanVien)
        {
            try
            {
                Connect();
                string sql = "SELECT COUNT(*) FROM NhanVien WHERE MaNhanVien = @MaNhanVien";

                SqlCommand cmd = new SqlCommand(sql, cn);
                cmd.Parameters.AddWithValue("@MaNhanVien", maNhanVien);

                int count = Convert.ToInt32(cmd.ExecuteScalar());
                return count > 0;
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