using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransferObject
{
    public class NhanVien_DTO
    {
        public int MaNhanVien { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string DisplayName { get; set; }
        public string HoTen { get; set; }
        public string GioiTinh { get; set; }
        public DateTime NgaySinh { get; set; }
        public string DiaChi { get; set; }
        public string SDT { get; set; }
        public string Email { get; set; }
        public DateTime NgayVaoLam { get; set; }
        public bool TrangThai { get; set; }
        public int MaTaiKhoan { get; set; }
        public int MaLoai {  get; set; }
        public NhanVien_DTO()
        {
        }

        public NhanVien_DTO(int maNhanVien, string username, string password, string displayName, string hoTen, string gioiTinh, DateTime ngaySinh, string diaChi, string sDT, string email, DateTime ngayVaoLam, bool trangThai, int maTaiKhoan, int maLoai)
        {
            MaNhanVien = maNhanVien;
            Username = username;
            Password = password;
            DisplayName = displayName;
            HoTen = hoTen;
            GioiTinh = gioiTinh;
            NgaySinh = ngaySinh;
            DiaChi = diaChi;
            SDT = sDT;
            Email = email;
            NgayVaoLam = ngayVaoLam;
            TrangThai = trangThai;
            MaTaiKhoan = maTaiKhoan;
            MaLoai = maLoai;
        }
    }



}
