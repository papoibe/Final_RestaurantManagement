using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Imaging; // Cho ImageAttributes và ColorMatrix
using System.IO; // Để sử dụng File.Exists
using TransferObject;
using BusinessLayer;



namespace PresentationLayer
{
    public partial class FrmQuanLyMon : Form
    {
        private MonAn_BL monAnBL;
        private List<MonAn_DTO> listMonAn;

        public FrmQuanLyMon()
        {
            InitializeComponent();
            monAnBL = new MonAn_BL();
        }

        private void LoadMonAn()
        {
            try
            {
                listMonAn = monAnBL.GetAll();

                // Tạo DataTable để hiển thị dữ liệu
                DataTable dt = new DataTable();
                dt.Columns.Add("MaMonAn", typeof(int));
                dt.Columns.Add("TenMonAn", typeof(string));
                dt.Columns.Add("DonGia", typeof(float));
                dt.Columns.Add("TrangThaiText", typeof(string));

                // Chuyển đổi dữ liệu từ List sang DataTable
                foreach (var mon in listMonAn)
                {
                    string trangThaiText = mon.TrangThai ? "Còn hàng" : "Hết hàng";
                    dt.Rows.Add(mon.MaMonAn, mon.TenMonAn, mon.DonGia, trangThaiText);
                }

                dgvFood.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải danh sách món ăn: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void SetupDataGridView()
        {
            // Thiết lập hiển thị các cột trong DataGridView
            dgvFood.AutoGenerateColumns = false;
            // Xóa tất cả các cột hiện có (nếu có)
            dgvFood.Columns.Clear();

            // Thêm cột Mã món ăn
            DataGridViewTextBoxColumn colMaMonAn = new DataGridViewTextBoxColumn();
            colMaMonAn.DataPropertyName = "MaMonAn";
            colMaMonAn.HeaderText = "Mã món";
            colMaMonAn.Name = "MaMonAn";
            colMaMonAn.Width = 70;
            dgvFood.Columns.Add(colMaMonAn);

            // Thêm cột Tên món ăn
            DataGridViewTextBoxColumn colTenMonAn = new DataGridViewTextBoxColumn();
            colTenMonAn.DataPropertyName = "TenMonAn";
            colTenMonAn.HeaderText = "Tên món ăn";
            colTenMonAn.Width = 200;
            dgvFood.Columns.Add(colTenMonAn);

            // Thêm cột Đơn giá
            DataGridViewTextBoxColumn colDonGia = new DataGridViewTextBoxColumn();
            colDonGia.DataPropertyName = "DonGia";
            colDonGia.HeaderText = "Đơn giá (VNĐ)";
            colDonGia.Width = 100;
            colDonGia.DefaultCellStyle.Format = "N0";
            colDonGia.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvFood.Columns.Add(colDonGia);

            // Thêm cột Trạng thái - sử dụng thuộc tính TrangThaiText
            DataGridViewTextBoxColumn colTrangThai = new DataGridViewTextBoxColumn();
            colTrangThai.DataPropertyName = "TrangThaiText";  // Liên kết với thuộc tính TrangThaiText mới
            colTrangThai.HeaderText = "Trạng thái";
            colTrangThai.Name = "TrangThaiText"; //để vẻ UI
            colTrangThai.Width = 100;
            dgvFood.Columns.Add(colTrangThai);

            // Thiết lập các thuộc tính khác cho DataGridView
            dgvFood.AllowUserToAddRows = false;
            dgvFood.EditMode = DataGridViewEditMode.EditProgrammatically;
            dgvFood.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvFood.MultiSelect = false;
        }

        private void FrmQuanLyMon_Load(object sender, EventArgs e)
        {
            LoadMonAn();
            SetupDataGridView();
        }

        private void btnComplete_Click(object sender, EventArgs e)
        {
            try
            {
                // Kiểm tra xem đã chọn món ăn nào chưa
                if (dgvFood.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Vui lòng chọn một món ăn để thay đổi trạng thái.", "Thông báo",
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // Lấy mã món ăn từ cột DataGridView
                int maMonAn = Convert.ToInt32(dgvFood.SelectedRows[0].Cells["MaMonAn"].Value);

                // Tìm món ăn trong danh sách
                MonAn_DTO selectedMonAn = listMonAn.Find(m => m.MaMonAn == maMonAn);

                if (selectedMonAn != null)
                {
                    // Trạng thái hiện tại
                    bool currentTrangThai = selectedMonAn.TrangThai;
                    bool newTrangThai = !currentTrangThai;

                    // Gọi hàm cập nhật sang trạng thái ngược lại
                    bool success = monAnBL.UpdateTrangThaiMonAn(selectedMonAn.MaMonAn, newTrangThai);

                    if (success)
                    {
                        // Cập nhật trạng thái trong object
                        selectedMonAn.TrangThai = newTrangThai;

                        // Cập nhật nút
                        btnComplete.Text = newTrangThai ? "TẮT" : "PHỤC VỤ";
                        btnComplete.BackColor = newTrangThai ? Color.Red : Color.LimeGreen;

                        // Refresh nhẹ hoặc reload toàn bộ nếu có xử lý filter/sort
                        LoadMonAn();
                    }
                    else
                    {
                        MessageBox.Show("Không thể thay đổi trạng thái món ăn.", "Lỗi",
                                       MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Không tìm thấy thông tin món ăn đã chọn.", "Lỗi",
                                   MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi cập nhật trạng thái món ăn: " + ex.Message, "Lỗi",
                               MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvFood_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            // Nếu đang xử lý cột TrangThaiText
            if (e.ColumnIndex >= 0 &&
                dgvFood.Columns[e.ColumnIndex].DataPropertyName == "TrangThaiText" &&
                e.Value != null)
            {
                // Màu xanh cho "Còn hàng", màu đỏ cho "Hết hàng"
                if (e.Value.ToString() == "Còn hàng")
                {
                    e.CellStyle.ForeColor = Color.Green;
                }
                else if (e.Value.ToString() == "Hết hàng")
                {
                    e.CellStyle.ForeColor = Color.Red;
                }
            }
        }

        private void dgvFood_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvFood.SelectedRows.Count <= 0) return;
            try
            {
                // Lấy mã món ăn từ hàng đang chọn
                int maMonAn = Convert.ToInt32(dgvFood.SelectedRows[0].Cells["MaMonAn"].Value);
                // Cập nhật thông tin món ăn từ listMonAn
                var selectedMonAn = listMonAn.Find(m => m.MaMonAn == maMonAn);
                if (selectedMonAn != null)
                {
                    btnComplete.Text = selectedMonAn.TrangThai ? "HẾT" : "PHỤC VỤ";
                    btnComplete.BackColor = selectedMonAn.TrangThai ? Color.Red : Color.LimeGreen;
                    txtName.Text = selectedMonAn.TenMonAn;

                    txtPrice.Text = selectedMonAn.DonGia.ToString("0.##");
                    // Cập nhật hình ảnh món ăn
                    string imagePath = $@"E:\DuPhong\RestaurantManagerment\PresentationLayer\picture\monAn\{maMonAn}.png";
                    if (File.Exists(imagePath))
                    {
                        // Tải hình ảnh
                        Image originalImage = Image.FromFile(imagePath);
                        //vẽ
                        // Nếu TrangThai là true (hết món), hiển thị hình ảnh bình thường
                        if (selectedMonAn.TrangThai == true)
                        {
                            pic.Image = originalImage;
                        }
                        // Nếu TrangThai là false (đang phục vụ), tạo phiên bản nhạt của hình ảnh
                        else
                        {
                            // Tạo bộ lọc màu nhạt cho hình ảnh
                            Bitmap lightImage = new Bitmap(originalImage.Width, originalImage.Height);
                            using (Graphics g = Graphics.FromImage(lightImage))
                            {
                                // Tạo ma trận chuyển đổi màu nhạt
                                ColorMatrix colorMatrix = new ColorMatrix(
                                    new float[][]
                                 {
                                    new float[] {0.65f, 0, 0, 0, 0},     // Giảm thêm độ đậm của màu đỏ
                                    new float[] {0, 0.65f, 0, 0, 0},     // Giảm thêm độ đậm của màu xanh lá
                                    new float[] {0, 0, 0.65f, 0, 0},     // Giảm thêm độ đậm của màu xanh dương
                                    new float[] {0, 0, 0, 1, 0},         // Giữ nguyên độ trong suốt
                                    new float[] {-0.1f, -0.1f, -0.1f, 0, 1} // Giảm độ sáng tổng thể (số âm làm tối hơn)
                            });

                                ImageAttributes attributes = new ImageAttributes();
                                attributes.SetColorMatrix(colorMatrix);

                                g.DrawImage(originalImage,
                                    new Rectangle(0, 0, originalImage.Width, originalImage.Height),
                                    0, 0, originalImage.Width, originalImage.Height,
                                    GraphicsUnit.Pixel, attributes);
                            }
                            pic.Image = lightImage;
                        }

                    }
                    else
                    {
                        pic.Image = null;
                        MessageBox.Show("Không tìm thấy hình ảnh cho món ăn này.", "Thông báo",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Đã xảy ra lỗi: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cboTrangThai_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selected = cboTrangThai.SelectedItem.ToString();

            if (selected == "Tất cả")
            {
                LoadMonAn();
                return;
            }

            bool status = selected == "Còn hàng";
            var filteredList = listMonAn.Where(m => m.TrangThai == status).ToList();

            // Tương tự như dạng hiển thị của LoadMonAn
            DataTable dt = new DataTable();
            dt.Columns.Add("MaMonAn", typeof(int));
            dt.Columns.Add("TenMonAn", typeof(string));
            dt.Columns.Add("DonGia", typeof(float));
            dt.Columns.Add("TrangThaiText", typeof(string));

            foreach (var mon in filteredList)
            {
                string trangThaiText = mon.TrangThai ? "Còn hàng" : "Hết hàng";
                dt.Rows.Add(mon.MaMonAn, mon.TenMonAn, mon.DonGia, trangThaiText);
            }

            dgvFood.DataSource = dt;

        }

        private void dgvFood_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            var row = dgvFood.Rows[e.RowIndex];
            string trangThai = row.Cells["TrangThaiText"].Value?.ToString();

            if (trangThai == "Hết hàng")
            {
                row.DefaultCellStyle.BackColor = Color.LightPink;
            }
            else
            {
                row.DefaultCellStyle.BackColor = Color.Honeydew;
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            string keyword = txtSearch.Text.Trim().ToLower();

            if (string.IsNullOrEmpty(keyword))
            {
                // Nếu không gõ gì, load danh sách đầy đủ
                dgvFood.DataSource = null;
                dgvFood.DataSource = listMonAn;
                return;
            }

            // Lọc danh sách (tìm tên món có chứa từ khóa)
            var ketQua = listMonAn
                            .Where(m => m.TenMonAn.ToLower().Contains(keyword))
                            .ToList();

            dgvFood.DataSource = null;
            dgvFood.DataSource = ketQua;
        }

        private void btnLamMoiTatCa_Click(object sender, EventArgs e)
        {
            try
            {
                // Lọc danh sách các món đang hết hàng (TrangThai = false)
                List<MonAn_DTO> hetHangList = listMonAn.Where(m => m.TrangThai == false).ToList();

                if (hetHangList.Count == 0)
                {
                    MessageBox.Show("Không có món nào đang hết hàng cần cập nhật!", "Thông báo",
                                   MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // Hiển thị thông báo xác nhận
                string message = $"Xác nhận đã sẵn sàng phục vụ toàn bộ món?";
                DialogResult result = MessageBox.Show(message, "Xác nhận",
                                                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    int successCount = 0;

                    // Cập nhật từng món trong danh sách
                    foreach (var monAn in hetHangList)
                    {
                        bool success = monAnBL.UpdateTrangThaiMonAn(monAn.MaMonAn, true);
                        if (success)
                        {
                            // Cập nhật trạng thái trong đối tượng
                            monAn.TrangThai = true;
                            successCount++;
                        }
                    }

                    // Thông báo kết quả
                    if (successCount > 0)
                    {
                        MessageBox.Show($"Đã cập nhật {successCount}/{hetHangList.Count} món sang trạng thái 'Còn hàng'.",
                                       "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // Tải lại danh sách món ăn để cập nhật giao diện
                        LoadMonAn();
                    }
                    else
                    {
                        MessageBox.Show("Không thể cập nhật trạng thái món ăn.", "Lỗi",
                                       MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi cập nhật trạng thái món ăn: " + ex.Message, "Lỗi",
                               MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCapNhat_Click(object sender, EventArgs e)
        {
            try
            {
                // Kiểm tra xem đã chọn món ăn nào chưa
                if (dgvFood.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Vui lòng chọn một món ăn để cập nhật giá.", "Thông báo",
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // Kiểm tra giá nhập vào có hợp lệ không
                if (string.IsNullOrWhiteSpace(txtPrice.Text))
                {
                    MessageBox.Show("Vui lòng nhập giá mới cho món ăn.", "Thông báo",
                                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Parse giá trị đơn giá mới
                if (!float.TryParse(txtPrice.Text, out float newPrice))
                {
                    MessageBox.Show("Giá tiền không hợp lệ. Vui lòng nhập số.", "Lỗi",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Kiểm tra giá trị âm
                if (newPrice <= 0)
                {
                    MessageBox.Show("Giá tiền phải lớn hơn 0.", "Lỗi",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Lấy mã món ăn từ hàng đang chọn
                int maMonAn = Convert.ToInt32(dgvFood.SelectedRows[0].Cells["MaMonAn"].Value);

                // Tìm món ăn đã chọn trong danh sách
                var monAn = listMonAn.Find(m => m.MaMonAn == maMonAn);
                if (monAn == null)
                {
                    MessageBox.Show("Không tìm thấy thông tin món ăn đã chọn.", "Lỗi",
                                   MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Kiểm tra xem giá mới có giống giá hiện tại không
                if (Math.Abs(monAn.DonGia - newPrice) < 0.01) // Sử dụng epsilon để so sánh số thực
                {
                    MessageBox.Show("Giá mới giống với giá hiện tại. Không cần cập nhật.", "Thông báo",
                                   MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // Hiển thị hộp thoại xác nhận
                DialogResult result = MessageBox.Show(
                    $"Xác nhận cập nhật giá món ăn từ {monAn.DonGia.ToString("N0")} VNĐ thành {newPrice.ToString("N0")} VNĐ?",
                    "Xác nhận",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    // Gọi hàm cập nhật giá
                    bool success = monAnBL.UpdatePrice(maMonAn, newPrice);

                    if (success)
                    {
                        // Cập nhật giá trong danh sách
                        monAn.DonGia = newPrice;

                        // Reload danh sách để hiển thị giá mới
                        LoadMonAn();
                    }
                    else
                    {
                        MessageBox.Show("Không thể cập nhật giá món ăn.", "Lỗi",
                                       MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi cập nhật giá món ăn: " + ex.Message, "Lỗi",
                               MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
