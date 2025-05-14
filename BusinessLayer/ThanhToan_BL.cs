using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer;
using TransferObject;

namespace BusinessLayer
{
    public class ThanhToan_BL
    {
        private ThanhToan_DAL thanhToanDAL;

        public ThanhToan_BL()
        {
            thanhToanDAL = new ThanhToan_DAL();
        }
        public List<HinhThucTT_DTO> GetHinhThucTT()
        {
            return thanhToanDAL.GetHinhThucTT();
        }
        public List<ThanhToan_DTO> GetAll()
        {
            return thanhToanDAL.GetAll();
        }
        public bool Insert(ThanhToan_DTO thanhToan)
        {
            return thanhToanDAL.InsertThanhToan(thanhToan);
        }
        public List<HoaDonTT_DTO> GetHD()
        {
            return thanhToanDAL.GetHD();
        }

        public List<HoaDonTT_DTO> SearchOrders(int? id = null, DateTime? time = null, string nameBan = null, int? status = null)
        {
            return thanhToanDAL.SearchOrders(id, time, nameBan, status);
        }
    }
}
