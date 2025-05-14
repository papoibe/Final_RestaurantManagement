using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer;
using TransferObject;
using System.Data;
using System.Data.SqlClient;

namespace BusinessLayer
{
    public class PhieuNhapKho_BL
    {
        private PhieuNhapKho_DAL phieuNhapDAL;

        public PhieuNhapKho_BL()
        {
            phieuNhapDAL = new PhieuNhapKho_DAL();
        }

        // Lấy danh sách phiếu nhập
        public List<PhieuNhap_DTO> GetPhieuNhapList()
        {
            try
            {
                return phieuNhapDAL.GetPhieuNhapList();
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        // Lấy thông tin phiếu nhập theo mã
        public PhieuNhap_DTO GetPhieuNhapById(int maPhieuNhap)
        {
            try
            {
                return phieuNhapDAL.GetPhieuNhapById(maPhieuNhap);
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        // Tạo phiếu nhập mới
        public int CreatePhieu(PhieuNhap_DTO phieuNhap)
        {
            try
            {
                return phieuNhapDAL.AddPhieuNhap(phieuNhap);
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        // Cập nhật phiếu nhập
        public bool UpdatePhieuNhap(PhieuNhap_DTO phieuNhap)
        {
            try
            {
                return phieuNhapDAL.UpdatePhieuNhap(phieuNhap);
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        // Cập nhật trạng thái phiếu
        public bool Update_TrangThaiPhieu(int maPhieuNhap, bool trangThai)
        {
            try
            {
                return phieuNhapDAL.Update_TrangThaiPhieu(maPhieuNhap, trangThai);
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        // xoa phieu 
        public bool DeletePhieuNhap(int maPhieuNhap)
        {
            try
            {
                return phieuNhapDAL.DeletePhieuNhap(maPhieuNhap);
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public bool ResetIdentity()
        {
            try
            {
                return phieuNhapDAL.ResetIdentity();
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        // kiem tra ma nhan vien ton tai 
        public bool KiemTraNhanVienTonTai(int maNhanVien)
        {
            try
            {
                // Gọi phương thức của DAL
                return phieuNhapDAL.KiemTraNhanVienTonTai(maNhanVien);
            }
            catch (SqlException ex)
            {
                // Ném ngoại lệ để xử lý ở lớp giao diện
                throw ex;
            }
        }
    }
}