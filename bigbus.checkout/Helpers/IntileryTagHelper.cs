using System;

namespace bigbus.checkout.Helpers
{
    public class IntileryTagHelper
    {
        public static string TrackSearch(string searchText)
        {
            try
            {
                return Environment.NewLine + "_itq.push(['_trackUserEvent', 'search', [" + Environment.NewLine +
                       @"{'name': 'Search.Text', 'value': '" + searchText + "'}" + Environment.NewLine +
                       "], 'Search' ]);" + Environment.NewLine;
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }

        public static string TrackProductView(Product product)
        {
            try
            {
                return Environment.NewLine + @"_itq.push([""_trackUserEvent"", ""view product"", [" + Environment.NewLine +
                       @"{""name"": ""Local Product.Prices"", ""value"": {" + product.ProductCost + "}" + Environment.NewLine + "}," + Environment.NewLine +
                       @"{""name"": ""Product.ID"", ""value"": """ + product.ProductId + @"""}," + Environment.NewLine +
                       @"{""name"": ""Local Product.ID"", ""value"": """ + product.ProductId + @"""}," + Environment.NewLine +
                       @"{""name"": ""Local Product.Language"", ""value"": """ + product.Language + @"""}," + Environment.NewLine +
                       @"{""name"": ""Local Product.Image"", ""value"": """ + product.ImageLink + @"""}," + Environment.NewLine +
                       @"{""name"": ""Local Product.Link"", ""value"": """ + product.ProductLink + @"""}," + Environment.NewLine +
                       @"{""name"": ""Local Product.Name"", ""value"": """ + product.Name + @"""}," + Environment.NewLine +
                       @"{""name"": ""Category.ID"", ""value"": """ + product.CategoryId + @"""}," +
                       Environment.NewLine +
                       @"{""name"": ""Local Category.ID"", ""value"": """ + product.CategoryId + @"""}," +
                       Environment.NewLine +
                       @"{""name"": ""Local Category.Name"", ""value"": """ + product.CategoryName + @"""}," + Environment.NewLine +
                       @"{""name"": ""Local Category.Language"", ""value"": """ + product.Language + @"""}," + Environment.NewLine +
                      @"{""name"": ""Brand.ID"", ""value"": """ + product.BrandId + @"""}," + Environment.NewLine +
                       @"{""name"": ""Brand.Name"", ""value"": """ + product.BrandName + @"""}" + Environment.NewLine +
                       @"], ""View Product"" ]);" + Environment.NewLine;
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }

        #region user account events

        public static string TrackUserSignIn(string email)
        {
            try
            {
                return Environment.NewLine + "_itq.push(['_trackUserEvent', 'sign in', [" + Environment.NewLine +
                       " {'name':'Customer.Email', 'value':'" + email + "'}" + Environment.NewLine +
                       "], 'Sign in']);" + Environment.NewLine;
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }

        public static string TrackUserSubscribe(string userEmail)
        {
            try
            {
                return Environment.NewLine + "_itq.push(['_trackUserEvent', 'subscribe'," + Environment.NewLine +
                       "[" + Environment.NewLine +
                       "{'name':'Customer.Email','value':'" + userEmail + "'}," + Environment.NewLine +
                       "{'name':'Customer.Subscribed','value':'true'}" + Environment.NewLine +
                       "]," + Environment.NewLine +
                       "Subscribe" + Environment.NewLine +
                       "]);" + Environment.NewLine;
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }

        public static string TrackUserSignOut(string userEmail)
        {
            try
            {
                return Environment.NewLine + "_itq.push(['_trackUserEvent', 'sign out'," + Environment.NewLine +
                    "[" + Environment.NewLine +
                        "{'name':'Customer.Email','value':'" + userEmail + "'}" + Environment.NewLine +
                    "]," + Environment.NewLine +
                    "Sign Out" + Environment.NewLine +
                    "]);";
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }

        #endregion

        #region basket events

        public static string TrackBasketItemAdd(ProductBasketLine line)
        {
            try
            {
                return Environment.NewLine + @"_itq.push([""_trackUserEvent"", ""add to basket"", [" + Environment.NewLine +
                       @"{""name"": ""Add To Basket.Price"", ""value"": """ + line.LineTotal + @"""}," + Environment.NewLine +
                       @"{""name"": ""Add To Basket.Quantity"", ""value"": """ + line.Quantity + @"""}," + Environment.NewLine +
                       @"{""name"": ""Product.ID"", ""value"": """ + line.ProductId + @"""}," + Environment.NewLine +
                       @"{""name"": ""Local Product.ID"", ""value"": """ + line.ProductId + @"""}," + Environment.NewLine +
                       @"{""name"": ""Local Product.Language"", ""value"": """ + line.ProductLanguage + @"""}," + Environment.NewLine +
                       @"{""name"": ""Local Product Variation.ID"", ""value"": """ + line.PassengerType + @"""}," + Environment.NewLine +
                       @"{""name"": ""Local Product Variation.Language"", ""value"": """ + line.ProductLanguage + @"""}," + Environment.NewLine +
                       @"{""name"": ""Local Product Variation.SKU"", ""value"": """ + line.ProductId + @"""}," + Environment.NewLine +
                       @"{""name"": ""Local Product Variation.Attributes"", ""value"": {" + Environment.NewLine +
                       @"""passenengerType"" : """ + line.PassengerType + @"""," + Environment.NewLine +
                       @"""ticketType"" : """ + line.TicketType + @"""," + Environment.NewLine +
                       @"""" + line.PassengerType + @""":""" + line.Quantity + @"""" + Environment.NewLine +
                       "}" + Environment.NewLine +
                       "}" + Environment.NewLine +
                       @"], ""Add To Basket"" ]);" + Environment.NewLine;
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }

        public static string TrackBasketItemUpdate(ProductBasketLine line)
        {
            try
            {
                return Environment.NewLine + @"_itq.push([""_trackUserEvent"", ""Update Basket"", [" + Environment.NewLine +
                       @"{""name"": ""Update Basket.Quantity"", ""value"": """ + line.Quantity + @"""}," + Environment.NewLine +
                       @"{""name"": ""Update Basket.Price"", ""value"": """ + line.LineTotal + @"""}," + Environment.NewLine +
                       @"{""name"": ""Product.ID"", ""value"": """ + line.ProductId + @"""}," + Environment.NewLine +
                       @"{""name"": ""Local Product.ID"", ""value"": """ + line.ProductId + @"""}," + Environment.NewLine +
                       @"{""name"": ""Local Product.Language"", ""value"": """ + line.ProductLanguage + @"""}," +
                       Environment.NewLine +
                       @"{""name"": ""Local Product Variation.ID"", ""value"": """ + line.PassengerType + @"""}," +
                       Environment.NewLine +
                       @"{""name"": ""Local Product Variation.Language"", ""value"": """ + line.ProductLanguage + @"""}," +
                       Environment.NewLine +
                       @"{""name"": ""Local Product Variation.SKU"", ""value"": """ + line.ProductId + @"""}" +
                       Environment.NewLine +
                       @"], ""Update Basket"" ])" + Environment.NewLine;
            }
            catch (Exception ex)
            {
                return string.Empty;
            }

        }

        public static string TrackBasketClear()
        {
            return @"_itq.push([""_trackUserEvent"", ""clear basket"", [], ""Clear Basket"" ])";
        }

        public static string TrackBasketRemoveItem(string productId, string productLanguage, string passengerType, string supplierProductCode)
        {
            try
            {
                return Environment.NewLine + @"_itq.push([""_trackUserEvent"", ""remove from basket"", [" + Environment.NewLine +
                       @"{""name"": ""Product.ID"", ""value"": """ + productId + @"""}," + Environment.NewLine +
                       @"{""name"": ""Local Product.ID"", ""value"": """ + productId + @"""}," + Environment.NewLine +
                       @"{""name"": ""Local Product.Language"", ""value"": """ + productLanguage + @"""}," + Environment.NewLine +
                       @"{""name"": ""Local Product Variation.ID"", ""value"": """ + passengerType + @"""}," + Environment.NewLine +
                       @"{""name"": ""Local Product Variation.Language"", ""value"": """ + productLanguage + @"""}," + Environment.NewLine +
                       @"{""name"": ""Local Product Variation.SKU"", ""value"": """ + productId + @"""}" + Environment.NewLine +
                       @"], ""Remove From Basket""]);";
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }

        #endregion

        #region Payment Transactions

        public static string TrackAddTrans(TransactionOrder order)
        {
            try
            {
                return
                    Environment.NewLine + @"_itq.push([""_addTrans""," +
                    @"""" + order.OrderNumber + @"""," + Environment.NewLine +
                    @"""" + order.StoreName + @"""," + Environment.NewLine +
                    @"""" + order.OrderTotal + @"""," + Environment.NewLine +
                    @"""" + order.OrderTax + @"""," + Environment.NewLine +
                    @"""" + order.ShippingFee + @"""," + Environment.NewLine +
                    @"""" + order.ShippingCity + @"""," + Environment.NewLine +
                    @"""" + order.ShippingState + @"""," + Environment.NewLine +
                    @"""" + order.Country + @"""," + Environment.NewLine +
                    "[" + Environment.NewLine +
                    @"    {""name"": ""Transaction.Currency"", ""value"": """ + order.TransactionCurrency + @"""}," + Environment.NewLine +
                    @"{""name"": ""Customer.Email"", ""value"": """ + order.Customer.Email + @"""}," + Environment.NewLine +
                    @"{""name"": ""Customer.First Name"", ""value"": """ + order.Customer.FirstName + @"""}," + Environment.NewLine +
                    @"{""name"": ""Customer.Last Name"", ""value"": """ + order.Customer.LastName + @"""}," + Environment.NewLine +
                    @"{""name"": ""Customer.Current Language"", ""value"": """ + order.Customer.Language + @"""}," + Environment.NewLine +
                    @"{""name"": ""Customer.Address 1"", ""value"": """ + order.Customer.Address1 + @"""}," + Environment.NewLine +
                    @"{""name"": ""Customer.Address 2"", ""value"": """ + order.Customer.Address2 + @"""}," + Environment.NewLine +
                    @"{""name"": ""Customer.Town"", ""value"": """ + order.Customer.Town + @"""}," + Environment.NewLine +
                    @"{""name"": ""Customer.Postcode"", ""value"": """ + order.Customer.PostCode + @"""}," + Environment.NewLine +
                    @"{""name"": ""Customer.Subscribed"", ""value"": ""false""}" + Environment.NewLine +
                    "]" + Environment.NewLine +
                    "]);" + Environment.NewLine;
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }

        public static string TrackAddTransItem(TransactionOrderItem item)
        {
            try
            {
                return Environment.NewLine + @"_itq.push([""_addItem""," + Environment.NewLine +
                       @"""" + item.OrderNumber + @"""," + Environment.NewLine +
                       @"""" + item.ProductItem.ProductSupplierCode + @"""," + Environment.NewLine +
                       @"""" + item.ProductItem.ProductName + @"""," + Environment.NewLine +
                       @"""" + item.ProductItem.TicketType + @"""," + Environment.NewLine +
                       @"""" + item.ItemPrice + @"""," + Environment.NewLine +
                       @"""" + item.ItemQuantity + @"""," + Environment.NewLine +
                       @"[" + Environment.NewLine +
                       @"{""name"": ""Product.ID"", ""value"": """ + item.ProductItem.ProductId + @"""}," + Environment.NewLine +
                       @"{""name"": ""Local Product.ID"", ""value"": """ + item.ProductItem.ProductId + @"""}," + Environment.NewLine +
                       @"{""name"": ""Local Product.Language"",  ""value"": """ + item.ProductItem.ProductLanguage + @"""}," + Environment.NewLine +
                       @"{""name"": ""Local Product Variation.ID"", ""value"": """ + item.ProductItem.PassengerType + @"""}," + Environment.NewLine +
                       @"{""name"": ""Local Product Variation.Language"", ""value"": """ + item.ProductItem.ProductLanguage + @"""}," + Environment.NewLine +
                       @"{""name"": ""Local Product Variation.SKU"", ""value"": """ + item.ProductItem.ProductId + @"""}" + Environment.NewLine +

                       "]" + Environment.NewLine +
                       "]);" + Environment.NewLine;
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }

        #endregion

        public class TransactionOrder
        {
            public string OrderNumber { get; set; }
            public string StoreName { get; set; }
            public decimal OrderTotal { get; set; }
            public decimal OrderTax { get; set; }
            public decimal ShippingFee { get; set; }
            public string ShippingCity { get; set; }
            public string ShippingState { get; set; }
            public string Country { get; set; }
            public string TransactionCurrency { get; set; }
            public TransactionCustomer Customer { get; set; }
        }

        public class TransactionOrderItem
        {
            public string OrderNumber { get; set; }
            public string ItemSku { get; set; }
            public string ItemName { get; set; }
            public string ItemCategory { get; set; }
            public decimal ItemPrice { get; set; }
            public int ItemQuantity { get; set; }
            public ProductBasketLine ProductItem { get; set; }
        }

        public class TransactionCustomer
        {
            public string Email { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Language { get; set; }
            public string Address1 { get; set; }
            public string Address2 { get; set; }
            public string PostCode { get; set; }
            public string Town { get; set; }
            public string Country { get; set; }
        }

        public class Product
        {
            public string ProductId { get; set; }

            public string Name { get; set; }

            public string Language { get; set; }

            public string ImageLink { get; set; }

            public string ProductLink { get; set; }

            public string CategoryId { get; set; }

            public string CategoryName { get; set; }

            public string BrandId { get; set; }

            public string BrandName { get; set; }

            public ProductCost ProductCost { get; set; }

        }

        public class ProductCost
        {
            public string CurrencySign { get; set; }

            public decimal AdultCost { get; set; }

            public decimal ChildCost { get; set; }

            public decimal FamilyCost { get; set; }

            public decimal RAdultCost { get; set; }

            public decimal RChildCost { get; set; }

            public decimal RFamilyCost { get; set; }

            public override string ToString()
            {
                return Environment.NewLine +
                    string.Format(
                        @"       ""adult"" :  ""{0}""," + Environment.NewLine +
                        @"       ""child"": ""{1}""," + Environment.NewLine +
                        @"       ""family"": ""{2}""" + Environment.NewLine
                       , CurrencySign + AdultCost, CurrencySign + ChildCost, CurrencySign + FamilyCost) + Environment.NewLine;
            }
        }

        public class ProductBasketLine
        {
            public string ProductName { get; set; }

            public string LineTotal { get; set; }

            public int Quantity { get; set; }

            public string ProductId { get; set; }

            public string ProductLanguage { get; set; }

            public string ProductSupplierCode { get; set; }

            public string PassengerType { get; set; }

            public string TicketType { get; set; }
        }
    }
}