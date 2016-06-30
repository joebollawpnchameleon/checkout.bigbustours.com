USE BigBus

IF NOT EXISTS (
  SELECT 1 
  FROM   sys.columns 
  WHERE  object_id = OBJECT_ID(N'[dbo].[tb_Ticket]') 
         AND name = 'NCEcrProductCode'
)
BEGIN
	
	ALTER TABLE dbo.tb_Ticket
	ADD NCEcrProductCode NVARCHAR(50)

END
