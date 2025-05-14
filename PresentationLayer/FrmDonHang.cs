using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using BusinessLayer;
using TransferObject;
namespace PresentationLayer
{
    public partial class FrmDonHang : Form
    {
        private int maDH, index, mahinhthucTT; // mã đơn hàng để load những món cũ và thêm những món mới theo mã đơn hàng, maBan để tạo đơn hàng mới theo mã bàn
        private MonAn_BL monAnBL = new MonAn_BL(); // tạo để load menu lên
        private DonHang_BL donHangBL = new DonHang_BL();// tạo để sử dụng hàm Insert 
        private Ban_BL banBL = new Ban_BL(); // tạo để cập nhật trạng thái bàn
        private DonHang_DTO donHang = new DonHang_DTO(); // tạo để tao
        private ChiTietDonHang_BL chiTietDonHang_BL = new ChiTietDonHang_BL(); // dùng để thêm chitietdonhang_DTO vào CSDL
        private List<MonAn_DTO> dsMonAnTam = new List<MonAn_DTO>();
        private List<ChiTietDonHang_DTO> dsChiTietTam = new List<ChiTietDonHang_DTO>(); // tạo để lưu món ăn tạm thời trước khi lưu vào csdl
        private MonAn_DTO MonAn_DTO; // tạo để lưu món ăn tạm thời trước khi lưu vào csdl
        private ThanhToan_BL thanhToan_BL = new ThanhToan_BL(); // tạo để sử dụng hàm Insert, getHinhThucTT vào bảng hình thức thanh toán
        private KhuyenMai_BL khuyenMai_BL = new KhuyenMai_BL(); // tạo để sử dụng hàm Insert, getHinhThucTT vào bảng hình thức thanh toán
        private Ban_DTO ban;

        public FrmDonHang(DonHang_DTO donHang, Ban_DTO ban) // Hàm này khởi tạo frm là đơn hàng mới được tạo theo mã bàn
        {
            InitializeComponent();
            this.KeyPreview = true;
            this.donHang = donHang;
            this.ban = ban;
        }
        private void FrmDonHang_Load(object sender, EventArgs e)
        {
            LoadMenu(); // load menu món ăn
            RefreshUI(); // cập nhật giao diện
        }

        private void RefreshUI()
        {
            // Cập nhật giao diện
            lb_Date.Text = DateTime.Now.ToString("dd/MM/yyyy"); // ngày tháng năm hiện tại 
            lb_Time.Text = DateTime.Now.ToString("HH:mm tt"); // giờ theo dạng giờ:phút PM hoặc AM
            lb_MaBan.Text = ban.TenBan.ToString(); //gắn tên bàn vào

            if (ban.TrangThai == false) // nếu bàn đã có đơn hàng trước đó 
            {
                LoadMonAnByMaDH(); // load những món của đơn hàng
                btn_ThanhToan.Enabled = true; // bật nút thanh toán
                maDH = donHang.MaDonHang;
                lb_MaDonHang.Text = maDH.ToString(); // gắn mã đơn hàng vào giao diện 
            }
            else
            {
                lb_MaDonHang.Text = string.Empty; // chưa có đơn hàng thì để trống 
            }

            // Tính và cập nhật tổng tiền
            float tongTien = dgv_DonHang.Rows.Cast<DataGridViewRow>()
                .Where(row => row.Cells["col_ThanhTien"].Value != null)
                .Sum(row => float.TryParse(row.Cells["col_ThanhTien"].Value.ToString(), out float thanhTien) ? thanhTien : 0);

            lb_TongTien.Text = tongTien.ToString();
            if (string.IsNullOrEmpty(lb_giamGia.Text))
            {
                lb_giamGia.Text = "0";
            }
            lb_ThanhToan.Text = (tongTien - float.Parse(lb_giamGia.Text)).ToString();
        } // cập nhật lại giao diện ( bàn, mã đơn, các món của đơn hiện tại nếu có, tiền của các món trong đơn hàng )

        // các hàm liên quan đến load đơn hàng cũ 
        private void LoadMonAnByMaDH()// hàm này gọi để load các món ăn trước đó theo mã đơn hàng ( món ăn cũ )
        {
            dgv_DonHang.Rows.Clear();
            // Lấy danh sách món ăn từ cơ sở dữ liệu theo mã hóa đơn
            var danhSachMonAn = monAnBL.GetMonAnByMaDH(donHang.MaDonHang);
            foreach (var monAn in danhSachMonAn)
            {
                dgv_DonHang.Rows.Add(monAn.MaChiTiet, monAn.TenMonAn, monAn.SoLuong, monAn.DonGia, monAn.ThanhTien, monAn.GhiChu);
            }
            //ThanhToan_DTO
        }

        // các hàm liên quan đến chức năng trong form
        private void txt_TimMon_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true; // tắt âm thanh tiếng ấn nút
                fLP_Menu.Controls.Clear(); // clear menu cũ để load các món được tìm thấy 

                var danhSachMonAn = monAnBL.GetMonAnByTen(txt_TimMon.Text);
                if (string.IsNullOrWhiteSpace(txt_TimMon.Text) || !danhSachMonAn.Any()) // nếu mà trong text không có ký tự gì 
                {
                    MessageBox.Show("Không tìm thấy món ăn phù hợp!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information); // thông báo không tìm thấy 
                    LoadMenu(); // load lại menu 
                    return;
                }
                else // tìm thấy món
                {
                    AddBackButton(); // thêm nút quay lại 
                    foreach (var monAn in danhSachMonAn) // load các món vào fLP_menu
                    {
                        AddDishButton(monAn);
                    }
                }
            }
        } // tìm kiếm món ăn theo tên 
        private void btn_GhiChu_Click(object sender, EventArgs e)
        {
            if (dgv_DonHang.CurrentRow == null)
            {
                MessageBox.Show("Vui lòng chọn một dòng trong danh sách.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            DataGridViewRow row = dgv_DonHang.CurrentRow; // Lấy dòng hiện tại
            int index = index_chiTietTam(row.Index); // Lưu lại chỉ số dòng được chọn trong dschitiettam ( vị trí của món chưa lưu vào csdl )
            string currentNote = row.Cells["col_Note"].Value?.ToString() ?? ""; // Lấy ghi chú cũ
            // Tạo và cấu hình TextBox để chỉnh sửa ghi chú
            var txtGhiChu = new TextBox
            {
                Width = 400,
                Height = 400,
                Multiline = true,
                Font = new Font("Arial", 16),
                Location = new Point(6, 142),
                Text = currentNote
            };

            txtGhiChu.KeyDown += (s, args) =>
            {
                if (args.KeyCode == Keys.Enter)
                {
                    args.SuppressKeyPress = true; // Ngăn tiếng beep và xuống dòng
                    string newNote = txtGhiChu.Text;

                    // Cập nhật ghi chú cho món đã lưu trong database
                    if (row.Cells["col_MaChiTiet"].Value is int ma && ma != 0)
                    {
                        var chiTiet = chiTietDonHang_BL.GetChiTietDonHang(ma);
                        chiTiet.GhiChu = newNote;
                        chiTietDonHang_BL.Update(chiTiet);
                    }
                    // Cập nhật ghi chú cho món chưa lưu trong dsChiTietTam
                    else if (index >= 0 && index < dsChiTietTam.Count)
                    {
                        dsChiTietTam[index].GhiChu = newNote;
                    }

                    row.Cells["col_Note"].Value = newNote; // Cập nhật trên DataGridView
                    this.Controls.Remove(txtGhiChu); // Xóa và giải phóng TextBox
                    txtGhiChu.Dispose();
                }
                else if (args.KeyCode == Keys.Escape) // Nếu nhấn Esc thì xóa TextBox
                {
                    this.Controls.Remove(txtGhiChu);
                    txtGhiChu.Dispose(); // Giải phóng tài nguyên
                }
            };

            this.Controls.Add(txtGhiChu); // Thêm TextBox vào form
            txtGhiChu.Focus();
            txtGhiChu.BringToFront();
        } // chỉnh sửa ghi chú món ăn trong dgv đơn hàng
        private void btn_SoLuong_Click(object sender, EventArgs e)
        {
            if (dgv_DonHang.CurrentRow == null)
            {
                MessageBox.Show("Vui lòng chọn một dòng trong danh sách.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            DataGridViewRow row = dgv_DonHang.CurrentRow; // Lấy dòng hiện tại
            int index = index_chiTietTam(row.Index); // Lưu lại chỉ số dòng được chọn trong dschitiettam ( vị trí của món chưa lưu vào csdl )
            int soLuongMoi = (int)num_SL.Value; // Lấy số lượng mới từ NumericUpDown
            // Xử lý món đã lưu (có mã chi tiết) hoặc chưa lưu (trong dsChiTietTam)
            var chiTiet = row.Cells["col_MaChiTiet"].Value is int ma && ma != 0
                ? chiTietDonHang_BL.GetChiTietDonHang(ma)
                : dsChiTietTam[index]; // Lấy chi tiết món ăn từ DataGridView hoặc danh sách tạm
            chiTiet.SoLuong = soLuongMoi; // Cập nhật số lượng
            if (decimal.TryParse(row.Cells["col_DonGia"].Value?.ToString(), out decimal donGia))
            {
                chiTiet.ThanhTien = (float)donGia * soLuongMoi;
                row.Cells["col_ThanhTien"].Value = chiTiet.ThanhTien;
            }

            row.Cells["col_SoLuong"].Value = soLuongMoi;
            if (chiTiet.MaChiTietDonHang != 0) chiTietDonHang_BL.Change(chiTiet);
            else dsChiTietTam[index] = chiTiet;
            lb_MaChiTiet.Text = $"Mã Chi Tiết: {chiTiet.MaChiTietDonHang}";
            num_SL.Value = 1; // Đặt lại giá trị của NumericUpDown về 1
            CapNhatTongTien();
        } // chỉnh sửa số lượng món ăn trong dgv đơn hàng
        private void btn_xoaMon_Click(object sender, EventArgs e)
        {
            // Kiểm tra nếu có dòng được chọn
            if (dgv_DonHang.CurrentRow == null)
            {
                MessageBox.Show("Vui lòng chọn một dòng trong danh sách.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            DataGridViewRow row = dgv_DonHang.CurrentRow;
            int index = index_chiTietTam(row.Index); // Lấy chỉ số trong dsChiTietTam
            // Xóa món đã lưu hoặc chưa lưu
            if (row.Cells["col_MaChiTiet"].Value is int ma && ma != 0)
                chiTietDonHang_BL.Delete(chiTietDonHang_BL.GetChiTietDonHang(ma));
            else if (index >= 0 && index < dsChiTietTam.Count)
                dsChiTietTam.RemoveAt(index);
            dgv_DonHang.Rows.Remove(row); // Xóa dòng khỏi DataGridView
            CapNhatTongTien();
        } // xóa món ăn trong dgv đơn hàng
        private void btn_ThanhToan_Click(object sender, EventArgs e)
        {

            if (banBL.GetTrangThaiBan(ban.MaBan) == true || dgv_DonHang.RowCount == 0) // Bàn chưa có đơn hàng
            {
                btn_ThanhToan.Enabled = false;// Không cho thanh toán
                return;
            }
            else if (dgv_DonHang.Rows.Cast<DataGridViewRow>().Any(row => !row.IsNewRow && row.Cells["col_MaChiTiet"].Value != null && row.Cells["col_MaChiTiet"].Value.Equals(0))) // Kiểm tra có món chưa lưu
            {
                DialogResult result = MessageBox.Show("Có món chưa được lưu. Bạn có muốn lưu đơn hàng trước khi thanh toán không?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    btn_Luu_Click(sender, e); // Gọi hàm lưu đơn hàng                  
                    HinhThucThanhToan(); // Tiếp tục đến bước chọn hình thức thanh toán
                }
            }
            else
            {
                HinhThucThanhToan(); // Tiếp tục đến bước chọn hình thức thanh toán nếu không có vấn đề
            }
        } // Thanh toán khi có đơn hàng và các món ăn đã có mã chi tiết
        private void btn_Luu_Click(object sender, EventArgs e)
        {
            if (banBL.GetTrangThaiBan(ban.MaBan) == true) // bàn chưa có đơn hàng
                donHangBL.Insert(donHang); // Lưu đơn hàng vào cơ sở dữ liệu    

            maDH = donHangBL.GetLastId(ban.MaBan); // Lấy mã đơn hàng vừa được tạo

            // Thêm bản ghi vào bảng HoaDonThanhToan nếu chưa có
            var hoaDonThanhToan_BL = new HoaDonThanhToan_BL();
            var hoaDonMoi = new HoaDonThanhToan_DTO
            {
                MaDonHang = maDH,
                NgayDatHang = DateTime.Now,
                NgayThanhToan = null,
                MaNhanVien = 1, // hoặc lấy từ context đăng nhập
                TongTien = 0,
                MaHinhThuc = null,
                TrangThai = "Chưa thanh toán",
                GhiChu = "Chưa thanh toán"
            };
            hoaDonThanhToan_BL.Insert(hoaDonMoi);

            lb_MaDonHang.Text = $"Mã đơn: {maDH}";
            if (dsChiTietTam.Count > 0) { btn_ThanhToan.Enabled = true; } // nếu có món ăn tạm thì bật nút thanh toán
            foreach (var monAn in dsChiTietTam)// LƯU MÓN ĂN VÀO BẢNG CHI TIẾT ĐƠN HÀNG ( MaDonHang, MaMonAn, SoLuong, DonGia, ThanhTien,GhiChu)
            {
                ChiTietDonHang_DTO chiTiet = new ChiTietDonHang_DTO // mã món, số lượng, đơn giá, thành tiền, ghi chú
                {
                    MaDonHang = maDH,
                    MaMonAn = monAn.MaMonAn,
                    SoLuong = monAn.SoLuong,
                    DonGia = monAn.DonGia,
                    ThanhTien = monAn.ThanhTien,
                    GhiChu = monAn.GhiChu
                };
                chiTietDonHang_BL.Insert(chiTiet);
            }
            dsChiTietTam.Clear(); // Xóa danh sách tạm

            donHang = donHangBL.GetById(maDH); // Lấy lại đơn hàng vừa được tạo
            donHang.TongTien = donHangBL.GetTongTien(maDH); // Cập nhật tổng tiền cho đơn hàng
            donHang.GiamGia = float.Parse(lb_giamGia.Text); // Cập nhật giảm giá cho đơn hàng
            donHangBL.Update(donHang); // Cập nhật đơn hàng
            // Cập nhật lại giao diện
            ban.TrangThai = false; // cập nhật trạng thái bàn
            RefreshUI();
            banBL.CapNhatTrangThaiBan(ban.MaBan, false); // cập nhật trạng thái bàn

        } // tạo mới đơn hàng bằng mã bàn và lưu các món ăn tạm vào bảng Chitietdonhang
        private void btn_cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        } // đóng form đơn hàng
        private void LoadTableButtons(EventHandler clickHandler, bool requireSelection = false, bool targetEmptyTables = false)
        {
            if (requireSelection && dgv_DonHang.SelectedRows.Count == 0) // Kiểm tra nếu không có món ăn nào được chọn
            {
                MessageBox.Show("Vui lòng chọn món ăn để tách!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            try
            {
                fLPBan.Visible = true;
                fLPBan.Controls.Clear();
                foreach (var ban in banBL.GetAll())
                {
                    if (targetEmptyTables && !ban.TrangThai) continue; // Chỉ show bàn trống (trạng thái bàn = true)
                    if (!targetEmptyTables && ban.TrangThai) continue; // Chỉ show bàn đã có đơn hàng (trạng thái bàn = false)
                    string name = ban.TrangThai ? ban.TenBan : $"{ban.TenBan}\n \n{donHangBL.GetTongTien(donHangBL.GetLastId(ban.MaBan))}";
                    Button btnBan = new Button
                    {
                        Text = name,
                        Width = 100,
                        Height = 100,
                        BackColor = ban.TrangThai ? Color.LightGreen : Color.LightYellow,
                        ForeColor = Color.Black,
                        Font = new Font("Arial", 12, FontStyle.Bold),
                        Tag = ban
                    };
                    if (clickHandler != null)
                    {
                        btnBan.Click += clickHandler;
                    }
                    fLPBan.Controls.Add(btnBan);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải danh sách bàn: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        } // load danh sách bàn cho chức năng chuyển bàn, gộp bàn, tách món
        private void btn_chuyenBan_Click(object sender, EventArgs e) => LoadTableButtons(BtnChuyenBan_Click, targetEmptyTables: true);
        private void btn_gopBan_Click(object sender, EventArgs e) => LoadTableButtons(BtnGopBan_Click);
        private void btn_tachMon_Click(object sender, EventArgs e) => LoadTableButtons(BtnTachMon_Click, requireSelection: true);
        private void btn_giamGia_Click(object sender, EventArgs e)
        {
            fLP_Menu.Controls.Clear();
            AddBackButton();
            var khuyenMai = khuyenMai_BL.GetAllKhuyenMai();
            foreach (var km in khuyenMai)
            {
                if (km.TrangThai == true && km.NgayBatDau <= DateTime.Now && km.NgayKetThuc >= DateTime.Now)
                {
                    var btn = new Button
                    {
                        Text = $"{km.TenKhuyenMai}\n{km.PhanTramGiam}%",
                        Size = new Size(140, 170),
                        BackColor = Color.LightGreen,
                        ForeColor = Color.Black,
                        Font = new Font("Arial", 12, FontStyle.Bold),
                        Tag = km // Lưu thông tin loại món ăn trong thuộc tính Tag
                    };
                    btn.Click += (s, e2) =>
                    {
                        // Xử lý sự kiện khi người dùng click vào loại món ăn
                        KhuyenMai_DTO kh = btn.Tag as KhuyenMai_DTO;
                        if (khuyenMai != null)
                        {
                            // Cập nhật giá trị giảm giá
                            lb_giamGia.Text = (float.Parse(lb_TongTien.Text) * kh.PhanTramGiam / 100).ToString();
                            CapNhatTongTien();
                        }
                    };
                    fLP_Menu.Controls.Add(btn);
                }
            }
        } // giảm giá hóa đơn

        // các hàm liên quan đến chọn món ăn
        private void LoadMenu() // hàm này load menu lên flowPanelLayout ( load lên menu theo loại món ăn)
        {
            var loaiMonAn = monAnBL.GetAllLoaiMonAn(); // lấy danh sách loại món ăn từ cơ sở dữ liệu
            foreach (var loai in loaiMonAn)
            {
                Button btnLoaiMonAn = new Button
                {
                    Text = loai.TenLoai,
                    Size = new Size(140, 140),
                    BackColor = Color.LightGreen,
                    ForeColor = Color.Black,
                    Font = new Font("Arial", 12, FontStyle.Bold),
                    Tag = loai // Lưu thông tin loại món ăn trong thuộc tính Tag
                };
                btnLoaiMonAn.Click += BtnLoaiMonAn_Click;
                fLP_Menu.Controls.Add(btnLoaiMonAn);
            }
        }
        private void BtnLoaiMonAn_Click(object sender, EventArgs e)  // hàm này xử lý sự kiện khi người dùng click vào loại món ăn ( load món ăn theo loại)
        {
            // Xử lý sự kiện khi người dùng click vào loại món ăn
            Button btnClicked = sender as Button;
            LoaiMonAn_DTO loaiMonAn = btnClicked.Tag as LoaiMonAn_DTO;
            if (loaiMonAn != null)
            {
                // Xóa các nút món ăn cũ
                fLP_Menu.Controls.Clear();
                // Tạo lại nút quay lại
                AddBackButton();
                foreach (var monAn in monAnBL.GetMonAnByLoai(loaiMonAn.MaLoai))
                {
                    AddDishButton(monAn); // Thêm món vào fLP_Menu theo dạng button
                }
            }
        }
        // thay đổi lại để load menu và button món nhanh hơn 
        private void AddDishButton(MonAn_DTO monAn)
        {
            // Tạo một button mới với thông tin món ăn (tên và giá) và định dạng
            var btn = new Button
            {
                Text = $"{monAn.TenMonAn}\n{monAn.DonGia}", // Hiển thị tên món và giá
                TextAlign = ContentAlignment.BottomCenter, // Căn giữa văn bản ở dưới
                Size = new Size(140, 170), // Kích thước button
                BackColor = Color.LightBlue, // Màu nền
                ForeColor = Color.OrangeRed, // Màu chữ
                Font = new Font("Arial", 12, FontStyle.Bold), // Font chữ
                Tag = monAn // Lưu thông tin món ăn để sử dụng sau
            };
            SetDishImage(btn, monAn); // Gán hình ảnh cho button
            btn.Click += BtnMonAn_Click; // Gán sự kiện click
            fLP_Menu.Controls.Add(btn); // Thêm button vào FlowLayoutPanel
        }// thêm món ăn vào menu 
        private void SetDishImage(Button btn, MonAn_DTO monAn)
        {
            // Tạo đường dẫn đến hình ảnh của món ăn
            string imagePath = Path.Combine(@"E:\DuPhong\RestaurantManagerment\PresentationLayer\picture\monAn", $"{monAn.MaMonAn}.png");
            if (File.Exists(imagePath)) // Kiểm tra nếu file hình ảnh tồn tại
            {
                using (var stream = new MemoryStream(File.ReadAllBytes(imagePath))) // Đọc file ảnh vào MemoryStream
                {
                    btn.BackgroundImage = Image.FromStream(stream); // Gán ảnh làm nền
                }
            }
            else // Nếu không tìm thấy ảnh món ăn
            {
                string defaultPath = Path.Combine(@"E:\DuPhong\RestaurantManagerment\PresentationLayer\picture\monAn", "default.png");
                if (File.Exists(defaultPath)) // Kiểm tra file ảnh mặc định
                {
                    using (var stream = new MemoryStream(File.ReadAllBytes(defaultPath))) // Đọc file ảnh mặc định
                    {
                        btn.BackgroundImage = Image.FromStream(stream); // Gán ảnh mặc định làm nền
                    }
                }
            }
            btn.BackgroundImageLayout = ImageLayout.Zoom; // Đặt chế độ hiển thị ảnh (phóng to/thu nhỏ giữ tỷ lệ)
        }// set hình ảnh của button món




        private void BtnMonAn_Click(object sender, EventArgs e)
        {
            // Xử lý sự kiện khi người dùng click vào món ăn
            Button btnClicked = sender as Button;
            MonAn_DTO monAn = btnClicked.Tag as MonAn_DTO;
            if (monAn != null && monAn.MaLoai == 2)
            {
                MonAn_DTO = monAn; // lưu món ăn tạm thời
                gB_TuyChonMon.Visible = true;
                lB_capDo.Visible = true; // hiện label cấp độ
            }
            else
            {
                MonAn_DTO = monAn;
                gB_TuyChonMon.Visible = true;
                label6.Visible = false;
                lB_capDo.Visible = false;
            }
            number_SoLuong.Value = num_SL.Value; // Đặt giá trị mặc định cho NumericUpDown là 1
            btn_xacNhan.Focus(); // Đặt focus vào NumericUpDown
        }  //  xử lý sự kiện khi người dùng click vào món ăn ( hiện lên gb_TuyChonMon cho phép chọn số lượng, cấp độ hoặc ghi chú )
        private void btn_xacNhan_Click(object sender, EventArgs e)
        {
            if (num_SL.Value == 0) // nếu số lượng món ăn là 0
            {
                MessageBox.Show("Vui lòng chọn số lượng món ăn!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                num_SL.Focus(); // Đặt focus vào NumericUpDown
            }
            else
            {
                number_SoLuong.Value = num_SL.Value; // lấy số lượng món ăn từ NumericUpDown
                int sl = (int)number_SoLuong.Value; // lấy số lượng món ăn từ NumericUpDown
                string ghichu = rTB_ghiChu.Text;
                if (lB_capDo.Visible == true) // nếu có cấp độ
                {
                    ghichu = "Cấp độ " + lB_capDo.Text + "\n" + rTB_ghiChu.Text; // lấy ghi chú từ label
                }

                dgv_DonHang.Rows.Add(0, MonAn_DTO.TenMonAn, sl, MonAn_DTO.DonGia, MonAn_DTO.DonGia * sl, ghichu); // thêm món ăn vào dgv_DonHang
                dsMonAnTam.Add(MonAn_DTO);
                ChiTietDonHang_DTO mon = new ChiTietDonHang_DTO
                {
                    MaMonAn = MonAn_DTO.MaMonAn,
                    SoLuong = sl, // Số lượng mặc định là 1
                    DonGia = MonAn_DTO.DonGia,
                    ThanhTien = MonAn_DTO.DonGia * sl, // Thành tiền (1 * Đơn giá)
                    GhiChu = ghichu // Ghi chú mặc định là rỗng
                };
                num_SL.Value = 1; // Đặt giá trị mặc định cho NumericUpDown là 1
                dsChiTietTam.Add(mon);// thêm món ăn vào danh sách tạm
                gB_TuyChonMon.Visible = false; // ẩn groupbox tùy chọn món ăn

                CapNhatTongTien(); // Cập nhật tổng tiền trên giao diện
            }
        }   // xử lý sự kiện sau khi người dùng đã hoàn thành các tác vụ ở gb_TuyChonMon ( chọn số lượng, ghi chú, cấp độ ) thì ấn xác nhận để thêm món ăn vào dgv_DonHang
        private void btn_xacNhan_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && gB_TuyChonMon.Visible)
            {
                btn_xacNhan.PerformClick(); // Giả lập nhấn nút Xác Nhận
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        } // hàm này để bỏ qua khi không muốn thay đổi thông tin mặc định ở gb_TuyChonMon (bỏ qua bước tùy chọn)
        private void btn_Huy_Click(object sender, EventArgs e)
        {
            gB_TuyChonMon.Visible = false; // ẩn groupbox tùy chọn món ăn
        } // vào tùy chọn nhưng không muốn chọn nữa.


        // các hàm liên quan đến thanh toán sau khi ấn vào button thanh toán
        private void HinhThucThanhToan()
        {
            fLP_Menu.Controls.Clear();
            AddBackButton(); // thêm nút quay lại

            var hinhThucTT_DTOs = thanhToan_BL.GetHinhThucTT(); // Lấy danh sách hình thức thanh toán từ cơ sở dữ liệu

            // Tạo các nút cho các phương thức thanh toán
            foreach (var htTT in hinhThucTT_DTOs)
            {
                Button btn = new Button
                {
                    Text = htTT.tenHinhThucTT, // Sửa thành tenHinhThucTT
                    Size = new Size(140, 170),
                    BackColor = Color.LightGreen,
                    ForeColor = Color.Black,
                    Font = new Font("Arial", 12, FontStyle.Bold),
                    Tag = htTT // Lưu thông tin hình thức thanh toán vào Tag
                };

                btn.Click += (s, e) =>
                {
                    HinhThucTT_DTO hinhThuc = btn.Tag as HinhThucTT_DTO; // Lấy thông tin hình thức thanh toán từ Tag
                    mahinhthucTT = hinhThuc.maHinhThucTT; // Lưu mã hình thức thanh toán vào biến toàn cục
                    switch (hinhThuc.maHinhThucTT) // Sửa thành tenHinhThucTT
                    {
                        case 1:
                        case 2:
                            fLP_Menu.Controls.Clear(); // Xóa các nút hiện tại trong fLP_Menu
                            fLP_Menu.Controls.Add(gBHinhThucTT); // Thêm gBHinhThucTT vào fLP_Menu
                            gBHinhThucTT.Visible = true; // Đảm bảo groupbox hiển thị
                            txt_SoTien.Text = String.Empty; // Đặt lại giá trị textbox
                            txt_SoTien.Focus(); // Đặt focus vào textbox
                            AddBackButton();
                            break;
                        case 3:
                        case 4:
                            ShowQRCode(hinhThuc); // Sửa thành tenHinhThucTT
                            break;
                        default:
                            MessageBox.Show("Hình thức thanh toán không được hỗ trợ!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            break;
                    }
                };
                fLP_Menu.Controls.Add(btn);
            }
        } // hàm này để chọn hình thức thanh toán
        private void txt_SoTien_KeyDown(object sender, KeyEventArgs e)
        {
            // xử lý khi ấn enter ở txt số tiền 
            // hiện ra button tiền thừa
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true; // Không phát tiếng beep
                string input = txt_SoTien.Text;
                if (decimal.TryParse(input, out decimal soTien))
                {
                    // Lấy tổng tiền từ lb_TongTien
                    if (decimal.TryParse(lb_ThanhToan.Text, out decimal tongTien))
                    {
                        decimal tienThua = soTien - tongTien; // Tính số tiền thừa
                        if (tienThua < 0)
                        {
                            MessageBox.Show("Số tiền đưa không đủ! Vui lòng kiểm tra lại.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        else
                        {
                            // Tạo button "Tiền thừa" trong fLP_Menu
                            Button btnTienThua = new Button
                            {
                                Text = $"Tiền thừa: {tienThua.ToString():NO} VND",
                                Size = new Size(140, 170),
                                BackColor = Color.LightGreen,
                                ForeColor = Color.Black,
                                Font = new Font("Arial", 12, FontStyle.Bold),
                                Tag = tienThua // Lưu số tiền thừa vào Tag
                            };
                            // Thêm button vào fLP_Menu
                            fLP_Menu.Controls.Add(btnTienThua);
                            // Xử lý sự kiện khi nhấn button "Tiền thừa"
                            btnTienThua.Click += (s, e2) =>
                            {
                                decimal soTienThua = (decimal)btnTienThua.Tag; // Lấy số tiền thừa từ Tag
                                HoanTatThanhToan(); // Hoàn tất thanh toán
                            };
                        }
                    }
                    else
                    {
                        MessageBox.Show("Tổng tiền không hợp lệ! Vui lòng kiểm tra lại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else
                {
                    MessageBox.Show("Số tiền không hợp lệ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        } // hàm này để nhập số tiền khi hình thức thanh toán là tiền mặt hoặc tính dụng
        private void ShowQRCode(HinhThucTT_DTO phuongThuc)
        {
            Form qrForm = new Form
            {
                Text = "Mã QR " + phuongThuc.tenHinhThucTT,
                Size = new Size(500, 700),
                StartPosition = FormStartPosition.CenterParent
            };

            PictureBox qrPic = new PictureBox
            {
                Dock = DockStyle.Fill,
                SizeMode = PictureBoxSizeMode.StretchImage
            };

            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory; // Thư mục chạy ứng dụng (bin\Debug hoặc bin\Release)
            string imageDirectory = Path.Combine(baseDirectory, @"..\..\PresentationLayer\picture"); // Đường dẫn ngược về project

            // Load hình ảnh từ thư mục
            if (phuongThuc.maHinhThucTT == 3) // Ngân hàng
            {
                qrForm.Text = "Mã QR Thanh Toán Ngân Hàng";
                string imagePath = Path.Combine(imageDirectory, "nganhang.jpg");
                if (File.Exists(imagePath))
                {
                    qrPic.Image = Image.FromFile(imagePath);
                }
                else
                {
                    MessageBox.Show($"Không tìm thấy hình nganhang.jpg tại: {imagePath}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else if (phuongThuc.maHinhThucTT == 4) // MoMo
            {
                qrForm.Text = "Mã QR Thanh Toán MoMo";
                string imagePath = Path.Combine(imageDirectory, "momo.jpg");
                if (File.Exists(imagePath))
                {
                    qrPic.Image = Image.FromFile(imagePath);
                }
                else
                {
                    MessageBox.Show($"Không tìm thấy hình momo.jpg tại: {imagePath}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            Button btnXacNhan = new Button
            {
                Text = "Xác nhận đã chuyển",
                Font = new Font("Arial", 20, FontStyle.Bold),
                Dock = DockStyle.Bottom,
                Height = 100,
                BackColor = Color.LightGreen
            };
            btnXacNhan.Click += (s, e) =>
            {
                qrForm.Close();
                MessageBox.Show("Xác nhận đã chuyển khoản thành công!", "Thành công");
                HoanTatThanhToan();
            };
            qrForm.Controls.Add(btnXacNhan);
            qrForm.Controls.Add(qrPic);
            qrForm.ShowDialog();
        }
        private void HoanTatThanhToan()
        {
            // lưu vào csdl
            donHang = donHangBL.GetById(maDH);
            donHang.GiamGia = float.Parse(lb_giamGia.Text); // cập nhật giảm giá
            donHang.TongTien = float.Parse(lb_ThanhToan.Text); // cập nhật tổng tiền
            donHang.MaTrangThai = 4;
            donHangBL.Update(donHang); // cập nhật đơn hàng
            
            // Bổ sung cập nhật trạng thái thanh toán vào bảng HoaDonThanhToan
            var hoaDonThanhToan_BL = new HoaDonThanhToan_BL();
            // Lấy đúng hóa đơn theo MaDonHang để update, không insert mới
            var hoaDon = hoaDonThanhToan_BL.GetAll().FirstOrDefault(hd => hd.MaDonHang == maDH);
            if (hoaDon != null)
            {
                hoaDon.NgayThanhToan = DateTime.Now;
                hoaDon.TongTien = (decimal)donHang.TongTien;
                hoaDon.MaHinhThuc = mahinhthucTT;
                hoaDon.TrangThai = "Đã thanh toán";
                hoaDon.GhiChu = "Thanh toán thành công";
                hoaDonThanhToan_BL.Update(hoaDon);
            }
            // Bổ sung lại insert vào bảng ThanhToan
            ThanhToan_DTO thanhToan_DTO = new ThanhToan_DTO
            {
                MaDonHang = donHang.MaDonHang,
                MaNhanVien = 1, // hoặc lấy từ context đăng nhập
                MaHinhThucTT = mahinhthucTT,
                SoTien = (float)donHang.TongTien,
                NgayThanhToan = DateTime.Now,
                GhiChu = "Thanh toán thành công"
            };
            thanhToan_BL.Insert(thanhToan_DTO);
            banBL.CapNhatTrangThaiBan(ban.MaBan, true); // cập nhật trạng thái bàn về trống
            this.Close();
        }


        // hàm phụ trợ
        private void dgv_DonHang_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < dgv_DonHang.Rows.Count)
            {
                DataGridViewRow selectedRow = dgv_DonHang.Rows[e.RowIndex];
                // Lấy giá trị cột MaChiTiet (giả sử tên cột là "col_MaChiTiet")
                object value = selectedRow.Cells["col_MaChiTiet"].Value;
                // Hiển thị giá trị (có thể dùng MessageBox hoặc gán vào label)
                lb_MaChiTiet.Text = "Mã Chi Tiết: " + value?.ToString();
                // Hiển thị số lượng trong NumericUpDown
                if (int.TryParse(selectedRow.Cells["col_SoLuong"].Value?.ToString(), out int soLuong))
                {
                    num_SL.Value = soLuong;
                }
            }
        } // 
        private int index_chiTietTam(int selected_index)// hàm này để lấy chỉ số trong dsChiTietTam trên dgv_DonHang
        {
            int count = 0;
            foreach (DataGridViewRow row in dgv_DonHang.Rows)
            {
                // Đảm bảo không phải là dòng mới (new row) trong chế độ AllowUserToAddRows = true
                if (!row.IsNewRow)
                {
                    // Giả sử cột có tên là "MaChiTiet"
                    var cellValue = row.Cells["col_MaChiTiet"].Value;

                    // Chuyển về int và kiểm tra khác 0
                    if (cellValue != null && int.TryParse(cellValue.ToString(), out int maChiTiet))
                    {
                        if (maChiTiet != 0)
                        {
                            count++;
                        }
                    }
                }
            }
            return selected_index - count;
        }
        private void CapNhatTongTien()// được gọi để hiển thị tổng tiền từ dgv_DonHang
        {
            float tongTien = dgv_DonHang.Rows.Cast<DataGridViewRow>()
                .Where(row => row.Cells["col_ThanhTien"].Value != null)
                .Sum(row => float.TryParse(row.Cells["col_ThanhTien"].Value.ToString(), out float thanhTien) ? thanhTien : 0);
            lb_TongTien.Text = tongTien.ToString();
            if (string.IsNullOrEmpty(lb_giamGia.Text))
            {
                lb_giamGia.Text = "0";
            }
            lb_ThanhToan.Text = (tongTien - float.Parse(lb_giamGia.Text)).ToString(); // Cập nhật tổng tiền thanh toán
        }
        private void BtnTachMon_Click(object sender, EventArgs e)
        {
            Button btnBan = sender as Button;
            Ban_DTO banCD = btnBan.Tag as Ban_DTO;
            int maDHTach = donHangBL.GetLastId(banCD.MaBan);
            foreach (DataGridViewRow row in dgv_DonHang.SelectedRows)
            {
                if (row.Cells["col_MaChiTiet"].Value != null && int.TryParse(row.Cells["col_MaChiTiet"].Value.ToString(), out int maChiTiet))
                {
                    var chiTiet = chiTietDonHang_BL.GetChiTietDonHang(maChiTiet);
                    if (chiTiet != null)
                    {
                        chiTiet.MaDonHang = maDHTach; // cập nhật mã đơn hàng
                        chiTietDonHang_BL.UpdateMaDH(chiTiet); // cập nhật chi tiết đơn hàng
                    }
                }
            }
            fLPBan.Visible = false;
            //donHang = donHangBL.GetById(maDHTach); // Lấy đơn hàng của bàn gộp
            DonHang_DTO temp = donHangBL.GetById(maDHTach); // Lấy đơn hàng của bàn gộp
            if (temp != null)
            {
                temp.TongTien = donHangBL.GetTongTien(maDHTach);
                donHang.TongTien = donHangBL.GetTongTien(donHang.MaDonHang);// Cập nhật tổng tiền
                donHangBL.Update(donHang);
                donHangBL.Update(temp);// Cập nhật đơn hàng gộp
            }
            dgv_DonHang.Rows.Clear(); // Xóa tất cả các dòng trong DataGridView
            RefreshUI();
        } // xử lý sự kiện khi tách món
        private void BtnChuyenBan_Click(object sender, EventArgs e)
        {

            // Lấy thông tin bàn từ thuộc tính Tag của button
            Button btnBan = sender as Button;
            Ban_DTO banCD = btnBan.Tag as Ban_DTO;
            if (banCD != null && banCD.TrangThai == true) // bàn trống
            {
                banBL.CapNhatTrangThaiBan(ban.MaBan, true); // cập nhật trạng thái bàn
                ban = banCD; // lưu bàn đã chọn vào biến toàn cục
                donHang.MaBan = ban.MaBan; // cập nhật mã bàn cho đơn hàng
                donHangBL.Update(donHang); // cập nhật đơn hàng
                lb_MaBan.Text = "Tên bàn: " + ban.TenBan; // cập nhật mã bàn lên label
                banBL.CapNhatTrangThaiBan(ban.MaBan, false); // cập nhật trạng thái bàn
            }
            fLPBan.Visible = false;
        } // xử lý sự kiện khi click chuyển bàn

        private void dgv_DonHang_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void BtnGopBan_Click(object sender, EventArgs e)
        {
            // Lấy thông tin bàn từ thuộc tính Tag của button
            Button btnBan = sender as Button;
            Ban_DTO banCD = btnBan.Tag as Ban_DTO;
            if (banCD != null && banCD.TrangThai == false) // bàn đã có đơn hàng
            {
                int maBanHienTai = ban.MaBan; // Lưu mã bàn hiện tại
                int maBanGop = banCD.MaBan; // Lưu mã bàn gộp
                int maDHHienTai = donHang.MaDonHang; // Lưu mã đơn hàng hiện tại
                int maDHGop = donHangBL.GetLastId(maBanGop); // Lấy mã đơn hàng của bàn gộp
                try
                {
                    // Kiểm tra nếu bàn hiện tại và bàn gộp là cùng một bàn
                    if (maBanHienTai == maBanGop)
                    {
                        MessageBox.Show("Không thể gộp bàn với chính nó!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // Lấy danh sách đơn hàng của bàn hiện tại
                    var chiTietDonHangHienTai = chiTietDonHang_BL.GetByMaDonHang(maDHHienTai);
                    var chiTietDonHangGop = chiTietDonHang_BL.GetByMaDonHang(maDHGop);
                    if (chiTietDonHangHienTai == null || chiTietDonHangGop == null)
                    {
                        MessageBox.Show("Không tìm thấy đơn hàng để gộp!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    // Gộp đơn hàng: Chuyển tất cả món ăn từ bàn hiện tại sang bàn gộp
                    foreach (var chiTiet in chiTietDonHangHienTai)
                    {
                        chiTiet.MaDonHang = maDHGop; // Cập nhật mã đơn hàng sang bàn gộp
                        chiTietDonHang_BL.UpdateMaDH(chiTiet); // Cập nhật chi tiết đơn hàng
                    }

                    // Lấy lại đơn hàng gộp để cập nhật
                    donHang.TongTien = 0;
                    donHang.MaTrangThai = 5;
                    donHangBL.Update(donHang); // Cập nhật đơn hàng hiện tại
                    donHang = donHangBL.GetById(maDHGop); // Lấy đơn hàng của bàn gộp
                    if (donHang != null)
                    {
                        donHang.TongTien = donHangBL.GetTongTien(maDHGop); // Cập nhật tổng tiền
                        donHangBL.Update(donHang); // Cập nhật đơn hàng gộp
                    }

                    dsChiTietTam.Clear(); // Xóa danh sách tạm
                    ban = banCD; // Chuyển sang bàn gộp

                    // Cập nhật trạng thái bàn
                    banBL.CapNhatTrangThaiBan(maBanHienTai, true);  // Đặt bàn hiện tại thành trống
                    banBL.CapNhatTrangThaiBan(maBanGop, false);     // Giữ bàn gộp ở trạng thái đang sử dụng

                    // Cập nhật giao diện
                    lb_MaBan.Text = "Tên bàn: " + ban.TenBan; // Cập nhật label
                    lb_MaDonHang.Text = "Mã đơn: " + maDHGop; // Cập nhật mã đơn hàng
                    dgv_DonHang.ClearSelection(); // Xóa chọn các dòng trong DataGridView
                    dgv_DonHang.Rows.Clear(); // Xóa tất cả các dòng trong DataGridView
                    RefreshUI();
                    MessageBox.Show($"Gộp bàn thành công! Đơn hàng từ Bàn {maBanHienTai} đã được gộp vào Bàn {maBanGop}.",
                        "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    fLPBan.Visible = false; // Ẩn danh sách bàn
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi gộp bàn: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Bàn đã chọn không phải là bàn có đơn hàng để gộp!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        } // xử lý sự kiện khi click gọp bàn
        private void AddBackButton()
        {
            Button btnBack = new Button
            {
                Text = "Quay lại",
                Size = new Size(140, 170),
                BackColor = Color.LightBlue,
                ForeColor = Color.Black,
                Font = new Font("Arial", 12, FontStyle.Bold)
            };
            btnBack.Click += (s, e) => { fLP_Menu.Controls.Clear(); LoadMenu(); };
            fLP_Menu.Controls.Add(btnBack);
        }
    }
}
