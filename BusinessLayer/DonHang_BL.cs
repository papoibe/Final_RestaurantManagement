using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer;
using TransferObject;
namespace BusinessLayer
{
    public class DonHang_BL
    {
        private DonHang_DAL donHangDAL;
        public DonHang_BL()
        {
            donHangDAL = new DonHang_DAL();
        }
        public List<DonHang_DTO> GetAll()
        {
            return donHangDAL.GetAll();
        }
        public DonHang_DTO GetById(int maDonHang)
        {
            return donHangDAL.GetById(maDonHang);
        }
        public bool Insert(DonHang_DTO donHang)
        {
            return donHangDAL.Insert(donHang);
        }
        public bool Update(DonHang_DTO donHang)
        {
            return donHangDAL.Update(donHang);
        }
        public int GetLastId(int Maban)
        {
            return donHangDAL.GetLastId(Maban);
        }
        public float GetTongTien(int maDonHang)
        {
            return donHangDAL.GetTongTien(maDonHang);
        }
    }
}
