using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using TransferObject;

namespace DataLayer
{
    public class NhanVien_DAL : DataProvider
    {
        private readonly Account_DAL _accountDal = new Account_DAL();

        // Lấy toàn bộ danh sách nhân viên
        public List<NhanVien_DTO> GetAll()
        {
            string query = @"
                SELECT nv.MaNhanVien, nv.HoTen, nv.GioiTinh, nv.NgaySinh, nv.DiaChi, nv.SDT,
                       nv.Email, nv.NgayVaoLam, nv.MaTaiKhoan, nv.TrangThai,
                       tk.TenDangNhap, tk.MatKhau, tk.TenHienThi, tk.MaLoai
                FROM NhanVien nv
                INNER JOIN TaiKhoan tk ON nv.MaTaiKhoan = tk.MaTaiKhoan";

            DataTable dt = ExecuteQuery(query);
            List<NhanVien_DTO> list = new List<NhanVien_DTO>();
            foreach (DataRow row in dt.Rows)
            {
                list.Add(MapDataRowToNhanVien(row));
            }
            return list;
        }

        private NhanVien_DTO MapDataRowToNhanVien(DataRow row)
        {
            return new NhanVien_DTO()
            {
                MaNhanVien = Convert.ToInt32(row["MaNhanVien"]),
                HoTen = row["HoTen"].ToString(),
                GioiTinh = row["GioiTinh"].ToString(),
                NgaySinh = Convert.ToDateTime(row["NgaySinh"]),
                DiaChi = row["DiaChi"].ToString(),
                SDT = row["SDT"].ToString(),
                Email = row["Email"].ToString(),
                NgayVaoLam = Convert.ToDateTime(row["NgayVaoLam"]),
                MaTaiKhoan = Convert.ToInt32(row["MaTaiKhoan"]),
                TrangThai = Convert.ToBoolean(row["TrangThai"]),
                Username = row["TenDangNhap"].ToString(),
                Password = row["MatKhau"].ToString(),
                DisplayName = row["TenHienThi"].ToString(),
                MaLoai = Convert.ToInt32(row["MaLoai"])
            };
        }

        public bool InsertNhanVien(NhanVien_DTO nv)
        {
            try
            {
                // Kiểm tra tên đăng nhập đã tồn tại chưa
                if (_accountDal.CheckAccountExists(nv.Username))
                    throw new Exception("Tên đăng nhập đã tồn tại. Vui lòng chọn tên khác.");

                // Thêm tài khoản
                string insertTaiKhoan = @"
                    INSERT INTO TaiKhoan (TenDangNhap, MatKhau, TenHienThi, MaLoai, TrangThai)
                    VALUES (@Username, @Password, @DisplayName, 2, 1);
                    SELECT SCOPE_IDENTITY();";
                SqlParameter[] taiKhoanParams = new SqlParameter[]
                {
                    new SqlParameter("@Username", nv.Username),
                    new SqlParameter("@Password", nv.Password),
                    new SqlParameter("@DisplayName", nv.DisplayName)
                };
                object result = MyExecuteScalar(insertTaiKhoan, CommandType.Text, taiKhoanParams);
                if (result == null || result == DBNull.Value)
                    return false;
                int newTaiKhoanId = Convert.ToInt32(result);

                // Thêm nhân viên (để MaNhanVien tự động tăng)
                string insertNhanVien = @"
                    INSERT INTO NhanVien (HoTen, GioiTinh, NgaySinh, DiaChi, SDT, Email, NgayVaoLam, MaTaiKhoan, TrangThai)
                    VALUES (@HoTen, @GioiTinh, @NgaySinh, @DiaChi, @SDT, @Email, @NgayVaoLam, @MaTaiKhoan, 1)";
                SqlParameter[] nhanVienParams = new SqlParameter[]
                {
                    new SqlParameter("@HoTen", nv.HoTen),
                    new SqlParameter("@GioiTinh", nv.GioiTinh),
                    new SqlParameter("@NgaySinh", nv.NgaySinh),
                    new SqlParameter("@DiaChi", nv.DiaChi),
                    new SqlParameter("@SDT", nv.SDT),
                    new SqlParameter("@Email", nv.Email),
                    new SqlParameter("@NgayVaoLam", nv.NgayVaoLam),
                    new SqlParameter("@MaTaiKhoan", newTaiKhoanId)
                };
                int rowsAffected = ExecuteNonQuery(insertNhanVien, nhanVienParams);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in InsertNhanVien: " + ex.Message);
                return false;
            }
        }

        public bool UpdateNhanVien(NhanVien_DTO nv)
        {
            try
            {
                // Cập nhật tài khoản
                string updateTaiKhoan = @"
                    UPDATE TaiKhoan
                    SET TenDangNhap = @Username, MatKhau = @Password, TenHienThi = @DisplayName
                    WHERE MaTaiKhoan = @MaTaiKhoan";
                SqlParameter[] taiKhoanParams = new SqlParameter[]
                {
                    new SqlParameter("@Username", nv.Username),
                    new SqlParameter("@Password", nv.Password),
                    new SqlParameter("@DisplayName", nv.DisplayName),
                    new SqlParameter("@MaTaiKhoan", nv.MaTaiKhoan)
                };
                ExecuteNonQuery(updateTaiKhoan, taiKhoanParams);

                // Cập nhật nhân viên
                string updateNhanVien = @"
                    UPDATE NhanVien
                    SET HoTen = @HoTen, GioiTinh = @GioiTinh, NgaySinh = @NgaySinh, DiaChi = @DiaChi, SDT = @SDT, Email = @Email, NgayVaoLam = @NgayVaoLam
                    WHERE MaNhanVien = @MaNhanVien";
                SqlParameter[] nhanVienParams = new SqlParameter[]
                {
                    new SqlParameter("@HoTen", nv.HoTen),
                    new SqlParameter("@GioiTinh", nv.GioiTinh),
                    new SqlParameter("@NgaySinh", nv.NgaySinh),
                    new SqlParameter("@DiaChi", nv.DiaChi),
                    new SqlParameter("@SDT", nv.SDT),
                    new SqlParameter("@Email", nv.Email),
                    new SqlParameter("@NgayVaoLam", nv.NgayVaoLam),
                    new SqlParameter("@MaNhanVien", nv.MaNhanVien)
                };
                int rowsAffected = ExecuteNonQuery(updateNhanVien, nhanVienParams);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in UpdateNhanVien: " + ex.Message);
                return false;
            }
        }

        public bool XoaNhanVien(int maNV, int maTK)
        {
            string query1 = $"DELETE FROM NhanVien WHERE MaNhanVien = {maNV}";
            string query2 = $"DELETE FROM TaiKhoan WHERE MaTaiKhoan = {maTK}";
            return ExecuteNonQuery(query1) > 0 && ExecuteNonQuery(query2) > 0;
        }
    }
}
