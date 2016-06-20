

IF NOT EXISTS (
  SELECT 1 
  FROM   sys.columns 
  WHERE  object_id = OBJECT_ID(N'[dbo].[tb_Order]') 
         AND name = 'FromNewCheckout'
)
BEGIN
	
	ALTER TABLE dbo.tb_Order
	ADD FromNewCheckout BIT NOT NULL DEFAULT(0)

END
