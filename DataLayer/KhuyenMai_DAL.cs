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
    public class KhuyenMai_DAL : DataProvider
    {
        public List<KhuyenMai_DTO> GetAllKhuyenMai()
        {
            List<KhuyenMai_DTO> list = new List<KhuyenMai_DTO>();
            string sql = "SELECT * FROM KhuyenMai";
            DataTable dt = ExecuteQuery(sql);

            foreach (DataRow row in dt.Rows)
            {
                list.Add(new KhuyenMai_DTO()
                {
                    MaKhuyenMai = Convert.ToInt32(row["MaKhuyenMai"]),
                    TenKhuyenMai = row["TenKhuyenMai"].ToString(),
                    NoiDung = row["NoiDung"].ToString(),
                    PhanTramGiam = Convert.ToSingle(row["PhanTramGiam"]),
                    NgayBatDau = Convert.ToDateTime(row["NgayBatDau"]),
                    NgayKetThuc = Convert.ToDateTime(row["NgayKetThuc"]),
                    TrangThai = Convert.ToBoolean(row["TrangThai"]),
                    GhiChu = row["GhiChu"].ToString()
                }); ;
            }

            return list;
        }

        public bool InsertKhuyenMai(KhuyenMai_DTO km)
        {
            string sql = "INSERT INTO KhuyenMai (TenKhuyenMai, NoiDung, PhanTramGiam, NgayBatDau, NgayKetThuc, TrangThai, GhiChu) " +
                         "VALUES (@TenKhuyenMai, @NoiDung, @PhanTramGiam, @NgayBatDau, @NgayKetThuc, @TrangThai, @GhiChu)";

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@TenKhuyenMai", km.TenKhuyenMai),
                new SqlParameter("@NoiDung", km.NoiDung),
                new SqlParameter("@PhanTramGiam", km.PhanTramGiam),
                new SqlParameter("@NgayBatDau", km.NgayBatDau),
                new SqlParameter("@NgayKetThuc", km.NgayKetThuc),
                new SqlParameter("@TrangThai", km.TrangThai),
                new SqlParameter("@GhiChu", km.GhiChu)
            };

            return ExecuteNonQuery(sql, parameters) > 0;
        }
        public bool UpdateTrangThai(int maKhuyenMai, bool trangThai)
        {
            string sql = "UPDATE KhuyenMai SET TrangThai = @TrangThai WHERE MaKhuyenMai = @MaKhuyenMai";

            SqlParameter[] parameters = new SqlParameter[]
            {
        new SqlParameter("@TrangThai", trangThai),
        new SqlParameter("@MaKhuyenMai", maKhuyenMai)
            };

            return ExecuteNonQuery(sql, parameters) > 0;
        }
        public bool UpdateKhuyenMai(KhuyenMai_DTO km)
        {
            string sql = @"UPDATE KhuyenMai SET 
                    TenKhuyenMai = @TenKhuyenMai,
                    NoiDung = @NoiDung,
                    PhanTramGiam = @PhanTramGiam,
                    NgayBatDau = @NgayBatDau,
                    NgayKetThuc = @NgayKetThuc,
                    TrangThai = @TrangThai,
                    GhiChu = @GhiChu
                   WHERE MaKhuyenMai = @MaKhuyenMai";

            // ✅ Bổ sung đầy đủ tham số, bao gồm @MaKhuyenMai
            SqlParameter[] parameters = new SqlParameter[]
            {
        new SqlParameter("@TenKhuyenMai", km.TenKhuyenMai),
        new SqlParameter("@NoiDung", km.NoiDung),
        new SqlParameter("@PhanTramGiam", km.PhanTramGiam),
        new SqlParameter("@NgayBatDau", km.NgayBatDau),
        new SqlParameter("@NgayKetThuc", km.NgayKetThuc),
        new SqlParameter("@TrangThai", km.TrangThai),
        new SqlParameter("@GhiChu", km.GhiChu),
        new SqlParameter("@MaKhuyenMai", km.MaKhuyenMai) // ⚠ BẮT BUỘC PHẢI CÓ
            };

            return ExecuteNonQuery(sql, parameters) > 0;
        }

        public bool DeleteKhuyenMai(string maKhuyenMai)
        {
            string sql = "DELETE FROM KhuyenMai WHERE MaKhuyenMai = @MaKhuyenMai";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@MaKhuyenMai", maKhuyenMai)
            };
            return ExecuteNonQuery(sql, parameters) > 0;
        }
    }
}