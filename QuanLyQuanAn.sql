USE [master]
GO
/****** Object:  Database [QuanLyQuanAn]    Script Date: 5/13/2025 10:03:56 PM ******/
CREATE DATABASE [QuanLyQuanAn]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'QuanLyQuanAn', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.THINH\MSSQL\DATA\QuanLyQuanAn.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'QuanLyQuanAn_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.THINH\MSSQL\DATA\QuanLyQuanAn_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [QuanLyQuanAn] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [QuanLyQuanAn].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [QuanLyQuanAn] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [QuanLyQuanAn] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [QuanLyQuanAn] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [QuanLyQuanAn] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [QuanLyQuanAn] SET ARITHABORT OFF 
GO
ALTER DATABASE [QuanLyQuanAn] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [QuanLyQuanAn] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [QuanLyQuanAn] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [QuanLyQuanAn] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [QuanLyQuanAn] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [QuanLyQuanAn] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [QuanLyQuanAn] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [QuanLyQuanAn] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [QuanLyQuanAn] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [QuanLyQuanAn] SET  DISABLE_BROKER 
GO
ALTER DATABASE [QuanLyQuanAn] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [QuanLyQuanAn] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [QuanLyQuanAn] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [QuanLyQuanAn] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [QuanLyQuanAn] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [QuanLyQuanAn] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [QuanLyQuanAn] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [QuanLyQuanAn] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [QuanLyQuanAn] SET  MULTI_USER 
GO
ALTER DATABASE [QuanLyQuanAn] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [QuanLyQuanAn] SET DB_CHAINING OFF 
GO
ALTER DATABASE [QuanLyQuanAn] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [QuanLyQuanAn] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [QuanLyQuanAn] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [QuanLyQuanAn] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
EXEC sys.sp_db_vardecimal_storage_format N'QuanLyQuanAn', N'ON'
GO
ALTER DATABASE [QuanLyQuanAn] SET QUERY_STORE = OFF
GO
USE [QuanLyQuanAn]
GO
/****** Object:  UserDefinedFunction [dbo].[fuChuyenCoDauThanhKhongDau]    Script Date: 5/13/2025 10:03:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[fuChuyenCoDauThanhKhongDau] ( @strInput NVARCHAR(4000) )
RETURNS NVARCHAR(4000)
AS
BEGIN
    IF @strInput IS NULL RETURN @strInput
    IF @strInput = '' RETURN @strInput
    
    DECLARE @RT NVARCHAR(4000)
    DECLARE @SIGN_CHARS NCHAR(136)
    DECLARE @UNSIGN_CHARS NCHAR (136)
    
    SET @SIGN_CHARS = N'ăâđêôơưàảãạáằẳẵặắầẩẫậấèẻẽẹéềểễệếìỉĩịíòỏõọóồổỗộốờởỡợớùủũụúừửữựứỳỷỹỵýĂÂĐÊÔƠƯÀẢÃẠÁẰẲẴẶẮẦẨẪẬẤÈẺẼẸÉỀỂỄỆẾÌỈĨỊÍÒỎÕỌÓỒỔỖỘỐỜỞỠỢỚÙỦŨỤÚỪỬỮỰỨỲỶỸỴÝ' + NCHAR(272) + NCHAR(208)
    SET @UNSIGN_CHARS = N'aadeoouaaaaaaaaaaaaaaeeeeeeeeeeiiiiioooooooooooooouuuuuuuuuuyyyyyAADEOOUAAAAAAAAAAAAAAAEEEEEEEEEEIIIIIOOOOOOOOOOOOOOOUUUUUUUUUUYYYYYDD'
    
    DECLARE @COUNTER int
    DECLARE @COUNTER1 int
    SET @COUNTER = 1
 
    WHILE (@COUNTER <=LEN(@strInput))
    BEGIN
        SET @COUNTER1 = 1
        WHILE (@COUNTER1 <=LEN(@SIGN_CHARS)+1)
        BEGIN
            IF UNICODE(SUBSTRING(@SIGN_CHARS, @COUNTER1,1)) = UNICODE(SUBSTRING(@strInput,@COUNTER ,1) )
            BEGIN
                IF @COUNTER=1
                    SET @strInput = SUBSTRING(@UNSIGN_CHARS, @COUNTER1,1) + SUBSTRING(@strInput, @COUNTER+1,LEN(@strInput)-1)
                ELSE
                    SET @strInput = SUBSTRING(@strInput, 1, @COUNTER-1) +SUBSTRING(@UNSIGN_CHARS, @COUNTER1,1) + SUBSTRING(@strInput, @COUNTER+1,LEN(@strInput)- @COUNTER)
                BREAK
            END
            SET @COUNTER1 = @COUNTER1 +1
        END
        SET @COUNTER = @COUNTER +1
    END
    
    SET @strInput = REPLACE(@strInput,' ','-')
    RETURN @strInput
END
GO
/****** Object:  Table [dbo].[NhanVien]    Script Date: 5/13/2025 10:03:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NhanVien](
	[MaNhanVien] [int] IDENTITY(1,1) NOT NULL,
	[HoTen] [nvarchar](100) NOT NULL,
	[GioiTinh] [nvarchar](10) NULL,
	[NgaySinh] [date] NULL,
	[DiaChi] [nvarchar](200) NULL,
	[SDT] [varchar](20) NULL,
	[Email] [varchar](100) NULL,
	[NgayVaoLam] [date] NOT NULL,
	[TrangThai] [bit] NOT NULL,
	[MaTaiKhoan] [int] NULL,
 CONSTRAINT [PK_NhanVien] PRIMARY KEY CLUSTERED 
(
	[MaNhanVien] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Ban]    Script Date: 5/13/2025 10:03:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Ban](
	[MaBan] [int] IDENTITY(1,1) NOT NULL,
	[TenBan] [nvarchar](50) NOT NULL,
	[SucChua] [int] NOT NULL,
	[TrangThai] [bit] NOT NULL,
	[GhiChu] [nvarchar](200) NULL,
 CONSTRAINT [PK_Ban] PRIMARY KEY CLUSTERED 
(
	[MaBan] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TrangThaiDonHang]    Script Date: 5/13/2025 10:03:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TrangThaiDonHang](
	[MaTrangThai] [int] IDENTITY(1,1) NOT NULL,
	[TenTrangThai] [nvarchar](50) NOT NULL,
	[MoTa] [nvarchar](200) NULL,
 CONSTRAINT [PK_TrangThaiDonHang] PRIMARY KEY CLUSTERED 
(
	[MaTrangThai] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DonHang]    Script Date: 5/13/2025 10:03:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DonHang](
	[MaDonHang] [int] IDENTITY(1,1) NOT NULL,
	[MaBan] [int] NOT NULL,
	[MaNhanVien] [int] NULL,
	[NgayTao] [datetime] NULL,
	[MaTrangThai] [int] NOT NULL,
	[GiamGia] [float] NULL,
	[TongTien] [float] NULL,
	[GhiChu] [nvarchar](200) NULL,
 CONSTRAINT [PK_DonHang] PRIMARY KEY CLUSTERED 
(
	[MaDonHang] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[View_DonHangHienTai]    Script Date: 5/13/2025 10:03:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[View_DonHangHienTai]
AS
SELECT 
    dh.MaDonHang,
    dh.NgayTao,
    b.TenBan,
    nv.HoTen AS NhanVien,
    tt.TenTrangThai,
    dh.TongTien,
    dh.GiamGia,
    dh.GhiChu
FROM DonHang dh
INNER JOIN Ban b ON dh.MaBan = b.MaBan
INNER JOIN NhanVien nv ON dh.MaNhanVien = nv.MaNhanVien
INNER JOIN TrangThaiDonHang tt ON dh.MaTrangThai = tt.MaTrangThai
WHERE dh.MaTrangThai IN (1, 2, 3) -- Mới, Đang chế biến, Đã phục vụ
GO
/****** Object:  Table [dbo].[ChiTietDonHang]    Script Date: 5/13/2025 10:03:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ChiTietDonHang](
	[MaChiTiet] [int] IDENTITY(1,1) NOT NULL,
	[MaDonHang] [int] NOT NULL,
	[MaMonAn] [int] NOT NULL,
	[SoLuong] [int] NULL,
	[DonGia] [float] NULL,
	[ThanhTien] [float] NULL,
	[GhiChu] [nvarchar](200) NULL,
 CONSTRAINT [PK_ChiTietDonHang] PRIMARY KEY CLUSTERED 
(
	[MaChiTiet] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MonAn]    Script Date: 5/13/2025 10:03:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MonAn](
	[MaMonAn] [int] IDENTITY(1,1) NOT NULL,
	[TenMonAn] [nvarchar](100) NOT NULL,
	[MaLoai] [int] NOT NULL,
	[DonGia] [float] NOT NULL,
	[MoTa] [nvarchar](500) NULL,
	[HinhAnh] [varchar](200) NULL,
	[TrangThai] [bit] NOT NULL,
 CONSTRAINT [PK_MonAn] PRIMARY KEY CLUSTERED 
(
	[MaMonAn] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[View_ChiTietDonHang]    Script Date: 5/13/2025 10:03:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[View_ChiTietDonHang]
AS
SELECT 
    ct.MaDonHang,
    m.MaMonAn,
    m.TenMonAn,
    ct.SoLuong,
    ct.DonGia,
    ct.ThanhTien,
    ct.GhiChu
FROM ChiTietDonHang ct
INNER JOIN MonAn m ON ct.MaMonAn = m.MaMonAn
GO
/****** Object:  Table [dbo].[LoaiMonAn]    Script Date: 5/13/2025 10:03:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LoaiMonAn](
	[MaLoai] [int] IDENTITY(1,1) NOT NULL,
	[TenLoai] [nvarchar](100) NOT NULL,
	[MoTa] [nvarchar](200) NULL,
 CONSTRAINT [PK_LoaiMonAn] PRIMARY KEY CLUSTERED 
(
	[MaLoai] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ThongTinDinhDuong]    Script Date: 5/13/2025 10:03:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ThongTinDinhDuong](
	[MaMonAn] [int] NOT NULL,
	[Calo] [float] NULL,
	[Protein] [float] NULL,
	[Carbohydrate] [float] NULL,
	[Fat] [float] NULL,
	[Fiber] [float] NULL,
	[Duong] [float] NULL,
	[Natri] [float] NULL,
	[ThanhPhanDiUng] [nvarchar](200) NULL,
	[PhanLoai] [nvarchar](100) NULL,
	[GhiChu] [nvarchar](200) NULL,
 CONSTRAINT [PK_ThongTinDinhDuong] PRIMARY KEY CLUSTERED 
(
	[MaMonAn] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[View_ThongTinMonAn]    Script Date: 5/13/2025 10:03:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[View_ThongTinMonAn]
AS
SELECT 
    m.MaMonAn,
    m.TenMonAn,
    l.TenLoai,
    m.DonGia,
    m.MoTa,
    m.HinhAnh,
    td.Calo,
    td.Protein,
    td.Carbohydrate,
    td.Fat,
    td.Fiber,
    td.Duong,
    td.Natri,
    td.ThanhPhanDiUng,
    td.PhanLoai
FROM MonAn m
INNER JOIN LoaiMonAn l ON m.MaLoai = l.MaLoai
LEFT JOIN ThongTinDinhDuong td ON m.MaMonAn = td.MaMonAn
WHERE m.TrangThai = 1
GO
/****** Object:  View [dbo].[View_DoanhThuTheoNgay]    Script Date: 5/13/2025 10:03:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[View_DoanhThuTheoNgay]
AS
SELECT 
    CONVERT(DATE, dh.NgayTao) AS Ngay,
    COUNT(dh.MaDonHang) AS SoDonHang,
    SUM(dh.TongTien) AS TongDoanhThu
FROM DonHang dh
WHERE dh.MaTrangThai = 4 -- Đã thanh toán
GROUP BY CONVERT(DATE, dh.NgayTao)
GO
/****** Object:  View [dbo].[View_MonAnBanChay]    Script Date: 5/13/2025 10:03:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[View_MonAnBanChay]
AS
SELECT TOP 10
    m.MaMonAn,
    m.TenMonAn,
    SUM(ct.SoLuong) AS TongSoLuong,
    SUM(ct.ThanhTien) AS TongDoanhThu
FROM ChiTietDonHang ct
INNER JOIN MonAn m ON ct.MaMonAn = m.MaMonAn
INNER JOIN DonHang dh ON ct.MaDonHang = dh.MaDonHang
WHERE dh.MaTrangThai = 4 -- Đã thanh toán
GROUP BY m.MaMonAn, m.TenMonAn
ORDER BY TongSoLuong DESC
GO
/****** Object:  Table [dbo].[NguyenVatLieu]    Script Date: 5/13/2025 10:03:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NguyenVatLieu](
	[MaNguyenLieu] [int] IDENTITY(1,1) NOT NULL,
	[TenNguyenLieu] [nvarchar](100) NOT NULL,
	[DonViTinh] [nvarchar](20) NOT NULL,
	[SoLuongTon] [float] NOT NULL,
	[MoTa] [nvarchar](200) NULL,
	[TrangThai] [bit] NOT NULL,
 CONSTRAINT [PK_NguyenVatLieu] PRIMARY KEY CLUSTERED 
(
	[MaNguyenLieu] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[View_TinhTrangKho]    Script Date: 5/13/2025 10:03:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[View_TinhTrangKho]
AS
SELECT 
    nv.MaNguyenLieu,
    nv.TenNguyenLieu,
    nv.DonViTinh,
    nv.SoLuongTon,
    CASE
        WHEN nv.SoLuongTon < 10 THEN N'Sắp hết'
        WHEN nv.SoLuongTon BETWEEN 10 AND 30 THEN N'Trung bình'
        ELSE N'Đầy đủ'
    END AS TinhTrang
FROM NguyenVatLieu nv
WHERE nv.TrangThai = 1
GO
/****** Object:  Table [dbo].[BaoCaoDoanhThu]    Script Date: 5/13/2025 10:03:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BaoCaoDoanhThu](
	[MaBaoCao] [int] IDENTITY(1,1) NOT NULL,
	[LoaiBaoCao] [nvarchar](50) NOT NULL,
	[NgayBatDau] [date] NOT NULL,
	[NgayKetThuc] [date] NOT NULL,
	[TongDoanhThu] [float] NULL,
	[GhiChu] [nvarchar](500) NULL,
	[NgayTao] [datetime] NOT NULL,
	[NguoiTao] [int] NOT NULL,
 CONSTRAINT [PK_BaoCaoDoanhThu] PRIMARY KEY CLUSTERED 
(
	[MaBaoCao] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[BaoCaoMonAn]    Script Date: 5/13/2025 10:03:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BaoCaoMonAn](
	[MaBaoCao] [int] IDENTITY(1,1) NOT NULL,
	[LoaiBaoCao] [nvarchar](50) NOT NULL,
	[NgayBatDau] [date] NOT NULL,
	[NgayKetThuc] [date] NOT NULL,
	[GhiChu] [nvarchar](500) NULL,
	[NgayTao] [datetime] NOT NULL,
	[NguoiTao] [int] NOT NULL,
 CONSTRAINT [PK_BaoCaoMonAn] PRIMARY KEY CLUSTERED 
(
	[MaBaoCao] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ChiTietBaoCaoMonAn]    Script Date: 5/13/2025 10:03:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ChiTietBaoCaoMonAn](
	[MaChiTiet] [int] IDENTITY(1,1) NOT NULL,
	[MaBaoCao] [int] NOT NULL,
	[MaMonAn] [int] NOT NULL,
	[SoLuongDaBan] [int] NOT NULL,
	[DoanhThu] [float] NULL,
	[TiLe] [float] NULL,
 CONSTRAINT [PK_ChiTietBaoCaoMonAn] PRIMARY KEY CLUSTERED 
(
	[MaChiTiet] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ChiTietKhuyenMai]    Script Date: 5/13/2025 10:03:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ChiTietKhuyenMai](
	[MaChiTiet] [int] IDENTITY(1,1) NOT NULL,
	[MaKhuyenMai] [int] NOT NULL,
	[MaMonAn] [int] NOT NULL,
	[PhanTramGiam] [float] NOT NULL,
	[GhiChu] [nvarchar](200) NULL,
 CONSTRAINT [PK_ChiTietKhuyenMai] PRIMARY KEY CLUSTERED 
(
	[MaChiTiet] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ChiTietPhieuNhap]    Script Date: 5/13/2025 10:03:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ChiTietPhieuNhap](
	[MaChiTiet] [int] IDENTITY(1,1) NOT NULL,
	[MaPhieuNhap] [int] NOT NULL,
	[MaNguyenLieu] [int] NOT NULL,
	[SoLuong] [float] NOT NULL,
	[DonGia] [float] NOT NULL,
	[ThanhTien] [float] NULL,
	[GhiChu] [nvarchar](200) NULL,
 CONSTRAINT [PK_ChiTietPhieuNhap] PRIMARY KEY CLUSTERED 
(
	[MaChiTiet] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CongThucMon]    Script Date: 5/13/2025 10:03:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CongThucMon](
	[MaCongThuc] [int] IDENTITY(1,1) NOT NULL,
	[MaMonAn] [int] NOT NULL,
	[MaNguyenLieu] [int] NOT NULL,
	[SoLuong] [float] NOT NULL,
	[DonViTinh] [nvarchar](20) NOT NULL,
	[GhiChu] [nvarchar](200) NULL,
 CONSTRAINT [PK_CongThucMon] PRIMARY KEY CLUSTERED 
(
	[MaCongThuc] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DatBan]    Script Date: 5/13/2025 10:03:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DatBan](
	[MaDatBan] [int] IDENTITY(1,1) NOT NULL,
	[MaBan] [int] NOT NULL,
	[TenKhachHang] [nvarchar](100) NOT NULL,
	[SDT] [varchar](20) NULL,
	[NgayDat] [date] NOT NULL,
	[GioDat] [time](7) NOT NULL,
	[SoNguoi] [int] NOT NULL,
	[TrangThai] [nvarchar](50) NOT NULL,
	[GhiChu] [nvarchar](200) NULL,
 CONSTRAINT [PK_DatBan] PRIMARY KEY CLUSTERED 
(
	[MaDatBan] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[HinhThucThanhToan]    Script Date: 5/13/2025 10:03:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[HinhThucThanhToan](
	[MaHinhThuc] [int] IDENTITY(1,1) NOT NULL,
	[TenHinhThuc] [nvarchar](50) NOT NULL,
	[MoTa] [nvarchar](200) NULL,
	[TrangThai] [bit] NOT NULL,
 CONSTRAINT [PK_HinhThucThanhToan] PRIMARY KEY CLUSTERED 
(
	[MaHinhThuc] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[HoaDonThanhToan]    Script Date: 5/13/2025 10:03:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[HoaDonThanhToan](
	[MaHoaDon] [int] IDENTITY(1,1) NOT NULL,
	[MaDonHang] [int] NOT NULL,
	[NgayThanhToan] [datetime] NULL,
	[MaNhanVien] [int] NOT NULL,
	[TongTien] [decimal](18, 0) NOT NULL,
	[MaHinhThuc] [int] NULL,
	[TrangThai] [nvarchar](20) NOT NULL,
	[GhiChu] [nvarchar](200) NULL,
	[NgayDatHang] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[MaHoaDon] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[KhuyenMai]    Script Date: 5/13/2025 10:03:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[KhuyenMai](
	[MaKhuyenMai] [int] IDENTITY(1,1) NOT NULL,
	[TenKhuyenMai] [nvarchar](100) NOT NULL,
	[NoiDung] [nvarchar](500) NULL,
	[PhanTramGiam] [float] NOT NULL,
	[NgayBatDau] [datetime] NOT NULL,
	[NgayKetThuc] [datetime] NOT NULL,
	[TrangThai] [bit] NOT NULL,
	[GhiChu] [nvarchar](200) NULL,
 CONSTRAINT [PK_KhuyenMai] PRIMARY KEY CLUSTERED 
(
	[MaKhuyenMai] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[KiemTraChatLuong]    Script Date: 5/13/2025 10:03:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[KiemTraChatLuong](
	[MaKiemTra] [varchar](10) NOT NULL,
	[LoaiKiemTra] [varchar](20) NOT NULL,
	[DoiTuongKiemTra] [varchar](10) NOT NULL,
	[NgayKiemTra] [datetime] NOT NULL,
	[NguoiKiemTra] [int] NOT NULL,
	[TieuChiKiemTra] [nvarchar](200) NULL,
	[GiaTri] [float] NULL,
	[DonVi] [nvarchar](10) NULL,
	[KetQua] [nvarchar](20) NULL,
	[GhiChu] [nvarchar](200) NULL,
	[HinhAnh] [varchar](200) NULL,
 CONSTRAINT [PK_KiemTraChatLuong] PRIMARY KEY CLUSTERED 
(
	[MaKiemTra] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[LoaiTaiKhoan]    Script Date: 5/13/2025 10:03:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LoaiTaiKhoan](
	[MaLoai] [int] IDENTITY(1,1) NOT NULL,
	[TenLoai] [nvarchar](50) NOT NULL,
	[MoTa] [nvarchar](200) NULL,
 CONSTRAINT [PK_LoaiTaiKhoan] PRIMARY KEY CLUSTERED 
(
	[MaLoai] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PhieuNhapKho]    Script Date: 5/13/2025 10:03:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PhieuNhapKho](
	[MaPhieuNhap] [int] IDENTITY(1,1) NOT NULL,
	[MaNhanVien] [int] NOT NULL,
	[NgayNhap] [datetime] NOT NULL,
	[NhaCungCap] [nvarchar](100) NULL,
	[TongTien] [float] NULL,
	[GhiChu] [nvarchar](200) NULL,
	[TrangThai] [bit] NOT NULL,
 CONSTRAINT [PK_PhieuNhapKho] PRIMARY KEY CLUSTERED 
(
	[MaPhieuNhap] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TaiKhoan]    Script Date: 5/13/2025 10:03:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TaiKhoan](
	[MaTaiKhoan] [int] IDENTITY(1,1) NOT NULL,
	[TenDangNhap] [nvarchar](50) NOT NULL,
	[TenHienThi] [nvarchar](100) NOT NULL,
	[MatKhau] [nvarchar](100) NOT NULL,
	[MaLoai] [int] NOT NULL,
	[TrangThai] [bit] NOT NULL,
 CONSTRAINT [PK_TaiKhoan] PRIMARY KEY CLUSTERED 
(
	[MaTaiKhoan] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ThanhToan]    Script Date: 5/13/2025 10:03:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ThanhToan](
	[MaThanhToan] [int] IDENTITY(1,1) NOT NULL,
	[MaDonHang] [int] NOT NULL,
	[MaNhanVien] [int] NOT NULL,
	[MaHinhThuc] [int] NOT NULL,
	[SoTien] [float] NOT NULL,
	[NgayThanhToan] [datetime] NULL,
	[GhiChu] [nvarchar](200) NULL,
 CONSTRAINT [PK_ThanhToan] PRIMARY KEY CLUSTERED 
(
	[MaThanhToan] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Ban] ON 

INSERT [dbo].[Ban] ([MaBan], [TenBan], [SucChua], [TrangThai], [GhiChu]) VALUES (1, N'Bàn A1', 4, 1, N'Ghi chú khác
 Phục vụ chính: 
 Loại Khách: ')
INSERT [dbo].[Ban] ([MaBan], [TenBan], [SucChua], [TrangThai], [GhiChu]) VALUES (2, N'Bàn A2', 4, 1, N'Ghi chú khác
 Phục vụ chính: 
 Loại Khách: ')
INSERT [dbo].[Ban] ([MaBan], [TenBan], [SucChua], [TrangThai], [GhiChu]) VALUES (3, N'Bàn A3', 4, 1, N'Ghi chú khác
 Phục vụ chính: 
 Loại Khách: ')
INSERT [dbo].[Ban] ([MaBan], [TenBan], [SucChua], [TrangThai], [GhiChu]) VALUES (4, N'Bàn A4', 4, 1, N'Ghi chú khác
 Phục vụ chính: 
 Loại Khách: ')
INSERT [dbo].[Ban] ([MaBan], [TenBan], [SucChua], [TrangThai], [GhiChu]) VALUES (5, N'Bàn A5', 4, 1, N'Ghi chú khác
 Phục vụ chính: 
 Loại Khách: ')
INSERT [dbo].[Ban] ([MaBan], [TenBan], [SucChua], [TrangThai], [GhiChu]) VALUES (6, N'Bàn A6', 4, 1, N'Ghi chú khác
 Phục vụ chính: 
 Loại Khách: ')
INSERT [dbo].[Ban] ([MaBan], [TenBan], [SucChua], [TrangThai], [GhiChu]) VALUES (7, N'Bàn A7', 4, 1, N'Ghi chú khác
 Phục vụ chính: 
 Loại Khách: ')
INSERT [dbo].[Ban] ([MaBan], [TenBan], [SucChua], [TrangThai], [GhiChu]) VALUES (8, N'Bàn A8', 6, 1, N'Ghi chú khác
 Phục vụ chính: 
 Loại Khách: ')
INSERT [dbo].[Ban] ([MaBan], [TenBan], [SucChua], [TrangThai], [GhiChu]) VALUES (9, N'Bàn B1', 6, 1, N'Ghi chú khác
 Phục vụ chính: 
 Loại Khách: ')
INSERT [dbo].[Ban] ([MaBan], [TenBan], [SucChua], [TrangThai], [GhiChu]) VALUES (10, N'Bàn B2', 10, 1, N'Ghi chú khác
 Phục vụ chính: 
 Loại Khách: ')
INSERT [dbo].[Ban] ([MaBan], [TenBan], [SucChua], [TrangThai], [GhiChu]) VALUES (11, N'Bàn B3', 4, 1, N'Ghi chú khác
 Phục vụ chính: 
 Loại Khách: ')
INSERT [dbo].[Ban] ([MaBan], [TenBan], [SucChua], [TrangThai], [GhiChu]) VALUES (12, N'Bàn B4', 4, 1, N'Ghi chú khác
 Phục vụ chính: 
 Loại Khách: ')
INSERT [dbo].[Ban] ([MaBan], [TenBan], [SucChua], [TrangThai], [GhiChu]) VALUES (13, N'Bàn B5', 4, 1, N'Ghi chú khác
 Phục vụ chính: 
 Loại Khách: ')
INSERT [dbo].[Ban] ([MaBan], [TenBan], [SucChua], [TrangThai], [GhiChu]) VALUES (14, N'Bàn B6', 4, 1, N'Ghi chú khác
 Phục vụ chính: 
 Loại Khách: ')
INSERT [dbo].[Ban] ([MaBan], [TenBan], [SucChua], [TrangThai], [GhiChu]) VALUES (15, N'Bàn B7', 4, 1, N'Ghi chú khác
 Phục vụ chính: 
 Loại Khách: ')
INSERT [dbo].[Ban] ([MaBan], [TenBan], [SucChua], [TrangThai], [GhiChu]) VALUES (16, N'Bàn B8', 4, 1, N'Ghi chú khác
 Phục vụ chính: 
 Loại Khách: ')
INSERT [dbo].[Ban] ([MaBan], [TenBan], [SucChua], [TrangThai], [GhiChu]) VALUES (20, N'MV 1', 4, 1, NULL)
INSERT [dbo].[Ban] ([MaBan], [TenBan], [SucChua], [TrangThai], [GhiChu]) VALUES (22, N'MV 2', 4, 1, NULL)
INSERT [dbo].[Ban] ([MaBan], [TenBan], [SucChua], [TrangThai], [GhiChu]) VALUES (23, N'MV 3', 4, 1, N'Ghi chú khác...')
INSERT [dbo].[Ban] ([MaBan], [TenBan], [SucChua], [TrangThai], [GhiChu]) VALUES (24, N'MV 4', 4, 1, N'Ghi chú khác...')
INSERT [dbo].[Ban] ([MaBan], [TenBan], [SucChua], [TrangThai], [GhiChu]) VALUES (25, N'MV 5', 4, 1, N'Ghi chú khác...')
INSERT [dbo].[Ban] ([MaBan], [TenBan], [SucChua], [TrangThai], [GhiChu]) VALUES (26, N'MV 6', 4, 1, N'Ghi chú khác...')
INSERT [dbo].[Ban] ([MaBan], [TenBan], [SucChua], [TrangThai], [GhiChu]) VALUES (27, N'MV 7', 4, 1, N'Ghi chú khác...')
INSERT [dbo].[Ban] ([MaBan], [TenBan], [SucChua], [TrangThai], [GhiChu]) VALUES (28, N'MV 8', 4, 1, N'Ghi chú khác...')
INSERT [dbo].[Ban] ([MaBan], [TenBan], [SucChua], [TrangThai], [GhiChu]) VALUES (29, N'App 1', 4, 1, N'Ghi chú khác...')
INSERT [dbo].[Ban] ([MaBan], [TenBan], [SucChua], [TrangThai], [GhiChu]) VALUES (30, N'App 2', 4, 1, N'Ghi chú khác...')
INSERT [dbo].[Ban] ([MaBan], [TenBan], [SucChua], [TrangThai], [GhiChu]) VALUES (31, N'App 3', 4, 1, N'Ghi chú khác...')
INSERT [dbo].[Ban] ([MaBan], [TenBan], [SucChua], [TrangThai], [GhiChu]) VALUES (32, N'App 4', 4, 1, N'Ghi chú khác...')
INSERT [dbo].[Ban] ([MaBan], [TenBan], [SucChua], [TrangThai], [GhiChu]) VALUES (33, N'App 5', 4, 1, N'Ghi chú khác...')
INSERT [dbo].[Ban] ([MaBan], [TenBan], [SucChua], [TrangThai], [GhiChu]) VALUES (34, N'App 6', 4, 1, N'Ghi chú khác
 Phục vụ chính: 
 Loại Khách: ')
INSERT [dbo].[Ban] ([MaBan], [TenBan], [SucChua], [TrangThai], [GhiChu]) VALUES (35, N'App 7', 4, 1, N'Ghi chú khác...')
INSERT [dbo].[Ban] ([MaBan], [TenBan], [SucChua], [TrangThai], [GhiChu]) VALUES (36, N'App 8', 4, 1, N'Ghi chú khác
 Phục vụ chính: 
 Loại Khách: ')
SET IDENTITY_INSERT [dbo].[Ban] OFF
GO
SET IDENTITY_INSERT [dbo].[ChiTietDonHang] ON 

INSERT [dbo].[ChiTietDonHang] ([MaChiTiet], [MaDonHang], [MaMonAn], [SoLuong], [DonGia], [ThanhTien], [GhiChu]) VALUES (1, 1, 14, 1, 49000, 49000, N'Cấp độ 
')
INSERT [dbo].[ChiTietDonHang] ([MaChiTiet], [MaDonHang], [MaMonAn], [SoLuong], [DonGia], [ThanhTien], [GhiChu]) VALUES (2, 1, 14, 1, 49000, 49000, N'Cấp độ 
3')
INSERT [dbo].[ChiTietDonHang] ([MaChiTiet], [MaDonHang], [MaMonAn], [SoLuong], [DonGia], [ThanhTien], [GhiChu]) VALUES (3, 1, 17, 1, 59000, 59000, N'Cấp độ 
3')
INSERT [dbo].[ChiTietDonHang] ([MaChiTiet], [MaDonHang], [MaMonAn], [SoLuong], [DonGia], [ThanhTien], [GhiChu]) VALUES (4, 1, 25, 1, 59000, 59000, N'Cấp độ 
1')
INSERT [dbo].[ChiTietDonHang] ([MaChiTiet], [MaDonHang], [MaMonAn], [SoLuong], [DonGia], [ThanhTien], [GhiChu]) VALUES (5, 1, 27, 1, 65000, 65000, N'1')
INSERT [dbo].[ChiTietDonHang] ([MaChiTiet], [MaDonHang], [MaMonAn], [SoLuong], [DonGia], [ThanhTien], [GhiChu]) VALUES (6, 2, 1, 1, 45000, 45000, N'')
INSERT [dbo].[ChiTietDonHang] ([MaChiTiet], [MaDonHang], [MaMonAn], [SoLuong], [DonGia], [ThanhTien], [GhiChu]) VALUES (7, 2, 3, 1, 39000, 39000, N'')
INSERT [dbo].[ChiTietDonHang] ([MaChiTiet], [MaDonHang], [MaMonAn], [SoLuong], [DonGia], [ThanhTien], [GhiChu]) VALUES (8, 2, 17, 2, 59000, 118000, N'Cấp độ 
')
INSERT [dbo].[ChiTietDonHang] ([MaChiTiet], [MaDonHang], [MaMonAn], [SoLuong], [DonGia], [ThanhTien], [GhiChu]) VALUES (9, 2, 19, 3, 49000, 147000, N'Cấp độ 
')
INSERT [dbo].[ChiTietDonHang] ([MaChiTiet], [MaDonHang], [MaMonAn], [SoLuong], [DonGia], [ThanhTien], [GhiChu]) VALUES (10, 3, 4, 1, 29000, 29000, N'')
INSERT [dbo].[ChiTietDonHang] ([MaChiTiet], [MaDonHang], [MaMonAn], [SoLuong], [DonGia], [ThanhTien], [GhiChu]) VALUES (11, 3, 6, 1, 29000, 29000, N'')
INSERT [dbo].[ChiTietDonHang] ([MaChiTiet], [MaDonHang], [MaMonAn], [SoLuong], [DonGia], [ThanhTien], [GhiChu]) VALUES (12, 3, 5, 1, 39000, 39000, N'')
INSERT [dbo].[ChiTietDonHang] ([MaChiTiet], [MaDonHang], [MaMonAn], [SoLuong], [DonGia], [ThanhTien], [GhiChu]) VALUES (13, 4, 4, 1, 29000, 29000, N's')
INSERT [dbo].[ChiTietDonHang] ([MaChiTiet], [MaDonHang], [MaMonAn], [SoLuong], [DonGia], [ThanhTien], [GhiChu]) VALUES (14, 4, 4, 1, 29000, 29000, N's')
INSERT [dbo].[ChiTietDonHang] ([MaChiTiet], [MaDonHang], [MaMonAn], [SoLuong], [DonGia], [ThanhTien], [GhiChu]) VALUES (15, 4, 1, 1, 45000, 45000, N's')
INSERT [dbo].[ChiTietDonHang] ([MaChiTiet], [MaDonHang], [MaMonAn], [SoLuong], [DonGia], [ThanhTien], [GhiChu]) VALUES (16, 11, 1, 1, 45000, 45000, N'')
INSERT [dbo].[ChiTietDonHang] ([MaChiTiet], [MaDonHang], [MaMonAn], [SoLuong], [DonGia], [ThanhTien], [GhiChu]) VALUES (17, 11, 3, 1, 39000, 39000, N'')
INSERT [dbo].[ChiTietDonHang] ([MaChiTiet], [MaDonHang], [MaMonAn], [SoLuong], [DonGia], [ThanhTien], [GhiChu]) VALUES (18, 12, 1, 1, 45000, 45000, N'')
INSERT [dbo].[ChiTietDonHang] ([MaChiTiet], [MaDonHang], [MaMonAn], [SoLuong], [DonGia], [ThanhTien], [GhiChu]) VALUES (19, 12, 3, 1, 39000, 39000, N'')
INSERT [dbo].[ChiTietDonHang] ([MaChiTiet], [MaDonHang], [MaMonAn], [SoLuong], [DonGia], [ThanhTien], [GhiChu]) VALUES (20, 12, 6, 1, 29000, 29000, N'')
INSERT [dbo].[ChiTietDonHang] ([MaChiTiet], [MaDonHang], [MaMonAn], [SoLuong], [DonGia], [ThanhTien], [GhiChu]) VALUES (21, 12, 7, 1, 39000, 39000, N'')
INSERT [dbo].[ChiTietDonHang] ([MaChiTiet], [MaDonHang], [MaMonAn], [SoLuong], [DonGia], [ThanhTien], [GhiChu]) VALUES (22, 17, 2, 1, 45000, 45000, N'')
INSERT [dbo].[ChiTietDonHang] ([MaChiTiet], [MaDonHang], [MaMonAn], [SoLuong], [DonGia], [ThanhTien], [GhiChu]) VALUES (23, 17, 4, 1, 29000, 29000, N'')
INSERT [dbo].[ChiTietDonHang] ([MaChiTiet], [MaDonHang], [MaMonAn], [SoLuong], [DonGia], [ThanhTien], [GhiChu]) VALUES (24, 17, 7, 1, 39000, 39000, N'')
INSERT [dbo].[ChiTietDonHang] ([MaChiTiet], [MaDonHang], [MaMonAn], [SoLuong], [DonGia], [ThanhTien], [GhiChu]) VALUES (25, 18, 1, 1, 45000, 45000, N'')
INSERT [dbo].[ChiTietDonHang] ([MaChiTiet], [MaDonHang], [MaMonAn], [SoLuong], [DonGia], [ThanhTien], [GhiChu]) VALUES (26, 18, 2, 1, 45000, 45000, N'')
INSERT [dbo].[ChiTietDonHang] ([MaChiTiet], [MaDonHang], [MaMonAn], [SoLuong], [DonGia], [ThanhTien], [GhiChu]) VALUES (27, 18, 6, 1, 29000, 29000, N'')
INSERT [dbo].[ChiTietDonHang] ([MaChiTiet], [MaDonHang], [MaMonAn], [SoLuong], [DonGia], [ThanhTien], [GhiChu]) VALUES (28, 19, 1, 1, 45000, 45000, N'')
INSERT [dbo].[ChiTietDonHang] ([MaChiTiet], [MaDonHang], [MaMonAn], [SoLuong], [DonGia], [ThanhTien], [GhiChu]) VALUES (29, 19, 3, 1, 39000, 39000, N'')
INSERT [dbo].[ChiTietDonHang] ([MaChiTiet], [MaDonHang], [MaMonAn], [SoLuong], [DonGia], [ThanhTien], [GhiChu]) VALUES (30, 19, 5, 1, 39000, 39000, N'')
INSERT [dbo].[ChiTietDonHang] ([MaChiTiet], [MaDonHang], [MaMonAn], [SoLuong], [DonGia], [ThanhTien], [GhiChu]) VALUES (31, 20, 4, 1, 29000, 29000, N'')
INSERT [dbo].[ChiTietDonHang] ([MaChiTiet], [MaDonHang], [MaMonAn], [SoLuong], [DonGia], [ThanhTien], [GhiChu]) VALUES (32, 20, 6, 1, 29000, 29000, N'')
INSERT [dbo].[ChiTietDonHang] ([MaChiTiet], [MaDonHang], [MaMonAn], [SoLuong], [DonGia], [ThanhTien], [GhiChu]) VALUES (33, 20, 7, 1, 39000, 39000, N'')
INSERT [dbo].[ChiTietDonHang] ([MaChiTiet], [MaDonHang], [MaMonAn], [SoLuong], [DonGia], [ThanhTien], [GhiChu]) VALUES (34, 20, 8, 1, 45000, 45000, N'')
INSERT [dbo].[ChiTietDonHang] ([MaChiTiet], [MaDonHang], [MaMonAn], [SoLuong], [DonGia], [ThanhTien], [GhiChu]) VALUES (35, 21, 2, 1, 45000, 45000, N'')
INSERT [dbo].[ChiTietDonHang] ([MaChiTiet], [MaDonHang], [MaMonAn], [SoLuong], [DonGia], [ThanhTien], [GhiChu]) VALUES (36, 21, 3, 1, 39000, 39000, N'')
INSERT [dbo].[ChiTietDonHang] ([MaChiTiet], [MaDonHang], [MaMonAn], [SoLuong], [DonGia], [ThanhTien], [GhiChu]) VALUES (37, 21, 6, 1, 29000, 29000, N'')
INSERT [dbo].[ChiTietDonHang] ([MaChiTiet], [MaDonHang], [MaMonAn], [SoLuong], [DonGia], [ThanhTien], [GhiChu]) VALUES (38, 22, 1, 1, 45000, 45000, N'')
INSERT [dbo].[ChiTietDonHang] ([MaChiTiet], [MaDonHang], [MaMonAn], [SoLuong], [DonGia], [ThanhTien], [GhiChu]) VALUES (39, 22, 3, 1, 39000, 39000, N'')
INSERT [dbo].[ChiTietDonHang] ([MaChiTiet], [MaDonHang], [MaMonAn], [SoLuong], [DonGia], [ThanhTien], [GhiChu]) VALUES (40, 22, 6, 1, 29000, 29000, N'')
INSERT [dbo].[ChiTietDonHang] ([MaChiTiet], [MaDonHang], [MaMonAn], [SoLuong], [DonGia], [ThanhTien], [GhiChu]) VALUES (41, 24, 1, 1, 45000, 45000, N'')
INSERT [dbo].[ChiTietDonHang] ([MaChiTiet], [MaDonHang], [MaMonAn], [SoLuong], [DonGia], [ThanhTien], [GhiChu]) VALUES (42, 24, 3, 1, 39000, 39000, N'')
INSERT [dbo].[ChiTietDonHang] ([MaChiTiet], [MaDonHang], [MaMonAn], [SoLuong], [DonGia], [ThanhTien], [GhiChu]) VALUES (43, 24, 6, 1, 29000, 29000, N'')
INSERT [dbo].[ChiTietDonHang] ([MaChiTiet], [MaDonHang], [MaMonAn], [SoLuong], [DonGia], [ThanhTien], [GhiChu]) VALUES (44, 24, 2, 1, 40000, 40000, N'')
INSERT [dbo].[ChiTietDonHang] ([MaChiTiet], [MaDonHang], [MaMonAn], [SoLuong], [DonGia], [ThanhTien], [GhiChu]) VALUES (45, 24, 6, 1, 29000, 29000, N'')
INSERT [dbo].[ChiTietDonHang] ([MaChiTiet], [MaDonHang], [MaMonAn], [SoLuong], [DonGia], [ThanhTien], [GhiChu]) VALUES (46, 24, 7, 1, 39000, 39000, N'')
SET IDENTITY_INSERT [dbo].[ChiTietDonHang] OFF
GO
SET IDENTITY_INSERT [dbo].[ChiTietPhieuNhap] ON 

INSERT [dbo].[ChiTietPhieuNhap] ([MaChiTiet], [MaPhieuNhap], [MaNguyenLieu], [SoLuong], [DonGia], [ThanhTien], [GhiChu]) VALUES (2, 2, 92, 20, 30000, 600000, N'Thanks')
INSERT [dbo].[ChiTietPhieuNhap] ([MaChiTiet], [MaPhieuNhap], [MaNguyenLieu], [SoLuong], [DonGia], [ThanhTien], [GhiChu]) VALUES (3, 2, 47, 20, 300, 6000, N'Thanks')
INSERT [dbo].[ChiTietPhieuNhap] ([MaChiTiet], [MaPhieuNhap], [MaNguyenLieu], [SoLuong], [DonGia], [ThanhTien], [GhiChu]) VALUES (4, 5, 13, 2, 20000, 40000, N'ádasdsadsa')
INSERT [dbo].[ChiTietPhieuNhap] ([MaChiTiet], [MaPhieuNhap], [MaNguyenLieu], [SoLuong], [DonGia], [ThanhTien], [GhiChu]) VALUES (5, 6, 28, 16, 2000, 32000, N'rau xanh')
SET IDENTITY_INSERT [dbo].[ChiTietPhieuNhap] OFF
GO
SET IDENTITY_INSERT [dbo].[DonHang] ON 

INSERT [dbo].[DonHang] ([MaDonHang], [MaBan], [MaNhanVien], [NgayTao], [MaTrangThai], [GiamGia], [TongTien], [GhiChu]) VALUES (1, 1, 1, CAST(N'2025-05-11T19:24:16.683' AS DateTime), 4, 42150, 238850, N'Ghi chú khác
 Phục vụ chính: 
 Loại Khách: ')
INSERT [dbo].[DonHang] ([MaDonHang], [MaBan], [MaNhanVien], [NgayTao], [MaTrangThai], [GiamGia], [TongTien], [GhiChu]) VALUES (2, 10, 1, CAST(N'2025-05-11T19:25:55.047' AS DateTime), 4, 0, 349000, N'Ghi chú khác
 Phục vụ chính: 
 Loại Khách: ')
INSERT [dbo].[DonHang] ([MaDonHang], [MaBan], [MaNhanVien], [NgayTao], [MaTrangThai], [GiamGia], [TongTien], [GhiChu]) VALUES (3, 6, 1, CAST(N'2025-05-11T19:28:29.810' AS DateTime), 4, 0, 97000, N'Ghi chú khác
 Phục vụ chính: 
 Loại Khách: ')
INSERT [dbo].[DonHang] ([MaDonHang], [MaBan], [MaNhanVien], [NgayTao], [MaTrangThai], [GiamGia], [TongTien], [GhiChu]) VALUES (4, 1, 1, CAST(N'2025-05-11T19:47:18.067' AS DateTime), 4, 0, 103000, N'Ghi chú khác
 Phục vụ chính: 
 Loại Khách: ')
INSERT [dbo].[DonHang] ([MaDonHang], [MaBan], [MaNhanVien], [NgayTao], [MaTrangThai], [GiamGia], [TongTien], [GhiChu]) VALUES (5, 1, 1, CAST(N'2025-05-11T20:15:14.957' AS DateTime), 1, 0, 0, N'Ghi chú khác
 Phục vụ chính: 
 Loại Khách: ')
INSERT [dbo].[DonHang] ([MaDonHang], [MaBan], [MaNhanVien], [NgayTao], [MaTrangThai], [GiamGia], [TongTien], [GhiChu]) VALUES (6, 2, 1, CAST(N'2025-05-11T20:16:49.347' AS DateTime), 1, 0, 0, N'Ghi chú khác
 Phục vụ chính: 
 Loại Khách: ')
INSERT [dbo].[DonHang] ([MaDonHang], [MaBan], [MaNhanVien], [NgayTao], [MaTrangThai], [GiamGia], [TongTien], [GhiChu]) VALUES (7, 1, 1, CAST(N'2025-05-11T20:24:40.193' AS DateTime), 1, 0, 0, N'Ghi chú khác
 Phục vụ chính: 
 Loại Khách: ')
INSERT [dbo].[DonHang] ([MaDonHang], [MaBan], [MaNhanVien], [NgayTao], [MaTrangThai], [GiamGia], [TongTien], [GhiChu]) VALUES (8, 5, 1, CAST(N'2025-05-11T20:25:59.860' AS DateTime), 1, 0, 0, N'Ghi chú khác
 Phục vụ chính: 
 Loại Khách: ')
INSERT [dbo].[DonHang] ([MaDonHang], [MaBan], [MaNhanVien], [NgayTao], [MaTrangThai], [GiamGia], [TongTien], [GhiChu]) VALUES (9, 1, 1, CAST(N'2025-05-11T20:29:53.970' AS DateTime), 1, 0, 0, N'Ghi chú khác
 Phục vụ chính: 
 Loại Khách: ')
INSERT [dbo].[DonHang] ([MaDonHang], [MaBan], [MaNhanVien], [NgayTao], [MaTrangThai], [GiamGia], [TongTien], [GhiChu]) VALUES (10, 4, 1, CAST(N'2025-05-11T20:30:44.467' AS DateTime), 1, 0, 0, N'Ghi chú khác
 Phục vụ chính: 
 Loại Khách: ')
INSERT [dbo].[DonHang] ([MaDonHang], [MaBan], [MaNhanVien], [NgayTao], [MaTrangThai], [GiamGia], [TongTien], [GhiChu]) VALUES (11, 3, 1, CAST(N'2025-05-11T20:33:02.467' AS DateTime), 4, 0, 84000, N'Ghi chú khác
 Phục vụ chính: 
 Loại Khách: ')
INSERT [dbo].[DonHang] ([MaDonHang], [MaBan], [MaNhanVien], [NgayTao], [MaTrangThai], [GiamGia], [TongTien], [GhiChu]) VALUES (12, 1, 1, CAST(N'2025-05-11T21:39:29.867' AS DateTime), 4, 0, 152000, N'Ghi chú khác
 Phục vụ chính: 
 Loại Khách: ')
INSERT [dbo].[DonHang] ([MaDonHang], [MaBan], [MaNhanVien], [NgayTao], [MaTrangThai], [GiamGia], [TongTien], [GhiChu]) VALUES (13, 3, 1, CAST(N'2025-05-11T22:00:43.067' AS DateTime), 1, 0, 0, N'Ghi chú khác
 Phục vụ chính: 
 Loại Khách: ')
INSERT [dbo].[DonHang] ([MaDonHang], [MaBan], [MaNhanVien], [NgayTao], [MaTrangThai], [GiamGia], [TongTien], [GhiChu]) VALUES (14, 4, 1, CAST(N'2025-05-11T22:01:49.103' AS DateTime), 1, 0, 0, N'Ghi chú khác
 Phục vụ chính: 
 Loại Khách: ')
INSERT [dbo].[DonHang] ([MaDonHang], [MaBan], [MaNhanVien], [NgayTao], [MaTrangThai], [GiamGia], [TongTien], [GhiChu]) VALUES (15, 2, 1, CAST(N'2025-05-11T23:48:09.687' AS DateTime), 1, 0, 0, N'Ghi chú khác
 Phục vụ chính: 
 Loại Khách: ')
INSERT [dbo].[DonHang] ([MaDonHang], [MaBan], [MaNhanVien], [NgayTao], [MaTrangThai], [GiamGia], [TongTien], [GhiChu]) VALUES (16, 1, 1, CAST(N'2025-05-11T23:52:52.320' AS DateTime), 1, 0, 0, N'Ghi chú khác
 Phục vụ chính: 
 Loại Khách: ')
INSERT [dbo].[DonHang] ([MaDonHang], [MaBan], [MaNhanVien], [NgayTao], [MaTrangThai], [GiamGia], [TongTien], [GhiChu]) VALUES (17, 3, 1, CAST(N'2025-05-11T23:55:01.780' AS DateTime), 4, 0, 113000, N'Ghi chú khác
 Phục vụ chính: 
 Loại Khách: ')
INSERT [dbo].[DonHang] ([MaDonHang], [MaBan], [MaNhanVien], [NgayTao], [MaTrangThai], [GiamGia], [TongTien], [GhiChu]) VALUES (18, 4, 1, CAST(N'2025-05-12T00:58:52.203' AS DateTime), 4, 0, 119000, N'Ghi chú khác
 Phục vụ chính: 
 Loại Khách: ')
INSERT [dbo].[DonHang] ([MaDonHang], [MaBan], [MaNhanVien], [NgayTao], [MaTrangThai], [GiamGia], [TongTien], [GhiChu]) VALUES (19, 4, 1, CAST(N'2025-05-12T01:09:20.097' AS DateTime), 4, 0, 123000, N'Ghi chú khác
 Phục vụ chính: 
 Loại Khách: ')
INSERT [dbo].[DonHang] ([MaDonHang], [MaBan], [MaNhanVien], [NgayTao], [MaTrangThai], [GiamGia], [TongTien], [GhiChu]) VALUES (20, 2, 1, CAST(N'2025-05-12T01:20:09.283' AS DateTime), 4, 0, 142000, N'Ghi chú khác
 Phục vụ chính: 
 Loại Khách: ')
INSERT [dbo].[DonHang] ([MaDonHang], [MaBan], [MaNhanVien], [NgayTao], [MaTrangThai], [GiamGia], [TongTien], [GhiChu]) VALUES (21, 3, 1, CAST(N'2025-05-12T12:15:05.363' AS DateTime), 4, 0, 113000, N'Ghi chú khác
 Phục vụ chính: 
 Loại Khách: ')
INSERT [dbo].[DonHang] ([MaDonHang], [MaBan], [MaNhanVien], [NgayTao], [MaTrangThai], [GiamGia], [TongTien], [GhiChu]) VALUES (22, 2, 1, CAST(N'2025-05-12T21:09:44.307' AS DateTime), 4, 16950, 96050, N'Ghi chú khác
 Phục vụ chính: 
 Loại Khách: ')
INSERT [dbo].[DonHang] ([MaDonHang], [MaBan], [MaNhanVien], [NgayTao], [MaTrangThai], [GiamGia], [TongTien], [GhiChu]) VALUES (23, 3, 1, CAST(N'2025-05-13T21:18:33.183' AS DateTime), 5, 0, 0, N'Ghi chú khác
 Phục vụ chính: 
 Loại Khách: ')
INSERT [dbo].[DonHang] ([MaDonHang], [MaBan], [MaNhanVien], [NgayTao], [MaTrangThai], [GiamGia], [TongTien], [GhiChu]) VALUES (24, 4, 1, CAST(N'2025-05-13T21:19:00.917' AS DateTime), 4, 0, 221000, N'Ghi chú khác
 Phục vụ chính: 
 Loại Khách: ')
SET IDENTITY_INSERT [dbo].[DonHang] OFF
GO
SET IDENTITY_INSERT [dbo].[HinhThucThanhToan] ON 

INSERT [dbo].[HinhThucThanhToan] ([MaHinhThuc], [TenHinhThuc], [MoTa], [TrangThai]) VALUES (1, N'Tiền mặt', N'Thanh toán bằng tiền mặt', 1)
INSERT [dbo].[HinhThucThanhToan] ([MaHinhThuc], [TenHinhThuc], [MoTa], [TrangThai]) VALUES (2, N'Thẻ tín dụng', N'Thanh toán bằng thẻ tín dụng', 1)
INSERT [dbo].[HinhThucThanhToan] ([MaHinhThuc], [TenHinhThuc], [MoTa], [TrangThai]) VALUES (3, N'Chuyển khoản', N'Thanh toán bằng chuyển khoản ngân hàng', 1)
INSERT [dbo].[HinhThucThanhToan] ([MaHinhThuc], [TenHinhThuc], [MoTa], [TrangThai]) VALUES (4, N'Ví điện tử', N'Thanh toán bằng ví điện tử (Momo, ZaloPay...)', 1)
SET IDENTITY_INSERT [dbo].[HinhThucThanhToan] OFF
GO
SET IDENTITY_INSERT [dbo].[HoaDonThanhToan] ON 

INSERT [dbo].[HoaDonThanhToan] ([MaHoaDon], [MaDonHang], [NgayThanhToan], [MaNhanVien], [TongTien], [MaHinhThuc], [TrangThai], [GhiChu], [NgayDatHang]) VALUES (5, 17, NULL, 1, CAST(0 AS Decimal(18, 0)), NULL, N'Chưa thanh toán', N'Chưa thanh toán', CAST(N'2025-05-11T23:55:10.097' AS DateTime))
INSERT [dbo].[HoaDonThanhToan] ([MaHoaDon], [MaDonHang], [NgayThanhToan], [MaNhanVien], [TongTien], [MaHinhThuc], [TrangThai], [GhiChu], [NgayDatHang]) VALUES (6, 18, NULL, 1, CAST(0 AS Decimal(18, 0)), NULL, N'Chưa thanh toán', N'Chưa thanh toán', CAST(N'2025-05-12T00:58:58.507' AS DateTime))
INSERT [dbo].[HoaDonThanhToan] ([MaHoaDon], [MaDonHang], [NgayThanhToan], [MaNhanVien], [TongTien], [MaHinhThuc], [TrangThai], [GhiChu], [NgayDatHang]) VALUES (7, 18, NULL, 1, CAST(0 AS Decimal(18, 0)), NULL, N'Chưa thanh toán', N'Chưa thanh toán', CAST(N'2025-05-12T00:59:19.403' AS DateTime))
INSERT [dbo].[HoaDonThanhToan] ([MaHoaDon], [MaDonHang], [NgayThanhToan], [MaNhanVien], [TongTien], [MaHinhThuc], [TrangThai], [GhiChu], [NgayDatHang]) VALUES (8, 19, CAST(N'2025-05-12T01:10:01.590' AS DateTime), 1, CAST(123000 AS Decimal(18, 0)), 3, N'Đã thanh toán', N'Thanh toán thành công', CAST(N'2025-05-12T01:09:33.787' AS DateTime))
INSERT [dbo].[HoaDonThanhToan] ([MaHoaDon], [MaDonHang], [NgayThanhToan], [MaNhanVien], [TongTien], [MaHinhThuc], [TrangThai], [GhiChu], [NgayDatHang]) VALUES (9, 20, CAST(N'2025-05-12T01:20:59.980' AS DateTime), 1, CAST(142000 AS Decimal(18, 0)), 3, N'Đã thanh toán', N'Thanh toán thành công', CAST(N'2025-05-12T01:20:17.617' AS DateTime))
INSERT [dbo].[HoaDonThanhToan] ([MaHoaDon], [MaDonHang], [NgayThanhToan], [MaNhanVien], [TongTien], [MaHinhThuc], [TrangThai], [GhiChu], [NgayDatHang]) VALUES (10, 21, CAST(N'2025-05-12T12:18:22.373' AS DateTime), 1, CAST(113000 AS Decimal(18, 0)), 3, N'Đã thanh toán', N'Thanh toán thành công', CAST(N'2025-05-12T12:15:12.363' AS DateTime))
INSERT [dbo].[HoaDonThanhToan] ([MaHoaDon], [MaDonHang], [NgayThanhToan], [MaNhanVien], [TongTien], [MaHinhThuc], [TrangThai], [GhiChu], [NgayDatHang]) VALUES (11, 22, CAST(N'2025-05-12T21:10:01.330' AS DateTime), 1, CAST(96050 AS Decimal(18, 0)), 3, N'Đã thanh toán', N'Thanh toán thành công', CAST(N'2025-05-12T21:09:56.183' AS DateTime))
INSERT [dbo].[HoaDonThanhToan] ([MaHoaDon], [MaDonHang], [NgayThanhToan], [MaNhanVien], [TongTien], [MaHinhThuc], [TrangThai], [GhiChu], [NgayDatHang]) VALUES (12, 23, NULL, 1, CAST(0 AS Decimal(18, 0)), NULL, N'Chưa thanh toán', N'Chưa thanh toán', CAST(N'2025-05-13T21:18:48.123' AS DateTime))
INSERT [dbo].[HoaDonThanhToan] ([MaHoaDon], [MaDonHang], [NgayThanhToan], [MaNhanVien], [TongTien], [MaHinhThuc], [TrangThai], [GhiChu], [NgayDatHang]) VALUES (13, 24, CAST(N'2025-05-13T21:22:38.240' AS DateTime), 1, CAST(221000 AS Decimal(18, 0)), 2, N'Đã thanh toán', N'Thanh toán thành công', CAST(N'2025-05-13T21:19:16.323' AS DateTime))
INSERT [dbo].[HoaDonThanhToan] ([MaHoaDon], [MaDonHang], [NgayThanhToan], [MaNhanVien], [TongTien], [MaHinhThuc], [TrangThai], [GhiChu], [NgayDatHang]) VALUES (14, 23, NULL, 1, CAST(0 AS Decimal(18, 0)), NULL, N'Chưa thanh toán', N'Chưa thanh toán', CAST(N'2025-05-13T21:19:32.423' AS DateTime))
INSERT [dbo].[HoaDonThanhToan] ([MaHoaDon], [MaDonHang], [NgayThanhToan], [MaNhanVien], [TongTien], [MaHinhThuc], [TrangThai], [GhiChu], [NgayDatHang]) VALUES (15, 24, NULL, 1, CAST(0 AS Decimal(18, 0)), NULL, N'Chưa thanh toán', N'Chưa thanh toán', CAST(N'2025-05-13T21:19:57.873' AS DateTime))
INSERT [dbo].[HoaDonThanhToan] ([MaHoaDon], [MaDonHang], [NgayThanhToan], [MaNhanVien], [TongTien], [MaHinhThuc], [TrangThai], [GhiChu], [NgayDatHang]) VALUES (16, 24, NULL, 1, CAST(0 AS Decimal(18, 0)), NULL, N'Chưa thanh toán', N'Chưa thanh toán', CAST(N'2025-05-13T21:19:59.230' AS DateTime))
SET IDENTITY_INSERT [dbo].[HoaDonThanhToan] OFF
GO
SET IDENTITY_INSERT [dbo].[KhuyenMai] ON 

INSERT [dbo].[KhuyenMai] ([MaKhuyenMai], [TenKhuyenMai], [NoiDung], [PhanTramGiam], [NgayBatDau], [NgayKetThuc], [TrangThai], [GhiChu]) VALUES (1, N'Khuyến mãi tháng 4', N'Khuyến mãi nhân dịp 30/4 - 1/5', 10, CAST(N'2025-04-20T00:00:00.000' AS DateTime), CAST(N'2025-05-05T23:59:59.000' AS DateTime), 1, NULL)
INSERT [dbo].[KhuyenMai] ([MaKhuyenMai], [TenKhuyenMai], [NoiDung], [PhanTramGiam], [NgayBatDau], [NgayKetThuc], [TrangThai], [GhiChu]) VALUES (2, N'Happy Hour', N'Giảm giá giờ vàng 17h-19h hàng ngày', 15, CAST(N'2025-04-01T17:00:00.000' AS DateTime), CAST(N'2025-12-31T19:00:00.000' AS DateTime), 1, N'Áp dụng từ thứ 2 đến thứ 6')
INSERT [dbo].[KhuyenMai] ([MaKhuyenMai], [TenKhuyenMai], [NoiDung], [PhanTramGiam], [NgayBatDau], [NgayKetThuc], [TrangThai], [GhiChu]) VALUES (4, N'Happy Hour', N'Giảm giá giờ vàng 17h-19h hàng ngày', 15, CAST(N'2025-04-01T17:00:00.000' AS DateTime), CAST(N'2025-12-31T19:00:00.000' AS DateTime), 0, N'Áp dụng từ thứ 2 đến thứ 6')
INSERT [dbo].[KhuyenMai] ([MaKhuyenMai], [TenKhuyenMai], [NoiDung], [PhanTramGiam], [NgayBatDau], [NgayKetThuc], [TrangThai], [GhiChu]) VALUES (1003, N'Chiến thắng Điện Biên Phủ ', N'Lễ lớn giảm 15%', 15, CAST(N'2025-05-12T17:00:00.000' AS DateTime), CAST(N'2025-11-30T19:00:00.000' AS DateTime), 1, N'Áp dụng cả tuần ')
INSERT [dbo].[KhuyenMai] ([MaKhuyenMai], [TenKhuyenMai], [NoiDung], [PhanTramGiam], [NgayBatDau], [NgayKetThuc], [TrangThai], [GhiChu]) VALUES (1004, N'Chiến thắng Điện Biên Phủ ', N'Lễ lớn giảm 15%', 15, CAST(N'2025-05-12T17:00:00.000' AS DateTime), CAST(N'2025-11-30T19:00:00.000' AS DateTime), 1, N'Áp dụng cả tuần ')
INSERT [dbo].[KhuyenMai] ([MaKhuyenMai], [TenKhuyenMai], [NoiDung], [PhanTramGiam], [NgayBatDau], [NgayKetThuc], [TrangThai], [GhiChu]) VALUES (1005, N'Chiến thắng Điện Biên Phủ ', N'Lễ lớn giảm 15%', 20, CAST(N'2025-05-12T17:00:00.000' AS DateTime), CAST(N'2025-11-30T19:00:00.000' AS DateTime), 1, N'Áp dụng cả tuần ')
SET IDENTITY_INSERT [dbo].[KhuyenMai] OFF
GO
INSERT [dbo].[KiemTraChatLuong] ([MaKiemTra], [LoaiKiemTra], [DoiTuongKiemTra], [NgayKiemTra], [NguoiKiemTra], [TieuChiKiemTra], [GiaTri], [DonVi], [KetQua], [GhiChu], [HinhAnh]) VALUES (N'', N'NguyenLieu', N'', CAST(N'2025-04-22T16:46:30.713' AS DateTime), 1, NULL, 0, NULL, N'Ð?t', NULL, NULL)
INSERT [dbo].[KiemTraChatLuong] ([MaKiemTra], [LoaiKiemTra], [DoiTuongKiemTra], [NgayKiemTra], [NguoiKiemTra], [TieuChiKiemTra], [GiaTri], [DonVi], [KetQua], [GhiChu], [HinhAnh]) VALUES (N'KT001', N'NguyenLieu', N'73', CAST(N'2025-04-24T21:24:30.787' AS DateTime), 1, N'Kiểm tra chất lượng nguyên liệu: Bánh cá cuộn rong biển', 0, NULL, N'Đạt', N'Kiểm tra nguyên liệu: Bánh cá cuộn rong biển (kg)', NULL)
INSERT [dbo].[KiemTraChatLuong] ([MaKiemTra], [LoaiKiemTra], [DoiTuongKiemTra], [NgayKiemTra], [NguoiKiemTra], [TieuChiKiemTra], [GiaTri], [DonVi], [KetQua], [GhiChu], [HinhAnh]) VALUES (N'KT002', N'NguyenLieu', N'2', CAST(N'2025-04-05T09:30:00.000' AS DateTime), 1, N'NhietDo', 2.5, N'°C', N'Ð?t', N'Nhiệt độ bảo quản đạt tiêu chuẩn', NULL)
INSERT [dbo].[KiemTraChatLuong] ([MaKiemTra], [LoaiKiemTra], [DoiTuongKiemTra], [NgayKiemTra], [NguoiKiemTra], [TieuChiKiemTra], [GiaTri], [DonVi], [KetQua], [GhiChu], [HinhAnh]) VALUES (N'KT003', N'MonAn', N'3', CAST(N'2025-04-05T12:00:00.000' AS DateTime), 2, N'MauSac', NULL, NULL, N'Ð?t', N'Màu sắc món ăn đạt tiêu chuẩn', N'KT003.jpg')
INSERT [dbo].[KiemTraChatLuong] ([MaKiemTra], [LoaiKiemTra], [DoiTuongKiemTra], [NgayKiemTra], [NguoiKiemTra], [TieuChiKiemTra], [GiaTri], [DonVi], [KetQua], [GhiChu], [HinhAnh]) VALUES (N'KT004', N'NguyenLieu', N'73 - Bánh ', CAST(N'2025-04-23T17:14:28.120' AS DateTime), 1, N'Kiểm tra chất lượng nguyên liệu: Bánh cá cuộn rong', 0, NULL, N'Ð?t', N'Kiểm tra nguyên liệu: Bánh cá cuộn rong biển (kg)', NULL)
INSERT [dbo].[KiemTraChatLuong] ([MaKiemTra], [LoaiKiemTra], [DoiTuongKiemTra], [NgayKiemTra], [NguoiKiemTra], [TieuChiKiemTra], [GiaTri], [DonVi], [KetQua], [GhiChu], [HinhAnh]) VALUES (N'KT005', N'NguyenLieu', N'73 - Bánh ', CAST(N'2025-04-23T17:14:28.120' AS DateTime), 1, N'Kiểm tra chất lượng nguyên liệu: Bánh cá cuộn rong', 0, NULL, N'Ð?t', N'Kiểm tra nguyên liệu: Bánh cá cuộn rong biển (kg)', NULL)
INSERT [dbo].[KiemTraChatLuong] ([MaKiemTra], [LoaiKiemTra], [DoiTuongKiemTra], [NgayKiemTra], [NguoiKiemTra], [TieuChiKiemTra], [GiaTri], [DonVi], [KetQua], [GhiChu], [HinhAnh]) VALUES (N'KT006', N'NguyenLieu', N'73 - Bánh ', CAST(N'2025-04-23T17:16:29.960' AS DateTime), 1, N'Kiểm tra chất lượng nguyên liệu: Bánh cá cuộn rong', 0, NULL, N'Ð?t', N'Kiểm tra nguyên liệu: Bánh cá cuộn rong biển (kg)', NULL)
INSERT [dbo].[KiemTraChatLuong] ([MaKiemTra], [LoaiKiemTra], [DoiTuongKiemTra], [NgayKiemTra], [NguoiKiemTra], [TieuChiKiemTra], [GiaTri], [DonVi], [KetQua], [GhiChu], [HinhAnh]) VALUES (N'KT007', N'NguyenLieu', N'73 - Bánh ', CAST(N'2025-04-23T17:16:29.960' AS DateTime), 1, N'Kiểm tra chất lượng nguyên liệu: Bánh cá cuộn rong', 0, NULL, N'Ð?t', N'Kiểm tra nguyên liệu: Bánh cá cuộn rong biển (kg)', NULL)
INSERT [dbo].[KiemTraChatLuong] ([MaKiemTra], [LoaiKiemTra], [DoiTuongKiemTra], [NgayKiemTra], [NguoiKiemTra], [TieuChiKiemTra], [GiaTri], [DonVi], [KetQua], [GhiChu], [HinhAnh]) VALUES (N'KT008', N'NguyenLieu', N'70 - Ba ch', CAST(N'2025-04-23T17:17:45.873' AS DateTime), 1, N'Kiểm tra chất lượng nguyên liệu: Ba chỉ heo cuộn', 0, NULL, N'Ð?t', N'Kiểm tra nguyên liệu: Ba chỉ heo cuộn (kg)', NULL)
INSERT [dbo].[KiemTraChatLuong] ([MaKiemTra], [LoaiKiemTra], [DoiTuongKiemTra], [NgayKiemTra], [NguoiKiemTra], [TieuChiKiemTra], [GiaTri], [DonVi], [KetQua], [GhiChu], [HinhAnh]) VALUES (N'KT009', N'NguyenLieu', N'73 - Bánh', CAST(N'2025-04-23T17:23:16.963' AS DateTime), 1, N'Kiểm tra chất lượng nguyên liệu: Bánh cá cuộn rong', 0, NULL, N'Ð?t', N'Kiểm tra nguyên liệu: Bánh cá cuộn rong biển (kg)', NULL)
INSERT [dbo].[KiemTraChatLuong] ([MaKiemTra], [LoaiKiemTra], [DoiTuongKiemTra], [NgayKiemTra], [NguoiKiemTra], [TieuChiKiemTra], [GiaTri], [DonVi], [KetQua], [GhiChu], [HinhAnh]) VALUES (N'KT010', N'MonAn', N'5', CAST(N'2025-04-23T17:29:30.047' AS DateTime), 1, N'Hương vị', 0, NULL, N'Ð?t', N'Kiểm tra món ăn: Phô mai que (39000)', NULL)
INSERT [dbo].[KiemTraChatLuong] ([MaKiemTra], [LoaiKiemTra], [DoiTuongKiemTra], [NgayKiemTra], [NguoiKiemTra], [TieuChiKiemTra], [GiaTri], [DonVi], [KetQua], [GhiChu], [HinhAnh]) VALUES (N'KT011', N'NguyenLieu', N'', CAST(N'2025-04-23T17:33:51.180' AS DateTime), 1, NULL, 0, NULL, N'Ð?t', NULL, NULL)
INSERT [dbo].[KiemTraChatLuong] ([MaKiemTra], [LoaiKiemTra], [DoiTuongKiemTra], [NgayKiemTra], [NguoiKiemTra], [TieuChiKiemTra], [GiaTri], [DonVi], [KetQua], [GhiChu], [HinhAnh]) VALUES (N'KT013', N'MonAn', N'5', CAST(N'2025-04-23T17:39:44.677' AS DateTime), 1, N'Hương vị', 0, NULL, N'Ð?t', N'Kiểm tra món ăn: Phô mai que (39000)', NULL)
INSERT [dbo].[KiemTraChatLuong] ([MaKiemTra], [LoaiKiemTra], [DoiTuongKiemTra], [NgayKiemTra], [NguoiKiemTra], [TieuChiKiemTra], [GiaTri], [DonVi], [KetQua], [GhiChu], [HinhAnh]) VALUES (N'KT014', N'NguyenLieu', N'70', CAST(N'2025-04-23T17:43:27.090' AS DateTime), 1, N'Kiểm tra chất lượng nguyên liệu: Ba chỉ heo cuộn', 0, NULL, N'Ð?t', N'Kiểm tra nguyên liệu: Ba chỉ heo cuộn (kg)', NULL)
INSERT [dbo].[KiemTraChatLuong] ([MaKiemTra], [LoaiKiemTra], [DoiTuongKiemTra], [NgayKiemTra], [NguoiKiemTra], [TieuChiKiemTra], [GiaTri], [DonVi], [KetQua], [GhiChu], [HinhAnh]) VALUES (N'KT015', N'NguyenLieu', N'70', CAST(N'2025-04-23T18:07:10.683' AS DateTime), 1, N'Kiểm tra chất lượng nguyên liệu: Ba chỉ heo cuộn', 0, NULL, N'Ð?t', N'Kiểm tra nguyên liệu: Ba chỉ heo cuộn (kg)', NULL)
INSERT [dbo].[KiemTraChatLuong] ([MaKiemTra], [LoaiKiemTra], [DoiTuongKiemTra], [NgayKiemTra], [NguoiKiemTra], [TieuChiKiemTra], [GiaTri], [DonVi], [KetQua], [GhiChu], [HinhAnh]) VALUES (N'KT016', N'MonAn', N'5', CAST(N'2025-04-23T18:10:53.507' AS DateTime), 1, N'Hương vị', 0, NULL, N'Ð?t', N'Kiểm tra món ăn: Phô mai que (39000)', NULL)
INSERT [dbo].[KiemTraChatLuong] ([MaKiemTra], [LoaiKiemTra], [DoiTuongKiemTra], [NgayKiemTra], [NguoiKiemTra], [TieuChiKiemTra], [GiaTri], [DonVi], [KetQua], [GhiChu], [HinhAnh]) VALUES (N'KT017', N'NguyenLieu', N'67', CAST(N'2025-04-23T18:32:21.167' AS DateTime), 1, N'Kiểm tra chất lượng nguyên liệu: Bánh bạch tuộc (takoyaki)', 0, NULL, N'Ð?t', N'Kiểm tra nguyên liệu: Bánh bạch tuộc (takoyaki) (kg)', NULL)
INSERT [dbo].[KiemTraChatLuong] ([MaKiemTra], [LoaiKiemTra], [DoiTuongKiemTra], [NgayKiemTra], [NguoiKiemTra], [TieuChiKiemTra], [GiaTri], [DonVi], [KetQua], [GhiChu], [HinhAnh]) VALUES (N'KT019', N'NguyenLieu', N'70', CAST(N'2025-04-24T18:26:02.160' AS DateTime), 1, N'Kiểm tra chất lượng nguyên liệu: Ba chỉ heo cuộn', 0, NULL, N'Cần kiểm tra lại', N'Kiểm tra nguyên liệu: Ba chỉ heo cuộn (kg)', NULL)
INSERT [dbo].[KiemTraChatLuong] ([MaKiemTra], [LoaiKiemTra], [DoiTuongKiemTra], [NgayKiemTra], [NguoiKiemTra], [TieuChiKiemTra], [GiaTri], [DonVi], [KetQua], [GhiChu], [HinhAnh]) VALUES (N'KT020', N'NguyenLieu', N'67', CAST(N'2025-04-24T18:26:33.090' AS DateTime), 1, N'Kiểm tra chất lượng nguyên liệu: Bánh bạch tuộc (takoyaki)', 0, NULL, N'Cần kiểm tra lại', N'Kiểm tra nguyên liệu: Bánh bạch tuộc (takoyaki) (kg)', NULL)
INSERT [dbo].[KiemTraChatLuong] ([MaKiemTra], [LoaiKiemTra], [DoiTuongKiemTra], [NgayKiemTra], [NguoiKiemTra], [TieuChiKiemTra], [GiaTri], [DonVi], [KetQua], [GhiChu], [HinhAnh]) VALUES (N'KT023', N'MonAn', N'3', CAST(N'2025-04-24T21:00:49.217' AS DateTime), 1, N'Hương vị', 0, NULL, N'Không đạt', N'Kiểm tra món ăn: Bánh bạch tuộc (39000)', NULL)
INSERT [dbo].[KiemTraChatLuong] ([MaKiemTra], [LoaiKiemTra], [DoiTuongKiemTra], [NgayKiemTra], [NguoiKiemTra], [TieuChiKiemTra], [GiaTri], [DonVi], [KetQua], [GhiChu], [HinhAnh]) VALUES (N'KT12', N'NguyenLieu', N'94', CAST(N'2025-04-24T21:27:12.620' AS DateTime), 1, N'Kiểm tra chất lượng nguyên liệu: Bánh gạo Hàn Quốc Ofood', 0, NULL, N'Đạt', N'Kiểm tra nguyên liệu: Bánh gạo Hàn Quốc Ofood (kg)', NULL)
INSERT [dbo].[KiemTraChatLuong] ([MaKiemTra], [LoaiKiemTra], [DoiTuongKiemTra], [NgayKiemTra], [NguoiKiemTra], [TieuChiKiemTra], [GiaTri], [DonVi], [KetQua], [GhiChu], [HinhAnh]) VALUES (N'KT18', N'NguyenLieu', N'70', CAST(N'2025-04-24T23:01:26.520' AS DateTime), 1, N'Kiểm tra chất lượng nguyên liệu: Ba chỉ heo cuộn', 20000, N'Kg', N'Đạt', N'Kiểm tra nguyên liệu: Ba chỉ heo cuộn (kg)', NULL)
INSERT [dbo].[KiemTraChatLuong] ([MaKiemTra], [LoaiKiemTra], [DoiTuongKiemTra], [NgayKiemTra], [NguoiKiemTra], [TieuChiKiemTra], [GiaTri], [DonVi], [KetQua], [GhiChu], [HinhAnh]) VALUES (N'KT21', N'MonAn', N'3', CAST(N'2025-05-09T21:26:45.717' AS DateTime), 1, N'Hương vị', 0, NULL, N'Đạt', N'Kiểm tra món ăn: Bánh bạch tuộc (39000)', NULL)
INSERT [dbo].[KiemTraChatLuong] ([MaKiemTra], [LoaiKiemTra], [DoiTuongKiemTra], [NgayKiemTra], [NguoiKiemTra], [TieuChiKiemTra], [GiaTri], [DonVi], [KetQua], [GhiChu], [HinhAnh]) VALUES (N'KT22', N'NguyenLieu', N'73', CAST(N'2025-04-26T13:48:14.037' AS DateTime), 1, N'Kiểm tra chất lượng nguyên liệu: Bánh cá cuộn rong biển', 0, NULL, N'Cần kiểm tra lại', N'Kiểm tra nguyên liệu: Bánh cá cuộn rong biển (kg)', NULL)
INSERT [dbo].[KiemTraChatLuong] ([MaKiemTra], [LoaiKiemTra], [DoiTuongKiemTra], [NgayKiemTra], [NguoiKiemTra], [TieuChiKiemTra], [GiaTri], [DonVi], [KetQua], [GhiChu], [HinhAnh]) VALUES (N'KT24', N'NguyenLieu', N'21', CAST(N'2025-05-09T21:25:25.087' AS DateTime), 1, N'Kiểm tra chất lượng nguyên liệu: Nước ép dưa lưới đông lạnh', 0, NULL, N'Không đạt', N'Kiểm tra nguyên liệu: Nước ép dưa lưới đông lạnh (lít)', NULL)
INSERT [dbo].[KiemTraChatLuong] ([MaKiemTra], [LoaiKiemTra], [DoiTuongKiemTra], [NgayKiemTra], [NguoiKiemTra], [TieuChiKiemTra], [GiaTri], [DonVi], [KetQua], [GhiChu], [HinhAnh]) VALUES (N'KT25', N'MonAn', N'2', CAST(N'2025-05-06T00:05:27.247' AS DateTime), 1, N'Hương vị', 0, NULL, N'Đạt', N'Kiểm tra món ăn: Sụn gà bắp (45000)', NULL)
INSERT [dbo].[KiemTraChatLuong] ([MaKiemTra], [LoaiKiemTra], [DoiTuongKiemTra], [NgayKiemTra], [NguoiKiemTra], [TieuChiKiemTra], [GiaTri], [DonVi], [KetQua], [GhiChu], [HinhAnh]) VALUES (N'KT26', N'NguyenLieu', N'94', CAST(N'2025-04-26T13:48:49.397' AS DateTime), 1, N'Kiểm tra chất lượng nguyên liệu: Bánh gạo Hàn Quốc Ofood', 0, NULL, N'Không đạt', N'Kiểm tra nguyên liệu: Bánh gạo Hàn Quốc Ofood (kg)', NULL)
INSERT [dbo].[KiemTraChatLuong] ([MaKiemTra], [LoaiKiemTra], [DoiTuongKiemTra], [NgayKiemTra], [NguoiKiemTra], [TieuChiKiemTra], [GiaTri], [DonVi], [KetQua], [GhiChu], [HinhAnh]) VALUES (N'KT27', N'NguyenLieu', N'64', CAST(N'2025-05-06T18:33:11.457' AS DateTime), 1, N'Kiểm tra chất lượng nguyên liệu: Nước dùng tương hàn', 0, NULL, N'Đạt', N'Kiểm tra nguyên liệu: Nước dùng tương hàn (lít)', NULL)
INSERT [dbo].[KiemTraChatLuong] ([MaKiemTra], [LoaiKiemTra], [DoiTuongKiemTra], [NgayKiemTra], [NguoiKiemTra], [TieuChiKiemTra], [GiaTri], [DonVi], [KetQua], [GhiChu], [HinhAnh]) VALUES (N'KT28', N'MonAn', N'4', CAST(N'2025-05-10T18:01:38.180' AS DateTime), 1, N'Hương vị', 0, NULL, N'Đạt', N'Kiểm tra món ăn: Phô mai viên (29000)', NULL)
INSERT [dbo].[KiemTraChatLuong] ([MaKiemTra], [LoaiKiemTra], [DoiTuongKiemTra], [NgayKiemTra], [NguoiKiemTra], [TieuChiKiemTra], [GiaTri], [DonVi], [KetQua], [GhiChu], [HinhAnh]) VALUES (N'KT29', N'MonAn', N'2', CAST(N'2025-05-11T04:11:17.257' AS DateTime), 1, N'Hương vị', 0, NULL, N'Cần kiểm tra lại', N'Kiểm tra món ăn: Sụn gà bắp (45000)', NULL)
INSERT [dbo].[KiemTraChatLuong] ([MaKiemTra], [LoaiKiemTra], [DoiTuongKiemTra], [NgayKiemTra], [NguoiKiemTra], [TieuChiKiemTra], [GiaTri], [DonVi], [KetQua], [GhiChu], [HinhAnh]) VALUES (N'KT30', N'NguyenLieu', N'67', CAST(N'2025-05-13T01:21:27.517' AS DateTime), 1, N'Kiểm tra chất lượng nguyên liệu: Bánh bạch tuộc (takoyaki)', 0, NULL, N'Không đạt', N'Kiểm tra nguyên liệu: Bánh bạch tuộc (takoyaki) (kg)', NULL)
INSERT [dbo].[KiemTraChatLuong] ([MaKiemTra], [LoaiKiemTra], [DoiTuongKiemTra], [NgayKiemTra], [NguoiKiemTra], [TieuChiKiemTra], [GiaTri], [DonVi], [KetQua], [GhiChu], [HinhAnh]) VALUES (N'KT31', N'NguyenLieu', N'70', CAST(N'2025-04-23T21:48:29.097' AS DateTime), 1, N'Kiểm tra chất lượng nguyên liệu: Ba chỉ heo cuộn', 0, NULL, N'Đạt', N'Kiểm tra nguyên liệu: Ba chỉ heo cuộn (kg)', NULL)
INSERT [dbo].[KiemTraChatLuong] ([MaKiemTra], [LoaiKiemTra], [DoiTuongKiemTra], [NgayKiemTra], [NguoiKiemTra], [TieuChiKiemTra], [GiaTri], [DonVi], [KetQua], [GhiChu], [HinhAnh]) VALUES (N'KT32', N'MonAn', N'3', CAST(N'2025-05-13T21:48:47.707' AS DateTime), 1, N'Hương vị', 0, NULL, N'Đạt', N'Kiểm tra món ăn: Bánh bạch tuộc (39000)', NULL)
GO
SET IDENTITY_INSERT [dbo].[LoaiMonAn] ON 

INSERT [dbo].[LoaiMonAn] ([MaLoai], [TenLoai], [MoTa]) VALUES (1, N'Khai vị', N'Các món khai vị')
INSERT [dbo].[LoaiMonAn] ([MaLoai], [TenLoai], [MoTa]) VALUES (2, N'Mì cay', N'Các món ăn chính')
INSERT [dbo].[LoaiMonAn] ([MaLoai], [TenLoai], [MoTa]) VALUES (3, N'Món phụ', N'Các món lẩu ')
INSERT [dbo].[LoaiMonAn] ([MaLoai], [TenLoai], [MoTa]) VALUES (4, N'Đồ uống', N'Các loại đồ uống')
INSERT [dbo].[LoaiMonAn] ([MaLoai], [TenLoai], [MoTa]) VALUES (5, N'Combo', N'Các món ăn combo')
INSERT [dbo].[LoaiMonAn] ([MaLoai], [TenLoai], [MoTa]) VALUES (6, N'Lẩu', N'Các món lẩu')
SET IDENTITY_INSERT [dbo].[LoaiMonAn] OFF
GO
SET IDENTITY_INSERT [dbo].[LoaiTaiKhoan] ON 

INSERT [dbo].[LoaiTaiKhoan] ([MaLoai], [TenLoai], [MoTa]) VALUES (1, N'Admin', N'Quản trị viên hệ thống')
INSERT [dbo].[LoaiTaiKhoan] ([MaLoai], [TenLoai], [MoTa]) VALUES (2, N'Nhân viên', N'Nhân viên phục vụ')
INSERT [dbo].[LoaiTaiKhoan] ([MaLoai], [TenLoai], [MoTa]) VALUES (3, N'Khách hàng', N'Tài khoản khách hàng')
SET IDENTITY_INSERT [dbo].[LoaiTaiKhoan] OFF
GO
SET IDENTITY_INSERT [dbo].[MonAn] ON 

INSERT [dbo].[MonAn] ([MaMonAn], [TenMonAn], [MaLoai], [DonGia], [MoTa], [HinhAnh], [TrangThai]) VALUES (1, N'Kimbap', 1, 45000, N'Cơm cuộn rong biển kiểu Hàn Quốc', N'kimbap.jpg', 1)
INSERT [dbo].[MonAn] ([MaMonAn], [TenMonAn], [MaLoai], [DonGia], [MoTa], [HinhAnh], [TrangThai]) VALUES (2, N'Sụn gà bắp', 1, 40000, N'Sụn gà chiên giòn với bắp ngọt', N'sun-ga-bap.jpg', 1)
INSERT [dbo].[MonAn] ([MaMonAn], [TenMonAn], [MaLoai], [DonGia], [MoTa], [HinhAnh], [TrangThai]) VALUES (3, N'Bánh bạch tuộc', 1, 39000, N'Bánh viên bạch tuộc truyền thống', N'banh-bach-tuoc.jpg', 1)
INSERT [dbo].[MonAn] ([MaMonAn], [TenMonAn], [MaLoai], [DonGia], [MoTa], [HinhAnh], [TrangThai]) VALUES (4, N'Phô mai viên', 1, 29000, N'Viên phô mai chiên giòn', N'pho-mai-vien.jpg', 1)
INSERT [dbo].[MonAn] ([MaMonAn], [TenMonAn], [MaLoai], [DonGia], [MoTa], [HinhAnh], [TrangThai]) VALUES (5, N'Phô mai que', 1, 39000, N'Phô mai que chiên giòn', N'pho-mai-que.jpg', 1)
INSERT [dbo].[MonAn] ([MaMonAn], [TenMonAn], [MaLoai], [DonGia], [MoTa], [HinhAnh], [TrangThai]) VALUES (6, N'Gà viên chiên giòn', 1, 29000, N'Viên gà tẩm bột chiên giòn', N'ga-vien-chien-gion.jpg', 1)
INSERT [dbo].[MonAn] ([MaMonAn], [TenMonAn], [MaLoai], [DonGia], [MoTa], [HinhAnh], [TrangThai]) VALUES (7, N'Đùi gà giòn', 1, 39000, N'Đùi gà rút xương chiên giòn', N'dui-ga-gion.jpg', 1)
INSERT [dbo].[MonAn] ([MaMonAn], [TenMonAn], [MaLoai], [DonGia], [MoTa], [HinhAnh], [TrangThai]) VALUES (8, N'Viên thanh cua phomai', 1, 45000, N'Viên thanh cua nhân phô mai béo ngậy', N'vien-thanh-cua-phomai.jpg', 1)
INSERT [dbo].[MonAn] ([MaMonAn], [TenMonAn], [MaLoai], [DonGia], [MoTa], [HinhAnh], [TrangThai]) VALUES (9, N'Salat xốt mè rang', 1, 35000, N'Salat tươi với xốt mè rang đặc biệt', N'salat-xot-me-rang.jpg', 1)
INSERT [dbo].[MonAn] ([MaMonAn], [TenMonAn], [MaLoai], [DonGia], [MoTa], [HinhAnh], [TrangThai]) VALUES (10, N'Rong biển cuộn fillet cá chiên', 1, 39000, N'Rong biển cuộn cá fillet chiên giòn', N'rong-bien-cuon-fillet-ca.jpg', 1)
INSERT [dbo].[MonAn] ([MaMonAn], [TenMonAn], [MaLoai], [DonGia], [MoTa], [HinhAnh], [TrangThai]) VALUES (11, N'Khoai tây chiên', 1, 32000, N'Khoai tây chiên giòn', N'khoai-tay-chien.jpg', 1)
INSERT [dbo].[MonAn] ([MaMonAn], [TenMonAn], [MaLoai], [DonGia], [MoTa], [HinhAnh], [TrangThai]) VALUES (12, N'Mandu chiên xốt cay', 1, 35000, N'Bánh xếp Hàn Quốc chiên với xốt cay', N'mandu-chien-xot-cay.jpg', 1)
INSERT [dbo].[MonAn] ([MaMonAn], [TenMonAn], [MaLoai], [DonGia], [MoTa], [HinhAnh], [TrangThai]) VALUES (13, N'Xiên bánh cá hầm', 1, 42000, N'Xiên bánh cá hầm kiểu Hàn Quốc', N'xien-banh-ca-ham.jpg', 1)
INSERT [dbo].[MonAn] ([MaMonAn], [TenMonAn], [MaLoai], [DonGia], [MoTa], [HinhAnh], [TrangThai]) VALUES (14, N'Kim chi cá', 2, 49000, N'Mì Kim chi cá', N'kim-chi-ca.jpg', 1)
INSERT [dbo].[MonAn] ([MaMonAn], [TenMonAn], [MaLoai], [DonGia], [MoTa], [HinhAnh], [TrangThai]) VALUES (15, N'Kim chi hải sản', 2, 62000, N'Mì Kim chi hải sản', N'kim-chi-hai-san.jpg', 1)
INSERT [dbo].[MonAn] ([MaMonAn], [TenMonAn], [MaLoai], [DonGia], [MoTa], [HinhAnh], [TrangThai]) VALUES (16, N'Kim chi thập cẩm', 2, 69000, N'Mì Kim chi thập cẩm', N'kim-chi-thap-cam.jpg', 1)
INSERT [dbo].[MonAn] ([MaMonAn], [TenMonAn], [MaLoai], [DonGia], [MoTa], [HinhAnh], [TrangThai]) VALUES (17, N'Kim chi bò', 2, 59000, N'Mì Kim chi bò', N'kim-chi-bo.jpg', 1)
INSERT [dbo].[MonAn] ([MaMonAn], [TenMonAn], [MaLoai], [DonGia], [MoTa], [HinhAnh], [TrangThai]) VALUES (18, N'Kim chi đùi gà', 2, 55000, N'Mì Kim chi đùi gà', N'kim-chi-dui-ga.jpg', 1)
INSERT [dbo].[MonAn] ([MaMonAn], [TenMonAn], [MaLoai], [DonGia], [MoTa], [HinhAnh], [TrangThai]) VALUES (19, N'Kim chi gogi', 2, 49000, N'Mì Kim chi gogi', N'kim-chi-gogi.jpg', 1)
INSERT [dbo].[MonAn] ([MaMonAn], [TenMonAn], [MaLoai], [DonGia], [MoTa], [HinhAnh], [TrangThai]) VALUES (20, N'Soyum hải sản', 2, 62000, N'Mì Soyum hải sản', N'soyum-hai-san.jpg', 1)
INSERT [dbo].[MonAn] ([MaMonAn], [TenMonAn], [MaLoai], [DonGia], [MoTa], [HinhAnh], [TrangThai]) VALUES (21, N'Soyum thập cẩm', 2, 69000, N'Mì Soyum thập cẩm', N'soyum-thap-cam.jpg', 1)
INSERT [dbo].[MonAn] ([MaMonAn], [TenMonAn], [MaLoai], [DonGia], [MoTa], [HinhAnh], [TrangThai]) VALUES (22, N'Soyum bò', 2, 59000, N'Mì Soyum bò', N'soyum-bo.jpg', 1)
INSERT [dbo].[MonAn] ([MaMonAn], [TenMonAn], [MaLoai], [DonGia], [MoTa], [HinhAnh], [TrangThai]) VALUES (23, N'Soyum đùi gà', 2, 59000, N'Mì Soyum đùi gà', N'soyum-dui-ga.jpg', 1)
INSERT [dbo].[MonAn] ([MaMonAn], [TenMonAn], [MaLoai], [DonGia], [MoTa], [HinhAnh], [TrangThai]) VALUES (24, N'Sincay bò', 2, 59000, N'Mì Sincay bò', N'sincay-bo.jpg', 1)
INSERT [dbo].[MonAn] ([MaMonAn], [TenMonAn], [MaLoai], [DonGia], [MoTa], [HinhAnh], [TrangThai]) VALUES (25, N'Sincay đùi gà', 2, 59000, N'Mì Sincay đùi gà', N'sincay-dui-ga.jpg', 1)
INSERT [dbo].[MonAn] ([MaMonAn], [TenMonAn], [MaLoai], [DonGia], [MoTa], [HinhAnh], [TrangThai]) VALUES (26, N'Sincay hải sản', 2, 62000, N'Mì Sincay hải sản', N'sincay-hai-san.jpg', 1)
INSERT [dbo].[MonAn] ([MaMonAn], [TenMonAn], [MaLoai], [DonGia], [MoTa], [HinhAnh], [TrangThai]) VALUES (27, N'Mì trộn tương đen bò mỹ', 3, 65000, N'Mì trộn tương đen bò mỹ', N'mi-tuong-den-bo.jpg', 1)
INSERT [dbo].[MonAn] ([MaMonAn], [TenMonAn], [MaLoai], [DonGia], [MoTa], [HinhAnh], [TrangThai]) VALUES (28, N'Mì trộn tương đen heo cuộn', 3, 65000, N'Mì trộn tương đen heo cuộn', N'mi-tuong-den-heo.jpg', 1)
INSERT [dbo].[MonAn] ([MaMonAn], [TenMonAn], [MaLoai], [DonGia], [MoTa], [HinhAnh], [TrangThai]) VALUES (29, N'Mì trộn tương đen gà', 3, 59000, N'Mì trộn tương đen gà', N'mi-tuong-den-ga.jpg', 1)
INSERT [dbo].[MonAn] ([MaMonAn], [TenMonAn], [MaLoai], [DonGia], [MoTa], [HinhAnh], [TrangThai]) VALUES (30, N'Mì trộn tương đen mandu', 3, 55000, N'Mì trộn tương đen mandu', N'mi-tuong-den-mandu.jpg', 1)
INSERT [dbo].[MonAn] ([MaMonAn], [TenMonAn], [MaLoai], [DonGia], [MoTa], [HinhAnh], [TrangThai]) VALUES (31, N'Mì xào sasin', 3, 55000, N'Mì xào sasin', N'mi-xao-sasin.jpg', 1)
INSERT [dbo].[MonAn] ([MaMonAn], [TenMonAn], [MaLoai], [DonGia], [MoTa], [HinhAnh], [TrangThai]) VALUES (32, N'Miếng trộn Hàn Quốc', 3, 59000, N'Miếng trộn Hàn Quốc', N'mieng-tron-han-quoc.jpg', 1)
INSERT [dbo].[MonAn] ([MaMonAn], [TenMonAn], [MaLoai], [DonGia], [MoTa], [HinhAnh], [TrangThai]) VALUES (33, N'Cơm trộn thịt bò mỹ', 3, 59000, N'Cơm trộn thịt bò mỹ', N'com-tron-bo-my.jpg', 1)
INSERT [dbo].[MonAn] ([MaMonAn], [TenMonAn], [MaLoai], [DonGia], [MoTa], [HinhAnh], [TrangThai]) VALUES (34, N'Cơm canh kim chi', 3, 55000, N'Cơm canh kim chi', N'com-canh-kim-chi.jpg', 1)
INSERT [dbo].[MonAn] ([MaMonAn], [TenMonAn], [MaLoai], [DonGia], [MoTa], [HinhAnh], [TrangThai]) VALUES (35, N'Mì tương Hàn thịt heo cuộn', 3, 62000, N'Mì tương Hàn thịt heo cuộn', N'mi-tuong-han-heo.jpg', 1)
INSERT [dbo].[MonAn] ([MaMonAn], [TenMonAn], [MaLoai], [DonGia], [MoTa], [HinhAnh], [TrangThai]) VALUES (36, N'Mì tương Hàn mandu', 3, 49000, N'Mì tương Hàn mandu', N'mi-tuong-han-mandu.jpg', 1)
INSERT [dbo].[MonAn] ([MaMonAn], [TenMonAn], [MaLoai], [DonGia], [MoTa], [HinhAnh], [TrangThai]) VALUES (37, N'Tokk heo xào', 3, 59000, N'Tokk heo xào', N'tokk-heo-xao.jpg', 1)
INSERT [dbo].[MonAn] ([MaMonAn], [TenMonAn], [MaLoai], [DonGia], [MoTa], [HinhAnh], [TrangThai]) VALUES (38, N'Tokk bò mỹ', 3, 62000, N'Tokk bò mỹ', N'tokk-bo-my.jpg', 1)
INSERT [dbo].[MonAn] ([MaMonAn], [TenMonAn], [MaLoai], [DonGia], [MoTa], [HinhAnh], [TrangThai]) VALUES (39, N'Mì trộn xốt phô mai', 3, 59000, N'Mì trộn xốt phô mai', N'mi-tron-pho-mai.jpg', 1)
INSERT [dbo].[MonAn] ([MaMonAn], [TenMonAn], [MaLoai], [DonGia], [MoTa], [HinhAnh], [TrangThai]) VALUES (40, N'Lẩu bò', 6, 199000, N'Lẩu bò 3 loại nước lèo', N'lau-bo.jpg', 1)
INSERT [dbo].[MonAn] ([MaMonAn], [TenMonAn], [MaLoai], [DonGia], [MoTa], [HinhAnh], [TrangThai]) VALUES (41, N'Lẩu hải sản', 6, 199000, N'Lẩu hải sản 3 loại nước lèo', N'lau-hai-san.jpg', 1)
INSERT [dbo].[MonAn] ([MaMonAn], [TenMonAn], [MaLoai], [DonGia], [MoTa], [HinhAnh], [TrangThai]) VALUES (42, N'Lẩu tok bò', 6, 199000, N'Lẩu tok bò', N'lau-tok-bo.jpg', 1)
INSERT [dbo].[MonAn] ([MaMonAn], [TenMonAn], [MaLoai], [DonGia], [MoTa], [HinhAnh], [TrangThai]) VALUES (43, N'Lẩu tok hải sản', 6, 199000, N'Lẩu tok hải sản', N'lau-tok-hai-san.jpg', 1)
INSERT [dbo].[MonAn] ([MaMonAn], [TenMonAn], [MaLoai], [DonGia], [MoTa], [HinhAnh], [TrangThai]) VALUES (44, N'Coca tươi size R', 4, 23000, N'Coca cola size R', N'coca-r.jpg', 1)
INSERT [dbo].[MonAn] ([MaMonAn], [TenMonAn], [MaLoai], [DonGia], [MoTa], [HinhAnh], [TrangThai]) VALUES (45, N'Coca tươi size L', 4, 27000, N'Coca cola size L', N'coca-l.jpg', 1)
INSERT [dbo].[MonAn] ([MaMonAn], [TenMonAn], [MaLoai], [DonGia], [MoTa], [HinhAnh], [TrangThai]) VALUES (46, N'Sprite tươi size R', 4, 23000, N'Sprite size R', N'sprite-r.jpg', 1)
INSERT [dbo].[MonAn] ([MaMonAn], [TenMonAn], [MaLoai], [DonGia], [MoTa], [HinhAnh], [TrangThai]) VALUES (47, N'Sprite tươi size L', 4, 27000, N'Sprite size L', N'sprite-l.jpg', 1)
INSERT [dbo].[MonAn] ([MaMonAn], [TenMonAn], [MaLoai], [DonGia], [MoTa], [HinhAnh], [TrangThai]) VALUES (48, N'Coca lon', 4, 29000, N'Coca cola lon', N'coca-lon.jpg', 1)
INSERT [dbo].[MonAn] ([MaMonAn], [TenMonAn], [MaLoai], [DonGia], [MoTa], [HinhAnh], [TrangThai]) VALUES (49, N'Sprite lon', 4, 29000, N'Sprite lon', N'sprite-lon.jpg', 1)
INSERT [dbo].[MonAn] ([MaMonAn], [TenMonAn], [MaLoai], [DonGia], [MoTa], [HinhAnh], [TrangThai]) VALUES (50, N'String lon', 4, 29000, N'String lon', N'string-lon.jpg', 1)
INSERT [dbo].[MonAn] ([MaMonAn], [TenMonAn], [MaLoai], [DonGia], [MoTa], [HinhAnh], [TrangThai]) VALUES (51, N'Nước suối chai', 4, 20000, N'Nước suối', N'nuoc-suoi.jpg', 1)
INSERT [dbo].[MonAn] ([MaMonAn], [TenMonAn], [MaLoai], [DonGia], [MoTa], [HinhAnh], [TrangThai]) VALUES (52, N'Trà sữa truyền thống trân châu đen', 4, 29000, N'Trà sữa truyền thống', N'tra-sua-truyen-thong.jpg', 1)
INSERT [dbo].[MonAn] ([MaMonAn], [TenMonAn], [MaLoai], [DonGia], [MoTa], [HinhAnh], [TrangThai]) VALUES (53, N'Trà sữa matcha trân châu đen', 4, 29000, N'Trà sữa matcha', N'tra-sua-matcha.jpg', 1)
INSERT [dbo].[MonAn] ([MaMonAn], [TenMonAn], [MaLoai], [DonGia], [MoTa], [HinhAnh], [TrangThai]) VALUES (54, N'Trà đào', 4, 29000, N'Trà đào', N'tra-dao.jpg', 1)
INSERT [dbo].[MonAn] ([MaMonAn], [TenMonAn], [MaLoai], [DonGia], [MoTa], [HinhAnh], [TrangThai]) VALUES (55, N'Trà dâu hoa hồng', 4, 29000, N'Trà dâu hoa hồng', N'tra-dau-hoa-hong.jpg', 1)
INSERT [dbo].[MonAn] ([MaMonAn], [TenMonAn], [MaLoai], [DonGia], [MoTa], [HinhAnh], [TrangThai]) VALUES (56, N'Nước gạo Hàn Quốc', 4, 35000, N'Nước gạo Hàn Quốc', N'nuoc-gao-han-quoc.jpg', 1)
INSERT [dbo].[MonAn] ([MaMonAn], [TenMonAn], [MaLoai], [DonGia], [MoTa], [HinhAnh], [TrangThai]) VALUES (57, N'Soda dâu dưa lưới', 4, 35000, N'Soda dâu dưa lưới', N'soda-dau-dua-luoi.jpg', 1)
INSERT [dbo].[MonAn] ([MaMonAn], [TenMonAn], [MaLoai], [DonGia], [MoTa], [HinhAnh], [TrangThai]) VALUES (58, N'Soda dừa dứa đác thơm', 4, 35000, N'Soda dừa dứa đác thơm', N'soda-dua-dua-thom.jpg', 1)
INSERT [dbo].[MonAn] ([MaMonAn], [TenMonAn], [MaLoai], [DonGia], [MoTa], [HinhAnh], [TrangThai]) VALUES (59, N'Soda thơm dưa lưới', 4, 35000, N'Soda thơm dưa lưới', N'soda-thom-dua-luoi.jpg', 1)
INSERT [dbo].[MonAn] ([MaMonAn], [TenMonAn], [MaLoai], [DonGia], [MoTa], [HinhAnh], [TrangThai]) VALUES (60, N'Nước gạo hoa anh đào', 4, 35000, N'Nước gạo hoa anh đào', N'nuoc-gao-hoa-anh-dao.jpg', 1)
SET IDENTITY_INSERT [dbo].[MonAn] OFF
GO
SET IDENTITY_INSERT [dbo].[NguyenVatLieu] ON 

INSERT [dbo].[NguyenVatLieu] ([MaNguyenLieu], [TenNguyenLieu], [DonViTinh], [SoLuongTon], [MoTa], [TrangThai]) VALUES (9, N'Bột trà sữa Original', N'kg', 10, N'Bột trà sữa Original', 1)
INSERT [dbo].[NguyenVatLieu] ([MaNguyenLieu], [TenNguyenLieu], [DonViTinh], [SoLuongTon], [MoTa], [TrangThai]) VALUES (10, N'Bột trà sữa matcha', N'kg', 10, N'Bột trà sữa hương matcha', 1)
INSERT [dbo].[NguyenVatLieu] ([MaNguyenLieu], [TenNguyenLieu], [DonViTinh], [SoLuongTon], [MoTa], [TrangThai]) VALUES (11, N'Bột trà đào', N'kg', 10, N'Bột trà hương đào', 1)
INSERT [dbo].[NguyenVatLieu] ([MaNguyenLieu], [TenNguyenLieu], [DonViTinh], [SoLuongTon], [MoTa], [TrangThai]) VALUES (12, N'Bột trà dâu', N'kg', 10, N'Bột trà hương dâu', 1)
INSERT [dbo].[NguyenVatLieu] ([MaNguyenLieu], [TenNguyenLieu], [DonViTinh], [SoLuongTon], [MoTa], [TrangThai]) VALUES (13, N'Bụp giấm', N'kg', 36, N'Bụp giấm tươi', 1)
INSERT [dbo].[NguyenVatLieu] ([MaNguyenLieu], [TenNguyenLieu], [DonViTinh], [SoLuongTon], [MoTa], [TrangThai]) VALUES (14, N'Nước đường', N'lít', 20, N'Nước đường pha chế', 1)
INSERT [dbo].[NguyenVatLieu] ([MaNguyenLieu], [TenNguyenLieu], [DonViTinh], [SoLuongTon], [MoTa], [TrangThai]) VALUES (15, N'Đào ngâm', N'kg', 15, N'Đào ngâm đóng hộp', 1)
INSERT [dbo].[NguyenVatLieu] ([MaNguyenLieu], [TenNguyenLieu], [DonViTinh], [SoLuongTon], [MoTa], [TrangThai]) VALUES (16, N'Nước cốt me Knorr', N'lít', 10, N'Nước cốt me thương hiệu Knorr', 1)
INSERT [dbo].[NguyenVatLieu] ([MaNguyenLieu], [TenNguyenLieu], [DonViTinh], [SoLuongTon], [MoTa], [TrangThai]) VALUES (17, N'Mứt dâu Vạn Thành', N'kg', 8, N'Mứt dâu thương hiệu Vạn Thành', 1)
INSERT [dbo].[NguyenVatLieu] ([MaNguyenLieu], [TenNguyenLieu], [DonViTinh], [SoLuongTon], [MoTa], [TrangThai]) VALUES (18, N'Mứt thơm Vạn Thành', N'kg', 8, N'Mứt thơm thương hiệu Vạn Thành', 1)
INSERT [dbo].[NguyenVatLieu] ([MaNguyenLieu], [TenNguyenLieu], [DonViTinh], [SoLuongTon], [MoTa], [TrangThai]) VALUES (19, N'Nụ hoa hồng khô', N'kg', 3, N'Nụ hoa hồng khô pha trà', 1)
INSERT [dbo].[NguyenVatLieu] ([MaNguyenLieu], [TenNguyenLieu], [DonViTinh], [SoLuongTon], [MoTa], [TrangThai]) VALUES (20, N'Đác rim thơm', N'kg', 5, N'Đác rim hương thơm', 1)
INSERT [dbo].[NguyenVatLieu] ([MaNguyenLieu], [TenNguyenLieu], [DonViTinh], [SoLuongTon], [MoTa], [TrangThai]) VALUES (21, N'Nước ép dưa lưới đông lạnh', N'lít', 10, N'Nước ép dưa lưới đông lạnh', 1)
INSERT [dbo].[NguyenVatLieu] ([MaNguyenLieu], [TenNguyenLieu], [DonViTinh], [SoLuongTon], [MoTa], [TrangThai]) VALUES (22, N'Nước ép thơm đông lạnh', N'lít', 10, N'Nước ép thơm đông lạnh', 1)
INSERT [dbo].[NguyenVatLieu] ([MaNguyenLieu], [TenNguyenLieu], [DonViTinh], [SoLuongTon], [MoTa], [TrangThai]) VALUES (23, N'Hạt chia', N'kg', 5, N'Hạt chia dinh dưỡng', 1)
INSERT [dbo].[NguyenVatLieu] ([MaNguyenLieu], [TenNguyenLieu], [DonViTinh], [SoLuongTon], [MoTa], [TrangThai]) VALUES (24, N'Nước gạo Hàn Quốc', N'lít', 15, N'Nước gạo kiểu Hàn Quốc', 1)
INSERT [dbo].[NguyenVatLieu] ([MaNguyenLieu], [TenNguyenLieu], [DonViTinh], [SoLuongTon], [MoTa], [TrangThai]) VALUES (25, N'BTP Syrup đường', N'lít', 20, N'Syrup đường pha chế', 1)
INSERT [dbo].[NguyenVatLieu] ([MaNguyenLieu], [TenNguyenLieu], [DonViTinh], [SoLuongTon], [MoTa], [TrangThai]) VALUES (26, N'Mứt dừa non Longbeach', N'kg', 8, N'Mứt dừa non thương hiệu Longbeach', 1)
INSERT [dbo].[NguyenVatLieu] ([MaNguyenLieu], [TenNguyenLieu], [DonViTinh], [SoLuongTon], [MoTa], [TrangThai]) VALUES (27, N'Chanh không hạt', N'kg', 10, N'Chanh tươi không hạt', 1)
INSERT [dbo].[NguyenVatLieu] ([MaNguyenLieu], [TenNguyenLieu], [DonViTinh], [SoLuongTon], [MoTa], [TrangThai]) VALUES (28, N'Lá dứa', N'kg', 18, N'Lá dứa tươi', 1)
INSERT [dbo].[NguyenVatLieu] ([MaNguyenLieu], [TenNguyenLieu], [DonViTinh], [SoLuongTon], [MoTa], [TrangThai]) VALUES (29, N'Củ cải trắng', N'kg', 15, N'Củ cải trắng tươi', 1)
INSERT [dbo].[NguyenVatLieu] ([MaNguyenLieu], [TenNguyenLieu], [DonViTinh], [SoLuongTon], [MoTa], [TrangThai]) VALUES (30, N'Ớt ba tri', N'kg', 5, N'Ớt giống ba tri', 1)
INSERT [dbo].[NguyenVatLieu] ([MaNguyenLieu], [TenNguyenLieu], [DonViTinh], [SoLuongTon], [MoTa], [TrangThai]) VALUES (31, N'Khoai tây', N'kg', 20, N'Khoai tây tươi', 1)
INSERT [dbo].[NguyenVatLieu] ([MaNguyenLieu], [TenNguyenLieu], [DonViTinh], [SoLuongTon], [MoTa], [TrangThai]) VALUES (32, N'Sả cây', N'kg', 8, N'Sả cây tươi', 1)
INSERT [dbo].[NguyenVatLieu] ([MaNguyenLieu], [TenNguyenLieu], [DonViTinh], [SoLuongTon], [MoTa], [TrangThai]) VALUES (33, N'Lá chanh', N'kg', 2, N'Lá chanh tươi', 1)
INSERT [dbo].[NguyenVatLieu] ([MaNguyenLieu], [TenNguyenLieu], [DonViTinh], [SoLuongTon], [MoTa], [TrangThai]) VALUES (34, N'Tắc', N'kg', 5, N'Tắc (quất) tươi', 1)
INSERT [dbo].[NguyenVatLieu] ([MaNguyenLieu], [TenNguyenLieu], [DonViTinh], [SoLuongTon], [MoTa], [TrangThai]) VALUES (35, N'Riềng', N'kg', 5, N'Riềng tươi', 1)
INSERT [dbo].[NguyenVatLieu] ([MaNguyenLieu], [TenNguyenLieu], [DonViTinh], [SoLuongTon], [MoTa], [TrangThai]) VALUES (36, N'Gừng', N'kg', 5, N'Gừng tươi', 1)
INSERT [dbo].[NguyenVatLieu] ([MaNguyenLieu], [TenNguyenLieu], [DonViTinh], [SoLuongTon], [MoTa], [TrangThai]) VALUES (37, N'Ngò gai', N'kg', 3, N'Ngò gai tươi', 1)
INSERT [dbo].[NguyenVatLieu] ([MaNguyenLieu], [TenNguyenLieu], [DonViTinh], [SoLuongTon], [MoTa], [TrangThai]) VALUES (38, N'Hành boaro', N'kg', 8, N'Hành boaro tươi', 1)
INSERT [dbo].[NguyenVatLieu] ([MaNguyenLieu], [TenNguyenLieu], [DonViTinh], [SoLuongTon], [MoTa], [TrangThai]) VALUES (39, N'Ớt hiểm giống batri', N'kg', 5, N'Ớt hiểm giống batri cay', 1)
INSERT [dbo].[NguyenVatLieu] ([MaNguyenLieu], [TenNguyenLieu], [DonViTinh], [SoLuongTon], [MoTa], [TrangThai]) VALUES (40, N'Nấm đông cô', N'kg', 8, N'Nấm đông cô tươi', 1)
INSERT [dbo].[NguyenVatLieu] ([MaNguyenLieu], [TenNguyenLieu], [DonViTinh], [SoLuongTon], [MoTa], [TrangThai]) VALUES (41, N'Bắp mỹ', N'kg', 15, N'Bắp mỹ ngọt', 1)
INSERT [dbo].[NguyenVatLieu] ([MaNguyenLieu], [TenNguyenLieu], [DonViTinh], [SoLuongTon], [MoTa], [TrangThai]) VALUES (42, N'Cà rốt', N'kg', 35, N'Cà rốt tươi', 1)
INSERT [dbo].[NguyenVatLieu] ([MaNguyenLieu], [TenNguyenLieu], [DonViTinh], [SoLuongTon], [MoTa], [TrangThai]) VALUES (43, N'Xà lách các loại', N'kg', 10, N'Xà lách mỹ, lô lô xanh', 1)
INSERT [dbo].[NguyenVatLieu] ([MaNguyenLieu], [TenNguyenLieu], [DonViTinh], [SoLuongTon], [MoTa], [TrangThai]) VALUES (44, N'Ớt chuông', N'kg', 8, N'Ớt chuông các màu', 1)
INSERT [dbo].[NguyenVatLieu] ([MaNguyenLieu], [TenNguyenLieu], [DonViTinh], [SoLuongTon], [MoTa], [TrangThai]) VALUES (45, N'Hành tây', N'kg', 12, N'Hành tây tươi', 1)
INSERT [dbo].[NguyenVatLieu] ([MaNguyenLieu], [TenNguyenLieu], [DonViTinh], [SoLuongTon], [MoTa], [TrangThai]) VALUES (46, N'Cà chua bi', N'kg', 10, N'Cà chua bi tươi', 1)
INSERT [dbo].[NguyenVatLieu] ([MaNguyenLieu], [TenNguyenLieu], [DonViTinh], [SoLuongTon], [MoTa], [TrangThai]) VALUES (47, N'Bắp hạt', N'kg', 55, N'Bắp hạt đông lạnh', 1)
INSERT [dbo].[NguyenVatLieu] ([MaNguyenLieu], [TenNguyenLieu], [DonViTinh], [SoLuongTon], [MoTa], [TrangThai]) VALUES (48, N'Tỏi xay', N'kg', 5, N'Tỏi xay sẵn', 1)
INSERT [dbo].[NguyenVatLieu] ([MaNguyenLieu], [TenNguyenLieu], [DonViTinh], [SoLuongTon], [MoTa], [TrangThai]) VALUES (49, N'Cải thảo', N'kg', 15, N'Cải thảo tươi', 1)
INSERT [dbo].[NguyenVatLieu] ([MaNguyenLieu], [TenNguyenLieu], [DonViTinh], [SoLuongTon], [MoTa], [TrangThai]) VALUES (50, N'Cải thìa', N'kg', 10, N'Cải thìa tươi', 1)
INSERT [dbo].[NguyenVatLieu] ([MaNguyenLieu], [TenNguyenLieu], [DonViTinh], [SoLuongTon], [MoTa], [TrangThai]) VALUES (51, N'Nấm kim châm', N'kg', 8, N'Nấm kim châm tươi', 1)
INSERT [dbo].[NguyenVatLieu] ([MaNguyenLieu], [TenNguyenLieu], [DonViTinh], [SoLuongTon], [MoTa], [TrangThai]) VALUES (52, N'Bắp cải tím', N'kg', 10, N'Bắp cải tím tươi', 1)
INSERT [dbo].[NguyenVatLieu] ([MaNguyenLieu], [TenNguyenLieu], [DonViTinh], [SoLuongTon], [MoTa], [TrangThai]) VALUES (53, N'Bắp cải trắng', N'kg', 32, N'Bắp cải trắng tươi', 1)
INSERT [dbo].[NguyenVatLieu] ([MaNguyenLieu], [TenNguyenLieu], [DonViTinh], [SoLuongTon], [MoTa], [TrangThai]) VALUES (54, N'Trứng gà (nguyên vỏ)', N'quả', 200, N'Trứng gà nguyên vỏ tươi', 1)
INSERT [dbo].[NguyenVatLieu] ([MaNguyenLieu], [TenNguyenLieu], [DonViTinh], [SoLuongTon], [MoTa], [TrangThai]) VALUES (55, N'Tôm', N'kg', 10, N'Tôm tươi', 1)
INSERT [dbo].[NguyenVatLieu] ([MaNguyenLieu], [TenNguyenLieu], [DonViTinh], [SoLuongTon], [MoTa], [TrangThai]) VALUES (56, N'Cá viên', N'kg', 10, N'Cá viên đông lạnh', 1)
INSERT [dbo].[NguyenVatLieu] ([MaNguyenLieu], [TenNguyenLieu], [DonViTinh], [SoLuongTon], [MoTa], [TrangThai]) VALUES (57, N'Đùi gà fillet ướp', N'kg', 15, N'Đùi gà fillet đã ướp gia vị', 1)
INSERT [dbo].[NguyenVatLieu] ([MaNguyenLieu], [TenNguyenLieu], [DonViTinh], [SoLuongTon], [MoTa], [TrangThai]) VALUES (58, N'Mực cắt khoanh', N'kg', 10, N'Mực tươi cắt khoanh', 1)
INSERT [dbo].[NguyenVatLieu] ([MaNguyenLieu], [TenNguyenLieu], [DonViTinh], [SoLuongTon], [MoTa], [TrangThai]) VALUES (59, N'Cá basa fillet', N'kg', 15, N'Cá basa fillet tươi', 1)
INSERT [dbo].[NguyenVatLieu] ([MaNguyenLieu], [TenNguyenLieu], [DonViTinh], [SoLuongTon], [MoTa], [TrangThai]) VALUES (60, N'Nạc vai heo', N'kg', 15, N'Nạc vai heo tươi', 1)
INSERT [dbo].[NguyenVatLieu] ([MaNguyenLieu], [TenNguyenLieu], [DonViTinh], [SoLuongTon], [MoTa], [TrangThai]) VALUES (61, N'Ba chỉ bò trải thẳng', N'kg', 32, N'Ba chỉ bò trải thẳng tươi', 1)
INSERT [dbo].[NguyenVatLieu] ([MaNguyenLieu], [TenNguyenLieu], [DonViTinh], [SoLuongTon], [MoTa], [TrangThai]) VALUES (62, N'Dồi sụn', N'kg', 10, N'Dồi sụn tươi', 1)
INSERT [dbo].[NguyenVatLieu] ([MaNguyenLieu], [TenNguyenLieu], [DonViTinh], [SoLuongTon], [MoTa], [TrangThai]) VALUES (63, N'Chả cá Hàn Quốc', N'kg', 15, N'Chả cá kiểu Hàn Quốc', 1)
INSERT [dbo].[NguyenVatLieu] ([MaNguyenLieu], [TenNguyenLieu], [DonViTinh], [SoLuongTon], [MoTa], [TrangThai]) VALUES (64, N'Nước dùng tương hàn', N'lít', 20, N'Nước dùng tương kiểu Hàn', 1)
INSERT [dbo].[NguyenVatLieu] ([MaNguyenLieu], [TenNguyenLieu], [DonViTinh], [SoLuongTon], [MoTa], [TrangThai]) VALUES (65, N'Nước dùng bánh cá hầm', N'lít', 20, N'Nước dùng bánh cá hầm', 1)
INSERT [dbo].[NguyenVatLieu] ([MaNguyenLieu], [TenNguyenLieu], [DonViTinh], [SoLuongTon], [MoTa], [TrangThai]) VALUES (66, N'Phô mai viên', N'kg', 10, N'Phô mai viên đông lạnh', 1)
INSERT [dbo].[NguyenVatLieu] ([MaNguyenLieu], [TenNguyenLieu], [DonViTinh], [SoLuongTon], [MoTa], [TrangThai]) VALUES (67, N'Bánh bạch tuộc (takoyaki)', N'kg', 10, N'Bánh bạch tuộc takoyaki đông lạnh', 1)
INSERT [dbo].[NguyenVatLieu] ([MaNguyenLieu], [TenNguyenLieu], [DonViTinh], [SoLuongTon], [MoTa], [TrangThai]) VALUES (68, N'Mandu', N'kg', 10, N'Bánh Mandu Hàn Quốc', 1)
INSERT [dbo].[NguyenVatLieu] ([MaNguyenLieu], [TenNguyenLieu], [DonViTinh], [SoLuongTon], [MoTa], [TrangThai]) VALUES (69, N'Xiên bánh cá hầm', N'kg', 10, N'Xiên bánh cá hầm Hàn Quốc', 1)
INSERT [dbo].[NguyenVatLieu] ([MaNguyenLieu], [TenNguyenLieu], [DonViTinh], [SoLuongTon], [MoTa], [TrangThai]) VALUES (70, N'Ba chỉ heo cuộn', N'kg', 10, N'Ba chỉ heo cuộn sẵn', 1)
INSERT [dbo].[NguyenVatLieu] ([MaNguyenLieu], [TenNguyenLieu], [DonViTinh], [SoLuongTon], [MoTa], [TrangThai]) VALUES (71, N'Khoai tây chiên', N'kg', 15, N'Khoai tây đông lạnh để chiên', 1)
INSERT [dbo].[NguyenVatLieu] ([MaNguyenLieu], [TenNguyenLieu], [DonViTinh], [SoLuongTon], [MoTa], [TrangThai]) VALUES (72, N'Kimbap chiên', N'kg', 10, N'Kimbap đông lạnh để chiên', 1)
INSERT [dbo].[NguyenVatLieu] ([MaNguyenLieu], [TenNguyenLieu], [DonViTinh], [SoLuongTon], [MoTa], [TrangThai]) VALUES (73, N'Bánh cá cuộn rong biển', N'kg', 10, N'Bánh cá cuộn rong biển đông lạnh', 1)
INSERT [dbo].[NguyenVatLieu] ([MaNguyenLieu], [TenNguyenLieu], [DonViTinh], [SoLuongTon], [MoTa], [TrangThai]) VALUES (74, N'Xúc xích', N'kg', 15, N'Xúc xích tươi', 1)
INSERT [dbo].[NguyenVatLieu] ([MaNguyenLieu], [TenNguyenLieu], [DonViTinh], [SoLuongTon], [MoTa], [TrangThai]) VALUES (75, N'Rong biển xốt', N'kg', 5, N'Rong biển đã ướp xốt', 1)
INSERT [dbo].[NguyenVatLieu] ([MaNguyenLieu], [TenNguyenLieu], [DonViTinh], [SoLuongTon], [MoTa], [TrangThai]) VALUES (76, N'Kim chi cải thảo', N'kg', 10, N'Kim chi cải thảo truyền thống', 1)
INSERT [dbo].[NguyenVatLieu] ([MaNguyenLieu], [TenNguyenLieu], [DonViTinh], [SoLuongTon], [MoTa], [TrangThai]) VALUES (77, N'Nước cốt kim chi', N'lít', 10, N'Nước cốt kim chi cô đặc', 1)
INSERT [dbo].[NguyenVatLieu] ([MaNguyenLieu], [TenNguyenLieu], [DonViTinh], [SoLuongTon], [MoTa], [TrangThai]) VALUES (78, N'Nấm mèo xào', N'kg', 8, N'Nấm mèo đã xào sơ', 1)
INSERT [dbo].[NguyenVatLieu] ([MaNguyenLieu], [TenNguyenLieu], [DonViTinh], [SoLuongTon], [MoTa], [TrangThai]) VALUES (79, N'Cải vàng', N'kg', 10, N'Cải vàng tươi', 1)
INSERT [dbo].[NguyenVatLieu] ([MaNguyenLieu], [TenNguyenLieu], [DonViTinh], [SoLuongTon], [MoTa], [TrangThai]) VALUES (80, N'Trứng ngâm tương', N'quả', 100, N'Trứng đã ngâm trong nước tương', 1)
INSERT [dbo].[NguyenVatLieu] ([MaNguyenLieu], [TenNguyenLieu], [DonViTinh], [SoLuongTon], [MoTa], [TrangThai]) VALUES (81, N'Xốt kem trắng', N'lít', 10, N'Xốt kem trắng đa dụng', 1)
INSERT [dbo].[NguyenVatLieu] ([MaNguyenLieu], [TenNguyenLieu], [DonViTinh], [SoLuongTon], [MoTa], [TrangThai]) VALUES (82, N'Xốt kem cam', N'lít', 10, N'Xốt kem hương cam', 1)
INSERT [dbo].[NguyenVatLieu] ([MaNguyenLieu], [TenNguyenLieu], [DonViTinh], [SoLuongTon], [MoTa], [TrangThai]) VALUES (83, N'Xốt tương đen', N'lít', 10, N'Xốt tương đen Hàn Quốc', 1)
INSERT [dbo].[NguyenVatLieu] ([MaNguyenLieu], [TenNguyenLieu], [DonViTinh], [SoLuongTon], [MoTa], [TrangThai]) VALUES (84, N'Xốt bánh gạo cay', N'lít', 10, N'Xốt bánh gạo cay Hàn Quốc', 1)
INSERT [dbo].[NguyenVatLieu] ([MaNguyenLieu], [TenNguyenLieu], [DonViTinh], [SoLuongTon], [MoTa], [TrangThai]) VALUES (85, N'Xốt cơm trộn', N'lít', 10, N'Xốt cơm trộn Hàn Quốc', 1)
INSERT [dbo].[NguyenVatLieu] ([MaNguyenLieu], [TenNguyenLieu], [DonViTinh], [SoLuongTon], [MoTa], [TrangThai]) VALUES (86, N'Nước nền Tokbokki', N'lít', 15, N'Nước nền làm Tokbokki', 1)
INSERT [dbo].[NguyenVatLieu] ([MaNguyenLieu], [TenNguyenLieu], [DonViTinh], [SoLuongTon], [MoTa], [TrangThai]) VALUES (87, N'Xốt kimbap', N'lít', 10, N'Xốt dùng cho kimbap', 1)
INSERT [dbo].[NguyenVatLieu] ([MaNguyenLieu], [TenNguyenLieu], [DonViTinh], [SoLuongTon], [MoTa], [TrangThai]) VALUES (88, N'Xốt bánh kẹp Hàn Quốc', N'lít', 10, N'Xốt bánh kẹp kiểu Hàn Quốc', 1)
INSERT [dbo].[NguyenVatLieu] ([MaNguyenLieu], [TenNguyenLieu], [DonViTinh], [SoLuongTon], [MoTa], [TrangThai]) VALUES (89, N'Xốt salad mè', N'lít', 10, N'Xốt salad mè rang', 1)
INSERT [dbo].[NguyenVatLieu] ([MaNguyenLieu], [TenNguyenLieu], [DonViTinh], [SoLuongTon], [MoTa], [TrangThai]) VALUES (90, N'Nước lẩu Tokbokki', N'lít', 15, N'Nước lẩu Tokbokki Hàn Quốc', 1)
INSERT [dbo].[NguyenVatLieu] ([MaNguyenLieu], [TenNguyenLieu], [DonViTinh], [SoLuongTon], [MoTa], [TrangThai]) VALUES (91, N'Xốt ớt độ', N'lít', 8, N'Xốt ớt độ cay', 1)
INSERT [dbo].[NguyenVatLieu] ([MaNguyenLieu], [TenNguyenLieu], [DonViTinh], [SoLuongTon], [MoTa], [TrangThai]) VALUES (92, N'Bánh gạo phô mai Green Food', N'kg', 50, N'Bánh gạo phô mai thương hiệu Green Food', 1)
INSERT [dbo].[NguyenVatLieu] ([MaNguyenLieu], [TenNguyenLieu], [DonViTinh], [SoLuongTon], [MoTa], [TrangThai]) VALUES (93, N'Mì Chinnoo', N'kg', 15, N'Mì Chinnoo Hàn Quốc', 1)
INSERT [dbo].[NguyenVatLieu] ([MaNguyenLieu], [TenNguyenLieu], [DonViTinh], [SoLuongTon], [MoTa], [TrangThai]) VALUES (94, N'Bánh gạo Hàn Quốc Ofood', N'kg', 15, N'Bánh gạo Hàn Quốc thương hiệu Ofood', 1)
INSERT [dbo].[NguyenVatLieu] ([MaNguyenLieu], [TenNguyenLieu], [DonViTinh], [SoLuongTon], [MoTa], [TrangThai]) VALUES (95, N'Phô mai sợi', N'kg', 10, N'Phô mai sợi đông lạnh', 1)
INSERT [dbo].[NguyenVatLieu] ([MaNguyenLieu], [TenNguyenLieu], [DonViTinh], [SoLuongTon], [MoTa], [TrangThai]) VALUES (96, N'Xốt Mayonnaise', N'lít', 10, N'Xốt Mayonnaise', 1)
INSERT [dbo].[NguyenVatLieu] ([MaNguyenLieu], [TenNguyenLieu], [DonViTinh], [SoLuongTon], [MoTa], [TrangThai]) VALUES (97, N'Xốt tương đen Thành Lợi', N'lít', 10, N'Xốt tương đen thương hiệu Thành Lợi', 1)
INSERT [dbo].[NguyenVatLieu] ([MaNguyenLieu], [TenNguyenLieu], [DonViTinh], [SoLuongTon], [MoTa], [TrangThai]) VALUES (98, N'Dầu hào', N'lít', 8, N'Dầu hào đậm đặc', 1)
INSERT [dbo].[NguyenVatLieu] ([MaNguyenLieu], [TenNguyenLieu], [DonViTinh], [SoLuongTon], [MoTa], [TrangThai]) VALUES (99, N'Xốt ớt xanh', N'lít', 8, N'Xốt ớt xanh cay', 1)
INSERT [dbo].[NguyenVatLieu] ([MaNguyenLieu], [TenNguyenLieu], [DonViTinh], [SoLuongTon], [MoTa], [TrangThai]) VALUES (100, N'Xốt chấm bò', N'lít', 8, N'Xốt chấm thịt bò', 1)
INSERT [dbo].[NguyenVatLieu] ([MaNguyenLieu], [TenNguyenLieu], [DonViTinh], [SoLuongTon], [MoTa], [TrangThai]) VALUES (101, N'Tỏi phi', N'kg', 5, N'Tỏi phi sẵn', 1)
INSERT [dbo].[NguyenVatLieu] ([MaNguyenLieu], [TenNguyenLieu], [DonViTinh], [SoLuongTon], [MoTa], [TrangThai]) VALUES (102, N'Tương ớt Sasin', N'lít', 10, N'Tương ớt thương hiệu Sasin', 1)
INSERT [dbo].[NguyenVatLieu] ([MaNguyenLieu], [TenNguyenLieu], [DonViTinh], [SoLuongTon], [MoTa], [TrangThai]) VALUES (103, N'Tương cà', N'lít', 10, N'Tương cà đa dụng', 1)
INSERT [dbo].[NguyenVatLieu] ([MaNguyenLieu], [TenNguyenLieu], [DonViTinh], [SoLuongTon], [MoTa], [TrangThai]) VALUES (104, N'Bánh mì sandwich hồng', N'gói', 30, N'Bánh mì sandwich vỏ hồng', 1)
INSERT [dbo].[NguyenVatLieu] ([MaNguyenLieu], [TenNguyenLieu], [DonViTinh], [SoLuongTon], [MoTa], [TrangThai]) VALUES (105, N'Dầu mè', N'lít', 8, N'Dầu mè nguyên chất', 1)
INSERT [dbo].[NguyenVatLieu] ([MaNguyenLieu], [TenNguyenLieu], [DonViTinh], [SoLuongTon], [MoTa], [TrangThai]) VALUES (106, N'Nước tương Magie', N'lít', 10, N'Nước tương thương hiệu Magie', 1)
INSERT [dbo].[NguyenVatLieu] ([MaNguyenLieu], [TenNguyenLieu], [DonViTinh], [SoLuongTon], [MoTa], [TrangThai]) VALUES (107, N'Trân châu 3Q Caramel', N'kg', 10, N'Trân châu 3Q hương caramel', 1)
GO
INSERT [dbo].[NguyenVatLieu] ([MaNguyenLieu], [TenNguyenLieu], [DonViTinh], [SoLuongTon], [MoTa], [TrangThai]) VALUES (108, N'Hạt màu điều', N'kg', 5, N'Hạt màu điều trang trí', 1)
INSERT [dbo].[NguyenVatLieu] ([MaNguyenLieu], [TenNguyenLieu], [DonViTinh], [SoLuongTon], [MoTa], [TrangThai]) VALUES (109, N'Hạt mè trắng', N'kg', 5, N'Hạt mè trắng', 1)
INSERT [dbo].[NguyenVatLieu] ([MaNguyenLieu], [TenNguyenLieu], [DonViTinh], [SoLuongTon], [MoTa], [TrangThai]) VALUES (110, N'Bột ớt Hea Cham Mat', N'kg', 5, N'Bột ớt thương hiệu Hea Cham Mat', 1)
INSERT [dbo].[NguyenVatLieu] ([MaNguyenLieu], [TenNguyenLieu], [DonViTinh], [SoLuongTon], [MoTa], [TrangThai]) VALUES (111, N'Hạt nêm vị bò Dasida', N'kg', 10, N'Hạt nêm vị bò thương hiệu Dasida', 1)
INSERT [dbo].[NguyenVatLieu] ([MaNguyenLieu], [TenNguyenLieu], [DonViTinh], [SoLuongTon], [MoTa], [TrangThai]) VALUES (112, N'Cơm trắng', N'kg', 50, N'Cơm trắng nấu sẵn', 1)
INSERT [dbo].[NguyenVatLieu] ([MaNguyenLieu], [TenNguyenLieu], [DonViTinh], [SoLuongTon], [MoTa], [TrangThai]) VALUES (113, N'Nước lèo kim chi', N'lít', 20, N'Nước lèo hương vị kim chi', 1)
INSERT [dbo].[NguyenVatLieu] ([MaNguyenLieu], [TenNguyenLieu], [DonViTinh], [SoLuongTon], [MoTa], [TrangThai]) VALUES (114, N'Nước lèo soyum', N'lít', 20, N'Nước lèo hương vị soyum', 1)
INSERT [dbo].[NguyenVatLieu] ([MaNguyenLieu], [TenNguyenLieu], [DonViTinh], [SoLuongTon], [MoTa], [TrangThai]) VALUES (115, N'Nước lèo sincay', N'lít', 20, N'Nước lèo hương vị sincay', 1)
SET IDENTITY_INSERT [dbo].[NguyenVatLieu] OFF
GO
SET IDENTITY_INSERT [dbo].[NhanVien] ON 

INSERT [dbo].[NhanVien] ([MaNhanVien], [HoTen], [GioiTinh], [NgaySinh], [DiaChi], [SDT], [Email], [NgayVaoLam], [TrangThai], [MaTaiKhoan]) VALUES (1, N'Nguyễn Văn Admin', N'Nam', CAST(N'1990-01-01' AS Date), N'123 Nguyễn Văn Linh, Quận 7, TP HCM', N'0901234567', N'admin@example.com', CAST(N'2023-01-01' AS Date), 1, 1)
INSERT [dbo].[NhanVien] ([MaNhanVien], [HoTen], [GioiTinh], [NgaySinh], [DiaChi], [SDT], [Email], [NgayVaoLam], [TrangThai], [MaTaiKhoan]) VALUES (2, N'Trần Thị Nhân Viên', N'Nữ', CAST(N'1995-05-15' AS Date), N'456 Lê Văn Việt, Quận 9, TP HCM', N'0912345678', N'nhanvien1@example.com', CAST(N'2023-02-01' AS Date), 1, 2)
INSERT [dbo].[NhanVien] ([MaNhanVien], [HoTen], [GioiTinh], [NgaySinh], [DiaChi], [SDT], [Email], [NgayVaoLam], [TrangThai], [MaTaiKhoan]) VALUES (3, N'Lê Văn Nhân Viên', N'Nam', CAST(N'1993-07-20' AS Date), N'789 Nguyễn Thị Thập, Quận 7, TP HCM', N'0923456789', N'nhanvien2@example.com', CAST(N'2023-03-01' AS Date), 1, 3)
INSERT [dbo].[NhanVien] ([MaNhanVien], [HoTen], [GioiTinh], [NgaySinh], [DiaChi], [SDT], [Email], [NgayVaoLam], [TrangThai], [MaTaiKhoan]) VALUES (7, N'phuoc thinh ', N'NAM', CAST(N'2025-05-09' AS Date), N'q7', N'113', N'thinh@', CAST(N'2025-05-09' AS Date), 1, 17)
INSERT [dbo].[NhanVien] ([MaNhanVien], [HoTen], [GioiTinh], [NgaySinh], [DiaChi], [SDT], [Email], [NgayVaoLam], [TrangThai], [MaTaiKhoan]) VALUES (8, N'phuoc thinh ', N'NAM', CAST(N'2025-05-09' AS Date), N'q7', N'113', N'thinh@', CAST(N'2025-05-09' AS Date), 1, 18)
INSERT [dbo].[NhanVien] ([MaNhanVien], [HoTen], [GioiTinh], [NgaySinh], [DiaChi], [SDT], [Email], [NgayVaoLam], [TrangThai], [MaTaiKhoan]) VALUES (9, N'phuoc thinh ', N'NAM', CAST(N'2025-05-09' AS Date), N'q7', N'113', N'thinh@', CAST(N'2025-05-09' AS Date), 1, 19)
INSERT [dbo].[NhanVien] ([MaNhanVien], [HoTen], [GioiTinh], [NgaySinh], [DiaChi], [SDT], [Email], [NgayVaoLam], [TrangThai], [MaTaiKhoan]) VALUES (12, N'phuoc thinh ', N'NAM', CAST(N'2025-05-09' AS Date), N'q7', N'113', N'thinh@', CAST(N'2025-05-09' AS Date), 1, 28)
INSERT [dbo].[NhanVien] ([MaNhanVien], [HoTen], [GioiTinh], [NgaySinh], [DiaChi], [SDT], [Email], [NgayVaoLam], [TrangThai], [MaTaiKhoan]) VALUES (14, N'Lê Nguyễn Phước Thịnh', N'NAM', CAST(N'2025-05-13' AS Date), N'Quận 7 ', N'0123445565', N'thinh@gmail.com', CAST(N'2025-05-13' AS Date), 1, 1009)
SET IDENTITY_INSERT [dbo].[NhanVien] OFF
GO
SET IDENTITY_INSERT [dbo].[PhieuNhapKho] ON 

INSERT [dbo].[PhieuNhapKho] ([MaPhieuNhap], [MaNhanVien], [NgayNhap], [NhaCungCap], [TongTien], [GhiChu], [TrangThai]) VALUES (2, 3, CAST(N'2025-04-24T17:52:27.440' AS DateTime), N'milo', 606000, N'Thanks', 1)
INSERT [dbo].[PhieuNhapKho] ([MaPhieuNhap], [MaNhanVien], [NgayNhap], [NhaCungCap], [TongTien], [GhiChu], [TrangThai]) VALUES (3, 2, CAST(N'2025-04-24T17:33:07.143' AS DateTime), NULL, NULL, NULL, 1)
INSERT [dbo].[PhieuNhapKho] ([MaPhieuNhap], [MaNhanVien], [NgayNhap], [NhaCungCap], [TongTien], [GhiChu], [TrangThai]) VALUES (4, 2, CAST(N'2025-04-24T17:51:53.263' AS DateTime), N'sdasdsa', NULL, N'ádasdsadsa', 1)
INSERT [dbo].[PhieuNhapKho] ([MaPhieuNhap], [MaNhanVien], [NgayNhap], [NhaCungCap], [TongTien], [GhiChu], [TrangThai]) VALUES (5, 1, CAST(N'2025-05-07T13:34:41.187' AS DateTime), N'sdasdsa', 40000, N'ádasdsadsa', 1)
INSERT [dbo].[PhieuNhapKho] ([MaPhieuNhap], [MaNhanVien], [NgayNhap], [NhaCungCap], [TongTien], [GhiChu], [TrangThai]) VALUES (6, 2, CAST(N'2025-05-07T14:27:51.963' AS DateTime), N'bánh mì', 32000, N'rau xanh', 1)
SET IDENTITY_INSERT [dbo].[PhieuNhapKho] OFF
GO
SET IDENTITY_INSERT [dbo].[TaiKhoan] ON 

INSERT [dbo].[TaiKhoan] ([MaTaiKhoan], [TenDangNhap], [TenHienThi], [MatKhau], [MaLoai], [TrangThai]) VALUES (1, N'admin', N'Quản trị viên', N'123456', 1, 1)
INSERT [dbo].[TaiKhoan] ([MaTaiKhoan], [TenDangNhap], [TenHienThi], [MatKhau], [MaLoai], [TrangThai]) VALUES (2, N'nhanvien1', N'Nhân viên 1', N'123456', 2, 1)
INSERT [dbo].[TaiKhoan] ([MaTaiKhoan], [TenDangNhap], [TenHienThi], [MatKhau], [MaLoai], [TrangThai]) VALUES (3, N'nhanvien4', N'Nhân viên 2', N'123456', 2, 1)
INSERT [dbo].[TaiKhoan] ([MaTaiKhoan], [TenDangNhap], [TenHienThi], [MatKhau], [MaLoai], [TrangThai]) VALUES (17, N'Thinh', N'Thinh', N'123', 2, 1)
INSERT [dbo].[TaiKhoan] ([MaTaiKhoan], [TenDangNhap], [TenHienThi], [MatKhau], [MaLoai], [TrangThai]) VALUES (18, N'quoc', N'Thinh', N'123', 2, 1)
INSERT [dbo].[TaiKhoan] ([MaTaiKhoan], [TenDangNhap], [TenHienThi], [MatKhau], [MaLoai], [TrangThai]) VALUES (19, N'trung', N'Thinh', N'123', 2, 1)
INSERT [dbo].[TaiKhoan] ([MaTaiKhoan], [TenDangNhap], [TenHienThi], [MatKhau], [MaLoai], [TrangThai]) VALUES (28, N'tinhssss', N'Thinh', N'123', 2, 1)
INSERT [dbo].[TaiKhoan] ([MaTaiKhoan], [TenDangNhap], [TenHienThi], [MatKhau], [MaLoai], [TrangThai]) VALUES (1009, N'test', N'Thịnh Lee', N'123', 2, 1)
SET IDENTITY_INSERT [dbo].[TaiKhoan] OFF
GO
SET IDENTITY_INSERT [dbo].[ThanhToan] ON 

INSERT [dbo].[ThanhToan] ([MaThanhToan], [MaDonHang], [MaNhanVien], [MaHinhThuc], [SoTien], [NgayThanhToan], [GhiChu]) VALUES (1, 1, 1, 1, 238850, CAST(N'2025-05-11T19:25:16.890' AS DateTime), N'Thanh toán thành công')
INSERT [dbo].[ThanhToan] ([MaThanhToan], [MaDonHang], [MaNhanVien], [MaHinhThuc], [SoTien], [NgayThanhToan], [GhiChu]) VALUES (2, 2, 1, 3, 349000, CAST(N'2025-05-11T19:26:49.450' AS DateTime), N'Thanh toán thành công')
INSERT [dbo].[ThanhToan] ([MaThanhToan], [MaDonHang], [MaNhanVien], [MaHinhThuc], [SoTien], [NgayThanhToan], [GhiChu]) VALUES (3, 3, 1, 3, 97000, CAST(N'2025-05-11T19:31:42.387' AS DateTime), N'Thanh toán thành công')
INSERT [dbo].[ThanhToan] ([MaThanhToan], [MaDonHang], [MaNhanVien], [MaHinhThuc], [SoTien], [NgayThanhToan], [GhiChu]) VALUES (4, 4, 1, 3, 103000, CAST(N'2025-05-11T20:15:04.070' AS DateTime), N'Thanh toán thành công')
INSERT [dbo].[ThanhToan] ([MaThanhToan], [MaDonHang], [MaNhanVien], [MaHinhThuc], [SoTien], [NgayThanhToan], [GhiChu]) VALUES (9, 11, 1, 3, 84000, CAST(N'2025-05-11T20:33:17.743' AS DateTime), N'Thanh toán thành công')
INSERT [dbo].[ThanhToan] ([MaThanhToan], [MaDonHang], [MaNhanVien], [MaHinhThuc], [SoTien], [NgayThanhToan], [GhiChu]) VALUES (10, 12, 1, 3, 152000, CAST(N'2025-05-11T21:42:07.940' AS DateTime), N'Thanh toán thành công')
INSERT [dbo].[ThanhToan] ([MaThanhToan], [MaDonHang], [MaNhanVien], [MaHinhThuc], [SoTien], [NgayThanhToan], [GhiChu]) VALUES (11, 17, 1, 3, 113000, CAST(N'2025-05-12T00:00:15.723' AS DateTime), N'Thanh toán thành công')
INSERT [dbo].[ThanhToan] ([MaThanhToan], [MaDonHang], [MaNhanVien], [MaHinhThuc], [SoTien], [NgayThanhToan], [GhiChu]) VALUES (12, 18, 1, 3, 119000, CAST(N'2025-05-12T00:59:22.980' AS DateTime), N'Thanh toán thành công')
INSERT [dbo].[ThanhToan] ([MaThanhToan], [MaDonHang], [MaNhanVien], [MaHinhThuc], [SoTien], [NgayThanhToan], [GhiChu]) VALUES (13, 20, 1, 3, 142000, CAST(N'2025-05-12T01:20:59.983' AS DateTime), N'Thanh toán thành công')
INSERT [dbo].[ThanhToan] ([MaThanhToan], [MaDonHang], [MaNhanVien], [MaHinhThuc], [SoTien], [NgayThanhToan], [GhiChu]) VALUES (14, 21, 1, 3, 113000, CAST(N'2025-05-12T12:18:22.383' AS DateTime), N'Thanh toán thành công')
INSERT [dbo].[ThanhToan] ([MaThanhToan], [MaDonHang], [MaNhanVien], [MaHinhThuc], [SoTien], [NgayThanhToan], [GhiChu]) VALUES (15, 22, 1, 3, 96050, CAST(N'2025-05-12T21:10:01.333' AS DateTime), N'Thanh toán thành công')
INSERT [dbo].[ThanhToan] ([MaThanhToan], [MaDonHang], [MaNhanVien], [MaHinhThuc], [SoTien], [NgayThanhToan], [GhiChu]) VALUES (16, 24, 1, 2, 221000, CAST(N'2025-05-13T21:22:38.243' AS DateTime), N'Thanh toán thành công')
SET IDENTITY_INSERT [dbo].[ThanhToan] OFF
GO
INSERT [dbo].[ThongTinDinhDuong] ([MaMonAn], [Calo], [Protein], [Carbohydrate], [Fat], [Fiber], [Duong], [Natri], [ThanhPhanDiUng], [PhanLoai], [GhiChu]) VALUES (1, 220, 6, 30, 5, 1.2, 2.5, 350, N'Trứng, Rong biển', N'Khai vị', N'Nhẹ và dễ tiêu')
INSERT [dbo].[ThongTinDinhDuong] ([MaMonAn], [Calo], [Protein], [Carbohydrate], [Fat], [Fiber], [Duong], [Natri], [ThanhPhanDiUng], [PhanLoai], [GhiChu]) VALUES (2, 310, 19, 15, 17, 0.8, 0.5, 580, N'Sụn gà', N'Khai vị', N'Nhiều đạm')
INSERT [dbo].[ThongTinDinhDuong] ([MaMonAn], [Calo], [Protein], [Carbohydrate], [Fat], [Fiber], [Duong], [Natri], [ThanhPhanDiUng], [PhanLoai], [GhiChu]) VALUES (3, 250, 8, 28, 10, 0.9, 1.2, 450, N'Hải sản', N'Khai vị', N'Bạch tuộc – không phù hợp dị ứng hải sản')
INSERT [dbo].[ThongTinDinhDuong] ([MaMonAn], [Calo], [Protein], [Carbohydrate], [Fat], [Fiber], [Duong], [Natri], [ThanhPhanDiUng], [PhanLoai], [GhiChu]) VALUES (4, 270, 6, 20, 20, 0.3, 1, 400, N'Sữa', N'Khai vị', N'Cao béo')
INSERT [dbo].[ThongTinDinhDuong] ([MaMonAn], [Calo], [Protein], [Carbohydrate], [Fat], [Fiber], [Duong], [Natri], [ThanhPhanDiUng], [PhanLoai], [GhiChu]) VALUES (5, 275, 7, 22, 20, 0.3, 1.2, 410, N'Sữa', N'Khai vị', N'Phô mai rất béo')
INSERT [dbo].[ThongTinDinhDuong] ([MaMonAn], [Calo], [Protein], [Carbohydrate], [Fat], [Fiber], [Duong], [Natri], [ThanhPhanDiUng], [PhanLoai], [GhiChu]) VALUES (6, 260, 9, 24, 16, 0.4, 1.3, 430, NULL, N'Khai vị', N'')
INSERT [dbo].[ThongTinDinhDuong] ([MaMonAn], [Calo], [Protein], [Carbohydrate], [Fat], [Fiber], [Duong], [Natri], [ThanhPhanDiUng], [PhanLoai], [GhiChu]) VALUES (7, 300, 13, 18, 22, 0.5, 1, 470, NULL, N'Khai vị', N'')
INSERT [dbo].[ThongTinDinhDuong] ([MaMonAn], [Calo], [Protein], [Carbohydrate], [Fat], [Fiber], [Duong], [Natri], [ThanhPhanDiUng], [PhanLoai], [GhiChu]) VALUES (8, 290, 12, 22, 18, 0.6, 1.1, 440, N'Cua, Phô mai', N'Khai vị', N'Không phù hợp dị ứng hải sản')
INSERT [dbo].[ThongTinDinhDuong] ([MaMonAn], [Calo], [Protein], [Carbohydrate], [Fat], [Fiber], [Duong], [Natri], [ThanhPhanDiUng], [PhanLoai], [GhiChu]) VALUES (9, 180, 3, 10, 10, 2.7, 1.5, 250, NULL, N'Khai vị', N'Rau xanh tốt cho tiêu hoá')
INSERT [dbo].[ThongTinDinhDuong] ([MaMonAn], [Calo], [Protein], [Carbohydrate], [Fat], [Fiber], [Duong], [Natri], [ThanhPhanDiUng], [PhanLoai], [GhiChu]) VALUES (10, 210, 6, 24, 8, 1.2, 1, 300, N'Cá', N'Khai vị', N'')
INSERT [dbo].[ThongTinDinhDuong] ([MaMonAn], [Calo], [Protein], [Carbohydrate], [Fat], [Fiber], [Duong], [Natri], [ThanhPhanDiUng], [PhanLoai], [GhiChu]) VALUES (11, 190, 3, 25, 10, 2.2, 0.5, 350, N'Khoai', N'Khai vị', N'Phù hợp mọi lứa tuổi')
INSERT [dbo].[ThongTinDinhDuong] ([MaMonAn], [Calo], [Protein], [Carbohydrate], [Fat], [Fiber], [Duong], [Natri], [ThanhPhanDiUng], [PhanLoai], [GhiChu]) VALUES (12, 230, 5, 28, 12, 0.4, 2, 370, N'Bột mì', N'Khai vị', N'')
INSERT [dbo].[ThongTinDinhDuong] ([MaMonAn], [Calo], [Protein], [Carbohydrate], [Fat], [Fiber], [Duong], [Natri], [ThanhPhanDiUng], [PhanLoai], [GhiChu]) VALUES (13, 240, 8, 20, 9, 0.7, 1.8, 360, N'Chả cá', N'Khai vị', N'')
INSERT [dbo].[ThongTinDinhDuong] ([MaMonAn], [Calo], [Protein], [Carbohydrate], [Fat], [Fiber], [Duong], [Natri], [ThanhPhanDiUng], [PhanLoai], [GhiChu]) VALUES (14, 390, 15, 35, 20, 1.8, 3.5, 600, N'Cá', N'Mì cay', N'Cao calo, nhiều đạm')
INSERT [dbo].[ThongTinDinhDuong] ([MaMonAn], [Calo], [Protein], [Carbohydrate], [Fat], [Fiber], [Duong], [Natri], [ThanhPhanDiUng], [PhanLoai], [GhiChu]) VALUES (15, 420, 18, 40, 22, 2, 4, 650, N'Hải sản', N'Mì cay', N'Nhiều năng lượng')
INSERT [dbo].[ThongTinDinhDuong] ([MaMonAn], [Calo], [Protein], [Carbohydrate], [Fat], [Fiber], [Duong], [Natri], [ThanhPhanDiUng], [PhanLoai], [GhiChu]) VALUES (16, 450, 18, 42, 25, 2.2, 5, 700, NULL, N'Mì cay', N'Nhiều thành phần')
INSERT [dbo].[ThongTinDinhDuong] ([MaMonAn], [Calo], [Protein], [Carbohydrate], [Fat], [Fiber], [Duong], [Natri], [ThanhPhanDiUng], [PhanLoai], [GhiChu]) VALUES (17, 400, 16, 38, 20, 1.9, 4, 630, N'Thịt bò', N'Mì cay', N'Nhiều protein')
INSERT [dbo].[ThongTinDinhDuong] ([MaMonAn], [Calo], [Protein], [Carbohydrate], [Fat], [Fiber], [Duong], [Natri], [ThanhPhanDiUng], [PhanLoai], [GhiChu]) VALUES (18, 410, 16, 37, 22, 2, 4.5, 640, N'Gà', N'Mì cay', N'Nhiều năng lượng')
INSERT [dbo].[ThongTinDinhDuong] ([MaMonAn], [Calo], [Protein], [Carbohydrate], [Fat], [Fiber], [Duong], [Natri], [ThanhPhanDiUng], [PhanLoai], [GhiChu]) VALUES (19, 395, 17, 36, 21, 2.3, 4.2, 620, N'Thịt heo', N'Mì cay', N'Không phù hợp dị ứng thịt đỏ')
INSERT [dbo].[ThongTinDinhDuong] ([MaMonAn], [Calo], [Protein], [Carbohydrate], [Fat], [Fiber], [Duong], [Natri], [ThanhPhanDiUng], [PhanLoai], [GhiChu]) VALUES (20, 420, 16, 40, 22, 2, 5, 640, N'Hải sản', N'Mì cay', N'Cao calo – nên chia nhỏ khẩu phần')
INSERT [dbo].[ThongTinDinhDuong] ([MaMonAn], [Calo], [Protein], [Carbohydrate], [Fat], [Fiber], [Duong], [Natri], [ThanhPhanDiUng], [PhanLoai], [GhiChu]) VALUES (21, 460, 20, 42, 23, 3.4, 5.2, 720, NULL, N'Mì cay', N'Dành cho người vận động nhiều')
INSERT [dbo].[ThongTinDinhDuong] ([MaMonAn], [Calo], [Protein], [Carbohydrate], [Fat], [Fiber], [Duong], [Natri], [ThanhPhanDiUng], [PhanLoai], [GhiChu]) VALUES (22, 440, 19, 40, 23, 3, 4.2, 680, NULL, N'Mì cay', N'Bổ sung năng lượng')
INSERT [dbo].[ThongTinDinhDuong] ([MaMonAn], [Calo], [Protein], [Carbohydrate], [Fat], [Fiber], [Duong], [Natri], [ThanhPhanDiUng], [PhanLoai], [GhiChu]) VALUES (23, 430, 18, 39, 21, 2.8, 4.1, 660, NULL, N'Mì cay', N'Nên ăn kèm rau sống')
INSERT [dbo].[ThongTinDinhDuong] ([MaMonAn], [Calo], [Protein], [Carbohydrate], [Fat], [Fiber], [Duong], [Natri], [ThanhPhanDiUng], [PhanLoai], [GhiChu]) VALUES (24, 440, 19, 38, 22, 2.6, 4.6, 670, NULL, N'Mì cay', N'Đậm đà, dễ ăn')
INSERT [dbo].[ThongTinDinhDuong] ([MaMonAn], [Calo], [Protein], [Carbohydrate], [Fat], [Fiber], [Duong], [Natri], [ThanhPhanDiUng], [PhanLoai], [GhiChu]) VALUES (25, 460, 20, 41, 23, 3.2, 5, 710, N'Hải sản', N'Mì cay', N'Nhiều đạm, không phù hợp dị ứng hải sản')
INSERT [dbo].[ThongTinDinhDuong] ([MaMonAn], [Calo], [Protein], [Carbohydrate], [Fat], [Fiber], [Duong], [Natri], [ThanhPhanDiUng], [PhanLoai], [GhiChu]) VALUES (26, 470, 21, 43, 24, 3, 5.5, 720, NULL, N'Mì cay', N'')
INSERT [dbo].[ThongTinDinhDuong] ([MaMonAn], [Calo], [Protein], [Carbohydrate], [Fat], [Fiber], [Duong], [Natri], [ThanhPhanDiUng], [PhanLoai], [GhiChu]) VALUES (27, 480, 22, 44, 25, 3.5, 6, 740, N'Thịt bò', N'Món phụ', N'Chứa nhiều tinh bột')
INSERT [dbo].[ThongTinDinhDuong] ([MaMonAn], [Calo], [Protein], [Carbohydrate], [Fat], [Fiber], [Duong], [Natri], [ThanhPhanDiUng], [PhanLoai], [GhiChu]) VALUES (28, 460, 21, 43, 24, 3.2, 5.8, 710, N'Thịt heo', N'Món phụ', N'')
INSERT [dbo].[ThongTinDinhDuong] ([MaMonAn], [Calo], [Protein], [Carbohydrate], [Fat], [Fiber], [Duong], [Natri], [ThanhPhanDiUng], [PhanLoai], [GhiChu]) VALUES (29, 455, 19, 42, 23, 3.1, 5.5, 705, N'Gà', N'Món phụ', N'Năng lượng cao')
INSERT [dbo].[ThongTinDinhDuong] ([MaMonAn], [Calo], [Protein], [Carbohydrate], [Fat], [Fiber], [Duong], [Natri], [ThanhPhanDiUng], [PhanLoai], [GhiChu]) VALUES (30, 440, 18, 40, 22, 2.9, 5, 690, NULL, N'Món phụ', N'Chứa nhiều carb')
INSERT [dbo].[ThongTinDinhDuong] ([MaMonAn], [Calo], [Protein], [Carbohydrate], [Fat], [Fiber], [Duong], [Natri], [ThanhPhanDiUng], [PhanLoai], [GhiChu]) VALUES (31, 450, 19, 41, 23, 3, 5.2, 690, NULL, N'Món phụ', N'Đậm vị Hàn Quốc')
INSERT [dbo].[ThongTinDinhDuong] ([MaMonAn], [Calo], [Protein], [Carbohydrate], [Fat], [Fiber], [Duong], [Natri], [ThanhPhanDiUng], [PhanLoai], [GhiChu]) VALUES (32, 430, 17, 38, 21, 2.7, 4.5, 670, NULL, N'Món phụ', N'Nhiều rau củ')
INSERT [dbo].[ThongTinDinhDuong] ([MaMonAn], [Calo], [Protein], [Carbohydrate], [Fat], [Fiber], [Duong], [Natri], [ThanhPhanDiUng], [PhanLoai], [GhiChu]) VALUES (33, 500, 25, 35, 18, 4, 2, 720, N'Thịt bò', N'Món phụ', N'Cao đạm')
INSERT [dbo].[ThongTinDinhDuong] ([MaMonAn], [Calo], [Protein], [Carbohydrate], [Fat], [Fiber], [Duong], [Natri], [ThanhPhanDiUng], [PhanLoai], [GhiChu]) VALUES (34, 420, 14, 40, 16, 3.5, 4, 660, NULL, N'Món phụ', N'Cay nhẹ')
INSERT [dbo].[ThongTinDinhDuong] ([MaMonAn], [Calo], [Protein], [Carbohydrate], [Fat], [Fiber], [Duong], [Natri], [ThanhPhanDiUng], [PhanLoai], [GhiChu]) VALUES (35, 430, 15, 38, 20, 2.6, 3.5, 640, N'Thịt heo', N'Món phụ', N'Nhiều béo')
INSERT [dbo].[ThongTinDinhDuong] ([MaMonAn], [Calo], [Protein], [Carbohydrate], [Fat], [Fiber], [Duong], [Natri], [ThanhPhanDiUng], [PhanLoai], [GhiChu]) VALUES (36, 400, 14, 36, 18, 2.5, 3, 600, NULL, N'Món phụ', N'Nhẹ nhàng dễ tiêu')
INSERT [dbo].[ThongTinDinhDuong] ([MaMonAn], [Calo], [Protein], [Carbohydrate], [Fat], [Fiber], [Duong], [Natri], [ThanhPhanDiUng], [PhanLoai], [GhiChu]) VALUES (37, 450, 20, 39, 23, 3, 3.8, 710, NULL, N'Món phụ', N'')
INSERT [dbo].[ThongTinDinhDuong] ([MaMonAn], [Calo], [Protein], [Carbohydrate], [Fat], [Fiber], [Duong], [Natri], [ThanhPhanDiUng], [PhanLoai], [GhiChu]) VALUES (38, 460, 22, 38, 24, 2.9, 4, 720, N'Thịt bò', N'Món phụ', N'Nên dùng nóng')
INSERT [dbo].[ThongTinDinhDuong] ([MaMonAn], [Calo], [Protein], [Carbohydrate], [Fat], [Fiber], [Duong], [Natri], [ThanhPhanDiUng], [PhanLoai], [GhiChu]) VALUES (39, 420, 18, 37, 22, 2.8, 3.9, 690, N'Sữa', N'Món phụ', N'Rất béo, không phù hợp người ăn kiêng')
INSERT [dbo].[ThongTinDinhDuong] ([MaMonAn], [Calo], [Protein], [Carbohydrate], [Fat], [Fiber], [Duong], [Natri], [ThanhPhanDiUng], [PhanLoai], [GhiChu]) VALUES (40, 750, 35, 25, 30, 4, 6, 1200, N'Thịt bò', N'Lẩu', N'Cung cấp nhiều năng lượng')
INSERT [dbo].[ThongTinDinhDuong] ([MaMonAn], [Calo], [Protein], [Carbohydrate], [Fat], [Fiber], [Duong], [Natri], [ThanhPhanDiUng], [PhanLoai], [GhiChu]) VALUES (41, 720, 32, 30, 28, 4, 5, 1100, N'Hải sản', N'Lẩu', N'Dành cho người không dị ứng hải sản')
INSERT [dbo].[ThongTinDinhDuong] ([MaMonAn], [Calo], [Protein], [Carbohydrate], [Fat], [Fiber], [Duong], [Natri], [ThanhPhanDiUng], [PhanLoai], [GhiChu]) VALUES (42, 700, 30, 26, 25, 3, 5, 1150, N'Thịt bò, bánh gạo', N'Lẩu', N'Vị cay nhẹ, phổ biến')
INSERT [dbo].[ThongTinDinhDuong] ([MaMonAn], [Calo], [Protein], [Carbohydrate], [Fat], [Fiber], [Duong], [Natri], [ThanhPhanDiUng], [PhanLoai], [GhiChu]) VALUES (43, 710, 31, 28, 26, 3, 5, 1180, N'Hải sản', N'Lẩu', N'Dễ gây dị ứng nếu nhạy cảm hải sản')
INSERT [dbo].[ThongTinDinhDuong] ([MaMonAn], [Calo], [Protein], [Carbohydrate], [Fat], [Fiber], [Duong], [Natri], [ThanhPhanDiUng], [PhanLoai], [GhiChu]) VALUES (44, 140, 0, 36, 0, 0, 34, 15, N'Cacbonat, caffeine', N'Đồ uống có gas', N'Không dành cho người tiểu đường')
INSERT [dbo].[ThongTinDinhDuong] ([MaMonAn], [Calo], [Protein], [Carbohydrate], [Fat], [Fiber], [Duong], [Natri], [ThanhPhanDiUng], [PhanLoai], [GhiChu]) VALUES (45, 170, 0, 43, 0, 0, 40, 20, N'Cacbonat, caffeine', N'Đồ uống có gas', N'Không dành cho trẻ nhỏ')
INSERT [dbo].[ThongTinDinhDuong] ([MaMonAn], [Calo], [Protein], [Carbohydrate], [Fat], [Fiber], [Duong], [Natri], [ThanhPhanDiUng], [PhanLoai], [GhiChu]) VALUES (46, 135, 0, 34, 0, 0, 33, 15, N'Cacbonat', N'Đồ uống có gas', N'')
INSERT [dbo].[ThongTinDinhDuong] ([MaMonAn], [Calo], [Protein], [Carbohydrate], [Fat], [Fiber], [Duong], [Natri], [ThanhPhanDiUng], [PhanLoai], [GhiChu]) VALUES (47, 165, 0, 41, 0, 0, 38, 18, N'Cacbonat', N'Đồ uống có gas', N'')
INSERT [dbo].[ThongTinDinhDuong] ([MaMonAn], [Calo], [Protein], [Carbohydrate], [Fat], [Fiber], [Duong], [Natri], [ThanhPhanDiUng], [PhanLoai], [GhiChu]) VALUES (48, 160, 0, 39, 0, 0, 37, 18, N'Cacbonat, caffeine', N'Đồ uống lon', N'Dễ gây béo phì nếu uống nhiều')
INSERT [dbo].[ThongTinDinhDuong] ([MaMonAn], [Calo], [Protein], [Carbohydrate], [Fat], [Fiber], [Duong], [Natri], [ThanhPhanDiUng], [PhanLoai], [GhiChu]) VALUES (49, 160, 0, 40, 0, 0, 38, 18, N'Cacbonat', N'Đồ uống lon', N'')
INSERT [dbo].[ThongTinDinhDuong] ([MaMonAn], [Calo], [Protein], [Carbohydrate], [Fat], [Fiber], [Duong], [Natri], [ThanhPhanDiUng], [PhanLoai], [GhiChu]) VALUES (50, 155, 0, 39, 0, 0, 36, 18, N'Cacbonat', N'Đồ uống lon', N'')
INSERT [dbo].[ThongTinDinhDuong] ([MaMonAn], [Calo], [Protein], [Carbohydrate], [Fat], [Fiber], [Duong], [Natri], [ThanhPhanDiUng], [PhanLoai], [GhiChu]) VALUES (51, 0, 0, 0, 0, 0, 0, 5, NULL, N'Nước lọc', N'Thích hợp mọi đối tượng')
INSERT [dbo].[ThongTinDinhDuong] ([MaMonAn], [Calo], [Protein], [Carbohydrate], [Fat], [Fiber], [Duong], [Natri], [ThanhPhanDiUng], [PhanLoai], [GhiChu]) VALUES (52, 280, 3, 38, 5, 0.5, 30, 80, N'Sữa, trân châu', N'Trà sữa', N'Cao đường, không phù hợp người tiểu đường')
INSERT [dbo].[ThongTinDinhDuong] ([MaMonAn], [Calo], [Protein], [Carbohydrate], [Fat], [Fiber], [Duong], [Natri], [ThanhPhanDiUng], [PhanLoai], [GhiChu]) VALUES (53, 290, 4, 40, 5.5, 0.6, 31, 85, N'Sữa, matcha, trân châu', N'Trà sữa', N'Nhiều năng lượng')
INSERT [dbo].[ThongTinDinhDuong] ([MaMonAn], [Calo], [Protein], [Carbohydrate], [Fat], [Fiber], [Duong], [Natri], [ThanhPhanDiUng], [PhanLoai], [GhiChu]) VALUES (54, 120, 0, 30, 0, 0.5, 28, 60, N'Trái cây', N'Trà trái cây', N'Dễ uống, hương đào tự nhiên')
INSERT [dbo].[ThongTinDinhDuong] ([MaMonAn], [Calo], [Protein], [Carbohydrate], [Fat], [Fiber], [Duong], [Natri], [ThanhPhanDiUng], [PhanLoai], [GhiChu]) VALUES (55, 130, 0, 32, 0, 0.5, 30, 65, N'Trái cây, hoa hồng', N'Trà trái cây', N'')
INSERT [dbo].[ThongTinDinhDuong] ([MaMonAn], [Calo], [Protein], [Carbohydrate], [Fat], [Fiber], [Duong], [Natri], [ThanhPhanDiUng], [PhanLoai], [GhiChu]) VALUES (56, 150, 1, 35, 1, 1.2, 32, 70, N'Gạo, sữa', N'Nước ngũ cốc', N'Bổ sung chất xơ nhẹ')
INSERT [dbo].[ThongTinDinhDuong] ([MaMonAn], [Calo], [Protein], [Carbohydrate], [Fat], [Fiber], [Duong], [Natri], [ThanhPhanDiUng], [PhanLoai], [GhiChu]) VALUES (57, 140, 0, 36, 0, 0.3, 35, 60, N'Dâu, dưa lưới', N'Soda', N'Không thích hợp khi đói')
INSERT [dbo].[ThongTinDinhDuong] ([MaMonAn], [Calo], [Protein], [Carbohydrate], [Fat], [Fiber], [Duong], [Natri], [ThanhPhanDiUng], [PhanLoai], [GhiChu]) VALUES (58, 145, 0, 37, 0, 0.3, 36, 62, N'Dừa, đác, thơm', N'Soda', N'Nhiều đường')
INSERT [dbo].[ThongTinDinhDuong] ([MaMonAn], [Calo], [Protein], [Carbohydrate], [Fat], [Fiber], [Duong], [Natri], [ThanhPhanDiUng], [PhanLoai], [GhiChu]) VALUES (59, 138, 0, 35, 0, 0.2, 34, 58, N'Dưa lưới, thơm', N'Soda', N'')
INSERT [dbo].[ThongTinDinhDuong] ([MaMonAn], [Calo], [Protein], [Carbohydrate], [Fat], [Fiber], [Duong], [Natri], [ThanhPhanDiUng], [PhanLoai], [GhiChu]) VALUES (60, 150, 1, 33, 1, 1, 28, 55, N'Gạo, hoa anh đào', N'Nước gạo', N'Ngọt nhẹ, dễ uống')
GO
SET IDENTITY_INSERT [dbo].[TrangThaiDonHang] ON 

INSERT [dbo].[TrangThaiDonHang] ([MaTrangThai], [TenTrangThai], [MoTa]) VALUES (1, N'Mới', N'Đơn hàng mới tạo')
INSERT [dbo].[TrangThaiDonHang] ([MaTrangThai], [TenTrangThai], [MoTa]) VALUES (2, N'Đang chế biến', N'Đơn hàng đang được chế biến')
INSERT [dbo].[TrangThaiDonHang] ([MaTrangThai], [TenTrangThai], [MoTa]) VALUES (3, N'Đã phục vụ', N'Đơn hàng đã được phục vụ')
INSERT [dbo].[TrangThaiDonHang] ([MaTrangThai], [TenTrangThai], [MoTa]) VALUES (4, N'Đã thanh toán', N'Đơn hàng đã thanh toán')
INSERT [dbo].[TrangThaiDonHang] ([MaTrangThai], [TenTrangThai], [MoTa]) VALUES (5, N'Đã hủy', N'Đơn hàng đã hủy')
SET IDENTITY_INSERT [dbo].[TrangThaiDonHang] OFF
GO
/****** Object:  Index [IX_ChiTietDonHang_MaDonHang]    Script Date: 5/13/2025 10:03:57 PM ******/
CREATE NONCLUSTERED INDEX [IX_ChiTietDonHang_MaDonHang] ON [dbo].[ChiTietDonHang]
(
	[MaDonHang] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_ChiTietDonHang_MaMonAn]    Script Date: 5/13/2025 10:03:57 PM ******/
CREATE NONCLUSTERED INDEX [IX_ChiTietDonHang_MaMonAn] ON [dbo].[ChiTietDonHang]
(
	[MaMonAn] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_ChiTietKhuyenMai_MaKhuyenMai]    Script Date: 5/13/2025 10:03:57 PM ******/
CREATE NONCLUSTERED INDEX [IX_ChiTietKhuyenMai_MaKhuyenMai] ON [dbo].[ChiTietKhuyenMai]
(
	[MaKhuyenMai] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_ChiTietKhuyenMai_MaMonAn]    Script Date: 5/13/2025 10:03:57 PM ******/
CREATE NONCLUSTERED INDEX [IX_ChiTietKhuyenMai_MaMonAn] ON [dbo].[ChiTietKhuyenMai]
(
	[MaMonAn] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_ChiTietPhieuNhap_MaNguyenLieu]    Script Date: 5/13/2025 10:03:57 PM ******/
CREATE NONCLUSTERED INDEX [IX_ChiTietPhieuNhap_MaNguyenLieu] ON [dbo].[ChiTietPhieuNhap]
(
	[MaNguyenLieu] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_ChiTietPhieuNhap_MaPhieuNhap]    Script Date: 5/13/2025 10:03:57 PM ******/
CREATE NONCLUSTERED INDEX [IX_ChiTietPhieuNhap_MaPhieuNhap] ON [dbo].[ChiTietPhieuNhap]
(
	[MaPhieuNhap] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_CongThucMon_MaMonAn]    Script Date: 5/13/2025 10:03:57 PM ******/
CREATE NONCLUSTERED INDEX [IX_CongThucMon_MaMonAn] ON [dbo].[CongThucMon]
(
	[MaMonAn] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_CongThucMon_MaNguyenLieu]    Script Date: 5/13/2025 10:03:57 PM ******/
CREATE NONCLUSTERED INDEX [IX_CongThucMon_MaNguyenLieu] ON [dbo].[CongThucMon]
(
	[MaNguyenLieu] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_DonHang_MaBan]    Script Date: 5/13/2025 10:03:57 PM ******/
CREATE NONCLUSTERED INDEX [IX_DonHang_MaBan] ON [dbo].[DonHang]
(
	[MaBan] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_DonHang_MaNhanVien]    Script Date: 5/13/2025 10:03:57 PM ******/
CREATE NONCLUSTERED INDEX [IX_DonHang_MaNhanVien] ON [dbo].[DonHang]
(
	[MaNhanVien] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_DonHang_MaTrangThai]    Script Date: 5/13/2025 10:03:57 PM ******/
CREATE NONCLUSTERED INDEX [IX_DonHang_MaTrangThai] ON [dbo].[DonHang]
(
	[MaTrangThai] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_MonAn_MaLoai]    Script Date: 5/13/2025 10:03:57 PM ******/
CREATE NONCLUSTERED INDEX [IX_MonAn_MaLoai] ON [dbo].[MonAn]
(
	[MaLoai] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ_TaiKhoan_TenDangNhap]    Script Date: 5/13/2025 10:03:57 PM ******/
ALTER TABLE [dbo].[TaiKhoan] ADD  CONSTRAINT [UQ_TaiKhoan_TenDangNhap] UNIQUE NONCLUSTERED 
(
	[TenDangNhap] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_ThanhToan_MaDonHang]    Script Date: 5/13/2025 10:03:57 PM ******/
CREATE NONCLUSTERED INDEX [IX_ThanhToan_MaDonHang] ON [dbo].[ThanhToan]
(
	[MaDonHang] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Ban] ADD  CONSTRAINT [DF__Ban__SucChua__693CA210]  DEFAULT ((4)) FOR [SucChua]
GO
ALTER TABLE [dbo].[Ban] ADD  CONSTRAINT [DF__Ban__TrangThai__6A30C649]  DEFAULT (N'Trống') FOR [TrangThai]
GO
ALTER TABLE [dbo].[BaoCaoDoanhThu] ADD  DEFAULT (getdate()) FOR [NgayTao]
GO
ALTER TABLE [dbo].[BaoCaoMonAn] ADD  DEFAULT (getdate()) FOR [NgayTao]
GO
ALTER TABLE [dbo].[ChiTietBaoCaoMonAn] ADD  DEFAULT ((0)) FOR [SoLuongDaBan]
GO
ALTER TABLE [dbo].[ChiTietDonHang] ADD  CONSTRAINT [DF__ChiTietDo__SoLuo__6E01572D]  DEFAULT ((1)) FOR [SoLuong]
GO
ALTER TABLE [dbo].[ChiTietDonHang] ADD  CONSTRAINT [DF__ChiTietDo__DonGi__6EF57B66]  DEFAULT ((0)) FOR [DonGia]
GO
ALTER TABLE [dbo].[ChiTietKhuyenMai] ADD  DEFAULT ((0)) FOR [PhanTramGiam]
GO
ALTER TABLE [dbo].[DatBan] ADD  DEFAULT ((1)) FOR [SoNguoi]
GO
ALTER TABLE [dbo].[DatBan] ADD  DEFAULT (N'Đã đặt') FOR [TrangThai]
GO
ALTER TABLE [dbo].[DonHang] ADD  CONSTRAINT [DF__DonHang__NgayTao__72C60C4A]  DEFAULT (getdate()) FOR [NgayTao]
GO
ALTER TABLE [dbo].[DonHang] ADD  CONSTRAINT [DF__DonHang__GiamGia__73BA3083]  DEFAULT ((0)) FOR [GiamGia]
GO
ALTER TABLE [dbo].[HinhThucThanhToan] ADD  DEFAULT ((1)) FOR [TrangThai]
GO
ALTER TABLE [dbo].[HoaDonThanhToan] ADD  DEFAULT (getdate()) FOR [NgayDatHang]
GO
ALTER TABLE [dbo].[KhuyenMai] ADD  DEFAULT ((0)) FOR [PhanTramGiam]
GO
ALTER TABLE [dbo].[KhuyenMai] ADD  DEFAULT ((1)) FOR [TrangThai]
GO
ALTER TABLE [dbo].[KiemTraChatLuong] ADD  DEFAULT (getdate()) FOR [NgayKiemTra]
GO
ALTER TABLE [dbo].[MonAn] ADD  DEFAULT ((0)) FOR [DonGia]
GO
ALTER TABLE [dbo].[MonAn] ADD  DEFAULT ((1)) FOR [TrangThai]
GO
ALTER TABLE [dbo].[NguyenVatLieu] ADD  DEFAULT ((0)) FOR [SoLuongTon]
GO
ALTER TABLE [dbo].[NguyenVatLieu] ADD  DEFAULT ((1)) FOR [TrangThai]
GO
ALTER TABLE [dbo].[NhanVien] ADD  DEFAULT (getdate()) FOR [NgayVaoLam]
GO
ALTER TABLE [dbo].[NhanVien] ADD  DEFAULT ((1)) FOR [TrangThai]
GO
ALTER TABLE [dbo].[PhieuNhapKho] ADD  DEFAULT (getdate()) FOR [NgayNhap]
GO
ALTER TABLE [dbo].[PhieuNhapKho] ADD  DEFAULT ((1)) FOR [TrangThai]
GO
ALTER TABLE [dbo].[TaiKhoan] ADD  DEFAULT ((1)) FOR [TrangThai]
GO
ALTER TABLE [dbo].[ThanhToan] ADD  DEFAULT (getdate()) FOR [NgayThanhToan]
GO
ALTER TABLE [dbo].[BaoCaoDoanhThu]  WITH CHECK ADD  CONSTRAINT [FK_BaoCaoDoanhThu_NhanVien] FOREIGN KEY([NguoiTao])
REFERENCES [dbo].[NhanVien] ([MaNhanVien])
GO
ALTER TABLE [dbo].[BaoCaoDoanhThu] CHECK CONSTRAINT [FK_BaoCaoDoanhThu_NhanVien]
GO
ALTER TABLE [dbo].[BaoCaoMonAn]  WITH CHECK ADD  CONSTRAINT [FK_BaoCaoMonAn_NhanVien] FOREIGN KEY([NguoiTao])
REFERENCES [dbo].[NhanVien] ([MaNhanVien])
GO
ALTER TABLE [dbo].[BaoCaoMonAn] CHECK CONSTRAINT [FK_BaoCaoMonAn_NhanVien]
GO
ALTER TABLE [dbo].[ChiTietBaoCaoMonAn]  WITH CHECK ADD  CONSTRAINT [FK_ChiTietBaoCaoMonAn_BaoCaoMonAn] FOREIGN KEY([MaBaoCao])
REFERENCES [dbo].[BaoCaoMonAn] ([MaBaoCao])
GO
ALTER TABLE [dbo].[ChiTietBaoCaoMonAn] CHECK CONSTRAINT [FK_ChiTietBaoCaoMonAn_BaoCaoMonAn]
GO
ALTER TABLE [dbo].[ChiTietBaoCaoMonAn]  WITH CHECK ADD  CONSTRAINT [FK_ChiTietBaoCaoMonAn_MonAn] FOREIGN KEY([MaMonAn])
REFERENCES [dbo].[MonAn] ([MaMonAn])
GO
ALTER TABLE [dbo].[ChiTietBaoCaoMonAn] CHECK CONSTRAINT [FK_ChiTietBaoCaoMonAn_MonAn]
GO
ALTER TABLE [dbo].[ChiTietDonHang]  WITH CHECK ADD  CONSTRAINT [FK_ChiTietDonHang_DonHang] FOREIGN KEY([MaDonHang])
REFERENCES [dbo].[DonHang] ([MaDonHang])
GO
ALTER TABLE [dbo].[ChiTietDonHang] CHECK CONSTRAINT [FK_ChiTietDonHang_DonHang]
GO
ALTER TABLE [dbo].[ChiTietDonHang]  WITH CHECK ADD  CONSTRAINT [FK_ChiTietDonHang_MonAn] FOREIGN KEY([MaMonAn])
REFERENCES [dbo].[MonAn] ([MaMonAn])
GO
ALTER TABLE [dbo].[ChiTietDonHang] CHECK CONSTRAINT [FK_ChiTietDonHang_MonAn]
GO
ALTER TABLE [dbo].[ChiTietKhuyenMai]  WITH CHECK ADD  CONSTRAINT [FK_ChiTietKhuyenMai_KhuyenMai] FOREIGN KEY([MaKhuyenMai])
REFERENCES [dbo].[KhuyenMai] ([MaKhuyenMai])
GO
ALTER TABLE [dbo].[ChiTietKhuyenMai] CHECK CONSTRAINT [FK_ChiTietKhuyenMai_KhuyenMai]
GO
ALTER TABLE [dbo].[ChiTietKhuyenMai]  WITH CHECK ADD  CONSTRAINT [FK_ChiTietKhuyenMai_MonAn] FOREIGN KEY([MaMonAn])
REFERENCES [dbo].[MonAn] ([MaMonAn])
GO
ALTER TABLE [dbo].[ChiTietKhuyenMai] CHECK CONSTRAINT [FK_ChiTietKhuyenMai_MonAn]
GO
ALTER TABLE [dbo].[ChiTietPhieuNhap]  WITH CHECK ADD  CONSTRAINT [FK_ChiTietPhieuNhap_NguyenVatLieu] FOREIGN KEY([MaNguyenLieu])
REFERENCES [dbo].[NguyenVatLieu] ([MaNguyenLieu])
GO
ALTER TABLE [dbo].[ChiTietPhieuNhap] CHECK CONSTRAINT [FK_ChiTietPhieuNhap_NguyenVatLieu]
GO
ALTER TABLE [dbo].[ChiTietPhieuNhap]  WITH CHECK ADD  CONSTRAINT [FK_ChiTietPhieuNhap_PhieuNhapKho] FOREIGN KEY([MaPhieuNhap])
REFERENCES [dbo].[PhieuNhapKho] ([MaPhieuNhap])
GO
ALTER TABLE [dbo].[ChiTietPhieuNhap] CHECK CONSTRAINT [FK_ChiTietPhieuNhap_PhieuNhapKho]
GO
ALTER TABLE [dbo].[CongThucMon]  WITH CHECK ADD  CONSTRAINT [FK_CongThucMon_MonAn] FOREIGN KEY([MaMonAn])
REFERENCES [dbo].[MonAn] ([MaMonAn])
GO
ALTER TABLE [dbo].[CongThucMon] CHECK CONSTRAINT [FK_CongThucMon_MonAn]
GO
ALTER TABLE [dbo].[CongThucMon]  WITH CHECK ADD  CONSTRAINT [FK_CongThucMon_NguyenVatLieu] FOREIGN KEY([MaNguyenLieu])
REFERENCES [dbo].[NguyenVatLieu] ([MaNguyenLieu])
GO
ALTER TABLE [dbo].[CongThucMon] CHECK CONSTRAINT [FK_CongThucMon_NguyenVatLieu]
GO
ALTER TABLE [dbo].[DatBan]  WITH CHECK ADD  CONSTRAINT [FK_DatBan_Ban] FOREIGN KEY([MaBan])
REFERENCES [dbo].[Ban] ([MaBan])
GO
ALTER TABLE [dbo].[DatBan] CHECK CONSTRAINT [FK_DatBan_Ban]
GO
ALTER TABLE [dbo].[DonHang]  WITH CHECK ADD  CONSTRAINT [FK_DonHang_NhanVien] FOREIGN KEY([MaNhanVien])
REFERENCES [dbo].[NhanVien] ([MaNhanVien])
GO
ALTER TABLE [dbo].[DonHang] CHECK CONSTRAINT [FK_DonHang_NhanVien]
GO
ALTER TABLE [dbo].[DonHang]  WITH CHECK ADD  CONSTRAINT [FK_DonHang_TrangThaiDonHang] FOREIGN KEY([MaTrangThai])
REFERENCES [dbo].[TrangThaiDonHang] ([MaTrangThai])
GO
ALTER TABLE [dbo].[DonHang] CHECK CONSTRAINT [FK_DonHang_TrangThaiDonHang]
GO
ALTER TABLE [dbo].[HoaDonThanhToan]  WITH CHECK ADD  CONSTRAINT [FK_HoaDonThanhToan_DonHang] FOREIGN KEY([MaDonHang])
REFERENCES [dbo].[DonHang] ([MaDonHang])
GO
ALTER TABLE [dbo].[HoaDonThanhToan] CHECK CONSTRAINT [FK_HoaDonThanhToan_DonHang]
GO
ALTER TABLE [dbo].[HoaDonThanhToan]  WITH CHECK ADD  CONSTRAINT [FK_HoaDonThanhToan_HinhThuc] FOREIGN KEY([MaHinhThuc])
REFERENCES [dbo].[HinhThucThanhToan] ([MaHinhThuc])
GO
ALTER TABLE [dbo].[HoaDonThanhToan] CHECK CONSTRAINT [FK_HoaDonThanhToan_HinhThuc]
GO
ALTER TABLE [dbo].[HoaDonThanhToan]  WITH CHECK ADD  CONSTRAINT [FK_HoaDonThanhToan_NhanVien] FOREIGN KEY([MaNhanVien])
REFERENCES [dbo].[NhanVien] ([MaNhanVien])
GO
ALTER TABLE [dbo].[HoaDonThanhToan] CHECK CONSTRAINT [FK_HoaDonThanhToan_NhanVien]
GO
ALTER TABLE [dbo].[KiemTraChatLuong]  WITH CHECK ADD  CONSTRAINT [FK_KiemTraChatLuong_NhanVien] FOREIGN KEY([NguoiKiemTra])
REFERENCES [dbo].[NhanVien] ([MaNhanVien])
GO
ALTER TABLE [dbo].[KiemTraChatLuong] CHECK CONSTRAINT [FK_KiemTraChatLuong_NhanVien]
GO
ALTER TABLE [dbo].[MonAn]  WITH CHECK ADD  CONSTRAINT [FK_MonAn_LoaiMonAn] FOREIGN KEY([MaLoai])
REFERENCES [dbo].[LoaiMonAn] ([MaLoai])
GO
ALTER TABLE [dbo].[MonAn] CHECK CONSTRAINT [FK_MonAn_LoaiMonAn]
GO
ALTER TABLE [dbo].[NhanVien]  WITH CHECK ADD  CONSTRAINT [FK_NhanVien_TaiKhoan] FOREIGN KEY([MaTaiKhoan])
REFERENCES [dbo].[TaiKhoan] ([MaTaiKhoan])
GO
ALTER TABLE [dbo].[NhanVien] CHECK CONSTRAINT [FK_NhanVien_TaiKhoan]
GO
ALTER TABLE [dbo].[PhieuNhapKho]  WITH CHECK ADD  CONSTRAINT [FK_PhieuNhapKho_NhanVien] FOREIGN KEY([MaNhanVien])
REFERENCES [dbo].[NhanVien] ([MaNhanVien])
GO
ALTER TABLE [dbo].[PhieuNhapKho] CHECK CONSTRAINT [FK_PhieuNhapKho_NhanVien]
GO
ALTER TABLE [dbo].[TaiKhoan]  WITH CHECK ADD  CONSTRAINT [FK_TaiKhoan_LoaiTaiKhoan] FOREIGN KEY([MaLoai])
REFERENCES [dbo].[LoaiTaiKhoan] ([MaLoai])
GO
ALTER TABLE [dbo].[TaiKhoan] CHECK CONSTRAINT [FK_TaiKhoan_LoaiTaiKhoan]
GO
ALTER TABLE [dbo].[ThanhToan]  WITH CHECK ADD  CONSTRAINT [FK_ThanhToan_DonHang] FOREIGN KEY([MaDonHang])
REFERENCES [dbo].[DonHang] ([MaDonHang])
GO
ALTER TABLE [dbo].[ThanhToan] CHECK CONSTRAINT [FK_ThanhToan_DonHang]
GO
ALTER TABLE [dbo].[ThanhToan]  WITH CHECK ADD  CONSTRAINT [FK_ThanhToan_HinhThucThanhToan] FOREIGN KEY([MaHinhThuc])
REFERENCES [dbo].[HinhThucThanhToan] ([MaHinhThuc])
GO
ALTER TABLE [dbo].[ThanhToan] CHECK CONSTRAINT [FK_ThanhToan_HinhThucThanhToan]
GO
ALTER TABLE [dbo].[ThanhToan]  WITH CHECK ADD  CONSTRAINT [FK_ThanhToan_NhanVien] FOREIGN KEY([MaNhanVien])
REFERENCES [dbo].[NhanVien] ([MaNhanVien])
GO
ALTER TABLE [dbo].[ThanhToan] CHECK CONSTRAINT [FK_ThanhToan_NhanVien]
GO
ALTER TABLE [dbo].[ThongTinDinhDuong]  WITH CHECK ADD  CONSTRAINT [FK_ThongTinDinhDuong_MonAn] FOREIGN KEY([MaMonAn])
REFERENCES [dbo].[MonAn] ([MaMonAn])
GO
ALTER TABLE [dbo].[ThongTinDinhDuong] CHECK CONSTRAINT [FK_ThongTinDinhDuong_MonAn]
GO
/****** Object:  StoredProcedure [dbo].[SP_BaoCaoDoanhThuTheoNgay]    Script Date: 5/13/2025 10:03:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_BaoCaoDoanhThuTheoNgay]
    @NgayBatDau DATE,
    @NgayKetThuc DATE
AS
BEGIN
    SELECT 
        CONVERT(DATE, dh.NgayTao) AS Ngay,
        COUNT(dh.MaDonHang) AS SoDonHang,
        SUM(dh.TongTien) AS TongDoanhThu
    FROM DonHang dh
    WHERE dh.MaTrangThai = 4 -- Đã thanh toán
        AND CONVERT(DATE, dh.NgayTao) BETWEEN @NgayBatDau AND @NgayKetThuc
    GROUP BY CONVERT(DATE, dh.NgayTao)
    ORDER BY Ngay
END
GO
/****** Object:  StoredProcedure [dbo].[SP_BaoCaoDoanhThuTheoThang]    Script Date: 5/13/2025 10:03:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_BaoCaoDoanhThuTheoThang]
    @Nam INT
AS
BEGIN
    SELECT 
        MONTH(dh.NgayTao) AS Thang,
        COUNT(dh.MaDonHang) AS SoDonHang,
        SUM(dh.TongTien) AS TongDoanhThu
    FROM DonHang dh
    WHERE dh.MaTrangThai = 4 -- Đã thanh toán
        AND YEAR(dh.NgayTao) = @Nam
    GROUP BY MONTH(dh.NgayTao)
    ORDER BY Thang
END
GO
/****** Object:  StoredProcedure [dbo].[SP_BaoCaoKhoNguyenLieuSapHet]    Script Date: 5/13/2025 10:03:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_BaoCaoKhoNguyenLieuSapHet]
    @MucCanhBao FLOAT = 10 -- Mặc định cảnh báo khi số lượng tồn < 10
AS
BEGIN
    SELECT 
        MaNguyenLieu,
        TenNguyenLieu,
        DonViTinh,
        SoLuongTon,
        MoTa
    FROM NguyenVatLieu
    WHERE TrangThai = 1 -- Đang sử dụng
        AND SoLuongTon < @MucCanhBao
    ORDER BY SoLuongTon
END
GO
/****** Object:  StoredProcedure [dbo].[SP_BaoCaoKiemTraChatLuong]    Script Date: 5/13/2025 10:03:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_BaoCaoKiemTraChatLuong]
    @NgayBatDau DATE,
    @NgayKetThuc DATE,
    @LoaiKiemTra VARCHAR(20) = NULL
AS
BEGIN
    SELECT 
        kt.MaKiemTra,
        kt.LoaiKiemTra,
        kt.DoiTuongKiemTra,
        kt.NgayKiemTra,
        nv.HoTen AS NguoiKiemTra,
        kt.TieuChiKiemTra,
        kt.GiaTri,
        kt.DonVi,
        kt.KetQua,
        kt.GhiChu
    FROM KiemTraChatLuong kt
    INNER JOIN NhanVien nv ON kt.NguoiKiemTra = nv.MaNhanVien
    WHERE CONVERT(DATE, kt.NgayKiemTra) BETWEEN @NgayBatDau AND @NgayKetThuc
        AND (@LoaiKiemTra IS NULL OR kt.LoaiKiemTra = @LoaiKiemTra)
    ORDER BY kt.NgayKiemTra DESC
END
GO
/****** Object:  StoredProcedure [dbo].[SP_BaoCaoMonAnBanChay]    Script Date: 5/13/2025 10:03:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_BaoCaoMonAnBanChay]
    @NgayBatDau DATE,
    @NgayKetThuc DATE,
    @SoLuong INT = 10
AS
BEGIN
    SELECT TOP (@SoLuong)
        m.MaMonAn,
        m.TenMonAn,
        SUM(ct.SoLuong) AS TongSoLuong,
        SUM(ct.ThanhTien) AS TongDoanhThu
    FROM ChiTietDonHang ct
    INNER JOIN MonAn m ON ct.MaMonAn = m.MaMonAn
    INNER JOIN DonHang dh ON ct.MaDonHang = dh.MaDonHang
    WHERE dh.MaTrangThai = 4 -- Đã thanh toán
        AND CONVERT(DATE, dh.NgayTao) BETWEEN @NgayBatDau AND @NgayKetThuc
    GROUP BY m.MaMonAn, m.TenMonAn
    ORDER BY TongSoLuong DESC
END
GO
/****** Object:  StoredProcedure [dbo].[SP_CapNhatTrangThaiBan]    Script Date: 5/13/2025 10:03:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_CapNhatTrangThaiBan]
    @MaBan INT,
    @TrangThai NVARCHAR(50)
AS
BEGIN
    UPDATE Ban
    SET TrangThai = @TrangThai
    WHERE MaBan = @MaBan
END
GO
/****** Object:  StoredProcedure [dbo].[SP_CapNhatTrangThaiDonHang]    Script Date: 5/13/2025 10:03:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_CapNhatTrangThaiDonHang]
    @MaDonHang INT,
    @MaTrangThai INT
AS
BEGIN
    UPDATE DonHang
    SET MaTrangThai = @MaTrangThai
    WHERE MaDonHang = @MaDonHang
    
    -- Nếu trạng thái là "Đã thanh toán" thì cập nhật trạng thái bàn
    IF @MaTrangThai = 4 -- Đã thanh toán
    BEGIN
        DECLARE @MaBan INT
        SELECT @MaBan = MaBan FROM DonHang WHERE MaDonHang = @MaDonHang
        
        EXEC SP_CapNhatTrangThaiBan @MaBan, N'Trống'
    END
END
GO
/****** Object:  StoredProcedure [dbo].[SP_GoiYMonAnTheoDinhDuong]    Script Date: 5/13/2025 10:03:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_GoiYMonAnTheoDinhDuong]
    @CaloMin FLOAT = NULL,
    @CaloMax FLOAT = NULL,
    @ProteinMin FLOAT = NULL,
    @ProteinMax FLOAT = NULL,
    @CarbMin FLOAT = NULL,
    @CarbMax FLOAT = NULL,
    @FatMin FLOAT = NULL,
    @FatMax FLOAT = NULL,
    @PhanLoai NVARCHAR(100) = NULL,
    @ThanhPhanDiUng NVARCHAR(200) = NULL
AS
BEGIN
    SELECT 
        m.MaMonAn,
        m.TenMonAn,
        l.TenLoai,
        m.DonGia,
        td.Calo,
        td.Protein,
        td.Carbohydrate,
        td.Fat,
        td.Fiber,
        td.Duong,
        td.Natri,
        td.ThanhPhanDiUng,
        td.PhanLoai
    FROM MonAn m
    INNER JOIN LoaiMonAn l ON m.MaLoai = l.MaLoai
    INNER JOIN ThongTinDinhDuong td ON m.MaMonAn = td.MaMonAn
    WHERE m.TrangThai = 1
        AND (@CaloMin IS NULL OR td.Calo >= @CaloMin)
        AND (@CaloMax IS NULL OR td.Calo <= @CaloMax)
        AND (@ProteinMin IS NULL OR td.Protein >= @ProteinMin)
        AND (@ProteinMax IS NULL OR td.Protein <= @ProteinMax)
        AND (@CarbMin IS NULL OR td.Carbohydrate >= @CarbMin)
        AND (@CarbMax IS NULL OR td.Carbohydrate <= @CarbMax)
        AND (@FatMin IS NULL OR td.Fat >= @FatMin)
        AND (@FatMax IS NULL OR td.Fat <= @FatMax)
        AND (@PhanLoai IS NULL OR td.PhanLoai LIKE N'%' + @PhanLoai + N'%')
        AND (@ThanhPhanDiUng IS NULL OR td.ThanhPhanDiUng NOT LIKE N'%' + @ThanhPhanDiUng + N'%')
    ORDER BY td.Calo
END
GO
/****** Object:  StoredProcedure [dbo].[SP_LayDanhSachBan]    Script Date: 5/13/2025 10:03:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_LayDanhSachBan]
AS
BEGIN
    SELECT MaBan, TenBan, SucChua, TrangThai, GhiChu
    FROM Ban
    ORDER BY TenBan
END
GO
/****** Object:  StoredProcedure [dbo].[SP_LayDanhSachMonAn]    Script Date: 5/13/2025 10:03:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_LayDanhSachMonAn]
AS
BEGIN
    SELECT m.MaMonAn, m.TenMonAn, l.TenLoai, m.DonGia, m.MoTa, m.HinhAnh, m.TrangThai
    FROM MonAn m
    INNER JOIN LoaiMonAn l ON m.MaLoai = l.MaLoai
    WHERE m.TrangThai = 1
    ORDER BY m.MaLoai, m.TenMonAn
END
GO
/****** Object:  StoredProcedure [dbo].[SP_TaoDonHang]    Script Date: 5/13/2025 10:03:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_TaoDonHang]
    @MaBan INT,
    @MaNhanVien INT,
    @GhiChu NVARCHAR(200) = NULL,
    @MaDonHang INT OUTPUT
AS
BEGIN
    INSERT INTO DonHang (MaBan, MaNhanVien, NgayTao, MaTrangThai, GiamGia, GhiChu)
    VALUES (@MaBan, @MaNhanVien, GETDATE(), 1, 0, @GhiChu)
    
    SET @MaDonHang = SCOPE_IDENTITY()
    
    -- Cập nhật trạng thái bàn
    EXEC SP_CapNhatTrangThaiBan @MaBan, N'Đang phục vụ'
    
    RETURN @MaDonHang
END
GO
/****** Object:  StoredProcedure [dbo].[SP_TaoMaKiemTraMoi]    Script Date: 5/13/2025 10:03:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- Sửa đổi Stored Procedure để tạo mã kiểm tra mới tự động
CREATE PROCEDURE [dbo].[SP_TaoMaKiemTraMoi]
AS
BEGIN
    DECLARE @id INT = 1
    DECLARE @NewMaKiemTra VARCHAR(20)  -- Tăng kích thước để chứa mã dài hơn
    
    -- Tìm ID trống nhỏ nhất
    WHILE EXISTS (
        SELECT 1 
        FROM KiemTraChatLuong 
        WHERE MaKiemTra LIKE 'KT%'
        AND ISNUMERIC(SUBSTRING(MaKiemTra, 3, LEN(MaKiemTra)-2)) = 1
        AND TRY_CAST(SUBSTRING(MaKiemTra, 3, LEN(MaKiemTra)-2) AS INT) = @id
    )
    BEGIN
        SET @id = @id + 1
    END
    
    -- Tạo mã mới với định dạng KTxxxx (số chữ số tùy theo giá trị @id)
    SET @NewMaKiemTra = 'KT' + CAST(@id AS VARCHAR(10))
    
    -- Trả về mã mới
    SELECT @NewMaKiemTra AS MaKiemTraMoi
END
GO
/****** Object:  StoredProcedure [dbo].[SP_TaoPhieuNhapKho]    Script Date: 5/13/2025 10:03:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_TaoPhieuNhapKho]
    @MaNhanVien INT,
    @NhaCungCap NVARCHAR(100) = NULL,
    @GhiChu NVARCHAR(200) = NULL,
    @MaPhieuNhap INT OUTPUT
AS
BEGIN
    INSERT INTO PhieuNhapKho (MaNhanVien, NgayNhap, NhaCungCap, GhiChu, TrangThai)
    VALUES (@MaNhanVien, GETDATE(), @NhaCungCap, @GhiChu, 1)
    
    SET @MaPhieuNhap = SCOPE_IDENTITY()
    
    RETURN @MaPhieuNhap
END
GO
/****** Object:  StoredProcedure [dbo].[SP_ThanhToanDonHang]    Script Date: 5/13/2025 10:03:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_ThanhToanDonHang]
    @MaDonHang INT,
    @MaNhanVien INT,
    @MaHinhThuc INT,
    @GiamGia FLOAT = 0,
    @GhiChu NVARCHAR(200) = NULL
AS
BEGIN
    DECLARE @TongTien FLOAT
    DECLARE @SoTienThanhToan FLOAT
    DECLARE @MaBan INT
    
    -- Cập nhật giảm giá cho đơn hàng
    UPDATE DonHang
    SET GiamGia = @GiamGia
    WHERE MaDonHang = @MaDonHang
    
    -- Tính tổng tiền sau khi giảm giá
    SELECT @TongTien = TongTien, @MaBan = MaBan
    FROM DonHang
    WHERE MaDonHang = @MaDonHang
    
    SET @SoTienThanhToan = @TongTien * (1 - @GiamGia / 100)
    
    -- Cập nhật tổng tiền sau khi giảm giá
    UPDATE DonHang
    SET TongTien = @SoTienThanhToan
    WHERE MaDonHang = @MaDonHang
    
    -- Tạo bản ghi thanh toán
    INSERT INTO ThanhToan (MaDonHang, MaNhanVien, MaHinhThuc, SoTien, NgayThanhToan, GhiChu)
    VALUES (@MaDonHang, @MaNhanVien, @MaHinhThuc, @SoTienThanhToan, GETDATE(), @GhiChu)
    
    -- Cập nhật trạng thái đơn hàng
    EXEC SP_CapNhatTrangThaiDonHang @MaDonHang, 4 -- Đã thanh toán
    
    -- Cập nhật trạng thái bàn
    EXEC SP_CapNhatTrangThaiBan @MaBan, N'Trống'
END
GO
/****** Object:  StoredProcedure [dbo].[SP_ThemChiTietDonHang]    Script Date: 5/13/2025 10:03:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_ThemChiTietDonHang]
    @MaDonHang INT,
    @MaMonAn INT,
    @SoLuong INT
AS
BEGIN
    DECLARE @DonGia FLOAT
    DECLARE @ThanhTien FLOAT
    
    -- Lấy đơn giá của món ăn
    SELECT @DonGia = DonGia FROM MonAn WHERE MaMonAn = @MaMonAn
    
    -- Tính thành tiền
    SET @ThanhTien = @SoLuong * @DonGia
    
    -- Kiểm tra xem món ăn đã có trong đơn hàng chưa
    IF EXISTS (SELECT 1 FROM ChiTietDonHang WHERE MaDonHang = @MaDonHang AND MaMonAn = @MaMonAn)
    BEGIN
        -- Nếu có rồi thì cập nhật số lượng
        UPDATE ChiTietDonHang
        SET SoLuong = SoLuong + @SoLuong,
            ThanhTien = ThanhTien + @ThanhTien
        WHERE MaDonHang = @MaDonHang AND MaMonAn = @MaMonAn
    END
    ELSE
    BEGIN
        -- Nếu chưa có thì thêm mới
        INSERT INTO ChiTietDonHang (MaDonHang, MaMonAn, SoLuong, DonGia, ThanhTien)
        VALUES (@MaDonHang, @MaMonAn, @SoLuong, @DonGia, @ThanhTien)
    END
    
    -- Cập nhật tổng tiền của đơn hàng
    UPDATE DonHang
    SET TongTien = (SELECT SUM(ThanhTien) FROM ChiTietDonHang WHERE MaDonHang = @MaDonHang)
    WHERE MaDonHang = @MaDonHang
END
GO
/****** Object:  StoredProcedure [dbo].[SP_ThemChiTietPhieuNhap]    Script Date: 5/13/2025 10:03:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_ThemChiTietPhieuNhap]
    @MaPhieuNhap INT,
    @MaNguyenLieu INT,
    @SoLuong FLOAT,
    @DonGia FLOAT,
    @GhiChu NVARCHAR(200) = NULL
AS
BEGIN
    DECLARE @ThanhTien FLOAT
    
    -- Tính thành tiền
    SET @ThanhTien = @SoLuong * @DonGia
    
    -- Thêm chi tiết phiếu nhập
    INSERT INTO ChiTietPhieuNhap (MaPhieuNhap, MaNguyenLieu, SoLuong, DonGia, ThanhTien, GhiChu)
    VALUES (@MaPhieuNhap, @MaNguyenLieu, @SoLuong, @DonGia, @ThanhTien, @GhiChu)
    
    -- Cập nhật tổng tiền của phiếu nhập
    UPDATE PhieuNhapKho
    SET TongTien = (SELECT SUM(ThanhTien) FROM ChiTietPhieuNhap WHERE MaPhieuNhap = @MaPhieuNhap)
    WHERE MaPhieuNhap = @MaPhieuNhap
    
    -- Cập nhật số lượng tồn của nguyên vật liệu
    UPDATE NguyenVatLieu
    SET SoLuongTon = SoLuongTon + @SoLuong
    WHERE MaNguyenLieu = @MaNguyenLieu
END
GO
/****** Object:  StoredProcedure [dbo].[SP_ThemKiemTraChatLuong]    Script Date: 5/13/2025 10:03:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- Tạo Stored Procedure để thêm phiếu kiểm tra chất lượng
CREATE PROCEDURE [dbo].[SP_ThemKiemTraChatLuong]
    @MaKiemTra VARCHAR(10),
    @LoaiKiemTra VARCHAR(20),
    @DoiTuongKiemTra VARCHAR(10),
    @NgayKiemTra DATETIME,
    @NguoiKiemTra INT,
    @TieuChiKiemTra NVARCHAR(50) = NULL,
    @GiaTri FLOAT = NULL,
    @DonVi NVARCHAR(10) = NULL,
    @KetQua VARCHAR(20),
    @GhiChu NVARCHAR(200) = NULL,
    @HinhAnh VARCHAR(200) = NULL
AS
BEGIN
    -- Kiểm tra xem mã kiểm tra đã tồn tại chưa
    IF EXISTS (SELECT 1 FROM KiemTraChatLuong WHERE MaKiemTra = @MaKiemTra)
    BEGIN
        -- Nếu đã tồn tại, trả về 0 (thất bại)
        RETURN 0
    END
    
    -- Nếu chưa tồn tại, thêm mới
    INSERT INTO KiemTraChatLuong (
        MaKiemTra, 
        LoaiKiemTra, 
        DoiTuongKiemTra, 
        NgayKiemTra, 
        NguoiKiemTra, 
        TieuChiKiemTra, 
        GiaTri, 
        DonVi, 
        KetQua, 
        GhiChu, 
        HinhAnh
    )
    VALUES (
        @MaKiemTra,
        @LoaiKiemTra,
        @DoiTuongKiemTra,
        @NgayKiemTra,
        @NguoiKiemTra,
        @TieuChiKiemTra,
        @GiaTri,
        @DonVi,
        @KetQua,
        @GhiChu,
        @HinhAnh
    )
    
    -- Trả về 1 (thành công)
    RETURN 1
END
GO
/****** Object:  StoredProcedure [dbo].[SP_TimKiemMonAn]    Script Date: 5/13/2025 10:03:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_TimKiemMonAn]
    @TuKhoa NVARCHAR(100)
AS
BEGIN
    SELECT m.MaMonAn, m.TenMonAn, l.TenLoai, m.DonGia, m.MoTa, m.HinhAnh, m.TrangThai
    FROM MonAn m
    INNER JOIN LoaiMonAn l ON m.MaLoai = l.MaLoai
    WHERE m.TrangThai = 1 AND (m.TenMonAn LIKE N'%' + @TuKhoa + N'%' OR l.TenLoai LIKE N'%' + @TuKhoa + N'%')
    ORDER BY m.MaLoai, m.TenMonAn
END
GO
USE [master]
GO
ALTER DATABASE [QuanLyQuanAn] SET  READ_WRITE 
GO
