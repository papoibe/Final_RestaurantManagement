using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer;
using TransferObject;

namespace BusinessLayer
{
    public class KhuyenMai_BL
    {
            KhuyenMai_DAL dal = new KhuyenMai_DAL();

            public List<KhuyenMai_DTO> GetAllKhuyenMai()
            {
                return dal.GetAllKhuyenMai();
            }

            public bool AddKhuyenMai(KhuyenMai_DTO km)
            {
                return dal.InsertKhuyenMai(km);
            }

            public bool UpdateKhuyenMai(KhuyenMai_DTO km)
            {
                return dal.UpdateKhuyenMai(km);
            }

            public bool DeleteKhuyenMai(string maKM)
            {
                return dal.DeleteKhuyenMai(maKM);
            }

            public bool CompleteKhuyenMai(KhuyenMai_DTO km)
            {
                return dal.UpdateTrangThai(km.MaKhuyenMai, km.TrangThai);
            }

         }
    }
