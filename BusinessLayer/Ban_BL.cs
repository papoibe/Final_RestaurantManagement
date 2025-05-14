using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer;
using TransferObject;
namespace BusinessLayer
{
    public class Ban_BL
    {
        private Ban_DAL banDAL;
        public Ban_BL()
        {
            banDAL = new Ban_DAL();
        }

        public List<Ban_DTO> GetAll()
        {
            try
            {
                return banDAL.GetAll();
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public bool CapNhatBan(Ban_DTO ban)
        {
            return banDAL.CapNhatBan(ban);
        }
        public bool CapNhatTrangThaiBan(int maBan, bool trangThai)
        {
            return banDAL.CapNhatTrangThaiBan(maBan, trangThai);
        }
        public bool GetTrangThaiBan(int maBan)
        {
            return banDAL.GetTrangThaiBan(maBan);
        }
        public float GetTienBan(int maDonHang)
        {
            return banDAL.GetTienBan(maDonHang);
        }

        public List<Ban_DTO> GetBanTrong()
        {
            return banDAL.GetBanTrong();
        }
        public List<String> GetTenBan()
        {
            List<Ban_DTO> banList = banDAL.GetAll();
            List<String> tenBanList = new List<String>();
            foreach (var ban in banList)
            {
                tenBanList.Add(ban.TenBan);
            }
            return tenBanList;
        }
    }
}
