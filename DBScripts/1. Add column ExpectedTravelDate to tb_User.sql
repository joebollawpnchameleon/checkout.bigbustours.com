
IF NOT EXISTS(SELECT * FROM sys.columns
WHERE Name = N'ExpectedTravelDate' AND OBJECT_ID = OBJECT_ID(N'tb_User'))
BEGIN
	ALTER TABLE tb_User add ExpectedTravelDate DateTime NULL
	PRINT 'Your Column ExpectedTravelDate has been created in table tb_User'
END 

