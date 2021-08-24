using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Accounts;
using Telerik.Web.UI;

public partial class AccountsOfficeWagesAdjustmentPosting : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            ViewState["vslid"] = Request.QueryString["vslid"];
            ViewState["pbid"] = Request.QueryString["pbid"];
            ViewState["pbno"] = Request.QueryString["pbno"];
            cmdHiddenSubmit.Attributes.Add("style", "display:none;");
            PhoenixToolbar toolbarsub = new PhoenixToolbar();
            toolbarsub.AddButton("With Budget", "WITHBUDGET");
            toolbarsub.AddButton("Without Budget", "WITHOUTBUDGET");
            toolbarsub.AddButton("Post Voucher", "POST");
            toolbarsub.AddButton("Arrears", "ARREARS");
            toolbarsub.AddButton("Back", "VOUCHER");
            MenuWagesAjustment.AccessRights = this.ViewState;
            MenuWagesAjustment.MenuList = toolbarsub.Show();
            MenuWagesAjustment.SelectedMenuIndex = 3;
            PhoenixToolbar toolbarpost = new PhoenixToolbar();
            toolbarpost.AddButton("Post", "POST", ToolBarDirection.Right);
            MenuPost.AccessRights = this.ViewState;
            MenuPost.MenuList = toolbarpost.Show();

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddImageButton("../Accounts/AccountsOfficeWagesAdjustmentPosting.aspx?vslid=" + ViewState["vslid"].ToString() + "&pbid=" + ViewState["pbid"].ToString() + "", "Export to Excel", "icon_xls.png", "Excel");
            toolbar.AddImageLink("javascript:CallPrint('gvWagesAdjustment')", "Print Grid", "icon_print.png", "PRINT");
            MenuOffice.AccessRights = this.ViewState;
            MenuOffice.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["VESSELID"] = null;
                gvWagesAdjustment.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuWagesAjustment_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            string date = string.Empty;

            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("WITHBUDGET"))
            {
                Response.Redirect("../Accounts/AccountsOfficePortageBillPostingBreakUp.aspx?pno=" + Request.QueryString["pno"] + "&vslid=" + Request.QueryString["vslid"] + "&pbid=" + Request.QueryString["pbid"] + "&withbudget=1");
            }
            if (CommandName.ToUpper().Equals("WITHOUTBUDGET"))
            {
                Response.Redirect("../Accounts/AccountsOfficePortageBillPostingBreakUp.aspx?pno=" + Request.QueryString["pno"] + "&vslid=" + Request.QueryString["vslid"] + "&pbid=" + Request.QueryString["pbid"] + "&withbudget=0");
            }
            if (CommandName.ToUpper().Equals("POST"))
            {
                Response.Redirect("../Accounts/AccountsOfficePortageBillPostingDraft.aspx?pno=" + Request.QueryString["pno"] + "&vslid=" + Request.QueryString["vslid"] + "&pbid=" + Request.QueryString["pbid"]);
            }
            if (CommandName.ToUpper().Equals("VOUCHER"))
            {
                Response.Redirect("../Accounts/AccountsOfficePortageBillPosting.aspx?pno=" + Request.QueryString["pno"], true);
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuPost_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("POST"))
            {
                PhoenixAccountsOfficePortageBill.OfficeWagesAdjustmentVoucherPosting(General.GetNullableInteger(ViewState["vslid"].ToString())
                    , General.GetNullableGuid(ViewState["pbid"].ToString()));
                ucStatus.Text = "Voucher Posted.";
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        Rebind();
    }
    protected void Rebind()
    {
        gvWagesAdjustment.SelectedIndexes.Clear();
        gvWagesAdjustment.EditIndexes.Clear();
        gvWagesAdjustment.DataSource = null;
        gvWagesAdjustment.Rebind();
    }
    private void BindData()
    {
        string[] alColumns = { "FLDFILENO", "FLDEMPLOYEENAME", "FLDRANKNAME", "FLDCOMPONENTNAME", "FLDSUBACCOUNT", "FLDOFFICEAMOUNT", "FLDVESSELAMOUNT", "FLDARREARS" };
        string[] alCaptions = { "File No.", "Employee", "Rank", "Component", "Budget Code", "Office Amount", "Vessel Amount", "Arrears" };

        DataSet ds = PhoenixAccountsOfficePortageBill.OfficeWagesAdjustmentSearch(General.GetNullableInteger(ViewState["vslid"].ToString())
            , new Guid(ViewState["pbid"].ToString()));
        General.SetPrintOptions("gvWagesAdjustment", "Wages Adjustment", alCaptions, alColumns, ds);
        gvWagesAdjustment.DataSource = ds;
        gvWagesAdjustment.VirtualItemCount = ds.Tables[0].Rows.Count;

    }

    protected void gvWagesAdjustment_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToUpper().Equals("SORT"))
            return;
    }
    protected void gvWagesAdjustment_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {           
        }
    }

    protected void gvWagesAdjustment_ItemCommand(object sender, GridCommandEventArgs e)
    {
    }
    protected void gvWagesAdjustment_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {           
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void ShowExcel()
    {

        string[] alColumns = { "FLDFILENO", "FLDEMPLOYEENAME", "FLDRANKNAME", "FLDCOMPONENTNAME", "FLDSUBACCOUNT", "FLDOFFICEAMOUNT", "FLDVESSELAMOUNT", "FLDARREARS" };
        string[] alCaptions = { "File Number", "Employee Name", "Rank", "Component", "Budget Code", "Office Amount", "Vessel Amount", "Arrears" };

        DataSet ds = PhoenixAccountsOfficePortageBill.OfficeWagesAdjustmentSearch(General.GetNullableInteger(ViewState["vslid"].ToString())
            , new Guid(ViewState["pbid"].ToString()));

        Response.AddHeader("Content-Disposition", "attachment; filename=WagesAdjustment.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Wages Adjustment</h3></td>");
        Response.Write("<td colspan='" + (alColumns.Length - 2).ToString() + "'>&nbsp;</td>");
        Response.Write("</tr>");
        Response.Write("</TABLE>");
        Response.Write("<br />");
        Response.Write("<TABLE BORDER='1' CELLPADDING='2' CELLSPACING='2' width='100%'>");
        Response.Write("<tr>");
        for (int i = 0; i < alCaptions.Length; i++)
        {
            Response.Write("<td width='20%'>");
            Response.Write("<b>" + alCaptions[i] + "</b>");
            Response.Write("</td>");
        }
        Response.Write("</tr>");
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            Response.Write("<tr>");
            for (int i = 0; i < alColumns.Length; i++)
            {
                Response.Write("<td>");
                Response.Write(dr[alColumns[i]]);
                Response.Write("</td>");
            }
            Response.Write("</tr>");
        }
        Response.Write("</TABLE>");
        Response.End();
    }
    protected void MenuOffice_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
