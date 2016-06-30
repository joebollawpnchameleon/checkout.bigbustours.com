USE BigBus

IF NOT EXISTS(SELECT * FROM sys.columns
WHERE Name = N'ConversionRate' AND OBJECT_ID = OBJECT_ID(N'tb_Currency'))
BEGIN
	ALTER TABLE tb_Currency add ConversionRate DECIMAL(18,2) NOT NULL DEFAULT(1.0)
	PRINT 'Your Column ConversionRate has been created in table tb_Currency'
END 
