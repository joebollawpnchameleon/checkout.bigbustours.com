

DECLARE @EngTemplateId UNIQUEIDENTIFIER
DECLARE @DeuTemplateId UNIQUEIDENTIFIER
DECLARE @SpaTemplateId UNIQUEIDENTIFIER
DECLARE @HunTemplateId UNIQUEIDENTIFIER
DECLARE @FraTemplateId UNIQUEIDENTIFIER
DECLARE @CmnTemplateId UNIQUEIDENTIFIER
DECLARE @YueTemplateId UNIQUEIDENTIFIER
DECLARE @EngNYTemplateId UNIQUEIDENTIFIER
DECLARE @SpaNYTemplateId UNIQUEIDENTIFIER
DECLARE @DeuNYTemplateId UNIQUEIDENTIFIER

SELECT TOP 1 @EngTemplateId = Id FROM dbo.tb_EmailTemplate WHERE Name = 'Order Confirmation Eng Standard'
SELECT TOP 1 @DeuTemplateId = Id FROM dbo.tb_EmailTemplate WHERE Name = 'Order Confirmation Deu Standard'
SELECT TOP 1 @SpaTemplateId = Id FROM dbo.tb_EmailTemplate WHERE Name = 'Order Confirmation Spanish Standard'
SELECT TOP 1 @HunTemplateId = Id FROM dbo.tb_EmailTemplate WHERE Name = 'Order Confirmation Hungarian Standard'
SELECT TOP 1 @FraTemplateId = Id FROM dbo.tb_EmailTemplate WHERE Name = 'Order Confirmation French Standard'
SELECT TOP 1 @CmnTemplateId = Id FROM dbo.tb_EmailTemplate WHERE Name = 'Order Confirmation Mandarin Standard'
SELECT TOP 1 @YueTemplateId = Id FROM dbo.tb_EmailTemplate WHERE Name = 'Order Confirmation Cantonese Standard'

SELECT TOP 1 @EngNYTemplateId = Id FROM dbo.tb_EmailTemplate WHERE Name = 'Order Confirmation Eng New York'
SELECT TOP 1 @SpaNYTemplateId = Id FROM dbo.tb_EmailTemplate WHERE Name = 'Order Confirmation Spanish NewYork'
SELECT TOP 1 @DeuNYTemplateId = Id FROM dbo.tb_EmailTemplate WHERE Name = 'Order Confirmation Deu New York'

INSERT INTO dbo.tb_Microsite_Email_Template
        ( 
          MicrositeId ,
          EmailTemplateId ,
          LanguageId ,
          Created ,
          TrustPilotLink ,
          TripAdvisorLink
        )
SELECT 'abudhabi', @EngTemplateId, 'eng', GETDATE(), 'https://www.trustpilot.com/review/bigbustours.com/abudhabi', 'https://www.tripadvisor.co.uk/Attraction_Review-g294013-d2269779-Reviews-Big_Bus_Tours_Abu_Dhabi-Abu_Dhabi_Emirate_of_Abu_Dhabi.html'
UNION
SELECT 'abudhabi', @DeuTemplateId, 'deu', GETDATE(), 'https://de.trustpilot.com/review/bigbustours.com/abudhabi', 'https://www.tripadvisor.de/Attraction_Review-g294013-d2269779-Reviews-Big_Bus_Tours_Abu_Dhabi-Abu_Dhabi_Emirate_of_Abu_Dhabi.html'
UNION
SELECT 'Budapest', @EngTemplateId, 'eng', GETDATE(), 'https://www.trustpilot.com/review/bigbustours.com/budapest', 'https://www.tripadvisor.co.uk/Attraction_Review-g274887-d3444302-Reviews-Big_Bus_Tours_Hop_On_Hop_Off_Sightseeing-Budapest_Central_Hungary.html'
UNION 
SELECT 'Budapest', @HunTemplateId, 'hun', GETDATE(), 'https://www.trustpilot.com/review/bigbustours.com/budapest',  'https://www.tripadvisor.co.hu/Attraction_Review-g274887-d3444302-Reviews-Big_Bus_Tours_Hop_On_Hop_Off_Sightseeing-Budapest_Central_Hungary.html'
UNION
SELECT 'Chicago', @EngTemplateId, 'eng', GETDATE(), 'https://uk.trustpilot.com/review/bigbustours.com/chicago','https://www.tripadvisor.com/Attraction_Review-g35805-d8298701-Reviews-Big_Bus_Tours_Chicago-Chicago_Illinois.html'
UNION
SELECT 'Dubai', @EngTemplateId, 'eng', GETDATE(), 'https://uk.trustpilot.com/review/bigbustours.com/dubai', 'https://www.tripadvisor.co.uk/Attraction_Review-g295424-d1569695-Reviews-Big_Bus_Tours-Dubai_Emirate_of_Dubai.html'
UNION
SELECT 'Dubai', @DeuTemplateId, 'deu', GETDATE(), 'https://de.trustpilot.com/review/bigbustours.com/dubai', 'https://www.tripadvisor.de/Attraction_Review-g295424-d1569695-Reviews-Big_Bus_Tours-Dubai_Emirate_of_Dubai.html'
UNION
SELECT 'hongkong', @EngTemplateId, 'eng', GETDATE(), 'https://uk.trustpilot.com/review/bigbustours.com/hongkong', 'https://www.tripadvisor.co.uk/Attraction_Review-g294217-d1775455-Reviews-Big_Bus_Tours_Hong_Kong-Hong_Kong.html'
UNION
SELECT 'hongkong', @CmnTemplateId, 'cmn', GETDATE(), 'https://uk.trustpilot.com/review/bigbustours.com/hongkong','https://cn.tripadvisor.com/Attraction_Review-g294217-d1775455-Reviews-Big_Bus_Tours_Hong_Kong-Hong_Kong.html'
UNION
SELECT 'hongkong', @YueTemplateId, 'yue', GETDATE(), 'https://uk.trustpilot.com/review/bigbustours.com/hongkong', 'http://www.tripadvisor.cn/Attraction_Review-g294217-d1775455-Reviews-Big_Bus_Tours_Hong_Kong-Hong_Kong.html'
UNION
SELECT 'istanbul', @EngTemplateId, 'eng', GETDATE(), 'https://uk.trustpilot.com/review/bigbustours.com/istanbul', 'https://www.tripadvisor.co.uk/Attraction_Review-g293974-d2312245-Reviews-Big_Bus_Tours_Istanbul-Istanbul.html'
UNION
SELECT 'lasvegas', @EngTemplateId, 'eng',GETDATE(), 'https://uk.trustpilot.com/review/bigbustours.com/lasvegas', 'https://www.tripadvisor.co.uk/Attraction_Review-g45963-d1951312-Reviews-Big_Bus_Tours-Las_Vegas_Nevada.html'
UNION
SELECT 'london', @EngTemplateId, 'eng', GETDATE(), 'https://uk.trustpilot.com/review/www.bigbustours.com', 'https://www.tripadvisor.co.uk/Attraction_Review-g186338-d187581-Reviews-Big_Bus_Tours_London-London_England.html'
UNION
SELECT 'london', @FraTemplateId, 'fra', GETDATE(), 'https://fr.trustpilot.com/review/www.bigbustours.com', 'https://www.tripadvisor.fr/Attraction_Review-g186338-d187581-Reviews-Big_Bus_Tours_London-London_England.html'
UNION
SELECT 'london', @SpaTemplateId, 'spa',GETDATE(), 'https://es.trustpilot.com/review/www.bigbustours.com', 'https://www.tripadvisor.es/Attraction_Review-g186338-d187581-Reviews-Big_Bus_Tours_London-London_England.html'
UNION 
SELECT 'london', @DeuTemplateId, 'deu', GETDATE(), 'https://de.trustpilot.com/review/www.bigbustours.com', 'https://www.tripadvisor.de/Attraction_Review-g186338-d187581-Reviews-Big_Bus_Tours_London-London_England.html'
UNION
SELECT 'miami', @EngTemplateId, 'eng',GETDATE(), 'https://uk.trustpilot.com/review/bigbustours.com/miami', 'https://www.tripadvisor.co.uk/Attraction_Review-g34438-d2236420-Reviews-Big_Bus_Tours-Miami_Florida.html'
UNION
SELECT 'muscat', @EngTemplateId, 'eng', GETDATE(), 'https://uk.trustpilot.com/review/bigbustours.com/abudhabi', 'https://www.tripadvisor.co.uk/Attraction_Review-g1940497-d3735608-Reviews-Big_Bus_Tours_Muscat-Muscat_Muscat_Governorate.html'
UNION
SELECT 'newyork', @EngNYTemplateId, 'eng', GETDATE(), 'https://uk.trustpilot.com/review/bigbustours.com/newyork', 'https://www.tripadvisor.co.uk/Attraction_Review-g60763-d2173651-Reviews-Big_Bus_Tours_New_York-New_York_City_New_York.html'
UNION
SELECT 'newyork', @SpaNYTemplateId, 'spa', GETDATE(), 'https://es.trustpilot.com/review/bigbustours.com/newyork','https://www.tripadvisor.es/Attraction_Review-g60763-d2173651-Reviews-Big_Bus_Tours_New_York-New_York_City_New_York.html'
UNION
SELECT 'newyork', @DeuNYTemplateId, 'deu',GETDATE(),'https://de.trustpilot.com/review/bigbustours.com/newyork', 'https://www.tripadvisor.de/Attraction_Review-g60763-d2173651-Reviews-Big_Bus_Tours_New_York-New_York_City_New_York.html'
UNION
SELECT 'paris', @EngTemplateId, 'eng', GETDATE(), 'https://uk.trustpilot.com/review/bigbustours.com/paris', 'https://www.tripadvisor.co.uk/Attraction_Review-g187147-d1008070-Reviews-Big_Bus_Paris-Paris_Ile_de_France.html'
UNION
SELECT 'paris', @FraTemplateId, 'fra', GETDATE(), 'https://fr.trustpilot.com/review/bigbustours.com/paris', 'https://www.tripadvisor.fr/Attraction_Review-g187147-d1008070-Reviews-Big_Bus_Paris-Paris_Ile_de_France.html'
UNION
SELECT 'paris', @SpaTemplateId, 'spa',GETDATE(), 'https://es.trustpilot.com/review/bigbustours.com/paris', 'https://www.tripadvisor.es/Attraction_Review-g187147-d1008070-Reviews-Big_Bus_Paris-Paris_Ile_de_France.html'
UNION 
SELECT 'paris', @DeuTemplateId, 'deu', GETDATE(), 'https://de.trustpilot.com/review/bigbustours.com/paris', 'https://www.tripadvisor.de/Attraction_Review-g187147-d1008070-Reviews-Big_Bus_Paris-Paris_Ile_de_France.html'
UNION
SELECT 'philadelphia', @EngTemplateId, 'eng', GETDATE(), 'https://uk.trustpilot.com/review/bigbustours.com/philadelphia', 'https://www.tripadvisor.co.uk/Attraction_Review-g60795-d2167855-Reviews-Big_Bus_Tours-Philadelphia_Pennsylvania.html'
UNION
SELECT 'philadelphia', @SpaTemplateId, 'spa',GETDATE(), 'https://es.trustpilot.com/review/bigbustours.com/philadelphia', 'https://www.tripadvisor.es/Attraction_Review-g60795-d2167855-Reviews-Big_Bus_Tours-Philadelphia_Pennsylvania.html'
UNION 
SELECT 'philadelphia', @DeuTemplateId, 'deu', GETDATE(), 'https://de.trustpilot.com/review/bigbustours.com/philadelphia', 'https://www.tripadvisor.de/Attraction_Review-g60795-d2167855-Reviews-Big_Bus_Tours-Philadelphia_Pennsylvania.html'
UNION 
SELECT 'Rome', @EngTemplateId, 'eng', GETDATE(), '', ''
UNION 
SELECT 'Rome', @SpaTemplateId, 'spa', GETDATE(), '', ''
UNION 
SELECT 'Rome', @DeuTemplateId, 'deu', GETDATE(), '', ''
UNION 
SELECT 'sanfrancisco', @EngTemplateId, 'eng', GETDATE(), 'https://www.trustpilot.com/review/bigbustours.com/sanfrancisco', 'https://www.tripadvisor.co.uk/Attraction_Review-g60713-d2547559-Reviews-Big_Bus_Tours-San_Francisco_California.html'
UNION
SELECT 'sanfrancisco', @SpaTemplateId, 'spa', GETDATE(), 'https://es.trustpilot.com/review/bigbustours.com/sanfrancisco', 'https://www.tripadvisor.es/Attraction_Review-g60713-d2547559-Reviews-Big_Bus_Tours-San_Francisco_California.html'
UNION
SELECT 'sanfrancisco', @DeuTemplateId, 'ger', GETDATE(), 'https://de.trustpilot.com/review/bigbustours.com/sanfrancisco', 'https://www.tripadvisor.de/Attraction_Review-g60713-d2547559-Reviews-Big_Bus_Tours-San_Francisco_California.html'
UNION
SELECT 'shanghai', @EngTemplateId, 'eng', GETDATE(), 'https://www.trustpilot.com/review/bigbustours.com/shanghai', 'https://www.tripadvisor.co.uk/Attraction_Review-g308272-d1931592-Reviews-Big_Bus_Tours_Shanghai-Shanghai.html'
UNION
SELECT 'shanghai', @SpaTemplateId, 'spa', GETDATE(), 'https://es.trustpilot.com/review/bigbustours.com/shanghai', 'https://www.tripadvisor.es/Attraction_Review-g308272-d1931592-Reviews-Big_Bus_Tours_Shanghai-Shanghai.html'
UNION
SELECT 'shanghai', @CmnTemplateId, 'cmn', GETDATE(), 'https://www.trustpilot.com/review/bigbustours.com/shanghai', 'https://cn.tripadvisor.com/Attraction_Review-g308272-d1931592-Reviews-Big_Bus_Tours_Shanghai-Shanghai.html'
UNION
SELECT 'shanghai', @YueTemplateId, 'yue', GETDATE(), 'https://www.trustpilot.com/review/bigbustours.com/shanghai', 'http://www.tripadvisor.cn/Attraction_Review-g308272-d1931592-Reviews-Big_Bus_Tours_Shanghai-Shanghai.html'
UNION
SELECT 'shanghai', @DeuTemplateId, 'deu', GETDATE(), 'https://de.trustpilot.com/review/bigbustours.com/shanghai', 'https://www.tripadvisor.de/Attraction_Review-g308272-d1931592-Reviews-Big_Bus_Tours_Shanghai-Shanghai.html'

UNION
SELECT 'sydney', @EngTemplateId, 'eng', GETDATE(), '', 'https://www.tripadvisor.co.uk/Attraction_Review-g255060-d3616812-Reviews-Sydney_Hop_On_Hop_Off_Tour-Sydney_New_South_Wales.html'
UNION
SELECT 'sydney', @SpaTemplateId, 'spa', GETDATE(), '', 'https://www.tripadvisor.es/Attraction_Review-g255060-d3616812-Reviews-Sydney_Hop_On_Hop_Off_Tour-Sydney_New_South_Wales.html'
UNION
SELECT 'sydney', @DeuTemplateId, 'deu', GETDATE(), '', 'https://www.tripadvisor.de/Attraction_Review-g255060-d3616812-Reviews-Sydney_Hop_On_Hop_Off_Tour-Sydney_New_South_Wales.html'

UNION
SELECT 'vienna', @EngTemplateId, 'eng', GETDATE(), 'https://uk.trustpilot.com/review/bigbustours.com/vienna', 'https://www.tripadvisor.co.uk/Attraction_Review-g190454-d5993969-Reviews-Big_Bus_Vienna_GmbH-Vienna.html'
UNION
SELECT 'vienna', @SpaTemplateId, 'spa', GETDATE(), 'https://es.trustpilot.com/review/bigbustours.com/vienna', 'https://www.tripadvisor.es/Attraction_Review-g190454-d5993969-Reviews-Big_Bus_Vienna_GmbH-Vienna.html'
UNION
SELECT 'vienna', @DeuTemplateId, 'deu', GETDATE(), 'https://de.trustpilot.com/review/bigbustours.com/vienna', 'https://www.tripadvisor.de/Attraction_Review-g190454-d5993969-Reviews-Big_Bus_Vienna_GmbH-Vienna.html'


UNION
SELECT 'washington', @EngTemplateId, 'eng', GETDATE(), 'https://www.trustpilot.com/review/bigbustours.com/washington', 'https://www.tripadvisor.co.uk/Attraction_Review-g28970-d656791-Reviews-Big_Bus_Tours_Washington_DC-Washington_DC_District_of_Columbia.html'
UNION
SELECT 'washington', @SpaTemplateId, 'spa', GETDATE(), 'https://es.trustpilot.com/review/bigbustours.com/washington', 'https://www.tripadvisor.es/Attraction_Review-g28970-d656791-Reviews-Big_Bus_Tours_Washington_DC-Washington_DC_District_of_Columbia.html'
UNION
SELECT 'washington', @DeuTemplateId, 'deu', GETDATE(), 'https://de.trustpilot.com/review/bigbustours.com/washington', 'https://www.tripadvisor.de/Attraction_Review-g28970-d656791-Reviews-Big_Bus_Tours_Washington_DC-Washington_DC_District_of_Columbia.html'