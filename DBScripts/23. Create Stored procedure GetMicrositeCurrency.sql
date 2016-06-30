USE BigBus

IF EXISTS (SELECT * FROM sys.objects 
                WHERE object_id = OBJECT_ID(N'[dbo].[GetMicrositeCurrency]') 
                  AND type in (N'P', N'PC'))

DROP PROCEDURE [dbo].[GetMicrositeCurrency]
GO

Create PROC GetMicrositeCurrency

	@micrositeid NVARCHAR(50)
AS

BEGIN

	SELECT TOP 1 * 
	FROM dbo.tb_Currency C
	INNER JOIN tb_MicroSite M
		ON M.Currency_Id = C.Id AND M.Id = @micrositeid

END