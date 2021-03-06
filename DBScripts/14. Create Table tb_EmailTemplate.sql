USE [BigBus]

IF NOT EXISTS (SELECT 1 FROM SYS.TABLES WHERE name = 'tb_EmailTemplate')
BEGIN

	CREATE TABLE [dbo].[tb_EmailTemplate](
		[Id] [UNIQUEIDENTIFIER] NOT NULL CONSTRAINT [DF_tb_EmailTemplate_Id]  DEFAULT (NEWID()),
		[LanguageId] [NVARCHAR](5) NOT NULL,
		[Name] [NVARCHAR](50) NOT NULL,
		[Title] [NVARCHAR](500) NOT NULL,
		[Created] [DATETIME] NOT NULL CONSTRAINT [DF_tb_EmailTemplate_LastModified]  DEFAULT (GETDATE()),
		[LastModified] [DATETIME] NULL,
		[ContentFile] [NVARCHAR](50) NULL,
	 CONSTRAINT [PK_tb_EmailTemplate] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]

END



