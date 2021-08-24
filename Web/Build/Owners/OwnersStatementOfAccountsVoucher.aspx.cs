using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using System.Collections.Specialized;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.Owners;
using System.Web.UI;
using System.Globalization;
using Telerik.Web.UI;

public partial class Owners_OwnersStatementOfAccountsVoucher : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            cmdHiddenSubmit.Attributes.Add("style", "display:none;");

            if (SessionUtil.CanAccess(this.ViewState, "Excel") || SessionUtil.CanAccess(this.ViewState, "PRINT"))
            {
                PhoenixToolbar toolbargrid = new PhoenixToolbar();
                if (SessionUtil.CanAccess(this.ViewState, "Excel"))
                    toolbargrid.AddFontAwesomeButton("../Owners/OwnersStatementOfAccountsVoucher.aspx", "Export to Excel", "<i class=\"fa fa-file-excel\"></i>", "Excel");
                if (SessionUtil.CanAccess(this.ViewState, "PRINT"))
                    toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvOwnersAccount')", "Print Grid", "<i class=\"fa fa-print\"></i>", "PRINT");
                MenuOrderForm.AccessRights = this.ViewState;
                MenuOrderForm.MenuList = toolbargrid.Show();
            }
            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;

                ViewState["debitnotereference"] = Request.QueryString["debitnotereference"].ToString();
                ViewState["accountid"] = Request.QueryString["accountid"].ToString();
                ViewState["Ownerid"] = Request.QueryString["Ownerid"].ToString();
                ViewState["ownerbudgetid"] = Request.QueryString["ownerbudgetid"].ToString();
                gvOwnersAccount.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void Rebind()
    {
        gvOwnersAccount.SelectedIndexes.Clear();
        gvOwnersAccount.EditIndexes.Clear();
        gvOwnersAccount.DataSource = null;
        gvOwnersAccount.Rebind();
    }
    protected void MenuOrderFormMain_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("SOA"))
            {
                Response.Redirect("../Owners/OwnersStatementOfAccounts.aspx", true);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();

        string[] alColumns = { "FLDOWNERBUDGETCODE", "FLDDESCRIPTION", "FLDVOUCHERDATE", "FLDVOUCHERROW", "FLDLONGDESCRIPTION", "FLDAMOUNT" };
        string[] alCaptions = { "Owner Budget Code", "Owner Budget Code Description", "Voucher Date", "Voucher Row", "Particulars", "Amount(USD)" };

        string sortexpression;
        int? sortdirection = null;
        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixOwnersStatementOfAccounts.StatementOfAccountVoucherListByOwnerBudget(
            ViewState["debitnotereference"].ToString(),
            int.Parse(ViewState["Ownerid"].ToString()),
            General.GetNullableGuid(ViewState["ownerbudgetid"].ToString()),
            int.Parse(ViewState["accountid"].ToString()),
            (int)ViewState["PAGENUMBER"],
            iRowCount,
            ref iRowCount,
            ref iTotalPageCount);
        Response.AddHeader("Content-Disposition", "attachment; filename=VoucherLineItemsDetails.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td colspan='" + (alColumns.Length - 2).ToString() + "'><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td></tr></table>");
        Response.Write("<br/>");
        Response.Write("<table><tr><td colspan='" + (alColumns.Length - 2).ToString() + "'><center> " + ds.Tables[0].Rows[0]["FLDACCOUNTCODE"].ToString() + " <td/></tr>");
        Response.Write("<tr><td colspan='" + (alColumns.Length - 2).ToString() + "'><center> " + ds.Tables[0].Rows[0]["FLDACCOUNTCODEDESC"].ToString() + " <td/></tr>");
        Response.Write("<tr><td colspan='" + (alColumns.Length - 2).ToString() + "'><center> " + ds.Tables[0].Rows[0]["FLDDEBITNOTEREFERENCE"].ToString() + " <td/></tr>");
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
                decimal s;

                if (i == 5)
                    Response.Write("<td align='right'>");
                else
                    Response.Write("<td align='left'>");
                Response.Write(decimal.TryParse(dr[alColumns[i]].ToString(), out s) ? String.Format("{0:#,###,###.##}", dr[alColumns[i]]) : dr[alColumns[i]]);
                Response.Write("</td>");
            }
            Response.Write("</tr>");
        }
        Response.Write("</TABLE>");
        Response.End();
    }
    protected void MenuOrderForm_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
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
    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDACCOUNTCODE", "FLDACCOUNTCODEDESC", "FLDDEBITNOTEREFERENCE", "FLDOWNERBUDGETCODE", "FLDDESCRIPTION", "FLDVOUCHERDATE", "FLDVOUCHERROW", "FLDLONGDESCRIPTION", "FLDAMOUNT" };
        string[] alCaptions = { "Account Code", "Account Description", "Statement Reference", "Owner Budget Code", "Owner Budget Code Description", "Voucher Date", "Voucher Row", "Particulars", "Amount(USD)" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());

        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        DataSet ds = new DataSet();

        ds = PhoenixOwnersStatementOfAccounts.StatementOfAccountVoucherListByOwnerBudget(
            ViewState["debitnotereference"].ToString(),
            int.Parse(ViewState["Ownerid"].ToString()),
            General.GetNullableGuid(ViewState["ownerbudgetid"].ToString()), //Filter.CurrentOwnerBudgetCodeSelection), 
            int.Parse(ViewState["accountid"].ToString()),
            (int)ViewState["PAGENUMBER"],
            gvOwnersAccount.PageSize,
            ref iRowCount,
            ref iTotalPageCount);

        if (ds.Tables[0].Rows.Count > 0)
        {
            //lblAccountCOde.Text = ds.Tables[0].Rows[0]["FLDACCOUNTCODE"].ToString();
            lblAccountId.Text = ds.Tables[0].Rows[0]["FLDACCOUNTID"].ToString();
            lblAccountCOdeDescription.Text = ds.Tables[0].Rows[0]["FLDACCOUNTCODEDESC"].ToString();
            lnkOwnerBudgetCode.Text = ds.Tables[0].Rows[0]["FLDOWNERBUDGETCODE"].ToString();
            lblBudgetCodeDescription.Text = ds.Tables[0].Rows[0]["FLDDESCRIPTION"].ToString();
            lblStatementReference.Text = ds.Tables[0].Rows[0]["FLDDEBITNOTEREFERENCE"].ToString();

        }
        gvOwnersAccount.DataSource = ds;
        gvOwnersAccount.VirtualItemCount = iRowCount;

        if (ds.Tables[1].Rows.Count > 0)
        {
            lblTotal.Text = String.Format(CultureInfo.InvariantCulture,"{0:#,#.00}", ds.Tables[1].Rows[0]["FLDAMOUNT"]);
        }
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        General.SetPrintOptions("gvOwnersAccount", "Order Form List", alCaptions, alColumns, ds);
    }

    protected void gvOwnersAccount_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
           
        }
    }
    protected void gvOwnersAccount_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {           
            if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBER"] = null;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvOwnersAccount_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvOwnersAccount.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        BindData();
    }
}
