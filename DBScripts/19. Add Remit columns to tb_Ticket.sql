USE BigBus

IF NOT EXISTS(SELECT * FROM sys.columns
WHERE Name = N'RAdultTicketEnabled' AND OBJECT_ID = OBJECT_ID(N'tb_Ticket'))
BEGIN
	ALTER TABLE tb_Ticket add RAdultTicketEnabled BIT NOT NULL DEFAULT(1)
	PRINT 'Your Column RAdultTicketEnabled has been created in table tb_Ticket'
END 

IF NOT EXISTS(SELECT * FROM sys.columns
WHERE Name = N'RChildTicketEnabled' AND OBJECT_ID = OBJECT_ID(N'tb_Ticket'))
BEGIN
	ALTER TABLE tb_Ticket add RChildTicketEnabled BIT NOT NULL DEFAULT(1)
	PRINT 'Your Column RChildTicketEnabled has been created in table tb_Ticket'
END 

IF NOT EXISTS(SELECT * FROM sys.columns
WHERE Name = N'EcrVersionId' AND OBJECT_ID = OBJECT_ID(N'tb_Ticket'))
BEGIN
	ALTER TABLE tb_Ticket add EcrVersionId INT NOT NULL DEFAULT(1)
	PRINT 'Your Column EcrVersionId has been created in table tb_Ticket'
END 