USE BigBus

IF NOT EXISTS(SELECT * FROM sys.columns
WHERE Name = N'EcrProductDimensionId' AND OBJECT_ID = OBJECT_ID(N'tb_OrderLine'))
BEGIN
	ALTER TABLE tb_OrderLine add EcrProductDimensionId NVARCHAR(100) NULL
	PRINT 'Your Column EcrProductDimensionId has been created in table tb_OrderLine'
END 


IF NOT EXISTS(SELECT * FROM sys.columns
WHERE Name = N'Discount' AND OBJECT_ID = OBJECT_ID(N'tb_OrderLine'))
BEGIN
	ALTER TABLE tb_OrderLine add Discount DECIMAL(18,2) NULL
	PRINT 'Your Column Discount has been created in table tb_OrderLine'
END 


IF NOT EXISTS(SELECT * FROM sys.columns
WHERE Name = N'ExternalCoupon' AND OBJECT_ID = OBJECT_ID(N'tb_OrderLine'))
BEGIN
	ALTER TABLE tb_OrderLine add ExternalCoupon NVARCHAR(100) NULL
	PRINT 'Your Column ExternalCoupon has been created in table tb_OrderLine'
END 

IF NOT EXISTS(SELECT 1 FROM sys.columns WHERE  OBJECT_ID = OBJECT_ID(N'[dbo].[tb_OrderLine]') 
         AND NAME = 'ExternalOrder' )
BEGIN
	ALTER TABLE dbo.tb_OrderLine
	ADD ExternalOrder INT NULL DEFAULT(1)

	PRINT('Column ExternalOrder added to table tb_OrderLine')
END


IF NOT EXISTS(SELECT * FROM sys.columns
WHERE Name = N'PurchaseMicrosite' AND OBJECT_ID = OBJECT_ID(N'tb_Order'))
BEGIN
	ALTER TABLE tb_Order add PurchaseMicrosite NVARCHAR(50) NULL
	PRINT 'Your Column PurchaseMicrosite has been created in table tb_Order'
END

IF NOT EXISTS(SELECT * FROM sys.columns
WHERE Name = N'PurchaseMicrositeCurrencyCode' AND OBJECT_ID = OBJECT_ID(N'tb_Order'))
BEGIN
	ALTER TABLE tb_Order add PurchaseMicrositeCurrencyCode NVARCHAR(5) NULL
	PRINT 'Your Column PurchaseMicrositeCurrencyCode has been created in table tb_Order'
END  