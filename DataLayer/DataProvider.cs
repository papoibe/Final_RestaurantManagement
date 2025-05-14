using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace DataLayer
{
    public class DataProvider
    {
        protected SqlConnection cn;

        public DataProvider()
        {
            //connect to database mỗi người là khác nhau nên vào tool sửa tên lại 
            string cnStr = "Data Source=LAPTOP-960OHC3A\\THINH;Initial Catalog=QuanLyQuanAn;Integrated Security=True";
            cn = new SqlConnection(cnStr);
        }

        public void Connect()
        {
            try
            {
                if (cn != null && cn.State == ConnectionState.Closed)
                {
                    cn.Open();
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public void DisConnect()
        {
            try
            {
                if (cn != null && cn.State == ConnectionState.Open)
                {
                    cn.Close();
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public object MyExecuteScalar(string sql, CommandType type)
        {
            try
            {
                Connect();
                SqlCommand cmd = new SqlCommand(sql, cn);
                cmd.CommandType = type;
                return cmd.ExecuteScalar();
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




        // Cải tiến phương thức MyExecuteScalar để đảm bảo kết nối được mở
        public object MyExecuteScalar(string sql, CommandType type, SqlParameter[] parameters = null)
        {
            try
            {
                // Đảm bảo kết nối được mở
                Connect();

                SqlCommand cmd = new SqlCommand(sql, cn);
                cmd.CommandType = type;

                if (parameters != null)
                    cmd.Parameters.AddRange(parameters);

                return cmd.ExecuteScalar();
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

        public SqlDataReader MyExecuteReader(string sql, CommandType type)
        {
            try
            {
                // Đảm bảo kết nối được mở
                Connect();

                SqlCommand cmd = new SqlCommand(sql, cn);
                cmd.CommandType = type;

                // CommandBehavior.CloseConnection đảm bảo kết nối sẽ đóng khi SqlDataReader được đóng
                return cmd.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        //PHƯƠNG THỨC mYEXECUTEREADER CÓ THAM SỐ ĐẦU VÀO LÀ MẢNG THAM SỐ
        public SqlDataReader MyExecuteReader(string sql, CommandType type, SqlParameter[] parameters = null)
        {
            try
            {
                // Đảm bảo kết nối được mở
                Connect();
                SqlCommand cmd = new SqlCommand(sql, cn);
                cmd.CommandType = type;
                if (parameters != null)
                    cmd.Parameters.AddRange(parameters);
                // CommandBehavior.CloseConnection đảm bảo kết nối sẽ đóng khi SqlDataReader được đóng
                return cmd.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public DataTable ExecuteQuery(string sql, SqlParameter[] parameters = null)
        {
            DataTable dt = new DataTable();
            try
            {
                Connect();
                SqlCommand cmd = new SqlCommand(sql, cn);
                cmd.CommandType = CommandType.Text;

                if (parameters != null)
                    cmd.Parameters.AddRange(parameters);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
                DisConnect();
            }
            return dt;
        }

        public int ExecuteNonQuery(string sql, SqlParameter[] parameters = null)
        {
            try
            {
                Connect();
                SqlCommand cmd = new SqlCommand(sql, cn);
                cmd.CommandType = CommandType.Text;

                if (parameters != null)
                    cmd.Parameters.AddRange(parameters);

                return cmd.ExecuteNonQuery();
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


        //phương thức cập nhập các mã tự động tăng giảm tìm ID trống nhỏ nhất
        public int Find_Id_Update(string tableName, string idColumnName)
        {
            try
            {
                // Đảm bảo kết nối được mở
                if (cn.State != ConnectionState.Open)
                    Connect();

                string sql = $@"
                DECLARE @id INT = 1;
                WHILE EXISTS (SELECT 1 FROM {tableName} WHERE {idColumnName} = @id)
                BEGIN
                    SET @id = @id + 1;
                END
                SELECT @id;";

                SqlCommand cmd = new SqlCommand(sql, cn);
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }




    }
}