using BusinessLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PresentationLayer
{
    public partial class FrmThanhToan: Form
    {
        private int maDH, maBan;
        private MonAn_BL MonAn_BL;
        public FrmThanhToan(int MaDH, int MaBan)
        {
            InitializeComponent();
            maDH = MaDH;
            maBan = MaBan;
        }

        private void FrmThanhToan_Load(object sender, EventArgs e)
        {
            //var monAnBL = new MonAn_BL();
            //lv_thanhtoan.View = View.Details;
            //ListViewItem item = new ListViewItem(MonAn_BL.TenMon);
            //item.SubItems.Add("1"); // Số lượng mặc định là 1
            //item.SubItems.Add(monAn.DonGia.ToString("C")); // Đơn giá
            //item.SubItems.Add(monAn.DonGia.ToString("C")); // Thành tiền (1 * Đơn giá)
            //lv_thanhtoan.Items.Add(item); // Thêm vào ListView
            //// Load thông tin thanh toán tại đây
            //// Ví dụ: lblMaDH.Text = maDH.ToString();
            //// lblMaBan.Text = maBan.ToString();
        }
        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}
