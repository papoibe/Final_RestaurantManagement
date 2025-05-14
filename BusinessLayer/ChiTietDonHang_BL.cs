using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer;
using TransferObject;
namespace BusinessLayer
{
    public class ChiTietDonHang_BL
    {
        private ChiTietDonHang_DAL chiTietDonHangDAL;

        public ChiTietDonHang_BL()
        {
            chiTietDonHangDAL = new ChiTietDonHang_DAL();
        }

        public List<ChiTietDonHang_DTO> GetAll()
        {
            return chiTietDonHangDAL.GetAll();
        }

        public List<ChiTietDonHang_DTO> GetByMaDonHang(int maDonHang)
        {
            return chiTietDonHangDAL.GetByMaDonHang(maDonHang);
        }

        public bool Insert(ChiTietDonHang_DTO chiTiet)
        {
            return chiTietDonHangDAL.Insert(chiTiet);
        }

        public bool Change(ChiTietDonHang_DTO chiTiet)
        {
            return chiTietDonHangDAL.Change(chiTiet);
        }
        public bool Delete(ChiTietDonHang_DTO chiTiet)
        {
            return chiTietDonHangDAL.Delete(chiTiet);
        }
        public ChiTietDonHang_DTO GetChiTietDonHang(int maChiTiet)
        {
            return chiTietDonHangDAL.GetChiTietDonHang(maChiTiet);
        }
        public bool Update(ChiTietDonHang_DTO chiTiet)
        {
            return chiTietDonHangDAL.Update(chiTiet);
        }
        public bool UpdateMaDH(ChiTietDonHang_DTO chiTiet)
        {
            return chiTietDonHangDAL.UpdateMaDH(chiTiet);
        }
    }
}
