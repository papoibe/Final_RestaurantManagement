using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using BusinessLayer;
using TransferObject;
using System.Net.Http.Headers;

namespace PresentationLayer
{
    public partial class RestaurantManagement : Form
    {
        private Account_BL accountBL;
        private NhanVien_BL nhanVienBL;
        private DonHang_BL donHangBL;
        // đánh dấu mục đã đăng nhập rồi
        private bool isAuthenticated = false;
        //Thêm biến lưu thông tin tài khoản đăng nhập 
        private Account_DTO loggedInAccount = null;

        //biến static để lưu trạng thái đăng nhập giữa các instance
        private static bool globalIsAuthenticated = false;
        private static Account_DTO globalLoggedInAccount = null;
        public RestaurantManagement()
        {
            InitializeComponent();
            accountBL = new Account_BL();
            nhanVienBL = new NhanVien_BL();
            donHangBL = new DonHang_BL();

            // Lấy trạng thái đăng nhập từ biến static
            isAuthenticated = globalIsAuthenticated;
            loggedInAccount = globalLoggedInAccount;
        }

        private void RestaurantManagement_Load(object sender, EventArgs e)
        {
            try
            {
                // Đăng ký sự kiện click cho các panel
                RegisterClickEvents();

                //khi khởi động form load dữ liệu ban đầu
                LoadDashboard();

            }
            catch (Exception ex)
            {

                // Bỏ qua lỗi khi load dữ liệu dashboard, hiển thị giá trị mặc định
                lblStaffCount.Text = "0";
                lblAdminCount.Text = "0";
                lblTodayIncome.Text = "$0.00";
                lblTotalIncome.Text = "$0.00";            
            }
        }

        private void MoFormDangNhap()
        {
            if (isAuthenticated)
                return;

            // hiển thị form đăng nhập
            FrmLogin frmLogin = new FrmLogin();
      

            // đóng for dashboard
            this.Hide();

            //đăng ký sự kiện đăng nhập thành công
            frmLogin.DangNhapThanhCong += FrmLogin_DangNhapThanhCong;
            //đăng ký sự kiện FormClosed để xử lý khi form đăng nhập đóng
            frmLogin.FormClosed += FrmLogin_FormClosed;

            frmLogin.Show();
        }

        // Sự kiện được gọi khi đăng nhập thành công
        private void FrmLogin_DangNhapThanhCong(object sender, Account_DTO taiKhoan)
        {
            // Đánh dấu đã đăng nhập thành công
            isAuthenticated = true;

            // Lưu thông tin tài khoản
            loggedInAccount = taiKhoan;

            // Cập nhật biến static
            globalIsAuthenticated = true;
            globalLoggedInAccount = taiKhoan;
        }


        private void Panel_Click(object sender, EventArgs e)
        {

            // Mở form đăng nhập nếu chưa đăng nhập
            if (!isAuthenticated)
            {
                MoFormDangNhap();
            }

        }

        private void RegisterClickEvents() //đệ quy
        {
            //danh sách
            Panel[] panels = { pnlStaffCard, pnlAdminCard, pnlTodayIncomeCard, pnlTotalIncomeCard };

            foreach (Panel panel in panels)
            {
                RegisterControlAndChildren(panel);

            }
        }
        
        private void RegisterControlAndChildren(Control control)
        {
            // Đăng ký sự kiện Click cho control hiện tại
            control.Click += new EventHandler(Panel_Click);
            // Đệ quy để đăng ký sự kiện Click cho tất cả các control con
            foreach (Control child in control.Controls)
            {
                RegisterControlAndChildren(child);
            }
        }

        // Load dữ liệu dashboard
        private void LoadDashboard()
        {
            try
            {
                //đếm sl nhân viên 
                List<NhanVien_DTO> danhSachNhanVien = nhanVienBL.LayDanhSachNhanVien();
                int soNhanVien = 0;
                int soAdmin = 0;

                foreach (var nv in danhSachNhanVien)
                {
                    if (nv.MaLoai == 2)
                        soNhanVien++;
                    else if (nv.MaLoai == 1)
                        soAdmin++;
                }
                lblStaffCount.Text = soNhanVien.ToString();
                lblAdminCount.Text = soAdmin.ToString();

                //tính doanh thu ngày hôm nay 
                DateTime today = DateTime.Today;
                BaoCaoDoanhThu_BL baoCaoDoanhThuBL = new BaoCaoDoanhThu_BL();
                DataTable dtDoanhThu = baoCaoDoanhThuBL.GetBaoCaoDoanhThuTheoNgay(today, today);

                double doanhThuNgay = 0;
                if (dtDoanhThu.Rows.Count > 0)
                {
                    //kiểm tra tên cột thực tế
                    if (dtDoanhThu.Columns.Contains("TongDoanhThu"))
                        doanhThuNgay = Convert.ToDouble(dtDoanhThu.Rows[0]["TongDoanhThu"]);
                    else if (dtDoanhThu.Columns.Contains("TongTien"))
                        doanhThuNgay = Convert.ToDouble(dtDoanhThu.Rows[0]["TongTien"]);
                }

                lblTodayIncome.Text = string.Format("{0:#,##0} VNĐ", doanhThuNgay);

                // Tính tổng món ăn đã bán
                BaoCaoMonAn_BL baoCaoMonAnBL = new BaoCaoMonAn_BL();
                DateTime startDate = DateTime.Today.AddDays(-30); // Lấy từ 30 ngày gần nhất
                DataTable dtMonAn = baoCaoMonAnBL.GetBaoCaoMonAnBanChay(startDate, today);

                int tongSoMonAn = 0;
                if (dtMonAn.Rows.Count > 0)
                {
                    string columnName = "TongSoLuong";
                    if (!dtMonAn.Columns.Contains(columnName))
                    {
                        // Tìm cột tương tự nếu không có cột TongSoLuong
                        foreach (DataColumn col in dtMonAn.Columns)
                        {
                            if (col.ColumnName.Contains("SoLuong") || col.ColumnName.Contains("Tong"))
                            {
                                columnName = col.ColumnName;
                                break;
                            }
                        }
                    }

                    foreach (DataRow row in dtMonAn.Rows)
                    {
                        tongSoMonAn += Convert.ToInt32(row[columnName]);
                    }
                }

                lblTotalIncome.Text = string.Format("{0} món", tongSoMonAn);
            }
            catch (Exception ex)
            {
                // Bỏ qua lỗi và hiển thị giá trị mặc định
                lblStaffCount.Text = "0";
                lblAdminCount.Text = "0";
                lblTodayIncome.Text = "$0.00";
                lblTotalIncome.Text = "$0.00";
            }
        }

        private void FrmLogin_FormClosed(object sender, FormClosedEventArgs e)
        {
            // kiểm xem còn form mở không
            if (Application.OpenForms.Count <= 1)
            {
                // không còn form nào mở thì mở dashboard 
                this.Show();
            }
           
        }

        // tạo phương thức pulbic để đặt trạng thái thành công đăng nhập
        public static void SetAuthenticationState(bool authenticated, Account_DTO account = null)
        {
            globalIsAuthenticated = authenticated;
            globalLoggedInAccount = account;
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void pnlStaffCard_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
