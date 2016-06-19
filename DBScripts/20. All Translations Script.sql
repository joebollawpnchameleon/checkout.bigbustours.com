USE BigBus

DECLARE @TranslationTab AS TABLE(PhraseId NVARCHAR(250), EnglishTerm NVARCHAR(500), FrenchTerm NVARCHAR(500), SpanishTerm NVARCHAR(500),
		HungarianTerm NVARCHAR(500), MandarinTerm NVARCHAR(500), CantoneseTerm NVARCHAR(500), GermanTerm NVARCHAR(500))

DECLARE @phraseid NVARCHAR(50)
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
