using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Purchase;
using SouthNests.Phoenix.Common;
using System.Web;
using System.Web.UI;
using Telerik.Web.UI;
public partial class PurchaseQuotationDeclineQuote : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        try
        {
            if (Request.QueryString["launchedfrom"] != null && Request.QueryString["launchedfrom"].ToString() == "VENDOR")
            {
                if (PhoenixSecurityContext.CurrentSecurityContext == null)
                    PhoenixSecurityContext.CurrentSecurityContext = PhoenixSecurityContext.SystemSecurityContext;
            }

            if (Request.QueryString["editable"] != null && Request.QueryString["editable"].ToString().ToUpper().Equals("TRUE"))
            {
                toolbarmain.AddButton("Save", "SAVE",ToolBarDirection.Right);
                MenuDeclineQuote.MenuList = toolbarmain.Show();
            }
            else
            {
                txtDeclineQuote.ReadOnly = true;
                txtDeclineQuote.CssClass = "readonlytextbox";
            }
            if (!IsPostBack)
            {
                if ((Request.QueryString["quotationid"] != null) && (Request.QueryString["quotationid"] != ""))
                {
                    ViewState["quotationid"] = Request.QueryString["quotationid"].ToString();
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

        DataSet ds = new DataSet();
        ds = PhoenixPurchaseQuotation.EditQuotation(new Guid(quotationid));
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];
            txtDeclineQuote.Text    = dr["FLDREMARKS"].ToString();
        }
    }

    protected void MenuDeclineQuote_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        try
        {
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (string.IsNullOrEmpty(txtDeclineQuote.Text))
                {
                    ucError.ErrorMessage = "Remark is Required.";
                    ucError.Visible = true;
                    return;
                }
                if ((Request.QueryString["quotationid"] != null) && (Request.QueryString["quotationid"] != ""))
                {
                    PhoenixPurchaseQuotation.UpdateQuotationComments(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(ViewState["quotationid"].ToString()), txtDeclineQuote.Text, 1);
                    ucStatus.Text = "Data has been Saved";
                    ucStatus.Visible = true;
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
