using System;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Purchase;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Common;
using System.Web.UI;
using System.Text;
using System.Web;
using Telerik.Web.UI;

public partial class PurchaseVendorRemarks : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        
        try
        {
            if (PhoenixSecurityContext.CurrentSecurityContext == null)
                PhoenixSecurityContext.CurrentSecurityContext = PhoenixSecurityContext.SystemSecurityContext;
            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Save", "SAVE", ToolBarDirection.Right);
            MenuFormRemarks.Title = "Confirm";
            MenuFormRemarks.MenuList = toolbarmain.Show();
            if (!IsPostBack)
            {
                if ((Request.QueryString["quotationid"] != null) && (Request.QueryString["quotationid"] != ""))
                {
                    ViewState["orderid"] = null;
                    ViewState["LASTUPDATEDREMARKS"] = null;
                    ViewState["quotationid"] = Request.QueryString["quotationid"].ToString();
                    Filter.CurrentPurchaseStockType = Request.QueryString["stocktype"].ToString();
                    BindData(Request.QueryString["quotationid"].ToString());
                }
            }
            
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindData(string quotationid)
    {
        DataSet dsquotationid = PhoenixPurchaseQuotation.EditQuotation(new Guid(quotationid));
        if (dsquotationid.Tables[0].Rows.Count > 0)
        {
            DataRow drquotationid = dsquotationid.Tables[0].Rows[0];
            ViewState["orderid"] = drquotationid["FLDORDERID"].ToString();
           
            txtRemarks.Content = HttpUtility.HtmlDecode(drquotationid["FLDREMARKS"].ToString());
            ViewState["LASTUPDATEDREMARKS"] = drquotationid["FLDREMARKS"].ToString();
        }

        DataSet dsVendor = PhoenixPurchaseQuotation.QuotationHeader(new Guid(quotationid));
        if (dsVendor.Tables[0].Rows.Count > 0)
        {
            DataRow dr = dsVendor.Tables[0].Rows[0];
            MenuFormRemarks.Title = "Confirmation [" + dr["FLDFORMNO"].ToString() + "]";
        }

        if (ViewState["orderid"] != null)
        {
            DataSet dsordeid = PhoenixPurchaseOrderForm.EditOrderForm(new Guid(ViewState["orderid"].ToString()));
            if (dsordeid.Tables[0].Rows.Count > 0)
            {
                DataRow drordeid = dsordeid.Tables[0].Rows[0];
                if (General.GetNullableDateTime(drordeid["FLDLATESTDELIVERYDATE"].ToString()) != null)
                    txtDeliveryDate.Text = drordeid["FLDLATESTDELIVERYDATE"].ToString();
            }
        }
    }

    protected void MenuFormRemarks_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        try
        {
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if ((Request.QueryString["quotationid"] != null) && (Request.QueryString["quotationid"] != ""))
                {
                    if (!String.IsNullOrEmpty(ViewState["LASTUPDATEDREMARKS"].ToString()))
                    {
                        string newAppendtxt = ViewState["LASTUPDATEDREMARKS"].ToString() + txtRemarks.Content.ToString();
                        PhoenixPurchaseQuotation.UpdateQuotationComments(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(ViewState["quotationid"].ToString()), newAppendtxt);
                    }
                    else
                    {
                        PhoenixPurchaseQuotation.UpdateQuotationComments(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(ViewState["quotationid"].ToString()), txtRemarks.Content);
                    }

                    if (ViewState["orderid"] != null)
                    {
                        PhoenixPurchaseOrderForm.OrderFormUpdateDeliveryDate(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(ViewState["orderid"].ToString()), General.GetNullableDateTime(txtDeliveryDate.Text));
                    }
                    ucStatus.Text = "Data has been Saved";
                    ucStatus.Visible = true;

                    BindData(ViewState["quotationid"].ToString());
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
