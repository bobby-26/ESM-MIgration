using System;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using System.Collections.Specialized;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;


public partial class PurchaseBulkPOFilter : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("GO", "GO", ToolBarDirection.Right);
            MenuFormFilter.MenuList = toolbarmain.Show();
            // MenuFormFilter.SetTrigger(pnlDiscussion);  

            txtVendorId.Attributes.Add("style", "visibility:hidden");
            if (!IsPostBack)
            {

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuFormFilter_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("GO"))
            {
                NameValueCollection criteria = new NameValueCollection();
                criteria.Clear();
                criteria.Add("txtReferenceNumber", txtReferenceNumber.Text);
                criteria.Add("txtVendorId", txtVendorId.Text);
                criteria.Add("txtInvoiceReferenceNumber", txtInvoiceReferenceNumber.Text);
                criteria.Add("txtInvoiceNumber", txtInvoiceNumber.Text);
                criteria.Add("ucCurrency", ucCurrency.SelectedCurrency);

                Filter.CurrentBulkPO = criteria;
            }

            if (Request.QueryString["callfrom"] != null && Request.QueryString["callfrom"].ToUpper() == "TECHNICALPO")
            {
                Response.Redirect("../Purchase/PurchaseBulkPOList.aspx");
            }
            else if (Request.QueryString["callfrom"] != null && Request.QueryString["callfrom"].ToUpper() == "DIRECTPO")
            {
                Response.Redirect("../Purchase/PurchaseBulkDirectPOList.aspx");
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
