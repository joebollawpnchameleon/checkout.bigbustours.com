USE [BigBus]

IF NOT EXISTS (SELECT 1 FROM SYS.TABLES WHERE NAME ='tb_Ecr_Barcodes')
BEGIN

	CREATE TABLE [dbo].[tb_Ecr_Barcodes](
		[Id] [UNIQUEIDENTIFIER] NOT NULL,
		[DateCreated] [DATETIME] NOT NULL,
		[TicketId] [NVARCHAR](100) NOT NULL,
		[OrderNumber] [INT] NOT NULL,
		[ImageId] [UNIQUEIDENTIFIER] NULL,
	 CONSTRAINT [PK_tb_Ecr_Barcodes] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
	
	ALTER TABLE [dbo].[tb_Ecr_Barcodes] ADD  CONSTRAINT [DF_tb_Ecr_Barcodes_Id]  DEFAULT (NEWID()) FOR [Id]
	
	ALTER TABLE [dbo].[tb_Ecr_Barcodes] ADD  CONSTRAINT [DF_tb_Ecr_Barcodes_DateCreated]  DEFAULT (GETDATE()) FOR [DateCreated]


END