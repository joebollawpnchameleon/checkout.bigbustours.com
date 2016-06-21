USE [BigBus]

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'tb_Ticket_EcrProduct_Dimension')
BEGIN

	CREATE TABLE [dbo].[tb_Ticket_EcrProduct_Dimension](
		[Id] [UNIQUEIDENTIFIER] NOT NULL CONSTRAINT [DF_tb_Ticket_EcrProduct_Dimension_Id]  DEFAULT (NEWID()),
		[Name] [NVARCHAR](50) NOT NULL,
		[Amount] [DECIMAL](10, 2) NOT NULL,
		[CurrencyCode] [NVARCHAR](10) NOT NULL,
		[CurrencyId] [NVARCHAR](50) NOT NULL,
		[ProductDimensionUID] [NVARCHAR](50) NOT NULL,
		[ProductTypeUID] [NVARCHAR](50) NULL,
		[TicketId] [UNIQUEIDENTIFIER] NOT NULL,
		[EcrSysId] [NVARCHAR](50) NULL,
	 CONSTRAINT [PK_tb_Ticket_EcrProduct_Dimension] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]


END


