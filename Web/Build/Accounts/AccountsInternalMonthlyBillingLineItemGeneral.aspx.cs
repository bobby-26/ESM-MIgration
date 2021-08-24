using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using System.Web.UI;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class AccountsInternalMonthlyBillingLineItemGeneral : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("POST", "POST", ToolBarDirection.Right);
            MenuStaging.AccessRights = this.ViewState;
            MenuStaging.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["POSTALL"] = "0";
                if (Request.QueryString["portagebillid"] != null && Request.QueryString["portagebillid"] != string.Empty)
                    ViewState["PORTAGEBILLID"] = Request.QueryString["portagebillid"];
                if (Request.QueryString["budgetbillingid"] != null && Request.QueryString["budgetbillingid"] != string.Empty)
                    ViewState["BUDGETBILLINGID"] = Request.QueryString["budgetbillingid"];
                if (Request.QueryString["postall"] != null && Request.QueryString["postall"] != string.Empty)
                    ViewState["POSTALL"] = Request.QueryString["postall"];

                BindPortageBillData();

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindPortageBillData()
    {
        if (ViewState["PORTAGEBILLID"] != null && ViewState["PORTAGEBILLID"].ToString() != "")
        {
            DataSet ds = new DataSet();
            ds = PhoenixAccountsInternalBilling.PortageBillEdit(new Guid(ViewState["PORTAGEBILLID"].ToString()));
            if (ds.Tables[0].Rows.Count > 0)
            {
                txtStartDate.Text = ds.Tables[0].Rows[0]["FLDFROMDATE"].ToString();
                txtEndDate.Text = ds.Tables[0].Rows[0]["FLDTODATE"].ToString();
            }
        }
    }

    protected void MenuStaging_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("POST"))
        {
            try
            {
                if (!IsValidPost(txtVoucherDate.Text))
                {
                    ucError.Visible = true;
                    return;
                }

                //ucConfirm.Visible = true;
                //ucConfirm.Text = "Do you want to Post Voucher .?";
                RadWindowManager1.RadConfirm("Do you want to Post Voucher .?", "PostRecord", 320, 150, null, "Post");
                return;
            }
            catch (Exception ex)
            {
                ucError.ErrorMessage = ex.Message;
                ucError.Visible = true;
            }
        }
    }
    protected void ucConfirm_Click(object sender, EventArgs e)
    {
        try
        {
            string Script = "";
            Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
            Script += " fnReloadList();";
            Script += "</script>" + "\n";

            PostVoucher();

            if (!ucError.Visible)
            {
                Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script, true);
                RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "refreshScript", "javascript:fnReloadList('codehelp1',null);", true);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }
    private void PostVoucher()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
            string sortexpression = (ViewState["SORTEXPRESSIONSUB"] == null) ? null : (ViewState["SORTEXPRESSIONSUB"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTIONSUB"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTIONSUB"].ToString());

            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = 10;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

            DataSet ds = PhoenixAccountsInternalBilling.BillingItemSearch(
                                                             new Guid(ViewState["PORTAGEBILLID"].ToString())
                                                            , sortexpression
                                                            , sortdirection
                                                            , (int)ViewState["PAGENUMBER"]
                                                            , iRowCount
                                                            , ref iRowCount
                                                            , ref iTotalPageCount);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow[] drows;
                Guid monthlybillingid, budgetbillingid, vesselbudgetallocationid;
                if (ViewState["POSTALL"] != null && ViewState["POSTALL"].ToString() == "0")
                {
                    drows = ds.Tables[0].Select("FLDBUDGETBILLINGID = '" + ViewState["BUDGETBILLINGID"].ToString() + "'");
                    foreach (DataRow dr in drows)
                    {
                        monthlybillingid = new Guid(dr["FLDMONTHLYBILLINGITEMID"].ToString());
                        budgetbillingid = new Guid(dr["FLDBUDGETBILLINGID"].ToString());
                        vesselbudgetallocationid = new Guid(dr["FLDVESSELBUDGETALLOCATIONID"].ToString());
                        PhoenixAccountsInternalBilling.VoucherPost(
                                                                        PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                       , new Guid(ViewState["PORTAGEBILLID"].ToString())
                                                                       , monthlybillingid
                                                                       , budgetbillingid
                                                                       , vesselbudgetallocationid
                                                                       , General.GetNullableDateTime(txtVoucherDate.Text.Trim()));
                    }
                }
                else
                {
                    PhoenixAccountsInternalBilling.VoucherPostAll(new Guid(ViewState["PORTAGEBILLID"].ToString()), General.GetNullableDateTime(txtVoucherDate.Text.Trim()));
                }
                ucStatus.Text = "Voucher(s) posted.";

            }

        }
        catch (Exception ex)
        {
            ucError.HeaderMessage = "";
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }
    protected bool IsValidPost(string voucherdate)
    {
        if (General.GetNullableDateTime(voucherdate) == null)
            ucError.Text = "Voucher Date is required to post a voucher.";

        return (!ucError.IsError);
    }
}
