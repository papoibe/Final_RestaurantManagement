using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BusinessLayer;
using TransferObject;
namespace PresentationLayer
   
{
    public partial class FrmLogin: Form
    {
        private Account_BL accountBL;
        private string captchaText = "";

        // event tự định nghĩa đăng nhập thành công 
        public event EventHandler<Account_DTO> DangNhapThanhCong;
        public FrmLogin()
        {
            InitializeComponent();
            accountBL = new Account_BL();
            GenerateCaptcha();
        }

        private void GenerateCaptcha()
        {
            Random rnd = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            captchaText = new string(Enumerable.Repeat(chars, 4)
                .Select(s => s[rnd.Next(s.Length)]).ToArray());

            int width = 130, height = 50;
            Bitmap bitmap = new Bitmap(width, height);
            Graphics g = Graphics.FromImage(bitmap);
            g.Clear(Color.White);

            // ======= VẼ NHIỄU: LINES =======
            for (int i = 0; i < 10; i++)
            {
                Pen pen = new Pen(Color.FromArgb(rnd.Next(100, 255), rnd.Next(255), rnd.Next(255)));
                Point p1 = new Point(rnd.Next(width), rnd.Next(height));
                Point p2 = new Point(rnd.Next(width), rnd.Next(height));
                g.DrawLine(pen, p1, p2);
            }

            // ======= VẼ NHIỄU: DOTS =======
            for (int i = 0; i < 100; i++)
            {
                int x = rnd.Next(width);
                int y = rnd.Next(height);
                bitmap.SetPixel(x, y, Color.FromArgb(rnd.Next(256), rnd.Next(256), rnd.Next(256)));
            }

            // ======= VẼ KÝ TỰ =======
            using (Font font = new Font("Arial", 20, FontStyle.Bold))
            {
                for (int i = 0; i < captchaText.Length; i++)
                {
                    Color randomColor = Color.FromArgb(rnd.Next(50, 180), rnd.Next(255), rnd.Next(255));
                    Brush b = new SolidBrush(randomColor);

                    int offsetX = 20 + i * 20 + rnd.Next(-2, 3); // dịch ngang
                    int offsetY = rnd.Next(5, 15);              // dịch dọc

                    g.DrawString(captchaText[i].ToString(), font, b, new PointF(offsetX, offsetY));
                }
            }

            // Gán cho PictureBox
            pic1.Image = bitmap;
            txtXacNhan.Clear();
        }
        //xu ly nut dang nhap 
        private void btn_DangNhap_Click(object sender, EventArgs e)
        {
            // Trước khi gọi accountBL.Login()
            if (txtXacNhan.Text.Trim().ToUpper() != captchaText)
            {
                MessageBox.Show("Mã xác nhận không đúng. Vui lòng thử lại!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                GenerateCaptcha(); // Tạo mã mới
                return;
            }
            try
            {
                //lay thong tin tu textbox
                string username = txt_Username.Text.Trim();
                string password = txt_Password.Text;

                //kiem tra thong tin nhap vao
                if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                {
                    MessageBox.Show("Vui long nhap day du ten dang nhap va mat khau!", "Thong bao",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    GenerateCaptcha();
                    return;
                }


                //tao doi tuong 
                Account_DTO account = new Account_DTO(username, password);

                //goi phuong thuc dang nhap tu BL 
                Account_DTO accountDTO = accountBL.Login(account);

                //kiem tra dang nhap thanh cong
                if (accountDTO != null)
                {
                    // Thông báo đăng nhập thành công
                    DangNhapThanhCong?.Invoke(this, accountDTO);
                    //neu dang nhap thanh cong, xac dinh ma loai
                    //neu ma loai = 1 thi la admin
                    //neu ma loai = 2 thi la nhan vien
                    if (accountDTO.Maloai == 1) //admin
                    {
                        try
                        {
                            //neu la admin
                            //thuc hien mo form admin
                            FrmAdmin adminForm = new FrmAdmin(accountDTO);
                            //truyen accountDTO vao form admin
                            //hien thi form admin   
                            adminForm.Show();
                            this.Hide();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }

                    }
                    else if (accountDTO.Maloai == 2)
                    {
                        try
                        {
                            //neu la nhan vien
                            //thuc hien mo form nhan vien
                            FrmWorkers workerForm = new FrmWorkers(accountDTO);
                            //truyen accountDTO vao form nhan vien
                            //hien thi form nhan vien
                            workerForm.Show();
                            this.Hide();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Khong co quyen truy cap!", "loi dang nhap",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                }
                else
                {
                    MessageBox.Show("Ten dang nhap hoac mat khau khong dung!", "loi dang nhap",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txt_Username.Clear();
                    txt_Password.Clear();
                    txt_Username.Focus();
                    GenerateCaptcha();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btn_exit_Click(object sender, EventArgs e)
        {

            DialogResult result = MessageBox.Show("Bạn có muốn thoát không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                 // đóng form
                 this.Close();
            }    
           
        }

        private void btn_DangKi_Click(object sender, EventArgs e)
        {
            FrmDangKy frmDangKy = new FrmDangKy();
            frmDangKy.ShowDialog();
        }

        private void FrmLogin_FormClosed(object sender, FormClosedEventArgs e)
        {

        }
    }
}
