
IF NOT EXISTS
(
SELECT 1
FROM sys.objects
WHERE type_desc LIKE '%CONSTRAINT' AND OBJECT_NAME(OBJECT_ID) = 'DF_ID'
)
BEGIN
	ALTER TABLE dbo.tb_ImageMetaData
	ADD CONSTRAINT DF_ID
	DEFAULT(NEWID()) FOR Id 
END


IF NOT EXISTS
(
SELECT 1
FROM sys.objects
WHERE type_desc LIKE '%CONSTRAINT' AND OBJECT_NAME(OBJECT_ID) = 'DF_ID_Address_PayPal'
)
BEGIN
	ALTER TABLE [tb_Transaction_AddressPaypal]
	ADD CONSTRAINT DF_ID_Address_PayPal
	DEFAULT(NEWID()) FOR Id 
END