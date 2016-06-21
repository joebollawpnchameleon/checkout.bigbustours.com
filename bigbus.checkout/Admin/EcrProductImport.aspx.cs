using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using bigbus.checkout.data.Model;
using bigbus.checkout.EcrWServiceRefV3;
using bigbus.checkout.Models;
using Common.Enums;
using Microsoft.VisualBasic.FileIO;

namespace bigbus.checkout.Admin
{
    public partial class EcrProductImport : AdminBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        protected void UploadButton_Click(object sender, EventArgs e)
        {
            if (FileUploader.HasFile)
                try
                {
                    var fullPath = Server.MapPath(AdminUploadPath) + FileUploader.FileName;

                    FileUploader.SaveAs(fullPath);
                    lbResult.Text = "File name: " +
                         FileUploader.PostedFile.FileName + "<br>" +
                         FileUploader.PostedFile.ContentLength + " kb<br>" +
                         "Content type: " +
                         FileUploader.PostedFile.ContentType + "<br><b>Uploaded Successfully";

                    var importedTickets = ParseCsv(fullPath);

                   
                }
                catch (Exception ex)
                {
                    lbResult.Text = "ERROR: " + ex.Message.ToString();
                }
            else
            {
                lbResult.Text = "You have not specified a file.";
            }
        }

        public  List<ImportedTicket> ParseCsv(string filPath)
        {
            var index = 0;

            var productList = EcrService.GetProductList();

            try
            {
                var importedTicketList = new List<ImportedTicket>();
                var parser = new TextFieldParser(filPath) {HasFieldsEnclosedInQuotes = true};
                var previousTicket = new ImportedTicket();

                parser.SetDelimiters(",");

                while (!parser.EndOfData)
                {
                    index++;
                    var fields = parser.ReadFields();

                    //we assume that first row is for titles.
                    if (index == 1 || fields == null || fields.Length < 13)
                    {
                        continue;
                    }

                    var productUid = fields[0].Trim();

                    var product = productList.FirstOrDefault(x => x.SysID.Equals(fields[1]));

                    if (product == null)
                    {
                        Log("Product not found sysid: " + fields[1]);
                    }

                    var dimension = new EcrProductDimension
                    {
                        AgeBand = fields[4],
                        DimensionSku = fields[3],
                        EcrProductSku = fields[1],
                        UiDimensionId = fields[2]
                    };

                    if (!string.IsNullOrEmpty(previousTicket.EcrProductUid) &&
                        previousTicket.EcrProductUid.Equals(productUid, StringComparison.CurrentCultureIgnoreCase))
                    {
                        previousTicket.EcrProductDimensionList.Add(dimension);
                    }
                    else
                    {
                        previousTicket = new ImportedTicket
                        {
                            EcrProductUid = fields[0],
                            EcrProductSku = fields[1],
                            TicketType = fields[5],
                            StartDate = Convert.ToDateTime(fields[6]),
                            MicroSiteId = fields[9],
                            Name = fields[10],
                            Description = fields[11],
                            FulfilmentInstructions = fields[12],
                            EcrProductDimensionList = new List<EcrProductDimension>() {dimension}
                        };

                        importedTicketList.Add(previousTicket);
                    }

                    //*** testing only please remove after
                    if (index == 2)
                        break;
                }

                parser.Close();

                CreateNewTicketsInDb(importedTicketList, productList);

                return importedTicketList;
            }
            catch(Exception ex)
            {
                Log("Import failed at step: " + index + ex.Message);
                return null;
            }
        }

        private void CreateNewTicketsInDb(List<ImportedTicket> importedTickets, Product[] ecrProductList)
        {
            foreach (var iticket in importedTickets)
            {
                 var product = ecrProductList.FirstOrDefault(x => x.SysID.Equals(iticket.EcrProductSku));

                var ticket = new Ticket
                {
                    StartDate = iticket.StartDate,
                    AdultTicketEnabled = true,
                    ChildTicketEnabled = true,
                    FamilyTicketEnabled = true,
                    ConcessionTicketEnabled = false,
                    AgentsOnly = false,
                    EnabledForAgents = false,
                    AttractionApi = null,
                    AttractionDisplayOrder = null,
                    DateCreated = DateTime.Now,
                    Enabled = true,
                    Description = iticket.Description,
                    IsPackage = false,
                    EcrVersionId = (int)EcrVersion.Three,
                    NcEcrProductCode = iticket.EcrProductSku,
                    TicketType = iticket.TicketType,
                    MicroSiteId = iticket.MicroSiteId,
                    Name = iticket.Name,
                    HasMobile = true
                };

                if (!string.IsNullOrEmpty(iticket.FulfilmentInstructions))
                {
                    var len = iticket.FulfilmentInstructions.Length;
                    var ticketDetails = iticket.FulfilmentInstructions;

                    ticket.TicketTextLine2 = (len <= 130) ? ticketDetails : ticketDetails.Substring(0, 130);
                    ticket.TicketTextLine3 = (len > 130) ? ticketDetails.Substring(130, 130) : string.Empty;
                    ticket.TicketTextBottomLine = (len > 260) ? ticketDetails.Substring(260, 40) : string.Empty;
                }

                TicketService.CreateTicket(ticket);

                if (ticket.Id.Equals(Guid.Empty))
                {
                    Log("Failed to create ticket into DB ecr ticket id sysid:" + iticket.EcrProductSku);
                }
                else //create related product dimensions - *** leave this for later as it is not needed at this stage.
                {
                    //var dimensions = iticket.EcrProductDimensionList;
                    
                    //if (product != null && dimensions.Count > 0)
                    //{
                    //    foreach (var dimension in product.ProductDimensions)
                    //    {
                    //        var ecrDimension = new TicketEcrDimension
                    //        {
                    //             Name = dimension.Name,
                    //             Amount =  dimension.Prices
                    //        };
                    //    }    
                    //}
                }
            }
        }
    }
}