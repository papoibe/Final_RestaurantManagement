using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BusinessLayer;
using TransferObject;

namespace PresentationLayer
{
    public partial class FrmWorkers : Form
    {
        private Account_DTO currentUser;
        public FrmWorkers(Account_DTO accountUser)
        {
            InitializeComponent();
            currentUser = accountUser;

            // Đặt trạng thái đăng nhập cho RestaurantManagement
            RestaurantManagement.SetAuthenticationState(true, accountUser);
        }

        private void FrmWorkers_Load(object sender, EventArgs e)
        {
            // Hiển thị thông tin người dùng
            lb_Username.Text = currentUser.Username;
            lb_DisplayName.Text = currentUser.DisplayName;
            lb_Welcome.Text = "Xin chào, " + currentUser.DisplayName + "Nhân Viên";

            LoadDashboard();

            // đặt màu nền cho button
            SetActiveButton(btnDashboard);
        }

        private void FrmWorkers_FormClosed(object sender, FormClosedEventArgs e)
        {
            // Đặt lại trạng thái đăng nhập khi đóng ứng dụng
            RestaurantManagement.SetAuthenticationState(false, null);
            Application.Exit(); // đóng form workers, thoát ứng dụng

        }

        private void btnDangXuat_Click(object sender, EventArgs e)
        {
            // đặt lại trạng thái đăng nhập
            RestaurantManagement.SetAuthenticationState(false, null); 

            // ẩn form hiện tại 
            this.Hide();

            //tạo và hiển thị form đăng nhập
            FrmLogin frmLogin = new FrmLogin();
            frmLogin.Show();

        }

        private void ClearPanel()
        {
            // Xóa tất cả các điều khiển trong panelMain
            pnlDashboard.Controls.Clear();
        }

        private void SetActiveButton(Button button)
        {
            // Đặt màu nền cho nút được chọn
            button.BackColor = Color.FromArgb(0, 0, 192);
            button.ForeColor = Color.White;
            // Đặt màu nền cho các nút khác
            foreach (Control control in pnlSidebar.Controls)
            {
                if (control is Button && control != button)
                {
                    control.BackColor = Color.FromArgb(0, 0, 64);
                    control.ForeColor = Color.White;
                }
            }
        }

        private void LoadDashboard()
        {
            ClearPanel();

            // tạo instance của form dashboard
            RestaurantManagement dashboard = new RestaurantManagement();

            //Thiết lập thuộc tính để form hiển thị 
            dashboard.TopLevel = false;
            dashboard.FormBorderStyle = FormBorderStyle.None;
            dashboard.Dock = DockStyle.Fill;

            // Thêm form vào panelMain
            pnlDashboard.Controls.Add(dashboard);

            //load dữ liệu dashboard
            dashboard.Show();

        }

        private void LoadQuanLyBan()
        {
            ClearPanel();
            // Tạo một instance của UserControl
            FrmQuanLyBan qlb = new FrmQuanLyBan();
            qlb.TopLevel = false;
            qlb.FormBorderStyle = FormBorderStyle.None;
            qlb.Dock = DockStyle.Fill;
            // Thêm form vào panelMain
            pnlDashboard.Controls.Add(qlb);
            qlb.Show();
        }


        private void LoadKhuyenMai()
        {
            ClearPanel();
            // Tạo một instance của UserControl
            FrmKhuyenMai km = new FrmKhuyenMai(false);
            km.TopLevel = false;
            km.FormBorderStyle = FormBorderStyle.None;
            km.Dock = DockStyle.Fill;
            // Thêm form vào panelMain
            pnlDashboard.Controls.Add(km);
            km.Show();
        }


        private void LoadQuanLyHoaDon()
        {
            ClearPanel();
            // Tạo một instance của UserControl
            FrmQuanLyHoaDon qlhd = new FrmQuanLyHoaDon();
            qlhd.TopLevel = false;
            qlhd.FormBorderStyle = FormBorderStyle.None;
            qlhd.Dock = DockStyle.Fill;
            // Thêm form vào panelMain
            pnlDashboard.Controls.Add(qlhd);
            qlhd.Show();
        }

        private void LoadQuanLyBep()
        {
            ClearPanel();
            // Tạo một instance của UserControl
            FrmQuanLyMon qlb = new FrmQuanLyMon();
            qlb.TopLevel = false;
            qlb.FormBorderStyle = FormBorderStyle.None;
            qlb.Dock = DockStyle.Fill;
            // Thêm form vào panelMain
            pnlDashboard.Controls.Add(qlb);
            qlb.Show();
        }

        private void LoadTTDD()
        {
            ClearPanel();
            // Tạo một instance của UserControl
            FrmThongTinDinhDuong ttdd = new FrmThongTinDinhDuong();
            ttdd.TopLevel = false;
            ttdd.FormBorderStyle = FormBorderStyle.None;
            ttdd.Dock = DockStyle.Fill;
            // Thêm form vào panelMain
            pnlDashboard.Controls.Add(ttdd);
            ttdd.Show();

        }
        private void btnQuanLyBan_Click(object sender, EventArgs e)
        {
            LoadQuanLyBan();
            // Đặt màu nền cho nút được chọnS
            SetActiveButton(btnQuanLyBan);
        }

        private void btnDashboard_Click(object sender, EventArgs e)
        {
            LoadDashboard(); 
            SetActiveButton(btnDashboard);
        }

        private void btnKhuyenMai_Click(object sender, EventArgs e)
        {
            LoadKhuyenMai();
            SetActiveButton(btnKhuyenMai);
        }

        private void btnQuanLyHoaDon_Click(object sender, EventArgs e)
        {
            LoadQuanLyHoaDon();
            SetActiveButton(btnQuanLyHoaDon);
        }

        private void btnQuanLyBep_Click(object sender, EventArgs e)
        {
            LoadQuanLyBep();
            SetActiveButton(btnQuanLyBep);
        }

        private void btnThongTinDD_Click(object sender, EventArgs e)
        {
            LoadTTDD();
            SetActiveButton(btnThongTinDD);

        }

        private void lb_Welcome_Click(object sender, EventArgs e)
        {

        }
    }
}
