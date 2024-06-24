CREATE TABLE NHANVIEN
(
	MANV 	CHAR(4) PRIMARY KEY,
	HOTEN	VARCHAR(40),
	EMAIL	VARCHAR(50),
	SODT	VARCHAR(20),
	NGSINH	SMALLDATETIME,
	LVL		SMALLINT,
	MACM	CHAR(4),
	GHICHU	VARCHAR(80),
);
CREATE TABLE CHUYENMON
(
	MACM	CHAR(4) PRIMARY KEY,
	TENCM	VARCHAR(15),
);
CREATE TABLE TAIKHOAN
(
	MATK	CHAR(4) PRIMARY KEY,
	MANV	CHAR(4),
	MAQH	CHAR(4),
	EMAIL	VARCHAR(50),
	PASS	VARCHAR(20),
);
CREATE TABLE QUYENHAN
(
	MAQH	CHAR(4) PRIMARY KEY,
	TENQH	VARCHAR(15),
);
CREATE TABLE LOAISK
(
	MALSK	CHAR(4) PRIMARY KEY,
	TENLSK	VARCHAR(20),
	MONEYMIN	MONEY,
	MONEYMAX	MONEY,
);
CREATE TABLE DUAN
(
	MADA	CHAR(4) PRIMARY KEY,
	MALSK	CHAR(4),
	MAOWNER	CHAR(4),
	TENDA	VARCHAR(80),
	NGANSACH	MONEY,
	TSTART	SMALLDATETIME,
	TEND	SMALLDATETIME,
	STAT	VARCHAR(15),

);
CREATE TABLE CONGVIEC
(
	MACV	CHAR(4) PRIMARY KEY,
	MADA	CHAR(4),
	MANV	CHAR(4),
	MACM	CHAR(4),
	TENCV	VARCHAR (80),
	TSTART	SMALLDATETIME,
	TEND	SMALLDATETIME,
	NGANSACH	MONEY,
	DADUNG	MONEY,
	TIENDO	SMALLINT,
	YCDINHKEM	VARCHAR(20),
	TEPDINHKEM	VARCHAR(50),
);
--------------------------------------------------------------------------------
--TẠO TRIGGER CHO ĐIỀU KIỆN NGÂN SÁCH
CREATE OR ALTER TRIGGER Check_NGANSACH
ON DUAN
AFTER INSERT, UPDATE
AS
BEGIN
    -- Khai báo các biến
    DECLARE @MALSK CHAR(10);
    DECLARE @MONEYMIN INT;
    DECLARE @MONEYMAX INT;
    DECLARE @TENLSK NVARCHAR(100);

    -- Lặp qua các hàng được chèn hoặc cập nhật
    DECLARE cur CURSOR FOR
    SELECT i.MALSK
    FROM inserted i;

    OPEN cur;
    FETCH NEXT FROM cur INTO @MALSK;

    WHILE @@FETCH_STATUS = 0
    BEGIN
        -- Lấy giá trị MONEYMIN, MONEYMAX và TENLSK từ bảng LOAISK tương ứng với MALSK
        SELECT @MONEYMIN = MONEYMIN, @MONEYMAX = MONEYMAX, @TENLSK = TENLSK
        FROM LOAISK
        WHERE MALSK = @MALSK;

        -- Kiểm tra điều kiện và nếu không thỏa mãn, phát sinh lỗi
        IF EXISTS (
            SELECT 1
            FROM inserted i
            WHERE i.NGANSACH > @MONEYMAX OR i.NGANSACH < @MONEYMIN
        )
        BEGIN
            DECLARE @ErrorMessage NVARCHAR(100);
            SET @ErrorMessage = N'Ngân sách phải nằm trong khoảng ' + CAST(@MONEYMIN AS NVARCHAR(10)) + N' và ' + CAST(@MONEYMAX AS NVARCHAR(10)) + N' cho sự kiện ' + @TENLSK;
            RAISERROR(@ErrorMessage, 16, 1);
            ROLLBACK TRANSACTION;
            CLOSE cur;
            DEALLOCATE cur;
            RETURN;
        END;

        FETCH NEXT FROM cur INTO @MALSK;
    END;

    CLOSE cur;
    DEALLOCATE cur;
END;
----------------------------------------------------------------------------
-- TÌNH TRẠNG DỰ ÁN
ALTER TABLE DUAN
ADD TINHTRANG varchar(20);

ALTER TABLE DUAN ADD CONSTRAINT Check_TinhTrang CHECK (TINHTRANG in ('Completed','On-going','Delayed','Canceled'))

ALTER TABLE NHANVIEN ADD CONSTRAINT Check_Gender CHECK (PHAI in ('Nam','Nữ'))

-- NGÂN SÁCH CÔNG VIỆC
ALTER TABLE CONGVIEC ADD CONSTRAINT Check_NganSachCV CHECK (NGANSACH >=0) 

-- KIỂM TRA TỔNG NGÂN SÁCH CÔNG VIỆC
CREATE OR ALTER TRIGGER Check_CongViec_Ngansach
ON DUAN
AFTER INSERT, UPDATE
AS
BEGIN
    -- Khai báo biến để lưu trữ NGANSACH của DUAN
    DECLARE @DuanNgansach INT;

    -- Lấy NGANSACH của DUAN
    SELECT @DuanNgansach = NGANSACH FROM DUAN WHERE MADA IN (SELECT MADA FROM inserted);

    -- Kiểm tra tổng NGANSACH của CÔNG VIỆC so với NGANSACH của DỰ ÁN
    IF EXISTS (
        SELECT 1
        FROM inserted i
        INNER JOIN (
            SELECT MADA, SUM(NGANSACH) AS TotalNgansach
            FROM CONGVIEC
            GROUP BY MADA
        ) cv ON i.MADA = cv.MADA
        WHERE cv.TotalNgansach > @DuanNgansach
    )
    BEGIN
        DECLARE @ErrorMessage NVARCHAR(100);
        SET @ErrorMessage = N'Tổng ngân sách cho công việc không được vượt quá ' + CAST(@DuanNgansach AS NVARCHAR(10));
        RAISERROR (@ErrorMessage, 16, 1);
        ROLLBACK TRANSACTION;
    END;
END;

-- KIỂM TRA NGÀY BẮT ĐẦU VÀ NGÀY KẾT THÚC CÔNG VIỆC SO VỚI DỰ ÁN
CREATE OR ALTER TRIGGER Check_CongViec_DuAn_Time
ON CONGVIEC
AFTER INSERT, UPDATE
AS
BEGIN

    -- Khai báo biến để lưu trữ TSTART của DUAN
    DECLARE @DuanStart SMALLDATETIME;

    -- Lấy TSTART của DUAN
    SELECT @DuanStart = TSTART FROM DUAN WHERE MADA IN (SELECT MADA FROM inserted);

    -- Khai báo biến để lưu trữ TEND của DUAN
    DECLARE @DuanEnd SMALLDATETIME;

    -- Lấy TEND của DUAN
    SELECT @DuanEnd = TEND FROM DUAN WHERE MADA IN (SELECT MADA FROM inserted);
    -- Kiểm tra ngân sách đã dùng so với ngân sách được cấp cho công việc

    IF EXISTS (
        SELECT 1
        FROM inserted i
        WHERE i.TSTART < @DuanStart
    )
    BEGIN
        RAISERROR (N'Ngày bắt đầu công việc phải bằng hoặc sau ngày bắt đầu dự án', 16, 1);
        ROLLBACK TRANSACTION;
    END;

    IF EXISTS (
        SELECT 1
        FROM inserted i
        WHERE i.TEND > @DuanEnd
    )
    BEGIN
        RAISERROR (N'Ngày kết thúc công việc phải bằng hoặc trước ngày kết thúc dự án', 16, 1);
        ROLLBACK TRANSACTION;
    END;

END;

-- KIỂM TRA NGÂN SÁCH ĐÃ DÙNG SO VỚI NGÂN SÁCH CÔNG VIỆC
CREATE OR ALTER TRIGGER Check_CongViec_Ngansach_Dung
ON CONGVIEC
AFTER INSERT, UPDATE
AS
BEGIN
    -- Kiểm tra ngân sách đã dùng so với ngân sách được cấp cho công việc
    IF EXISTS (
        SELECT 1
        FROM inserted i
        WHERE i.DADUNG > i.NGANSACH
           OR i.DADUNG < 0
    )
    BEGIN
        RAISERROR (N'Không được dùng quá ngân sách được cấp cho công việc', 16, 1);
        ROLLBACK TRANSACTION;
    END;
END;

-- KIỂM TRA CÔNG VIỆC ĐÚNG CHUYÊN MÔN CỦA NHÂN VIÊN
CREATE OR ALTER TRIGGER Check_PhanCong_ChuyenMon
ON CONGVIEC
AFTER INSERT, UPDATE
AS
BEGIN
    -- Kiểm tra phân công công việc đúng với chuyên môn của nhân viên
    IF EXISTS (
        SELECT 1
        FROM inserted i
        INNER JOIN NHANVIEN nv ON i.MANV = nv.MANV
        INNER JOIN PHANCONG p ON i.MACV = p.MACV
        WHERE nv.MACM <> i.MACM
    )
    BEGIN
        RAISERROR (N'Nhân viên phải được phân công công việc đúng với chuyên môn của mình', 16, 1);
        ROLLBACK TRANSACTION;
    END;
END;

-- KIỂM TRA TINH TRANG DU AN CÓ CANCELED HAY DELAYED HAY KHÔNG
CREATE OR ALTER TRIGGER Check_TinhTrangDuAn
ON CONGVIEC
AFTER UPDATE, INSERT, DELETE
AS
BEGIN
    -- Kiểm tra tình trạng của dự án
    IF EXISTS (
        SELECT 1
        FROM inserted i
        INNER JOIN DUAN d ON i.MADA = d.MADA
        WHERE d.TINHTRANG IN ('Canceled', 'Delayed', 'Completed')
    )
    BEGIN
        RAISERROR (N'Không được cập nhật công việc khi dự án đang ở trạng thái Canceled, Completed hoặc Delayed', 16, 1);
        ROLLBACK TRANSACTION;
    END;
END;

-- KIỂM TRA TỆP ĐÍNH KÈM NẾU TIẾN ĐỘ LÀ 100% VÀ CÓ YÊU CẦU ĐÍNH KÈM
CREATE OR ALTER TRIGGER Check_TienDo_YeuCauDinhKem
ON CONGVIEC
AFTER UPDATE
AS
BEGIN
    -- Kiểm tra tiến độ và yêu cầu đính kèm
    IF EXISTS (
        SELECT 1
        FROM inserted i
        WHERE i.TIENDO = 100 AND i.YCDINHKEM IS NOT NULL AND 
		NOT EXISTS (
			SELECT 1
        	FROM [dbo].[DINHKEM] b
			WHERE i.MACV = b.MACV
		)
    )
    BEGIN
        RAISERROR (N'Cần có tệp đính kèm khi tiến độ là 100 và có yêu cầu đính kèm', 16, 1);
        ROLLBACK TRANSACTION;
    END;
END;

-- KIỂM TRA TIẾN ĐỘ (LỚN HƠN 10% TIẾN ĐỘ TRƯỚC)
CREATE OR ALTER TRIGGER Check_TienDo
ON CONGVIEC
AFTER UPDATE
AS
BEGIN
    IF EXISTS (
        SELECT 1
        FROM inserted i
        INNER JOIN deleted d ON i.MACV = d.MACV
        WHERE i.TIENDO > d.TIENDO AND i.TIENDO < d.TIENDO * 1.1
    )
    BEGIN
        RAISERROR (N'Tiến độ cập nhật sau phải lớn hơn tiến độ trước ít nhất 10%', 16, 1);
        ROLLBACK TRANSACTION;
    END;
END;

-----------UPDATE TIEN DO------------
	
CREATE OR ALTER TRIGGER trg_CapNhatTIENDO ON CONGVIEC 
AFTER INSERT, UPDATE, DELETE AS 
BEGIN 
-- Khai báo biến để lưu trữ mã dự án 
DECLARE @MADA CHAR(4); 
-- Lấy mã dự án từ các hàng được chèn, cập nhật hoặc xóa 
SELECT @MADA = MADA FROM inserted 
-- Cập nhật cột DADUNG trong bảng DUAN 
UPDATE DUAN SET TIENDO = ( SELECT AVG(TIENDO) FROM CONGVIEC WHERE CONGVIEC.MADA = @MADA ) WHERE DUAN.MADA = @MADA; 
END;
-----------TAO TAI KHOAN-------------
CREATE PROCEDURE proc_tao_tai_khoan
    @maqh NVARCHAR(50),
    @email NVARCHAR(100),
    @pass NVARCHAR(100),
    @manv NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;

    -- Kiểm tra xem tài khoản đã tồn tại hay chưa
    IF EXISTS (SELECT 1 FROM TaiKhoan WHERE EMAIL = @email)
    BEGIN
        RAISERROR ('Email đã được sử dụng cho một tài khoản khác.', 16, 1)
        RETURN;
    END

    -- Thêm tài khoản mới vào bảng TaiKhoan
	INSERT INTO NhanVien (MANV) VALUES (@manv)
    INSERT INTO TaiKhoan (MAQH, EMAIL, PASS, MANV)
    VALUES (@maqh, @email, @pass, @manv);
	
    -- Thêm logic khác nếu cần

    -- Trả về thông báo thành công
    SELECT 'Tạo tài khoản thành công' AS Result;
END

------------DOI MAT KHAU-------------
CREATE PROCEDURE proc_change_password
    @Email NVARCHAR(100),
    @OldPassword NVARCHAR(100),
    @NewPassword NVARCHAR(100)
AS
BEGIN
    DECLARE @UserID NVARCHAR(6)

    -- Kiểm tra xem email và mật khẩu cũ có khớp không
    SELECT @UserID = MANV
    FROM TaiKhoan
    WHERE Email = @Email AND PASS = @OldPassword

    IF @UserID IS NULL
    BEGIN
        -- Trả về thông báo lỗi nếu thông tin không khớp
        SELECT 'Mật khẩu cũ không chính xác' AS Result
        RETURN
    END

    -- Cập nhật mật khẩu mới cho tài khoản
    UPDATE TaiKhoan
    SET PASS = @NewPassword
    WHERE MANV = @UserID

    -- Trả về thông báo thành công
    SELECT 'Đổi mật khẩu thành công' AS Result
END
