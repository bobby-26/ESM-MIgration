using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Common;
using System.Web;
using System.Web.UI;
using Telerik.Web.UI;

public partial class AccountsInvoiceDirectPurchaseOrderCancelRemarks : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {

        try
        {
            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Save", "SAVE",ToolBarDirection.Right);
            MenuDelInstruction.Title = "Cancel Purchase Order";
            MenuDelInstruction.MenuList = toolbarmain.Show();

            if (!IsPostBack)
            {
                if ((Request.QueryString["directpo"] != null) && (Request.QueryString["directpo"] != ""))
                    ViewState["directpo"] = Request.QueryString["directpo"].ToString();
            }

            if (ViewState["directpo"] != null)
            {
                DataTable dt = PhoenixAccountsInvoice.InvoiceDirectPOEdit(new Guid(ViewState["directpo"].ToString()));
                if (dt.Rows.Count > 0)
                {
                    //decimal? advanceamt = 0.00M;

                    //advanceamt = General.GetNullableDecimal(dt.Rows[0]["FLDPURCHASEADVANCEAMOUNT"].ToString());

                    //if (advanceamt != null && advanceamt > 0)
                    //{
                    string lblDTkey = dt.Rows[0]["FLDDTKEY"].ToString();
                    ifMoreInfo.Attributes["src"] = "../Common/CommonFileAttachment.aspx?dtkey=" + lblDTkey + "&mod=" + PhoenixModule.ACCOUNTS + "&type=POWithAdvance";//+ "&U=null"
                    //}                    
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuDelInstruction_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidRemarks())
                {
                    ucError.Visible = true;
                    return;
                }
                if ((Request.QueryString["directpo"] != null) && (Request.QueryString["directpo"] != ""))
                {
                    string attachmentflag;
                    DataTable dt = PhoenixAccountsInvoice.AdavanceDirectPOCancelValidation(General.GetNullableGuid(Request.QueryString["directpo"].ToString()), 1);
                    if (dt.Rows.Count > 0)
                    {
                        attachmentflag = dt.Rows[0]["FLDFLAG"].ToString();
                        if (attachmentflag == "1")
                        {
                            ucError.ErrorMessage = "Attachment is mandatory to Cancel PO which is having Adavance amount";
                            ucError.Visible = true;
                            return;
                        }
                    }
                    PhoenixAccountsInvoice.InvoiceDirectPurchaseOrderCancel(new Guid(ViewState["directpo"].ToString()), txtRemarks.Text);
                    String scriptupdate = String.Format("javascript:fnReloadList('codehelp1','IfMoreInfo',null);");
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptupdate, true);
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool IsValidRemarks()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (string.IsNullOrEmpty(txtRemarks.Text))
        {
            ucError.ErrorMessage = "Remarks is required";
        }

        return (!ucError.IsError);
    }
}
