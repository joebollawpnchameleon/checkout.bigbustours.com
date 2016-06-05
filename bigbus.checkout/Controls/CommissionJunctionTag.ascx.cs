using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using bigbus.checkout.data.Model;


namespace bigbus.checkout.Controls
{
    public partial class CommissionJunctionTag : BaseControl
    {
        public string CommissionJunctionString = String.Empty;
        public Order Order { get; set; }
        private string actionID = "384664";//default london value
        private string containerTagID = "13623"; //default london value
        //public string OrderId { get; set; }

        public void Page_PreRender(object o, EventArgs a)
        {
            try
            {
                GenerateCJString();
            }
            catch (Exception ex)
            {
                //ignore
            }

        }

        private void GenerateCJString()
        {

            var orderId = Request["id"];
            if (Order == null)
                {
                    Order = BasePage.CheckoutService.GetFullOrder(orderId);
                }
            setActionAndContainerTagIDPerCity();
            var cjString = new StringBuilder();
            cjString.Append("https://www.emjcd.com/tags/c?containerTagId=");

            //Itemx = 1 as only tickets we sell , if needs to change, later update
           cjString.Append(containerTagID);
            cjString.Append("&ITEM1=1");

            //check as amount can be differnt for differnt tikcets, we
            cjString.Append("&AMT1=");
            cjString.Append(Order.Total);
            //by default taking 1 as qty
            cjString.Append("&QTY1=1");
            cjString.Append("&CID=1537042");
            cjString.Append("&OID=");
            cjString.Append(Order.OrderNumber);
            cjString.Append("&TYPE=");
            cjString.Append(actionID);
            cjString.Append("&CURRENCY=");
            cjString.Append(Order.Currency.ISOCode);
            cjString.Append("&DISCOUNT=0");
            CommissionJunctionString = cjString.ToString();

        }
        // hardcoded Containter tag id and Action ID from Commission junction
        private void setActionAndContainerTagIDPerCity()
        {

            switch (BasePage.CurrentSite.Id)
            {
                case "abudhabi":
                    actionID ="384665";
                    containerTagID ="13624";
                    break;
                case "budapest":
                     actionID ="384666";
                    containerTagID ="13625";
                    break;
                case "dubai":
                    actionID = "384667";
                    containerTagID = "13626";
                    break;
                case "chicago":
                    actionID = "384668";
                    containerTagID = "13627";
                    break;
                case "hongkong":
                    actionID = "384669";
                    containerTagID = "13628";
                    break;
                case "istanbul":
                    actionID = "384670";
                    containerTagID = "13629";
                    break;
                case "lasvegas":
                    actionID = "384671";
                    containerTagID = "13630";
                    break;
                case "miami":
                    actionID = "384672";
                    containerTagID = "13631";
                    break;
                case "muscat":
                    actionID = "384673";
                    containerTagID = "13632";
                    break;
                case "newyork":
                    actionID = "384715";
                    containerTagID = "13633";
                    break;
                case "sanfrancisco":
                    actionID = "384716";
                    containerTagID = "13634";
                    break;
                case "vienna":
                    actionID = "384717";
                    containerTagID = "13635";
                    break;
                case "washington":
                    actionID = "384718";
                    containerTagID = "13636";
                    break;
            }
        }

    }
}
