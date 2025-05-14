using System.Collections.Generic;
using DataLayer;
using TransferObject;
using System.Data;

namespace BusinessLayer
{
    public class HoaDonThanhToan_BL
    {
        private HoaDonThanhToan_DAL hoaDonThanhToanDAL;

        public HoaDonThanhToan_BL()
        {
            hoaDonThanhToanDAL = new HoaDonThanhToan_DAL();
        }
        public List<HoaDonThanhToan_DTO> GetAll()
        {
            return hoaDonThanhToanDAL.GetAll();
        }

        public HoaDonThanhToan_DTO GetById(int maHoaDon)
        {
            return hoaDonThanhToanDAL.GetById(maHoaDon);
        }

        public bool Insert(HoaDonThanhToan_DTO hoaDon)
        {
            return hoaDonThanhToanDAL.Insert(hoaDon);
        }

        public bool Update(HoaDonThanhToan_DTO hoaDon)
        {
            return hoaDonThanhToanDAL.Update(hoaDon);
        }
    }
}