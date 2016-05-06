using System;
using System.Text;
using System.Collections.Specialized;
using System.Net;
using System.IO;
using System.Web;
using Services.Infrastructure;
using Common.Model.PayPal;

namespace Services.Implementation
{
    public class PayPalService : BaseService, IPaypalService
    {
        private readonly ILoggerService _loggerService;
        private readonly bool _inTestMode;
        private readonly string _endpointurl;
        private readonly string _payPalUrl;
        private string _apiUsername;
        private string _apiPassword;
        private string _apiSignature;
        private readonly string _subject = string.Empty;
        private readonly string _bnCode = "PP-ECWizard";

        //private const string RealEndPoint = "https://api-3t.paypal.com/nvp";
        //private const string SandBoxEndPoint = "https://api-3t.sandbox.paypal.com/nvp";

        //private const string RealExpressCheckout = "https://www.paypal.com/cgi-bin/webscr?cmd=_express-checkout";
        //private const string SandBoxExpressCheckout = "https://www.sandbox.paypal.com/cgi-bin/webscr?cmd=_express-checkout";

        //private const string Cvv2 = "CVV2";
        private const string Signature = "SIGNATURE";
        private const string Pwd = "PWD";
        //private const string ACCT = "ACCT";

        private string _sessionId;

        //HttpWebRequest Timeout specified in milliseconds
        private const int Timeout = 20000;
        //private static readonly string[] SECURED_NVPS = new string[] { ACCT, CVV2, SIGNATURE, PWD };

        public PayPalService(PayPalInitStructure initStructure, ILoggerService loggerService)
        {
            _loggerService = loggerService;
            _endpointurl = initStructure.RealEndPoint;
            _payPalUrl = initStructure.ExpressCheckoutEndPoint;
            _inTestMode = initStructure.InTestMode;
            _apiUsername = initStructure.ApiUserName;
            _apiPassword = initStructure.ApiPassword;
            _apiSignature = initStructure.ApiSignature;
        }

        public void SetUserSessionId(string sessionId)
        {
            _sessionId = sessionId;
        }
        /// <summary>
        /// Sets the API Credentials
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="pwd"></param>
        /// <param name="signature"></param>
        /// <returns></returns>
        public void SetCredentials(string userid, string pwd, string signature)
        {
            _apiUsername = userid;
            _apiPassword = pwd;
            _apiSignature = signature;
        }

        public PayPalReturn ShortcutExpressCheckout(string returnUrl, string cancelUrl, string pageStyle, PayPalOrder order, bool commit)
        {
            return ShortcutExpressCheckout(returnUrl, cancelUrl, pageStyle, order, commit, null);
        }

        /// <summary>
        /// ShortcutExpressCheckout: The method that calls SetExpressCheckout API
        /// </summary>
        public PayPalReturn ShortcutExpressCheckout(string returnUrl, string cancelUrl, string pageStyle, PayPalOrder order, bool commit,
            string solutionType = null)
        {
            var encoder = new NvpCodec();
            encoder["METHOD"] = "SetExpressCheckout";
            encoder["RETURNURL"] = returnUrl;
            encoder["CANCELURL"] = cancelUrl;
            encoder["AMT"] = order.OrderTotal.ToString("N2");
            encoder["TAXAMT"] = order.OrderTaxTotal.ToString("N2");
            encoder["ITEMAMT"] = order.OrderSubTotal.ToString("N2");
            encoder["PAYMENTACTION"] = "Sale";
            encoder["CURRENCYCODE"] = order.ISOCurrencyCode;

            encoder["NOSHIPPING"] = "0";
            encoder["LOCALECODE"] = order.orderLanguage;
            encoder["REQCONFIRMSHIPPING"] = "0";
            if (!string.IsNullOrEmpty(solutionType))
                encoder["SOLUTIONTYPE"] = solutionType;

            if (!string.IsNullOrEmpty(pageStyle))
                encoder["PAGESTYLE"] = pageStyle;

            var c = 0;
            foreach (var item in order.Items)
            {
                encoder[string.Concat("L_NAME", c)] = item.ProductName;
                encoder[string.Concat("L_AMT", c)] = item.LineItemPrice.ToString("N2");
                encoder[string.Concat("L_NUMBER", c)] = item.ProductId;
                encoder[string.Concat("L_QTY", c)] = item.Quantity.ToString();
                encoder[string.Concat("L_TAXAMT", c)] = item.LineItemTax.ToString("N2");
                c++;
            }

            var pStrrequestforNvp = encoder.Encode();
            var pStresponsenvp = HttpCall(pStrrequestforNvp);

            var decoder = new NvpCodec();
            decoder.Decode(pStresponsenvp);

            var res = new PayPalReturn {AUK = decoder["ACK"].ToLower()};

            if (res.AUK != null && (res.AUK == "success" || res.AUK == "successwithwarning"))
            {
                res.Token = decoder["TOKEN"];
                res.RedirectURL = string.Concat(_payPalUrl, "&token=", res.Token, commit ? "&useraction=commit" : string.Empty);
            }
            else
            {
                res.IsError = true;
                res.ErrorMessage = decoder["L_LONGMESSAGE0"]; //decoder["L_SHORTMESSAGE0"]
                res.ErrorCode = decoder["L_ERRORCODE0"];
                LoggerService.LogItem(res.ErrorCode + Environment.NewLine + res.ErrorMessage);
            }

            return res;
        }

        public PayPalReturn ConfirmCheckoutDetails(string token)
        {
            var encoder = new NvpCodec();
            encoder["METHOD"] = "GetExpressCheckoutDetails";
            encoder["TOKEN"] = token;

            var pStrrequestforNvp = encoder.Encode();
            var pStresponsenvp = HttpCall(pStrrequestforNvp);
            //_log.Info(string.Concat("ConfirmCheckoutDetails | ", pStresponsenvp));

            var decoder = new NvpCodec();
            decoder.Decode(pStresponsenvp);

            var res = new PayPalReturn
            {
                PayPalReturnUserInfo = new PayPalReturnUserInfo(),
                AUK = decoder["ACK"].ToLower()
            };

            if (res.AUK != null && (res.AUK == "success" || res.AUK == "successwithwarning"))
            {
                res.Token = decoder["TOKEN"];
                res.PayPalReturnUserInfo.Payer_Id = decoder["PAYERID"];
                res.PayPalReturnUserInfo.CountryCode = decoder["COUNTRYCODE"];
                res.PayPalReturnUserInfo.Firstname = decoder["FIRSTNAME"];
                res.PayPalReturnUserInfo.Lastname = decoder["LASTNAME"];
                res.PayPalReturnUserInfo.PayerStatus = decoder["PAYERSTATUS"];
                res.PayPalReturnUserInfo.Email = decoder["EMAIL"];

                if (string.IsNullOrEmpty(decoder["SHIPTONAME"])) return res;

                res.PayPalReturnUserInfo.AddressInfo = new PayPalAddressInfo
                {
                    Name = decoder["SHIPTONAME"],
                    Street = decoder["SHIPTOSTREET"],
                    Street2 = decoder["SHIPTOSTREET2"],
                    City = decoder["SHIPTOCITY"],
                    State = decoder["SHIPTOSTATE"],
                    Postcode = decoder["SHIPTOZIP"],
                    CountryCode = decoder["SHIPTOCOUNTRYCODE"]
                };
            }
            else
            {
                res.IsError = true;
                res.ErrorMessage = decoder["L_LONGMESSAGE0"]; //decoder["L_SHORTMESSAGE0"]
                res.ErrorCode = decoder["L_ERRORCODE0"];
            }

            return res;
        }

        public PayPalReturn ConfirmPayment(string finalPaymentAmount, string currencyCode, string token, string PayerId)
        {
            var encoder = new NvpCodec();
            encoder["METHOD"] = "DoExpressCheckoutPayment";
            encoder["TOKEN"] = token;
            encoder["PAYMENTACTION"] = "Sale";
            encoder["PAYERID"] = PayerId;
            encoder["AMT"] = finalPaymentAmount;
            encoder["CURRENCYCODE"] = currencyCode;
            encoder["NOSHIPPING"] = "1";
            encoder["REQCONFIRMSHIPPING"] = "0";

            var pStrrequestforNvp = encoder.Encode();
            var pStresponsenvp = HttpCall(pStrrequestforNvp);
           
            var decoder = new NvpCodec();
            decoder.Decode(pStresponsenvp);

            var res = new PayPalReturn
            {
                PayPalReturnUserInfo = new PayPalReturnUserInfo(),
                AUK = decoder["ACK"].ToLower()
            };

            if (res.AUK != null && (res.AUK == "success" || res.AUK == "successwithwarning"))
            {
                res.Token = decoder["TOKEN"];
                res.PayPalReturnUserInfo.Payer_Id = decoder["PAYERID"];
                res.Transaction_Id = decoder["TRANSACTIONID"];
            }
            else
            {
                res.IsError = true;
                res.ErrorMessage = decoder["L_LONGMESSAGE0"]; //decoder["L_SHORTMESSAGE0"]
                res.ErrorCode = decoder["L_ERRORCODE0"];
            }

            return res;
        }


        /// <summary>
        /// HttpCall: The main method that is used for all API calls
        /// </summary>
        /// <param name="nvpRequest"></param>
        /// <returns></returns>
        public string HttpCall(string nvpRequest) //CallNvpServer
        {
            var url = _endpointurl;

            //To Add the credentials from the profile
            var strPost = nvpRequest + "&" + BuildCredentialsNvpString();
            strPost = strPost + "&BUTTONSOURCE=" + NvpCodec.UrlEncode(_bnCode);

            //Added for sandbox security update - Zendesk #1240
            if (_inTestMode)
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            var objRequest = (HttpWebRequest)WebRequest.Create(url);
            objRequest.Timeout = Timeout;
            objRequest.Method = "POST";
            objRequest.ContentLength = strPost.Length;

            try
            {
                using (var myWriter = new StreamWriter(objRequest.GetRequestStream()))
                {
                    myWriter.Write(strPost);
                }
            }
            catch (Exception e)
            {
                LoggerService.LogItem(e.Message);
            }

            //Retrieve the Response returned from the NVP API call to PayPal
            var objResponse = (HttpWebResponse)objRequest.GetResponse();
         
            string result;
            using (var sr = new StreamReader(objResponse.GetResponseStream()))
            {
                result = sr.ReadToEnd();
            }

            return result;
        }

        /// <summary>
        /// Credentials added to the NVP string
        /// </summary>
        /// <param name="profile"></param>
        /// <returns></returns>
        private string BuildCredentialsNvpString()
        {
            var codec = new NvpCodec();

            if (!string.IsNullOrWhiteSpace(_apiUsername))
                codec["USER"] = _apiUsername;

            if (!string.IsNullOrWhiteSpace(_apiPassword))
                codec[Pwd] = _apiPassword;

            if (!string.IsNullOrWhiteSpace(_apiSignature))
                codec[Signature] = _apiSignature;

            if (!string.IsNullOrWhiteSpace(_subject))
                codec["SUBJECT"] = _subject;

            codec["VERSION"] = "60.0";

            return codec.Encode();
        }
    }
}
