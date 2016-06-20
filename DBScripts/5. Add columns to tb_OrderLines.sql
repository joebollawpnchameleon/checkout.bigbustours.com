USE BigBus

IF NOT EXISTS(SELECT * FROM sys.columns
WHERE Name = N'EcrProductDimensionId' AND OBJECT_ID = OBJECT_ID(N'tb_OrderLine'))
BEGIN
	ALTER TABLE tb_OrderLine add EcrProductDimensionId NVARCHAR(100) NULL
	PRINT 'Your Column EcrProductDimensionId has been created in table tb_OrderLine'
END 

