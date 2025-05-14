# RestaurantManagementProject
========== Hệ Thống Quản Lý Quán Ăn ===========
+======Thành viên đóng góp======== 
+ Lê Nguyễn Phước Thịnh
+ Trần Quốc Thịnh
+ Nguyễn Trọng Thịnh

- Đây là Repo chứa source code hoàn chỉnh của đồ án quản lý nhà hàng/quán ăn mà em/tôi đã Push lên repo mới để tránh việc xung đột
- Lịch sử và quá trình commit được lưu vào đường dẫn của Repo: https://github.com/papoibe/RestaurantManagerment

- Những thay đổi mới cho Project này để trở nên hoàn chỉnh: 
+ thay đổi giao diện của tất cả các Form phù hợp với hệ thống: Phước Thịnh
+ Chỉnh sửa code đúng quy tắt Lập trình cơ sở dữ liệu: Phước Thịnh
+ cập nhật FrmQuanLyMon - logic hoạt động, cập nhật: Trọng Thịnh
+ Chỉnh sửa Logic DonHang - LapOrder - ThanhToan: Quốc Thịnh  
+ chỉnh sửa show tìm kiếm của quản lý hóa đơn (FrmQuanLyHoaDon): bổ sung database HoaDonThanhToan và logic code HoaDonThanhToan: mục đích hiển thị các hóa đơn chưa thanh toán và cập nhật sau khi thanh toán: Phước Thịnh
+ Bổ sung logic: như một hệ thống - Logic phân quyền các cá nhân thực hiện các nghiệp vụ : Phước Thịnh
+ ===các loại báo cáo: Phước Thịnh ======= 
Báo cáo tồn kho
Báo cáo doanh thu theo loại
Các biểu đồ tương ứng
Biểu đồ món ăn bán chạy
Biểu đồ doanh thu theo theo ngày
In hóa đơn
xuất báo cáo - FrmBaoCao: nhấn vào button báo cáo nào thì xuất báo cáo theo dạng đó

- Cách chạy Project
1: Git clone : https://github.com/papoibe/Final_RestaurantManagement.git
2: mở QuanLyQuanAn.sql -> vào SQL Server Management -> đưa script của db sau đó Execute để tạo db 
3: vào project: 
+ Thay đổi đường dẫn source DataLayer/DataProvider: dòng thứ 18 (Tool -> Connect to database)
+ Thay đổi đường dẫn source Presentation/FrmDonHang: dòng thứ 418 - 428 (E:\...\...\PresentationLayer\picture\monAn)
+ Thay đổi đường dẫn source Presentation/FrmQuanLyMon: dòng thứ 205 (E:\...\...\PresentationLayer\picture\monAn)
+ Mở NuGet Package Manager Console trong Visual Studio: Chạy lệnh: Install-Package ClosedXML: (nếu như Install bị lỗi là do trong project đã được cài nhưng .csproj chưa được cập nhật thì: chạy Uninstall-Package ClosedXML sau đó cài đặt lại)

+ Mô hình có thể không được hoàn chỉnh như một hệ thống thực thế chúng tôi/em sẽ cố gắng update để đưa đồ án hoạt động thực tế 





