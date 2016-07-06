/* Create barcode prefix for all ecr tickets */

SET NOCOUNT On

DECLARE @tickettab AS TABLE(TicketId UNIQUEIDENTIFIER, TicketType NVARCHAR(50), MicroSiteId NVARCHAR(50))
DECLARE @productdimensiontab AS TABLE(Id INT IDENTITY(1,1), PassengerType NVARCHAR(50))
DECLARE @ticketid UNIQUEIDENTIFIER
DECLARE @tickettype NVARCHAR(50)
DECLARE @micrositeid NVARCHAR(50)
DECLARE @MaxPrefix NVARCHAR(10)
DECLARE @CountInsert INT

--SELECT @MaxPrefix  = MAX([BarcodePrefix]) FROM dbo.tb_Barcode
SET @MaxPrefix = '25100'
SET @CountInsert = 0

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
		PRINT(' Attraction - Max ' + CAST(@MaxPrefix AS NVARCHAR(20)))

		SET @MaxPrefix = @MaxPrefix + 1		

		PRINT(' Attraction - Max Incremented ' + CAST(@MaxPrefix AS NVARCHAR(20)))

		INSERT INTO dbo.tb_Barcode
        ( 
          Microsite_Id ,
          Ticket_Type ,
          BarcodePrefix ,
          NextAvailableBarcode ,
          NextAvailableDrBarcode ,
          Ticket_TicketType ,
          Ticket_Id 
        )
		SELECT   
          @micrositeid , 
          'all' ,
          @MaxPrefix , 
          0 , 
          0 , 
          @tickettype , 
          @ticketid 
        
		PRINT(' Attraction - Max - after insert ' + CAST(@MaxPrefix AS NVARCHAR(20)))
		SET @MaxPrefix = @MaxPrefix + 1		
		PRINT(' Attraction - Max - after insert:' + CAST(@MaxPrefix AS NVARCHAR(20)))
	END
    ELSE IF @tickettype = 'Tour'
	BEGIN
		
		PRINT(' Tour - max attr ' + CAST(@MaxPrefix AS NVARCHAR(20)))

		DELETE FROM @productdimensiontab

		INSERT INTO @productdimensiontab
		SELECT DISTINCT Name
		FROM dbo.tb_Ticket_EcrProduct_Dimension
		WHERE TicketId = @TicketId

		SELECT @CountInsert = COUNT(Id) FROM @productdimensiontab
		
		PRINT(' Tour - inserting: ' + CAST(@CountInsert AS NVARCHAR(20)))

		INSERT INTO dbo.tb_Barcode
        ( 
          Microsite_Id ,
          Ticket_Type ,
          BarcodePrefix ,
          NextAvailableBarcode ,
          NextAvailableDrBarcode ,
          Ticket_TicketType ,
          Ticket_Id 
        )
		SELECT 
		 @micrositeid , 
          PassengerType ,
          (@MaxPrefix + Id), 
          0 , 
          0 , 
          @tickettype , 
          @ticketid 
		FROM @productdimensiontab

		SET @MaxPrefix = @MaxPrefix + @CountInsert + 1

		PRINT(' Tour - max attr - after insert: ' + CAST(@MaxPrefix AS NVARCHAR(20)))

	END    
    
END

--SELECT *
--FROM @tickettab
--ORDER BY TicketType