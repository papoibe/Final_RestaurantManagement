using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer;
using TransferObject;

namespace BusinessLayer
{
    public class NhanVien_BL
    {
        private readonly NhanVien_DAL _dal = new NhanVien_DAL();

        public List<NhanVien_DTO> LayDanhSachNhanVien()
        {
            return _dal.GetAll();
        }
        public bool ThemNhanVien(NhanVien_DTO nv)
        {
            return _dal.InsertNhanVien(nv);
        }
        public bool CapNhatNhanVien(NhanVien_DTO nv)
        {
            return _dal.UpdateNhanVien(nv);
        }
        public bool XoaNhanVien(int maNV, int maTK)
        {
            return _dal.XoaNhanVien(maNV, maTK);
        }
    }
}
