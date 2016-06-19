

IF NOT EXISTS (
  SELECT 1 
  FROM   sys.columns 
  WHERE  object_id = OBJECT_ID(N'[dbo].[tb_Email]') 
         AND name = 'OrderId'
)
BEGIN
	
	ALTER TABLE dbo.tb_Email
	ADD OrderId NVARCHAR(50) NULL

END
