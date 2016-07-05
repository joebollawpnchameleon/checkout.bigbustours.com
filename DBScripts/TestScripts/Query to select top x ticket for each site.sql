alter PROC GetEcrTicketList
AS

BEGIN
	DECLARE @SelectedTicket AS TABLE(Id UNIQUEIDENTIFIER, MSEcrVersionid int,CurrencyCode NVARCHAR(10), CurrencySymbol NVARCHAR(5))
	DECLARE @microTab AS TABLE(MicrositeId NVARCHAR(50), TicketCount INT)


	INSERT INTO @microTab
	SELECT microsite_id, COUNT(Id)
	FROM dbo.tb_Ticket 
	WHERE id IN
	(
		SELECT Id 
		FROM dbo.tb_Ticket
		WHERE EcrVersionId = 3 AND NCEcrProductCode IS NOT NULL 
	)
	GROUP BY MicroSite_Id

	--SELECT * FROM @microTab

	WHILE ((SELECT COUNT(MicrositeId) FROM @microTab) > 0)
	BEGIN
		DECLARE @currentsite NVARCHAR(50)

		SELECT TOP 1 @currentsite = MicroSiteId FROM @microTab

		DELETE FROM @microTab WHERE MicrositeId = @currentsite

		INSERT INTO @SelectedTicket(Id, MSEcrVersionid, CurrencyCode, CurrencySymbol)
		SELECT TOP 2 t.Id, ms.EcrVersionId, c.ISOCode, c.Symbol 
		FROM dbo.tb_Ticket t
		INNER JOIN dbo.tb_MicroSite ms
			ON ms.id = t.MicroSite_Id
		INNER JOIN dbo.tb_Currency c
			ON c.id = ms.Currency_Id        
		WHERE NCEcrProductCode IS NOT NULL AND
		MicroSite_Id = @currentsite AND Enabled = 1
		AND TicketType IN ('Tour')

		INSERT INTO @SelectedTicket(Id, MSEcrVersionid,CurrencyCode, CurrencySymbol)
		SELECT TOP 2 t.Id, ms.EcrVersionId, c.ISOCode, c.Symbol 
		FROM dbo.tb_Ticket t
		INNER JOIN dbo.tb_MicroSite ms
			ON ms.id = t.MicroSite_Id
		INNER JOIN dbo.tb_Currency c
			ON c.id = ms.Currency_Id        
		WHERE NCEcrProductCode IS NOT NULL AND
		MicroSite_Id = @currentsite AND Enabled = 1
		AND TicketType IN ('Attraction')
	
	END


	SELECT t.Id, t.TicketType, t.MicroSite_Id, t.Name, s.MSEcrVersionid, t.NCEcrProductCode, s.CurrencyCode, s.CurrencySymbol, 
		dbo.GetTicketSKUs(t.Id) AS DetailsCsv
	FROM dbo.tb_Ticket t
	INNER JOIN @SelectedTicket s
	ON s.Id = t.Id

END