USE BigBus

IF NOT EXISTS (
  SELECT 1 
  FROM   sys.columns 
  WHERE  object_id = OBJECT_ID(N'[dbo].[tb_MicroSite]') 
         AND name = 'NewCKEcrVersionId'
)
BEGIN
	
	ALTER TABLE dbo.tb_MicroSite
	ADD [NewCKEcrVersionId] INT NOT NULL DEFAULT(1)

END
