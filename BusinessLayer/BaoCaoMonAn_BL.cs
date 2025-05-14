using DataLayer;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransferObject;

namespace BusinessLayer
{
    public class BaoCaoMonAn_BL
    {
        private BaoCaoMonAn_DAL baoCaoMonAnDAL;
        public BaoCaoMonAn_BL()
        {
            baoCaoMonAnDAL = new BaoCaoMonAn_DAL();
        }

        public DataTable GetBaoCaoMonAnBanChay(DateTime ngayBatDau, DateTime ngayKetThuc, int soLuong = 10)
        {
            try
            {
                return baoCaoMonAnDAL.GetBaoCaoMonAnBanChay(ngayBatDau, ngayKetThuc, soLuong);
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public int LuuBaoCaoMonAn(BaoCaoMonAn baoCao)
        {
            try
            {
                return baoCaoMonAnDAL.LuuBaoCaoMonAn(baoCao);
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
    }
}
