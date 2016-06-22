USE BigBus

DECLARE @TranslationTab AS TABLE(PhraseId NVARCHAR(250), EnglishTerm NVARCHAR(500), FrenchTerm NVARCHAR(500), SpanishTerm NVARCHAR(500),
		HungarianTerm NVARCHAR(500), MandarinTerm NVARCHAR(500), CantoneseTerm NVARCHAR(500), GermanTerm NVARCHAR(500))

DECLARE @phraseid NVARCHAR(250)
DECLARE @EnglishTerm NVARCHAR(500)
DECLARE @FrenchTerm NVARCHAR(500)
DECLARE @SpanishTerm NVARCHAR(500)
DECLARE @HungarianTerm NVARCHAR(500)
DECLARE @MandarinTerm NVARCHAR(500)
DECLARE @CantoneseTerm NVARCHAR(500)
DECLARE @GermanTerm NVARCHAR(500)

INSERT INTO @TranslationTab
        
SELECT 'PaymentOrFillYourDetails',N'Or fill in your details below to pay by credit/debit card',N'Ou remplissez les informations ci-dessous pour payer par carte de crédit/débit',N'O rellene sus datos a continuación para pagar con tarjeta de crédito o débito',N'Vagy adja meg adatait alább hitelkártyás/bankkártyás fizetéshez',N'或在以下位置填写您的信息，以信用卡／扣帐卡付款',N'或在以下位置填寫您的資料，以信用卡／扣帳卡付款',N'Oder füllen Sie die untenstehenden Felder aus, um per Kredit-/Debitkarte zu zahlen'
UNION SELECT 'IHaveReadAndAgreedToTCPP',N'I have read and agree to the Terms and Conditions and Privacy Policy',N'J''ai lu et j''accepte les Conditions générales et la Politique de confidentialité',N'He leído y acepto los Términos y condiciones y la Política de privacidad',N'Elolvastam és elfogadom a Feltételeket és az Adatvédelmi szabályzatot',N'我已阅读及同意条款和条件与私隐政策',N'我已閱讀及同意條款及細則與私隱政策',N'Ich habe die AGB und Datenschutzerklärung gelesen und bin damit einverstanden'
UNION SELECT 'PleaseIncludeOnMarketingEmails',N'Please include me on marketing emails',N'Je souhaite recevoir vos communications de marketing',N'Deseo recibir mensajes de correo electrónico promocionales',N'Szeretnék kapni marketing e-maileket',N'请向我发送营销电邮',N'請向我發送推廣電郵',N'Bitte senden Sie mir Marketing-E-Mails'
UNION SELECT 'BackToHomePage',N'Back to homepage',N'Retour à la page d''accueil',N'Volver a la página de inicio',N'Vissza a kezdőlapra',N'回到主页',N'回到主頁',N'Zurück zur Startseite'
UNION SELECT 'DidURequestSMS',N'Did you request your tickets to be sent by SMS?',N'Avez-vous demandé à recevoir vos billets par SMS ?',N'¿Pidió que le enviaran los billetes por SMS?',N'SMS-ben kérte a jegyeit?',N'您要求以 SMS 发送您的车票吗？',N'您要求以 SMS 發送您的車票嗎？',N'Haben Sie angefordert, dass Ihres Tickets per SMS gesendet werden?'
UNION SELECT 'IfYouRequestedSMS',N'If you requested to have your tickets sent via SMS, remember that there is no need to print your tickets, just bring your phone with you with the ticket page open to show it to one of our on street staff who will issue you your ticket.',N'Si vous avez demandé à recevoir vos billets par SMS, rappelez-vous qu''il n''est pas nécessaire de les imprimer. Apportez simplement votre téléphone et montrez la page du billet électronique à l''un des membres de notre personnel, qui vous délivrera votre billet physique.',N'Si pidió que le enviaran sus billetes por SMS, recuerde que no es necesario que los imprima. Simplemente lleve su teléfono móvil con los billetes en pantalla y muéstreselos a un miembro de nuestro personal para que le expida una copia.',N'Ha azt kérte, hogy jegyeit SMS-ben küldjük el, ne feledje, hogy a jegyeket nem szükséges kinyomtatnia, elég, ha a telefonján megmutatja a jegyet bemutató oldalt a jegyet kiállító személyzetnek.',N'假如您要求以 SMS 发送车票，请紧记您无需列印车票，只需带同您的手机并向其中一位街头票务员展示开启的车票页面，票务员便可为您出票。',N'假如您要求以 SMS 發送車票，請緊記您無需列印車票，只需帶同您的電話，並向其中一位票務員展示開啟的車票頁，票務員即可為您出票。',N'Wenn Sie Ihre Tickets per SMS erhalten haben, brauchen Sie diese nicht auszudrucken. Bringen Sie einfach Ihr Handy mit und zeigen Sie unseren Mitarbeitern die SMS-Nachricht. Darauf hin erhalten Sie Ihr Ticket.'
UNION SELECT 'H1PrintUrTicket',N'Print your tickets',N'Imprimez vos billets',N'Imprima sus billetes',N'Nyomtassa ki a jegyet',N'列印车票',N'列印車票',N'Tickets drucken'
UNION SELECT 'DownloadOurApp',N'Download our app',N'Téléchargez notre application',N'Descargue nuestra aplicación',N'Töltse le alkalmazásunkat',N'下载我们的应用程序',N'下載我們的應用程式',N'Laden Sie unsere App herunter'
UNION SELECT 'EnjoyTheTour',N'Enjoy the tour',N'Profitez de notre tour',N'Disfrute del tour',N'Élvezze a túrát',N'享受您的旅程',N'享受您的旅程',N'Genießen Sie die Rundfahrt'
UNION SELECT 'Checkout_Paypal',N'Checkout With Credit Card',N'Réglez avec une carte de crédit',N'Finalizar compra con tarjeta de crédito',N'Fizetés hitelkártyával',N'以信用卡结帐',N'以信用卡結帳',N'Zur Kasse mit Kreditkarte'
UNION SELECT 'Checkout_Credit_Card',N'Checkout With Credit Card',N'Réglez avec une carte de crédit',N'Finalizar compra con tarjeta de crédito',N'Fizetés hitelkártyával',N'以信用卡结帐',N'以信用卡結帳',N'Zur Kasse mit Kreditkarte'
UNION SELECT 'Complete_Ticket_ Purchase',N'Complete Ticket Purchase',N'Terminez l''achat de vos billets',N'Completar la compra del billete',N'Jegyvásárlás befejezése',N'完成车票购买程序',N'完成車票購買程序',N'Ticketkauf abschließen'
UNION SELECT 'Session_Details_NotFound',N'Sorry we couldn''t find your session details!',N'Nous sommes désolés, nous n''avons pas retrouvé les détails de votre session !',N'¡No hemos encontrado sus datos de sesión!',N'Sajnáljuk, nem találtuk munkamenetének részleteit!',N'抱歉，我们找不到您的浏览信息！',N'抱歉，我們無法找到您的瀏覽資料！',N'Leider konnten wir die Daten für Ihren letzten Besuch nicht finden!'
UNION SELECT 'Session_Basket_NotFound',N'Sorry we couldn''t find any basket details!',N'Nous sommes désolés, nous n''avons pas retrouvé les détails de votre panier !',N'¡No hemos encontrado datos de la cesta!',N'Sajnáljuk, nem találtuk kosarának részleteit!',N'抱歉，我们找不到您的购物篮信息！',N'抱歉，我們無法找到任何購物籃資料！',N'Leider konnten wir Ihre Warenkorbinformationen nicht finden!'
UNION SELECT 'Session_Save_Failed',N'Sorry we couldn''t save your basket details!',N'Nous sommes désolés, nous n''avons pas pu enregistrer les détails de votre panier !',N'¡No hemos podido guardar los datos de su cesta!',N'Sajnáljuk, nem tudtuk menteni kosarának részleteit!',N'抱歉，我们不能储存您的购物篮信息！',N'抱歉，我們無法儲存您的購物籃資料！',N'Leider konnten wir Ihre Warenkorbinformationen nicht speichern!'
UNION SELECT 'FailedToCreateUser',N'Failed to create the user',N'La création de l''utilisateur a échoué',N'Fallo al crear el usuario',N'A felhasználó létrehozása nem sikerült',N'无法建立使用者资料',N'無法建立使用者資料',N'Fehler beim Erstellen des Benutzers'
UNION SELECT 'Booking_failed',N'Sorry failed to book ticket! Please contact our admin for more details.',N'Nous sommes désolés, la réservation de vos billets a échoué ! Pour de plus amples informations, veuillez contacter notre administration.',N'¡Fallo al reservar el billete! Póngase en contacto con el administrador para obtener más información.',N'Sajnáljuk, a jegyfoglalás nem sikerült! A részletekért lépjen kapcsolatba ügyintézőnkkel.',N'抱歉，预订车票失败！请联系我们的服务员询问详情。',N'抱歉，無法預訂車票！請聯絡我們的服務員查詢詳情。',N'Die Buchung des Tickets ist leider gescheitert! Bitte kontaktieren Sie uns für weitere Infos.'
UNION SELECT 'Ticket_Type',N'Ticket Type',N'Type de ticket',N'Tipo de billete',N'Jegy típusa',N'车票种类',N'車票種類',N'Tickettyp'
UNION SELECT 'Date',N'Date',N'Date',N'Fecha',N'Dátum',N'日期',N'日期',N'Datum'
UNION SELECT 'Lead_Name',N'Lead Name',N'Nom de l''acquéreur',N'Nombre del viajero principal',N'Fő utas neve',N'主要旅行者姓名',N'主要旅行者姓名',N'Hauptkundenname'
UNION SELECT 'Payment_Type',N'Payment Type',N'Type de paiement',N'Método de pago',N'Fizetés típusa',N'付款种类',N'付款類型',N'Zahlungsart'
UNION SELECT 'Order_Number',N'Order Number',N'Numéro de commande',N'Número de pedido',N'Rendelés száma',N'订单编号',N'訂單編號',N'Bestellnummer'
UNION SELECT 'Ticket_Price',N'Ticket Price',N'Prix du ticket',N'Precio del billete',N'Jegyár',N'车票价钱',N'車票價錢',N'Ticket-Preis'
UNION SELECT 'Order_Total',N'Order Total',N'Total de la commande',N'Total del pedido',N'Rendelés teljes összege',N'订单总金额',N'訂單總金額',N'Gesamtbestellung'
UNION SELECT 'Adult',N'Adult',N'Adulte',N'Adulto',N'Felnőtt',N'成人',N'成人',N'Erwachsener'
UNION SELECT 'Child',N'Child',N'Enfant',N'Niño',N'Gyerek',N'儿童',N'兒童',N'Kind'
UNION SELECT 'Family',N'Family',N'Famille',N'Familiar',N'Családi',N'家庭',N'家庭',N'Familie'
UNION SELECT 'ExpectedTourDate',N'Expected Bus Tour Date',N'Date prévue de votre visite en bus',N'Fecha prevista del viaje en autobús',N'Buszos túra várható dátuma',N'预定巴士游日期',N'預定巴士遊日期',N'Voraussichtlicher Termin der Rundfahrt'
UNION SELECT 'YouCanUseUrTicketAnyDay',N'You can use your ticket on any day, but knowing your expected bus tour date helps us ensure you have an enjoyable experience',N'Vous pouvez utiliser votre billet n''importe quel jour mais nous serons plus sûrs que vous vivrez une expérience agréable si vous savez à quelle date votre visite est prévue',N'Puede usar su billete cualquier día, pero conocer de antemano la fecha prevista del viaje en autobús contribuirá a que disfrute de una experiencia inolvidable',N'Jegyét bármely napon felhasználhatja, de ha ismerjük buszos túrájának várható dátumát, akkor könnyebben gondoskodhatunk a kellemes élményéről',N'您可以在任何日子使用您的车票，但知道您的预定巴士游日期有助我们确保您有愉快的体验',N'您可以在任何日期使用您的車票，但知道您的預定巴士遊日期可幫助我們確保您有愉快的體驗',N'Sie können Ihr Ticket an jedem beliebigen Tag verwenden. Ein voraussichtlicher Termin hilft uns jedoch, Ihr Reiseerlebnis so angenehm wie möglich zu gestalten'
UNION SELECT 'YourOrderDetails', 'Your Order Details', 'Détails de votre commande','Su información de pedido', 'Megrendelés részletei','您的订单信息', '您的訂單詳情','Ihre Bestelldaten Megrendelés részletei'
UNION SELECT 'ViewInBrowser', 'View this email in your browser', 'Visualiser cet e-mail dans votre navigateur', 'Ver este mensaje en su navegador', 'E-mail megtekintése a böngészőjében', '在您的浏览器中阅读此电邮', '在您的瀏覽器中閱讀此電郵', 'Diese E-Mail im Browser anzeigen'
UNION SELECT 'ContactUs', 'Contact us', 'Nous contacter', 'Contacte con nosotros', 'Kapcsolat', '联系我们', '聯絡我們', 'Kontaktieren Sie uns'
UNION SELECT 'Radult',N'Remit Adult',N'Acompte Adulte',N'Pago Parcial Anticipado Adulto',N'Felnőtt jegy részfizetése',N'预付成人',N'預付成人',N'Anzahlung Erwachsene'
UNION SELECT 'Rchild',N'Remit Child',N'Acompte Enfant',N'Pago Parcial Anticipado Niño',N'Gyermek jegy részfizetése',N'预付儿童',N'預付兒童',N'Anzahlung Kinder'
UNION SELECT 'RAdultAge16+',N'Remit (Adult 16+)',N'Acompte (Adulte 16 ans et +)',N'Pago Parcial Anticipado (Adulto 16+)',N'Részfizetés (Felnőtt 16+)',N'预付 (16 岁以上成人)',N'預付 (16 歲以上成人)',N'Anzahlung (Erwachsene 16+)'
UNION SELECT 'RChildAge5+',N'Remit Child (5 to 15)',N'Acompte Enfant (5 à 15 ans)',N'Pago Parcial Anticipado Niño (5 a 15 años)',N'Gyermek jegy részfizetése (5 -15)',N'预付儿童 (5 至 15 岁)',N'預付兒童 (5 至 15 歲)',N'Anzahlung Kinder (5 bis 15)'
UNION SELECT 'FormFieldMandatoryMessage',N'These fields are mandatory',N'Ces champs sont obligatoires',N'Estos campos son obligatorios',N'Ezek kötelező mezők',N'必填栏目',N'必填欄目',N'Diese Felder sind Pflichtfelder'
UNION SELECT 'IhavereadandagreetotheTermsandConditionsParameterised',N'I have read and agree to the {0}, and {1}',N'J''ai lu et j''accepte les {0} et la {1}',N'He leido y acepto {0}, y {1}',N'Elolvastam és elfogadom a következőket: {0}, és {1}',N'本人已阅读并同意 {0} 和 {1}',N'本人已閱讀及同意 {0} 和 {1}',N'Ich habe {0} und {1} gelesen und bin damit einverstanden'
UNION SELECT 'PrivacyPolicyandCookie',N'Privacy Policy &amp; Cookies',N'Politique de confidentialité &amp; Cookies',N'Política de Privacidad &amp; Cookies',N'Adatvédelmi nyilatkozat &amp; Cookie-k',N'私隐政策和 Cookies',N'私隱政策及 Cookies',N'Datenschutzerklärung &amp; Cookies'
UNION SELECT 'ClickDownloadPrintTickets',N'Click the link above to view, download and print your ticket. It will feature a code which will be checked by a Big Bus Tours staff member when you’re ready to board the bus.',N'Cliquez sur le lien ci-dessus pour afficher, télécharger et imprimer votre billet. Il contient un code qu’un membre du personnel Big Bus Tours vérifiera avant que vous ne montiez à bord du bus.',N'Haga clic en el enlace de abajo para ver, descargar e imprimir su billete. Contiene un código que el personal de Big Bus Tours validará cuando vaya a subir al autobús.',N'Kattintson a fenti hivatkozásra a jegy megtekintéséhez, letöltéséhez és nyomtatásához. A jegyen egy kód látható, amelyet a Big Bus Tours személyzete ellenőrizni fog a buszra való felszállás előtt.',N'点按以上连结，查看、下载和列印您的车票。这将包含一个代码，在您准备登车时 Big Bus Tours 票务员将会检查代码。',N'點按以上連結，查看、下載和列印您的車票。這將包含一個代碼，在您準備登車時 Big Bus Tours 票務員將會檢查代碼。',N'Klicken Sie auf den Link oben, um Ihr Ticket herunterzuladen und auszudrucken. Es enthält einen Code, der von einem Big Bus Tours-Mitarbeiter beim Einsteigen in den Bus kontrolliert wird.'
UNION SELECT 'ViewBusStopsDownload',N'View bus stop locations, bus timetables, city maps and great things to do in [city name] with our free app – available now from the {0} or {1}',N'Consultez l''emplacement des arrêts de bus, les horaires des bus, des plans de la ville et les choses à ne pas manquer à [city name] depuis notre application gratuite, dorénavant disponible sur {0} ou {1}',N'Revise las paradas de autobús, horarios, mapas de las ciudades y las visitas que puede hacer en [city name], con nuestra aplicación gratuita ya disponible desde el  {0} o {1}',N'Nézze meg a buszmegállók helyszíneit, a buszok menetrendjét, a várostérképeket vagy [city name] legjobb programjait ingyenes alkalmazásunkkal, amelyet innen tölthet le: {0} vagy {1}',N'利用我们的免费应用程序查看巴士站点位置、巴士班次时间表、城市地图及 [城市名称] 中的好去处，应用程序现已在 {0} 或 {1} 上提供。',N'利用我們的免費應用程式查看巴士站點位置、巴士班次時間表、城市地圖及 [城市名稱] 中的好去處，應用程式現已在 {0} 或 {1} 上提供。',N'Infos zu unseren Bushaltestellen, Busfahrpläne, Stadtpläne und Aktivitäten in [city name] erhalten Sie auf unserer kostenlosen App – jetzt im {0} oder {1} erhältlich'
UNION SELECT 'ShowYourPrintedTicket',N'Show your printed ticket to a member of staff at one of our stops or aboard the bus. They’ll issue you a receipt that you’ll be able to use to hop on the bus throughout the validity period of your ticket.',N'Montrez votre billet imprimé à l’un des membres de notre personnel à l’un de nos arrêts ou à bord du bus. Il vous remettra un reçu que vous pourrez utiliser pour monter à bord du bus pendant toute la période de validité de votre billet.',N'Muestre su billete impreso a nuestro personal en una de nuestras paradas o súba al autobús. Le darán un recibo que podrá usar para subir al autobús durante el periodo de validez de su billete.',N'Mutassa meg kinyomtatott jegyét személyzetünk egy tagjának a buszmegállóban vagy a buszon. Ő kiállít egy bizonylatot, amellyel a jegy érvényességi ideje alatt bármikor felszállhat buszainkra.',N'在我们任何一个站点或在登车时向票务员出示您的列印车票。他们将会发出收据，在您的车票有效期内您可凭收据随时登车。',N'在我們任何一個站點或在登車時向票務員出示您的列印車票。他們將會發出收據，在您的車票有效期內您可憑收據隨時登車。',N'Zeigen Sie Ihr ausgedrucktes Ticket einem Mitarbeiter an einer unserer Haltestellen oder an Bord des Busses. Sie erhalten eine Quittung, mit der Sie dann während der Gültigkeitsdauer Ihres Tickets jederzeit wieder in den Bus einsteigen können.'
UNION SELECT 'RequestTicketViaSms',N'If you requested to have your tickets sent via SMS, remember that there is no need to print your tickets, just bring your phone with you with the ticket page open to show it to one of our on street staff who will issue you your ticket.',N'Si vous avez demandé à recevoir vos billets par SMS, rappelez-vous qu''il n''est pas nécessaire de les imprimer. Apportez simplement votre téléphone et montrez la page du billet électronique à l''un des membres de notre personnel, qui vous délivrera votre billet physique.',N'Si solicitó que le enviaran sus billetes por SMS, recuerde que no es necesario que los imprima. Simplemente lleve su teléfono móvil con los billetes en pantalla y muéstreselos a un miembro de nuestro personal para que le den su billete de acceso.',N'Ha azt kérte, hogy jegyeit SMS-ben küldjük el, ne feledje, hogy a jegyeket nem szükséges kinyomtatnia, elég, ha a telefonján megmutatja a jegyet bemutató oldalt a jegyet kiállító személyzetnek.',N'假如您要求以 SMS 发送车票，请紧记您无需列印车票，只需带同您的手机并向其中一位街头票务员展示开启的车票页面，票务员便可为您出票。',N'假如您要求以 SMS 發送車票，請緊記您無需列印車票，只需帶同您的電話，並向其中一位票務員展示開啟的車票頁，票務員即可為您出票。',N'Wenn Sie Ihre Tickets per SMS erhalten haben, brauchen Sie diese nicht auszudrucken. Bringen Sie einfach Ihr Handy mit und zeigen Sie unseren Mitarbeitern die SMS-Nachricht. Darauf hin erhalten Sie Ihr Ticket.'
UNION SELECT 'H1DownloadBBTour',N'Download the Big Bus Tours mobile app and enhance your sightseeing experience.',N'Téléchargez l''application mobile Big Bus Tours pour profiter encore davantage de votre visite.',N'Descargue la aplicación para móvil de Big Bus Tour y mejore su visita turística',N'Töltse le a Big Bus Tours mobilalkalmazást, és tegye még nagyszerűbbé városnézési élményét.',N'下载 Big Bus Tours 流动应用程序，提升您的观光体验。',N'下載 Big Bus Tours 流動應用程式，提升您的觀光體驗。',N'Laden Sie die Big Bus Tours App herunter, um weitere Infos für Ihre Besichtigungstour zu erhalten.'
UNION SELECT 'H1RequestTicketViaSms',N'Did you request your tickets to be sent via SMS?',N'Avez-vous demandé à recevoir vos billets par SMS ?',N'¿Solicitó que le enviaran sus billetes por SMS?',N'SMS-ben kérte a jegyeit?',N'您是否要求透过 SMS 传送您的车票？',N'您是否要求透過 SMS 傳送您的車票？',N'Haben Sie angefordert, dass Ihre Tickets per SMS gesendet werden?'
UNION SELECT 'H1EnjoyYourTour',N'Enjoy the tour',N'Profitez de notre tour',N'Disfrute el tour',N'Élvezze a túrát',N'享受您的旅程',N'享受您的旅程',N'Genießen Sie die Rundfahrt'
UNION SELECT 'H1DownloadFreeApp',N'Download our Free App',N'Téléchargez notre application gratuite',N'Descargue nuestra Aplicación Gratuita',N'Töltse le ingyenes alkalmazásunkat',N'下载我们的免费应用程序',N'下載我們的免費應用程式',N' Laden Sie unsere kostenlose App herunter'

DECLARE @count INT

WHILE((SELECT COUNT(PhraseId) FROM @TranslationTab) > 0)
BEGIN
	
	SELECT TOP 1  @phraseid = PhraseId, 
			 @EnglishTerm = EnglishTerm ,
			 @FrenchTerm = FrenchTerm,
			 @SpanishTerm = SpanishTerm,
			 @HungarianTerm = HungarianTerm, 
			 @MandarinTerm = MandarinTerm,
			 @CantoneseTerm = CantoneseTerm,
			 @GermanTerm = GermanTerm
	FROM @TranslationTab

	DELETE FROM @TranslationTab WHERE PhraseId = @phraseid

	SELECT @count = COUNT(PhraseId) FROM @TranslationTab

	PRINT 'Count ' + CAST(@count AS VARCHAR(10)) + ' ' + @phraseid

	IF NOT EXISTS (SELECT 1 FROM tb_phrase WHERE Id = @phraseid)
	BEGIN
		INSERT INTO tb_Phrase 
		SELECT @phraseid

		INSERT INTO tb_phrase_language(Phrase_Id, Language_Id, Translation)
		SELECT @phraseid, 'eng', @EnglishTerm
		UNION
		SELECT @phraseid, 'fra', @FrenchTerm
		UNION
		SELECT @phraseid, 'spa', @SpanishTerm	
		UNION
		SELECT @phraseid, 'hun', @HungarianTerm	
		UNION
		SELECT @phraseid, 'cmn', @MandarinTerm
		UNION
		SELECT @phraseid, 'yue', @CantoneseTerm
		UNION
		SELECT @phraseid, 'deu', @GermanTerm

		PRINT('Phrase Id' + @phraseid + ' Inserted')

	END

END
