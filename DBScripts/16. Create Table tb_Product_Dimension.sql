USE [BigBus]


IF NOT EXISTS(SELECT 1 FROM sys.tables where NAME ='tb_Product_Dimension')
BEGIN

	CREATE TABLE [dbo].[tb_Product_Dimension](
		[Id] [UNIQUEIDENTIFIER] NOT NULL,
		[TicketId] [UNIQUEIDENTIFIER] NOT NULL,
		[DateCreated] [DATETIME] NOT NULL,
		[ProductDimensionUID] [NVARCHAR](100) NOT NULL,
	 CONSTRAINT [PK_tb_Product_Dimension] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]

	
	ALTER TABLE [dbo].[tb_Product_Dimension] ADD  CONSTRAINT [DF_tb_Product_Dimension_Id]  DEFAULT (NEWID()) FOR [Id]
	
	ALTER TABLE [dbo].[tb_Product_Dimension] ADD  CONSTRAINT [DF_tb_Product_Dimension_DateCreated]  DEFAULT (GETDATE()) FOR [DateCreated]
	


END