using System;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;
using TransferObject;

namespace PresentationLayer.Helpers
{
    public class PrintHelper
    {
        private string tenNhaHang = "NHÀ HÀNG QUÁN ĂN 3T";
        private string diaChiNhaHang = "123 Đường 8, Quận 7, TP.HCM";
        private string sdtNhaHang = "0123.456.789";

        private Font titleFont = new Font("Arial", 16, FontStyle.Bold);
        private Font headerFont = new Font("Arial", 12, FontStyle.Bold);
        private Font normalFont = new Font("Arial", 10, FontStyle.Regular);
        private Font totalFont = new Font("Arial", 12, FontStyle.Bold);

        private HoaDonTT_DTO hoaDon;
        private DataGridView dgvChiTiet;

        public PrintHelper(HoaDonTT_DTO hoaDon, DataGridView dgvChiTiet)
        {
            this.hoaDon = hoaDon;
            this.dgvChiTiet = dgvChiTiet;
        }

        public void Print()
        {
            PrintDocument pd = new PrintDocument();
            pd.PrintPage += new PrintPageEventHandler(PrintPage);

            PrintPreviewDialog printPreviewDialog = new PrintPreviewDialog();
            printPreviewDialog.Document = pd;
            printPreviewDialog.ShowDialog();
        }

        private void PrintPage(object sender, PrintPageEventArgs e)
        {
            Graphics g = e.Graphics;
            int startX = 10;
            int startY = 10;
            int offset = 20;

            // Vẽ header
            g.DrawString(tenNhaHang, titleFont, Brushes.Black, new Point(startX + 100, startY));
            startY += 30;
            g.DrawString(diaChiNhaHang, normalFont, Brushes.Black, new Point(startX + 50, startY));
            startY += 20;
            g.DrawString($"SĐT: {sdtNhaHang}", normalFont, Brushes.Black, new Point(startX + 80, startY));
            startY += 30;

            // Vẽ tiêu đề hóa đơn
            g.DrawString("HÓA ĐƠN THANH TOÁN", headerFont, Brushes.Black, new Point(startX + 80, startY));
            startY += 30;

            // Thông tin hóa đơn
            g.DrawString($"Số HĐ: {hoaDon.MaDonHang}", normalFont, Brushes.Black, new Point(startX, startY));
            g.DrawString($"Ngày: {hoaDon.NgayThanhToan?.ToString("dd/MM/yyyy HH:mm")}", normalFont, Brushes.Black, new Point(startX + 200, startY));
            startY += offset;

            g.DrawString($"Thu ngân: {hoaDon.HoTen}", normalFont, Brushes.Black, new Point(startX, startY));
            g.DrawString($"Bàn: {hoaDon.TenBan}", normalFont, Brushes.Black, new Point(startX + 200, startY));
            startY += 30;

            // Vẽ header của bảng chi tiết
            g.DrawString("STT", headerFont, Brushes.Black, new Point(startX, startY));
            g.DrawString("Tên món", headerFont, Brushes.Black, new Point(startX + 50, startY));
            g.DrawString("SL", headerFont, Brushes.Black, new Point(startX + 200, startY));
            g.DrawString("Đơn giá", headerFont, Brushes.Black, new Point(startX + 250, startY));
            g.DrawString("Thành tiền", headerFont, Brushes.Black, new Point(startX + 350, startY));
            startY += offset;

            // Vẽ line
            g.DrawLine(new Pen(Color.Black), new Point(startX, startY), new Point(startX + 450, startY));
            startY += 5;

            // Vẽ chi tiết món ăn
            int stt = 1;
            foreach (DataGridViewRow row in dgvChiTiet.Rows)
            {
                if (row.Cells[0].Value != null)
                {
                    g.DrawString(stt.ToString(), normalFont, Brushes.Black, new Point(startX, startY));
                    g.DrawString(row.Cells[2].Value.ToString(), normalFont, Brushes.Black, new Point(startX + 50, startY));
                    g.DrawString(row.Cells[1].Value.ToString(), normalFont, Brushes.Black, new Point(startX + 200, startY));
                    decimal thanhTien = Convert.ToDecimal(row.Cells[3].Value);
                    decimal donGia = thanhTien / Convert.ToDecimal(row.Cells[1].Value);
                    g.DrawString(donGia.ToString("#,##0"), normalFont, Brushes.Black, new Point(startX + 250, startY));
                    g.DrawString(thanhTien.ToString("#,##0"), normalFont, Brushes.Black, new Point(startX + 350, startY));
                    startY += offset;
                    stt++;
                }
            }

            // Vẽ line
            startY += 5;
            g.DrawLine(new Pen(Color.Black), new Point(startX, startY), new Point(startX + 450, startY));
            startY += 20;

            // Vẽ tổng tiền
            g.DrawString("Tổng tiền:", totalFont, Brushes.Black, new Point(startX + 250, startY));
            g.DrawString(hoaDon.TongTien.ToString("#,##0") + " VNĐ", totalFont, Brushes.Black, new Point(startX + 350, startY));
            startY += offset;

            // Vẽ phương thức thanh toán
            g.DrawString("Phương thức TT:", normalFont, Brushes.Black, new Point(startX + 250, startY));
            g.DrawString(hoaDon.TenHinhThuc, normalFont, Brushes.Black, new Point(startX + 350, startY));
            startY += 40;

            // Vẽ footer
            g.DrawString("Cảm ơn quý khách và hẹn gặp lại!", new Font("Arial", 10, FontStyle.Italic), Brushes.Black, new Point(startX + 80, startY));
        }
    }
}