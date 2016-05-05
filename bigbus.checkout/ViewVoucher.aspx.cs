//using System;
//using BigBus.BusinessObjects;
//using BigBus.helpers;
//using BigBusWebsite;
//using BigBusWebsite.Classes.Helpers;

using bigbus.checkout.Models;

public partial class viewevoucher : BasePage
{
    //Log _log;
    //private Session _thisSession;

    //public override bool MustBeSecure
    //{
    //    get
    //    {
    //        return true;
    //    }
    //}

    //public override bool PreventBrowserAndProxyCaching
    //{
    //    get
    //    {
    //        return true;
    //    }
    //}

    //private string _thisOrderId;

    //public string ThisOrderId
    //{
    //    get
    //    {
    //        try
    //        {
    //            if (_thisOrderId == null)
    //            {
    //                if (!string.IsNullOrEmpty(Request["Id"]))
    //                {
    //                    _thisOrderId = Request["Id"];
    //                }
    //            }
    //        }
    //        catch
    //        {
    //            Response.Redirect(BBURL_Insecure("default.aspx"));
    //        }

    //        return _thisOrderId;
    //    }
    //}


    //protected override void Page_Load(object sender, EventArgs eventArgs)
    //{
    //    base.Page_Load(sender, eventArgs);

    //    //add no follow meta
    //    try
    //    {
    //        var master = (layout_microsite)this.Master;
    //        if (master != null) master.AddMetas(@"<meta name=""robots"" content=""noindex"">");
    //    }
    //    catch
    //    {
    //        //ignore
    //    }

    //    TestPdfVoucher();
    //    evoucherviewedtext.SiteText_Id = Subsite + ":evoucherviewedtext";
    //    var thisOrder = GetObjectFactory().GetByOQL<Order>("?(*)Order(Id = $p0$)", ThisOrderId);

    //    if (thisOrder == null)
    //    {
    //        Response.Redirect(BBURL_Insecure("default.aspx"));
    //    }
    //    else
    //    {
    //        try
    //        {
    //            _thisSession = GetSession() as Session;

    //            var validAgent =
    //                _thisSession != null &&
    //                _thisSession.User != null &&
    //                _thisSession.IsLoggedInUser &&
    //                _thisSession.User.IsAgent;

    //            var url =
    //                string.Concat(
    //                    SiteUrl("voucher.aspx"),
    //                    SiteUrl("voucher.aspx").Contains("?") ? "&Id=" : "?Id=",
    //                    ThisOrderId,
    //                    (!string.IsNullOrEmpty(Request["ap"]) && validAgent ? "&ap=" + Request["ap"] : string.Empty),
    //                    (!string.IsNullOrEmpty(Request["pat"]) && validAgent ? "&pat=" + Request["pat"] : string.Empty),
    //                    validAgent ? "&sid=" + _thisSession.Id : string.Empty);

    //            if (url.StartsWith("https"))
    //            {
    //                url = url.Replace("https", "http");
    //            }

    //            var buffer = PdfHelpers.GeneratePdf(url, thisOrder.OrderNumber);
    //            thisOrder = GetObjectFactory().GetByOQL<Order>("?(*)Order(Id = $p0$)", ThisOrderId);
    //            thisOrder.NumbViewPDF = thisOrder.NumbViewPDF + 1;
    //            thisOrder.OpenForPrinting = true;
    //            thisOrder.DatePDFLastViewed = CityDateTime.Now;
    //            thisOrder.PersistData();

    //            var result = ClientContentRenderer.RenderPdf(buffer, ThisOrderId + ".pdf");

    //            if (!string.IsNullOrEmpty(result))
    //            {
    //                throw new Exception("Pdf Generation failed with exception: " + Environment.NewLine + result + Environment.NewLine);
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            _log = GetObjectFactory().GetBlankNew<Log>();
    //            _log.Error(ex.ToString());
    //        }

    //        Response.End();
    //    }
    //}

    ///// <summary>
    ///// //Muhsin: Test voucher view
    ///// </summary>
    //private void TestPdfVoucher()
    //{
    //    //Set the appropriate ContentType.
    //    Response.ContentType = "Application/pdf";

    //    if (Request["testvoucher"] != null)
    //    {
    //        Response.WriteFile(MapPath("~/testvoucher.pdf"));
    //        Response.End();
    //    }

    //    if (Request["testagentvoucher"] != null)
    //    {
    //        Response.WriteFile(MapPath("~/testagentvoucher.pdf"));
    //        Response.End();
    //    }
    //}
}
