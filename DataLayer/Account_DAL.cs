using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using TransferObject;
using System.Security.Principal;

namespace DataLayer
{
    public class Account_DAL : DataProvider
    {
        //kiem tra dang nhap
        //tra ve thong tin tai khoan
        public Account_DTO Login(Account_DTO account)
        {
            try
            {
                string sql = "SELECT TenDangNhap, TenHienThi, MaLoai FROM TaiKhoan WHERE TenDangNhap = @Username AND MatKhau = @Password";
                SqlCommand cmd = new SqlCommand(sql, cn);
                cmd.Parameters.AddWithValue("@Username", account.Username);
                cmd.Parameters.AddWithValue("@Password", account.Password);
                Connect();

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    //tao doi tuong  Account_DTO voi data trong database
                    Account_DTO account_DTO = new Account_DTO(
                        reader["TenDangNhap"].ToString(),
                        account.Password, //giu nguyen mat khau
                        reader["TenHienThi"].ToString(),
                        Convert.ToInt32(reader["MaLoai"])
                    );
                    reader.Close();
                    return account_DTO;
                }
                else
                {
                    reader.Close();
                    return null; //khong ton tai tai khoan
                }
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


        //them tai khoan
        public bool AddAccount(Account_DTO account)
        {
            try
            {
                string sql = "INSERT INTO TaiKhoan (TenDangNhap, MatKhau, TenHienThi, MaLoai) VALUES (@Username, @Password, @DisplayName, @MaLoai)";
                SqlCommand cmd = new SqlCommand(sql, cn);
                cmd.Parameters.AddWithValue("@Username", account.Username);
                cmd.Parameters.AddWithValue("@Password", account.Password);
                cmd.Parameters.AddWithValue("@DisplayName", account.DisplayName);
                cmd.Parameters.AddWithValue("@MaLoai", account.Maloai);
                Connect();
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
        //kiem tra ton tai tai khoan
        public bool CheckAccountExists(string username)
        {
            try
            {
                string sql = "SELECT COUNT(*) FROM TaiKhoan WHERE TenDangNhap = @Username";
                SqlCommand cmd = new SqlCommand(sql, cn);
                cmd.Parameters.AddWithValue("@Username", username);
                Connect();
                int count = (int)cmd.ExecuteScalar();
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