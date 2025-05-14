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

-- Tạo Stored Procedure để tạo mã kiểm tra mới tự động
CREATE PROCEDURE [dbo].[SP_TaoMaKiemTraMoi]
AS
BEGIN
    DECLARE @LastNumber INT
    DECLARE @NewMaKiemTra VARCHAR(10)
    
    -- Lấy số lớn nhất hiện tại
    SELECT @LastNumber = MAX(CAST(SUBSTRING(MaKiemTra, 3, LEN(MaKiemTra) - 2) AS INT)) 
    FROM KiemTraChatLuong 
    WHERE MaKiemTra LIKE 'KT%'
    
    -- Nếu không có mã nào, bắt đầu từ 0
    IF @LastNumber IS NULL
        SET @LastNumber = 0
    
    -- Tạo mã mới
    SET @NewMaKiemTra = 'KT' + RIGHT('000' + CAST((@LastNumber + 1) AS VARCHAR(3)), 3)
    
    -- Trả về mã mới
    SELECT @NewMaKiemTra AS MaKiemTraMoi
END
GO


-- Sửa đổi Stored Procedure để tạo mã kiểm tra mới tự động
ALTER PROCEDURE [dbo].[SP_TaoMaKiemTraMoi]
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