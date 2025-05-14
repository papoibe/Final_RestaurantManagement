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
    public partial class FrmAdmin : Form
    {
        private Account_DTO currentUser;

        public FrmAdmin(Account_DTO accountUser)
        {
            InitializeComponent();
            currentUser = accountUser;


            // Đặt trạng thái đăng nhập cho RestaurantManagement
            RestaurantManagement.SetAuthenticationState(true, accountUser);
        }

        private void FrmAdmin_Load(object sender, EventArgs e)
        {
            // Hiển thị thông tin người dùng
            lb_Username.Text = currentUser.Username;
            lb_DisplayName.Text = currentUser.DisplayName;
            lb_Welcome.Text = "Xin chào, " + currentUser.DisplayName + "Quản trị viên";

            // Mặc định khi load form sẽ hiện thị dashboard
            LoadDashboard();

            // đặt màu nền cho button 
            SetActiveButton(btnDashboard);
        }

        // xóa các control trong panelMain
        private void ClearPanel()
        {
            // Xóa tất cả các điều khiển trong panelMain
            pnlContent.Controls.Clear();
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

        private void FrmAdmin_FormClosed(object sender, FormClosedEventArgs e)
        {
            // Khi form admin đóng, sẽ gọi hàm SetAuthenticationState để đặt trạng thái đăng nhập về false
            RestaurantManagement.SetAuthenticationState(false, null);
            Application.Exit(); // đóng admin sẽ đóng ứng dụng 
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
            pnlContent.Controls.Add(dashboard);

            //load dữ liệu dashboard
            dashboard.Show();

        }

        private void LoadQuanLyKho()
        {
            ClearPanel();
            // tạo instance của form dashboard
            FrmQuanLyKho kho = new FrmQuanLyKho();
            //Thiết lập thuộc tính để form hiển thị 
            kho.TopLevel = false;
            kho.FormBorderStyle = FormBorderStyle.None;
            kho.Dock = DockStyle.Fill;
            // Thêm form vào panelMain
            pnlContent.Controls.Add(kho);
            kho.Show();
        }



        private void LoadQuanLyCL()
        {
            ClearPanel();
            FrmKiemSoatChatLuong cl = new FrmKiemSoatChatLuong();
            cl.TopLevel = false;
            cl.FormBorderStyle = FormBorderStyle.None;
            cl.Dock = DockStyle.Fill;
            pnlContent.Controls.Add(cl);
            cl.Show();
        }

        private void LoadQuanLyNV()
        {
            ClearPanel();
            FrmQuanLyNhanVien nv = new FrmQuanLyNhanVien();
            nv.TopLevel = false;
            nv.FormBorderStyle = FormBorderStyle.None;
            nv.Dock = DockStyle.Fill;
            pnlContent.Controls.Add(nv);
            nv.Show();
        }

        private void LoadBaoCao()
        {
            ClearPanel();
            FrmBaoCao bc = new FrmBaoCao();
            bc.TopLevel = false;
            bc.FormBorderStyle = FormBorderStyle.None;
            bc.Dock = DockStyle.Fill;
            pnlContent.Controls.Add(bc);
            bc.Show();
        }

        private void LoadThongTinhDD()
        {
            ClearPanel();
            FrmThongTinDinhDuong dinhDuong = new FrmThongTinDinhDuong();
            dinhDuong.TopLevel = false;
            dinhDuong.FormBorderStyle = FormBorderStyle.None;
            dinhDuong.Dock = DockStyle.Fill;
            pnlContent.Controls.Add(dinhDuong);
            dinhDuong.Show();
        }

        private void LoadQuanLyBan()
        {
            ClearPanel();
            FrmQuanLyBan ban = new FrmQuanLyBan();
            ban.TopLevel = false;
            ban.FormBorderStyle = FormBorderStyle.None;
            ban.Dock = DockStyle.Fill;
            pnlContent.Controls.Add(ban);
            ban.Show();
        }

        private void LoadKhuyenMai()
        {
            ClearPanel();
            FrmKhuyenMai km = new FrmKhuyenMai();
            km.TopLevel = false;
            km.FormBorderStyle = FormBorderStyle.None;
            km.Dock = DockStyle.Fill;
            pnlContent.Controls.Add(km);
            km.Show();
        }

        private void LoadQuanLyMon()
        {
            ClearPanel();
            FrmQuanLyMon mon = new FrmQuanLyMon();
            mon.TopLevel = false;
            mon.FormBorderStyle = FormBorderStyle.None;
            mon.Dock = DockStyle.Fill;
            pnlContent.Controls.Add(mon);
            mon.Show();
        }

        private void LoadQuanLyHoaDon()
        {
            ClearPanel();
            FrmQuanLyHoaDon hd = new FrmQuanLyHoaDon();
            hd.TopLevel = false;
            hd.FormBorderStyle = FormBorderStyle.None;
            hd.Dock = DockStyle.Fill;
            pnlContent.Controls.Add(hd);
            hd.Show();
        }


  

        private void btnDashboard_Click(object sender, EventArgs e)
        {
            LoadDashboard(); 
            SetActiveButton(btnDashboard);
        }

        private void btnQuanLyKho_Click(object sender, EventArgs e)
        {
            LoadQuanLyKho();
            SetActiveButton(btnQuanLyKho);
        }
    
        private void btnQuanLyQC_Click(object sender, EventArgs e)
        {
            LoadQuanLyCL();
            SetActiveButton(btnQuanLyQC);
        }

        private void btnQuanLyNhanVien_Click(object sender, EventArgs e)
        {
            LoadQuanLyNV();
            SetActiveButton(btnQuanLyNhanVien);
        }

        private void btnBaoCao_Click(object sender, EventArgs e)
        {
            LoadBaoCao();
            SetActiveButton(btnBaoCao);
        }

        private void btnTTDD_Click(object sender, EventArgs e)
        {
            LoadThongTinhDD();
            SetActiveButton(btnTTDD);
        }

        private void btnDangXuat_Click(object sender, EventArgs e)
        {
            // Đặt lại trạng thái đăng nhập
            RestaurantManagement.SetAuthenticationState(false, null);

            // Ẩn form hiện tại thay vì đóng nó
            this.Hide();

            // Tạo và hiển thị form đăng nhập
            FrmLogin frmLogin = new FrmLogin();
            frmLogin.Show();
        }

        private void btnKhuyenMai_Click(object sender, EventArgs e)
        {
            LoadKhuyenMai();
            SetActiveButton(btnKhuyenMai);
        }

        private void btnQuanLyMon_Click(object sender, EventArgs e)
        {
            LoadQuanLyMon();
            SetActiveButton(btnQuanLyMon);
        }

        private void btnQuanLyBan_Click(object sender, EventArgs e)
        {
            LoadQuanLyBan();
            SetActiveButton(btnQuanLyBan);
        }

        private void btnQuanLyHoaDon_Click(object sender, EventArgs e)
        {
            LoadQuanLyHoaDon();
            SetActiveButton(btnQuanLyHoaDon);
        }
    }
}
