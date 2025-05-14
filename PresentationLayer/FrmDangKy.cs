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
    public partial class FrmDangKy : Form
    {
        private Account_BL accountBL;
        public FrmDangKy()
        {
            InitializeComponent();
            accountBL = new Account_BL();
        }

        // xu ly khi an nut dang ky
        private void btn_DangKy_Click(object sender, EventArgs e)
        {
            try
            {
                //lay thong tin tu textbox
                string username = txt_Username.Text.Trim();
                string password = txt_Password.Text;
                string displayName = txt_DisplayName.Text.Trim();
                int maloai = 2; // 2 la nhan vien, 1 la admin nhung de mac dinh cho nhanh thi ta cho nhan vien

                // gan gia tri mac dinh maloai la 2 

                //kiem tra thong tin nhap vao
                if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(displayName))
                {
                    MessageBox.Show("Vui long nhap day du thong tin!", "Thong bao",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                //tao doi tuong Account
                Account_DTO account = new Account_DTO(username, password, displayName, maloai);
                //goi phuong thuc dang ky
                if (accountBL.Register(account))
                {
                    MessageBox.Show("Dang ky thanh cong!", "Thong bao", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Tai khoan da ton tai!", "loi dang ky", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txt_Username.Clear();
                    txt_Username.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Loi: " + ex.Message, "loi dang ky", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btn_Thoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    
    }
}
