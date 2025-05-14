using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransferObject;
using System.Data;
using System.Data.SqlClient;

namespace DataLayer
{
    public class PhieuNhapKho_DAL : DataProvider
    {
        // Lấy danh sách phiếu nhập
        public List<PhieuNhap_DTO> GetPhieuNhapList()
        {
            // Lấy tất cả các bảng từ bảng PhieuNhapKho và sắp xếp giảm dần theo ngày
            string sql = "SELECT * FROM PhieuNhapKho ORDER BY NgayNhap DESC";
            // Danh sách
            List<PhieuNhap_DTO> phieuNhapList = new List<PhieuNhap_DTO>();

            try
            {
                Connect();
                SqlDataReader reader = MyExecuteReader(sql, CommandType.Text);

                while (reader.Read())
                {
                    // Tạo đối tượng
                    PhieuNhap_DTO phieuNhap = new PhieuNhap_DTO();
                    phieuNhap.MaPhieuNhap = Convert.ToInt32(reader["MaPhieuNhap"]);
                    phieuNhap.MaNhanVien = Convert.ToInt32(reader["MaNhanVien"]);
                    phieuNhap.NgayNhap = Convert.ToDateTime(reader["NgayNhap"]);

                    // Xử lý trường NhaCungCap có thể null
                    phieuNhap.NhaCungCap = reader["NhaCungCap"] == DBNull.Value ? null : reader["NhaCungCap"].ToString();

                    // Xử lý trường TongTien có thể null
                    phieuNhap.TongTien = reader["TongTien"] == DBNull.Value ? 0 : Convert.ToSingle(reader["TongTien"]);

                    // Xử lý trường GhiChu có thể null
                    phieuNhap.GhiChu = reader["GhiChu"] == DBNull.Value ? null : reader["GhiChu"].ToString();

                    phieuNhap.TrangThai = Convert.ToBoolean(reader["TrangThai"]);

                    // Thêm đối tượng PhieuNhap_DTO vào danh sách phieuNhapList
                    phieuNhapList.Add(phieuNhap);
                }

                reader.Close();
                return phieuNhapList;
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

        // Lấy thông tin phiếu nhập theo mã
        public PhieuNhap_DTO GetPhieuNhapById(int maPhieuNhap)
        {
            string sql = "SELECT * FROM PhieuNhapKho WHERE MaPhieuNhap = @MaPhieuNhap";
            PhieuNhap_DTO phieuNhap = null;

            try
            {
                Connect();
                SqlCommand cmd = new SqlCommand(sql, cn);
                cmd.Parameters.AddWithValue("@MaPhieuNhap", maPhieuNhap);
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    phieuNhap = new PhieuNhap_DTO();
                    phieuNhap.MaPhieuNhap = Convert.ToInt32(reader["MaPhieuNhap"]);
                    phieuNhap.MaNhanVien = Convert.ToInt32(reader["MaNhanVien"]);
                    phieuNhap.NgayNhap = Convert.ToDateTime(reader["NgayNhap"]);

                    // Xử lý trường NhaCungCap có thể null
                    phieuNhap.NhaCungCap = reader["NhaCungCap"] == DBNull.Value ? null : reader["NhaCungCap"].ToString();

                    // Xử lý trường TongTien có thể null
                    phieuNhap.TongTien = reader["TongTien"] == DBNull.Value ? 0 : Convert.ToSingle(reader["TongTien"]);

                    // Xử lý trường GhiChu có thể null
                    phieuNhap.GhiChu = reader["GhiChu"] == DBNull.Value ? null : reader["GhiChu"].ToString();

                    phieuNhap.TrangThai = Convert.ToBoolean(reader["TrangThai"]);
                }

                reader.Close();
                return phieuNhap;
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

        // Thêm phiếu nhập
        public int AddPhieuNhap(PhieuNhap_DTO phieuNhap)
        {
            try
            {
                Connect();

                // Sử dụng kế thừa phương thức tiện ích để tìm ID trống nhỏ nhất DataProvider
                int newId = Find_Id_Update("PhieuNhapKho", "MaPhieuNhap");

                // Bật IDENTITY_INSERT để có thể chèn giá trị ID cụ thể
                SqlCommand cmdSetIdentityInsert = new SqlCommand("SET IDENTITY_INSERT PhieuNhapKho ON", cn);
                cmdSetIdentityInsert.ExecuteNonQuery();

                // Tạo câu SQL với ID cụ thể
                string sql = @"INSERT INTO PhieuNhapKho (MaPhieuNhap, MaNhanVien, NgayNhap, NhaCungCap, GhiChu, TrangThai) 
            VALUES (@MaPhieuNhap, @MaNhanVien, @NgayNhap, @NhaCungCap, @GhiChu, @TrangThai)";

                SqlCommand cmd = new SqlCommand(sql, cn);
                cmd.Parameters.AddWithValue("@MaPhieuNhap", newId);
                cmd.Parameters.AddWithValue("@MaNhanVien", phieuNhap.MaNhanVien);
                cmd.Parameters.AddWithValue("@NgayNhap", phieuNhap.NgayNhap);

                // Xử lý giá trị null
                if (string.IsNullOrWhiteSpace(phieuNhap.NhaCungCap))
                    cmd.Parameters.AddWithValue("@NhaCungCap", DBNull.Value);
                else
                    cmd.Parameters.AddWithValue("@NhaCungCap", phieuNhap.NhaCungCap);

                if (string.IsNullOrWhiteSpace(phieuNhap.GhiChu))
                    cmd.Parameters.AddWithValue("@GhiChu", DBNull.Value);
                else
                    cmd.Parameters.AddWithValue("@GhiChu", phieuNhap.GhiChu);

                cmd.Parameters.AddWithValue("@TrangThai", phieuNhap.TrangThai);

                cmd.ExecuteNonQuery();

                // Tắt IDENTITY_INSERT
                SqlCommand cmdUnsetIdentityInsert = new SqlCommand("SET IDENTITY_INSERT PhieuNhapKho OFF", cn);
                cmdUnsetIdentityInsert.ExecuteNonQuery();

                return newId;
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Lỗi SQL: " + ex.Message);
                throw ex;
            }
            finally
            {
                DisConnect();
            }
        }

        // Cập nhật phiếu nhập, 
        public bool UpdatePhieuNhap(PhieuNhap_DTO phieuNhap)
        {
            string sql = "UPDATE PhieuNhapKho SET NgayNhap = @NgayNhap, NhaCungCap = @NhaCungCap, GhiChu = @GhiChu WHERE MaPhieuNhap = @MaPhieuNhap";

                try
                {
                    Connect();
                    SqlCommand cmd = new SqlCommand(sql, cn);
                    cmd.Parameters.AddWithValue("@MaPhieuNhap", phieuNhap.MaPhieuNhap);
                    cmd.Parameters.AddWithValue("@MaNhanVien", phieuNhap.MaNhanVien);
                    cmd.Parameters.AddWithValue("@NgayNhap", phieuNhap.NgayNhap);
        
                    // Xử lý chuỗi rỗng là NULL
                    if (string.IsNullOrWhiteSpace(phieuNhap.NhaCungCap))
                        cmd.Parameters.AddWithValue("@NhaCungCap", DBNull.Value);
                    else
                        cmd.Parameters.AddWithValue("@NhaCungCap", phieuNhap.NhaCungCap);

                    // Xử lý chuỗi rỗng là NULL
                    if (string.IsNullOrWhiteSpace(phieuNhap.GhiChu))
                        cmd.Parameters.AddWithValue("@GhiChu", DBNull.Value);
                    else
                        cmd.Parameters.AddWithValue("@GhiChu", phieuNhap.GhiChu);

                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
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

        // Cập nhật trạng thái phiếu nhập
        public bool Update_TrangThaiPhieu(int maPhieuNhap, bool trangThai)
        {
            // Update trạng thái dựa trên mã phiếu nhập
            string sql = "UPDATE PhieuNhapKho SET TrangThai = @TrangThai WHERE MaPhieuNhap = @MaPhieuNhap";
            try
            {
                Connect();
                SqlCommand cmd = new SqlCommand(sql, cn);
                cmd.Parameters.AddWithValue("@TrangThai", trangThai); // Trạng thái muốn cập nhật
                cmd.Parameters.AddWithValue("@MaPhieuNhap", maPhieuNhap); // Mã phiếu nhập để xác định dòng cần cập nhật 
                int rowsAffected = cmd.ExecuteNonQuery(); // Lấy số dòng bị ảnh hưởng
                return rowsAffected > 0;  // Trả về true nếu có 1 dòng bị ảnh hưởng
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

        // Xóa phiếu nhập
        public bool DeletePhieuNhap(int maPhieuNhap)
        {
            try
            {
                Connect();

                // Đầu tiên, xóa các chi tiết phiếu nhập
                string sqlDeleteChiTiet = "DELETE FROM ChiTietPhieuNhap WHERE MaPhieuNhap = @MaPhieuNhap";
                SqlCommand cmdDeleteChiTiet = new SqlCommand(sqlDeleteChiTiet, cn);
                cmdDeleteChiTiet.Parameters.AddWithValue("@MaPhieuNhap", maPhieuNhap);
                cmdDeleteChiTiet.ExecuteNonQuery();

                // Sau đó, xóa phiếu nhập
                string sqlDeletePhieu = "DELETE FROM PhieuNhapKho WHERE MaPhieuNhap = @MaPhieuNhap";
                SqlCommand cmdDeletePhieu = new SqlCommand(sqlDeletePhieu, cn);
                cmdDeletePhieu.Parameters.AddWithValue("@MaPhieuNhap", maPhieuNhap);
                int result = cmdDeletePhieu.ExecuteNonQuery();

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

        // phuong thuc reset gia tri IDENTITY cua PhieuNhapKho sau khi xoa data
        // Trong PhieuNhapKho_DAL.cs
        public bool ResetIdentity()
        {
            try
            {
                Connect();

                // Kiểm tra xem bảng có trống không
                string sqlCheck = "SELECT COUNT(*) FROM PhieuNhapKho";
                SqlCommand cmdCheck = new SqlCommand(sqlCheck, cn);
                int count = Convert.ToInt32(cmdCheck.ExecuteScalar());

                if (count == 0)
                {
                    // Nếu bảng trống, thực hiện reset IDENTITY
                    string sqlReset = "DBCC CHECKIDENT ('PhieuNhapKho', RESEED, 0)";
                    SqlCommand cmdReset = new SqlCommand(sqlReset, cn);
                    cmdReset.ExecuteNonQuery();

                    // Reset IDENTITY cho bảng ChiTietPhieuNhap
                    string sqlResetChiTiet = "DBCC CHECKIDENT ('ChiTietPhieuNhap', RESEED, 0)";
                    SqlCommand cmdResetChiTiet = new SqlCommand(sqlResetChiTiet, cn);
                    cmdResetChiTiet.ExecuteNonQuery();

                    return true;
                }

                return false; // Không reset được vì bảng không trống
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
                // Tạo SQL query kiểm tra nhân viên
                string sql = "SELECT COUNT(*) FROM NhanVien WHERE MaNhanVien = @MaNhanVien";

                SqlCommand cmd = new SqlCommand(sql, cn);
                cmd.Parameters.AddWithValue("@MaNhanVien", maNhanVien);

                // Thực thi truy vấn và lấy kết quả
                int count = Convert.ToInt32(cmd.ExecuteScalar());

                // Trả về true nếu có ít nhất 1 nhân viên với mã đó
                return count > 0;
            }
            catch (SqlException ex)
            {
                // Ném ngoại lệ để xử lý ở lớp cao hơn
                throw ex;
            }
            finally
            {
                DisConnect();
            }
        }

    }
}