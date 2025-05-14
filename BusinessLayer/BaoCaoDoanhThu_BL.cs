using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using DataLayer;
using TransferObject;


namespace BusinessLayer
{
    public class BaoCaoDoanhThu_BL
    {
        private BaoCaoDoanhThu_DAL baoCaoDoanhThuDAL;

        public BaoCaoDoanhThu_BL()
        {
            baoCaoDoanhThuDAL = new BaoCaoDoanhThu_DAL();
        }

        public DataTable GetBaoCaoDoanhThuTheoNgay(DateTime ngayBatDau, DateTime ngayKetThuc)
        {
            try
            {
                return baoCaoDoanhThuDAL.GetBaoCaoDoanhThuTheoNgay(ngayBatDau, ngayKetThuc);
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataTable GetBaoCaoDoanhThuTheoThang(int nam)
        {
            try
            {
                return baoCaoDoanhThuDAL.GetBaoCaoDoanhThuTheoThang(nam);
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public DataTable GetBaoCaoDoanhThuTheoLoaiMonAn(DateTime ngayBatDau, DateTime ngayKetThuc)
        {
            try
            {
                return baoCaoDoanhThuDAL.GetBaoCaoDoanhThuTheoLoaiMonAn(ngayBatDau, ngayKetThuc);
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }


        public DataTable GetBaoCaoTonKho()
        {
            try
            {
                return baoCaoDoanhThuDAL.GetBaoCaoTonKho();
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }


        public int LuuBaoCaoDoanhThu(BaoCaoDoanhThu baoCao)
        {
            try
            {
                return baoCaoDoanhThuDAL.LuuBaoCaoDoanhThu(baoCao);
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }




    }
}




