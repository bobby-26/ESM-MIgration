using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using System.Drawing;
using System.Web.UI;
using Telerik.Web.UI;

public partial class Accounts_AccountsDashboardRemittanceSummaryApproval : PhoenixBasePage
{
    public static decimal dTotalUSDAmount = 0;
    public static decimal dSumUSDAmount = 0;
    public static string strdTotalUSDAmount = string.Empty;
    decimal subTotal = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Approve", "APPROVED", ToolBarDirection.Right);

            MenuLineItem.AccessRights = this.ViewState;
            MenuLineItem.Title = "MD Approval";
            MenuLineItem.MenuList = toolbarmain.Show();
            if (!IsPostBack)
            {
                ViewState["PRVINSID"] = "";
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;
                gvRemittance.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddImageButton("../Accounts/AccountsRemittanceSummaryApproval.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbargrid.AddImageLink("javascript:CallPrint('gvRemittance')", "Print Grid", "icon_print.png", "PRINT");
            MenuOrderLineItem.AccessRights = this.ViewState;
            MenuOrderLineItem.MenuList = toolbargrid.Show();
            //  MenuOrderLineItem.SetTrigger(pnlStockItemEntry);
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
        subTotal = 0;
        ViewState["PRVINSID"] = "";
        DataSet ds = new DataSet();
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        ds = PhoenixAccountsAllotmentRemittance.ListRemittanceSummaryApproval(PhoenixSecurityContext.CurrentSecurityContext.CompanyID, Int32.Parse(ViewState["PAGENUMBER"].ToString())
                                            , gvRemittance.PageSize
                                            , ref iRowCount, ref iTotalPageCount);

        {
            gvRemittance.DataSource = ds;
            gvRemittance.VirtualItemCount = iRowCount;
            ViewState["ROWCOUNT"] = iRowCount;

            ViewState["PRVINSID"] = string.Empty;
            DataTable dt = ds.Tables[0];
        }
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

        string[] alColumns = { "FLDBANKACCOUNTNUMBER", "FLDSUPPLIERNAME", "FLDPAYMENTCURRENCYCODE", "FLDTOTALPAYABLEAMOUNT", "FLDUSDEQUVALENT" };
        string[] alCaptions = { "Bank Account", "Supplier", "Currency", "Amount", "USD Equivalent" };
        General.SetPrintOptions("gvRemittance", "Remittance Summary Approval", alCaptions, alColumns, ds);
    }

    protected void MenuOrderLineItem_TabStripCommand(object sender, EventArgs e)
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

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {

    }

    protected void MenuLineItem_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("APPROVED"))
        {
            PhoenixAccountsRemittance.RemittanceInstructionApprovedByMD(PhoenixSecurityContext.CurrentSecurityContext.UserCode, PhoenixSecurityContext.CurrentSecurityContext.CompanyID);
            ucStatus.Text = "Remittance Approved";
            Rebind();
            String scriptinsert = String.Format("javascript:parent.fnReloadList('code1');");
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptinsert, true);

            PhoenixAccountsAllotmentRemittance.AllotmentRemittanceBatchListUpdate(PhoenixSecurityContext.CurrentSecurityContext.CompanyID);
        }
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();

        string[] alColumns = { "FLDBANKACCOUNTNUMBER", "FLDSUPPLIERNAME", "FLDPAYMENTCURRENCYCODE", "FLDTOTALPAYABLEAMOUNT", "FLDUSDEQUVALENT" };
        string[] alCaptions = { "Bank Account", "Supplier", "Currency", "Amount", "USD Equivalent" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixAccountsAllotmentRemittance.ListRemittanceSummaryApproval(PhoenixSecurityContext.CurrentSecurityContext.CompanyID, 1
                                            , PhoenixGeneralSettings.CurrentGeneralSetting.ExcelRecordsCount
                                            , ref iRowCount, ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=RemittanceSummaryApproval.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Remittance Summary Approval</h3></td>");
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
    protected void gvRemittance_RowCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName == "Page")
        {
            ViewState["PAGENUMBER"] = null;
        }
        if (e.CommandName.ToUpper().Equals("REJECT"))
        {
            PhoenixAccountsRemittance.RemittanceInstructionRejectedByMD(((RadLabel)e.Item.FindControl("lblBankInstructionId")).Text, PhoenixSecurityContext.CurrentSecurityContext.UserCode);
            Rebind();
        }
        else
        {
            Rebind();
        }
    }
    protected void gvRemittance_OnRowCreated(object sender, GridViewRowEventArgs e)
    {
        DataRowView drv = (DataRowView)e.Row.DataItem;

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            DataTable dt = (e.Row.DataItem as DataRowView).DataView.Table;
            if (drv["FLDBANKACCOUNTNUMBER"].ToString() == string.Empty) return;
            if (ViewState["PRVINSID"].ToString() != drv["FLDBANKACCOUNTNUMBER"].ToString())
            {
                this.AddTotalRow("Sub Total", subTotal.ToString("N2"));
                ViewState["PRVINSID"] = drv["FLDBANKACCOUNTNUMBER"].ToString();
                subTotal = 0;
            }
            subTotal += Convert.ToDecimal(dt.Rows[e.Row.RowIndex]["FLDUSDEQUVALENT"]);

        }
    }
    protected void gvRemittance_ItemDataBound(Object sender, GridItemEventArgs e)
    {

        if (e.Item is GridDataItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            ImageButton cmdReject = (ImageButton)e.Item.FindControl("cmdReject");
            if (cmdReject != null)
                cmdReject.Visible = SessionUtil.CanAccess(this.ViewState, cmdReject.CommandName);
        }
    }

    private void AddTotalRow(string labelText, string value)
    {
        GridViewRow row = new GridViewRow(0, 0, DataControlRowType.DataRow, DataControlRowState.Normal);
        row.BackColor = ColorTranslator.FromHtml("#F9F9F9");
        row.Cells.AddRange(new TableCell[3] { new TableCell { ColumnSpan=4 } //Empty Cell                                                                               
                                        ,new TableCell { Text = labelText, HorizontalAlign = HorizontalAlign.Right,CssClass="bold"}
                                        ,new TableCell { Text = value, HorizontalAlign = HorizontalAlign.Right, CssClass="bold" }});

        gvRemittance.Controls[0].Controls.Add(row);
    }

    protected void Rebind()
    {
        gvRemittance.SelectedIndexes.Clear();
        gvRemittance.EditIndexes.Clear();
        gvRemittance.DataSource = null;
        gvRemittance.Rebind();
    }
    protected void gvRemittance_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvRemittance.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
