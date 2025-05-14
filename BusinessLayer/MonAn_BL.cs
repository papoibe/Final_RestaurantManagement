using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer;
using TransferObject;

namespace BusinessLayer
{
    public class MonAn_BL
    {
        private MonAn_DAL monAnDAL;
        private LoaiMonAn_DAL loaiMonAn_DAL;
        public MonAn_BL()
        {
            monAnDAL = new MonAn_DAL();
        }

        public List<MonAn_DTO> GetAll()
        {
            return monAnDAL.GetAll();
        }

        public List<MonAn_DTO> GetMonAnByLoai(int maLoai) // hàm lấy món ăn theo loại món
        {
            return monAnDAL.GetMonAnByLoai(maLoai);
        }

        public MonAn_DTO GetMonAnByID(int id)
        {
            return monAnDAL.GetMonAnByID(id);
        }
        public List<MonAn_MaDH_DTO> GetMonAnByMaDH(int maDH)
        {
            return monAnDAL.GetMonByMaDH(maDH);
        }
        public List<LoaiMonAn_DTO> GetAllLoaiMonAn() // hàm lấy loại món ăn
        {
            loaiMonAn_DAL = new LoaiMonAn_DAL();
            return loaiMonAn_DAL.GetAll();
        }
        public List<MonAn_DTO> GetMonAnByTen(string tenMon)
        {
            return monAnDAL.GetMonAnByTen(tenMon);
        }

        //trongthinh
        public bool UpdateTrangThaiMonAn(int maMonAn, bool trangThai)
        {
            try
            {
                // Call the DAL method to update the status
                return monAnDAL.UpdateTrangThaiMonAn(maMonAn, trangThai);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi xử lý cập nhật trạng thái món ăn: " + ex.Message);
            }
        }
        public bool UpdatePrice(int maMonAn, float donGiaMoi)
        {
            try
            {
                // Call the DAL method to update the price
                return monAnDAL.UpdatePrice(maMonAn, donGiaMoi);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi xử lý cập nhật giá món ăn: " + ex.Message);
            }
        }
    }
}