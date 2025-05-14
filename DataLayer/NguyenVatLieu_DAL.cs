using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransferObject;

namespace DataLayer
{
    public class NguyenVatLieu_DAL : DataProvider
    {
        // Lấy danh sách nguyên vật liệu
        public List<NguyenVatLieu_DTO> GetNguyenVatLieuList()
        {
            string sql = "SELECT * FROM NguyenVatLieu WHERE TrangThai = 1 ORDER BY TenNguyenLieu";
            List<NguyenVatLieu_DTO> nvlList = new List<NguyenVatLieu_DTO>();

            try
            {
                Connect();
                SqlDataReader reader = MyExecuteReader(sql, CommandType.Text);

                while (reader.Read())
                {
                    NguyenVatLieu_DTO nvl = new NguyenVatLieu_DTO();
                    nvl.MaNguyenLieu = Convert.ToInt32(reader["MaNguyenLieu"]);
                    nvl.TenNguyenLieu = reader["TenNguyenLieu"].ToString();
                    nvl.DonViTinh = reader["DonViTinh"].ToString();
                    nvl.SoLuongTon = Convert.ToSingle(reader["SoLuongTon"]);
                    nvl.MoTa = reader["MoTa"] == DBNull.Value ? null : reader["MoTa"].ToString();
                    nvl.TrangThai = Convert.ToBoolean(reader["TrangThai"]);

                    nvlList.Add(nvl);
                }

                reader.Close();
                return nvlList;
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

        // Lấy thông tin nguyên vật liệu theo mã
        public NguyenVatLieu_DTO GetNguyenVatLieuById(int maNguyenLieu)
        {
            string sql = "SELECT * FROM NguyenVatLieu WHERE MaNguyenLieu = @MaNguyenLieu";

            try
            {
                Connect();
                SqlCommand cmd = new SqlCommand(sql, cn);
                cmd.Parameters.AddWithValue("@MaNguyenLieu", maNguyenLieu);

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    NguyenVatLieu_DTO nvl = new NguyenVatLieu_DTO();
                    nvl.MaNguyenLieu = Convert.ToInt32(reader["MaNguyenLieu"]);
                    nvl.TenNguyenLieu = reader["TenNguyenLieu"].ToString();
                    nvl.DonViTinh = reader["DonViTinh"].ToString();
                    nvl.SoLuongTon = Convert.ToSingle(reader["SoLuongTon"]);
                    nvl.MoTa = reader["MoTa"] == DBNull.Value ? null : reader["MoTa"].ToString();
                    nvl.TrangThai = Convert.ToBoolean(reader["TrangThai"]);

                    reader.Close();
                    return nvl;
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

        // Cập nhật số lượng tồn
        public bool UpdateSoLuongTon(int maNguyenLieu, float soLuongMoi)
        {
            string sql = "UPDATE NguyenVatLieu SET SoLuongTon = @SoLuongTon WHERE MaNguyenLieu = @MaNguyenLieu";

            try
            {
                Connect();
                SqlCommand cmd = new SqlCommand(sql, cn);
                cmd.Parameters.AddWithValue("@MaNguyenLieu", maNguyenLieu);
                cmd.Parameters.AddWithValue("@SoLuongTon", soLuongMoi);

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
    }
}