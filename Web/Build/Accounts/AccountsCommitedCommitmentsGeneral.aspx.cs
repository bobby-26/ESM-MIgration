using System;
using System.Data;
using System.Net.Mail;
using System.Web;
using System.Web.UI;
using System.Text;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Collections.Specialized;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class Accounts_AccountsCommitedCommitmentsGeneral : PhoenixBasePage
{
    protected override void Render(HtmlTextWriter writer)
    {
        base.Render(writer);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);

        cmdHiddenSubmit.Attributes.Add("style", "display:none;");

        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Change Received Date", "GOODSRECEIVEDDATEUPDATE", ToolBarDirection.Right);
        toolbar.AddButton("Exclude", "EXCLUDE",ToolBarDirection.Right);
        toolbar.AddButton("Change Budget Codes", "CHANGEBUDGETCODE",ToolBarDirection.Right);
        toolbar.AddButton("Change Committed Date", "CHANGECOMMITTEDDATE",ToolBarDirection.Right);
       
      
        MenuAdvancePayment.AccessRights = this.ViewState;
        MenuAdvancePayment.MenuList = toolbar.Show();

        if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            ViewState["COMMITMENTID"] = "";

            ViewState["ORDERID"] = "";
            ViewState["VESSELID"] = "";

            if (Request.QueryString["COMMITMENTID"] != null && Request.QueryString["COMMITMENTID"] != string.Empty)
            {
                ViewState["COMMITMENTID"] = Request.QueryString["COMMITMENTID"].ToString();

                CommitmentEdit();
            }
        }
    }

    private void Reset()
    {

    }

    protected void MenuAdvancePayment_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        try
        {
            if (CommandName.ToUpper().Equals("NEW"))
            {
               
            }
            if (CommandName.ToUpper().Equals("EXCLUDE"))
            {

                if (!IsValidExclude())
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixAccountsCommittedCosts.AccountsCommittedExcludedUpdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                                , General.GetNullableGuid(ViewState["COMMITMENTID"].ToString())
                                                                                ,General.GetNullableDateTime(ucDate.Text));

                String scriptupdate = String.Format("javascript:parent.fnReloadList('code1');");
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptupdate, true);
            }
            if (CommandName.ToUpper().Equals("GOODSRECEIVEDDATEUPDATE"))
            {

                if (!IsValidGoodsReceiveddate())
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixAccountsCommittedCosts.AccountsCommittedGoodsReceivedDateUpdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                                , General.GetNullableGuid(ViewState["COMMITMENTID"].ToString())
                                                                                , General.GetNullableDateTime(ucgoodsreceiveddate.Text));

                String scriptupdate = String.Format("javascript:parent.fnReloadList('code1');");
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptupdate, true);
            }
            if (CommandName.ToUpper().Equals("CHANGEBUDGETCODE"))
            {
                if (General.GetNullableGuid(ViewState["ORDERID"].ToString()) != null)
                {
                    String scriptpopup = String.Format(
                   "javascript:parent.openNewWindow('codehelp1', '', '"+Session["sitepath"]+"/Accounts/AccountsCommittedBudgetCodesRefresh.aspx?ORDERID=" + ViewState["ORDERID"].ToString() +
                   "&VESSELID=" + ViewState["VESSELID"].ToString() + "&ACCOUNTID=" + ViewState["ACCOUNTID"] + "');");
                    ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", scriptpopup, true);
                }
            }
            if (CommandName.ToUpper().Equals("CHANGECOMMITTEDDATE"))
            {
                if (General.GetNullableGuid(ViewState["ORDERID"].ToString()) != null)
                {
                    String scriptpopup = String.Format(
                   "javascript:parent.openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Accounts/AccountsCommittedDateRefresh.aspx?ORDERID=" + ViewState["ORDERID"].ToString() +
                   "&VESSELID=" + ViewState["VESSELID"].ToString() + "');");
                    ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", scriptpopup, true);
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    public bool IsValidExclude()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableDateTime(ucDate.Text) == null)
            ucError.ErrorMessage = "Excluded with effect from date is required.";

        return (!ucError.IsError);
    }

    public bool IsValidGoodsReceiveddate()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableDateTime(ucgoodsreceiveddate.Text) == null)
            ucError.ErrorMessage = "Goods Received Date is required.";

        return (!ucError.IsError);
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
    }

    protected void CommitmentEdit()
    {
        try
        {
            if (ViewState["COMMITMENTID"] != null)
            {
                DataSet ds = PhoenixAccountsCommittedCosts.AccountsCommittedList(General.GetNullableGuid(ViewState["COMMITMENTID"].ToString()));

                DataRow dr = ds.Tables[0].Rows[0];

                txtPoNumber.Text = dr["FLDREFERENCENUMBER"].ToString();
                txtSupplierName.Text = dr["FLDNAME"].ToString();
                txtVesselAccountCode.Text = dr["FLDACCOUNTNAME"].ToString();
                txtBudgetCOde.Text = dr["FLDBUDGETCODEDESCRIPTION"].ToString();
                txtOrderedDate.Text = dr["FLDORDEREDDATE"].ToString();
                txtBudgetGroup.Text = dr["FLDBUDGETGROUP"].ToString();
                txtCommittedDate.Text = dr["FLDCOMMITTEDDATE"].ToString();
                txtReversedDate.Text = dr["FLDREVERSEDDATE"].ToString();
                txtReasonReversal.Text = dr["FLDREASONFORREVERSAL"].ToString();
                txtReasonCommitted.Text = dr["FLDCOMMITTEDDATECHANGE"].ToString();
                txtPurchaseInvNo.Text = dr["FLDVOUCHERNUMBER"].ToString();
                ucDate.Text = dr["FLDEXCLUDEDWITHEFFECT"].ToString();
                txtExcludedOn.Text = dr["FLDEXCLUDEDBY"].ToString();
                txtOwnerBudgetCode.Text = dr["FLDOWNERBUDGETGROUP"].ToString();
                txtPODescription.Text = dr["FLDPODESCRIPTION"].ToString();
                txtInvoiceStatus.Text = dr["FLDINVOICESTATUS"].ToString();
                ucgoodsreceiveddate.Text = dr["FLDGOODSRECEIVEDDATE"].ToString();
                //if (General.GetNullableDateTime(ucDate.Text) != null)
                //{
                //    ucDate.Enabled = false;
                //}

                ViewState["ORDERID"] = dr["FLDORDERID"].ToString();
                ViewState["VESSELID"] = dr["FLDVESSELID"].ToString();
                ViewState["ACCOUNTID"] = dr["FLDACCOUNTID"].ToString();

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}

