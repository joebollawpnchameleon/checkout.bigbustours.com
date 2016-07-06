USE [BigBus]
GO

CREATE FUNCTION GetTicketSKUs
(
@ticketid UNIQUEIDENTIFIER
)
RETURNS NVARCHAR(MAX)
BEGIN
	DECLARE @sql NVARCHAR(max)

	SET @sql = ''

	SELECT @sql = @sql + (CASE WHEN @sql = '' THEN @sql ELSE  ',' END)  
		 + ProductTypeSku + '|' + Name  
	FROM dbo.tb_Ticket_EcrProduct_Dimension
	WHERE TicketId = @ticketid

	RETURN @sql
END
