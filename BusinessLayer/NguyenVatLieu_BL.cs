using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using DataLayer;
using TransferObject;

namespace BusinessLayer
{
    public class NguyenVatLieu_BL
    {
        private NguyenVatLieu_DAL nguyenVatLieuDAL;

        public NguyenVatLieu_BL()
        {
            nguyenVatLieuDAL = new NguyenVatLieu_DAL();
        }

        // Lấy danh sách nguyên vật liệu
        public List<NguyenVatLieu_DTO> GetNguyenVatLieuList()
        {
            try
            {
                return nguyenVatLieuDAL.GetNguyenVatLieuList();
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        // Lấy thông tin nguyên vật liệu theo mã
        public NguyenVatLieu_DTO GetNguyenVatLieuById(int maNguyenLieu)
        {
            try
            {
                return nguyenVatLieuDAL.GetNguyenVatLieuById(maNguyenLieu);
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        // Cập nhật số lượng tồn
        public bool UpdateSoLuongTon(int maNguyenLieu, float soLuongThem)
        {
            try
            {
                // Lấy nguyên vật liệu hiện tại
                NguyenVatLieu_DTO nvl = GetNguyenVatLieuById(maNguyenLieu);
                if (nvl == null)
                    return false;

                // Tính số lượng tồn mới
                float soLuongMoi = nvl.SoLuongTon + soLuongThem;

                // Không cho phép số lượng tồn âm
                if (soLuongMoi < 0)
                    return false;

                // Cập nhật số lượng tồn
                return nguyenVatLieuDAL.UpdateSoLuongTon(maNguyenLieu, soLuongMoi);
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
    }
}