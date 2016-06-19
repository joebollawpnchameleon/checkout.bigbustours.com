
UPDATE [tb_BasketLine] SET [Price]=0.0 WHERE [Price] IS NULL
ALTER TABLE tb_BasketLine ADD CONSTRAINT [tb_BasketLine_NotNullPrice] DEFAULT 0.0 FOR [Price]
ALTER TABLE tb_BasketLine ALTER COLUMN [Price] decimal NOT NULL
