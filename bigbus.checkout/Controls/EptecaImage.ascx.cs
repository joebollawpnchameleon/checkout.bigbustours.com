using System;
using System.Text;
using System.Web;
using System.Configuration;
using bigbus.checkout.data.Model;
using bigbus.checkout.Controls;

namespace BigBusWebsite.controls
{
    public partial class EptecaImage : BaseControl
    {
        public Boolean isEptec = false;
        public string EptecString = String.Empty;
        public Order Order { get; set; }

        public string OrderId { get; set; }
        public void Page_PreRender(object o, EventArgs a)
        {
            try
            {
                GenerateEptecaString();
            }
            catch (Exception)
            {
            }
           
        }

        private void GenerateEptecaString()
        {
            var vendorID = ConfigurationManager.AppSettings["EptecaVendorID"];
          
            if (string.IsNullOrWhiteSpace(OrderId))
            {
                string euid = String.Empty;
                string ebid = String.Empty;
                string uniqueIDstring = String.Empty;
                if (Request.Cookies["eptec"] == null)
                {
                    Guid uniqueID = Guid.NewGuid();
                    HttpCookie _eptecCookie = new HttpCookie("eptec");
                    _eptecCookie["EUID"] = Request.QueryString["EUID"].ToString();
                    _eptecCookie["EBID"] = Request.QueryString["EBID"].ToString();
                    _eptecCookie["uniqueID"] = uniqueID.ToString();
                    _eptecCookie.Expires = DateTime.Now.AddDays(30);
                    Response.Cookies.Add(_eptecCookie);

                }

                HttpCookie _eptecCookie2 = Request.Cookies["eptec"];

                if (_eptecCookie2 != null)
                {
                    euid = _eptecCookie2["EUID"].ToString();
                    ebid = _eptecCookie2["EBID"].ToString();
                    uniqueIDstring = _eptecCookie2["uniqueID"].ToString();
                }


                StringBuilder eptecStringTemp = new StringBuilder();
                eptecStringTemp.Append("//landing.epteca.com/hotlist/action/");
                eptecStringTemp.Append(uniqueIDstring + "/");
                //"http://landing.epteca.com/hotlist/action/UNIQUE_ID/USER_ID/VENDOR_ID/BRAND_ID/ACTION/PRICE/CURRENCY/";
                eptecStringTemp.Append(euid + "/" + vendorID + "/" + ebid + "/Landing/");
                isEptec = true;


                EptecString = eptecStringTemp.ToString();
            }
            else
            {
                if (Order == null)
                {
                    Order = BasePage.CheckoutService.GetFullOrder(OrderId);
                }
                if (Order != null)
                {
                    StringBuilder eptecStringTemp = new StringBuilder();
                                     

                    int orderT = Convert.ToInt16(Order.Total*100);

                    HttpCookie _eptecCookie = Request.Cookies["eptec"];
                    if (_eptecCookie != null)
                    {
                        eptecStringTemp.Append("//landing.epteca.com/hotlist/action/");
                        eptecStringTemp.Append(_eptecCookie["uniqueID"].ToString() + "/");
                        eptecStringTemp.Append(_eptecCookie["EUID"] + "/" + vendorID + "/" + _eptecCookie["EBID"] + "/Purchase/");
                        eptecStringTemp.Append(orderT);
                        eptecStringTemp.Append("/" + Order.Currency.ISOCode);
                    }

                    isEptec = true;
                    EptecString = eptecStringTemp.ToString();
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}
