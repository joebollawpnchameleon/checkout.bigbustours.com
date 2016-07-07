

/* Create barcode prefix for all ecr tickets */

SET NOCOUNT On

DECLARE @tickettab AS TABLE(TicketId UNIQUEIDENTIFIER, TicketType NVARCHAR(50), MicroSiteId NVARCHAR(50))
DECLARE @productdimensiontab AS TABLE(Id INT IDENTITY(1,1), PassengerType NVARCHAR(50))
DECLARE @ticketid UNIQUEIDENTIFIER
DECLARE @tickettype NVARCHAR(50)
DECLARE @micrositeid NVARCHAR(50)
DECLARE @MaxPrefix NVARCHAR(10)
DECLARE @CountInsert INT
DECLARE @tab AS TABLE
        ( 
		  Id int identity(1,1),
          Microsite_Id NVARCHAR(50) ,
          Ticket_Type NVARCHAR(50),
          BarcodePrefix NVARCHAR(6) ,
          NextAvailableBarcode int ,
          NextAvailableDrBarcode int ,
          Ticket_TicketType NVARCHAR(50),
          Ticket_Id UNIQUEIDENTIFIER
        )

SET @MaxPrefix = '25100'

INSERT INTO @tickettab
        ( TicketId, TicketType, MicroSiteId )
SELECT t.Id, TicketType, t.MicroSite_Id
FROM dbo.tb_Ticket t
LEFT JOIN dbo.tb_Barcode bc
	ON bc.Ticket_Id = t.Id
WHERE NCEcrProductCode IS NOT NULL AND bc.Id IS NULL

WHILE(SELECT COUNT(TicketId) FROM @tickettab) > 0
BEGIN
	
	SELECT TOP 1 @ticketid = TicketId, @tickettype = TicketType, @micrositeid = MicroSiteId 
	FROM @tickettab

	DELETE FROM @tickettab WHERE TicketId = @ticketid

	IF @tickettype = 'Attraction'
	BEGIN
		
		INSERT INTO @tab
		SELECT   
          @micrositeid , 
          'all' ,
          0 , 
          0 , 
          0 , 
          @tickettype , 
          @ticketid 
        
	END
    ELSE IF @tickettype = 'Tour'
	BEGIN
		
		DELETE FROM @productdimensiontab

		INSERT INTO @productdimensiontab
		SELECT DISTINCT Name
		FROM dbo.tb_Ticket_EcrProduct_Dimension
		WHERE TicketId = @TicketId

		INSERT INTO @tab
		SELECT 
		 @micrositeid , 
          PassengerType ,
          0, 
          0 , 
          0 , 
          @tickettype , 
          @ticketid 
		FROM @productdimensiontab
		
	END    
    
END

update @tab set BarcodePrefix = @MaxPrefix + Id

insert into tb_barcode( 
          Microsite_Id  ,
          Ticket_Type,
          BarcodePrefix ,
          NextAvailableBarcode ,
          NextAvailableDrBarcode  ,
          Ticket_TicketType ,
          Ticket_Id 
        )
select Microsite_Id, Ticket_Type, BarcodePrefix, 0, 0, Ticket_TicketType, Ticket_Id 
FROM @tab 
ORDER BY BarcodePrefix



