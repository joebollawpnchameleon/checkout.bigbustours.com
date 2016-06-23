USE BigBus

IF NOT EXISTS(SELECT * FROM sys.columns
WHERE Name = N'ExternalCoupon' AND OBJECT_ID = OBJECT_ID(N'tb_Order'))
BEGIN
	ALTER TABLE tb_Order add ExternalCoupon NVARCHAR(100) NULL
	PRINT 'Your Column ExternalCoupon has been created in table tb_Order'
END 