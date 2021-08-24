using System;
using System.Data;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;

public partial class PurchaseBulkPOAttachments : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindURL();
        }
    }

    protected void BindURL()
    {        
        if (Filter.CurrentSelectedBulkOrderId != null && Filter.CurrentSelectedBulkOrderId != string.Empty)
        {
            DataSet ds = PhoenixPurchaseBulkPurchase.BulkPOEdit(new Guid(Filter.CurrentSelectedBulkOrderId.ToString()));
            if (ds.Tables[0].Rows.Count > 0)
            {
                string invoicecreatedyn = ds.Tables[0].Rows[0]["FLDCOPIEDSTATUS"].ToString();
                ViewState["DTKey"] = ds.Tables[0].Rows[0]["FLDDTKEY"].ToString();

                PhoenixToolbar toolbar = new PhoenixToolbar();
                
                toolbar.AddButton("Bulk PO", "BULKPO",ToolBarDirection.Left);
                toolbar.AddButton("Line Items", "LINEITEM", ToolBarDirection.Left);
                toolbar.AddButton("Vessels", "VESSEL", ToolBarDirection.Left);
                toolbar.AddButton("Attachments", "ATTACHMENTS", ToolBarDirection.Left);

                MenuAttachment.Title = "Attachment    ( Bulk Purchase Reference Number - " + ds.Tables[0].Rows[0]["FLDFORMNUMBER"].ToString() + "     )";

                MenuAttachment.MenuList = toolbar.Show();
                MenuAttachment.SelectedMenuIndex = 3;
                // done below changes for the bug id: 9446..
                if (invoicecreatedyn == "0" || invoicecreatedyn == "1")
                {
                    ifMoreInfo.Attributes["src"] = "../Common/CommonFileAttachment.aspx?DTKEY=" + ViewState["DTKey"].ToString() + "&MOD=" + PhoenixModule.PURCHASE + "&type=BULKPO";
                }
                else
                    ifMoreInfo.Attributes["src"] = "../Common/CommonFileAttachment.aspx?DTKEY=" + ViewState["DTKey"].ToString() + "&MOD=" + PhoenixModule.PURCHASE + "&type=BULKPO" + "&u=" + invoicecreatedyn;
                // 9446 over..
            }
        }
        else
        {            
            ViewState["DTKey"] = "";
            ifMoreInfo.Attributes["src"] = "../Common/CommonFileAttachment.aspx?DTKEY=" + ViewState["DTKey"].ToString() + "&MOD=" + PhoenixModule.PURCHASE + "&type=BULKPO";
        }
    }

    protected void MenuAttachment_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("BULKPO"))
        {
            if (Filter.CurrentSelectedBulkPOType == "1")
                Response.Redirect("../Purchase/PurchaseBulkPOList.aspx", false);
            else
                Response.Redirect("../Purchase/PurchaseBulkDirectPOList.aspx", false);
        } 
        if (CommandName.ToUpper().Equals("LINEITEM"))
        {
            Response.Redirect("../Purchase/PurchaseBulkPOLineItemList.aspx", false);
        }
        if (CommandName.ToUpper().Equals("VESSEL"))
        {
            Response.Redirect("../Purchase/PurchaseBulkPOLineItemByVesselList.aspx");
        }       
    }
}
