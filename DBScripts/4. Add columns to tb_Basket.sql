USE BigBus


IF NOT EXISTS(SELECT 1 FROM sys.columns WHERE  OBJECT_ID = OBJECT_ID(N'[dbo].[tb_BasketLine]') 
         AND NAME = 'ExternalOrder' )
BEGIN
	ALTER TABLE dbo.tb_BasketLine
	ADD ExternalOrder INT NULL DEFAULT(1)

	PRINT('Column ExternalOrder added to table tb_BasketLine')
END

IF NOT EXISTS(SELECT * FROM sys.columns
WHERE Name = N'ExternalCoupon' AND OBJECT_ID = OBJECT_ID(N'tb_BasketLine'))
BEGIN
	ALTER TABLE tb_BasketLine add ExternalCoupon NVARCHAR(100) NULL
	PRINT 'Your Column ExternalCoupon has been created in table tb_BasketLine'
END 

IF NOT EXISTS(SELECT * FROM sys.columns
WHERE Name = N'Price' AND OBJECT_ID = OBJECT_ID(N'tb_BasketLine'))
BEGIN
	ALTER TABLE tb_BasketLine add Price DECIMAL(18,2) NOT NULL DEFAULT 0.0
	PRINT 'Your Column Price has been created in table tb_BasketLine'
END 

IF NOT EXISTS(SELECT * FROM sys.columns
WHERE Name = N'Discount' AND OBJECT_ID = OBJECT_ID(N'tb_BasketLine'))
BEGIN
	ALTER TABLE tb_BasketLine add Discount DECIMAL(18,2) NULL
	PRINT 'Your Column Discount has been created in table tb_BasketLine'
END 

IF NOT EXISTS(SELECT * FROM sys.columns
WHERE Name = N'ExternalCoupon' AND OBJECT_ID = OBJECT_ID(N'tb_BasketLine'))
BEGIN
	ALTER TABLE tb_BasketLine add ExternalCoupon NVARCHAR(100) NULL
	PRINT 'Your Column ExternalCoupon has been created in table tb_BasketLine'
END 

IF NOT EXISTS(SELECT * FROM sys.columns
WHERE Name = N'LineTotal' AND OBJECT_ID = OBJECT_ID(N'tb_BasketLine'))
BEGIN
	ALTER TABLE tb_BasketLine add LineTotal DECIMAL(18,2) NULL
	PRINT 'Your Column LineTotal has been created in table tb_BasketLine'
END 

IF NOT EXISTS(SELECT * FROM sys.columns
WHERE Name = N'EcrProductDimensionId' AND OBJECT_ID = OBJECT_ID(N'tb_BasketLine'))
BEGIN
	ALTER TABLE tb_BasketLine add EcrProductDimensionId NVARCHAR(100) NULL
	PRINT 'Your Column EcrProductDimensionId has been created in table tb_BasketLine'
END 

IF NOT EXISTS(SELECT * FROM sys.columns
WHERE Name = N'ExternalCookieValue' AND OBJECT_ID = OBJECT_ID(N'tb_Basket'))
BEGIN
	ALTER TABLE tb_Basket add ExternalCookieValue NVARCHAR(100) NULL
	PRINT 'Your Column ExternalCookieValue has been created in table tb_Basket'
END 

IF NOT EXISTS(SELECT * FROM sys.columns
WHERE Name = N'ExternalCoupon' AND OBJECT_ID = OBJECT_ID(N'tb_Basket'))
BEGIN
	ALTER TABLE tb_Basket add ExternalCoupon NVARCHAR(100) NULL
	PRINT 'Your Column ExternalCoupon has been created in table tb_Basket'
END 

IF NOT EXISTS(SELECT * FROM sys.columns
WHERE Name = N'PurchaseLanguage' AND OBJECT_ID = OBJECT_ID(N'tb_Basket'))
BEGIN
	ALTER TABLE tb_Basket add PurchaseLanguage NVARCHAR(3) NULL
	PRINT 'Your Column PurchaseLanguage has been created in table tb_Basket'
END 

