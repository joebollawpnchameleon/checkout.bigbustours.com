USE BigBus

IF EXISTS (SELECT * FROM sys.objects 
                WHERE object_id = OBJECT_ID(N'[dbo].[SP_Order_GetOrderLineEcrVersions]') 
                  AND type in (N'P', N'PC'))

DROP PROCEDURE [dbo].[SP_Order_GetOrderLineEcrVersions]
GO

CREATE PROC SP_Order_GetOrderLineEcrVersions
	@orderid UNIQUEIDENTIFIER
AS
BEGIN
	
	SELECT DISTINCT ol.Microsite_Id, ol.Id, ms.NewCKEcrVersionId, t.NCEcrProductCode, ms.UseQR, 
		ms.ECRVersion2Enabled, ms.Name as MicrositeName
	FROM dbo.tb_Orderline ol
	INNER JOIN tb_Microsite ms
		ON ms.Id = ol.Microsite_Id
	INNER JOIN dbo.tb_Ticket t
		ON t.Id = ol.Ticket_Id    
	WHERE Order_Id = @orderid

END

GO