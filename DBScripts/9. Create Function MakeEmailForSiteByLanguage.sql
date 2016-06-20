-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date, ,>
-- Description:	<Description, ,>
-- =============================================
Create FUNCTION  MakeEmailForSiteByLanguage(
	@CurrentMicrosite NVARCHAR(50),
	@CurrentLanguage VARCHAR(3),
	@currentPageLocation NVARCHAR(1000),
	@adobeProductsLink NVARCHAR(2000),
	@ViewVoucherLink NVARCHAR(1000),
	@AddressDetails NVARCHAR(1000)
)
RETURNS NVARCHAR(max)
AS
BEGIN 
DECLARE @OrderNumber INT
DECLARE @orderdate DATETIME
DECLARE @OrderCurrencySymbol NCHAR(5)
DECLARE @OrderTotal DECIMAL(18,2)
DECLARE @OrderTotalQuantity INT
DECLARE @OrderPaymentMethod VARCHAR(20)
DECLARE @CardLastDigits VARCHAR(5)
DECLARE @OrderUserName VARCHAR(50)
DECLARE @OrderId UNIQUEIDENTIFIER

DECLARE @OrderFriendlyEmail NVARCHAR(200)
DECLARE @UserAddressLine1 NVARCHAR(1000)
DECLARE @UserAddressLine2 NVARCHAR(1000)
DECLARE @OrderCity  NVARCHAR(100)
DECLARE @OrderPostCode  NVARCHAR(100)
DECLARE @OrderStateProvince  NVARCHAR(100)
DECLARE @OrderCountry NVARCHAR(100)
DECLARE @OrderPhoneNumber NVARCHAR(100)
DECLARE @OrderLeadTravelerName NVARCHAR(100)
DECLARE @OrderGiftTravelerName NVARCHAR(100)
DECLARE @FinalVoucherLink NVARCHAR(1000)

		
-- Grab some values


-- Get any first valid order from microsite

SELECT TOP 1 @OrderId = o.Id, @OrderNumber = o.OrderNumber,@orderdate = o.DateCreated, @OrderTotal = o.Total, @OrderTotalQuantity = o.TotalQuantity, 
@OrderCurrencySymbol = c.Symbol, @OrderPaymentMethod = o.PaymentMethod, @CardLastDigits = o.CCLast4Digits,@OrderUserName= o.UserName,
@OrderFriendlyEmail = o.eMailAddress, @UserAddressLine1 = u.AddressLine1, @UserAddressLine2 = u.AddressLine2, @OrderCity = u.City,
@OrderPostCode = u.PostCode, @OrderStateProvince = u.StateProvince, @OrderCountry = u.Country_Id, @OrderPhoneNumber = u.PhoneNumber,
@OrderLeadTravelerName = o.LeadTravelerName, @OrderGiftTravelerName = o.GiftTravelerName
FROM dbo.tb_Order o
INNER JOIN dbo.tb_Orderline ol ON ol.Order_Id = o.Id
INNER JOIN dbo.tb_Currency c ON c.id = o.Currency_Id
INNER JOIN dbo.tb_User u ON u.Id = o.User_Id
WHERE o.Currency_Id IS NOT NULL AND o.User_Id IS NOT NULL AND o.OrderNumber IS NOT NULL


SET @FinalVoucherLink = REPLACE(@ViewVoucherLink, '{Language_Id}', @CurrentLanguage)
SET @FinalVoucherLink = REPLACE(@FinalVoucherLink, '{Microsite_Id}', @CurrentMicrosite)
SET @FinalVoucherLink = REPLACE(@FinalVoucherLink, '{Order_Id}', @OrderId)

DECLARE @sql nvarchar(MAX)
SET @sql =''

SELECT @sql =
		  dbo.GetTranslation('email_Your_trip_with_BigBus_has_been_booked', @CurrentLanguage) + ' (' +  dbo.GetTranslation('email_Order_number', @CurrentLanguage) + ': ' + CAST(@OrderNumber AS VARCHAR(20)) + ')' + Char(13) + Char(13) +
          dbo.GetTranslation('email_To_view_and_print_your_e-voucher_you_will_need_Adobe_Acrobat_Reader', @CurrentLanguage) + ' ' + @adobeProductsLink + Char(13) + Char(13) +
          dbo.GetTranslation('email_To_view_your_e-voucher', @CurrentLanguage) + Char(13) +@FinalVoucherLink + Char(13) + CHAR(13) +

          dbo.GetTranslation('email_Order_details', @CurrentLanguage) + ': ' + CHAR(13) +
          dbo.GetTranslation('email_Order_number', @CurrentLanguage) + ': ' + CAST(@OrderNumber AS VARCHAR(20)) + Char(13) +
          dbo.GetTranslation('email_Date_order_placed', @CurrentLanguage) + ': ' + CONVERT(NVARCHAR(20), @orderdate, 103) + CHAR(13) +
          dbo.GetTranslation('email_Total_cost_of_order', @CurrentLanguage) +': ' + @OrderCurrencySymbol + CAST(@OrderTotal AS NVARCHAR(10)) + CHAR(13) +
          dbo.GetTranslation('email_Total_tickets_purchased', @CurrentLanguage) + ': ' + CAST(@OrderTotalQuantity AS NVARCHAR(10))+ Char(13) + CHAR(13) ;


        if (@OrderPaymentMethod = 'CreditCard')        
             SET @sql = @sql + dbo.GetTranslation('email_Last_four_digits_of_Credit_Card', @CurrentLanguage) + ': ' + @CardLastDigits + CHAR(13)
        
        ELSE if @OrderPaymentMethod = 'DebitCard'        
            SET  @sql = @sql + dbo.GetTranslation('email_Last_four_digits_of_Dedit_Card', @CurrentLanguage) + ': ' + @CardLastDigits + CHAR(13)
        
        ELSE IF (ISNULL(@CardLastDigits, '') <> '')        
             SET  @sql = @sql + dbo.GetTranslation('email_Last_four_digits_of_Credit_Card', @CurrentLanguage) + ': ' + @CardLastDigits + CHAR(13)
        
				
         SET  @sql = @sql +  dbo.GetTranslation('email_Name', @CurrentLanguage) + ': ' + @OrderUserName + CHAR(13) +
			  dbo.GetTranslation('email_User_email_address', @CurrentLanguage) + ': ' + @OrderFriendlyEmail + Char(13) + CHAR(13) +

			  dbo.GetTranslation('email_User_address', @CurrentLanguage) + ': ' + @UserAddressLine1 + CHAR(13) +
				@UserAddressLine2 + CHAR(13) +  
				@OrderCity + CHAR(13) +
				@OrderPostCode + CHAR(13) +
				@OrderStateProvince + CHAR(13) +
				@OrderCountry + Char(13) 

        if (ISNULL(@OrderPhoneNumber, '') <> '')        
               SET  @sql = @sql + dbo.GetTranslation('email_Telephone', @CurrentLanguage) + ': ' + @OrderPhoneNumber + Char(13) 
        

        if (ISNULL(@OrderLeadTravelerName, '') <> '')       
               SET  @sql = @sql + dbo.GetTranslation('email_Lead_traveler', @CurrentLanguage) + ': ' + @OrderLeadTravelerName + Char(13) 
        

        if (@OrderGiftTravelerName <> '')        
             SET  @sql = @sql + dbo.GetTranslation('email_Gift_ticket_for', @CurrentLanguage) + ': ' + @OrderGiftTravelerName + Char(13) 
        
		SET  @sql = @sql + dbo.GetTranslation('email_Ticket_details', @CurrentLanguage) + ': ' + CHAR(13)

		
		DECLARE @orderlinesTab AS TABLE(Id UNIQUEIDENTIFIER, TicketId UNIQUEIDENTIFIER, TicketType NVARCHAR(50), Quantity INT, TicketName NVARCHAR(100), TicketDate DATETIME2)

		Declare @lasVegas24HourTicket bit
		DECLARE @orderlineId UNIQUEIDENTIFIER, @TicketId UNIQUEIDENTIFIER, @TicketType NVARCHAR(50), 
			@Quantity INT, @TicketName NVARCHAR(100), @TicketTypeString NVARCHAR(500), @TicketDate DATETIME2

	
		INSERT INTO @orderlinesTab		        
		SELECT Id, Ticket_Id, TicketType, TicketQuantity, (SELECT Name FROM dbo.tb_Ticket WHERE Id = Ticket_Id) , TicketDate
		FROM dbo.tb_Orderline WHERE Order_Id = @OrderId

        set @lasVegas24HourTicket = 0

		
        WHILE ((SELECT COUNT(0) FROM @orderlinesTab) > 0)
		BEGIN
			SELECT TOP 1 @orderlineId = Id, @TicketId = TicketId, @TicketType = TicketType, @Quantity = Quantity, @TicketName = @TicketName FROM @orderlinesTab

			DELETE FROM @orderlinesTab WHERE Id = @orderlineId

			IF @TicketId = '2E24096E-54D8-4582-8E9C-B57BC3C2AD9E'
			BEGIN	
				SET @lasVegas24HourTicket = 1
			END
            
			SELECT @TicketName = Name FROM dbo.tb_Ticket WHERE Id = @TicketId
			 

			IF  @TicketType = 'groupadult' 
                SET @TicketTypeString = CHAR(13) +  dbo.GetTranslation('email_Group_adult', @CurrentLanguage)                    
            ELSE IF @TicketType = 'groupchild'                 
                SET @TicketTypeString = CHAR(13) +  dbo.GetTranslation('email_Group_child', @CurrentLanguage)                    
            Else
                SET @TicketTypeString = CHAR(13) + @TicketType;
                
			
			SET  @sql = @sql + ISNULL(@TicketName, '') + ' - ' + CAST(@Quantity AS VARCHAR(10)) + ' ' + @TicketType + ' tickets'

			IF @TicketDate IS NOT NULL
				SET  @sql = @sql + dbo.GetTranslation('email_Ticket_date', @CurrentLanguage) + ': ' + CONVERT(VARCHAR(10),@TicketDate, 103)
			ELSE
				SET  @sql = @sql + dbo.GetTranslation('email_Open_day_ticket', @CurrentLanguage)

			---- Ignored email Departure point & departure time, add later.
			
            SET @sql = @sql + CHAR(13)

		END          
		

		IF (@CurrentMicrosite = 'paris')        
		BEGIN
			SET @sql = @sql + CHAR(13) + dbo.GetTranslation('email_paris_pleasepresentphotoid', @CurrentLanguage) + CHAR(13) + CHAR(13)
			+ dbo.GetTranslation('email_Enjoy_your_trip', @CurrentLanguage) + CHAR(13) + CHAR(13) + dbo.GetTranslation('email_paris_ifyouhaveanyqueries', @CurrentLanguage) + ':' + CHAR(13)
		END
		ELSE
			SET @sql = @sql + CHAR(13) + dbo.GetTranslation('email_Enjoy_your_trip', @CurrentLanguage) + CHAR(13) + CHAR(13) + dbo.GetTranslation('email_If you_have_any_queries', @CurrentLanguage) + ': ' + CHAR(13)
       

	   IF (@CurrentMicrosite = 'lasvegas'    AND   @lasVegas24HourTicket  = 1)
		BEGIN
			SET @sql = @sql + CHAR(13) + 'Recieve a free second day with 24 hour day tour tickets when purchasing online. Your free second day will be issued when you redeem your ticket and start the tour. The second day must be concurrent with the first. In effect, the validity of your ticket will be extended to 48 hours.'
		END                

        -- work out address info later   emailMessage += MicrositeAddressInfo.GetAddressInfo(_currentMicroSiteId, this._translationServices, true, true, false);
		SET @sql = @sql + ISNULL(@AddressDetails, '') + CHAR(13)

        DECLARE  @newsites AS TABLE(Id VARCHAR(20))
		
		INSERT INTO @newsites
		SELECT 'abudhabi' 
		UNION SELECT 'budapest'
		UNION SELECT  'istanbul'
		UNION SELECT   'muscat'
		 UNION SELECT   'newyork'
		 UNION SELECT    'vienna'		

        IF EXISTS (SELECT 1 FROM @newsites WHERE Id = @CurrentMicrosite)
        BEGIN
            SET @sql = @sql + CHAR(13) + CHAR(13) +
				 'tp_shop: bigbustours-' + @CurrentMicrosite + '.com ' + Char(13) 
        END

        IF (@CurrentMicrosite = 'london')
        BEGIN
             SET @sql = @sql + CHAR(13) + dbo.GetTranslation('email_PleaseExchangeTicket', @CurrentLanguage) + Char(13)
        END

		

		IF (@CurrentMicrosite = 'newyork')
        BEGIN
             DECLARE @CustomNYText VARCHAR(MAX)
			 DECLARE @CirqueDuSoleilText varchar(MAX)
			 
			 SELECT @CustomNYText = dbo.GetTranslation('email_customNewyorkText', @CurrentLanguage) -- by language
			 SELECT @CirqueDuSoleilText = dbo.GetTranslation('email_customNewyorkCirqueDuSoleilText', @CurrentLanguage)  -- by language

			 IF ISNULL(@CustomNYText, '') <> ''
				SET @sql = @sql + CHAR(13) + @CustomNYText + CHAR(13)

			 IF ISNULL(@CirqueDuSoleilText, '') <> ''
				SET @sql = @sql + CHAR(13) + @CirqueDuSoleilText + CHAR(13)
        END

       
	   RETURN @sql


End