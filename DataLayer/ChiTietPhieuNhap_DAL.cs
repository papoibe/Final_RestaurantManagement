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
    public class ChiTietPhieuNhap_DAL : DataProvider
    {
        // Lấy chi tiết phiếu nhập theo mã phiếu nhập
        // Trong file ChiTietPhieuNhap_DAL.cs
        public List<ChiTietPhieuNhap_DTO> GetChiTiet_ById(int maPhieuNhap)
        {
            // Lấy info chi tiết phiếu cùng với tên nguyên liệu
            string sql = @"SELECT ct.*, nv.TenNguyenLieu 
                 FROM ChiTietPhieuNhap ct
                 INNER JOIN NguyenVatLieu nv ON ct.MaNguyenLieu = nv.MaNguyenLieu
                 WHERE ct.MaPhieuNhap = @MaPhieuNhap";

            // Tạo list để lưu trữ kết quả chi tiết phiếu nhập
            List<ChiTietPhieuNhap_DTO> chiTietList = new List<ChiTietPhieuNhap_DTO>();

            try
            {
                Connect(); // ĐẢM BẢO KẾT NỐI ĐƯỢC MỞ TRƯỚC KHI THỰC HIỆN TRUY VẤN

                SqlCommand cmd = new SqlCommand(sql, cn);
                cmd.Parameters.AddWithValue("@MaPhieuNhap", maPhieuNhap);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    ChiTietPhieuNhap_DTO chiTiet = new ChiTietPhieuNhap_DTO();
                    chiTiet.MaChiTiet = Convert.ToInt32(reader["MaChiTiet"]);
                    chiTiet.MaPhieuNhap = Convert.ToInt32(reader["MaPhieuNhap"]);
                    chiTiet.MaNguyenLieu = Convert.ToInt32(reader["MaNguyenLieu"]);
                    chiTiet.TenNguyenLieu = reader["TenNguyenLieu"].ToString();
                    chiTiet.SoLuong = Convert.ToSingle(reader["SoLuong"]);
                    chiTiet.DonGia = Convert.ToSingle(reader["DonGia"]);
                    chiTiet.ThanhTien = Convert.ToSingle(reader["ThanhTien"]);

                    // Xử lý trường GhiChu có thể null
                    chiTiet.GhiChu = reader["GhiChu"] == DBNull.Value ? "" : reader["GhiChu"].ToString();

                    chiTietList.Add(chiTiet);
                }
                reader.Close();
                return chiTietList;
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

        // Thêm chi tiết phiếu nhập
        public bool AddChiTietPhieuNhap(ChiTietPhieuNhap_DTO chiTiet)
        {
            try
            {
                Connect();

                // Sử dụng kế thừa phương thức tiện ích để tìm ID trống nhỏ nhất DataProvider
                int newId = Find_Id_Update("ChiTietPhieuNhap", "MaChiTiet");

                // Tính thành tiền
                float thanhTien = chiTiet.SoLuong * chiTiet.DonGia;

                // Bật IDENTITY_INSERT để có thể chèn giá trị ID cụ thể
                SqlCommand cmdSetIdentityInsert = new SqlCommand("SET IDENTITY_INSERT ChiTietPhieuNhap ON", cn);
                cmdSetIdentityInsert.ExecuteNonQuery();

                // Tạo câu SQL với tham số
                string sql = @"INSERT INTO ChiTietPhieuNhap (MaChiTiet, MaPhieuNhap, MaNguyenLieu, SoLuong, DonGia, ThanhTien, GhiChu)
                    VALUES (@MaChiTiet, @MaPhieuNhap, @MaNguyenLieu, @SoLuong, @DonGia, @ThanhTien, @GhiChu)";

                SqlCommand cmd = new SqlCommand(sql, cn);
                cmd.Parameters.AddWithValue("@MaChiTiet", newId);
                cmd.Parameters.AddWithValue("@MaPhieuNhap", chiTiet.MaPhieuNhap);
                cmd.Parameters.AddWithValue("@MaNguyenLieu", chiTiet.MaNguyenLieu);
                cmd.Parameters.AddWithValue("@SoLuong", chiTiet.SoLuong);
                cmd.Parameters.AddWithValue("@DonGia", chiTiet.DonGia);
                cmd.Parameters.AddWithValue("@ThanhTien", thanhTien);

    
                // Xử lý chuỗi rỗng là NULL
                if (string.IsNullOrWhiteSpace(chiTiet.GhiChu))
                    cmd.Parameters.AddWithValue("@GhiChu", DBNull.Value);
                else
                    cmd.Parameters.AddWithValue("@GhiChu", chiTiet.GhiChu);

                // Thực thi câu lệnh
                int rowsAffected = cmd.ExecuteNonQuery();

                // Tắt IDENTITY_INSERT
                SqlCommand cmdUnsetIdentityInsert = new SqlCommand("SET IDENTITY_INSERT ChiTietPhieuNhap OFF", cn);
                cmdUnsetIdentityInsert.ExecuteNonQuery();

                // Nếu thêm thành công, cập nhật số lượng tồn trong bảng NguyenVatLieu
                if (rowsAffected > 0)
                {
                    // Cập nhật tổng tiền phiếu nhập
                    UpdateTongTienPhieu(chiTiet.MaPhieuNhap);

                    // Cập nhật số lượng tồn nguyên vật liệu
                    UpdateSoLuongTonNVL(chiTiet.MaNguyenLieu, chiTiet.SoLuong);

                    return true;
                }
                return false;
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

        // Cập nhật tổng tiền phiếu nhập
        private void UpdateTongTienPhieu(int maPhieuNhap)
        {
            try
            {
                string sql = @"UPDATE PhieuNhapKho 
                            SET TongTien = (SELECT SUM(ThanhTien) FROM ChiTietPhieuNhap WHERE MaPhieuNhap = @MaPhieuNhap)
                            WHERE MaPhieuNhap = @MaPhieuNhap";

                SqlCommand cmd = new SqlCommand(sql, cn);
                cmd.Parameters.AddWithValue("@MaPhieuNhap", maPhieuNhap);
                cmd.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        // Cập nhật số lượng tồn nguyên vật liệu
        private void UpdateSoLuongTonNVL(int maNguyenLieu, float soLuong)
        {
            try
            {
                string sql = @"UPDATE NguyenVatLieu 
                            SET SoLuongTon = SoLuongTon + @SoLuong
                            WHERE MaNguyenLieu = @MaNguyenLieu";

                SqlCommand cmd = new SqlCommand(sql, cn);
                cmd.Parameters.AddWithValue("@MaNguyenLieu", maNguyenLieu);
                cmd.Parameters.AddWithValue("@SoLuong", soLuong);
                cmd.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        // Cập nhật chi tiết phiếu nhập
        public bool UpdateChiTietPhieuNhap(ChiTietPhieuNhap_DTO chiTiet)
        {
            try
            {
                Connect();
                // Tính thành tiền
                float thanhTien = chiTiet.SoLuong * chiTiet.DonGia;
                // Tạo câu SQL với tham số
                string sql = @"UPDATE ChiTietPhieuNhap 
                            SET MaNguyenLieu = @MaNguyenLieu, SoLuong = @SoLuong, DonGia = @DonGia, ThanhTien = @ThanhTien, GhiChu = @GhiChu
                            WHERE MaChiTiet = @MaChiTiet";
                SqlCommand cmd = new SqlCommand(sql, cn);
                cmd.Parameters.AddWithValue("@MaChiTiet", chiTiet.MaChiTiet);
                cmd.Parameters.AddWithValue("@MaNguyenLieu", chiTiet.MaNguyenLieu);
                cmd.Parameters.AddWithValue("@SoLuong", chiTiet.SoLuong);
                cmd.Parameters.AddWithValue("@DonGia", chiTiet.DonGia);
                cmd.Parameters.AddWithValue("@ThanhTien", thanhTien);
                // Xử lý chuỗi rỗng là NULL
                if (string.IsNullOrWhiteSpace(chiTiet.GhiChu))
                    cmd.Parameters.AddWithValue("@GhiChu", DBNull.Value);
                else
                    cmd.Parameters.AddWithValue("@GhiChu", chiTiet.GhiChu);
                // Thực thi câu lệnh
                int rowsAffected = cmd.ExecuteNonQuery();
                // Nếu cập nhật thành công, cập nhật số lượng tồn trong bảng NguyenVatLieu
                if (rowsAffected > 0)
                {
                    // Cập nhật tổng tiền phiếu nhập
                    UpdateTongTienPhieu(chiTiet.MaPhieuNhap);
                    // Cập nhật số lượng tồn nguyên vật liệu
                    UpdateSoLuongTonNVL(chiTiet.MaNguyenLieu, chiTiet.SoLuong);
                    return true;
                }
                return false;
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