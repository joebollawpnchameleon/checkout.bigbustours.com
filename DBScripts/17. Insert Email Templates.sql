USE BigBus

DECLARE @titleTable AS TABLE(LanguageId VARCHAR(10), Title NVARCHAR(500))

-- gather titles for all template languages
INSERT INTO @titleTable
SELECT distinct Language_Id, Translation + 
	(' (' + (SELECT Translation FROM dbo.tb_Phrase_Language pl 
	WHERE pl.Language_Id = pl2.language_Id AND pl.Phrase_Id ='email_Order_number') + ': [Order_Number])' )
FROM dbo.tb_Phrase_Language pl2
WHERE Phrase_Id ='email_Your_trip_with_BigBus_has_been_booked'
AND Language_Id IN ('eng','deu','fra', 'hun', 'spa', 'cmn','yue', 'kor', 'ita')


IF NOT EXISTS(SELECT 1 FROM dbo.tb_EmailTemplate WHERE Name = 'Order Confirmation Eng Standard')
BEGIN
	INSERT INTO tb_EmailTemplate(LanguageId, Name, Title, ContentFile)
	SELECT 'eng','Order Confirmation Eng Standard', (select top 1 Title from @titleTable where LanguageId = 'eng'), 'eng-standard.html'
END

IF NOT EXISTS(SELECT 1 FROM dbo.tb_EmailTemplate WHERE Name = 'Order Confirmation Deu New York')
BEGIN
INSERT INTO tb_EmailTemplate(LanguageId, Name, Title, ContentFile)
SELECT 'deu', 'Order Confirmation Deu New York', (select top 1 Title from @titleTable where LanguageId = 'deu'), 'deu-newyork.html'
END

IF NOT EXISTS(SELECT 1 FROM dbo.tb_EmailTemplate WHERE Name = 'Order Confirmation Deu Standard')
BEGIN
INSERT INTO tb_EmailTemplate(LanguageId, Name, Title, ContentFile) 
SELECT 'deu', 'Order Confirmation Deu Standard', (select top 1 Title from @titleTable where LanguageId = 'deu'),'deu-standard.html'
END

IF NOT EXISTS(SELECT 1 FROM dbo.tb_EmailTemplate WHERE Name = 'Order Confirmation Eng New York')
BEGIN
INSERT INTO tb_EmailTemplate(LanguageId, Name, Title, ContentFile) 
SELECT 'eng', 'Order Confirmation Eng New York', (select top 1 Title from @titleTable where LanguageId = 'eng'), 'eng-newyork.html'
END

IF NOT EXISTS(SELECT 1 FROM dbo.tb_EmailTemplate WHERE Name = 'Order Confirmation French Standard')
BEGIN
INSERT INTO tb_EmailTemplate(LanguageId, Name, Title, ContentFile) 
SELECT 'fra', 'Order Confirmation French Standard', (select top 1 Title from @titleTable where LanguageId = 'fra'),'fra-standard.html'
END

IF NOT EXISTS(SELECT 1 FROM dbo.tb_EmailTemplate WHERE Name = 'Order Confirmation Hungarian Standard')
BEGIN
INSERT INTO tb_EmailTemplate(LanguageId, Name, Title, ContentFile) 
SELECT 'hun', 'Order Confirmation Hungarian Standard', (select top 1 Title from @titleTable where LanguageId = 'hun'),'hun-standard.html'
END

IF NOT EXISTS(SELECT 1 FROM dbo.tb_EmailTemplate WHERE Name = 'Order Confirmation Spanish NewYork')
BEGIN
INSERT INTO tb_EmailTemplate(LanguageId, Name, Title, ContentFile) 
SELECT 'spa', 'Order Confirmation Spanish NewYork', (select top 1 Title from @titleTable where LanguageId = 'spa'), 'spa-newyork.html'
END

IF NOT EXISTS(SELECT 1 FROM dbo.tb_EmailTemplate WHERE Name = 'Order Confirmation Spanish Standard')
BEGIN
INSERT INTO tb_EmailTemplate(LanguageId, Name, Title, ContentFile) 
SELECT 'spa', 'Order Confirmation Spanish Standard', (select top 1 Title from @titleTable where LanguageId = 'spa'), 'spa-standard.html'
END

IF NOT EXISTS(SELECT 1 FROM dbo.tb_EmailTemplate WHERE Name = 'Order Confirmation Mandarin Standard')
BEGIN
INSERT INTO tb_EmailTemplate(LanguageId, Name, Title, ContentFile) 
SELECT 'cmn', 'Order Confirmation Mandarin Standard', (select top 1 Title from @titleTable where LanguageId = 'cmn'), 'cmn-standard.html'
END

IF NOT EXISTS(SELECT 1 FROM dbo.tb_EmailTemplate WHERE Name = 'Order Confirmation Cantonese Standard')
BEGIN
	INSERT INTO tb_EmailTemplate(LanguageId, Name, Title, ContentFile) 
	SELECT 'yue', 'Order Confirmation Cantonese Standard', (select top 1 Title from @titleTable where LanguageId = 'yue'), 'yue-standard.html'
END

