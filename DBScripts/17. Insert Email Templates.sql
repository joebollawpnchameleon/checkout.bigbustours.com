
DECLARE @titleTable AS TABLE(LanguageId VARCHAR(10), Title NVARCHAR(500))

-- gather titles for all template languages
INSERT INTO @titleTable
SELECT distinct Language_Id, Translation + 
	(' (' + (SELECT Translation FROM dbo.tb_Phrase_Language pl 
	WHERE pl.Language_Id = pl2.language_Id AND pl.Phrase_Id ='email_Order_number') + ': [Order_Number])' )
FROM dbo.tb_Phrase_Language pl2
WHERE Phrase_Id ='email_Your_trip_with_BigBus_has_been_booked'
AND Language_Id IN ('eng','deu','fra', 'hun', 'spa', 'cmn','yue', 'kor', 'ita')

--select * from @titleTable


INSERT INTO tb_EmailTemplate(LanguageId, Name, Title, ContentFile)
SELECT 'eng','Order Confirmation Eng Standard', (select top 1 Title from @titleTable where LanguageId = 'eng'), 'eng-standard.html'
UNION
SELECT 'deu', 'Order Confirmation Deu New York', (select top 1 Title from @titleTable where LanguageId = 'deu'), 'deu-newyork.html'
UNION
SELECT 'deu', 'Order Confirmation Deu Standard',(select top 1 Title from @titleTable where LanguageId = 'deu'),'deu-standard.html'
UNION
SELECT 'eng', 'Order Confirmation Eng New York', (select top 1 Title from @titleTable where LanguageId = 'eng'), 'eng-newyork.html'
UNION
SELECT 'fra', 'Order Confirmation French Standard', (select top 1 Title from @titleTable where LanguageId = 'fra'),'fra-standard.html'
UNION
SELECT 'hun', 'Order Confirmation Hungarian Standard', (select top 1 Title from @titleTable where LanguageId = 'hun'),'hun-standard.html'
UNION
SELECT 'spa', 'Order Confirmation Spanish NewYork', (select top 1 Title from @titleTable where LanguageId = 'spa'), 'spa-newyork.html'
UNION
SELECT 'spa', 'Order Confirmation Spanish Standard', (select top 1 Title from @titleTable where LanguageId = 'spa'), 'spa-standard.html'
UNION
SELECT 'cmn', 'Order Confirmation Mandarin Standard', (select top 1 Title from @titleTable where LanguageId = 'cmn'), 'cmn-standard.html'
UNION
SELECT 'yue', 'Order Confirmation Cantonese Standard', (select top 1 Title from @titleTable where LanguageId = 'yue'), 'yue-standard.html'
