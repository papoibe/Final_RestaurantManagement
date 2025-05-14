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
    public class KiemTraChatLuong_BL
    {
        private KiemTraChatLuong_DAL kiemTraDAL;
        public KiemTraChatLuong_BL()
        {
            kiemTraDAL = new KiemTraChatLuong_DAL();
        }
        // Lấy danh sách kiểm tra chất lượng
        public List<KiemTraChatLuong_DTO> GetKiemTraChatLuongList()
        {
            try
            {
                return kiemTraDAL.GetKiemTraChatLuongList();
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        //lay thong tin kiem tra chat luong theo ma
        public KiemTraChatLuong_DTO GetKiemTraById(string maKiemTra)
        {
            try
            {
                return kiemTraDAL.GetKiemTraById(maKiemTra);
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        // them phieu kiem tra chat luong
        public bool AddKiemTraChatLuong(KiemTraChatLuong_DTO kiemTra)
        {
            try
            {
                return kiemTraDAL.AddKiemTraChatLuong(kiemTra);
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        //cap nhat phieu kiem tra chat luong
        public bool UpdateKiemTraChatLuong(KiemTraChatLuong_DTO kiemTra)
        {
            try
            {
                return kiemTraDAL.UpdateKiemTraChatLuong(kiemTra);
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        //xoa phieu kiem tra chat luong
        public bool DeleteKiemTraChatLuong(string maKiemTra)
        {
            try
            {
                return kiemTraDAL.DeleteKiemTraChatLuong(maKiemTra);
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        //kiem tra ton tai phieu kiem tra chat luong
        public bool KiemTraNhanVienTonTai(int maNhanVien)
        {
            try
            {
                return kiemTraDAL.KiemTraNhanVienTonTai(maNhanVien);
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
    }
}
