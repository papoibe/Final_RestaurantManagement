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
    public class ChiTietPhieuNhap_BL
    {
        private ChiTietPhieuNhap_DAL chiTietDAL;

        public ChiTietPhieuNhap_BL()
        {
            chiTietDAL = new ChiTietPhieuNhap_DAL();
        }

        // Lấy chi tiết phiếu nhập theo mã
        public List<ChiTietPhieuNhap_DTO> GetChiTiet_ById(int maPhieuNhap)
        {
            try
            {
                return chiTietDAL.GetChiTiet_ById(maPhieuNhap);
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        // Thêm chi tiết phiếu nhập
        public bool AddChiTietPhieu(ChiTietPhieuNhap_DTO chiTiet)
        {
            try
            {
                // Tổng cộng = số lượng * đơn giá
                chiTiet.ThanhTien = chiTiet.SoLuong * chiTiet.DonGia;
                return chiTietDAL.AddChiTietPhieuNhap(chiTiet);
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        // cập nhật chi tiết phiếu nhập
        public bool UpdateChiTietPhieu(ChiTietPhieuNhap_DTO chiTiet)
        {
            try
            {
                //tinh toan thanh tien
                chiTiet.ThanhTien = chiTiet.SoLuong * chiTiet.DonGia;
                // goi phuong thuc tu dal de cap nhat du lieu
                return chiTietDAL.UpdateChiTietPhieuNhap(chiTiet);
            }
            catch (SqlException ex)
            {

                throw ex;
            }
        }



    }
}