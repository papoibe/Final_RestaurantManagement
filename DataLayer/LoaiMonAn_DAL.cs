using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using TransferObject;
namespace DataLayer
{
    public class LoaiMonAn_DAL : DataProvider
    {
        public List<LoaiMonAn_DTO> GetAll()
        {
            List<LoaiMonAn_DTO> lst = new List<LoaiMonAn_DTO>();
            string sql = "SELECT * FROM LoaiMonAn";
            SqlDataReader dr = MyExecuteReader(sql, System.Data.CommandType.Text);
            while (dr.Read())
            {
                LoaiMonAn_DTO loaiMonAn = new LoaiMonAn_DTO(dr.GetInt32(0), dr.GetString(1), dr.IsDBNull(2) ? string.Empty : dr.GetString(2));
                lst.Add(loaiMonAn);
            }
            dr.Close();
            return lst;
        }
    }

}
